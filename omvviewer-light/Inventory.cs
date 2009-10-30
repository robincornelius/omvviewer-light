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

// Inventory.cs created with MonoDevelop
// User: robin at 20:08 19/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Threading;
using OpenMetaverse;
using OpenMetaverse.Assets;
using System.Collections.Generic;
using Gdk;
using Gtk;
using System.IO;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


namespace omvviewerlight
{
	[System.ComponentModel.ToolboxItem(true)]
    public partial class invthreaddata
    {
		public UUID key;
        public TreeIter iter;
		public string path;
        public bool cacheonly;
        public bool recurse;
		public invthreaddata(UUID keyx, string pathx,TreeIter iterx,bool cache_only,bool recurse_on)
        {
            key = keyx;
            iter = iterx;
			path = pathx;
            cacheonly = cache_only ;
            recurse = recurse_on;
		}
    }
	
	public partial class Inventory : Gtk.Bin
	{ 	
		public int no_items;
        Dictionary<invthreaddata, List<InventoryBase>> incomming = new Dictionary<invthreaddata, List<InventoryBase>>();  
		Dictionary<UUID, Gtk.TreeIter> assetmap = new Dictionary<UUID, Gtk.TreeIter>();
        List<InventoryBase> cutcopylist = new List<InventoryBase>();
        bool iscut = false;
        String[] SearchFolders = { "" };
        UUID trash_folder=UUID.Zero;
		//initialize our list to store the folder contents
		Gtk.TreeStore inventory = new Gtk.TreeStore (typeof(Gdk.Pixbuf),typeof (string), typeof (UUID),typeof(InventoryBase));		
		Gdk.Pixbuf folder_closed = MainClass.GetResource("inv_folder_plain_closed.png");
		Gdk.Pixbuf folder_open = MainClass.GetResource("inv_folder_plain_open.png");
		Gdk.Pixbuf item_landmark = MainClass.GetResource("inv_item_landmark.png");
		Gdk.Pixbuf item_animation = MainClass.GetResource("inv_item_animation.png");
		Gdk.Pixbuf item_clothing = MainClass.GetResource("inv_item_clothing.png");
        Gdk.Pixbuf item_object = MainClass.GetResource("inv_item_object.png");
        Gdk.Pixbuf item_gesture = MainClass.GetResource("inv_item_gesture.png");
        Gdk.Pixbuf item_notecard = MainClass.GetResource("inv_item_notecard.png");
        Gdk.Pixbuf item_script = MainClass.GetResource("inv_item_script.png");
        Gdk.Pixbuf item_snapshot = MainClass.GetResource("inv_item_snapshot.png");
        Gdk.Pixbuf item_texture = MainClass.GetResource("inv_item_texture.png");

        Gdk.Pixbuf item_sound = MainClass.GetResource("inv_item_sound.png");
        Gdk.Pixbuf item_callingcard = MainClass.GetResource("inv_item_callingcard_offline.png");
		
		Gdk.Pixbuf item_clothing_eyes = MainClass.GetResource("inv_item_eyes.png");
		Gdk.Pixbuf item_clothing_gloves = MainClass.GetResource("inv_item_gloves.png");
		Gdk.Pixbuf item_clothing_hair= MainClass.GetResource("inv_item_hair.png");
		Gdk.Pixbuf item_clothing_jacket= MainClass.GetResource("inv_item_jacket.png");
		Gdk.Pixbuf item_clothing_pants= MainClass.GetResource("inv_item_pants.png");
		Gdk.Pixbuf item_clothing_shoes= MainClass.GetResource("inv_item_shoes.png");
		Gdk.Pixbuf item_clothing_skin= MainClass.GetResource("inv_item_skin.png");
		Gdk.Pixbuf item_clothing_skirt= MainClass.GetResource("inv_item_skirt.png");
		Gdk.Pixbuf item_clothing_underpants= MainClass.GetResource("inv_item_underpants.png");
		Gdk.Pixbuf item_clothing_undershirt= MainClass.GetResource("inv_item_undershirt.png");
	
		Gdk.Pixbuf item_clothing_shirt= MainClass.GetResource("inv_item_shirt.png");
		Gdk.Pixbuf item_clothing_socks= MainClass.GetResource("inv_item_socks.png");
		Gdk.Pixbuf item_clothing_shape= MainClass.GetResource("inv_item_shape.png");

        Gdk.Pixbuf folder_texture = MainClass.GetResource("inv_folder_texture.png");
        Gdk.Pixbuf folder_sound = MainClass.GetResource("inv_folder_sound.png");
        Gdk.Pixbuf folder_animation = MainClass.GetResource("inv_folder_animation.png");
        Gdk.Pixbuf folder_gesture = MainClass.GetResource("inv_folder_gesture.png");
        Gdk.Pixbuf folder_snapshot = MainClass.GetResource("inv_folder_snapshot.png");
        Gdk.Pixbuf folder_trash = MainClass.GetResource("inv_folder_trash.png");
        Gdk.Pixbuf folder_notecard = MainClass.GetResource("inv_folder_notecard.png");
        Gdk.Pixbuf folder_script = MainClass.GetResource("inv_folder_script.png");
        Gdk.Pixbuf folder_lostandfound = MainClass.GetResource("inv_folder_lostandfound.png");
        Gdk.Pixbuf folder_landmark = MainClass.GetResource("inv_folder_landmark.png");
        Gdk.Pixbuf folder_bodypart = MainClass.GetResource("inv_folder_bodypart.png");
        Gdk.Pixbuf folder_callingcard = MainClass.GetResource("inv_folder_callingcard.png");
        Gdk.Pixbuf folder_clothing = MainClass.GetResource("inv_folder_clothing.png");
        MyTreeViewColumn col;
        Gtk.CellRendererText renderer;

        bool inventoryloaded = false;

        Gtk.TreeModelFilter filter;

        TreeIter TLI;

        bool filteractive = false;
        bool fetcherrunning = false;
		bool fetchrun=false;
        int recursion = 0;
        bool abortfetch = false;

		private Gtk.TreeIter global_thread_tree;

        List<Gtk.TreeIter> filtered = new List<TreeIter>();

        enum foldersorttype
        {
            SORT_NAME,
            SORT_DATE
        };

        foldersorttype preferedsort = foldersorttype.SORT_DATE;

		~Inventory()
		{
  			Console.WriteLine("Inventory Cleaned up");
		}
	    	
        new public void Dispose()
        {
            Console.WriteLine("Running cleanup code for inventory");

            MainClass.onRegister -= new MainClass.register(MainClass_onRegister);
            MainClass.onDeregister -= new MainClass.deregister(MainClass_onDeregister);
            MainClass_onDeregister();

            this.treeview_inv.RowExpanded -= new Gtk.RowExpandedHandler(onRowExpanded);
            this.treeview_inv.RowCollapsed -= new Gtk.RowCollapsedHandler(onRowCollapsed);
            this.treeview_inv.ButtonPressEvent -= new ButtonPressEventHandler(treeview_inv_ButtonPressEvent);			

            Gtk.Notebook p;
            p = (Gtk.Notebook)this.Parent;
            p.RemovePage(p.PageNum(this));
        }
		

		public Inventory()
		{
			this.Build();		
			
            treeview_inv.AppendColumn("",new CellRendererPixbuf(),"pixbuf",0);
            
            Gtk.CellRendererText item_name = new Gtk.CellRendererText();
            renderer = item_name;
            item_name.Editable = true;
            item_name.Edited += new EditedHandler(item_name_Edited);
          
			col = new MyTreeViewColumn("Name", item_name, "text", 1,true);
            col.setmodel(inventory);

            treeview_inv.InsertColumn(col, 1);
			
            this.treeview_inv.RowExpanded += new Gtk.RowExpandedHandler(onRowExpanded);
			this.treeview_inv.RowCollapsed += new Gtk.RowCollapsedHandler(onRowCollapsed);
            this.treeview_inv.ButtonPressEvent += new ButtonPressEventHandler(treeview_inv_ButtonPressEvent);
			
			this.treeview_inv.Selection.Mode = SelectionMode.Multiple;    

            filter = new Gtk.TreeModelFilter(inventory, null);
            filter.VisibleFunc = new TreeModelFilterVisibleFunc(FilterTree); 
            treeview_inv.Model = filter;
			treeview_inv.HeadersClickable=true;
			
            this.inventory.SetSortFunc(0, sortinventoryfunc);
            this.inventory.SetSortColumnId(0, SortType.Ascending);
			this.inventory.SetSortFunc(1, sortinventoryfunc);
            this.inventory.SetSortColumnId(1, SortType.Ascending);
        
  
            MainClass.onRegister += new MainClass.register(MainClass_onRegister);
            MainClass.onDeregister += new MainClass.deregister(MainClass_onDeregister);
            if(MainClass.client != null ) { MainClass_onRegister(); }


			this.label_aquired.Text="";
			this.label_createdby.Text="";
			this.label_name.Text="";
			this.label_group.Text="";
			this.label_saleprice.Text="";

            if (MainClass.client != null)
            {
                if (MainClass.client.Network.LoginStatusCode == OpenMetaverse.LoginStatus.Success)
                {
                    inventoryloaded = true;
                    inventory.Clear();			
				    populate_top_level_inv();
				}
            }
		}

