// Inventory.cs created with MonoDevelop
// User: robin at 20:08 19/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
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
			Gtk.TreeIter iter = inventory.AppendValues ("My Crap",MainClass.client.Inventory.Store.RootFolder.UUID);
			recurseinv(MainClass.client.Inventory.Store.RootFolder.UUID,iter);

			iter = inventory.AppendValues ("Derfault crap",MainClass.client.Inventory.Store.LibraryFolder.UUID);
			recurseinv(MainClass.client.Inventory.Store.LibraryFolder.UUID,iter);
	
		}
		
		void recurseinv(LLUUID target,Gtk.TreeIter iter)
		{
		    List<InventoryBase> myObjects  =MainClass.client.Inventory.FolderContents(target,MainClass.client.Self.AgentID,true,true,InventorySortOrder.ByDate,30000);

			foreach (InventoryBase item in myObjects)
            {
				Gtk.TreeIter iter2 =inventory.AppendValues (iter, item.Name,item.UUID);
				recurseinv(item.UUID,iter2);
			}				
		}
		
	}
}
