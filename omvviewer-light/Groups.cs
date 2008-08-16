// Groups.cs created with MonoDevelop
// User: robin at 16:20 16/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;
using libsecondlife;
using Gtk;


namespace omvviewerlight
{
	
	
	public partial class Groups : Gtk.Bin
	{
	
		Gtk.ListStore store;
		List<Group> groups_recieved=new List<Group>();
		
		public Groups()
		{
			this.Build();
			store= new Gtk.ListStore (typeof(string),typeof(Group));			
			treeview1.AppendColumn("Group",new CellRendererText(),"text",0);
			treeview1.Model=store;
	
			MainClass.client.Groups.OnCurrentGroups += new libsecondlife.GroupManager.CurrentGroupsCallback(onGroups);
			MainClass.client.Groups.RequestCurrentGroups();
		}
		
		void onGroups(Dictionary<LLUUID,Group> groups)
		{
			
			Gtk.Application.Invoke(delegate {
				
				foreach(KeyValuePair <LLUUID,Group> group in groups)
				{
					if(!this.groups_recieved.Contains(group.Value))
					{
						string name;
						name=group.Value.Name;
						if(MainClass.client.Self.ActiveGroup==group.Value.ID)
						{
							name="<b>"+name+"<b>";
						}
						store.AppendValues(group.Value.Name,group.Value);
						this.groups_recieved.Add(group.Value);
					}
				}
			});
		}

		protected virtual void OnButtonGroupimClicked (object sender, System.EventArgs e)
		{			
		    Gtk.TreeModel mod;
			Gtk.TreeIter iter;
			
			if(treeview1.Selection.GetSelected(out mod,out iter))			
			{
				Group group=(Group)mod.GetValue(iter,1);
				MainClass.win.startGroupIM(group.ID);
			}
		}
	}
}
