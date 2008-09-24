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

// GroupInfo.cs created with MonoDevelop
// User: robin at 17:59 18/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;
using OpenMetaverse;
using Gtk;

namespace omvviewerlight
{
	
	
	public partial class GroupInfo : Gtk.Window
	{
		Gtk.ListStore store_members;		
		Gtk.ListStore store_membersandroles_members;

        bool nobody_cares = false;

		UUID groupkey;
		UUID founder_key;
		
		public GroupInfo(Group group) : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();

			groupkey=group.ID;
			
			store_members = new Gtk.ListStore (typeof(string),typeof(string),typeof(string),typeof(UUID));			
			treeview_members.AppendColumn("Member name",new CellRendererText(),"text",0);
			treeview_members.AppendColumn("Title",new CellRendererText(),"text",1);
			treeview_members.AppendColumn("Last login",new CellRendererText(),"text",2);
			treeview_members.Model=store_members;
				
			//Tree view for Members & Roles, Members
			
			this.store_membersandroles_members=new Gtk.ListStore(typeof(string),typeof(string),typeof(string),typeof(UUID));
			this.treeview_members1.AppendColumn("Member name",new CellRendererText(),"text",0);
			this.treeview_members1.AppendColumn("Land",new CellRendererText(),"text",1);
			this.treeview_members1.AppendColumn("Title",new CellRendererText(),"text",2);
			this.treeview_members1.Model=store_membersandroles_members;		
			
			MainClass.client.Groups.OnGroupProfile += new OpenMetaverse.GroupManager.GroupProfileCallback(onGroupProfile);
			MainClass.client.Groups.OnGroupMembers += new OpenMetaverse.GroupManager.GroupMembersCallback(onGroupMembers);
			MainClass.client.Groups.OnGroupTitles += new OpenMetaverse.GroupManager.GroupTitlesCallback(onGroupTitles);
			
			MainClass.client.Groups.RequestGroupProfile(group.ID);
			MainClass.client.Groups.RequestGroupMembers(group.ID);
			MainClass.client.Groups.RequestGroupTitles(group.ID);
			MainClass.client.Groups.RequestGroupRoles(group.ID);
			
			TryGetImage img=new TryGetImage(this.image_group_emblem,group.InsigniaID);
			this.label_name.Text=group.Name;
	
			AsyncNameUpdate ud=new AsyncNameUpdate(group.FounderID,false);  
			ud.onNameCallBack += delegate(string namex,object[] values){this.label_foundedby.Text="Founded by "+namex;};

			this.entry_enrollmentfee.Text=group.MembershipFee.ToString();
			if(group.MembershipFee>0)
				this.checkbutton_mature.Active=true;
			
			this.checkbutton_group_notices.Active=group.AcceptNotices;
			this.checkbutton_openenrolement.Active=group.OpenEnrollment;
			//this.checkbutton_showinpofile.Active=group.AllowPublish;
			this.checkbutton_showinsearch.Active=group.ShowInList;
			this.checkbutton_mature.Active=group.MaturePublish;
			this.textview_group_charter.Buffer.Text=group.Charter;
            this.DeleteEvent += new DeleteEventHandler(GroupWindow_DeleteEvent);
	
		}

         [GLib.ConnectBefore]
        void GroupWindow_DeleteEvent(object o, DeleteEventArgs args)
        {
            nobody_cares = true;
            MainClass.client.Groups.OnGroupProfile -= new OpenMetaverse.GroupManager.GroupProfileCallback(onGroupProfile);
            MainClass.client.Groups.OnGroupMembers -= new OpenMetaverse.GroupManager.GroupMembersCallback(onGroupMembers);
            MainClass.client.Groups.OnGroupTitles -= new OpenMetaverse.GroupManager.GroupTitlesCallback(onGroupTitles);
            this.DeleteEvent -= new DeleteEventHandler(GroupWindow_DeleteEvent);
        }
		
