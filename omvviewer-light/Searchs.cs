// Searches.cs created with MonoDevelop
// User: robin at 09:22 13/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;
using libsecondlife;

namespace omvviewerlight
{
	
	public partial class Searches : Gtk.Bin
	{
		
		LLUUID queryid;
		Gtk.ListStore store;
		
		public Searches()
		{
			this.Build();
					
			store= new Gtk.ListStore (typeof(bool),typeof(string),typeof(LLUUID));
			
			treeview1.AppendColumn("Online",new Gtk.CellRendererToggle(),"active",0);
			treeview1.AppendColumn("Name",new Gtk.CellRendererText(),"text",1);		
			store.SetSortColumnId(1,Gtk.SortType.Ascending);
			treeview1.Model=store;
			
			MainClass.client.Directory.OnDirPeopleReply += new libsecondlife.DirectoryManager.DirPeopleReplyCallback(onFindPeople);
		}

		void onFindPeople(LLUUID query,List <libsecondlife.DirectoryManager.AgentSearchData> people)
		{
			Gtk.Application.Invoke(delegate {
			
			if(query!=queryid)
					return;

			foreach(libsecondlife.DirectoryManager.AgentSearchData person in people)
			{
					store.AppendValues (person.Online,person.FirstName+" "+person.LastName,person.AgentID);		
					if(!MainClass.av_names.ContainsKey(person.AgentID))
						MainClass.av_names.Add(person.AgentID,person.FirstName+" "+person.LastName);
			}
		
			});
		}
			                                            	                                         	
		protected virtual void OnButton1Clicked (object sender, System.EventArgs e)
		{
			store.Clear();
			queryid=LLUUID.Random();
			libsecondlife.DirectoryManager.DirFindFlags findFlags;
			findFlags=libsecondlife.DirectoryManager.DirFindFlags.People;
			string searchText;
			searchText=entry1.Text;
			int queryStart=1;

			store.Clear();
			MainClass.client.Directory.StartPeopleSearch(findFlags,searchText,queryStart,queryid);
		}

		protected virtual void OnButton2Clicked (object sender, System.EventArgs e)
		{
						//beter work out who we have selected
			Gtk.TreeModel mod;
			Gtk.TreeIter iter;			
			
			if(treeview1.Selection.GetSelected(out mod,out iter))			
			{
				//ALL i want is a fucking UUID
				LLUUID id=(LLUUID)mod.GetValue(iter,2);
				MainClass.win.startIM(id);
			}

		}

		protected virtual void OnButtonPayClicked (object sender, System.EventArgs e)
		{

			Gtk.TreeModel mod;
			Gtk.TreeIter iter;
			
			if(treeview1.Selection.GetSelected(out mod,out iter))			
			{
				LLUUID id=(LLUUID)mod.GetValue(iter,2);			
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
				LLUUID id=(LLUUID)mod.GetValue(iter,2);			
				ProfileVIew p=new ProfileVIew(id);
				p.Show();
			}				
		}
	}
}
