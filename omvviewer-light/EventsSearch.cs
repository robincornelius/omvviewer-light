// EventsSearch.cs created with MonoDevelop
// User: robin at 15:33 28/09/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using OpenMetaverse;
using System.Collections.Generic;


namespace omvviewerlight
{
	
	
	public partial class EventsSearch : Gtk.Bin
	{
		
		UUID queryid;
		Gtk.ListStore store;
        int events_found;
		
		public EventsSearch()
		{
			this.Build();
			
			store= new Gtk.ListStore (typeof(string),typeof(string));
			
			treeview1.AppendColumn("Evevnt Name",new Gtk.CellRendererText(),"text",0);
			treeview1.AppendColumn("Sim        ",new Gtk.CellRendererText(),"text",1);		
			//treeview1.AppendColumn("Trafic",new Gtk.CellRendererText(),"text",2);		
			//treeview1.AppendColumn("Loc      ",new Gtk.CellRendererText(),"text",3);
		
		//	store.SetSortColumnId(3,Gtk.SortType.Ascending);
		//	store.SetSortColumnId(2,Gtk.SortType.Ascending);
			store.SetSortColumnId(1,Gtk.SortType.Ascending);
			store.SetSortColumnId(0,Gtk.SortType.Ascending);
			
			treeview1.Model=store;
			
			MainClass.client.Directory.OnEventsReply += new OpenMetaverse.DirectoryManager.EventReplyCallback(onEvents);
		}

        void onEvents(UUID query,List <OpenMetaverse.DirectoryManager.EventsSearchData> matchedevents)
		{
				if(query!=queryid)
					return;

                events_found += matchedevents.Count;
			
				Gtk.Application.Invoke(delegate {

                this.label_info.Text = "Search returned " + events_found.ToString() + " results";
	    

				foreach(OpenMetaverse.DirectoryManager.EventsSearchData anevent in matchedevents)
				{	
					//Vector3 pos=new Vector3(((int)anevent.GlobalX)&0x0000FF,((int)anevent.GlobalY)&0x0000FF,anevent.GlobalZ);

					store.AppendValues(anevent.Name,anevent);
				}
			
			 });
			}

        protected virtual void OnButtonSearchClicked (object sender, System.EventArgs e)
        {
			
			store.Clear();
			this.label_info.Text="Searching..........";
            events_found = 0;
			
	//		OpenMetaverse.DirectoryManager.DirFindFlags flags;
		//	flags=OpenMetaverse.DirectoryManager.DirFindFlags.NameSort;
			
		//	if(this.checkbutton_mature.Active==false)
		//	flags|=OpenMetaverse.DirectoryManager.DirFindFlags.PgSimsOnly;
				
		//	EventCategories	cats;
		//	cats=EventCateogries.
				
			
			OpenMetaverse.Parcel.ParcelCategory pcat;
			pcat=OpenMetaverse.Parcel.ParcelCategory.Any;
			queryid=UUID.Random();
			MainClass.client.Directory.StartEventsSearch(entry1.Text,true,OpenMetaverse.DirectoryManager.EventCategories.All);
			
        }
	
		
		
		
		
	}
}
