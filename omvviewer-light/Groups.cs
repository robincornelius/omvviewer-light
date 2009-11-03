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
	[System.ComponentModel.ToolboxItem(true)]
	public partial class Groups : Gtk.Bin
	{
	
		Gtk.ListStore store;
		Dictionary <UUID,Group> groups_recieved=new	Dictionary <UUID,Group>();
		Gtk.TreeIter active_group_iter;

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
		
            MainClass.onRegister += new MainClass.register(MainClass_onRegister);
            MainClass.onDeregister += new MainClass.deregister(MainClass_onDeregister);
            if(MainClass.client != null ) { MainClass_onRegister(); }

	
            if (MainClass.client != null)
            {
                if (MainClass.client.Network.LoginStatusCode == OpenMetaverse.LoginStatus.Success)
                {
                    MainClass.client.Groups.RequestCurrentGroups();
               }
            }	
		}

        void MainClass_onDeregister()
        {
            //REFACTOR ME, MAINCLASS IS DUPLICATING
            if (MainClass.client != null)
            {
                MainClass.client.Groups.CurrentGroups -= new EventHandler<CurrentGroupsEventArgs>(Groups_CurrentGroups);
                MainClass.client.Groups.GroupJoinedReply -= new EventHandler<GroupOperationEventArgs>(Groups_GroupJoinedReply);
                MainClass.client.Groups.GroupLeaveReply -= new EventHandler<GroupOperationEventArgs>(Groups_GroupLeaveReply);
            }
        }

        void MainClass_onRegister()
        {
            //REFACTOR ME, MAINCLASS IS DUPLICATING
            MainClass.client.Groups.CurrentGroups += new EventHandler<CurrentGroupsEventArgs>(Groups_CurrentGroups);
            MainClass.client.Groups.GroupJoinedReply += new EventHandler<GroupOperationEventArgs>(Groups_GroupJoinedReply);
            MainClass.client.Groups.GroupLeaveReply += new EventHandler<GroupOperationEventArgs>(Groups_GroupLeaveReply);
        }


        new public void Dispose()
        {
            MainClass.onRegister -= new MainClass.register(MainClass_onRegister);
            MainClass.onDeregister -= new MainClass.deregister(MainClass_onDeregister);
            MainClass_onDeregister();

            Gtk.Notebook p;
            p = (Gtk.Notebook)this.Parent;
            p.RemovePage(p.PageNum(this));
        }
		


        void Groups_GroupJoinedReply(object sender, GroupOperationEventArgs e)
		{
			Gtk.Application.Invoke(delegate{				
				if(e.Success==true)
				{
					store.Clear();
					this.groups_recieved.Clear();
					MainClass.client.Groups.RequestCurrentGroups();
				}			
			});			
		}


        
        void Groups_GroupLeaveReply(object sender, GroupOperationEventArgs e)
		{
			Gtk.Application.Invoke(delegate{				
				if(e.Success==true)
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

        }

        void Groups_CurrentGroups(object sender, CurrentGroupsEventArgs e)
        {
			
				Gtk.Application.Invoke(delegate {
			    lock(this.groups_recieved)
                {
				foreach(KeyValuePair <UUID,Group> group in e.Groups)
				{
					if(!this.groups_recieved.ContainsKey(group.Key))
					{
                        bool active = false;
						if(MainClass.client.Self.ActiveGroup==group.Value.ID)
						{
                            active = true;
                        }
                        Gtk.TreeIter iter=store.AppendValues(group.Value.Name, group.Value,active);
						if(active)
						    active_group_iter=iter;
						
						this.groups_recieved.Add(group.Key,group.Value);
				   }
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
