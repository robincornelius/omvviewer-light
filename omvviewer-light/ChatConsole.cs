/*
omvviewer-light a Text based client to metaverses such as Linden Labs Secondlife(tm)
    Copyright (C) 2008  Robin Cornelius <robin.cornelius@gmail.com>

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
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
            Gtk.TextTag objectIMchat;
            Gtk.TextTag systemchat;
		    Gtk.TextTag ownerobjectchat;

			
		public Gtk.Label tabLabel;
		public LLUUID im_key=libsecondlife.LLUUID.Zero;
		public LLUUID im_session_id=libsecondlife.LLUUID.Zero;
		
		~ChatConsole()
		{
           
					Console.Write("*!*!*!*!*!*!*!*! ALL CLEAN ON CHAT !*!*!*!*!*!*!*\n|");
		}
		
		public ChatConsole()
		{
			dosetup();
            this.im_session_id = LLUUID.Zero;
            this.im_key = LLUUID.Zero;
			MainClass.client.Self.OnChat += new libsecondlife.AgentManager.ChatCallback(onChat);
            MainClass.client.Self.OnInstantMessage += new libsecondlife.AgentManager.InstantMessageCallback(onIM);
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
			}
			else
			{
				im_key=im.FromAgentID;				
			}

			// Pass the message on to the chat system as the event will not have been triggered as its
            // only just registered.
			onIM(im,null);
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
                thispage = 1; //FUCKING PARENT CHAT WINDOW STUFF

            Console.Write("On switch page " + thispage.ToString() + ":" + args.PageNum.ToString());
			if(thispage==args.PageNum)
			{
                Console.Write("Setting color to black\n");

			    Gdk.Color col = new Gdk.Color(0,0,0);
				Gtk.StateType type = new Gtk.StateType();
				type|=Gtk.StateType.Active;
			    if(this.tabLabel!=null)
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

                Console.Write("Red tab test " + activepage.ToString() + ":" + thispage.ToString() + "\n");

                if (thispage == -1)
                {
                    if(this.Parent!=null)
                        thispage = MainClass.win.getnotebook().PageNum(this.Parent);

                    Console.Write("Tried to get parent we are now on :"+thispage.ToString()+"\n");
                }

                if (thispage == -1 && this.tabLabel != null)
                {
                    //I'm guessing that we are the chat window and the fucking parent operators are still not working
                     if(activepage!=1)
                     {
                        Console.Write("Assuming chat so going to red that one\n");
                         this.tabLabel.ModifyFg(type, col);
                     }
                }
                  
				if(thispage!=-1)
				{
                    Console.Write("Got an index\n");

                    if(activepage!=thispage)
                        if(this.tabLabel!=null)
					        this.tabLabel.ModifyFg(type,col);					
					return;
				}	
			});
		}
		
		public ChatConsole(LLUUID target)
		{
			dosetup();
			MainClass.client.Self.OnInstantMessage += new libsecondlife.AgentManager.InstantMessageCallback(onIM);			
			im_key=target;
		}

		public ChatConsole(LLUUID target,bool igroup)
		{
			dosetup();
			MainClass.client.Self.OnInstantMessage += new libsecondlife.AgentManager.InstantMessageCallback(onIM);
			im_key=LLUUID.Zero;			
			MainClass.client.Self.RequestJoinGroupChat(target);
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
            objectIMchat = new Gtk.TextTag("objectIMchat");

			
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
			Console.Write("**** CHAT CONSOLE SETUP ****\n");

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
					MainClass.client.Self.RequestLeaveGroupChat(im_session_id);		
			
			Console.Write("ref count is "+this.RefCount.ToString()+"\n");
			nb.RemovePage(pageno);
			Console.Write("ref count is "+this.RefCount.ToString()+"\n");
			
			MainClass.client.Self.OnChat -= new libsecondlife.AgentManager.ChatCallback(onChat);
            MainClass.client.Self.OnInstantMessage -= new libsecondlife.AgentManager.InstantMessageCallback(onIM);
			MainClass.client.Self.OnGroupChatJoin -= new libsecondlife.AgentManager.GroupChatJoined(onGroupChatJoin);
			
			Console.Write("Trying to destroy chat window\n");
			
			 MainClass.win.getnotebook().SwitchPage -=  new SwitchPageHandler(onSwitchPage);
			
				Console.Write("ref count is "+this.RefCount.ToString()+"\n");
			this.Destroy();
		
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

			if(im.Dialog!=null)
				Console.Write("**** DIALOGUE RESPONSE ****\n"+im.Dialog.ToString()+"\n");
			
			//Reject some IMs that we handle else where
			
			if(im.Dialog==libsecondlife.InstantMessageDialog.InventoryOffered)
				return;
			
			if(im.Dialog==libsecondlife.InstantMessageDialog.TaskInventoryOffered)
				return;
			
			if(im.Dialog==libsecondlife.InstantMessageDialog.InventoryAccepted)
				return;
				
			if(im.Dialog==libsecondlife.InstantMessageDialog.InventoryAccepted)
				return;
				
            Console.Write("IM FROM " + im.FromAgentID + " : " + im.FromAgentName + " : " + im.IMSessionID + "\n");

            redtab();
            windownotify();

            // Is this from an object?
            //null session ID

            if (im.IMSessionID == LLUUID.Zero)
            {
                //Its an object message, display in chat not IM
                if ((this.im_key == LLUUID.Zero) && (this.im_session_id ==LLUUID.Zero))
                {
                    // We are the chat console not an IM tab
                    Gtk.Application.Invoke(delegate
                    {
                        Gtk.TextIter iter;
                        string buffer;

                        iter = textview_chat.Buffer.EndIter;
                        buffer = im.FromAgentName;
                        textview_chat.Buffer.InsertWithTags(ref iter, buffer, bold, objectIMchat);

                        iter = textview_chat.Buffer.EndIter;
                        buffer = " "+im.Message + "\n";
                        textview_chat.Buffer.InsertWithTags(ref iter, buffer, objectIMchat);
                        textview_chat.ScrollMarkOnscreen(textview_chat.Buffer.InsertMark);
                    
                    });
                    
                  

                    return;


                }


            }
           

            Gtk.Application.Invoke(delegate
            {
               
			
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
				return; //WTF???? why do i get empty messages which are not the above types


            redtab();
            windownotify();

			
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
                    displaychat(message, fromName, avchat, bold);	
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
                    displaychat(message, fromName, systemchat, systemchat);		
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
                        channel = 0;
                    }
                }
            }

            MainClass.client.Self.Chat(outtext, channel, type);
			
			this.entry_chat.Text="";
			
		}

      
        void windownotify()
        {
            Gtk.Application.Invoke(delegate
            {
                if (!MainClass.win.Visible)
                {
                    MainClass.win.trayIcon.Blinking = true;
                    MainClass.win.UrgencyHint = true;
                    MainClass.win.trayIcon.Blinking = true;

                }
            });

        }

        void displaychat(string message, string name, TextTag message_tag, TextTag name_tag)
        {
            string buffer;
            TextIter iter;
            
            bool emote = false;

            if (message.Length > 3)
                if (message.Substring(0, 3) == "/me")
                    emote=true;

            if (emote == false)
            {
                iter = textview_chat.Buffer.EndIter;
                buffer = name;
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

        }					
	}
}
