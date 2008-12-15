/*
omvviewerlight a Text based client to metaverses such as Linden Labs Secondlife(tm)
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
		
		public FriendsList()
		{
			Console.Write("Building friends list window\n");
			this.Build();
			store= new Gtk.ListStore (typeof(Gdk.Pixbuf),typeof(string),typeof(string),typeof(bool));
			
			treeview_friends.AppendColumn("Online",new CellRendererPixbuf(),"pixbuf",0);
			treeview_friends.AppendColumn("Name",new Gtk.CellRendererText(),"text",1);		
			treeview_friends.Model=store;
            store.SetSortColumnId(1, SortType.Ascending);
            store.SetSortFunc(1,sortfunc);
			
			online_img=MainClass.GetResource("icon_avatar_online.tga");
			offline_img=MainClass.GetResource("icon_avatar_offline.tga");
				
			MainClass.client.Network.OnLogin += new OpenMetaverse.NetworkManager.LoginCallback(onLogin);		
			MainClass.client.Friends.OnFriendOnline += new OpenMetaverse.FriendsManager.FriendOnlineEvent(onFriendOnline);
			MainClass.client.Friends.OnFriendOffline += new OpenMetaverse.FriendsManager.FriendOfflineEvent(onFriendOffline);
            MainClass.client.Friends.OnFriendshipResponse += new FriendsManager.FriendshipResponseEvent(Friends_OnFriendshipResponse);
            MainClass.client.Friends.OnFriendshipTerminated += new FriendsManager.FriendshipTerminatedEvent(Friends_OnFriendshipTerminated);
        }

        int sortfunc(Gtk.TreeModel model, Gtk.TreeIter a, Gtk.TreeIter b)
        {
            string nameA = (string)store.GetValue(a, 1);
            string nameB = (string)store.GetValue(b, 1);

            bool Pa = (bool)store.GetValue(a, 3);
            bool Pb =(bool)store.GetValue(b, 3);

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
                Gtk.TreeIter iter = store.AppendValues(friend.IsOnline ? online_img : offline_img, friend.Name, friend.UUID.ToString(), friend.IsOnline);
				AsyncNameUpdate ud=new AsyncNameUpdate(friend.UUID,false);  
				ud.addparameters(iter);
				ud.onNameCallBack += delegate(string namex,object[] values){Gtk.TreeIter iterx=(Gtk.TreeIter)values[0]; store.SetValue(iterx,1,namex);};
                ud.go();
            });
		}
			
		
		bool myfunc(Gtk.TreeModel mod, Gtk.TreePath path, Gtk.TreeIter iter)
		{			
			string id =(string)store.GetValue(iter,2);
			UUID lid=(UUID)id;
		
			FriendInfo finfo;
			if(MainClass.client.Friends.FriendList.TryGetValue(lid,out finfo))
			{
                store.SetValue(iter, 0, finfo.IsOnline ? MainClass.GetResource("icon_avatar_online.tga") : MainClass.GetResource("icon_avatar_offline.tga"));
			    store.SetValue(iter, 3, finfo.IsOnline);
            }
			return false;
		}
		
		protected virtual void OnTreeviewFriendsCursorChanged (object sender, System.EventArgs e)
		{
			Gtk.TreeModel mod;
			Gtk.TreeIter iter;
			
			if(treeview_friends.Selection.GetSelected(out mod,out iter))			
			   {
				string id=(string)mod.GetValue(iter,2);
				UUID lid=(UUID)id;
				FriendInfo finfo;
				if(MainClass.client.Friends.FriendList.TryGetValue(lid,out finfo))
				{
					this.checkbutton_modobjects.Active=finfo.CanModifyTheirObjects;
					this.checkbutton_onlinestatus.Active=finfo.CanSeeMeOnline;
					this.checkbutton_map.Active=finfo.CanSeeMeOnMap;
				}
			}
		}

		protected virtual void OnCheckbuttonOnlinestatusClicked (object sender, System.EventArgs e)
		{
			Gtk.TreeModel mod;
			Gtk.TreeIter iter;
			
			if(treeview_friends.Selection.GetSelected(out mod,out iter))			
			{
				string id=(string)mod.GetValue(iter,2);
				UUID lid=(UUID)id;
				FriendInfo finfo;
				if(MainClass.client.Friends.FriendList.TryGetValue(lid,out finfo))
				{
					finfo.CanSeeMeOnline=this.checkbutton_onlinestatus.Active;

					FriendRights rights=getrights(finfo);

					if(finfo.CanSeeMeOnline)
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
				string id=(string)mod.GetValue(iter,2);
				UUID lid=(UUID)id;
				FriendInfo finfo;
				if(MainClass.client.Friends.FriendList.TryGetValue(lid,out finfo))
				{
					finfo.CanSeeMeOnMap=this.checkbutton_map.Active;
					FriendRights rights=getrights(finfo);

					if(finfo.CanSeeMeOnMap)
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
				string id=(string)mod.GetValue(iter,2);
				UUID lid=(UUID)id;
				FriendInfo finfo;
				if(MainClass.client.Friends.FriendList.TryGetValue(lid,out finfo))
				{
					finfo.CanModifyMyObjects=this.checkbutton_modobjects.Active;
					FriendRights rights=getrights(finfo);

					if(finfo.CanModifyMyObjects)
						rights|=FriendRights.CanModifyObjects;
					else
						rights&=~FriendRights.CanModifyObjects;
	
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
				string id=(string)mod.GetValue(iter,2);
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
				string id=(string)mod.GetValue(iter,2);
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
				string id=(string)mod.GetValue(iter,2);
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
				string id=(string)mod.GetValue(iter,2);
				UUID lid=(UUID)id;
					
				ProfileVIew profile=new ProfileVIew(lid);
				profile.Show();
			}
		}
	}
}