        void MainClass_onDeregister()
        {
            if (MainClass.client != null)
            {
                MainClass.client.Network.OnLogin -= new OpenMetaverse.NetworkManager.LoginCallback(onLogin);
                MainClass.client.Network.OnLogoutReply -= new NetworkManager.LogoutCallback(Network_OnLogoutReply);
                MainClass.client.Network.OnEventQueueRunning -= new OpenMetaverse.NetworkManager.EventQueueRunningCallback(onEventQueue);
            }
            
            MainWindow.OnInventoryAccepted -= new MainWindow.InventoryAccepted(win_OnInventoryAccepted);
        }

        void MainClass_onRegister()
        {
           
            inventory.Clear();
            inventoryloaded = false;
            assetmap.Clear();

           filteractive = false;
           fetcherrunning = false;
           fetchrun = false;
           recursion = 0;
           abortfetch = false;

            MainClass.client.Network.OnLogin += new OpenMetaverse.NetworkManager.LoginCallback(onLogin);
            MainClass.client.Network.OnLogoutReply += new NetworkManager.LogoutCallback(Network_OnLogoutReply);
            MainClass.client.Network.OnEventQueueRunning += new OpenMetaverse.NetworkManager.EventQueueRunningCallback(onEventQueue);
            MainWindow.OnInventoryAccepted += new MainWindow.InventoryAccepted(win_OnInventoryAccepted);

        }

        void item_name_Edited(object o, EditedArgs args)
        {
            Gtk.TreeModel mod;
            Gtk.TreeIter iter;
            
            TreePath[] paths = treeview_inv.Selection.GetSelectedRows(out mod);
            if (mod.GetIter(out iter, paths[0]))
            {
                InventoryBase item = (InventoryBase)mod.GetValue(iter, 3);
               
                if(item.UUID==MainClass.client.Inventory.Store.RootFolder.UUID || item.UUID==MainClass.client.Inventory.Store.LibraryFolder.UUID)
                {
                    args.RetVal=true;
                    return;
                }
                if(item is InventoryItem)
                    MainClass.client.Inventory.MoveItem(item.UUID,item.ParentUUID,args.NewText);

                if(item is InventoryFolder)
                    MainClass.client.Inventory.MoveFolder(item.UUID, item.ParentUUID, args.NewText);

                inventory.SetValue(filter.ConvertIterToChildIter(iter), 1, args.NewText);
           
                args.RetVal = false;
            }
        }

        void win_OnInventoryAccepted(AssetType type, UUID objectID)
        {
            //We have new inventory given to us and we have accepted it update the view
            UUID folder = MainClass.client.Inventory.FindFolderForType(type);
            Gtk.TreeIter iter;

            if(assetmap.TryGetValue(folder,out iter))
            {
                //request an update of that folder
              //  TreePath path = inventory.GetPath(iter);
              //  Thread invRunner = new Thread(new ParameterizedThreadStart(UpdateRow));
              //  invthreaddata x = new invthreaddata(folder, path.ToString(), iter, false,true);
                //invRunner.Start(x);
            }
        }

        void Network_OnLogoutReply(List<UUID> inventoryItems)
        {
            abortfetch = true;
        }
     
        private bool FilterTree(Gtk.TreeModel model, Gtk.TreeIter iter)
        {

            lock (inventory)
            {

                try
                {
                    if (this.entry_search.Text == "")
                        return true;

                    if (filtered.Contains(iter))//*sigh*
                        return true;

                    object obj = model.GetValue(iter, 1);
                    if (obj == null)
                        return false;

                    string Name = (string)obj;

                    if (Name.Contains(this.entry_search.Text))
                    {
                        filtered.Add(iter);//*sigh*

                        TreePath path = model.GetPath(iter);
                        while (path.Depth > 1)
                        {
                            path.Up();
                            TreeIter iter2;
                            model.GetIter(out iter2, path);
                            filtered.Add(iter2);//*sigh*                    
                        }

                        return true;
                    }

                    return false;
                }
                catch
                {
                    return false;
                }
            }
            
        }
        

        int sortinventoryfunc(Gtk.TreeModel model, Gtk.TreeIter a, Gtk.TreeIter b)
        {
            lock(inventory)
            {
            try
            {
                int colid;
                SortType order;
                int aa = 1;
                int bb = -1;
				
				int aaa=1;
				int bbb=-1;
				
				
               	inventory.GetSortColumnId(out colid, out order);
         		if(order==SortType.Ascending)
				{
					aaa=-1;
					bbb=1;
				}
					
                // We probably want to sort my name or date, and may want to group special folders first
                InventoryBase itema = (InventoryBase)model.GetValue(a, 3);
                InventoryBase itemb = (InventoryBase)model.GetValue(b, 3);

                if (itema == null || itemb == null)
                    return 0;
				
                    if (itema is InventoryFolder && itemb is InventoryFolder)
                    {
						//Two system folders
						if (((InventoryFolder)itema).PreferredType != AssetType.Unknown && ((InventoryFolder)itemb).PreferredType != AssetType.Unknown)
						{
							//do name sort
							int ret = string.Compare(itema.Name, itemb.Name);
	                        if (ret == 1)
	                            return aa;
	                        if (ret == -1)
	                            return bb;
						}
					
						//Two non-system folders
						if (((InventoryFolder)itema).PreferredType == AssetType.Unknown && ((InventoryFolder)itemb).PreferredType == AssetType.Unknown)
						{
							//do name sort
							int ret = string.Compare(itema.Name, itemb.Name);
	                        if (ret == 1)
	                            return aa;
	                        if (ret == -1)
	                            return bb;
						}
					
					    if (this.check_special_folders.Active)
						{
							//Set sepcial folders to top always
	                        if (((InventoryFolder)itema).PreferredType != ((InventoryFolder)itemb).PreferredType)
	                        {
	                            // we are comparing a standard folder to a special folder
	                            if (((InventoryFolder)itema).PreferredType == AssetType.Unknown && ((InventoryFolder)itemb).PreferredType != AssetType.Unknown)
								{
									return bbb;
								}
					    			
	                             return aaa;
							}
						}
						else
						{
							//do name sort
							int ret = string.Compare(itema.Name, itemb.Name);
	                        if (ret == 1)
	                            return aa;
	                        if (ret == -1)
	                            return bb;
						}	
						return 0;		
                     }
				
				
				
                    if (this.radiobutton2.Active) //NAME
                    {
                        int ret = string.Compare(itema.Name, itemb.Name);
                        if (ret == 1)
                            return aa;
                        if (ret == -1)
                            return bb;
                    }

                    if (this.radiobutton1.Active)
                    {
                        if (itema is InventoryItem && itemb is InventoryItem)
                        {
                            if (((InventoryItem)itema).CreationDate > ((InventoryItem)itemb).CreationDate)
                                return aa;
                            return bb;
                        }
                        else
                        {
                            int ret = string.Compare(itema.Name, itemb.Name);
                            if (ret == 1)
                                return aa;
                            if (ret == -1)
                                return bb;
                        }
                    }

                
            }
            catch
            {
            }

            return 0;

            }
        }

