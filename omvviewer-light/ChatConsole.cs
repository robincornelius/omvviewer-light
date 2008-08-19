// ChatConsole.cs created with MonoDevelop
// User: robin at 16:20 11/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;
using libsecondlife;
using Gtk;

namespace omvviewerlight
{
	public partial class ChatConsole : Gtk.Bin
	{
			Gdk.Color col_red = new Gdk.Color(255,0,0);
			Gdk.Color col_blue = new Gdk.Color(0,0,255);
			Gdk.Color col_green = new Gdk.Color(0,255,0);
		    Gtk.TextTag bold;
		    Gtk.TextTag avchat;
		    Gtk.TextTag objectchat;
		    Gtk.TextTag systemchat;
		    Gtk.TextTag ownerobjectchat;
			
		public Gtk.Label tabLabel;
		public LLUUID im_key=libsecondlife.LLUUID.Zero;
		public LLUUID im_session_id=libsecondlife.LLUUID.Zero;
		
		public void kicknames()
		{
			if(im_key!=LLUUID.Zero)
				MainClass.client.Avatars.RequestAvatarName(im_key);
			
			if(im_session_id!=LLUUID.Zero)
				MainClass.client.Groups.RequestGroupName(im_session_id);

		}
		
		~ChatConsole()
		{
			MainClass.win.getnotebook().SwitchPage -= new SwitchPageHandler(onSwitchPage);
			
			if(im_key!=libsecondlife.LLUUID.Zero)
				if(MainClass.win.active_ims.Contains(im_key))
					MainClass.win.active_ims.Remove(im_key);	
		
			if(im_session_id!=libsecondlife.LLUUID.Zero)
				if(MainClass.win.active_groups_ims.Contains(im_key))
					MainClass.win.active_groups_ims.Remove(im_key);	
		
		}
		
		public ChatConsole()
		{
			dosetup();
			MainClass.client.Self.OnChat += new libsecondlife.AgentManager.ChatCallback(onChat);			
		}

		
		public ChatConsole(InstantMessage im)
		{
			dosetup();
			MainClass.client.Self.OnInstantMessage += new libsecondlife.AgentManager.InstantMessageCallback(onIM);
			if(im.GroupIM)
			{
				this.im_session_id=im.IMSessionID;
				im_key=LLUUID.Zero;			
				MainClass.client.Self.OnGroupChatJoin += new libsecondlife.AgentManager.GroupChatJoined(onGroupChatJoin);
				MainClass.client.Self.RequestJoinGroupChat(im.IMSessionID);
				MainClass.client.Groups.OnGroupNames += new libsecondlife.GroupManager.GroupNamesCallback(onGroupNames);
				MainClass.client.Avatars.OnAvatarNames += new libsecondlife.AvatarManager.AvatarNamesCallback(onAvatarNames);
			
			}
			else
			{
				im_key=im.FromAgentID;				
				MainClass.client.Avatars.OnAvatarNames += new libsecondlife.AvatarManager.AvatarNamesCallback(onAvatarNames);
			}
			
			onIM(im,null); //yea, i forgot this, need to display text from first IM, lol
				
		}
		
