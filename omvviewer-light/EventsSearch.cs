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
			
			store= new Gtk.ListStore (typeof(string));
			
			treeview1.AppendColumn("Evevnt Name",new Gtk.CellRendererText(),"text",0);
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
					store.AppendValues(anevent.Name,anevent);
				}
			
			 });
			}

        protected virtual void OnButtonSearchClicked (object sender, System.EventArgs e)
        {
			
			store.Clear();
			this.label_info.Text="Searching..........";
            events_found = 0;
			
			//OpenMetaverse.Parcel.ParcelCategory pcat;
			//pcat=OpenMetaverse.Parcel.ParcelCategory.Any;
			queryid=UUID.Random();
			MainClass.client.Directory.StartEventsSearch(entry1.Text,true,"",0,OpenMetaverse.DirectoryManager.EventCategories.All,queryid);			
        }
	
		
		
		
		
	}
}