		void populate_top_level_inv()
		{
            lock (inventory)
            {
                if (MainClass.client.Inventory.Store.Items != null)
                {
                    foreach (KeyValuePair<UUID, InventoryNode> kvp in MainClass.client.Inventory.Store.Items)
                    {
                        if (kvp.Value.Data != null)
                        {
                            if (kvp.Value.Data.ParentUUID == UUID.Zero)
                            {
                                if (!assetmap.ContainsKey(MainClass.client.Inventory.Store.RootFolder.UUID))
                                {
                                    InventoryFolder fdr = new InventoryFolder(MainClass.client.Inventory.Store.RootFolder.UUID);
                                    fdr.Name = "My Inventory";
                                    Gtk.TreeIter iterx = inventory.AppendValues(folder_closed, kvp.Value.Data.Name, kvp.Value.Data.UUID, fdr);
                                    Console.Write("Creating top level folder " + kvp.Value.Data.Name + " : " + MainClass.client.Inventory.Store.Items[kvp.Value.Data.UUID].ToString());
                                    assetmap.Add(MainClass.client.Inventory.Store.RootFolder.UUID, iterx);
                                    inventory.AppendValues(iterx, folder_closed, "Waiting...", kvp.Value.Data.UUID, null);
                                    if (kvp.Value.Data.Name == "My Inventory")
                                        TLI = iterx;
                                }

                               
                            }
                            Console.Write(kvp.Value.Data.ParentUUID.ToString() + " : ");
                        }
                    }

                    this.no_items = 0;
                    MainClass.client.Inventory.Store.RestoreFromDisk(MainClass.client.Settings.TEXTURE_CACHE_DIR + System.IO.Path.DirectorySeparatorChar + MainClass.client.Inventory.Store.RootFolder.UUID.ToString() + ".osl");

                    fetcherrunning = true;
                    Thread invRunner = new Thread(new ParameterizedThreadStart(fetchinventory));
                    invthreaddata itd = new invthreaddata(MainClass.client.Inventory.Store.RootFolder.UUID, "0", TLI, true, true);
                    invRunner.Start(itd);
                }
            }
		}
		
        void menu_ware_ButtonPressEvent(object o, ButtonPressEventArgs args)
        {
            Gtk.TreeModel mod;
            Gtk.TreeIter iter;
            TreePath[] paths = treeview_inv.Selection.GetSelectedRows(out mod);
            mod.GetIter(out iter, paths[0]);
            InventoryBase item = (InventoryBase)mod.GetValue(iter, 3);

            if (item is InventoryFolder)
            {
                MainClass.client.Appearance.WearOutfit(item.UUID,true);
            }
        }

        void onemptytrash(object o, ButtonPressEventArgs args)
        {
          
            MessageDialog md = new MessageDialog(MainClass.win, DialogFlags.Modal, MessageType.Question, ButtonsType.YesNo, "Are you sure you wish to empty the trash\n All items will be deleted forever");
            ResponseType result = (ResponseType)md.Run();
            md.Destroy();
            if (result == ResponseType.Yes)
            {

                if(trash_folder==UUID.Zero)
                {
                    Logger.Log("ERROR: Don't know where the trash folder is",Helpers.LogLevel.Error);
                    return;
                }

                Gtk.TreeIter trash_iter = assetmap[trash_folder];
                Gtk.TreePath trash_path = inventory.GetPath(trash_iter);
              
                List<InventoryBase> myObjects = new List<InventoryBase>();
                myObjects = MainClass.client.Inventory.Store.GetContents(trash_folder);

              
                foreach (InventoryBase item in myObjects)
                {
                    if (assetmap.ContainsKey(item.UUID))
                    {
                        Gtk.TreeIter rm_iter = assetmap[item.UUID];
                        inventory.Remove(ref rm_iter);
                        assetmap.Remove(item.UUID);
                    }

                }

                MainClass.client.Inventory.EmptyTrash(); 
            }
        }

        void onemptylostandfound(object o, ButtonPressEventArgs args)
        {
           
            MessageDialog md = new MessageDialog(MainClass.win, DialogFlags.Modal, MessageType.Question, ButtonsType.YesNo, "Are you sure you wish to empty the Lost and Found\n All items will be deleted forever");
            ResponseType result = (ResponseType)md.Run();
            md.Destroy();


            if (result == ResponseType.Yes)
            {
                MainClass.client.Inventory.EmptyLostAndFound();
            }
        }
		
		void ondeleteasset(object o, ButtonPressEventArgs args)
		{
            lock (inventory)
            {
                Gtk.TreeModel mod;
                Gtk.TreeIter iter;

                TreePath[] paths = treeview_inv.Selection.GetSelectedRows(out mod);

                Dictionary<UUID, UUID> folders = new Dictionary<UUID, UUID>();
                Dictionary<UUID, UUID> items = new Dictionary<UUID, UUID>();

                Dictionary<UUID, InventoryBase> inv_items = new Dictionary<UUID, InventoryBase>();

                if (trash_folder == UUID.Zero)
                {
                    Logger.Log("ERROR: Don't know where the trash folder is", Helpers.LogLevel.Error);
                    return;
                }
                Gtk.TreeIter trash_iter = assetmap[trash_folder];

                foreach (TreePath path in paths)
                {
                    if (mod.GetIter(out iter, path))
                    {
                        InventoryBase item = (InventoryBase)mod.GetValue(iter, 3);
                        if (item is InventoryItem)
                            items.Add(item.UUID, trash_folder);
                        if (item is InventoryFolder)
                            folders.Add(item.UUID, trash_folder);

                        inv_items.Add(item.UUID, item);
                    }
                }

                MessageDialog md = new MessageDialog(MainClass.win, DialogFlags.Modal, MessageType.Question, ButtonsType.YesNo, "Are you sure you wish to delete selected items and folders\n All children in folders will also be deleted");
                ResponseType result = (ResponseType)md.Run();
                md.Destroy();
                if (result == ResponseType.Yes)
                {
                    if (items.Count > 0)
                    {
                        MainClass.client.Inventory.MoveItems(items);
                    }
                    if (folders.Count > 0)
                    {
                        MainClass.client.Inventory.MoveFolders(folders);
                    }

                    foreach (KeyValuePair<UUID, UUID> kvp in items)
                    {
                        Gtk.TreeIter iterx;
                        iterx = assetmap[kvp.Key];
                        inventory.Remove(ref iterx);
                        if (assetmap.ContainsKey(kvp.Key))
                            assetmap.Remove(kvp.Key);

                        assetmap.Add(kvp.Key, inventory.AppendValues(trash_iter, getprettyicon(inv_items[kvp.Key]), inv_items[kvp.Key].Name, inv_items[kvp.Key].UUID, inv_items[kvp.Key]));
                    }

                    foreach (KeyValuePair<UUID, UUID> kvp in folders)
                    {
                        Gtk.TreeIter iterx;
                        iterx = assetmap[kvp.Key];
                        inventory.Remove(ref iterx);
                        if (assetmap.ContainsKey(kvp.Key))
                            assetmap.Remove(kvp.Key);

                        assetmap.Add(kvp.Key, inventory.AppendValues(trash_iter, getprettyicon(inv_items[kvp.Key]), inv_items[kvp.Key].Name, inv_items[kvp.Key].UUID, inv_items[kvp.Key]));
                        // We need to make sure the view still knows about children of this folder as well or it will show it in trash with none
                        inventory.AppendValues(assetmap[kvp.Key], item_object, "Waiting...", UUID.Zero, null);

                    }
                }
            }
		}
		
		void ongiveasset(object o, ButtonPressEventArgs args)
		{
			 Gtk.TreeModel mod;
			Gtk.TreeIter iter;
            NamePicker np = new NamePicker();
            TreePath[] paths = treeview_inv.Selection.GetSelectedRows(out mod);
            List<InventoryBase> items = new List<InventoryBase>();

            foreach (TreePath path in paths)
            {
                if (mod.GetIter(out iter, path))
                {
                    InventoryBase item = (InventoryBase)mod.GetValue(iter, 3);
                    items.Add(item);
                    if(np.Name=="")
                    {
                        if(item is InventoryItem)
				            np.item_name=((InventoryItem)item).Name;
                    }
                }

            }

            if (paths.Length > 1)
            {
                np.Name += " and " + paths.Length.ToString() + " other items ";
            }
				
	        np.items = items;
			
			np.UserSel += new NamePicker.UserSelected(ongiveasset2);
			np.Show();
			
		}

        void ongiveasset2(UUID id, UUID asset, string item_name, string user_name, List<InventoryBase> items)
		{
			MessageDialog md = new MessageDialog(MainClass.win,DialogFlags.Modal,MessageType.Question,ButtonsType.YesNo,"Are you sure you wish to give\n"+item_name+"to "+user_name);
			ResponseType result=(ResponseType)md.Run();	
			md.Destroy();
			
			if(result==ResponseType.Yes)
			{
                foreach (InventoryBase item in items)
                {
                    if(item is InventoryItem)
                        MainClass.client.Inventory.GiveItem(item.UUID, item.Name,((InventoryItem)item).AssetType, id, false);
                    if(item is InventoryFolder)
                        MainClass.client.Inventory.GiveItem(item.UUID, item.Name, AssetType.Folder, id, false);
                }
			}
		}
		
