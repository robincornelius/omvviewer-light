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

// Groups.cs created with MonoDevelop
// User: robin at 16:20 16/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;
using OpenMetaverse;
using Gtk;

namespace omvviewerlight
{
	public partial class Groups : Gtk.Bin
	{
	
		Gtk.ListStore store;
		List<Group> groups_recieved=new List<Group>();
		Gtk.TreeIter active_group_iter;

        public void kill()
        {
            MainClass.client.Groups.OnCurrentGroups -= new OpenMetaverse.GroupManager.CurrentGroupsCallback(onGroups);
	        Gtk.Notebook p;
            p = (Gtk.Notebook)this.Parent;
            p.RemovePage(p.PageNum(this));
        }

		public Groups()
		{
   
			this.Build();
			store= new Gtk.ListStore (typeof(string),typeof(Group),typeof(bool));			
			//treeview1.AppendColumn("Group",new CellRendererText(),"text",0);
               
            Gtk.TreeViewColumn groupColumn = new Gtk.TreeViewColumn();
            groupColumn.Title = "Group";
            Gtk.CellRendererText groupNameCell = new Gtk.CellRendererText();
            groupColumn.PackStart(groupNameCell, true);

            groupColumn.SetCellDataFunc(groupNameCell, new Gtk.TreeCellDataFunc(RenderGroupName));
            treeview1.AppendColumn(groupColumn);
		
            treeview1.Model=store;
	
			//REFACTOR ME, MAINCLASS IS DUPLICATING
			MainClass.client.Groups.OnCurrentGroups += new OpenMetaverse.GroupManager.CurrentGroupsCallback(onGroups);
			MainClass.client.Groups.OnGroupJoined += new OpenMetaverse.GroupManager.GroupJoinedCallback(onGroupJoined);
    		MainClass.client.Groups.OnGroupLeft += new OpenMetaverse.GroupManager.GroupLeftCallback(onGroupLeft);

            if (MainClass.client != null)
            {
                if (MainClass.client.Network.LoginStatusCode == OpenMetaverse.LoginStatus.Success)
                {
                    MainClass.client.Groups.RequestCurrentGroups();
               }
            }	
		}

		void onGroupJoined(UUID group,bool success)
		{
			Gtk.Application.Invoke(delegate{				
				if(success==true)
				{
					store.Clear();
					this.groups_recieved.Clear();
					MainClass.client.Groups.RequestCurrentGroups();
				}			
			});			
		}
		void onGroupLeft(UUID group,bool success)
		{
			Gtk.Application.Invoke(delegate{				
				if(success==true)
				{
					store.Clear();
					this.groups_recieved.Clear();
					MainClass.client.Groups.RequestCurrentGroups();
				}			
			});			
		}

		
        private void RenderGroupName(Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
        {
            bool active = (bool)model.GetValue(iter, 2);
            string text = (string)model.GetValue(iter, 0);

            if (active == true)
            {
                (cell as Gtk.CellRendererText).Foreground = "red";
            }
            else
            {
                (cell as Gtk.CellRendererText).Foreground = "black";
            }

            (cell as Gtk.CellRendererText).Text = text;

        }		void onGroups(Dictionary<UUID,Group> groups)
		{
			
			Gtk.Application.Invoke(delegate {
				
				foreach(KeyValuePair <UUID,Group> group in groups)
				{
					if(!this.groups_recieved.Contains(group.Value))
					{
                        bool active = false;
						if(MainClass.client.Self.ActiveGroup==group.Value.ID)
						{
                            active = true;
                        }
                        Gtk.TreeIter iter=store.AppendValues(group.Value.Name, group.Value,active);
						if(active)
						    active_group_iter=iter;
						
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

		protected virtual void OnButtonInfoClicked (object sender, System.EventArgs e)
		{
		    Gtk.TreeModel mod;
			Gtk.TreeIter iter;
			
			if(treeview1.Selection.GetSelected(out mod,out iter))			
			{
				Group group=(Group)mod.GetValue(iter,1);
				GroupInfo info=new GroupInfo(group.ID,true);
				info.Show();
			}
		}

		
		protected virtual void OnActivateGroupClicked (object sender, System.EventArgs e)
		{
		    Gtk.TreeModel mod;
			Gtk.TreeIter iter;
			
			if(treeview1.Selection.GetSelected(out mod,out iter))			
			{
				Group group=(Group)mod.GetValue(iter,1);
				MainClass.client.Groups.ActivateGroup(group.ID);
				mod.SetValue(iter,2,true);
				mod.SetValue(active_group_iter,2,false);
				active_group_iter=iter;
                treeview1.QueueDraw();
			}		

		}
	}
}
