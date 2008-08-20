// GroupInfo.cs created with MonoDevelop
// User: robin at 17:59 18/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;
using libsecondlife;
using Gtk;

namespace omvviewerlight
{
	
	
	public partial class GroupInfo : Gtk.Window
	{
		Gtk.ListStore store_members;		
		LLUUID groupkey;
		LLUUID founder_key;
		
		public GroupInfo(Group group) : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();

			groupkey=group.ID;
			
			store_members = new Gtk.ListStore (typeof(string),typeof(string),typeof(string),typeof(LLUUID));			
		
			treeview_members.AppendColumn("Member name",new CellRendererText(),"text",0);
			treeview_members.AppendColumn("Title",new CellRendererText(),"text",1);
			treeview_members.AppendColumn("Last login",new CellRendererText(),"text",2);
		
			MainClass.client.Avatars.OnAvatarNames += new libsecondlife.AvatarManager.AvatarNamesCallback(onAvatarNames);			
				
			treeview_members.Model=store_members;
				
			MainClass.client.Groups.OnGroupProfile += new libsecondlife.GroupManager.GroupProfileCallback(onGroupProfile);
			MainClass.client.Groups.OnGroupMembers += new libsecondlife.GroupManager.GroupMembersCallback(onGroupMembers);
			MainClass.client.Groups.OnGroupTitles += new libsecondlife.GroupManager.GroupTitlesCallback(onGroupTitles);
			
			MainClass.client.Groups.RequestGroupProfile(group.ID);
			MainClass.client.Groups.RequestGroupMembers(group.ID);
			MainClass.client.Groups.RequestGroupTitles(group.ID);
			MainClass.client.Groups.RequestGroupRoles(group.ID);
			
			TryGetImage img=new TryGetImage(this.image_group_emblem,group.InsigniaID);
			this.label_name.Text=group.Name;
		
			founder_key=group.FounderID;
			string name;
			if(MainClass.av_names.TryGetValue(group.FounderID,out name))
			{
				this.label_name.Text="Founded by "+MainClass.av_names[group.FounderID];
			}
			
		}
		
		void onGroupTitles(Dictionary <LLUUID,libsecondlife.GroupTitle> titles)
		{
			Gtk.Application.Invoke(delegate {	

			foreach(KeyValuePair  <LLUUID,libsecondlife.GroupTitle> title in titles)
			{
				this.combobox_active_title.InsertText(0,title.Value.Title);
				Console.Write("Title : "+title.Value.Title+" : "+title.Value.Selected.ToString()+"\n");
				if(title.Value.Selected)
				{
					//this.combobox_active_title.ActiveText=title.Value.Title;
						this.combobox_active_title.Active=0;
				}
			}	
		
			});
		}
		
		void onGroupMembers(Dictionary <LLUUID,GroupMember> members)		
		{
			lock(store_members)
			{

			foreach(KeyValuePair <LLUUID,GroupMember> member in members)
			{
				string name;
				if(!MainClass.av_names.TryGetValue(member.Value.ID,out name))
				{
					name="Waiting...";
					MainClass.client.Avatars.RequestAvatarName(member.Value.ID);
				}
					
				Gtk.Application.Invoke(delegate {	
						this.store_members.AppendValues(name,member.Value.Title,member.Value.OnlineStatus,member.Value.ID);
				});
			}
			}
		}
		
		void onGroupProfile(GroupProfile group)
		{
			
			if(group.ID!=this.groupkey)
				return;
			
			Gtk.Application.Invoke(delegate {	
			
			this.textview_group_charter.Buffer.Text=group.Charter;
			this.checkbutton_group_notices.Active=group.AcceptNotices;
			this.checkbutton_openenrolement.Active=group.OpenEnrollment;
			this.entry_enrollmentfee.Text=group.MembershipFee.ToString();
			this.checkbutton_showinpofile.Active=group.ShowInList;
			this.checkbutton_showinsearch.Active=group.AllowPublish;

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
				lock(store_members){					
					this.store_members.Foreach(myfunc_members);
			}
		});
		
		}

		bool myfunc_members(Gtk.TreeModel mod, Gtk.TreePath path, Gtk.TreeIter iter)
		{			
			bool stillwaiting;
			string name=(string)store_members.GetValue(iter,0);
			LLUUID id =(LLUUID)store_members.GetValue(iter,3);
			string member_name;
			if(MainClass.av_names.TryGetValue(id,out member_name))
			{
				store_members.SetValue(iter,0,member_name);
			}
				return false;
		}
		
		
	}
}
