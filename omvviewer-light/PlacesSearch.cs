// PlacesSearch.cs created with MonoDevelop
// User: robin at 17:55 13/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;
using libsecondlife;

namespace omvviewerlight
{
	
	public partial class PlacesSearch : Gtk.Bin
	{
	
		LLUUID queryid;
		Gtk.ListStore store;
		
		public PlacesSearch()
		{
			this.Build();
			store= new Gtk.ListStore (typeof(string),typeof(string),typeof(string),typeof(string),typeof(LLVector3));
			
			treeview1.AppendColumn("Name",new Gtk.CellRendererText(),"text",0);
			treeview1.AppendColumn("Sim",new Gtk.CellRendererText(),"text",1);		
			treeview1.AppendColumn("Trafic",new Gtk.CellRendererText(),"text",2);		
			treeview1.AppendColumn("Loc",new Gtk.CellRendererText(),"text",3);
		
			store.SetSortColumnId(3,Gtk.SortType.Ascending);
			store.SetSortColumnId(2,Gtk.SortType.Ascending);
			store.SetSortColumnId(1,Gtk.SortType.Ascending);
			store.SetSortColumnId(0,Gtk.SortType.Ascending);
			
			treeview1.Model=store;
			
			MainClass.client.Directory.OnPlacesReply += new libsecondlife.DirectoryManager.PlacesReplyCallback(onPlaces);
		
			
		}
				
			void onPlaces(LLUUID query,List <libsecondlife.DirectoryManager.PlacesSearchData> matchedplaces)
			{
				Gtk.Application.Invoke(delegate {

				if(query!=queryid)
					return;

				foreach(libsecondlife.DirectoryManager.PlacesSearchData place in matchedplaces)
				{	
					LLVector3 pos=new LLVector3(((int)place.GlobalX)&0x0000FF,((int)place.GlobalY)&0x0000FF,place.GlobalZ);
					store.AppendValues(place.Name,place.SimName,place.Dwell.ToString(),MainClass.prettyvector(pos,2),pos);
				}
			
			 });
			}
				
		protected virtual void OnButtonSearchClicked (object sender, System.EventArgs e)
		{
			store.Clear();
			libsecondlife.DirectoryManager.DirFindFlags flags;
			flags=libsecondlife.DirectoryManager.DirFindFlags.NameSort;
			
			if(this.checkbutton_mature.Active==false)
			flags|=libsecondlife.DirectoryManager.DirFindFlags.PgSimsOnly;
				
			libsecondlife.Parcel.ParcelCategory pcat;
			pcat=libsecondlife.Parcel.ParcelCategory.Any;
			queryid=LLUUID.Random();
			MainClass.client.Directory.StartPlacesSearch(flags,pcat,entry1.Text,"",MainClass.client.Self.ActiveGroup,queryid);
		}

		protected virtual void OnButtonTPClicked (object sender, System.EventArgs e)
		{
			
			LLVector3 pos=new LLVector3();
			string sim;
			
			//beter work out who we have selected
			Gtk.TreeModel mod;
			Gtk.TreeIter iter;
					
			if(treeview1.Selection.GetSelected(out mod,out iter))			
			{
				sim=(string)mod.GetValue(iter,1);
				pos=(LLVector3)mod.GetValue(iter,4);
										
				TeleportProgress tp = new TeleportProgress();
				tp.Show();
				tp.teleport(sim,pos);
			}
		}
				
	}
}
