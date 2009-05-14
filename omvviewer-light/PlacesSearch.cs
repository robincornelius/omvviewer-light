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

// PlacesSearch.cs created with MonoDevelop
// User: robin at 17:55 13/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;
using OpenMetaverse;

namespace omvviewerlight
{
	
	[System.ComponentModel.ToolboxItem(true)]	
	public partial class PlacesSearch : Gtk.Bin
	{
	
		UUID queryid;
		Gtk.ListStore store;
        int places_found;
		TryGetImage getter;
		UUID current_image=UUID.Zero;
		
		public PlacesSearch()
		{
			this.Build();
			store= new Gtk.ListStore (typeof(string),typeof(string),typeof(string),typeof(string),typeof(Vector3),typeof(UUID));
			
            MyTreeViewColumn mycol;
            mycol = new MyTreeViewColumn("Name", new Gtk.CellRendererText(), "text", 0,true);
            mycol.setmodel(store);
            treeview1.AppendColumn(mycol);

            mycol = new MyTreeViewColumn("Sim", new Gtk.CellRendererText(), "text", 1,false);
            mycol.setmodel(store);
            treeview1.AppendColumn(mycol);

            mycol = new MyTreeViewColumn("Traffic", new Gtk.CellRendererText(), "text", 2,false);
            mycol.setmodel(store);
            treeview1.AppendColumn(mycol);
			store.SetSortFunc(2,numericsort);
			
            mycol = new MyTreeViewColumn("Location", new Gtk.CellRendererText(), "text", 3,false);
            mycol.setmodel(store);
            treeview1.AppendColumn(mycol);

            treeview1.HeadersClickable = true;
			treeview1.Model=store;
            store.SetSortColumnId(0, Gtk.SortType.Ascending);

			MainClass.client.Directory.OnPlacesReply += new OpenMetaverse.DirectoryManager.PlacesReplyCallback(onPlaces);
		}
		
		new public void Dispose()
		{
			
			MainClass.client.Directory.OnPlacesReply -= new OpenMetaverse.DirectoryManager.PlacesReplyCallback(onPlaces);
			//Finalize();
			//System.GC.SuppressFinalize(this);
		}
		

		int numericsort(Gtk.TreeModel model, Gtk.TreeIter a, Gtk.TreeIter b)
		{

            float Pa;
            float.TryParse((string)store.GetValue(a, 2), out Pa);
            float Pb;
            float.TryParse((string)store.GetValue(b, 2), out Pb);

			int tSortColumnId;
            Gtk.SortType order;

            store.GetSortColumnId(out tSortColumnId, out order);
	
            if (Pa == Pb)
               return 0;
		
            if(order==Gtk.SortType.Ascending)
            {
                if (Pa > Pb)
                   return -1;
                else
                    return 1;
            }
            else
            {
                 if (Pa > Pb)
                   return 1;
                 else
                    return -1;
            }
        }
				
			void onPlaces(UUID query,List <OpenMetaverse.DirectoryManager.PlacesSearchData> matchedplaces)
			{
				if(query!=queryid)
					return;

			
			
                places_found += matchedplaces.Count;
				Gtk.Application.Invoke(delegate {

                    this.label_info.Text = "Search returned " + places_found.ToString() + " results";
	    

				foreach(OpenMetaverse.DirectoryManager.PlacesSearchData place in matchedplaces)
				{	
					Vector3 pos=new Vector3(((int)place.GlobalX)&0x0000FF,((int)place.GlobalY)&0x0000FF,place.GlobalZ);
					store.AppendValues(place.Name,place.SimName,place.Dwell.ToString(),MainClass.prettyvector(pos,2),pos,place.SnapshotID);
				}
			
			 });
			}
				
		protected virtual void OnButtonSearchClicked (object sender, System.EventArgs e)
		{
			store.Clear();
			this.label_info.Text="Searching..........";
            places_found = 0;
			
			OpenMetaverse.DirectoryManager.DirFindFlags flags;
			flags=OpenMetaverse.DirectoryManager.DirFindFlags.NameSort;
			
			if(this.checkbutton_mature.Active==false)
			flags|=OpenMetaverse.DirectoryManager.DirFindFlags.PgSimsOnly;
			
			OpenMetaverse.ParcelCategory pcat;
            pcat = OpenMetaverse.ParcelCategory.Any;
			queryid=UUID.Random();
			MainClass.client.Directory.StartPlacesSearch(flags,pcat,entry1.Text,"",MainClass.client.Self.ActiveGroup,queryid);
		}

		protected virtual void OnButtonTPClicked (object sender, System.EventArgs e)
		{
			
			Vector3 pos=new Vector3();
			string sim;
			
			//beter work out who we have selected
			Gtk.TreeModel mod;
			Gtk.TreeIter iter;
					
			if(treeview1.Selection.GetSelected(out mod,out iter))			
			{
				sim=(string)mod.GetValue(iter,1);
				pos=(Vector3)mod.GetValue(iter,4);
										
				TeleportProgress tp = new TeleportProgress();
				tp.Show();
				tp.teleport(sim,pos);
			}
		}

		protected virtual void OnTreeview1CursorChanged (object sender, System.EventArgs e)
		{
			
			Gtk.TreeModel mod;
			Gtk.TreeIter iter;
			
			if(this.treeview1.Selection.GetSelected(out mod,out iter))			
			{
				UUID id=(UUID)mod.GetValue(iter,5);

				this.current_image=id;
				
				if(getter!=null)
					getter.abort();
							
				TryGetImage i = new TryGetImage(this.image_parcel,id,175,175,false);
				getter=i;
				
			}
		}

		protected virtual void OnEventbox3ButtonPressEvent (object o, Gtk.ButtonPressEventArgs args)
		{
			TexturePreview tp= new TexturePreview(this.current_image,"Location snapsnot",false);
			tp.ShowAll();
		}				
	}
}