        void Inventory_OnTaskItemReceived(UUID itemID, UUID folderID, UUID creatorID, UUID assetID, InventoryType type)
        {

            Console.Write("\nOn Task Item Recieved\n");
        }

        void Inventory_OnTaskInventoryReply(UUID itemID, short serial, string assetFilename)
        {
            Console.Write("\nOn Task Inventory Reply\n");
        }

        void Inventory_onFolderUpdated(UUID folderID)
        {

        }

		void FixBorkedFolder(object o, ButtonPressEventArgs args)
		{
            /*
			  Gtk.TreeModel mod;
			    Gtk.TreeIter iter;

               
                if (this.treeview_inv.Selection.GetSelected(out mod, out iter))
                {   
					
					
					
					    UUID id=(UUID)mod.GetValue(iter, 2);
						Console.WriteLine("ID is "+id.ToString());
						InventoryBase item = (InventoryBase)MainClass.client.Inventory.Store.Items[id].Data;
			
			            MainClass.client.Inventory.MoveFolder(id,MainClass.client.Inventory.Store.RootFolder.UUID);
				
				
			}
             * */
		}

        void Teleporttolandmark(object o, ButtonPressEventArgs args)
        {
            Gtk.TreeModel mod;
            Gtk.TreeIter iter;
            TreePath[] paths = treeview_inv.Selection.GetSelectedRows(out mod);
            mod.GetIter(out iter, paths[0]);
            TeleportProgress tp = new TeleportProgress();
            tp.Show();
            InventoryLandmark item = (InventoryLandmark)mod.GetValue(iter, 3);

            tp.teleportassetid(item.AssetUUID,item.Name);
			//MainClass.client.Self.Teleport(item.AssetUUID);
        }

      
        [GLib.ConnectBefore]
        void treeview_inv_ButtonPressEvent(object o, ButtonPressEventArgs args)
        {
            lock (inventory)
            {
                if (args.Event.Button == 3)//Fuck this should be a define
                {
                    // Do the context sensitive stuff here
                    // Detect type of asset selected and show an approprate menu
                    // maybe
                    Gtk.TreeModel mod;
                    Gtk.TreeIter iter;
                    InventoryBase item = null;

                    Console.WriteLine("ROOT IS " + MainClass.client.Inventory.Store.RootFolder.UUID.ToString());

                    TreePath[] paths = treeview_inv.Selection.GetSelectedRows(out mod);

                    if (paths.Length == 1)
                    {
                        //all good and simple
                        TreeIter itera;
                        mod.GetIter(out itera, paths[0]);
                        UUID ida = (UUID)mod.GetValue(itera, 2);
                        item = (InventoryBase)MainClass.client.Inventory.Store.GetNodeFor(ida).Data;

                    }

                    if (paths.Length > 1)
                    {
                        bool allsame = true;
                        bool wearables = true;
                        bool folders = true;
                        TreeIter itera, iterb;

                        foreach (TreePath path in paths)
                        {
                            mod.GetIter(out itera, path);
                            UUID ida = (UUID)mod.GetValue(itera, 2);
                            InventoryBase itema = (InventoryBase)MainClass.client.Inventory.Store.GetNodeFor(ida).Data;
                            if (!(itema is InventoryWearable))
                                wearables = false;
                            if (!(itema is InventoryFolder))
                                folders = false;

                            foreach (TreePath innerpath in paths)
                            {
                                mod.GetIter(out iterb, innerpath);
                                UUID idb = (UUID)mod.GetValue(iterb, 2);
                                InventoryBase itemb = (InventoryBase)MainClass.client.Inventory.Store.GetNodeFor(idb).Data;

                                if (itema.GetType() != itemb.GetType())
                                {
                                    allsame = false;
                                }
                            }

                        }

                        //ok if allsame==true we can allow specific extra menu options
                        //or if all wearables then we can allow wearable options
                        if (allsame)
                        {
                            mod.GetIter(out iter, paths[0]);
                            UUID ida = (UUID)mod.GetValue(iter, 2);
                            item = (InventoryBase)MainClass.client.Inventory.Store.GetNodeFor(ida).Data;
                        }
                        else if (wearables)
                        {
                            item = new InventoryWearable(UUID.Zero); //fake an item
                        }

                    }


                    if (item == null)
                        return;

                    Gtk.Menu menu = new Gtk.Menu();

                    Console.WriteLine("Item is " + item.ToString() + " ID is " + item.UUID.ToString());

                    Console.WriteLine("Item parent is " + item.ToString() + " ID is " + item.ParentUUID.ToString());

                    if (item is InventoryLandmark)
                    {
                        Gtk.ImageMenuItem menu_tp_lm = new ImageMenuItem("Teleport to Landmark");
                        menu_tp_lm.Image = new Gtk.Image(MainClass.GetResource("icon_place.png"));
                        menu_tp_lm.ButtonPressEvent += new ButtonPressEventHandler(Teleporttolandmark);
                        menu.Append(menu_tp_lm);
                    }

                    if (item is InventoryFolder)
                    {
                        if (item.UUID == trash_folder)
                        {
                            Gtk.ImageMenuItem menu_delete_folder = new ImageMenuItem("Empty Trash");
                            menu_delete_folder.Image = new Gtk.Image(MainClass.GetResource("inv_folder_trash.png"));
                            menu_delete_folder.ButtonPressEvent += new ButtonPressEventHandler(onemptytrash);
                            menu.Append(menu_delete_folder);
                        }
                        else
                        {
                          
      
                          if (item.UUID == MainClass.client.Inventory.Store.LibraryFolder.UUID)
                                return;



                            Gtk.MenuItem menu_wear_folder = new MenuItem("Wear folder contents");
                            Gtk.ImageMenuItem menu_give_folder = new ImageMenuItem("Give folder to user");
                            menu_give_folder.Image = new Gtk.Image(MainClass.GetResource("ff_edit_theirs.png"));

                            Gtk.ImageMenuItem new_note = new ImageMenuItem("Create new notecard");
                            new_note.Image = new Gtk.Image(MainClass.GetResource("inv_item_notecard.png"));

                            Gtk.ImageMenuItem new_script = new ImageMenuItem("Create new script");
                            new_script.Image = new Gtk.Image(MainClass.GetResource("inv_item_script.png"));

                            Gtk.ImageMenuItem new_folder = new ImageMenuItem("Create new folder");
                            new_folder.Image = new Gtk.Image(MainClass.GetResource("inv_folder_plain_open.png"));

                            Gtk.ImageMenuItem menu_cut_folder = new ImageMenuItem("Cut Folder");
                            menu_cut_folder.Image = new Gtk.Image(Gtk.Stock.Cut, IconSize.Menu);

                            Gtk.ImageMenuItem menu_copy_folder = new ImageMenuItem("Copy Folder");
                            menu_copy_folder.Image = new Gtk.Image(Gtk.Stock.Copy, IconSize.Menu);

                            Gtk.ImageMenuItem menu_paste_folder = new ImageMenuItem("Paste here..");
                            menu_paste_folder.Image = new Gtk.Image(Gtk.Stock.Paste, IconSize.Menu);


                            Gtk.ImageMenuItem menu_delete_folder = new ImageMenuItem("Delete Folder");
                            menu_delete_folder.Image = new Gtk.Image(MainClass.GetResource("inv_folder_trash.png"));

                            menu_delete_folder.ButtonPressEvent += new ButtonPressEventHandler(ondeleteasset);
                            menu_give_folder.ButtonPressEvent += new ButtonPressEventHandler(ongiveasset);
                            menu_wear_folder.ButtonPressEvent += new ButtonPressEventHandler(menu_ware_ButtonPressEvent);
                            //menu_debork.ButtonPressEvent += new ButtonPressEventHandler(FixBorkedFolder);
                            new_note.ButtonPressEvent += new ButtonPressEventHandler(menu_on_new_note);
                            new_script.ButtonPressEvent += new ButtonPressEventHandler(menu_on_new_script);
                            new_folder.ButtonPressEvent += new ButtonPressEventHandler(menu_on_new_folder);
                            menu_cut_folder.ButtonPressEvent += new ButtonPressEventHandler(menu_on_cut_folder);
                            menu_copy_folder.ButtonPressEvent += new ButtonPressEventHandler(menu_on_copy_folder);
                            menu_paste_folder.ButtonPressEvent += new ButtonPressEventHandler(menu_on_paste_folder);

                            Gtk.Label x = new Gtk.Label("Folder Item");

                            if (item.UUID != MainClass.client.Inventory.Store.RootFolder.UUID)
                            {
                                //menu.Append(menu_debork);
                                menu.Append(menu_wear_folder);
                            }

                            if (paths.Length == 1)
                            {
                                menu.Append(new Gtk.SeparatorMenuItem());
                                menu.Append(new_note);
                                menu.Append(new_script);
                                menu.Append(new_folder);
                            }

                            if (item.UUID != MainClass.client.Inventory.Store.RootFolder.UUID)
                            {
                                menu.Append(new Gtk.SeparatorMenuItem());
                                menu.Append(menu_give_folder);
                            }

                            menu.Append(new Gtk.SeparatorMenuItem());

                            if (item.UUID != MainClass.client.Inventory.Store.RootFolder.UUID)
                            {
                                menu.Append(menu_cut_folder);
                            }
                                //menu.Append(menu_copy_folder);
                            if (cutcopylist.Count > 0)
                                menu.Append(menu_paste_folder);

                            if (item.UUID != MainClass.client.Inventory.Store.RootFolder.UUID)
                            {
                                menu.Append(new Gtk.SeparatorMenuItem());
                                menu.Append(menu_delete_folder);
                            }
                        }
                    }
                    if (item is InventoryNotecard)
                    {
                        Gtk.MenuItem menu_read_note = new MenuItem("Open notecard");
                        menu_read_note.ButtonPressEvent += new ButtonPressEventHandler(onOpenNotecard);
                        menu.Append(menu_read_note);

                    }

                    if (item is InventoryLSL)
                    {
                        Gtk.MenuItem menu_read_note = new MenuItem("Open script");
                        menu_read_note.ButtonPressEvent += new ButtonPressEventHandler(onOpenScript);
                        menu.Append(menu_read_note);

                    }

                    if (item is InventoryTexture || item is InventorySnapshot)
                    {
                        Gtk.MenuItem menu_view_texture = new MenuItem("View texture");
                        menu_view_texture.ButtonPressEvent += new ButtonPressEventHandler(onViewTexture);
                        menu.Append(menu_view_texture);
                    }




                    if (item is InventoryAttachment || item is InventoryObject)
                    {
                        Gtk.MenuItem menu_attach_item = new MenuItem("Attach (default pos)");
                        menu_attach_item.ButtonPressEvent += new ButtonPressEventHandler(menu_attach_item_ButtonPressEvent);
                        menu.Append(menu_attach_item);
                    }

                    if (item is InventoryWearable)
                    {
                        Gtk.MenuItem menu_attach_item = new MenuItem("Wear");
                        menu_attach_item.ButtonPressEvent += new ButtonPressEventHandler(menu_wear_item_ButtonPressEvent);
                        menu.Append(menu_attach_item);
                    }

                    if (item is InventoryItem)
                    {

                        Gtk.ImageMenuItem menu_give_item = new ImageMenuItem("Give item to user");
                        menu_give_item.Image = new Gtk.Image(MainClass.GetResource("ff_edit_theirs.png"));

                        Gtk.ImageMenuItem menu_delete_item = new ImageMenuItem("Delete item");
                        menu_delete_item.Image = new Gtk.Image(MainClass.GetResource("inv_folder_trash.png"));

                        menu_give_item.ButtonPressEvent += new ButtonPressEventHandler(ongiveasset);
                        menu_delete_item.ButtonPressEvent += new ButtonPressEventHandler(ondeleteasset);
                        menu.Append(new Gtk.SeparatorMenuItem());
                        menu.Append(menu_give_item);
                        menu.Append(new Gtk.SeparatorMenuItem());

                        Gtk.ImageMenuItem menu_cut_folder = new ImageMenuItem("Cut Item(s)");
                        menu_cut_folder.Image = new Gtk.Image(Gtk.Stock.Cut, IconSize.Menu);
                        Gtk.ImageMenuItem menu_copy_folder = new ImageMenuItem("Copy Item(s)");
                        menu_copy_folder.Image = new Gtk.Image(Gtk.Stock.Copy, IconSize.Menu);
                        menu_cut_folder.ButtonPressEvent += new ButtonPressEventHandler(menu_on_cut_folder);
                        menu_copy_folder.ButtonPressEvent += new ButtonPressEventHandler(menu_on_copy_folder);
                        menu.Append(menu_cut_folder);
                        // menu.Append(menu_copy_folder);

                        menu.Append(new Gtk.SeparatorMenuItem());
                        menu.Append(menu_delete_item);
                    }

                    menu.Popup();
                    menu.ShowAll();

                }
            }
        }

