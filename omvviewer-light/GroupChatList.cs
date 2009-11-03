// GroupChatList.cs created with MonoDevelop
// User: robin at 15:21 12/02/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using OpenMetaverse;
using Gtk;
using System.Collections.Generic;

namespace omvviewerlight
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class GroupChatList : Gtk.Bin
	{
		UUID session;
		Gtk.ListStore store;

		public GroupChatList()
		{
			this.Build();
			store= new Gtk.ListStore (typeof(string),typeof(ChatSessionMember));
			MyTreeViewColumn tvc;
            Gtk.CellRendererText NameRenderer=new Gtk.CellRendererText();
			tvc=new MyTreeViewColumn("Name",NameRenderer,"text",0,true);			
			tvc.Sizing=Gtk.TreeViewColumnSizing.Autosize;
			tvc.setmodel(store);
			
            tvc.SetCellDataFunc(NameRenderer,new Gtk.TreeCellDataFunc(RenderName));
            
            this.treeview_members.AppendColumn(tvc);
			this.treeview_members.HeadersClickable=true;
			store.SetSortColumnId(0,SortType.Ascending);

            this.treeview_members.Selection.Mode = SelectionMode.Multiple;    
            this.treeview_members.ButtonPressEvent += new ButtonPressEventHandler(treeview_ButtonPressEvent);
			Dictionary <UUID,TreeIter> memberstree= new Dictionary<UUID,TreeIter>();
			
			treeview_members.Model=store;
		
        }


        private void RenderName(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
        {
            ChatSessionMember member=(ChatSessionMember)model.GetValue(iter,1);


            if(member.IsModerator==true)
            {
                (cell as Gtk.CellRendererText).Weight = 20000;
            }
            else
                (cell as Gtk.CellRendererText).Weight = 1;

            if(member.MuteText==true)
            {
                (cell as Gtk.CellRendererText).Strikethrough = true;
            }
            else
                (cell as Gtk.CellRendererText).Strikethrough = false;

            if(member.CanVoiceChat==false)
            {
                (cell as Gtk.CellRendererText).Foreground="darkgreen";
            }
            else
                (cell as Gtk.CellRendererText).Foreground = "black";
        }


        [GLib.ConnectBefore]
        void treeview_ButtonPressEvent(object o, ButtonPressEventArgs args)
        {
            if (args.Event.Button == 3)//Fuck this should be a define
            {

                Gtk.TreeModel mod;
                Gtk.TreeIter iter;
             
                TreePath[] paths = treeview_members.Selection.GetSelectedRows(out mod);


                ChatSessionMember me = MainClass.client.Self.GroupChatSessions[session].Find(delegate(ChatSessionMember member)
                {
                    return member.AvatarKey == MainClass.client.Self.AgentID;
                });


                bool i_am_a_moderator = false;


              
                    if (me.IsModerator == true)
                        i_am_a_moderator = true;

                Gtk.Menu menu = new Gtk.Menu();


                if(i_am_a_moderator==true)
                {
                    bool muted=false;
                    bool not_muted=false;
                    foreach (TreePath path in paths)
                    {
                       
                        store.GetIter(out iter, path);
                        ChatSessionMember member=(ChatSessionMember)store.GetValue(iter,1);
                        if(member.MuteText==true)
                            muted=true;

                        if(member.MuteText==false)
                            not_muted=true;
                    }

                    if(not_muted==true && muted==false)
                    {
                           //We can mute
                          Gtk.MenuItem menu_mute = new MenuItem("Mute");
                          menu_mute.ButtonPressEvent+=new ButtonPressEventHandler(menu_mute_ButtonPressEvent);
                          menu.Append(menu_mute);
                    }

                     if(not_muted==false && muted==true)
                    {
                           //We can mute
                          Gtk.MenuItem menu_unmute = new MenuItem("UN-mute");
                          menu_unmute.ButtonPressEvent+=new ButtonPressEventHandler(menu_unmute_ButtonPressEvent);
                          menu.Append(menu_unmute);
                    }
                }

                if (paths.Length == 1)
                {

                    Gtk.ImageMenuItem menu_IM = new ImageMenuItem("IM");
                    menu_IM.Image = new Gtk.Image(MainClass.GetResource("icon_group.png"));
                    menu_IM.ButtonPressEvent += new ButtonPressEventHandler(menu_IM_ButtonPressEvent);
                    menu.Append(menu_IM);
                }
                else if (paths.Length > 1)
                {
                    Gtk.ImageMenuItem menu_IM = new ImageMenuItem("Confrence");
                    menu_IM.Image = new Gtk.Image(MainClass.GetResource("icn_voice-groupfocus.png"));
                    menu_IM.ButtonPressEvent += new ButtonPressEventHandler(menu_IM_ButtonPressEvent);
                    menu.Append(menu_IM);
                }
                

                menu.Popup();
                menu.ShowAll();

            }
        }

        void menu_IM_ButtonPressEvent(object o, ButtonPressEventArgs args)
        {
            Gtk.TreeModel mod;
            Gtk.TreeIter iter;

            TreePath[] paths = treeview_members.Selection.GetSelectedRows(out mod);


            if (paths.Length == 1)
            {
                store.GetIter(out iter,paths[0]);
                UUID target = ((ChatSessionMember)store.GetValue(iter, 1)).AvatarKey;
                MainClass.win.startIM(target);
            }
            else if (paths.Length > 1)
            {
                List <UUID> targets=new List<UUID>();
                foreach(Gtk.TreePath path in paths)
                {
                      store.GetIter(out iter,path);
                      UUID target=((ChatSessionMember)store.GetValue(iter,1)).AvatarKey;
                      targets.Add(target);
                }

                MainClass.win.startConfrenceIM(targets);
            }


        }

        void menu_unmute_ButtonPressEvent(object o, ButtonPressEventArgs args)
        {
            Gtk.TreeModel mod;
            Gtk.TreeIter iter;

            TreePath[] paths = treeview_members.Selection.GetSelectedRows(out mod);
            foreach (TreePath path in paths)
            {
                store.GetIter(out iter, path);
                ChatSessionMember member=(ChatSessionMember)store.GetValue(iter,1);
                MainClass.client.Self.ModerateChatSessions(session,member.AvatarKey,"text",false);
            }

            
        }

        void menu_mute_ButtonPressEvent(object o, ButtonPressEventArgs args)
        {
            Gtk.TreeModel mod;
            Gtk.TreeIter iter;

            TreePath[] paths = treeview_members.Selection.GetSelectedRows(out mod);
            foreach (TreePath path in paths)
            {
              
                store.GetIter(out iter, path);
                ChatSessionMember member = (ChatSessionMember)store.GetValue(iter, 1);
                MainClass.client.Self.ModerateChatSessions(session, member.AvatarKey, "text", true);
            }
        }
		
		public void setsession(UUID id)
		{
			session=id;		
			
                Gtk.Application.Invoke(delegate{			
				this.store.Clear();
				if(MainClass.client.Self.GroupChatSessions.ContainsKey(session))			
				    MainClass.client.Self.GroupChatSessions[session].ForEach(delegate (ChatSessionMember member)
					{

                       

	                    string extra= member.IsModerator==true?" (moderator)":"";
						Gtk.TreeIter iter = store.AppendValues("Waiting...",member);
			            AsyncNameUpdate ud=new AsyncNameUpdate(member.AvatarKey,false);  
				        ud.addparameters(iter);
			           			
				        ud.onNameCallBack += delegate(string namex,object[] values){Gtk.TreeIter iterx=(Gtk.TreeIter)values[0]; lock(store){store.SetValue(iterx,0,namex);}};
			 	        ud.go();						
	  			    });	
				
	        MainClass.client.Self.ChatSessionMemberAdded += new EventHandler<ChatSessionMemberAddedEventArgs>(Self_ChatSessionMemberAdded);
            MainClass.client.Self.ChatSessionMemberLeft += new EventHandler<ChatSessionMemberLeftEventArgs>(Self_ChatSessionMemberLeft);
            });			

		}

        void Self_ChatSessionMemberAdded(object sender, ChatSessionMemberAddedEventArgs e)   
		{
			if(session!=e.SessionID)
			return;
			
            Gtk.Application.Invoke(delegate
            {
			    
                ChatSessionMember member=MainClass.client.Self.GroupChatSessions[session].Find
                (
                    delegate (ChatSessionMember member2)
                    {
                        return member2.AvatarKey==e.AgentID;
                    }
                );


                Gtk.TreeIter iter = store.AppendValues("Waiting...", member);
		        AsyncNameUpdate ud=new AsyncNameUpdate(e.AgentID,false);  
			    ud.addparameters(iter);

			    ud.onNameCallBack += delegate(string namex,object[] values)
                {
                    Gtk.TreeIter iterx=(Gtk.TreeIter)values[0];

                    Gtk.Application.Invoke(delegate {
                        lock(store)
                        {
                            if(store.IterIsValid(iterx))
                                store.SetValue(iterx,0,namex);
                        };
                    });
                };

			    ud.go(); 
               });
         }

        void Self_ChatSessionMemberLeft(object sender, ChatSessionMemberLeftEventArgs e)
        {
			if(session!=e.SessionID)
			return;
            Gtk.Application.Invoke(delegate{
			    lock(store)
                {
			        store.Foreach(delegate(Gtk.TreeModel mod, Gtk.TreePath path, Gtk.TreeIter iter)
                    {
				        UUID id=((ChatSessionMember)store.GetValue(iter,1)).AvatarKey;
 				        if(id==e.AgentID)
                        {

                            Gtk.Application.Invoke(delegate
                            {
                                store.Remove(ref iter);
                            });
                            return true; 
                        }
                       
			            return false;
			        });	
			    }	
            });		
		}
	}
}
