/*
omvviewerlight a Text based client to metaverses such as Linden Labs Secondlife(tm)
    Copyright (C) 2008,2009  Robin Cornelius <robin.cornelius@gmail.com>

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 2 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

// ChatConsole.cs created with MonoDevelop
// User: robin at 16:20 11/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;
using OpenMetaverse;
using Gdk;
using Gtk;
using GLib;


namespace omvviewerlight
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class ChatConsole : Gtk.Bin
	{
		Gtk.TextTag bold;
		Gtk.TextTag avchat;
        Gtk.TextTag selfavchat;
		Gtk.TextTag objectchat;
        Gtk.TextTag objectIMchat;
        public Gtk.TextTag systemchat;
		Gtk.TextTag ownerobjectchat;
        Gtk.TextTag onoffline;
        Gtk.TextTag highlightchat;

        Gtk.TextTag typing_tag;
		TextMark preTyping;
		TextMark postTyping;
		
		bool istyping=false;
		bool istypingsent=false;
		
		public Gtk.Label tabLabel;
		bool lookatrunning=false;
		UUID lookat;
		byte [] bucket;
		
		private UUID im_target=UUID.Zero;
				
		bool joined_group_chat=false;
		
		enum chat_type
		{
			CHAT_TYPE_NONE,
			CHAT_TYPE_CHAT,
			CHAT_TYPE_IM,
			CHAT_TYPE_GROUP_IM,
			CHAT_TYPE_CONFRENCE
		};
		
		chat_type current_chat_type=chat_type.CHAT_TYPE_NONE;

		
		public ChatConsole()
		{
			dosetup();
            current_chat_type = chat_type.CHAT_TYPE_CHAT;

            MainClass.onRegister += new MainClass.register(MainClass_onRegister);
            MainClass.onDeregister += new MainClass.deregister(MainClass_onDeregister);
            //This class is always present so we don't need to add at constructor time	
		}

        void MainClass_onRegister()
        {
            textview_chat.Buffer.Clear();

            MainClass.client.Self.ChatFromSimulator += new EventHandler<ChatEventArgs>(Self_ChatFromSimulator);
            MainClass.client.Self.IM += new EventHandler<InstantMessageEventArgs>(Self_IM);
            MainClass.client.Friends.FriendOffline += new EventHandler<FriendInfoEventArgs>(Friends_FriendOffline);
            MainClass.client.Friends.FriendOnline += new EventHandler<FriendInfoEventArgs>(Friends_FriendOnline);
            MainClass.client.Self.MoneyBalanceReply += new EventHandler<MoneyBalanceReplyEventArgs>(Self_MoneyBalanceReply);
            MainClass.client.Self.GroupChatJoined += new EventHandler<GroupChatJoinedEventArgs>(Self_GroupChatJoined);

        }

        
        void MainClass_onDeregister()
        {
            if(MainClass.client!=null)
            {
                MainClass.client.Self.ChatFromSimulator -= new EventHandler<ChatEventArgs>(Self_ChatFromSimulator);
                MainClass.client.Self.IM -= new EventHandler<InstantMessageEventArgs>(Self_IM);
                MainClass.client.Friends.FriendOffline -= new EventHandler<FriendInfoEventArgs>(Friends_FriendOffline);
                MainClass.client.Friends.FriendOnline -= new EventHandler<FriendInfoEventArgs>(Friends_FriendOnline);
                MainClass.client.Self.MoneyBalanceReply -= new EventHandler<MoneyBalanceReplyEventArgs>(Self_MoneyBalanceReply);

                MainClass.client.Self.GroupChatJoined -= new EventHandler<GroupChatJoinedEventArgs>(Self_GroupChatJoined);
   
            }

        }

        void Self_MoneyBalanceReply(object sender, MoneyBalanceReplyEventArgs e)
        {
            if (current_chat_type == chat_type.CHAT_TYPE_CHAT)
            {
                if (e.Description != "")
                {
                    Gtk.Application.Invoke(delegate
                    {
                        displaychat(e.Description, "Payment :", this.systemchat, this.systemchat);
                    });
                }
            }
        }

        new public void Dispose()
        {
            Console.WriteLine("Disposing of the chatconsole control");
            MainClass.onRegister -= new MainClass.register(MainClass_onRegister);
            MainClass.onDeregister -= new MainClass.deregister(MainClass_onDeregister);
            MainClass_onDeregister();
        }
		
        void Friends_FriendOnline(object sender, FriendInfoEventArgs e)
        {
            if(current_chat_type==chat_type.CHAT_TYPE_CHAT)
            {
                //SIGH this is necessary to prevent a bunch of " is online" messages at login
                 AsyncNameUpdate ud = new AsyncNameUpdate(e.Friend.UUID, false);
                 ud.onNameCallBack += delegate(string namex, object[] values) { displaychat("is online", namex, onoffline, onoffline); };
                 ud.go();             
            }
            else if (current_chat_type == chat_type.CHAT_TYPE_IM && im_target == e.Friend.UUID)
            {
                Gtk.Application.Invoke(delegate
                {
                    displaychat("is online", e.Friend.Name, onoffline, onoffline);
                });
            }
        }

        void Friends_FriendOffline(object sender, FriendInfoEventArgs e)
        {
            if (current_chat_type == chat_type.CHAT_TYPE_CHAT)
            {
                //this is the main chat winddow, notify for all friends here
                Gtk.Application.Invoke(delegate
                {
                    displaychat("is offline", e.Friend.Name, onoffline, onoffline);
                });
            }
            else if (current_chat_type == chat_type.CHAT_TYPE_IM && im_target == e.Friend.UUID)
            {
                Gtk.Application.Invoke(delegate
                {
                    displaychat("is offline", e.Friend.Name, onoffline, onoffline);
                });
            }
        }

		void show_group_list(UUID target)
		{
			GroupChatList groupchatlist=new GroupChatList();
			hbox2.PackEnd(groupchatlist);
			groupchatlist.WidthRequest=150;
			hbox2.ShowAll();
			groupchatlist.WidthRequest=150;
			groupchatlist.setsession(target);
		}
		

		public ChatConsole(InstantMessage im)
		{
            lock (MainClass.win.im_queue)
            {
                dosetup();

                if (im.Dialog==InstantMessageDialog.MessageFromAgent)
                {
                    if (im.BinaryBucket.Length <= 1)
                    {
                        //Plain IM
                        Logger.Log("Starting a direct IM " + im.IMSessionID.ToString(), Helpers.LogLevel.Info);
                        current_chat_type = chat_type.CHAT_TYPE_IM;
                        im_target = im.FromAgentID;
                        foreach (InstantMessage qim in MainClass.win.im_queue)
                        {
                            if (qim.FromAgentID == im_target)
                            {
                                InstantMessageEventArgs e = new InstantMessageEventArgs(qim, null);
                                this.Self_IM(this, e);
                            }
                        }

                        MainClass.win.im_queue.RemoveAll(TestRemove);

                        MainClass.win.im_windows.Add(im.FromAgentID, this);
                        if (MainClass.win.im_registering.Contains(im.FromAgentID))
                            MainClass.win.im_registering.Remove(im.FromAgentID);

                    }
                    else if(MainClass.client.Groups.GroupName2KeyCache.ContainsKey(im.IMSessionID))
                    {
                        //Group IM
                        Logger.Log("Starting a new group chat for session id " + im.IMSessionID.ToString(), Helpers.LogLevel.Info);
                        current_chat_type = chat_type.CHAT_TYPE_GROUP_IM;
                        this.im_target = im.IMSessionID;
                        this.textview_chat.Buffer.Insert(textview_chat.Buffer.EndIter, "Trying to join group chat session, please wait........\n");
                        Gtk.Timeout.Add(10000, kick_group_join);
                        MainClass.client.Self.RequestJoinGroupChat(im.IMSessionID);
                        MainClass.win.im_windows.Add(im.IMSessionID, this);

                        InstantMessageEventArgs e = new InstantMessageEventArgs(im, null);
                        Self_IM(this, e);
                       

                    }
                    else
                    {
                        //Confrence IM
                        Logger.Log("Starting a new confrence chat for session id " + im.IMSessionID.ToString(), Helpers.LogLevel.Info);
                        current_chat_type = chat_type.CHAT_TYPE_CONFRENCE;
                        this.im_target = im.IMSessionID;
                        show_group_list(im.IMSessionID);
                        MainClass.win.im_windows.Add(im.IMSessionID, this);
                        MainClass.client.Self.ChatterBoxAcceptInvite(im.IMSessionID);
                        bucket = im.BinaryBucket;
                        InstantMessageEventArgs e = new InstantMessageEventArgs(im, null);
                        Self_IM(this, e);
                    }

                }
                else if( im.Dialog==InstantMessageDialog.MessageFromObject)
                {
                    //Object IM
                    if (current_chat_type == chat_type.CHAT_TYPE_CHAT)
                    {
                        this.displaychat(im.Message, im.FromAgentName, objectchat, objectchat);
                    }

                    return;
                }
            }
		}

        bool TestRemove(InstantMessage x)
        {
            if(x.FromAgentID==im_target)
                return true;

            return false;
        }
		
		public void onSwitchPage(object o, SwitchPageArgs args)
		{
			//If we switch to *this* page then remove a possible red tab lable
			int thispage=MainClass.win.getnotebook().PageNum(this);

            if (thispage == -1)
            {
                if(this.Parent!=null)
                    thispage = MainClass.win.getnotebook().PageNum(this.Parent);
            }

            if (thispage == -1)
                thispage = 1; //PARENT CHAT WINDOW STUFF

      	    if(thispage==args.PageNum)
			{
          
			    Gdk.Color col = new Gdk.Color(0,0,0);
				Gtk.StateType type = new Gtk.StateType();
				type|=Gtk.StateType.Active;
			    if(this.tabLabel!=null)
				    this.tabLabel.ModifyFg(type,col);				
			}

            MainClass.win.UrgencyHint = false;
            MainClass.win.trayIcon.Blinking = false;
		}

        void Self_GroupChatJoined(object sender, GroupChatJoinedEventArgs e)
        {

            if (e.Success == false)
            {
                this.textview_chat.Buffer.Insert(textview_chat.Buffer.EndIter, "Failed to join group chat "+e.SessionName+".. retrying...\n");
                return;
            }

			Console.WriteLine("On groupchat join for "+e.SessionID.ToString());

            if (!MainClass.win.im_windows.ContainsKey(e.SessionID))
                MainClass.win.im_windows.Add(e.SessionID,this);

			if(e.SessionID!=im_target && im_target!=e.TmpSessionID)
				return;
			
			if(e.TmpSessionID==im_target)
			{
				im_target=e.SessionID;
                this.bucket = OpenMetaverse.Utils.StringToBytes(e.SessionName);
			}
			
			this.joined_group_chat=true;
			
			string buffer="Joined group chat\n";
			TextIter iter;
	
			Gtk.Application.Invoke(delegate {
                show_group_list(im_target);		
				iter=textview_chat.Buffer.EndIter;
				textview_chat.Buffer.InsertWithTags(ref iter,buffer,bold);						
				textview_chat.ScrollMarkOnscreen(textview_chat.Buffer.InsertMark);
			});
		}	
		
		public void redtab()
		{
			Gtk.Application.Invoke(delegate {	
				Gdk.Color col = new Gdk.Color(255,0,0);
				Gtk.StateType type = new Gtk.StateType();			
				type|=Gtk.StateType.Active;	
					
				int activepage=MainClass.win.getnotebook().CurrentPage;
				int thispage=MainClass.win.getnotebook().PageNum(this);
			
                if (thispage == -1)
                {
                    if(this.Parent!=null)
                        thispage = MainClass.win.getnotebook().PageNum(this.Parent);
                }

                if (thispage == -1 && this.tabLabel != null)
                {
                    //I'm guessing that we are the chat window and the fucking parent operators are still not working
                     if(activepage!=1)
                     {
                         this.tabLabel.ModifyFg(type, col);
                     }
                }
                  
				if(thispage!=-1)
				{
                    if(activepage!=thispage)
                        if(this.tabLabel!=null)
					        this.tabLabel.ModifyFg(type,col);					
					return;
				}	
			});
		}
		
		public ChatConsole(UUID target)
		{
			dosetup();
            current_chat_type = chat_type.CHAT_TYPE_IM;
//			MainClass.client.Self.OnInstantMessage += new OpenMetaverse.AgentManager.InstantMessageCallback(onIM);
            im_target = target;
            if(!MainClass.win.im_windows.ContainsKey(target))
                MainClass.win.im_windows.Add(target, this);
		}

		public ChatConsole(UUID target,bool igroup)
		{			
            
			dosetup();
            current_chat_type = chat_type.CHAT_TYPE_GROUP_IM;
			im_target=target;

//	        MainClass.client.Self.OnGroupChatJoin += new AgentManager.GroupChatJoinedCallback(onGroupChatJoin);
//			MainClass.client.Self.OnInstantMessage += new OpenMetaverse.AgentManager.InstantMessageCallback(onIM);
			this.textview_chat.Buffer.Insert(textview_chat.Buffer.EndIter,"Trying to join group chat session, please wait........\n");
			joined_group_chat=false;
			Gtk.Timeout.Add(10000,kick_group_join);
			MainClass.client.Self.RequestJoinGroupChat(target);
           
		}
		
		public ChatConsole(List <UUID> targets)
		{
			  dosetup();
			  this.textview_chat.Buffer.Insert(textview_chat.Buffer.EndIter,"Trying to join confrence chat session, please wait........\n");
//			  MainClass.client.Self.OnGroupChatJoin += new AgentManager.GroupChatJoinedCallback(onGroupChatJoin);
//            MainClass.client.Self.OnInstantMessage += new OpenMetaverse.AgentManager.InstantMessageCallback(onIM);
			  current_chat_type = chat_type.CHAT_TYPE_CONFRENCE;
              this.im_target = UUID.Random();
              MainClass.client.Self.StartIMConference(targets, this.im_target);
		}
		
		bool kick_group_join()
		{
            Console.WriteLine("Kick_group_join");
			if(joined_group_chat==true)
				return false;

			this.textview_chat.Buffer.Insert(textview_chat.Buffer.EndIter,"Retrying to join group chat session, please wait........\n");
			MainClass.client.Self.RequestJoinGroupChat(im_target);
			
			return true;
		}

		void settagtable()
		{	
            avchat.ForegroundGdk = MainClass.appsettings.convertfromsetting(MainClass.appsettings.color_chat);

            bold.Weight=Pango.Weight.Bold;
            bold.FontDesc = Pango.FontDescription.FromString("Arial Bold");

            selfavchat.Weight = Pango.Weight.Bold;
            selfavchat.ForegroundGdk = MainClass.appsettings.convertfromsetting(MainClass.appsettings.color_chat_system);
         
            objectchat.ForegroundGdk = MainClass.appsettings.convertfromsetting(MainClass.appsettings.color_chat_object);
            ownerobjectchat.ForegroundGdk = MainClass.appsettings.convertfromsetting(MainClass.appsettings.color_chat_object_owner);  
			
			systemchat.Weight=Pango.Weight.Ultrabold;
            systemchat.ForegroundGdk = MainClass.appsettings.convertfromsetting(MainClass.appsettings.color_chat_system);

            typing_tag.ForegroundGdk =
            MainClass.appsettings.convertfromsetting(MainClass.appsettings.color_chat_typing);

            onoffline.Weight = Pango.Weight.Bold;
            onoffline.ForegroundGdk = MainClass.appsettings.convertfromsetting(MainClass.appsettings.color_chat_online);

            highlightchat.ForegroundGdk = MainClass.appsettings.convertfromsetting(MainClass.appsettings.color_chat_highlight);   
        }
		
		void onSettingsUpdate()
		{
		    settagtable();				
		}

		void dosetup()
		{
			Build();
			
		    bold=new Gtk.TextTag("bold");
			avchat=new Gtk.TextTag("avchat");
            selfavchat = new Gtk.TextTag("selfavchat");
			objectchat=new Gtk.TextTag("objectchat");
			systemchat=new Gtk.TextTag("systemchat");
			ownerobjectchat=new Gtk.TextTag("ownerobjectchat");
            objectIMchat = new Gtk.TextTag("objectIMchat");
            typing_tag = new Gtk.TextTag("typing");
            onoffline = new Gtk.TextTag("onoffline");
            highlightchat = new Gtk.TextTag("hightlight");

			MainClass.appsettings.onSettingsUpdate+=new MySettings.SettingsUpdate(onSettingsUpdate);
			
			textview_chat.Buffer.TagTable.Add(bold);
			textview_chat.Buffer.TagTable.Add(avchat);
			textview_chat.Buffer.TagTable.Add(systemchat);
			textview_chat.Buffer.TagTable.Add(objectchat);
			textview_chat.Buffer.TagTable.Add(ownerobjectchat);
            textview_chat.Buffer.TagTable.Add(typing_tag);
            textview_chat.Buffer.TagTable.Add(onoffline);
            textview_chat.Buffer.TagTable.Add(highlightchat);	    

		    settagtable();		
		}


		public void clickclosed(object obj, EventArgs args)
		{
		    int pageno=1;
			Gtk.Notebook nb;
			nb =(Gtk.Notebook)this.Parent;
			pageno=nb.PageNum((Gtk.Widget)this);

			if(MainClass.win.im_windows.ContainsKey(im_target))
 			    MainClass.win.im_windows.Remove(im_target);
			
 		    if(MainClass.win.active_ims.Contains(im_target))
               MainClass.win.active_ims.Remove(im_target);

            if (current_chat_type == chat_type.CHAT_TYPE_GROUP_IM)
            {
                MainClass.client.Self.RequestLeaveGroupChat(im_target);	
            }
	
			nb.RemovePage(pageno);
			
			MainClass_onDeregister();            
            MainClass.win.getnotebook().SwitchPage -=  new SwitchPageHandler(onSwitchPage);
			this.Destroy();	
		}
		
        void Self_IM(object sender, InstantMessageEventArgs e)
		{
			
            if (this.current_chat_type==chat_type.CHAT_TYPE_CHAT)
            {
                //we are the chat console not an IM window;
                //We handle Some types of IM packet here

                // we also do the console logging here

                if (e.IM.Dialog != InstantMessageDialog.StartTyping && e.IM.Dialog != InstantMessageDialog.StopTyping)
                    Console.WriteLine("New IM recieved " + e.IM.ToString() + " " + OpenMetaverse.Utils.BytesToString(e.IM.BinaryBucket));


               if (e.IM.Dialog == OpenMetaverse.InstantMessageDialog.InventoryOffered)
                {
                    Gtk.Application.Invoke(delegate
                    {
                        displaychat(e.IM.FromAgentName + " gave you " + e.IM.Message, "(new inventory)", this.systemchat, this.systemchat);
                    });
                    return;
                }
                if (e.IM.Dialog == OpenMetaverse.InstantMessageDialog.TaskInventoryOffered)
                {
                    Gtk.Application.Invoke(delegate
                    {
                        displaychat(e.IM.FromAgentName + " gave you " + e.IM.Message, "(new inventory)", this.systemchat, this.systemchat);
                    });
                    
                    return;
                }

                if (e.IM.Dialog == InstantMessageDialog.MessageFromObject)
                {
                    Gtk.Application.Invoke(delegate
                    {
                        displaychat(e.IM.Message, e.IM.FromAgentName, objectIMchat, objectIMchat);
                    });
                }

                return;
            }	

            //Not group IM ignore messages not destine for im_target
            if (current_chat_type == chat_type.CHAT_TYPE_GROUP_IM || current_chat_type == chat_type.CHAT_TYPE_CONFRENCE) 
			{
                if (e.IM.IMSessionID != im_target)
                    return;
			}
			
            if(current_chat_type==chat_type.CHAT_TYPE_IM)
			{
                if (e.IM.FromAgentID != im_target || e.IM.Dialog != InstantMessageDialog.MessageFromAgent || e.IM.BinaryBucket.Length>1)
					return;
			}

			// Is this a typing message
			
			if(e.IM.Dialog == InstantMessageDialog.StartTyping)
			{
				if(istyping==false)
				{
	                 Gtk.Application.Invoke(delegate
	                 {
	                     displaychat("is typing...", e.IM.FromAgentName, typing_tag, typing_tag);
	                 });
				}
				return;
			}

			if(e.IM.Dialog == InstantMessageDialog.StopTyping)
			{
				if(istyping==false)
				{
	                 Gtk.Application.Invoke(delegate
	                 {
						if(this.istyping==true)
						{
							TextIter start=this.textview_chat.Buffer.GetIterAtMark(this.preTyping);
							TextIter end=this.textview_chat.Buffer.GetIterAtMark(this.postTyping);
							textview_chat.Buffer.SelectRange(start,end);
							textview_chat.Buffer.DeleteSelection(false,false);			
							istyping=false;
						}	                
					});
				}
				return;
			}
				
			//Reject some IMs that we handle else where
			// Pretty much eveything not directly associated with this IM session
            // All invites,requests,accepts etc are handled in MainWindow

			   if(e.IM.Dialog!=OpenMetaverse.InstantMessageDialog.MessageFromAgent &&
			   e.IM.Dialog!=OpenMetaverse.InstantMessageDialog.SessionSend &&
			   e.IM.Dialog!=OpenMetaverse.InstantMessageDialog.SessionGroupStart &&
               e.IM.Dialog!=InstantMessageDialog.BusyAutoResponse
				)
				{
                    Console.Write("IM REJECTED IN IM WINDOW FROM " + e.IM.FromAgentID + " : " + e.IM.FromAgentName + " : " + e.IM.IMSessionID + "\n");
					return;	
                }
    						
            redtab();
			
			if(MainClass.appsettings.notify_IM && this.current_chat_type==chat_type.CHAT_TYPE_IM)
				windownotify();

            if (MainClass.appsettings.notify_group_IM && this.current_chat_type == chat_type.CHAT_TYPE_GROUP_IM)
				windownotify();

            Gtk.Application.Invoke(delegate
            {
                displaychat(e.IM.Message, e.IM.FromAgentName, avchat, bold); 
			});	
	
			}

        void Self_ChatFromSimulator(object sender, ChatEventArgs e)
        {

			if(e.Type==ChatType.StartTyping || e.Type==ChatType.StopTyping || e.Type==ChatType.Debug)
				return;

			if(e.Message=="")
				return; //WTF???? why do i get empty messages which are not the above types

            redtab();
			
			if((MainClass.appsettings.notify_chat && e.SourceType==ChatSourceType.Agent) || e.SourceType==ChatSourceType.System)
				windownotify();

			if(MainClass.appsettings.notify_object_chat && e.SourceType==ChatSourceType.Object)
				windownotify();

//FIXME				
//			if(e.Type==ChatType.Whisper)
//				e.FromName=e.FromName+" whispers";
//			if(e.Type==ChatType.Shout)
//				e.FromName=e.FromName+" shouts";
	
			if(e.SourceType==ChatSourceType.Agent)
			{
				Gtk.Application.Invoke(delegate {						
                    displaychat(e.Message, e.FromName, avchat, bold);
					if(e.SourceID!=MainClass.client.Self.AgentID)
					{
						if(lookatrunning==false)
						{
							this.lookat=UUID.Random();
							//MainClass.client.Self.LookAtEffect(MainClass.client.Self.AgentID,id,Vector3d.Zero,LookAtType.Mouselook,lookat);
							//GLib.Timeout.Add(3000,ClearLookAt);
							lookatrunning=true;
						}
					}
				});
				return;
			}

			if(e.Type==ChatType.OwnerSay)
			{
				Gtk.Application.Invoke(delegate {
                    displaychat(e.Message, e.FromName, ownerobjectchat, ownerobjectchat);
				});
				return;		
			}
			
			if(e.SourceType==ChatSourceType.Object)
			{
				Gtk.Application.Invoke(delegate {
                    displaychat(e.Message, e.FromName, objectchat, objectchat);
					});
				return;
			}

			if(e.SourceType==ChatSourceType.System)
			{
				Gtk.Application.Invoke(delegate {
                    //FIXME
                    //e.FromName = "Secondlife ";
                    displaychat(e.Message, e.FromName, systemchat, systemchat);		
				});
				return;
			}
			
		}
		
		bool ClearLookAt()
		{
            Console.WriteLine("Clear lookat");
			MainClass.client.Self.LookAtEffect(MainClass.client.Self.AgentID,UUID.Zero,Vector3d.Zero,LookAtType.Clear,UUID.Zero);
			lookatrunning=false;
			return false;
			
		}

		protected virtual void OnEntryChatActivated (object sender, System.EventArgs e)
		{

            if (this.entry_chat.Text == "")
                return;

            if (current_chat_type == chat_type.CHAT_TYPE_IM)
            {
                    MainClass.client.Self.InstantMessage(im_target, entry_chat.Text);
                    this.displaychat(entry_chat.Text, MainClass.client.Self.Name, avchat, bold);
                    this.entry_chat.Text = "";
                    istypingsent = false;
                    return;
            }

            if (current_chat_type == chat_type.CHAT_TYPE_GROUP_IM || current_chat_type == chat_type.CHAT_TYPE_CONFRENCE)
			{
                Logger.Log("replying via session id " + im_target.ToString(), Helpers.LogLevel.Info);
				MainClass.client.Self.InstantMessageGroup(im_target,entry_chat.Text);
				this.entry_chat.Text="";
				istypingsent=false;
				return;
  		    }
				
			ChatType type=OpenMetaverse.ChatType.Normal;
			if(this.combobox_say_type.ActiveText=="Say")
				type=OpenMetaverse.ChatType.Normal;
			if(this.combobox_say_type.ActiveText=="Shout")
				type=OpenMetaverse.ChatType.Shout;
			if(this.combobox_say_type.ActiveText=="Whisper")
				type=OpenMetaverse.ChatType.Whisper;
			
			int channel=0;
            string outtext = this.entry_chat.Text;
			
			if(this.entry_chat.Text.StartsWith("/"))
			{
                char[] nums = new char[] {'0','1','2','3','4','5','6','7','8','9'};
                string newtext=entry_chat.Text.Substring(1);
                newtext=newtext.TrimStart(nums);
                int diff;
                diff = entry_chat.Text.Length - newtext.Length;


                if (diff > 1)
                {
                    outtext = newtext;
                    string substr = this.entry_chat.Text.Substring(1, diff-1);
 
                    try
                    {
                        channel = int.Parse(substr);
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.ToString());
                        channel = 0;
                    }
                }
            }

            MainClass.client.Self.Chat(outtext, channel, type);
			MainClass.client.Self.AnimationStop(Animations.TYPE,true);
			
			this.entry_chat.Text="";
			
			istypingsent=false;
		}

      
        void windownotify()
        {
            Gtk.Application.Invoke(delegate
            {
                if (!MainClass.win.Visible || MainClass.win.IsActive==false)
                {
                    MainClass.win.trayIcon.Blinking = true;
                    MainClass.win.UrgencyHint = true;
                    MainClass.win.trayIcon.Blinking = true;

                }
				if(!(MainClass.win.Focus is Gtk.Widget))
				{
					MainClass.win.UrgencyHint = true;
				}
				});

        }

        public void displaychat(string message, string name, TextTag message_tag, TextTag name_tag)
        {
            string buffer;
            TextIter iter;
			bool removedtyping=false;
            bool emote = false;
			
			if(message=="is typing...")
			{
				this.textview_chat.Buffer.DeleteMark("Typing start");
				this.textview_chat.Buffer.DeleteMark("Typing end");
				this.preTyping=textview_chat.Buffer.CreateMark("Typing start",textview_chat.Buffer.EndIter,true);
				istyping=true;
			}
			else
			{
				if(istyping==true)
				{
					TextIter start=this.textview_chat.Buffer.GetIterAtMark(this.preTyping);
					TextIter end=this.textview_chat.Buffer.GetIterAtMark(this.postTyping);
					textview_chat.Buffer.SelectRange(start,end);
					textview_chat.Buffer.DeleteSelection(false,false);
					istyping=false;
				}
			}
				
            if (message.Length > 3)
                if (message.Substring(0, 3) == "/me")
                    emote=true;
			
			if(MainClass.appsettings.timestamps)
			{
				iter = textview_chat.Buffer.EndIter;				
				DateTime CurrTime = DateTime.Now;
                string time = string.Format("[{0:D2}:{1:D2}] ", CurrTime.Hour, CurrTime.Minute);
				textview_chat.Buffer.Insert(ref iter,time);
            }
            if (emote == false)
            {
				iter = textview_chat.Buffer.EndIter;
                buffer = name+" ";
                textview_chat.Buffer.InsertWithTags(ref iter, buffer, name_tag);
                iter = textview_chat.Buffer.EndIter;
                buffer = message + "\n";

                if (current_chat_type != chat_type.CHAT_TYPE_IM)
                {
                       //If highlight==true
                       if(buffer.ToLower().Contains(MainClass.client.Self.FirstName.ToLower())) 
                       {
                           Console.WriteLine("Highlighting messsage");
                           message_tag = this.highlightchat;
                       }
                }

                textview_chat.Buffer.InsertWithTags(ref iter, buffer, message_tag);
                TextMark mark = textview_chat.Buffer.CreateMark("xyz", textview_chat.Buffer.EndIter, true);
                textview_chat.ScrollMarkOnscreen(mark);
                textview_chat.Buffer.DeleteMark("xyz");
            }
            else
            {
                if (message.Length < 3)
                    return;

                message = message.Substring(3, message.Length-3);
                iter = textview_chat.Buffer.EndIter;
                buffer = name+message + "\n";
                textview_chat.Buffer.InsertWithTags(ref iter, buffer, message_tag);
                TextMark mark = textview_chat.Buffer.CreateMark("xyz", textview_chat.Buffer.EndIter, true);
                textview_chat.ScrollMarkOnscreen(mark);
                textview_chat.Buffer.DeleteMark("xyz");

            }
			
			if(message=="is typing...")
			{
				this.postTyping=textview_chat.Buffer.CreateMark("Typing end",textview_chat.Buffer.EndIter,true);				
			}
			
			if(removedtyping==true)
			{
				this.istyping=false; //set this false or recuse to hell
				displaychat("is typing...", name, this.typing_tag, this.typing_tag);				
			}

        }

        protected virtual void OnEntryChatChanged (object sender, System.EventArgs e)
		{
			if(im_target!=OpenMetaverse.UUID.Zero)
			{
				if(istypingsent==false)
				{	
				  //  Console.Write("\nSending typing message\n");
                  byte[] binaryBucket;
                  binaryBucket = new byte[0];
			      MainClass.client.Self.InstantMessage(MainClass.client.Self.Name,im_target,"typing",im_target,InstantMessageDialog.StartTyping,InstantMessageOnline.Online,Vector3.Zero, UUID.Zero,binaryBucket);
				  MainClass.client.Self.AnimationStart(Animations.TYPE,true);
                  istypingsent=true;
				  GLib.Timeout.Add(10000,StopTyping);
				 }
            }			
	    }
		
	    bool StopTyping()
	    {
            Console.WriteLine("Stop typing");
            byte[] binaryBucket;
            binaryBucket = new byte[0];
		    MainClass.client.Self.InstantMessage(MainClass.client.Self.Name,im_target,"",im_target,InstantMessageDialog.StopTyping,InstantMessageOnline.Online,Vector3.Zero, UUID.Zero,binaryBucket);
		    MainClass.client.Self.AnimationStop(Animations.TYPE,true);
     			
            istypingsent=false;
		    return false;	 
      }	
	}
}
