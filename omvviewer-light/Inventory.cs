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
using Gdk;
using Gtk;

namespace omvviewerlight
{

	public partial class Inventory : Gtk.Bin
	{
		String[] SearchFolders = { "" };
		//initialize our list to store the folder contents
        LLUUID inventoryItems;
		Gtk.TreeStore inventory = new Gtk.TreeStore (typeof(Gdk.Pixbuf),typeof (string), typeof (LLUUID));		
		Gdk.Pixbuf folder_closed = new Gdk.Pixbuf("inv_folder_plain_closed.tga");
		Gdk.Pixbuf folder_open = new Gdk.Pixbuf("inv_folder_plain_open.tga");
		Gdk.Pixbuf item_landmark = new Gdk.Pixbuf("inv_item_landmark.tga");
		Gdk.Pixbuf item_animation = new Gdk.Pixbuf("inv_item_animation.tga");
		Gdk.Pixbuf item_clothing = new Gdk.Pixbuf("inv_item_clothing.tga");
		
		public Inventory()
		{
			this.Build();		
			treeview_inv.AppendColumn("",new CellRendererPixbuf(),"pixbuf",0);
			treeview_inv.AppendColumn("Name",new  Gtk.CellRendererText(),"text",1);
			treeview_inv.Model=inventory;
			this.treeview_inv.RowExpanded += new Gtk.RowExpandedHandler(onRowExpanded);
			this.treeview_inv.RowCollapsed += new Gtk.RowCollapsedHandler(onRowCollapsed);
			MainClass.client.Network.OnLogin += new libsecondlife.NetworkManager.LoginCallback(onLogin);		
				
		}
	
		void onLogin(LoginStatus status,string message)
		{
			if(LoginStatus.Success==status)
			{
				Gtk.Application.Invoke(delegate {
					inventory.Clear();
					Gtk.TreeIter iter = inventory.AppendValues(folder_closed,"Inventory", MainClass.client.Inventory.Store.RootFolder.UUID);
					inventory.AppendValues(iter,folder_closed, "Waiting...", MainClass.client.Inventory.Store.RootFolder.UUID);		
				});
			}
		}
				
		void onRowCollapsed(object o,Gtk.RowCollapsedArgs args)
		{
			LLUUID key=(LLUUID)this.inventory.GetValue(args.Iter,2);
			inventory.SetValue(args.Iter,0,folder_closed);
		}

		void onRowExpanded(object o,Gtk.RowExpandedArgs args)
		{
			LLUUID key=(LLUUID)this.inventory.GetValue(args.Iter,2);
			inventory.SetValue(args.Iter,0,folder_open);
			Console.Write("Expanding to id :"+key.ToString());
			MainClass.client.Inventory.RequestFolderContents(key, MainClass.client.Self.AgentID, true, true, InventorySortOrder.ByDate);
			List<InventoryBase> myObjects  =MainClass.client.Inventory.FolderContents(key,MainClass.client.Self.AgentID,true,true,InventorySortOrder.ByDate,30000);
					
			if (myObjects == null)
				return;

			foreach (InventoryBase item in myObjects)
			{
				Gdk.Pixbuf buf=getprettyicon(item);
				Gtk.TreeIter iter2 = inventory.AppendValues(args.Iter,buf, item.Name, item.UUID);

				if (item is InventoryFolder)
				{
					inventory.AppendValues(iter2, folder_closed,"Waiting...", item.UUID);	
				}
				
			}				
		}
		
		Gdk.Pixbuf getprettyicon(InventoryBase item)
		{
			if (item is InventoryFolder)
				return this.folder_closed;
					
			if(item is libsecondlife.InventoryLandmark)
				return this.item_landmark;
			
			if(item is libsecondlife.InventoryAnimation)
				return this.item_animation;
			
			if(item is libsecondlife.InventoryWearable)
				return this.item_clothing;
			
			return folder_closed;
		}		
	}
}