        void menu_on_cut_folder(object o, ButtonPressEventArgs args)
        {

            Gtk.TreeModel mod;
            Gtk.TreeIter iter;

            TreePath[] paths = treeview_inv.Selection.GetSelectedRows(out mod);
            iscut = true;
            cutcopylist.Clear();

            foreach (TreePath path in paths)
            {
                if (mod.GetIter(out iter, path))
                {
                    InventoryBase item = (InventoryBase)mod.GetValue(iter, 3);
                    cutcopylist.Add(item);
                   
                }
            }

        }

        void menu_on_copy_folder(object o, ButtonPressEventArgs args)
        {

            Gtk.TreeModel mod;
            Gtk.TreeIter iter;

            TreePath[] paths = treeview_inv.Selection.GetSelectedRows(out mod);
            iscut = false;
            cutcopylist.Clear();

            foreach (TreePath path in paths)
            {
                if (mod.GetIter(out iter, path))
                {
                    InventoryBase item = (InventoryBase)mod.GetValue(iter, 3);
                    cutcopylist.Add(item);
                }
            }

        }

        void menu_on_paste_folder(object o, ButtonPressEventArgs args)
        {

            Gtk.TreeModel mod;
            Gtk.TreeIter iter;

            TreePath[] paths = treeview_inv.Selection.GetSelectedRows(out mod);
            Dictionary<UUID, UUID> folders = new Dictionary<UUID, UUID>();
            Dictionary<UUID, UUID> items = new Dictionary<UUID, UUID>();

            Dictionary<UUID, InventoryBase> inv_items = new Dictionary<UUID, InventoryBase>();


            if (paths.Length == 1)
            {
                mod.GetIter(out iter, paths[0]);
                InventoryBase dest_item = (InventoryBase)mod.GetValue(iter, 3);
                Gtk.TreeIter dest_iter;
                if(!assetmap.TryGetValue(dest_item.UUID,out dest_iter))
                {
                    Logger.Log("Error destination folder is not in view",Helpers.LogLevel.Error);
                    return;
                }

                foreach (InventoryBase item in cutcopylist)
                {
                    if (item == null)
                        continue;

                    if (item is InventoryItem)
                    {
                        items.Add(item.UUID, dest_item.UUID);
                    }
                    if (item is InventoryFolder)
                    {
                        folders.Add(item.UUID, dest_item.UUID);
                    }

                    inv_items.Add(item.UUID, item);
                }

                if(items.Count > 0)
                    MainClass.client.Inventory.MoveItems(items);

                if (folders.Count > 0)
                    MainClass.client.Inventory.MoveFolders(folders);

                foreach (KeyValuePair<UUID, UUID> kvp in items)
                {
                    Gtk.TreeIter iterx;
                    iterx = assetmap[kvp.Key];
                    inventory.Remove(ref iterx);
                    if (assetmap.ContainsKey(kvp.Key))
                        assetmap.Remove(kvp.Key);

                    assetmap.Add(kvp.Key, inventory.AppendValues(dest_iter, getprettyicon(inv_items[kvp.Key]), inv_items[kvp.Key].Name, inv_items[kvp.Key].UUID, inv_items[kvp.Key]));
                }

                foreach (KeyValuePair<UUID, UUID> kvp in folders)
                {
                    Gtk.TreeIter iterx;
                    iterx = assetmap[kvp.Key];
                    inventory.Remove(ref iterx);
                    if (assetmap.ContainsKey(kvp.Key))
                        assetmap.Remove(kvp.Key);

                    assetmap.Add(kvp.Key, inventory.AppendValues(dest_iter, getprettyicon(inv_items[kvp.Key]), inv_items[kvp.Key].Name, inv_items[kvp.Key].UUID, inv_items[kvp.Key]));
                    // We need to make sure the view still knows about children of this folder as well or it will show it in trash with none
                    inventory.AppendValues(assetmap[kvp.Key], item_object, "Waiting...", UUID.Zero, null);

                }

            }
        }

		void onOpenNotecard (object o, ButtonPressEventArgs args)
		{
         
            Gtk.TreeModel mod;
			Gtk.TreeIter iter;

            TreePath[] paths = treeview_inv.Selection.GetSelectedRows(out mod);

            foreach (TreePath path in paths)
            {
                if (mod.GetIter(out iter, path))
                {
                    InventoryBase item = (InventoryBase)mod.GetValue(iter, 3);

                    NotecardReader nr = new NotecardReader(item.UUID, UUID.Zero, UUID.Zero);
                }
            }
			
	}
		
