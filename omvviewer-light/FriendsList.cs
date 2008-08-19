// FriendsList.cs created with MonoDevelop
// User: robin at 10:56 11/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;
using libsecondlife;
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
			store= new Gtk.ListStore (typeof(Gdk.Pixbuf),typeof(string),typeof(string));
			
			treeview_friends.AppendColumn("Online",new CellRendererPixbuf(),"pixbuf",0);
			treeview_friends.AppendColumn("Name",new Gtk.CellRendererText(),"text",1);		
			treeview_friends.Model=store;
			
			online_img=new Gdk.Pixbuf("icon_avatar_online.tga");
			offline_img=new Gdk.Pixbuf("icon_avatar_offline.tga");
				
			MainClass.client.Avatars.OnAvatarNames += new libsecondlife.AvatarManager.AvatarNamesCallback(onAvatarNames);			
			MainClass.client.Network.OnLogin += new libsecondlife.NetworkManager.LoginCallback(onLogin);		
			MainClass.client.Friends.OnFriendOnline += new libsecondlife.FriendsManager.FriendOnlineEvent(onFriendOnline);
			MainClass.client.Friends.OnFriendOffline += new libsecondlife.FriendsManager.FriendOfflineEvent(onFriendOffline);			
		}	
		
		void onLogin(LoginStatus status,string message)
		{
			if(LoginStatus.Success==status)
			{
				Gtk.Application.Invoke(delegate {
				store.Clear();
				populate_list();
				store.Foreach(myfunc);
				});
			}
		}
		
		void onFriendOnline(FriendInfo friend)
		{
			//Untill i can find a better way to access the store directly
			Gtk.Application.Invoke(delegate {			
				store.Foreach(myfunc);
			});
		
		}
		
		void onFriendOffline(FriendInfo friend)
		{
			Gtk.Application.Invoke(delegate {			
				store.Foreach(myfunc);
			});	
		}
		
		void populate_list()		
		{
			MainClass.client.Friends.FriendList.ForEach(delegate(FriendInfo friend)
			{
				store.AppendValues (friend.IsOnline?online_img:offline_img,friend.Name,friend.UUID.ToString());
			});
		}
			
		void onAvatarNames(Dictionary<LLUUID, string> names)
		{	
			foreach(KeyValuePair<LLUUID,string> name in names)
			{
				if(!MainClass.av_names.ContainsKey(name.Key))
					MainClass.av_names.Add(name.Key,name.Value);		
			}
			
			Gtk.Application.Invoke(delegate {			
				store.Foreach(myfunc);
			});	

		}
		
		bool myfunc(Gtk.TreeModel mod, Gtk.TreePath path, Gtk.TreeIter iter)
		{			
			string id =(string)store.GetValue(iter,2);
			LLUUID lid=(LLUUID)id;
		
			if(MainClass.av_names!=null)
			{			
				string name;
				if(MainClass.av_names.TryGetValue(lid,out name))
				{
					store.SetValue(iter,1,name);
				}
			}
			
			FriendInfo finfo;
			if(MainClass.client.Friends.FriendList.TryGetValue(lid,out finfo))
			{
				store.SetValue(iter,0,finfo.IsOnline?new Gdk.Pixbuf("icon_avatar_online.tga"):new Gdk.Pixbuf("icon_avatar_offline.tga"));
			}
			return false;
		}
		
		protected virtual void OnTreeviewFriendsCursorChanged (object sender, System.EventArgs e)
		{
			Gtk.TreeModel mod;
			Gtk.TreeIter iter;
			
			if(treeview_friends.Selection.GetSelected(out mod,out iter))			
			   {
				//ALL i want is a fucking UUID
				string id=(string)mod.GetValue(iter,2);
				LLUUID lid=(LLUUID)id;
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
				//ALL i want is a fucking UUID
				string id=(string)mod.GetValue(iter,2);
				LLUUID lid=(LLUUID)id;
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
				//ALL i want is a fucking UUID
				string id=(string)mod.GetValue(iter,2);
				LLUUID lid=(LLUUID)id;
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
				//ALL i want is a fucking UUID
				string id=(string)mod.GetValue(iter,2);
				LLUUID lid=(LLUUID)id;
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
				//ALL i want is a fucking UUID
				string id=(string)mod.GetValue(iter,2);
				LLUUID lid=(LLUUID)id;
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
				LLUUID lid=(LLUUID)id;
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
				PayWindow pay=new PayWindow(id,0);
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
				LLUUID lid=(LLUUID)id;
					
				ProfileVIew profile=new ProfileVIew(lid);
				profile.Show();
			}

		
		}
		
	}
}

