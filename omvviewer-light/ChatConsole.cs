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

        Gtk.TextTag typing_tag;
		TextMark preTyping;
		TextMark postTyping;
		
		bool istyping=false;
		bool istypingsent=false;
		
		public Gtk.Label tabLabel;
	//	public UUID im_target=OpenMetaverse.UUID.Zero;
	//	public UUID im_target=OpenMetaverse.UUID.Zero;
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

			MainClass.client.Network.OnLogin += new OpenMetaverse.NetworkManager.LoginCallback(onLogin);		
			MainClass.client.Self.OnChat += new OpenMetaverse.AgentManager.ChatCallback(onChat);
            MainClass.client.Self.OnInstantMessage += new OpenMetaverse.AgentManager.InstantMessageCallback(onIM);
            MainClass.client.Friends.OnFriendOffline += new FriendsManager.FriendOfflineEvent(Friends_OnFriendOffline);
            MainClass.client.Friends.OnFriendOnline += new FriendsManager.FriendOnlineEvent(Friends_OnFriendOnline);
		}
		
        void Friends_OnFriendOnline(FriendInfo friend)
        {
            if(current_chat_type==chat_type.CHAT_TYPE_CHAT)
            {
				AsyncNameUpdate ud=new AsyncNameUpdate(friend.UUID,false);
                ud.onNameCallBack += delegate(string namex, object[] values) { displaychat("is online", namex, onoffline, onoffline); };
                ud.go();
				   
            }
            else if (current_chat_type == chat_type.CHAT_TYPE_IM && im_target == friend.UUID)
            {
                Gtk.Application.Invoke(delegate
                {
                    displaychat("is online", friend.Name, onoffline, onoffline);
                });
            }
        }

        void Friends_OnFriendOffline(FriendInfo friend)
        {
            if (current_chat_type == chat_type.CHAT_TYPE_CHAT)
            {
                //this is the main chat winddow, notify for all friends here
                Gtk.Application.Invoke(delegate
                {
                    displaychat("is offline", friend.Name, onoffline, onoffline);
                });
            }
            else if (current_chat_type == chat_type.CHAT_TYPE_IM && im_target == friend.UUID)
            {
                Gtk.Application.Invoke(delegate
                {
                    displaychat("is offline", friend.Name, onoffline, onoffline);
                });
            }
        }

      		
        void onLogin(LoginStatus status, string message)
        {
            if (LoginStatus.Success == status)
            {
                Gtk.Application.Invoke(delegate
                {
                    //The login reply comes in way too late and we loose some chat
                    //this.textview_chat.Buffer.Clear();
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

                if (im.GroupIM && im.BinaryBucket.Length <1)
                {
                    current_chat_type =chat_type.CHAT_TYPE_GROUP_IM;
                    this.im_target = im.IMSessionID;
                    show_group_list(im_target);
                    MainClass.client.Self.OnGroupChatJoin += new AgentManager.GroupChatJoinedCallback(onGroupChatJoin);
                    this.textview_chat.Buffer.Insert(textview_chat.Buffer.EndIter, "Trying to join group chat session, please wait........\n");
                    Gtk.Timeout.Add(10000, kick_group_join);
                    MainClass.client.Self.RequestJoinGroupChat(im.IMSessionID);
                    onIM(im, null);
                }

                if (im.BinaryBucket.Length > 1)
                {
                    current_chat_type = chat_type.CHAT_TYPE_CONFRENCE;
                    this.im_target = im.IMSessionID;
					show_group_list(this.im_target);
					MainClass.client.Self.ChatterBoxAcceptInvite(im.IMSessionID);
					bucket=im.BinaryBucket;
                    onIM(im, null);
                }

                if (!im.GroupIM)
                {
                    current_chat_type = chat_type.CHAT_TYPE_IM;
                    im_target = im.FromAgentID;
                    foreach (InstantMessage qim in MainClass.win.im_queue)
                    {
                        if (qim.FromAgentID == im_target)
                            onIM(qim, null);
                    }

                    MainClass.win.im_queue.RemoveAll(TestRemove);

                    MainClass.win.im_windows.Add(im.FromAgentID, this);
                    if (MainClass.win.im_registering.Contains(im.FromAgentID))
                        MainClass.win.im_registering.Remove(im.FromAgentID);
                }

                MainClass.client.Self.OnInstantMessage += new OpenMetaverse.AgentManager.InstantMessageCallback(onIM);
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

        void onGroupChatJoin(UUID groupChatSessionID, string sessionName, UUID tmpSessionID, bool success)
		{
			if(groupChatSessionID!=im_target)
				return;

            MainClass.client.Self.OnGroupChatJoin -= new AgentManager.GroupChatJoinedCallback(onGroupChatJoin);

			this.joined_group_chat=true;
			
			string buffer="Joined group chat\n";
			TextIter iter;
	
			Gtk.Application.Invoke(delegate {						
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
			MainClass.client.Self.OnInstantMessage += new OpenMetaverse.AgentManager.InstantMessageCallback(onIM);
            im_target = target;
		}

		public ChatConsole(UUID target,bool igroup)
		{			
            
			dosetup();
            current_chat_type = chat_type.CHAT_TYPE_GROUP_IM;
			im_target=target;
            this.show_group_list(im_target);
	        MainClass.client.Self.OnGroupChatJoin += new AgentManager.GroupChatJoinedCallback(onGroupChatJoin);
			MainClass.client.Self.OnInstantMessage += new OpenMetaverse.AgentManager.InstantMessageCallback(onIM);
			this.textview_chat.Buffer.Insert(textview_chat.Buffer.EndIter,"Trying to join group chat session, please wait........\n");
			joined_group_chat=false;
			Gtk.Timeout.Add(10000,kick_group_join);
			MainClass.client.Self.RequestJoinGroupChat(target);			
		}

	
		bool kick_group_join()
		{
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
			
			MainClass.appsettings.onSettingsUpdate+=new MySettings.SettingsUpdate(onSettingsUpdate);
			
			textview_chat.Buffer.TagTable.Add(bold);
			textview_chat.Buffer.TagTable.Add(avchat);
			textview_chat.Buffer.TagTable.Add(systemchat);
			textview_chat.Buffer.TagTable.Add(objectchat);
			textview_chat.Buffer.TagTable.Add(ownerobjectchat);
            textview_chat.Buffer.TagTable.Add(typing_tag);
            textview_chat.Buffer.TagTable.Add(onoffline);
			
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

            if (current_chat_type == chat_type.CHAT_TYPE_GROUP_IM)
            {
                MainClass.client.Self.RequestLeaveGroupChat(im_target);	
            }
	
			nb.RemovePage(pageno);
			
	        MainClass.client.Network.OnLogin -= new OpenMetaverse.NetworkManager.LoginCallback(onLogin);		
			MainClass.client.Self.OnChat -= new OpenMetaverse.AgentManager.ChatCallback(onChat);
            MainClass.client.Self.OnInstantMessage -= new OpenMetaverse.AgentManager.InstantMessageCallback(onIM);
            MainClass.client.Friends.OnFriendOffline -= new FriendsManager.FriendOfflineEvent(Friends_OnFriendOffline);
            MainClass.client.Friends.OnFriendOnline -= new FriendsManager.FriendOnlineEvent(Friends_OnFriendOnline);
			MainClass.win.getnotebook().SwitchPage -=  new SwitchPageHandler(onSwitchPage);
			
			this.Destroy();	
			//Finalize();
			//System.GC.SuppressFinalize(this);
		}
		
		
		void onIM(InstantMessage im, Simulator sim)
		{
			           
			Console.WriteLine("New IM recieved "+im.ToString());

			Console.WriteLine("Buckert is "+im.BinaryBucket.Length.ToString() + " DATA :"+MainWindow.BytesToString(im.BinaryBucket));
			

            if (this.current_chat_type==chat_type.CHAT_TYPE_CHAT)
            {
                //we are the chat console not an IM window;
                //We handle Some types of IM packet here
              
               if (im.Dialog == OpenMetaverse.InstantMessageDialog.InventoryOffered)
                {
                    displaychat(im.FromAgentName+" gave you "+im.Message, "(new inventory)", this.systemchat,this.systemchat);
                    return;
                }
                if (im.Dialog == OpenMetaverse.InstantMessageDialog.TaskInventoryOffered)
                {
                    displaychat(im.FromAgentName + " gave you " + im.Message, "(new inventory)", this.systemchat, this.systemchat);
                    return;
                }

                if (im.IMSessionID == UUID.Zero)
                {
                    Gtk.Application.Invoke(delegate
                    {
                        displaychat(im.Message, im.FromAgentName, objectIMchat, objectIMchat);
                    });
                }

                return;
            }

            //Not group IM ignore messages not destine for im_target
            if (current_chat_type == chat_type.CHAT_TYPE_GROUP_IM) 
			{
				if(im.IMSessionID!=this.im_target)
					return;
			}
			
			if( current_chat_type == chat_type.CHAT_TYPE_CONFRENCE)
			{
				string incomming=MainWindow.BytesToString(im.BinaryBucket);
                string test=MainWindow.BytesToString(this.bucket);
					if(incomming!=test)
                       return;	
            }			

            if(current_chat_type==chat_type.CHAT_TYPE_IM)
			{
				if(im.FromAgentID!=im_target && im.IMSessionID!=im_target)
					return;
			}

			// Is this a typing message
			
			if(im.Dialog == InstantMessageDialog.StartTyping)
			{
				if(istyping==false)
				{
	                 Gtk.Application.Invoke(delegate
	                 {
	                     displaychat("is typing...", im.FromAgentName, typing_tag, typing_tag);
	                 });
				}
				return;
			}

			if(im.Dialog == InstantMessageDialog.StopTyping)
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
			
			   if(im.Dialog!=OpenMetaverse.InstantMessageDialog.MessageFromAgent &&
			   im.Dialog!=OpenMetaverse.InstantMessageDialog.SessionSend &&
			   im.Dialog!=OpenMetaverse.InstantMessageDialog.SessionGroupStart
				)
				{
                    Console.Write("IM REJECTED IN IM WINDOW FROM " + im.FromAgentID + " : " + im.FromAgentName + " : " + im.IMSessionID + "\n");
					return;	
                }
    						
            Console.Write("IM FROM " + im.FromAgentID + " : " + im.FromAgentName + " : " + im.IMSessionID + "\n");

            redtab();
			
			if(MainClass.appsettings.notify_IM && im_target==UUID.Zero)
				windownotify();

			if(MainClass.appsettings.notify_group_IM && im_target!=UUID.Zero)
				windownotify();

			
            // Is this from an object?
            //null session ID

           
           
            Gtk.Application.Invoke(delegate
            {
                displaychat(im.Message, im.FromAgentName, avchat, bold); 
			});	
	
			}
			                                       
		void onChat(string message, ChatAudibleLevel audible, ChatType type, ChatSourceType sourcetype,string fromName, UUID id, UUID ownerid, Vector3 position)
		{

			if(type==ChatType.StartTyping || type==ChatType.StopTyping || type==ChatType.Debug)
				return;

			if(message=="")
				return; //WTF???? why do i get empty messages which are not the above types


            redtab();
			
			if((MainClass.appsettings.notify_chat && sourcetype==ChatSourceType.Agent) || sourcetype==ChatSourceType.System)
				windownotify();
			if(MainClass.appsettings.notify_object_chat && sourcetype==ChatSourceType.Object)
				windownotify();
							
			if(type==ChatType.Whisper)
				fromName=fromName+" whispers";
			if(type==ChatType.Shout)
				fromName=fromName+" shouts";
	
			if(sourcetype==ChatSourceType.Agent)
			{
				Gtk.Application.Invoke(delegate {						
                    displaychat(message, fromName, avchat, bold);
					if(id!=MainClass.client.Self.AgentID)
					{
						if(lookatrunning==false)
						{
							this.lookat=UUID.Random();
							MainClass.client.Self.LookAtEffect(MainClass.client.Self.AgentID,id,Vector3d.Zero,LookAtType.Mouselook,lookat);
							GLib.Timeout.Add(3000,ClearLookAt);
							lookatrunning=true;
						}
					}
				});
				return;
			}

			if(type==ChatType.OwnerSay)
			{
				Gtk.Application.Invoke(delegate {
                    displaychat(message, fromName, ownerobjectchat, ownerobjectchat);
				});
				return;		
			}
			
			if(sourcetype==ChatSourceType.Object)
			{
				Gtk.Application.Invoke(delegate {
                    displaychat(message, fromName, objectchat, objectchat);
					});
				return;
			}

			if(sourcetype==ChatSourceType.System)
			{
				Gtk.Application.Invoke(delegate {
                    fromName = "Secondlife ";
                    displaychat(message, fromName, systemchat, systemchat);		
				});
				return;
			}
			
		}
		
		bool ClearLookAt()
		{
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

            if (current_chat_type == chat_type.CHAT_TYPE_GROUP_IM)
			{				
				MainClass.client.Self.InstantMessageGroup(im_target,entry_chat.Text);
				this.entry_chat.Text="";
				istypingsent=false;
				return;
  		    }
			
           if (current_chat_type == chat_type.CHAT_TYPE_CONFRENCE)
 		   {		
				if( MainClass.client.Self.GroupChatSessions.Dictionary.ContainsKey(this.im_target))
                {
					this.displaychat(entry_chat.Text, MainClass.client.Self.Name, avchat, bold);
                    foreach(OpenMetaverse.ChatSessionMember member in MainClass.client.Self.GroupChatSessions.Dictionary[this.im_target])
				    {
					    MainClass.client.Self.InstantMessage(MainClass.client.Self.Name,member.AvatarKey,entry_chat.Text,this.im_target,InstantMessageDialog.MessageFromAgent,InstantMessageOnline.Online,new Vector3(),UUID.Zero,this.bucket);
					}	
					this.entry_chat.Text="";
					istypingsent=false;
                }
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
			
					  //  Console.Write("\nSending typing message\n");
                        byte[] binaryBucket;
                        binaryBucket = new byte[0];
			MainClass.client.Self.InstantMessage(MainClass.client.Self.Name,im_target,"",im_target,InstantMessageDialog.StopTyping,InstantMessageOnline.Online,Vector3.Zero, UUID.Zero,binaryBucket);
					    MainClass.client.Self.AnimationStop(Animations.TYPE,true);
 			
istypingsent=false;
		     return false;	
			
		 
      }
							
	}
}