		void menu_on_new_folder (object o, ButtonPressEventArgs args)
		{
         
            Gtk.TreeModel mod;
			Gtk.TreeIter iter;

            TreePath[] paths = treeview_inv.Selection.GetSelectedRows(out mod);
			if(paths.Length!=1)
				return;			

            foreach (TreePath path in paths)
            {
                if (mod.GetIter(out iter, path))
				{
                	InventoryBase item = (InventoryBase)mod.GetValue(iter, 3);
					if(item is InventoryFolder)
					{
						Gtk.TreeIter iterx;
						if(assetmap.TryGetValue(item.UUID,out iterx))
						{
							UUID newfolder=MainClass.client.Inventory.CreateFolder(item.UUID,"New Folder");		
							Gdk.Pixbuf buf = getprettyicon(item);	//a folder so fine.
                            InventoryFolder nf = new InventoryFolder(newfolder);
                            nf.Name = "New Folder";
                            nf.ParentUUID = item.UUID;
                            nf.Version = 1;
                            Gtk.TreeIter newiter=inventory.AppendValues(iterx, buf, "New Folder", newfolder, (InventoryBase)nf);
							assetmap.Add(newfolder,newiter);
                            treeview_inv.Selection.UnselectAll();
                           
                            treeview_inv.Selection.SelectIter(filter.ConvertChildIterToIter(newiter));
                            treeview_inv.ScrollToCell(inventory.GetPath(newiter), null, true, (float)0.5, (float)0.5);
                            treeview_inv.SetCursor(inventory.GetPath(newiter), null, true);
                        }
 		            }
                }
			}	
	}
		

		void menu_on_new_note (object o, ButtonPressEventArgs args)
		{
         
            Gtk.TreeModel mod;
			Gtk.TreeIter iter;

            TreePath[] paths = treeview_inv.Selection.GetSelectedRows(out mod);
			if(paths.Length!=1)
				return;			

            foreach (TreePath path in paths)
            {
                if (mod.GetIter(out iter, path))
				{
                	InventoryBase item = (InventoryBase)mod.GetValue(iter, 3);
					if(item is InventoryFolder)
					{
                    	MainClass.client.Inventory.RequestCreateItem(item.UUID,
                    	"New Note", "New Note", AssetType.Notecard, UUID.Random(), InventoryType.Notecard, PermissionMask.All,
                    	delegate(bool success, InventoryItem itemx)
                    	{
							if (success) // upload the asset
							{

                                AssetNotecard note = new AssetNotecard();
								note.BodyText="Add your notes here....";
								note.Encode();
								
						        MainClass.client.Inventory.RequestUploadNotecardAsset(note.AssetData, itemx.UUID,delegate (bool success2,string status,UUID item_uuid, UUID asset_uuid)
								{                                                  
								    Gtk.TreeIter iterx;
	  
									if(success2 && assetmap.TryGetValue(item.UUID,out iterx))
									{
										Gdk.Pixbuf buf = getprettyicon(itemx);
                                        Gtk.TreeIter newiter = inventory.AppendValues(iterx, buf, "New Note", itemx.UUID, itemx);
                                        assetmap.Add(item_uuid,newiter);
                                        
                                        treeview_inv.Selection.SelectIter(filter.ConvertChildIterToIter(newiter));
                                        treeview_inv.ScrollToCell(inventory.GetPath(newiter), null, true, (float)0.5, (float)0.5);
                                        treeview_inv.SetCursor(inventory.GetPath(newiter), null, true);
                                        
                                    }		
							    });
							}
						});
							
 		            }
                }
			}	
	}
		
		void menu_on_new_script (object o, ButtonPressEventArgs args)
		{
         
            Gtk.TreeModel mod;
			Gtk.TreeIter iter;

            TreePath[] paths = treeview_inv.Selection.GetSelectedRows(out mod);
			if(paths.Length!=1)
				return;			

            foreach (TreePath path in paths)
            {
                if (mod.GetIter(out iter, path))
				{
                	InventoryBase item = (InventoryBase)mod.GetValue(iter, 3);
					if(item is InventoryFolder)
					{
                    	MainClass.client.Inventory.RequestCreateItem(item.UUID,
                    	"New Script", "New Script", AssetType.LSLText, UUID.Random(), InventoryType.LSL, PermissionMask.All,
                    	delegate(bool success, InventoryItem itemx)
                    	{
							if (success) // upload the asset
							{
						        MainClass.client.Inventory.RequestUploadNotecardAsset(Utils.StringToBytes("Add your code here..."), itemx.UUID,delegate (bool success2,string status,UUID item_uuid, UUID asset_uuid)
								{                                                  
								    Gtk.TreeIter iterx;
	  
									if(success2 && assetmap.TryGetValue(item.UUID,out iterx))
									{
										Gdk.Pixbuf buf = getprettyicon(itemx);
                                        Gtk.TreeIter newiter = inventory.AppendValues(iterx, buf, "New Script", itemx.UUID, itemx);
									 	assetmap.Add(item_uuid,newiter);

                                        treeview_inv.Selection.SelectIter(filter.ConvertChildIterToIter(newiter));
                                        treeview_inv.ScrollToCell(inventory.GetPath(newiter), null, true, (float)0.5, (float)0.5);
                                        treeview_inv.SetCursor(inventory.GetPath(newiter), null, true);
                                       
                                    }		
							    });
							}
						});
							
 		            }
                }
            }	
		}
		
		
		void onOpenScript (object o, ButtonPressEventArgs args)
		{
         
            Gtk.TreeModel mod;
			Gtk.TreeIter iter;

            TreePath[] paths = treeview_inv.Selection.GetSelectedRows(out mod);

            foreach (TreePath path in paths)
            {
                if (mod.GetIter(out iter, path))
                {
                    InventoryBase item = (InventoryBase)mod.GetValue(iter, 3);

					NotecardReader nr = new NotecardReader(item.UUID, UUID.Zero, UUID.Zero);
                    
                }
            }
			
		}

						
		void onViewTexture (object o, ButtonPressEventArgs args)
		{
            Gtk.TreeModel mod;
			Gtk.TreeIter iter;

            TreePath[] paths = treeview_inv.Selection.GetSelectedRows(out mod);

            foreach (TreePath path in paths)
            {
                if (mod.GetIter(out iter, path))
                {
                  
                    InventoryBase item = (InventoryBase)mod.GetValue(iter, 3);
                    TexturePreview tp = new TexturePreview(item.UUID, item.Name, true);
                    tp.ShowAll();
                }
            }
			
		}

		
        void menu_wear_item_ButtonPressEvent(object o, ButtonPressEventArgs args)
        {
            
            Gtk.TreeModel mod;
			Gtk.TreeIter iter;

            TreePath[] paths = treeview_inv.Selection.GetSelectedRows(out mod);
            List<InventoryBase> ibs = new List<InventoryBase>();

            foreach (TreePath path in paths)
            {
                if (mod.GetIter(out iter, path))
                {
                    InventoryBase item = (InventoryBase)mod.GetValue(iter, 3);
                    ibs.Add(item);
                }
            }

            MainClass.client.Appearance.AddToOutfit(ibs, true);
        }
		
        void menu_attach_item_ButtonPressEvent(object o, ButtonPressEventArgs args)
        {
             Gtk.TreeModel mod;
			Gtk.TreeIter iter;

            TreePath[] paths = treeview_inv.Selection.GetSelectedRows(out mod);

            foreach (TreePath path in paths)
            {
                if (mod.GetIter(out iter, path))
                {
                    InventoryBase item = (InventoryBase)mod.GetValue(iter, 3);
                    MainClass.client.Appearance.Attach((InventoryItem)item, AttachmentPoint.Default);
                }
            }
        }

		void onEventQueue(Simulator sim)
		{
			if(sim.ID==MainClass.client.Network.CurrentSim.ID)
			{
                if (inventoryloaded == false)
                {
                    inventoryloaded = true;
                    Gtk.Application.Invoke(delegate
                    {
                        inventory.Clear();
                        populate_top_level_inv();
                    });
                }
				 
			}
		}

        void onLogin(LoginStatus status, string message)
		{
			if(LoginStatus.Success==status)
			{

			}

		}
				
