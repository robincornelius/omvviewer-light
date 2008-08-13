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
			store= new Gtk.ListStore (typeof(string),typeof(string),typeof(string),typeof(string),typeof(string),typeof(string));
			
			treeview1.AppendColumn("Name",new Gtk.CellRendererText(),"text",0);
			treeview1.AppendColumn("Sim",new Gtk.CellRendererText(),"text",1);		
			treeview1.AppendColumn("Trafic",new Gtk.CellRendererText(),"text",2);		
			treeview1.AppendColumn("X",new Gtk.CellRendererText(),"text",2);
			treeview1.AppendColumn("Y",new Gtk.CellRendererText(),"text",3);
			treeview1.AppendColumn("Z",new Gtk.CellRendererText(),"text",4);
			
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
						store.AppendValues(place.Name,place.SimName,place.Dwell.ToString(),place.GlobalX.ToString(),place.GlobalY.ToString(),place.GlobalZ.ToString());
					}
					
			 });
			}
				
		protected virtual void OnButtonSearchClicked (object sender, System.EventArgs e)
		{
			libsecondlife.DirectoryManager.DirFindFlags flags;
			flags=libsecondlife.DirectoryManager.DirFindFlags.NameSort;
			libsecondlife.Parcel.ParcelCategory pcat;
			pcat=libsecondlife.Parcel.ParcelCategory.Any;
			queryid=LLUUID.Random();
			MainClass.client.Directory.StartPlacesSearch(flags,pcat,entry1.Text,"",MainClass.client.Self.ActiveGroup,queryid);
		}
	}
}
