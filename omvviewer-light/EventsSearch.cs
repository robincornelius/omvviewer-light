/*
omvviewerlight a Text based client to metaverses such as Linden Labs Secondlife(tm)
    Copyright (C) 2008  Robin Cornelius <robin.cornelius@gmail.com>

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
		OpenMetaverse.DirectoryManager.EventInfo selected_event;
		
		public EventsSearch()
		
	{
			this.Build();
			
			store= new Gtk.ListStore (typeof(string),typeof(string),typeof(OpenMetaverse.DirectoryManager.EventsSearchData));
			
			treeview1.AppendColumn("Event Name",new Gtk.CellRendererText(),"text",0);
			treeview1.AppendColumn("Time",new Gtk.CellRendererText(),"text",1);
		
			store.SetSortColumnId(0,Gtk.SortType.Ascending);
			
			treeview1.Model=store;
			
			MainClass.client.Directory.OnEventsReply += new OpenMetaverse.DirectoryManager.EventReplyCallback(onEvents);
			MainClass.client.Directory.OnEventInfo += new OpenMetaverse.DirectoryManager.EventInfoCallback(onEventInfo);
			this.combobox_category.InsertText(0,OpenMetaverse.DirectoryManager.EventCategories.Sports.ToString());	
			this.combobox_category.InsertText(0,OpenMetaverse.DirectoryManager.EventCategories.Pageants.ToString());
			this.combobox_category.InsertText(0,OpenMetaverse.DirectoryManager.EventCategories.Nightlife.ToString());
			this.combobox_category.InsertText(0,OpenMetaverse.DirectoryManager.EventCategories.Miscellaneous.ToString());
			this.combobox_category.InsertText(0,OpenMetaverse.DirectoryManager.EventCategories.LiveMusic.ToString());
			this.combobox_category.InsertText(0,OpenMetaverse.DirectoryManager.EventCategories.Games.ToString());
			this.combobox_category.InsertText(0,OpenMetaverse.DirectoryManager.EventCategories.Education.ToString());
			this.combobox_category.InsertText(0,OpenMetaverse.DirectoryManager.EventCategories.Discussion.ToString());
			this.combobox_category.InsertText(0,OpenMetaverse.DirectoryManager.EventCategories.Commercial.ToString());
			this.combobox_category.InsertText(0,OpenMetaverse.DirectoryManager.EventCategories.Charity.ToString());
			this.combobox_category.InsertText(0,OpenMetaverse.DirectoryManager.EventCategories.Arts.ToString());
			this.combobox_category.InsertText(0,OpenMetaverse.DirectoryManager.EventCategories.All.ToString());

			this.combobox_category.Active=0;
			this.button_notify.Sensitive=false;
			this.button_teleport.Sensitive=false;
			
		}

		new public void Dispose()
		{
			
			MainClass.client.Directory.OnEventsReply -= new OpenMetaverse.DirectoryManager.EventReplyCallback(onEvents);
			MainClass.client.Directory.OnEventInfo -= new OpenMetaverse.DirectoryManager.EventInfoCallback(onEventInfo);
			
			//Finalize();
			//System.GC.SuppressFinalize(this);
		}
		
		void onEventInfo(OpenMetaverse.DirectoryManager.EventInfo anevent)
	    {
				Gtk.Application.Invoke(delegate {
				this.entry_name.Text=anevent.Name;
				this.entry_date.Text=anevent.Date.ToString();
				this.entry_location.Text=anevent.SimName;
				this.entry_cat.Text=anevent.Category.ToString();
				
				AsyncNameUpdate ud=new AsyncNameUpdate(anevent.Creator,false);  
				ud.onNameCallBack += delegate(string namex,object[] values){this.entry_organiser.Text=namex;};
                ud.go();	
				this.entry_duration.Text=anevent.Duration.ToString();
				this.textview_eventinfo.Buffer.Text=anevent.Desc;
                selected_event=anevent;
			    this.button_notify.Sensitive=false;
			    this.button_teleport.Sensitive=true;
				
			});
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
					store.AppendValues(anevent.Name,anevent.Date.ToString(),anevent);
				}
			
			 });
			}

        protected virtual void OnButtonSearchClicked (object sender, System.EventArgs e)
        {
			
			store.Clear();
			this.label_info.Text="Searching..........";
            events_found = 0;
			this.button_notify.Sensitive=false;
			this.button_teleport.Sensitive=false;
						
			//OpenMetaverse.Parcel.ParcelCategory pcat;
			//pcat=OpenMetaverse.Parcel.ParcelCategory.Any;
			queryid=UUID.Random();
			OpenMetaverse.DirectoryManager.EventCategories selectcat;
            selectcat=(OpenMetaverse.DirectoryManager.EventCategories)Enum.Parse(typeof(OpenMetaverse.DirectoryManager.EventCategories),this.combobox_category.ActiveText);		
		    MainClass.client.Directory.StartEventsSearch(entry_name.Text,this.checkbutton_mature.Active,"",0,selectcat,queryid);			
			
		}

        protected virtual void OnTreeview1CursorChanged (object sender, System.EventArgs e)
        {
			Gtk.TreeModel mod;
			Gtk.TreeIter iter;
			
			if(treeview1.Selection.GetSelected(out mod,out iter))			
			{
				OpenMetaverse.DirectoryManager.EventsSearchData anevent;
				anevent=(OpenMetaverse.DirectoryManager.EventsSearchData)mod.GetValue(iter,2);
				MainClass.client.Directory.EventInfoRequest(anevent.ID);
			}
        }

        protected virtual void OnButtonTeleportClicked (object sender, System.EventArgs e)
		{
			string sim=selected_event.SimName;
			Vector3d pos=selected_event.GlobalPos;
            float local_x,local_y;
			OpenMetaverse.Helpers.GlobalPosToRegionHandle((float)pos.X,(float)pos.Y,out local_x,out local_y);
			Vector3 local_pos;
			local_pos.X=local_x;
			local_pos.Y=local_y;
            local_pos.Z=(float)pos.Z;			

		    TeleportProgress tp = new TeleportProgress();
			tp.Show();
			tp.teleport(sim,local_pos);

        }

        protected virtual void OnButtonNotifyClicked (object sender, System.EventArgs e)
		{

        }
	}
}