		void onGroupTitles(Dictionary <UUID,OpenMetaverse.GroupTitle> titles)
		{
			Gtk.Application.Invoke(delegate {	

			foreach(KeyValuePair  <UUID,OpenMetaverse.GroupTitle> title in titles)
			{
				this.combobox_active_title.InsertText(0,title.Value.Title);
				Console.Write("Title : "+title.Value.Title+" : "+title.Value.Selected.ToString()+"\n");
				if(title.Value.Selected)
				{
						this.combobox_active_title.Active=0;
				}
			}	
		
			});
		}
		
		void onGroupMembers(Dictionary <UUID,GroupMember> members)		
		{
			List<UUID> names = new List<UUID>(members.Keys);
			MainClass.name_cache.reqnames(names);

            Console.Write("\n Got the group list\n");

			foreach(KeyValuePair <UUID,GroupMember> member in members)
			{

				Gtk.TreeIter iter=store_members.AppendValues("Waiting...",member.Value.Title,member.Value.OnlineStatus,member.Value.ID);
				
				AsyncNameUpdate ud=new AsyncNameUpdate(member.Value.ID,false);  
				ud.addparameters(iter);
				ud.onNameCallBack += delegate(string namex,object[] values){if(nobody_cares){return;} Gtk.TreeIter iterx=(Gtk.TreeIter)values[0]; store_members.SetValue(iterx,0,namex);};

                Gtk.TreeIter iter2 = store_membersandroles_members.AppendValues("Waiting...", member.Value.Contribution.ToString(), member.Value.Title);
				AsyncNameUpdate ud2=new AsyncNameUpdate(member.Value.ID,false);  
				ud2.addparameters(iter2);
				ud2.onNameCallBack += delegate(string namex,object[] values){if(nobody_cares){return;} Gtk.TreeIter iterx=(Gtk.TreeIter)values[0]; store_membersandroles_members.SetValue(iterx,0,namex);};		
			
			}

            Console.Write("Queueing tree for draw\n");

			Gtk.Application.Invoke(delegate {	
					this.treeview_members.QueueDraw();
                    this.treeview_members1.QueueDraw();
				});
		}
		
		void onGroupProfile(Group group)
		{
			
			if(group.ID!=this.groupkey)
				return;
			
			Gtk.Application.Invoke(delegate {	
			
			this.entry_enrollmentfee.Text=group.MembershipFee.ToString();
			
			if(group.MembershipFee>0)
				this.checkbutton_mature.Active=true;
			
		//	this.checkbutton_group_notices.Active=group.AcceptNotices;
			this.checkbutton_openenrolement.Active=group.OpenEnrollment;
			this.checkbutton_showinpofile.Active=group.AllowPublish;
			this.checkbutton_showinsearch.Active=group.ShowInList;
			this.checkbutton_mature.Active=group.MaturePublish;
			this.textview_group_charter.Buffer.Text=group.Charter;

			if((group.Powers & GroupPowers.SendNotices)==GroupPowers.SendNotices)
					this.button_send_notice.Sensitive=true;
				else
					this.button_send_notice.Sensitive=false;
					
				
				
			});
		}
		
		bool myfunc_members(Gtk.TreeModel mod, Gtk.TreePath path, Gtk.TreeIter iter)
		{			
			bool stillwaiting;
			string name=(string)store_members.GetValue(iter,0);
			UUID id =(UUID)store_members.GetValue(iter,3);
			string member_name;
			if(MainClass.name_cache.av_names.TryGetValue(id,out member_name))
			{
				store_members.SetValue(iter,0,member_name);
			}
				return false;
		}

		protected virtual void OnComboboxActiveTitleChanged (object sender, System.EventArgs e)
		{
			//MainClass.client.Groups.ActivateTitle(
		
		}

		protected virtual void OnCheckbuttonGroupNoticesClicked (object sender, System.EventArgs e)
		{
			if(this.checkbutton_group_notices.Active)
			{
//				MainClass.client.Groups.SendGroupNotice();
	//			GroupNotice note;
				
			}
			else
			{
			}
			
		}

		protected virtual void OnCheckbuttonShowinpofileClicked (object sender, System.EventArgs e)
		{
			if(this.checkbutton_showinpofile.Active)
			{
			}
			else
			{
			}
		}
		
		
	}
}