		void onRowCollapsed(object o,Gtk.RowCollapsedArgs args)
		{
            lock(inventory)
            {
            if (filteractive == true)
                return;

            try
            {
                Gdk.Pixbuf image = folder_closed;
                TreeIter iter = filter.ConvertIterToChildIter(args.Iter);
                
                UUID key = (UUID)this.inventory.GetValue(iter, 2);
                InventoryBase item = (InventoryBase)this.inventory.GetValue(iter, 3);
                if (item == null)
                    return;
                if (item is InventoryFolder)
                {
                    image = getprettyfoldericon((InventoryFolder)item);
                }

                inventory.SetValue(iter, 0, image);
            }
            catch
            {
            }
		   }
        }

		void onRowExpanded(object o,Gtk.RowExpandedArgs args)
		{
            lock (inventory)
            {

                // Avoid updaing rows in the middle of a filter operation
                if (filteractive == true)
                    return;

                //We can't do this or it confuses the hell out of stuff
                //if (fetcherrunning == true)
                  //  return;

                try
                {
                    TreeIter iter = filter.ConvertIterToChildIter(args.Iter);

                    UUID key = (UUID)this.inventory.GetValue(iter, 2);

                    if (inventory.GetValue(iter, 0) == folder_closed)
                        inventory.SetValue(iter, 0, folder_open);

                    TreePath path = inventory.GetPath(iter);
                    path.Down();
                    TreeIter iter2;
                    inventory.GetIter(out iter2, path);

                    string Name = inventory.GetValue(iter2, 1).ToString();
                    //if (Name == "Waiting...")
                    {
  
                        Thread invRunner = new Thread(new ParameterizedThreadStart(fetchinventory));
                        invthreaddata itd = new invthreaddata(key, filter.ConvertPathToChildPath(args.Path).ToString(), iter, false, false);
                        invRunner.Start(itd);

                       
                    }
                }
                catch
                {

                }

            }
		}
		
		void fetchinventory(object x)
		{
            recursion++;
            invthreaddata itd = (invthreaddata)x;
 			UUID start=itd.key;
            TreeIter iter = itd.iter;
            bool cache = itd.cacheonly;
			bool alreadyseen=true;
            bool recurse = itd.recurse;
            List<InventoryBase> myObjects;
            List <invthreaddata> runners= new List<invthreaddata>();
        
       		
            System.Threading.AutoResetEvent prefetch=new AutoResetEvent(false);
            TreePath path=null;

            Gtk.Application.Invoke(delegate
            {

                path = inventory.GetPath(iter);

                 if (path == null)
                 {
                     Console.WriteLine("*!*!*!*! WTF? we got a NULL path in the fetchinventory()");
                     return;
                 }

                 path.Down();

                 if (MainClass.client == null)
                 {
                     recursion--;
                     return;
                 }

                 InventoryNode node = MainClass.client.Inventory.Store.GetNodeFor(start);
                 if (node.NeedsUpdate == true)
                 {
                     alreadyseen = false;
                 }

                 //Check for a waiting here, we need to use this to decide which fetcher to use in a moment
                 if (cache == false)
                 {
                     TreeIter childiter;
                     inventory.GetIter(out childiter, path);
                     if ("Waiting..." == (string)inventory.GetValue(childiter, 1))
                     {
                         alreadyseen = false;
                     }
                 }

                 prefetch.Set();
            });

            prefetch.WaitOne();

            // Use an approprate fetcher based on various flags
            if(cache==true || alreadyseen==true)
                myObjects = MainClass.client.Inventory.Store.GetContents(start);
            else 
 	            myObjects = MainClass.client.Inventory.FolderContents(start, MainClass.client.Self.AgentID, true, true, InventorySortOrder.ByDate, 30000);
			
			//Console.WriteLine("Got objects # "+myObjects.Count.ToString());

            AutoResetEvent postfetch = new AutoResetEvent(false);

            Gtk.Application.Invoke(delegate
            {
                if (myObjects == null || myObjects.Count==0)
                {
				    recursion--;
                    postfetch.Set();
                    return;
			    }
    			
			    //Console.WriteLine("Possible refilter");
			    if(filteractive==true)
			    {
			       filter.Refilter();
			       filter.Refilter(); //*sigh*
	            }

                this.label_fetched.Text="fetched "+this.no_items.ToString()+" items";
           
          
			List<InventoryBase> folders = new List<InventoryBase>();

			foreach (InventoryBase item in myObjects)
            {

                if(item.Name=="Trash" && item is InventoryFolder && trash_folder==UUID.Zero)
                {
                    trash_folder=item.UUID;
                }

                if (assetmap.ContainsKey(item.UUID))
                {
                    TreeIter iterx = assetmap[item.UUID];
                    InventoryBase itemx = (InventoryBase)inventory.GetValue(iterx,3);
                    if (itemx is InventoryFolder)
					{
						invthreaddata itd2 = new invthreaddata(item.UUID, path.ToString(), iterx,cache,true);
                        runners.Add(itd2);
                        //fetchinventory((object)itd2);
                    }
                    continue;
                }

              
                   this.no_items++;
                   Gdk.Pixbuf buf = getprettyicon(item);
				  
				 
				   global_thread_tree = inventory.AppendValues(iter, buf, item.Name, item.UUID, item);
                                               
                   if (!assetmap.ContainsKey(item.UUID))
                           assetmap.Add(item.UUID, global_thread_tree);

				
					Gtk.TreeIter iter2=global_thread_tree;
					
                     if (item is InventoryFolder)
                     {
					    inventory.AppendValues(iter2, item_object, "Waiting...", item.UUID, null);
					    invthreaddata itd2 = new invthreaddata(((InventoryFolder)item).UUID, "", iter2,cache,true);
                        runners.Add(itd2);

                        if (abortfetch == true)
                        {
                            postfetch.Set();
                            return;
                        }
                     }
			}


            //We should preserve Waiting... messages for folders we don't yet have the children for
            //or else the user can't open them as there is no + to click on. But we need to get rid
            // of them for folders we have just got the data for!
            if (cache == false || (cache == true && myObjects.Count > 0))
            {
                TreeIter childiter;
                inventory.GetIter(out childiter, path);
                if ("Waiting..." == (string)inventory.GetValue(childiter, 1))
                {
                    inventory.Remove(ref childiter);

                    alreadyseen = false;
                }
            }
              
            postfetch.Set();
            });

            postfetch.WaitOne();

            if (abortfetch == true)
                return;

            if (myObjects == null || myObjects.Count == 0)
                return;

            if (recurse)
            {
                foreach (invthreaddata itdn in runners)
                {
                    if (abortfetch == true)
                        return;

                    fetchinventory((object)itdn);
                }
            }
			
            recursion--;

            if (recursion == 0)
            {
				fetcherrunning = false;
				if(cache==false && recurse==true)
				{
	                fetchrun=true;
					Console.WriteLine("Fetch Complete");
 
                    Gtk.Application.Invoke(delegate{
						this.label_fetched.Text="fetched "+this.no_items.ToString()+" items (Finished)";
					});
	             }
            }
			return;
		}
			
        Gdk.Pixbuf getprettyfoldericon(InventoryFolder item)
        {
            // Assume this is a InventoryFolder
            if (item.PreferredType == AssetType.Animation)
                return this.folder_animation;

            if (item.PreferredType == AssetType.Gesture)
                return this.folder_gesture;

            if (item.PreferredType == AssetType.Sound)
                return this.folder_sound;

            if (item.PreferredType == AssetType.Texture)
                return this.folder_texture;

            if (item.PreferredType == AssetType.SnapshotFolder)
                return this.folder_snapshot;

            if(item.PreferredType == AssetType.TrashFolder)
                return this.folder_trash;

            if (item.PreferredType == AssetType.Notecard)
                return this.folder_notecard;

            if (item.PreferredType == AssetType.LSLText || item.PreferredType == AssetType.LSLBytecode)
                return this.folder_script;

            if (item.PreferredType == AssetType.LostAndFoundFolder)
                return this.folder_lostandfound;

            if (item.PreferredType == AssetType.Landmark)
                return this.folder_landmark;

            if (item.PreferredType == AssetType.Bodypart)
                return this.folder_bodypart;

            if (item.PreferredType == AssetType.CallingCard)
                return this.folder_callingcard;

            if (item.PreferredType == AssetType.Clothing)
                return this.folder_clothing;

            return folder_closed;
        }

