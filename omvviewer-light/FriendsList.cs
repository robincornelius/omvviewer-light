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

// FriendsList.cs created with MonoDevelop
// User: robin at 10:56 11/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;
using OpenMetaverse;
using Gtk;

namespace omvviewerlight
{			
	public partial class FriendsList : Gtk.Bin
	{
		Gtk.ListStore store;
		Gdk.Pixbuf online_img;
		Gdk.Pixbuf offline_img;
		Gdk.Pixbuf img_edit_theirs;
		Gdk.Pixbuf img_edit_mine;
		Gdk.Pixbuf img_see_my_status;
		Gdk.Pixbuf img_map_me;
		Gdk.Pixbuf img_blank;

		public FriendsList()
		{			
			Console.Write("Building friends list window\n");
			this.Build();
			store= new Gtk.ListStore (typeof(Gdk.Pixbuf),typeof(string),typeof(Gdk.Pixbuf),typeof(Gdk.Pixbuf),typeof(Gdk.Pixbuf),typeof(Gdk.Pixbuf),typeof(string),typeof(bool));
            
			MyTreeViewColumn mycol;
				
			mycol = new MyTreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0);
            mycol.setmodel(store);
			mycol.Expand=false;
			mycol.FixedWidth=24;
			treeview_friends.AppendColumn(mycol);
			
			mycol = new MyTreeViewColumn("Name", new CellRendererText(), "text", 1);
			mycol.Expand=true;
			mycol.setmodel(store);
            treeview_friends.AppendColumn(mycol);			

			mycol = new MyTreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 2);
            mycol.setmodel(store);
			mycol.Expand=false;
			mycol.Spacing=0;
			mycol.FixedWidth=24;
			mycol.Sizing=Gtk.TreeViewColumnSizing.Fixed;
			treeview_friends.AppendColumn(mycol);
			
