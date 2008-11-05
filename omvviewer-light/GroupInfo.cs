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
		//Dictionary <UUID, GroupRole> grouproles;
		//List<KeyValuePair<UUID,UUID>> rolesmembers;
		Gtk.ListStore store_members;		
		Gtk.ListStore store_membersandroles_members;
		Gtk.ListStore assigned_roles;
        Gtk.ListStore notice_list;
        Gtk.TreeStore store_membersandroles_powers;
        Gtk.TreeStore store_roles_list;
        Gtk.TreeStore store_roles_abilities;
        Gtk.TreeStore store_roles_members;

        Gtk.TreeStore store_abilities;
        Gtk.TreeStore store_roles_with_ability;
        Gtk.TreeStore store_members_with_ability;
		
        UUID request_members;
        UUID request_titles;
        UUID request_roles;
        UUID request_roles_members;

        bool nobody_cares = false;

        bool name_poll = false;
		
		UUID groupkey;

        List<UUID> rcvd_names = new List<UUID>();

		Gdk.Pixbuf folder_open = new Gdk.Pixbuf("inv_folder_plain_open.tga");
		Gdk.Pixbuf tick = new Gdk.Pixbuf("tick.tga");
		Gdk.Pixbuf cross = new Gdk.Pixbuf("cross.tga");
		
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

			//Tree view for Roles
			assigned_roles = new Gtk.ListStore (typeof(bool),typeof(string),typeof(GroupPowers));					
			this.treeview_assigned_roles.AppendColumn("",new Gtk.CellRendererToggle(),"active",0);
			this.treeview_assigned_roles.AppendColumn("Role",new CellRendererText(),"text",1);
			this.treeview_assigned_roles.Model=assigned_roles;
			
            //Tree view for group notices
            notice_list = new Gtk.ListStore(typeof(string), typeof(string), typeof(UUID));
            this.treeview_notice_list.AppendColumn("From", new CellRendererText(), "text", 0);
            this.treeview_notice_list.AppendColumn("Subject", new CellRendererText(), "text", 1);
            this.treeview_notice_list.Model = notice_list;

            store_membersandroles_powers = new Gtk.TreeStore(typeof(Gdk.Pixbuf), typeof(string), typeof(GroupPowers));
			this.treeview_allowed_ability1.AppendColumn("",new CellRendererPixbuf(),"pixbuf",0);
            this.treeview_allowed_ability1.AppendColumn("Role", new CellRendererText(), "text", 1);
            this.treeview_allowed_ability1.Model = store_membersandroles_powers;

			this.store_roles_list = new Gtk.TreeStore(typeof(string),typeof(string),typeof(string),typeof(UUID));
			this.treeview_roles_list.AppendColumn("Role Name",new CellRendererText(), "text", 0);
			this.treeview_roles_list.AppendColumn("Title",new CellRendererText(), "text", 1);
			this.treeview_roles_list.AppendColumn("Members",new CellRendererText(), "text", 2);
			this.treeview_roles_list.Model=this.store_roles_list;

			this.store_roles_abilities = new Gtk.TreeStore(typeof(Gdk.Pixbuf), typeof(string), typeof(UUID));
			this.treeview_roles_abilities.AppendColumn("",new CellRendererPixbuf(),"pixbuf",0);
			//this.treeview_allowed_ability1.AppendColumn("", new Gtk.CellRendererToggle(), "active", 0);
            this.treeview_roles_abilities.AppendColumn("Allowed Abilities", new CellRendererText(), "text", 1);
            this.treeview_roles_abilities.Model = this.store_roles_abilities;
			
			this.store_roles_members = new Gtk.TreeStore(typeof(string));
			treeview_roles_assigned_members.AppendColumn("Assigned Members", new CellRendererText(), "text", 0);
			treeview_roles_assigned_members.Model=this.store_roles_members;
		
			this.store_abilities=new Gtk.TreeStore(typeof(Gdk.Pixbuf), typeof(string), typeof(GroupPowers));
			this.treeview_abilities.AppendColumn("",new CellRendererPixbuf(),"pixbuf",0);
			this.treeview_abilities.AppendColumn("Abilities", new CellRendererText(), "text", 1);
			this.treeview_abilities.Model=this.store_abilities;
			
			this.store_members_with_ability=new Gtk.TreeStore(typeof(string));
			this.treeview_members_with_ability.AppendColumn("Members with ability",new CellRendererText(), "text", 0);
			this.treeview_members_with_ability.Model=this.store_members_with_ability;
			
			this.store_roles_with_ability=new Gtk.TreeStore(typeof(string));
			this.treeview_roles_with_ability.AppendColumn("Roles with ability",new CellRendererText(), "text", 0);
			this.treeview_roles_with_ability.Model=this.store_roles_with_ability;

			GroupPowers powers=new GroupPowers();
			this.showpowers(this.store_abilities,powers);
			this.treeview_abilities.ExpandAll();
			
			MainClass.client.Groups.OnGroupProfile += new OpenMetaverse.GroupManager.GroupProfileCallback(onGroupProfile);
            MainClass.client.Groups.OnGroupMembers += new OpenMetaverse.GroupManager.GroupMembersCallback(onGroupMembers);
            MainClass.client.Groups.OnGroupTitles += new OpenMetaverse.GroupManager.GroupTitlesCallback(onGroupTitles);
            MainClass.client.Groups.OnGroupRoles += new OpenMetaverse.GroupManager.GroupRolesCallback(onGroupRoles);
            MainClass.client.Groups.OnGroupRolesMembers += new OpenMetaverse.GroupManager.GroupRolesMembersCallback(onGroupRolesMembers);
            MainClass.client.Groups.OnGroupNoticesList += new GroupManager.GroupNoticesListCallback(Groups_OnGroupNoticesList);

			MainClass.client.Self.OnInstantMessage += new OpenMetaverse.AgentManager.InstantMessageCallback(onIM);			
			
			MainClass.client.Groups.RequestGroupProfile(group.ID);

            Console.WriteLine("group id is " + group.ID.ToString());
            rcvd_names.Clear();

            name_poll = true;
            Gtk.Timeout.Add(500, updategroupmembers);
            request_members = MainClass.client.Groups.RequestGroupMembers(group.ID);
            request_titles = MainClass.client.Groups.RequestGroupTitles(group.ID);
            request_roles = MainClass.client.Groups.RequestGroupRoles(group.ID);  //this is indexed by group ID
            request_roles_members = MainClass.client.Groups.RequestGroupRoleMembers(group.ID);

            request_roles = group.ID; //CORRECT
            request_roles_members = group.ID;
            request_titles = group.ID;
            //request_members = group.ID;

            MainClass.client.Groups.RequestGroupNoticeList(group.ID);
			
			TryGetImage img=new TryGetImage(this.image_group_emblem,group.InsigniaID,128,128);
			this.label_name.Text=group.Name;
	
			AsyncNameUpdate ud=new AsyncNameUpdate(group.FounderID,false);  
			ud.onNameCallBack += delegate(string namex,object[] values){this.label_foundedby.Text="Founded by "+namex;};
            ud.go();

			this.entry_enrollmentfee.Text=group.MembershipFee.ToString();
			if(group.MembershipFee>0)
				this.checkbutton_mature.Active=true;
			
			this.checkbutton_group_notices.Active=group.AcceptNotices;
			this.checkbutton_openenrolement.Active=group.OpenEnrollment;
			this.checkbutton_showinpofile.Active=group.AllowPublish;
			this.checkbutton_showinsearch.Active=group.ShowInList;
			this.checkbutton_mature.Active=group.MaturePublish;
			this.textview_group_charter.Buffer.Text=group.Charter;
            this.DeleteEvent += new DeleteEventHandler(GroupWindow_DeleteEvent);
	
			this.notebook1.Page=0;
			this.notebook2.Page=0;
			
		}

        void Groups_OnGroupNoticesList(UUID groupID, GroupNoticeList notice)
        {
            if (groupID != this.groupkey)
                return;
            Console.Write("Notice list entry: From: "+notice.FromName+"\nSubject: "+notice.Subject + "\n");

            this.notice_list.AppendValues(notice.FromName, notice.Subject, notice.NoticeID);
        }

		void onGroupRolesMembers(List<KeyValuePair<UUID,UUID>> rolesmember)
		{
			Console.Write("Group roles members recieved\n");

			//rolesmembers=rolesmember;
		}
		
		void onGroupRoles(Dictionary <UUID, GroupRole> roles)
		{
			// Maybe we should flag up that the roles have been recieved?
			Console.Write("Group roles recieved\n");
			//grouproles=roles;
			
			foreach(KeyValuePair <UUID,GroupRole> kvp in roles)
			{
				this.store_roles_list.AppendValues(kvp.Value.Name,kvp.Value.Title,"0",kvp.Value.ID);
			}
		}
		
        [GLib.ConnectBefore]
        void GroupWindow_DeleteEvent(object o, DeleteEventArgs args)
        {
            nobody_cares = true;
            MainClass.client.Groups.OnGroupProfile -= new OpenMetaverse.GroupManager.GroupProfileCallback(onGroupProfile);
            MainClass.client.Groups.OnGroupMembers -= new OpenMetaverse.GroupManager.GroupMembersCallback(onGroupMembers);
            MainClass.client.Groups.OnGroupTitles -= new OpenMetaverse.GroupManager.GroupTitlesCallback(onGroupTitles);
            MainClass.client.Groups.OnGroupRoles -= new OpenMetaverse.GroupManager.GroupRolesCallback(onGroupRoles);
            MainClass.client.Groups.OnGroupRolesMembers -= new OpenMetaverse.GroupManager.GroupRolesMembersCallback(onGroupRolesMembers);
            MainClass.client.Groups.OnGroupNoticesList -= new GroupManager.GroupNoticesListCallback(Groups_OnGroupNoticesList);
			MainClass.client.Self.OnInstantMessage -= new OpenMetaverse.AgentManager.InstantMessageCallback(onIM);			
			     
			
			this.DeleteEvent -= new DeleteEventHandler(GroupWindow_DeleteEvent);
        }
		
		void onGroupTitles(Dictionary <UUID,OpenMetaverse.GroupTitle> titles)
		{
            Console.Write("Group titles recieved\n");

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

        bool updategroupmembers()
        {

            List<UUID> names = new List<UUID>();

            //List<UUID> names = new List<UUID>(MainClass.client.Groups.GroupMembersCaches.Dictionary[request_members].Keys);
            //MainClass.name_cache.reqnames(names);

            if (!MainClass.client.Groups.GroupMembersCaches.Dictionary.ContainsKey(request_members))
                return name_poll;

            lock(MainClass.client.Groups.GroupMembersCaches)
            {
                foreach (KeyValuePair<UUID, GroupMember> member in MainClass.client.Groups.GroupMembersCaches.Dictionary[request_members])
                {
                    if (!rcvd_names.Contains(member.Key))
                    {
                        rcvd_names.Add(member.Key);
                        names.Add(member.Key);

                        Gtk.TreeIter iter = store_members.AppendValues("Waiting...", member.Value.Title, member.Value.OnlineStatus, member.Value.ID);

                        AsyncNameUpdate ud = new AsyncNameUpdate(member.Value.ID, false);
                        ud.addparameters(iter);
                        ud.onNameCallBack += delegate(string namex, object[] values) { if (nobody_cares) { return; } Gtk.TreeIter iterx = (Gtk.TreeIter)values[0]; store_members.SetValue(iterx, 0, namex); };
                        ud.go();

                        Gtk.TreeIter iter2 = store_membersandroles_members.AppendValues("Waiting...", member.Value.Contribution.ToString(), member.Value.Title, member.Value.ID);
                        AsyncNameUpdate ud2 = new AsyncNameUpdate(member.Value.ID, false);
                        ud2.addparameters(iter2);
                        ud2.onNameCallBack += delegate(string namex, object[] values) { if (nobody_cares) { return; } Gtk.TreeIter iterx = (Gtk.TreeIter)values[0]; store_membersandroles_members.SetValue(iterx, 0, namex); };
                        ud2.go();


                    }
                }
            }

            MainClass.name_cache.reqnames(names);

          
            this.treeview_members.QueueDraw();
            this.treeview_members1.QueueDraw();
            

            return name_poll;
        }

		void onGroupMembers(Dictionary <UUID,GroupMember> members)		
		{

            Console.WriteLine("All group members recieved");
            name_poll = false;
            return;

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

		protected virtual void OnTreeviewMembers1CursorChanged (object sender, System.EventArgs e)
		{
			//This is the Members List on the Members role tab
			Gtk.TreeModel mod;
			Gtk.TreeIter iter;
            Dictionary <UUID, GroupRole> grouproles;
            Dictionary<UUID, GroupMember> groupmembers;
            List<KeyValuePair<UUID,UUID>> rolesmembers;
			
			if(this.treeview_members1.Selection.GetSelected(out mod,out iter))			
			{
				UUID id=(UUID)mod.GetValue(iter,3);
				Console.Write("Selected id "+id.ToString()+"\n");
                GroupMember member;
				//Now populate the roles list

                if (!MainClass.client.Groups.GroupRolesCaches.TryGetValue(request_roles, out grouproles))
                    return;
                if(!MainClass.client.Groups.GroupRolesMembersCaches.TryGetValue(request_roles_members, out rolesmembers))
                     return;
                if (!MainClass.client.Groups.GroupMembersCaches.TryGetValue(request_members, out groupmembers)) 
                     return;

                store_membersandroles_powers.Clear();

				Console.WriteLine("Tring to get group powers for id "+id.ToString());
				
                if (groupmembers.TryGetValue(id, out member))
                {
					Console.WriteLine("Got a power "+member.Powers.ToString());
					showpowers(store_membersandroles_powers,member.Powers);
					this.treeview_allowed_ability1.ExpandAll();
                }

    		    this.assigned_roles.Clear();
				
			    Console.Write("Got group roles from cache\n");
	
				foreach(KeyValuePair<UUID,GroupRole> kvp in grouproles)
				{
					bool ingroup=false;
					Console.Write("Appending value "+kvp.Value.Name+"\n");

					foreach(KeyValuePair<UUID,UUID> rolesmember in rolesmembers)
					{
						if(rolesmember.Value==id && kvp.Value.ID==rolesmember.Key)
							ingroup=true;
					}
					
					assigned_roles.AppendValues(ingroup,kvp.Value.Name,kvp.Value.ID);	
				}
									
			}
		}

		protected virtual void OnTreeviewNoticeListCursorChanged (object sender, System.EventArgs e)
		{
			//This is the Members List on the Members role tab
			Gtk.TreeModel mod;
			Gtk.TreeIter iter;
			
			if(this.treeview_notice_list.Selection.GetSelected(out mod,out iter))			
			{
				UUID id=(UUID)mod.GetValue(iter,2);
				MainClass.client.Groups.RequestGroupNotice(id);
				this.entry1.Text=(string)mod.GetValue(iter,1);
			}
		
		}
			
		void onIM(InstantMessage im, Simulator sim)
		{
			
			if(im.Dialog!=OpenMetaverse.InstantMessageDialog.GroupNoticeRequested)
				return;
			textview_notice.Buffer.SetText(im.Message);
		}

		protected virtual void OnTreeviewRolesListCursorChanged (object sender, System.EventArgs e)
		{
				//This is the Members List on the Members role tab
					Gtk.TreeModel mod;
					Gtk.TreeIter iter;
					if(this.treeview_roles_list.Selection.GetSelected(out mod,out iter))			
					{
						UUID id=(UUID)mod.GetValue(iter,3);
						Dictionary <UUID, GroupRole> grouproles;

                        if (!MainClass.client.Groups.GroupRolesCaches.TryGetValue(request_roles, out grouproles))
                            return;

						GroupRole role;
						grouproles.TryGetValue(id,out role);
						this.entry_roles_name.Text=role.Name;
			            this.entry_roles_title.Text=role.Title;
	                    this.textview_roles_description.Buffer.Text=role.Description;             
                        this.store_roles_abilities.Clear();
				        showpowers(this.store_roles_abilities,role.Powers);
			            this.treeview_roles_abilities.ExpandAll();   
			            
				        //now to fine members with this role
				        List<KeyValuePair<UUID,UUID>> rolesmembers;
                        if (!MainClass.client.Groups.GroupRolesMembersCaches.TryGetValue(this.request_roles_members, out rolesmembers))
                            return;
				
				        this.store_roles_members.Clear();

                        if (role.ID == UUID.Zero)
                        {
                            foreach (KeyValuePair<UUID, GroupMember> member in MainClass.client.Groups.GroupMembersCaches.Dictionary[request_members])
                            {
                                string name = "";
                                MainClass.name_cache.av_names.TryGetValue(member.Key, out name);
                                this.store_roles_members.AppendValues(name);

                            }
                            return;
                        }

					    foreach(KeyValuePair<UUID,UUID> rolesmember in rolesmembers)
					    {
					        // rolesmember.Value is the user UUID
					        // .key is the group role UUIS
						    if(rolesmember.Key==id)
					        {    
						          string name="";
						          MainClass.name_cache.av_names.TryGetValue(rolesmember.Value,out name);
						          this.store_roles_members.AppendValues(name);
					        }
					    }

				
				
			       }			

		}

        void showpowers(Gtk.TreeStore store,GroupPowers powers)
        {
					Gtk.TreeIter iterx;
					bool test;
                    iterx = store.AppendValues(folder_open, "Membership Managment", GroupPowers.None);
					test=(powers & GroupPowers.Invite) == GroupPowers.Invite;
					store.AppendValues(iterx,test?tick:cross,"Invite people to group",GroupPowers.Invite);
					test=(powers & GroupPowers.Eject) == GroupPowers.Eject;
					store.AppendValues(iterx,test?tick:cross,"Eject members",GroupPowers.Eject);
					test=(powers & GroupPowers.ChangeOptions) == GroupPowers.ChangeOptions; //?????????
					store.AppendValues(iterx,test?tick:cross,"Toggle Open Enrollment",GroupPowers.ChangeOptions);

                   
					iterx = store.AppendValues(folder_open, "Roles", GroupPowers.None);
					test=(powers & GroupPowers.CreateRole) == GroupPowers.CreateRole; //?????????
					store.AppendValues(iterx,test?tick:cross,"Toggle Open Enrollment",GroupPowers.CreateRole);
					test=(powers & GroupPowers.DeleteRole) == GroupPowers.DeleteRole; //?????????
					store.AppendValues(iterx,test?tick:cross,"Delete ROles",GroupPowers.DeleteRole);
					test=(powers & GroupPowers.RoleProperties) == GroupPowers.RoleProperties; //?????????
					store.AppendValues(iterx,test?tick:cross,"Change ROle names,titles",GroupPowers.RoleProperties);
					test=(powers & GroupPowers.AssignMemberLimited) == GroupPowers.AssignMemberLimited; //?????????
					store.AppendValues(iterx,test?tick:cross,"Assign Members to Assigners Role",GroupPowers.AssignMemberLimited);
					test=(powers & GroupPowers.AssignMember) == GroupPowers.AssignMember; //?????????
					store.AppendValues(iterx,test?tick:cross,"Assign Members to Any Role",GroupPowers.AssignMember);
					test=(powers & GroupPowers.RemoveMember) == GroupPowers.RemoveMember; //?????????
					store.AppendValues(iterx,test?tick:cross,"Remove Members from Roles",GroupPowers.RemoveMember);
					test=(powers & GroupPowers.ChangeIdentity) == GroupPowers.ChangeIdentity; //?????????
					store.AppendValues(iterx,test?tick:cross,"Assign and Remove Abilities",GroupPowers.ChangeIdentity);
                  
                    return;
					iterx = store.AppendValues(folder_open, "Parcel Managment", GroupPowers.None);
					test=(powers & GroupPowers.LandDeed) == GroupPowers.LandDeed; //?????????
					store.AppendValues(iterx,test?tick:cross,"Deed land and buy land for group",GroupPowers.LandDeed);
					test=(powers & GroupPowers.LandRelease) == GroupPowers.LandRelease; //?????????
					store.AppendValues(iterx,test?tick:cross,"Abandon land",GroupPowers.LandRelease);
					test=(powers & GroupPowers.LandSetSale) == GroupPowers.LandSetSale; //?????????
					store.AppendValues(iterx,test?tick:cross,"Set land for sale info",GroupPowers.LandSetSale);					
					test=(powers & GroupPowers.LandDivideJoin) == GroupPowers.LandDivideJoin;
					store.AppendValues(iterx,test?tick:cross,"Join and Divide Parcels",GroupPowers.LandDivideJoin);
					                                              
                    iterx = store.AppendValues(folder_open, "Parcel Identy", GroupPowers.None);
					test=(powers & GroupPowers.FindPlaces) == GroupPowers.FindPlaces;
					store.AppendValues(iterx,test?tick:cross,"Toggle show in Find Places",GroupPowers.FindPlaces);
					test=(powers & GroupPowers.LandChangeIdentity) == GroupPowers.LandChangeIdentity;
					store.AppendValues(iterx,test?tick:cross,"Change parcel name and Description",GroupPowers.LandChangeIdentity);
					test=(powers & GroupPowers.SetLandingPoint) == GroupPowers.SetLandingPoint;
					store.AppendValues(iterx,test?tick:cross,"Set Landing point",GroupPowers.SetLandingPoint);				
						
					iterx = store.AppendValues(folder_open, "Parcel Settings", GroupPowers.None);
					test=(powers & GroupPowers.ChangeMedia) == GroupPowers.ChangeMedia;
					store.AppendValues(iterx,test?tick:cross,"Change music & media settings",GroupPowers.ChangeMedia);
    				test=(powers & GroupPowers.ChangeOptions) == GroupPowers.ChangeOptions;
					store.AppendValues(iterx,test?tick:cross,"Toggle various about->land options",GroupPowers.ChangeOptions);
                    
					iterx = store.AppendValues(folder_open, "Parcel Powers", GroupPowers.None);
    				test=(powers & GroupPowers.AllowEditLand) == GroupPowers.AllowEditLand;
					store.AppendValues(iterx,test?tick:cross,"Always allow Edit Terrain",GroupPowers.AllowEditLand);
    				test=(powers & GroupPowers.AllowFly) == GroupPowers.AllowFly;
					store.AppendValues(iterx,test?tick:cross,"Always allow fly",GroupPowers.AllowFly);
    				test=(powers & GroupPowers.AllowRez) == GroupPowers.AllowRez;
					store.AppendValues(iterx,test?tick:cross,"Always allow Create Objects",GroupPowers.AllowRez);
    				test=(powers & GroupPowers.AllowLandmark) == GroupPowers.AllowLandmark;
					store.AppendValues(iterx,test,"Always allow Create Landmarks",GroupPowers.AllowLandmark);
    				test=(powers & GroupPowers.AllowSetHome) == GroupPowers.AllowSetHome;
					store.AppendValues(iterx,test?tick:cross,"Allow Set Home to Hete on group land",GroupPowers.AllowSetHome);
					
			        iterx = store.AppendValues(folder_open, "Parcel Access", GroupPowers.None);
    				test=(powers & GroupPowers.LandManageAllowed) == GroupPowers.LandManageAllowed;
					store.AppendValues(iterx,test?tick:cross,"Manage parcel Access lists",GroupPowers.LandManageAllowed);    				test=(powers & GroupPowers.LandManageBanned) == GroupPowers.LandManageBanned;
					store.AppendValues(iterx,test?tick:cross,"Manage Ban lists",GroupPowers.LandManageBanned);    				test=(powers & GroupPowers.LandEjectAndFreeze) == GroupPowers.LandEjectAndFreeze;
					store.AppendValues(iterx,test?tick:cross,"Eject and freeze Residents on parcel",GroupPowers.LandEjectAndFreeze);					
                    iterx = store.AppendValues(folder_open, "Parcel Content", GroupPowers.None);
                    test=(powers & GroupPowers.ReturnGroupOwned) == GroupPowers.ReturnGroupOwned;
					store.AppendValues(iterx,test?tick:cross,"Return objects owner by group",GroupPowers.ReturnGroupSet);                    test=(powers & GroupPowers.ReturnGroupSet) == GroupPowers.ReturnGroupSet;
					store.AppendValues(iterx,test?tick:cross,"Return objects set to group",GroupPowers.ReturnGroupSet);                    test=(powers & GroupPowers.ReturnNonGroup) == GroupPowers.ReturnNonGroup;
					store.AppendValues(iterx,test?tick:cross,"Return non-group objects",GroupPowers.ReturnNonGroup);                    test=(powers & GroupPowers.LandGardening) == GroupPowers.LandGardening;
			        store.AppendValues(iterx,test?tick:cross,"Landscaping using Linden Plants",GroupPowers.LandGardening);					
                    iterx = store.AppendValues(folder_open, "Object Managment", GroupPowers.None);
                    test=(powers & GroupPowers.DeedObject) == GroupPowers.DeedObject;
					store.AppendValues(iterx,test?tick:cross,"Deed objects to group",GroupPowers.DeedObject);			        test=(powers & GroupPowers.ObjectManipulate) == GroupPowers.ObjectManipulate;
					store.AppendValues(iterx,test?tick:cross,"Manipulate (move,copy,modify) group objetcs",GroupPowers.ObjectManipulate);                    test=(powers & GroupPowers.ObjectSetForSale) == GroupPowers.ObjectSetForSale;
					store.AppendValues(iterx,test?tick:cross,"Set group objects for sale",GroupPowers.ObjectSetForSale);

                    iterx = store.AppendValues(folder_open, "Notices", GroupPowers.None);
					test=(powers & GroupPowers.SendNotices) == GroupPowers.SendNotices;
					store.AppendValues(iterx,test?tick:cross,"Send Notices",GroupPowers.SendNotices);                    test=(powers & GroupPowers.ReceiveNotices) == GroupPowers.ReceiveNotices;
					store.AppendValues(iterx,test?tick:cross,"Receive Notices and view past Notices",GroupPowers.ReceiveNotices);

                    iterx = store.AppendValues(folder_open, "Proposals", GroupPowers.None);
                    test=(powers & GroupPowers.StartProposal) == GroupPowers.StartProposal;
					store.AppendValues(iterx,test?tick:cross,"Create Proposals",GroupPowers.StartProposal);                    test=(powers & GroupPowers.VoteOnProposal) == GroupPowers.VoteOnProposal;
					store.AppendValues(iterx,test?tick:cross,"Vote on Proposals",GroupPowers.VoteOnProposal);

                    iterx = store.AppendValues(folder_open, "Chat", GroupPowers.None);
			        test=(powers & GroupPowers.JoinChat) == GroupPowers.JoinChat;
			        store.AppendValues(iterx,test?tick:cross,"Join Group Chat",GroupPowers.JoinChat);			        test=(powers & GroupPowers.AllowVoiceChat) == GroupPowers.AllowVoiceChat;
					store.AppendValues(iterx,test?tick:cross,"Join Group Voice Chat",GroupPowers.AllowVoiceChat);    
				this.treeview_allowed_ability1.ExpandAll();
				
				
				
			}
					
					protected virtual void OnTreeviewAbilitiesCursorChanged (object sender, System.EventArgs e)
					{
					Gtk.TreeModel mod;
			Gtk.TreeIter iter;
					if(this.treeview_abilities.Selection.GetSelected(out mod,out iter))			
			{
				GroupPowers powers=(GroupPowers)mod.GetValue(iter,2);
                //power should be singular
                List<KeyValuePair<UUID,UUID>> rolesmembers;
               
                  if (!MainClass.client.Groups.GroupRolesMembersCaches.TryGetValue(request_roles_members, out rolesmembers))
                    return;

					this.store_members_with_ability.Clear();
						Dictionary <UUID, GroupRole> grouproles;
						MainClass.client.Groups.GroupRolesCaches.TryGetValue(request_roles,out grouproles);

			    foreach(KeyValuePair<UUID,UUID> rolesmember in rolesmembers)
			    {
					 // rolesmember.Value is the user UUID
				// .key is the group role UUIS
				UUID user=rolesmember.Value;
					UUID rolekey=rolesmember.Key;
					GroupRole role;
                    if (!grouproles.TryGetValue(rolekey, out role))
                        continue;
				if((role.Powers & powers) == powers)
                     {
                          string name="";

                          if (!MainClass.name_cache.av_names.TryGetValue(user, out name))
                              continue;
                          this.store_members_with_ability.AppendValues(name);
                     }
                }
				
				this.store_roles_with_ability.Clear();
                foreach(KeyValuePair<UUID, GroupRole> role in grouproles)
                {
                  
       				if((role.Value.Powers & powers) == powers)
                     {
                       
                          this.store_roles_with_ability.AppendValues(role.Value.Name);
                     }


                }
                

            }
        }
	}
}