		Gdk.Pixbuf getprettyicon(InventoryBase item)
		{
            if (item is InventoryFolder)
                return getprettyfoldericon((InventoryFolder)item);
					
			if(item is OpenMetaverse.InventoryLandmark)
				return this.item_landmark;
			
			if(item is OpenMetaverse.InventoryAnimation)
				return this.item_animation;
			
			if(item is OpenMetaverse.InventoryWearable)
			{
				OpenMetaverse.InventoryWearable wearable=(OpenMetaverse.InventoryWearable)item;
				switch(wearable.WearableType)
				{
				case WearableType.Eyes:
                return this.item_clothing_eyes;					
              
				case WearableType.Gloves:
                return this.item_clothing_gloves;					
              
				case WearableType.Hair:
                return this.item_clothing_hair;					
                
				case WearableType.Jacket:
                return this.item_clothing_jacket;					
              
				case WearableType.Pants:
                return this.item_clothing_pants;					
              
				case WearableType.Shape:
                return this.item_clothing_shape;					
             
				case WearableType.Shirt:
                return this.item_clothing_shirt;					
               
				case WearableType.Shoes:
                return this.item_clothing_shoes;					
               
				case WearableType.Skin:
				return this.item_clothing_skin;					
            
				case WearableType.Skirt:
				return this.item_clothing_skirt;					
				
				case WearableType.Socks:
				return this.item_clothing_socks;					
					
				case WearableType.Underpants:
				return this.item_clothing_underpants;					
              
				case WearableType.Undershirt:
				return this.item_clothing_undershirt;					
             	
				default:
				return this.item_clothing;
	         
                }				
                
            }
            if (item is OpenMetaverse.InventoryGesture)
                return this.item_gesture;

            if (item is OpenMetaverse.InventoryNotecard)
                return this.item_notecard;

            if (item is OpenMetaverse.InventoryLSL)
                return this.item_script;

            if (item is OpenMetaverse.InventorySnapshot)
                return this.item_snapshot;

            if (item is OpenMetaverse.InventoryTexture)
                return this.item_texture;

            if (item is OpenMetaverse.InventorySound)
                return this.item_sound;

            if (item is OpenMetaverse.InventoryCallingCard)
                return this.item_callingcard;
         
			return item_object;
		}

		protected virtual void OnTreeviewInvCursorChanged (object sender, System.EventArgs e)
		{
			
			
            Gtk.TreeModel mod;
			Gtk.TreeIter iter;

            TreePath[] paths = treeview_inv.Selection.GetSelectedRows(out mod);

            foreach (TreePath path in paths)
            {
                if (mod.GetIter(out iter, path))
                {
					InventoryBase item = (InventoryBase)mod.GetValue(iter, 3);
					if(item==null)
                       continue;
					//Console.WriteLine("ITEM ID" + item.UUID.ToString() + " Parent " + item.ParentUUID.ToString());

                    if (item is InventoryItem)
                    {
                        this.label_createdby.Text = ((InventoryItem)item).CreatorID.ToString();
                        this.label_aquired.Text = ((InventoryItem)item).CreationDate.ToString();
                        this.checkbutton_copy.Active = OpenMetaverse.PermissionMask.Copy == (((InventoryItem)item).Permissions.OwnerMask & OpenMetaverse.PermissionMask.Copy);
                        this.checkbutton_mod.Active = OpenMetaverse.PermissionMask.Modify == (((InventoryItem)item).Permissions.OwnerMask & OpenMetaverse.PermissionMask.Modify);
                        this.checkbutton_trans.Active = OpenMetaverse.PermissionMask.Transfer == (((InventoryItem)item).Permissions.OwnerMask & OpenMetaverse.PermissionMask.Transfer);

                        this.checkbutton_copynext.Active = OpenMetaverse.PermissionMask.Copy == (((InventoryItem)item).Permissions.NextOwnerMask & OpenMetaverse.PermissionMask.Copy);
                        this.checkbutton_modnext.Active = OpenMetaverse.PermissionMask.Modify == (((InventoryItem)item).Permissions.NextOwnerMask & OpenMetaverse.PermissionMask.Modify);
                        this.checkbutton_transnext.Active = OpenMetaverse.PermissionMask.Transfer == (((InventoryItem)item).Permissions.NextOwnerMask & OpenMetaverse.PermissionMask.Transfer);

                        AsyncNameUpdate ud = new AsyncNameUpdate((UUID)((InventoryItem)item).CreatorID.ToString(), false);
                        ud.onNameCallBack += delegate(string name, object[] values) { this.label_createdby.Text = name; };
                        ud.go();

                        AsyncNameUpdate ud2 = new AsyncNameUpdate(((InventoryItem)item).GroupID, true);
                        ud2.onNameCallBack += delegate(string name, object[] values) { this.label_group.Text = name; };
                        ud2.go();

                        this.label_saleprice.Text = ((InventoryItem)item).SalePrice.ToString();
                    }
                    else
                    {
                        this.label_saleprice.Text = "";
                        this.label_createdby.Text = "";
                        this.label_aquired.Text = "";
                        this.checkbutton_mod.Active=false;
                        this.checkbutton_trans.Active=false;
                        this.checkbutton_copynext.Active=false;
                        this.checkbutton_modnext.Active=false;
                        this.checkbutton_transnext.Active = false;
                        this.label_group.Text = "";
					    this.label_createdby.Text = "";
                    }
                }
            }
		}

		protected virtual void OnEntrySearchChanged (object sender, System.EventArgs e)
		{
           
        lock(inventory)
        {
			if(fetchrun==false && fetcherrunning==false)
			{
				Logger.Log("Starting Inventory Fetch all",Helpers.LogLevel.Info);
			    fetcherrunning = true;
                Thread invRunner = new Thread(new ParameterizedThreadStart(fetchinventory));
                invthreaddata itd = new invthreaddata(MainClass.client.Inventory.Store.RootFolder.UUID, "0:0", TLI,false,true);
                invRunner.Start(itd);
			}

            filteractive = true;

            filter.ClearCache();
            filtered.Clear(); //*sigh*
            
            if (this.entry_search.Text == "")
            {
                filteractive = false;
                filter.Refilter();
                return;
            }
           
            //Becuase we use our own filter we have to do two passes at the data to first find matches, then to find parents of matches
          
            filter.Refilter();
            filter.Refilter(); //*sigh*
          
            treeview_inv.ExpandAll();
            }

		}

        protected virtual void OnButtonFetchAllInvClicked(object sender, System.EventArgs e)
        {
            if (fetcherrunning == false)
            {
                fetcherrunning = true;
                Thread invRunner = new Thread(new ParameterizedThreadStart(fetchinventory));
                invthreaddata itd = new invthreaddata(MainClass.client.Inventory.Store.RootFolder.UUID, "0:0", TLI,false,true);
                invRunner.Start(itd);
            }
		}                 
		
 		protected virtual void OnButtonCollapseAllClicked (object sender, System.EventArgs e)
		{
			treeview_inv.CollapseAll();
		}
	
		protected virtual void OnButtonExpandallClicked (object sender, System.EventArgs e)
		{
			treeview_inv.ExpandAll();
		}
		
		protected virtual void OnRadiobutton1Clicked (object sender, System.EventArgs e)
		{
            try
            {
                int Col;
                SortType order;
                inventory.GetSortColumnId(out Col, out order);
                if (Col==1)
                {
                    inventory.SetSortColumnId(0, order);
                }
                else
                {
                    inventory.SetSortColumnId(1, order);
                }
                inventory.ChangeSortColumn();
            }
            catch (Exception ee)
            {
                Logger.Log("Error on swithing sort for inventory " + ee.Message, Helpers.LogLevel.Error);
            }
		}
		protected virtual void OnRadiobuton2Clicked (object sender, System.EventArgs e)
		{
             try
            {
                int Col;
                SortType order;
                inventory.GetSortColumnId(out Col, out order);
                if (Col==1)
                {
                    inventory.SetSortColumnId(0, order);
                }
                else
                {
                    inventory.SetSortColumnId(1, order);
                }
                inventory.ChangeSortColumn();
            }
            catch (Exception ee)
            {
                Logger.Log("Error on swithing sort for inventory " + ee.Message, Helpers.LogLevel.Error);
            }
            
		}
		
		
		protected virtual void OnCheckSpecialFoldersClicked (object sender, System.EventArgs e)
		{
             try
            {
                int Col;
                SortType order;
                inventory.GetSortColumnId(out Col, out order);
                if (Col==1)
                {
                    inventory.SetSortColumnId(0, order);
                }
                else
                {
                    inventory.SetSortColumnId(1, order);
                }
                inventory.ChangeSortColumn();
            }
            catch (Exception ee)
            {
                Logger.Log("Error on swithing sort for inventory " + ee.Message, Helpers.LogLevel.Error);
            }

	}
	
	
		
	}	
}