		public void onSwitchPage(object o, SwitchPageArgs args)
		{
	 
		
			    int thispage=MainClass.win.getnotebook().PageNum(this);
			  if(thispage==args.PageNum)
			{
			    Gdk.Color col = new Gdk.Color(0,0,0);
				Gtk.StateType type = new Gtk.StateType();
				type|=Gtk.StateType.Active;			
				this.tabLabel.ModifyFg(type,col);				
			}
		}
		
				
		void onGroupChatJoin(LLUUID groupChatSessionID, LLUUID tmpSessionID, bool success)
		{
			
			if(groupChatSessionID!=this.im_session_id)
				return;
			
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
			Console.Write(activepage.ToString()+" : "+thispage.ToString()+"\n");
			int index=-1;
			if(thispage==-1)
			{
					if(activepage!=1)
				
				if(tabLabel!=null)	
							this.tabLabel.ModifyFg(type,col);					
				return;
						
			}
			else
			{
				
				this.tabLabel.ModifyFg(type,col);					
				return;
				}
				
				
			});
		}
		
		public ChatConsole(LLUUID target)
		{
			dosetup();
			MainClass.client.Self.OnInstantMessage += new libsecondlife.AgentManager.InstantMessageCallback(onIM);
			MainClass.client.Avatars.OnAvatarNames += new libsecondlife.AvatarManager.AvatarNamesCallback(onAvatarNames);		
			
			im_key=target;
		}

		public ChatConsole(LLUUID target,bool igroup)
		{
			dosetup();
			MainClass.client.Self.OnInstantMessage += new libsecondlife.AgentManager.InstantMessageCallback(onIM);
			im_key=LLUUID.Zero;			
			MainClass.client.Self.OnGroupChatJoin += new libsecondlife.AgentManager.GroupChatJoined(onGroupChatJoin);
			MainClass.client.Self.RequestJoinGroupChat(target);
			MainClass.client.Groups.OnGroupNames += new libsecondlife.GroupManager.GroupNamesCallback(onGroupNames);
			MainClass.client.Avatars.OnAvatarNames += new libsecondlife.AvatarManager.AvatarNamesCallback(onAvatarNames);

			im_session_id=target;
		}
		
		
		void dosetup()
		{
			this.Build();
			bold=new Gtk.TextTag("bold");
			avchat=new Gtk.TextTag("avchat");
			objectchat=new Gtk.TextTag("objectchat");
			systemchat=new Gtk.TextTag("systemchat");
			ownerobjectchat=new Gtk.TextTag("ownerobjectchat");
			
			bold.Weight=Pango.Weight.Bold;
		
			objectchat.ForegroundGdk=col_green;
			
			ownerobjectchat.ForegroundGdk=col_blue;
			
			systemchat.Weight=Pango.Weight.Ultrabold;
			systemchat.ForegroundGdk=col_red;
			
			textview_chat.Buffer.TagTable.Add(bold);
			textview_chat.Buffer.TagTable.Add(avchat);
			textview_chat.Buffer.TagTable.Add(systemchat);
			textview_chat.Buffer.TagTable.Add(objectchat);
			textview_chat.Buffer.TagTable.Add(ownerobjectchat);
			
			
		}

		public void clickclosed(object obj, EventArgs args)
		{
		    int pageno=1;
			Gtk.Notebook nb;
			nb =(Gtk.Notebook)this.Parent;
			pageno=nb.PageNum((Gtk.Widget)this);

		    if(im_key!=libsecondlife.LLUUID.Zero)
				if(MainClass.win.active_ims.Contains(im_key))
					MainClass.win.active_ims.Remove(im_key);	
			
			if(im_session_id!=libsecondlife.LLUUID.Zero)
				if(MainClass.win.active_ims.Contains(im_session_id))
					MainClass.win.active_ims.Remove(im_session_id);	
		
			nb.RemovePage(pageno);
		}
		
		
		void onIM(InstantMessage im, Simulator sim)
		{
			//Not group IM ignore messages not destine for im_key
			
			if(im.GroupIM==true)
			{
				if(im.IMSessionID!=this.im_session_id)
					return;
			}
			else
			{
				if(im.FromAgentID!=this.im_key && im.IMSessionID!=this.im_session_id)
					return;
			}

			// Is this a typing message
			
			if(im.Message=="typing")
			{
				return;
			}
            Gtk.Application.Invoke(delegate
            {		
			    redtab();
			
			    string buffer;
			    TextIter iter;
	
							
			
				iter=textview_chat.Buffer.EndIter;
				buffer=im.FromAgentName+": ";
				textview_chat.Buffer.InsertWithTags(ref iter,buffer,bold);
				
				buffer=im.Message+"\n";
				textview_chat.Buffer.InsertWithTags(ref iter,buffer,avchat);
                TextMark mark=textview_chat.Buffer.CreateMark("xyz", textview_chat.Buffer.EndIter, true);
                textview_chat.ScrollMarkOnscreen(mark);
				textview_chat.Buffer.DeleteMark("xyz");
			});	
	
			}
			                                       
		void onChat(string message, ChatAudibleLevel audible, ChatType type, ChatSourceType sourcetype,string fromName, LLUUID id, LLUUID ownerid, LLVector3 position)
		{

			if(type==ChatType.StartTyping || type==ChatType.StopTyping || type==ChatType.Debug ||type==ChatType.OwnerSay)
				return;

			if(message=="")
				return; //WTF???? why do i get empty messages

			Gtk.Application.Invoke(delegate {						
			
			if(!MainClass.win.Visible)
			{
				MainClass.win.trayIcon.Blinking=true;
				MainClass.win.UrgencyHint=true;
				Gdk.Color col = new Gdk.Color(255,0,0);
				Gtk.StateType xtype = new Gtk.StateType();			
				xtype|=Gtk.StateType.Active;	
				this.tabLabel.ModifyFg(xtype,col);									
				MainClass.win.UrgencyHint=true;
				MainClass.win.trayIcon.Blinking=true;

				}
	        });
			
			string buffer;
			TextIter iter;

			if(type==ChatType.Whisper)
				fromName=fromName+" whispers: ";
			if(type==ChatType.Shout)
				fromName=fromName+" shouts: ";
			if(type==ChatType.Normal)
				fromName=fromName+" ";

			if(sourcetype==ChatSourceType.Agent)
			{
				Gtk.Application.Invoke(delegate {						
				
					iter=textview_chat.Buffer.EndIter;
					buffer=fromName;
					textview_chat.Buffer.InsertWithTags(ref iter,buffer,bold);

					iter=textview_chat.Buffer.EndIter;
					buffer=message+"\n";
					textview_chat.Buffer.InsertWithTags(ref iter,buffer,avchat);
                    TextMark mark = textview_chat.Buffer.CreateMark("xyz", textview_chat.Buffer.EndIter, true);
                    textview_chat.ScrollMarkOnscreen(mark);
					textview_chat.Buffer.DeleteMark("xyz");
	
				});
				return;
			}

			if(type==ChatType.OwnerSay)
			{
				Gtk.Application.Invoke(delegate {						

					iter=textview_chat.Buffer.EndIter;
					buffer=fromName;
					textview_chat.Buffer.InsertWithTags(ref iter,buffer,bold,ownerobjectchat);

					iter=textview_chat.Buffer.EndIter;
					buffer=message+"\n";
					textview_chat.Buffer.InsertWithTags(ref iter,buffer,ownerobjectchat);
                    TextMark mark = textview_chat.Buffer.CreateMark("xyz", textview_chat.Buffer.EndIter, true);
					textview_chat.ScrollMarkOnscreen(mark);
					textview_chat.Buffer.DeleteMark("xyz");

				});
				return;		
			}
			
			if(sourcetype==ChatSourceType.Object)
			{
				Gtk.Application.Invoke(delegate {						
					iter=textview_chat.Buffer.EndIter;
					buffer=fromName;
					textview_chat.Buffer.InsertWithTags(ref iter,buffer,bold,objectchat);

					iter=textview_chat.Buffer.EndIter;
					buffer=message+"\n";
					textview_chat.Buffer.InsertWithTags(ref iter,buffer,objectchat);
					textview_chat.ScrollMarkOnscreen(textview_chat.Buffer.InsertMark);
					});
				return;
			}

			if(sourcetype==ChatSourceType.System)
			{
				Gtk.Application.Invoke(delegate {						

					iter=textview_chat.Buffer.EndIter;
					buffer=fromName;
					textview_chat.Buffer.InsertWithTags(ref iter,buffer,bold,systemchat);

					iter=textview_chat.Buffer.EndIter;
					buffer=message+"\n";
					textview_chat.Buffer.InsertWithTags(ref iter,buffer,systemchat);
					textview_chat.ScrollMarkOnscreen(textview_chat.Buffer.InsertMark);				
				});
				return;
			}
			
		}

		protected virtual void OnEntryChatActivated (object sender, System.EventArgs e)
		{
			
			if(im_key!=libsecondlife.LLUUID.Zero)
			{
				MainClass.client.Self.InstantMessage(im_key,entry_chat.Text);

				string buffer;
				TextIter iter;
			
				iter=textview_chat.Buffer.EndIter;
				buffer=MainClass.client.Self.Name+": ";
				textview_chat.Buffer.InsertWithTags(ref iter,buffer,bold);
			
				buffer=entry_chat.Text+"\n";
				textview_chat.Buffer.InsertWithTags(ref iter,buffer,avchat);
			
				textview_chat.ScrollMarkOnscreen(textview_chat.Buffer.InsertMark);
				this.entry_chat.Text="";
								
				return;
			}
			
			if(this.im_session_id!=libsecondlife.LLUUID.Zero)
			{				
				MainClass.client.Self.InstantMessageGroup(im_session_id,entry_chat.Text);
				this.entry_chat.Text="";
				return;
			}
			
			ChatType type=libsecondlife.ChatType.Normal;
			if(this.combobox_say_type.ActiveText=="Say")
				type=libsecondlife.ChatType.Normal;
			if(this.combobox_say_type.ActiveText=="Shout")
				type=libsecondlife.ChatType.Shout;
			if(this.combobox_say_type.ActiveText=="Whisper")
				type=libsecondlife.ChatType.Whisper;
			
			int channel=0;
			
			if(this.entry_chat.Text.StartsWith("/"))
			{
				//TODO fix this so not only a space works!
				int pos;
				pos=this.entry_chat.Text.IndexOf(" ");
				string substr=this.entry_chat.Text.Substring(1,pos);
				Console.Write("Saying on channle :"+substr+"\n");
				channel = int.Parse(substr);
			}
			
			MainClass.client.Self.Chat(this.entry_chat.Text+"\n",channel,type);
			
			this.entry_chat.Text="";
			
		}
		
		void onAvatarNames(Dictionary <LLUUID,string>names)
		{
			if(this.im_key==LLUUID.Zero)
				return; //I DONT CARE 
			
			foreach(KeyValuePair <LLUUID,string> kvp in names)
			{
				if(!MainClass.av_names.ContainsKey(kvp.Key))
				{
					MainClass.av_names.Add(kvp.Key,kvp.Value);
				}
			}	
			
			Gtk.Application.Invoke(delegate {						
			
				if(this.tabLabel.Text=="Waiting...")
				{
					string name;
					if(MainClass.av_names.TryGetValue(this.im_key,out name))
					{
						tabLabel.Text=name;
						tabLabel.QueueDraw();
					}
				}
			});
		}
		
		void onGroupNames(Dictionary <LLUUID,string>groups)
	    {
			
			if(this.im_key!=LLUUID.Zero)
				return;
			string group;
			Gtk.Application.Invoke(delegate {											
				if(MainClass.client.Groups.GroupName2KeyCache.TryGetValue(this.im_session_id,out group))
				{
					if(this.tabLabel.Text=="Waiting...")
					{
						tabLabel.Text=group;
						tabLabel.QueueDraw();
						
					}				
				}			
			});

			}
		
					
	}
}
