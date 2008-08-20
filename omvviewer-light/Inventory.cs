/*
omvviewer-light a Text based client to metaverses such as Linden Labs Secondlife(tm)
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

// Inventory.cs created with MonoDevelop
// User: robin at 20:08 19/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Threading;
using libsecondlife;
using System.Collections.Generic;

namespace omvviewerlight
{

	public partial class Inventory : Gtk.Bin
	{
		String[] SearchFolders = { "" };
		//initialize our list to store the folder contents
        LLUUID inventoryItems;
		Gtk.TreeStore inventory = new Gtk.TreeStore (typeof (string), typeof (LLUUID));		
		
		public Inventory()
		{
			this.Build();
	
			treeview_inv.AppendColumn("Name",new  Gtk.CellRendererText(),"text",0);
			treeview_inv.Model=inventory;

		}

		protected virtual void OnButtonGetinvClicked (object sender, System.EventArgs e)
		{
            inventory.Clear();
            Thread InvRunner = new Thread(new ThreadStart(this.gogetinv));
            InvRunner.Start();
		}

        void gogetinv()
        {
           

            Gtk.Application.Invoke(delegate
            {
                    Gtk.TreeIter iter = inventory.AppendValues("My Crap", MainClass.client.Inventory.Store.RootFolder.UUID);
          
                
                    recurseinv(MainClass.client.Inventory.Store.RootFolder.UUID, iter);
             
       
                iter = inventory.AppendValues("Derfault crap", MainClass.client.Inventory.Store.LibraryFolder.UUID);
                recurseinv(MainClass.client.Inventory.Store.LibraryFolder.UUID, iter);
            });

            


        }
		
		void recurseinv(LLUUID target,Gtk.TreeIter iter)
		{
            MainClass.client.Inventory.RequestFolderContents(target, MainClass.client.Self.AgentID, true, true, InventorySortOrder.ByDate);
		    List<InventoryBase> myObjects  =MainClass.client.Inventory.FolderContents(target,MainClass.client.Self.AgentID,true,true,InventorySortOrder.ByDate,30000);

            if (myObjects == null)
                return;

			foreach (InventoryBase item in myObjects)
            {
               // Gtk.Application.Invoke(delegate
              //  {
                    Gtk.TreeIter iter2 = inventory.AppendValues(iter, item.Name, item.UUID);
               // });
                recurseinv(item.UUID,iter2);
			}				
		}
		
	}
}
