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

// Searches.cs created with MonoDevelop
// User: robin at 09:22 13/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;
using OpenMetaverse;

namespace omvviewerlight
{
	
	[System.ComponentModel.ToolboxItem(true)]	
	public partial class Searches : Gtk.Bin
	{
		
		UUID queryid;
		Gtk.ListStore store;
        int people_found;
		
		public Searches()
		{
			this.Build();
					
			store= new Gtk.ListStore (typeof(bool),typeof(string),typeof(UUID));
			
			treeview1.AppendColumn("Online",new Gtk.CellRendererToggle(),"active",0);
			treeview1.AppendColumn("Name",new Gtk.CellRendererText(),"text",1);		
			store.SetSortColumnId(1,Gtk.SortType.Ascending);
			treeview1.Model=store;
	
            MainClass.onRegister += new MainClass.register(MainClass_onRegister);
            MainClass.onDeregister += new MainClass.deregister(MainClass_onDeregister);
            MainClass_onRegister();
        }

        void MainClass_onDeregister()
        {
            MainClass.client.Directory.OnDirPeopleReply -= new OpenMetaverse.DirectoryManager.DirPeopleReplyCallback(onFindPeople);

        }

        void MainClass_onRegister()
        {
            MainClass.client.Directory.OnDirPeopleReply += new OpenMetaverse.DirectoryManager.DirPeopleReplyCallback(onFindPeople);

        }

        new public void Dispose()
        {
            MainClass.onRegister -= new MainClass.register(MainClass_onRegister);
            MainClass.onDeregister -= new MainClass.deregister(MainClass_onDeregister);
            MainClass_onDeregister();
        }
		
		void onFindPeople(UUID query,List <OpenMetaverse.DirectoryManager.AgentSearchData> people)
		{
			if(query!=queryid)
					return;

            people_found += people.Count;
			Gtk.Application.Invoke(delegate {


			this.label_info.Text="Search returned "+people_found.ToString()+" results";
			
		     if (people.Count == 0)
                return;
		
			foreach(OpenMetaverse.DirectoryManager.AgentSearchData person in people)
			{

                if (person.AgentID == UUID.Zero)
                    continue;
					store.AppendValues (person.Online,person.FirstName+" "+person.LastName,person.AgentID);		
					if(!MainClass.name_cache.av_names.ContainsKey(person.AgentID))
						MainClass.name_cache.av_names.Add(person.AgentID,person.FirstName+" "+person.LastName);
			}
		
			});
		}
			                                            	                                         	
		protected virtual void OnButton1Clicked (object sender, System.EventArgs e)
		{
			store.Clear();
			this.label_info.Text="Searching..........";
            people_found = 0;
		
			queryid=UUID.Random();
            OpenMetaverse.DirectoryManager.DirFindFlags findFlags;
			findFlags=OpenMetaverse.DirectoryManager.DirFindFlags.People;
			string searchText;
			searchText=entry1.Text;
			int queryStart=0;

			store.Clear();
			MainClass.client.Directory.StartPeopleSearch(findFlags,searchText,queryStart,queryid);
		}

		protected virtual void OnButton2Clicked (object sender, System.EventArgs e)
		{
			Gtk.TreeModel mod;
			Gtk.TreeIter iter;			
			
			if(treeview1.Selection.GetSelected(out mod,out iter))			
			{
				UUID id=(UUID)mod.GetValue(iter,2);
				MainClass.win.startIM(id);
			}

		}

		protected virtual void OnButtonPayClicked (object sender, System.EventArgs e)
		{

			Gtk.TreeModel mod;
			Gtk.TreeIter iter;
			
			if(treeview1.Selection.GetSelected(out mod,out iter))			
			{
				UUID id=(UUID)mod.GetValue(iter,2);			
				PayWindow pay=new PayWindow(id,0);
				pay.Show();
			}		
		}

		protected virtual void OnButtonProfileClicked (object sender, System.EventArgs e)
		{
			Gtk.TreeModel mod;
			Gtk.TreeIter iter;
			
			if(treeview1.Selection.GetSelected(out mod,out iter))			
			{
				UUID id=(UUID)mod.GetValue(iter,2);			
				ProfileVIew p=new ProfileVIew(id);
				p.Show();
			}				
		}
	}
}
