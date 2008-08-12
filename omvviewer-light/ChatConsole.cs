// ChatConsole.cs created with MonoDevelop
// User: robin at 16:20 11/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
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
			
		LLUUID im_key=libsecondlife.LLUUID.Zero;
		LLUUID im_session_id=libsecondlife.LLUUID.Zero;
		
		~ChatConsole()
		{
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
			}
			else
			{
				im_key=im.FromAgentID;				
			}
		}

		public ChatConsole(LLUUID target)
		{
			dosetup();
			MainClass.client.Self.OnInstantMessage += new libsecondlife.AgentManager.InstantMessageCallback(onIM);
			im_key=target;
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
				if(im.FromAgentID!=this.im_key)
					return;
			}
			
			string buffer;
			TextIter iter;
	
			Gtk.Application.Invoke(delegate {						
			
				iter=textview_chat.Buffer.EndIter;
				buffer=im.FromAgentName+": ";
				textview_chat.Buffer.InsertWithTags(ref iter,buffer,bold);
				
				buffer=im.Message+"\n";
				textview_chat.Buffer.InsertWithTags(ref iter,buffer,avchat);
				
				textview_chat.ScrollMarkOnscreen(textview_chat.Buffer.InsertMark);
			});	
	
			}
			                                       
		void onChat(string message, ChatAudibleLevel audible, ChatType type, ChatSourceType sourcetype,string fromName, LLUUID id, LLUUID ownerid, LLVector3 position)
		{

			if(type==ChatType.StartTyping || type==ChatType.StopTyping || type==ChatType.Debug ||type==ChatType.OwnerSay)
				return;

			if(message=="")
				return; //WTF???? why do i get empty messages
			
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
					textview_chat.ScrollMarkOnscreen(textview_chat.Buffer.InsertMark);
					
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
					textview_chat.ScrollMarkOnscreen(textview_chat.Buffer.InsertMark);
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

				string buffer;
				TextIter iter;
	
				MainClass.client.Self.InstantMessageGroup(im_session_id,entry_chat.Text);

				iter=textview_chat.Buffer.EndIter;
				buffer=MainClass.client.Self.Name+": ";
				textview_chat.Buffer.InsertWithTags(ref iter,buffer,bold);
			
				buffer=entry_chat.Text+"\n";
				textview_chat.Buffer.InsertWithTags(ref iter,buffer,avchat);

				
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
	}
}