			mycol = new MyTreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 3);
            mycol.setmodel(store);
			mycol.Expand=false;
			mycol.Spacing=0;
			mycol.Sizing=Gtk.TreeViewColumnSizing.Fixed;
			mycol.FixedWidth=24;
			treeview_friends.AppendColumn(mycol);
			
			mycol = new MyTreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 4);
            mycol.setmodel(store);
			mycol.Expand=false;
			mycol.Spacing=0;
			mycol.FixedWidth=24;
			mycol.Sizing=Gtk.TreeViewColumnSizing.Fixed;
			treeview_friends.AppendColumn(mycol);
			
			mycol = new MyTreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 5);
            mycol.setmodel(store);
			mycol.Expand=false;
			mycol.Spacing=0;
			mycol.FixedWidth=24;
			mycol.Sizing=Gtk.TreeViewColumnSizing.Fixed;
			treeview_friends.AppendColumn(mycol);
		
			treeview_friends.Model=store;
            store.SetSortColumnId(1, SortType.Ascending);
            store.SetSortFunc(1,sortfunc);
			treeview_friends.HeadersClickable=true;
			
			
			online_img=MainClass.GetResource("icon_avatar_online.tga");
			offline_img=MainClass.GetResource("icon_avatar_offline.tga");
		    this.img_blank=MainClass.GetResource("blank_arrow.tga");
			this.img_edit_mine=MainClass.GetResource("ff_edit_mine.tga");
			this.img_edit_theirs=MainClass.GetResource("ff_edit_theirs.tga");
			this.img_map_me=MainClass.GetResource("ff_visible_map.tga");
			this.img_see_my_status=MainClass.GetResource("ff_visible_online.tga");
			
			MainClass.client.Network.OnLogin += new OpenMetaverse.NetworkManager.LoginCallback(onLogin);		
			MainClass.client.Friends.OnFriendOnline += new OpenMetaverse.FriendsManager.FriendOnlineEvent(onFriendOnline);
			MainClass.client.Friends.OnFriendOffline += new OpenMetaverse.FriendsManager.FriendOfflineEvent(onFriendOffline);
            MainClass.client.Friends.OnFriendshipResponse += new FriendsManager.FriendshipResponseEvent(Friends_OnFriendshipResponse);
            MainClass.client.Friends.OnFriendshipTerminated += new FriendsManager.FriendshipTerminatedEvent(Friends_OnFriendshipTerminated);
            MainClass.client.Friends.OnFriendRights += new FriendsManager.FriendRightsEvent(Friends_OnFriendRights);
        }

        void Friends_OnFriendRights(FriendInfo friend)
        {
            Gtk.Application.Invoke(delegate
            {
                lock (store)
                {
                    store.Foreach(myfunc);
                }
            });

        }

        int sortfunc(Gtk.TreeModel model, Gtk.TreeIter a, Gtk.TreeIter b)
        {
            string nameA = (string)store.GetValue(a, 1);
            string nameB = (string)store.GetValue(b, 1);

            bool Pa = (bool)store.GetValue(a, 7);
            bool Pb =(bool)store.GetValue(b, 7);

            if (Pa == Pb)
            {
               return string.Compare(nameA, nameB);
            }

            if (Pa == true)
               return -1;

            return 1;
        }

        void Friends_OnFriendshipTerminated(UUID agentID, string agentName)
        {
            Gtk.Application.Invoke(delegate
            {
				lock(store)
				{
                    // Needs testing!
                    populate_list();
				}
            });
        }

        void Friends_OnFriendshipResponse(UUID agentID, string agentName, bool accepted)
        {
            Gtk.Application.Invoke(delegate
            {
				lock(store)
				{
                    if (accepted == true)
                    {
                        Gtk.TreeIter iter = store.AppendValues(online_img, agentName, agentID.ToString(), true);
                        AsyncNameUpdate ud = new AsyncNameUpdate(agentID, false);
                        ud.addparameters(iter);
                        ud.onNameCallBack += delegate(string namex, object[] values) { Gtk.TreeIter iterx = (Gtk.TreeIter)values[0]; store.SetValue(iterx, 1, namex); };
                        ud.go();
                    }
				}
			});
        }	
		
		void onLogin(LoginStatus status,string message)
		{
						
			if(LoginStatus.Success==status)
			{
				Gtk.Application.Invoke(delegate {
					lock(store)
					{
						store.Clear();
						populate_list();
						store.Foreach(myfunc);
					}				
				});
			}
		}
		
		void onFriendOnline(FriendInfo friend)
		{
			Gtk.Application.Invoke(delegate {
				lock(store)
				{
					store.Foreach(myfunc);
				}
			});
		
		}
		
		void onFriendOffline(FriendInfo friend)
		{
			Gtk.Application.Invoke(delegate {			
				lock(store)
				{
					store.Foreach(myfunc);
				}
			});	
		}
		
		void populate_list()		
		{
            store.Clear();
			MainClass.client.Friends.FriendList.ForEach(delegate(FriendInfo friend)
			{
                Gtk.TreeIter iter = store.AppendValues(friend.IsOnline ? online_img : offline_img, 
				                                       friend.Name,
				                                       friend.CanSeeMeOnline ? this.img_see_my_status :img_blank,
				                                       friend.CanSeeMeOnMap ? this.img_map_me : img_blank,
				                                       friend.CanModifyMyObjects ? this.img_edit_mine : img_blank,
				                                       friend.CanModifyTheirObjects ? this.img_edit_theirs : img_blank,
				                                       friend.UUID.ToString(), 
				                                       friend.IsOnline);
				AsyncNameUpdate ud=new AsyncNameUpdate(friend.UUID,false);  
				ud.addparameters(iter);
				ud.onNameCallBack += delegate(string namex,object[] values){Gtk.TreeIter iterx=(Gtk.TreeIter)values[0]; store.SetValue(iterx,1,namex);};
                ud.go();
            });
		}
			
		
		bool myfunc(Gtk.TreeModel mod, Gtk.TreePath path, Gtk.TreeIter iter)
		{			
			string id =(string)store.GetValue(iter,6);
			UUID lid=(UUID)id;
		
			FriendInfo finfo;
			if(MainClass.client.Friends.FriendList.TryGetValue(lid,out finfo))
			{
                store.SetValue(iter, 0, finfo.IsOnline ? MainClass.GetResource("icon_avatar_online.tga") : MainClass.GetResource("icon_avatar_offline.tga"));
				store.SetValue(iter, 2, finfo.CanSeeMeOnline ? this.img_see_my_status : img_blank);
		        store.SetValue(iter, 3, finfo.CanSeeMeOnMap ? this.img_map_me : img_blank);
		        store.SetValue(iter, 4, finfo.CanModifyMyObjects ? this.img_edit_mine : img_blank);
		        store.SetValue(iter, 5, finfo.CanModifyTheirObjects ? this.img_edit_theirs : img_blank);			
				store.SetValue(iter, 7, finfo.IsOnline);
            }
			return false;
		}
		
		protected virtual void OnCheckbuttonOnlinestatusClicked (object sender, System.EventArgs e)
		{
			Gtk.TreeModel mod;
			Gtk.TreeIter iter;
			
			if(treeview_friends.Selection.GetSelected(out mod,out iter))			
			{
				string id=(string)mod.GetValue(iter,6);
				UUID lid=(UUID)id;
				FriendInfo finfo;
				if(MainClass.client.Friends.FriendList.TryGetValue(lid,out finfo))
				{					Gtk.CheckMenuItem se=(Gtk.CheckMenuItem)sender;                    					FriendRights rights=getrights(finfo);
                    se.Active = !se.Active;
					if(se.Active)
						rights|=FriendRights.CanSeeOnline;
					else
						rights&=~FriendRights.CanSeeOnline;
				
					MainClass.client.Friends.GrantRights(lid,rights);
				}
			}
          
		}

		protected virtual void OnCheckbuttonMapClicked (object sender, System.EventArgs e)
		{
			Gtk.TreeModel mod;
			Gtk.TreeIter iter;
			
			if(treeview_friends.Selection.GetSelected(out mod,out iter))			
			{
				string id=(string)mod.GetValue(iter,6);
				UUID lid=(UUID)id;
				FriendInfo finfo;
				if(MainClass.client.Friends.FriendList.TryGetValue(lid,out finfo))
				{
					FriendRights rights=getrights(finfo);
					Gtk.CheckMenuItem se=(Gtk.CheckMenuItem)sender;
                    se.Active = !se.Active;
					if(se.Active)
						rights|=FriendRights.CanSeeOnMap;
					else
						rights&=~FriendRights.CanSeeOnMap;

					MainClass.client.Friends.GrantRights(lid,rights);
						
				}
			}
          
		}

		protected virtual void OnCheckbuttonModobjectsClicked (object sender, System.EventArgs e)
		{
			Gtk.TreeModel mod;
			Gtk.TreeIter iter;
			
			if(treeview_friends.Selection.GetSelected(out mod,out iter))			
			{
				string id=(string)mod.GetValue(iter,6);
				UUID lid=(UUID)id;
				FriendInfo finfo;
				if(MainClass.client.Friends.FriendList.TryGetValue(lid,out finfo))
				{
					FriendRights rights=getrights(finfo);
					Gtk.CheckMenuItem se=(Gtk.CheckMenuItem)sender;
                    se.Active = !se.Active;

                    if (se.Active)
                        rights |= FriendRights.CanModifyObjects;
                    else
                        rights &= ~FriendRights.CanModifyObjects;
	
					MainClass.client.Friends.GrantRights(lid,rights);
	
				}
			}
         
		}

		FriendRights getrights(FriendInfo finfo)
		{
			FriendRights rights=new FriendRights();
			rights=0;
			
			if(finfo.CanModifyMyObjects)
				rights|=FriendRights.CanModifyObjects;
			
			if(finfo.CanSeeMeOnMap)
				rights|=FriendRights.CanSeeOnMap;
			
			if(finfo.CanSeeMeOnline)
				rights|=FriendRights.CanSeeOnline;

			return rights;
		}
		
		protected virtual void OnButtonIMClicked (object sender, System.EventArgs e)
		{
			//beter work out who we have selected
			Gtk.TreeModel mod;
			Gtk.TreeIter iter;
				
			if(treeview_friends.Selection.GetSelected(out mod,out iter))			
			{
				string id=(string)mod.GetValue(iter,6);
				UUID lid=(UUID)id;
				MainClass.win.startIM(lid);
			}
		}

		protected virtual void OnButtonTeleportClicked (object sender, System.EventArgs e)
		{
			Gtk.TreeModel mod;
			Gtk.TreeIter iter;
			
			if(treeview_friends.Selection.GetSelected(out mod,out iter))			
			{
				string id=(string)mod.GetValue(iter,6);
				UUID lid=(UUID)id;
				MainClass.client.Self.SendTeleportLure(lid);
			}

		}

		protected virtual void OnButtonPayClicked (object sender, System.EventArgs e)
		{
			Gtk.TreeModel mod;
			Gtk.TreeIter iter;
			
			if(treeview_friends.Selection.GetSelected(out mod,out iter))			
			{
				string id=(string)mod.GetValue(iter,6);
				PayWindow pay=new PayWindow((UUID)id,0);
				pay.Show();
			}
		}

		protected virtual void OnButtonProfileClicked (object sender, System.EventArgs e)
		{
			Gtk.TreeModel mod;
			Gtk.TreeIter iter;
			
			if(treeview_friends.Selection.GetSelected(out mod,out iter))			
			{
				string id=(string)mod.GetValue(iter,6);
				UUID lid=(UUID)id;
					
				ProfileVIew profile=new ProfileVIew(lid);
				profile.Show();
			}
		}

		[GLib.ConnectBefore]
		protected virtual void OnTreeviewFriendsButtonPressEvent (object o, Gtk.ButtonPressEventArgs args)
		{
			if (args.Event.Button == 3)//Fuck this should be a define
            {
                Gtk.TreeModel mod;
			    Gtk.TreeIter iter;
	            if (this.treeview_friends.Selection.GetSelected(out mod, out iter))
                {   
					string id=(string)mod.GetValue(iter,6);
					UUID lid=(UUID)id;
					FriendInfo finfo;
					if(MainClass.client.Friends.FriendList.TryGetValue(lid,out finfo))
					{
						Gtk.CheckMenuItem see_me_online=new Gtk.CheckMenuItem("Can see my online status");
					    see_me_online.Active=finfo.CanSeeMeOnline;
						see_me_online.ButtonPressEvent+=new ButtonPressEventHandler(OnCheckbuttonOnlinestatusClicked);
						                                                            
						Gtk.CheckMenuItem see_me_on_map=new Gtk.CheckMenuItem("Can see on the map");
					    see_me_on_map.Active=finfo.CanSeeMeOnMap;
						see_me_on_map.ButtonPressEvent+=new ButtonPressEventHandler(OnCheckbuttonMapClicked);
						
						Gtk.CheckMenuItem modify_mine=new Gtk.CheckMenuItem("Can modify my objects");
					    modify_mine.Active=finfo.CanModifyMyObjects;
						modify_mine.ButtonPressEvent+=new ButtonPressEventHandler(OnCheckbuttonModobjectsClicked);
							
						Gtk.Menu menu = new Gtk.Menu();						
	                    menu.Append(see_me_online);
				        menu.Append(see_me_on_map);
				        menu.Append(modify_mine);
				    	menu.Popup();
						menu.ShowAll();
					}				
					}
			}
		}
	}
}

