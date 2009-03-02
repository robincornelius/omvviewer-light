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
using System.Collections.Generic;
using Gdk;
using Gtk;
using System.IO;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


namespace omvviewerlight
{
    public partial class invthreaddata
    {
		public UUID key;
        public TreeIter iter;
		public string path;
        public bool cacheonly;
		public invthreaddata(UUID keyx, string pathx,TreeIter iterx,bool cache)
        {
            key = keyx;
            iter = iterx;
			path = pathx;
            cacheonly = cache;
		}
    }
	
	public partial class Inventory : Gtk.Bin
	{ 	
		public int no_items;
        Dictionary<invthreaddata, List<InventoryBase>> incomming = new Dictionary<invthreaddata, List<InventoryBase>>();  
		Dictionary<UUID, Gtk.TreeIter> assetmap = new Dictionary<UUID, Gtk.TreeIter>();
        Dictionary<UUID, Gtk.TreeIter> invmap = new Dictionary<UUID, TreeIter>();
		String[] SearchFolders = { "" };
		//initialize our list to store the folder contents
		Gtk.TreeStore inventory = new Gtk.TreeStore (typeof(Gdk.Pixbuf),typeof (string), typeof (UUID),typeof(InventoryBase));		
		Gdk.Pixbuf folder_closed = MainClass.GetResource("inv_folder_plain_closed.tga");
		Gdk.Pixbuf folder_open = MainClass.GetResource("inv_folder_plain_open.tga");
		Gdk.Pixbuf item_landmark = MainClass.GetResource("inv_item_landmark.tga");
		Gdk.Pixbuf item_animation = MainClass.GetResource("inv_item_animation.tga");
		Gdk.Pixbuf item_clothing = MainClass.GetResource("inv_item_clothing.tga");
        Gdk.Pixbuf item_object = MainClass.GetResource("inv_item_object.tga");
        Gdk.Pixbuf item_gesture = MainClass.GetResource("inv_item_gesture.tga");
        Gdk.Pixbuf item_notecard = MainClass.GetResource("inv_item_notecard.tga");
        Gdk.Pixbuf item_script = MainClass.GetResource("inv_item_script.tga");
        Gdk.Pixbuf item_snapshot = MainClass.GetResource("inv_item_snapshot.tga");
        Gdk.Pixbuf item_texture = MainClass.GetResource("inv_item_texture.tga");

        Gdk.Pixbuf item_sound = MainClass.GetResource("inv_item_sound.tga");
        Gdk.Pixbuf item_callingcard = MainClass.GetResource("inv_item_callingcard_offline.tga");
		
		Gdk.Pixbuf item_clothing_eyes = MainClass.GetResource("inv_item_eyes.tga");
		Gdk.Pixbuf item_clothing_gloves = MainClass.GetResource("inv_item_gloves.tga");
		Gdk.Pixbuf item_clothing_hair= MainClass.GetResource("inv_item_hair.tga");
		Gdk.Pixbuf item_clothing_jacket= MainClass.GetResource("inv_item_jacket.tga");
		Gdk.Pixbuf item_clothing_pants= MainClass.GetResource("inv_item_pants.tga");
		Gdk.Pixbuf item_clothing_shoes= MainClass.GetResource("inv_item_shoes.tga");
		Gdk.Pixbuf item_clothing_skin= MainClass.GetResource("inv_item_skin.tga");
		Gdk.Pixbuf item_clothing_skirt= MainClass.GetResource("inv_item_skirt.tga");
		Gdk.Pixbuf item_clothing_underpants= MainClass.GetResource("inv_item_underpants.tga");
		Gdk.Pixbuf item_clothing_undershirt= MainClass.GetResource("inv_item_undershirt.tga");
	
		Gdk.Pixbuf item_clothing_shirt= MainClass.GetResource("inv_item_shirt.tga");
		Gdk.Pixbuf item_clothing_socks= MainClass.GetResource("inv_item_socks.tga");
		Gdk.Pixbuf item_clothing_shape= MainClass.GetResource("inv_item_shape.tga");

        Gdk.Pixbuf folder_texture = MainClass.GetResource("inv_folder_texture.tga");
        Gdk.Pixbuf folder_sound = MainClass.GetResource("inv_folder_sound.tga");
        Gdk.Pixbuf folder_animation = MainClass.GetResource("inv_folder_animation.tga");
        Gdk.Pixbuf folder_gesture = MainClass.GetResource("inv_folder_gesture.tga");
        Gdk.Pixbuf folder_snapshot = MainClass.GetResource("inv_folder_snapshot.tga");
        Gdk.Pixbuf folder_trash = MainClass.GetResource("inv_folder_trash.tga");
        Gdk.Pixbuf folder_notecard = MainClass.GetResource("inv_folder_notecard.tga");
        Gdk.Pixbuf folder_script = MainClass.GetResource("inv_folder_script.tga");
        Gdk.Pixbuf folder_lostandfound = MainClass.GetResource("inv_folder_lostandfound.tga");
        Gdk.Pixbuf folder_landmark = MainClass.GetResource("inv_folder_landmark.tga");
        Gdk.Pixbuf folder_bodypart = MainClass.GetResource("inv_folder_bodypart.tga");
        Gdk.Pixbuf folder_callingcard = MainClass.GetResource("inv_folder_callingcard.tga");
        Gdk.Pixbuf folder_clothing = MainClass.GetResource("inv_folder_clothing.tga");

      
        bool inventoryloaded = false;

        Gtk.TreeModelFilter filter;

        TreeIter TLI;

        bool filteractive = false;
        bool fetcherrunning = false;
        int recursion = 0;
		
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

           


         //   System.Runtime.Serialization.SerializationInfo info=new System.Runtime.Serialization.SerializationInfo(typeof(OpenMetaverse.InventoryNode),
         //   MainClass.client.Inventory.Store.Items.GetObjectData(



			Console.WriteLine("Inventory Cleaned up");
		}
	    	
        new public void Dispose()
		{
            Console.WriteLine("Running cleanup code for inventory");
			
            this.treeview_inv.RowExpanded -= new Gtk.RowExpandedHandler(onRowExpanded);
            this.treeview_inv.RowCollapsed -= new Gtk.RowCollapsedHandler(onRowCollapsed);
            MainClass.client.Network.OnLogin -= new OpenMetaverse.NetworkManager.LoginCallback(onLogin);
            this.treeview_inv.ButtonPressEvent -= new ButtonPressEventHandler(treeview_inv_ButtonPressEvent);			
			MainClass.client.Network.OnEventQueueRunning -= new OpenMetaverse.NetworkManager.EventQueueRunningCallback(onEventQueue);
        	
            Gtk.Notebook p;
            p = (Gtk.Notebook)this.Parent;
            p.RemovePage(p.PageNum(this));
			
			//Finalize();
			//System.GC.SuppressFinalize(this);
		}

		public Inventory()
		{
			this.Build();		
			

            treeview_inv.AppendColumn("",new CellRendererPixbuf(),"pixbuf",0);
            MyTreeViewColumn col = new MyTreeViewColumn("Name", new Gtk.CellRendererText(), "text", 1,true);
            col.setmodel(inventory);
            treeview_inv.InsertColumn(col, 1);
            //treeview_inv.Model=inventory;

            this.treeview_inv.RowExpanded += new Gtk.RowExpandedHandler(onRowExpanded);
			this.treeview_inv.RowCollapsed += new Gtk.RowCollapsedHandler(onRowCollapsed);
            this.treeview_inv.ButtonPressEvent += new ButtonPressEventHandler(treeview_inv_ButtonPressEvent);

			this.treeview_inv.Selection.Mode = SelectionMode.Multiple;    

            filter = new Gtk.TreeModelFilter(inventory, null);
            filter.VisibleFunc = new TreeModelFilterVisibleFunc(FilterTree); 
            treeview_inv.Model = filter;
			treeview_inv.HeadersClickable=true;
			
            this.inventory.SetSortFunc(1, sortinventoryfunc);
            this.inventory.SetSortColumnId(1, SortType.Ascending);

            MainClass.client.Network.OnLogin += new OpenMetaverse.NetworkManager.LoginCallback(onLogin);
            MainClass.client.Network.OnLogoutReply += new NetworkManager.LogoutCallback(Network_OnLogoutReply);
			MainClass.client.Network.OnEventQueueRunning += new OpenMetaverse.NetworkManager.EventQueueRunningCallback(onEventQueue);
            MainClass.client.Inventory.OnFolderUpdated += new InventoryManager.FolderUpdatedCallback(Inventory_onFolderUpdated);
            MainClass.client.Inventory.OnCacheDelete += new InventoryManager.CacheStaleCallback(Inventory_OnCacheDelete);

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

        void Inventory_OnCacheDelete(List<UUID> delete_list)
        {
            Console.WriteLine("Cache delete");
            Gtk.Application.Invoke(delegate
            {
                foreach (UUID item in delete_list)
                {
                    if (this.invmap.ContainsKey(item))
                    {
                        Console.WriteLine("Trying to remove item " + item.ToString());
                        TreeIter iter = invmap[item];
                        //this.inventory.Remove(ref iter);
                        //invmap.Remove(item);
                        //if(assetmap.ContainsKey(item))
                        //    assetmap.Remove(item);
                        string name=(string)inventory.GetValue(iter, 1);
                        inventory.SetValue(iter, 1, name + " (delete)");
                    }

                }
            });
        }

        void Network_OnLogoutReply(List<UUID> inventoryItems)
        {
            try
            {
                //Save cache inventory;
             //   MainClass.client.Inventory.Store.cache_inventory_to_disk(MainClass.client.Settings.TEXTURE_CACHE_DIR + "\\" + MainClass.client.Inventory.Store.RootFolder.UUID.ToString() + ".osl");
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception saving inventory :"+e.Message);
            }
        }
     
        private bool FilterTree(Gtk.TreeModel model, Gtk.TreeIter iter)
        {
            try
            {
                    if (this.entry_search.Text == "")
                        return true;

                    if (filtered.Contains(iter))//*sigh*
                        return true;

                    string Name = model.GetValue(iter, 1).ToString();
                    

                    if (Name.Contains(this.entry_search.Text))
                    {
                        
                        filtered.Add(iter);//*sigh*
                       
                        TreePath path = model.GetPath(iter);
                        while(path.Depth>1)
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
        

        int sortinventoryfunc(Gtk.TreeModel model, Gtk.TreeIter a, Gtk.TreeIter b)
        {
            try
            {
                int colid;
                SortType order;
                int aa = 1;
                int bb = -1;

                inventory.GetSortColumnId(out colid, out order);
                if (order == SortType.Ascending)
                {
                    aa = -1;
                    bb = 1;
                }

                // We probably want to sort my name or date, and may want to group special folders first
                InventoryBase itema = (InventoryBase)model.GetValue(a, 3);
                InventoryBase itemb = (InventoryBase)model.GetValue(b, 3);

                if (itema == null || itemb == null)
                    return 0;

                if (this.check_special_folders.Active)
                {
                    if (itema is InventoryFolder && itemb is InventoryFolder)
                    {
                        if (((InventoryFolder)itema).PreferredType != ((InventoryFolder)itemb).PreferredType)
                        {
                            // we are comparing a standard folder to a special folder
                            if (((InventoryFolder)itema).PreferredType == AssetType.Unknown)
                                return aa;

                            return bb;
                        }
                    }

                    if (this.preferedsort == foldersorttype.SORT_NAME)
                    {
                        int ret = string.Compare(itema.Name, itemb.Name);
                        if (ret == 1)
                            return aa;
                        if (ret == -1)
                            return bb;
                    }

                    if (this.preferedsort == foldersorttype.SORT_DATE)
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
            }
            catch
            {
            }

            return 0;
        }

		void populate_top_level_inv()
		{
            
				if( MainClass.client.Inventory.Store.Items!=null)
					{
						foreach(KeyValuePair <UUID, InventoryNode> kvp in MainClass.client.Inventory.Store.Items)
						{
							if(kvp.Value.Data!=null)
							{
								if(kvp.Value.Data.ParentUUID==UUID.Zero)
								{
									Gtk.TreeIter iterx = inventory.AppendValues(folder_closed, kvp.Value.Data.Name, kvp.Value.Data.UUID,null);
							        Console.Write("Creating top level folder "+kvp.Value.Data.Name+" : "+MainClass.client.Inventory.Store.Items[kvp.Value.Data.UUID].ToString());
									inventory.AppendValues(iterx, folder_closed, "Waiting...", kvp.Value.Data.UUID, null);
                                    if (kvp.Value.Data.Name == "My Inventory")
                                        TLI = iterx;
								}
								Console.Write(kvp.Value.Data.ParentUUID.ToString() +" : ");
							}
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
		
		void ondeleteasset(object o, ButtonPressEventArgs args)
		{
			 Gtk.TreeModel mod;
			Gtk.TreeIter iter;
		
			TreePath[] paths = treeview_inv.Selection.GetSelectedRows(out mod);

            foreach (TreePath path in paths)
            {
                if (mod.GetIter(out iter, path))
                {
                    InventoryBase item = (InventoryBase)mod.GetValue(iter, 3);

                    if (item is InventoryItem)
                    {
                        MessageDialog md = new MessageDialog(MainClass.win, DialogFlags.Modal, MessageType.Question, ButtonsType.YesNo, "Are you sure you wish to delete\n" + ((InventoryItem)item).Name + "to ");
                        ResponseType result = (ResponseType)md.Run();
                        if (result == ResponseType.Yes)
                        {
                            md.Destroy();
                            MainClass.client.Inventory.RemoveItem(item.UUID);
                            return;
                        }
                        md.Destroy();
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

        void Inventory_onFolderUpdated(UUID folderID,bool updated)
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
            if (args.Event.Button == 3)//Fuck this should be a define
            {
                // Do the context sensitive stuff here
                // Detect type of asset selected and show an approprate menu
                // maybe
                Gtk.TreeModel mod;
			    Gtk.TreeIter iter;
                InventoryBase item = null;

				Console.WriteLine("ROOT IS "+MainClass.client.Inventory.Store.RootFolder.UUID.ToString());

				TreePath[] paths = treeview_inv.Selection.GetSelectedRows(out mod);				
				if (paths.Length==1)
				{
					//all good and simple
                    TreeIter itera;
                    mod.GetIter(out itera, paths[0]);
					UUID ida=(UUID)mod.GetValue(itera, 2);
                    item = (InventoryBase)MainClass.client.Inventory.Store.GetNodeFor(ida).Data;
                   
				}

				if(paths.Length!=1)
				{
					bool allsame=true;
					bool wearables=true;
                    bool folders = true;
                    TreeIter itera,iterb;

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

							if(itema.GetType()!=itemb.GetType())
							{
                                allsame = false;
							}
						}
						
					}
   
					//ok if allsame==true we can allow specific extra menu options
					//or if all wearables then we can allow wearable options
					if(allsame)
					{
						mod.GetIter(out iter, paths[0]);
						UUID ida=(UUID)mod.GetValue(iter, 2);
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

						Console.WriteLine("Item is "+item.ToString()+" ID is "+item.UUID.ToString());
					
						Console.WriteLine("Item parent is "+item.ToString()+" ID is "+item.ParentUUID.ToString());
						
                        if(item is InventoryLandmark)
                        {
							Gtk.ImageMenuItem menu_tp_lm = new ImageMenuItem("Teleport to Landmark");
							menu_tp_lm.Image=new Gtk.Image(MainClass.GetResource("icon_place.tga"));
							menu_tp_lm.ButtonPressEvent += new ButtonPressEventHandler(Teleporttolandmark);
                            menu.Append(menu_tp_lm);
                        }
                       
                        if (item is InventoryFolder)
                        {
                            //Gtk.MenuItem menu_debork = new MenuItem("Debork folder");
							
                            Gtk.MenuItem menu_wear_folder = new MenuItem("Wear folder contents");
                            Gtk.ImageMenuItem menu_give_folder = new ImageMenuItem("Give folder to user");
							menu_give_folder.Image=new Gtk.Image(MainClass.GetResource("ff_edit_theirs.tga"));
                            Gtk.ImageMenuItem menu_delete_folder = new ImageMenuItem("Delete Folder");
							menu_delete_folder.Image=new Gtk.Image(MainClass.GetResource("inv_folder_trash.tga"));

                            menu_delete_folder.ButtonPressEvent += new ButtonPressEventHandler(ondeleteasset);
                            menu_give_folder.ButtonPressEvent += new ButtonPressEventHandler(ongiveasset);
                            menu_wear_folder.ButtonPressEvent += new ButtonPressEventHandler(menu_ware_ButtonPressEvent);
                            //menu_debork.ButtonPressEvent += new ButtonPressEventHandler(FixBorkedFolder);
							
							Gtk.Label x=new Gtk.Label("Folder Item");
							
							//menu.Append(menu_debork);
							menu.Append(menu_wear_folder);
                            menu.Append(menu_give_folder);
                            menu.Append(menu_delete_folder);
                        }

						if(item is InventoryNotecard)
						{
				             Gtk.MenuItem menu_read_note = new MenuItem("Open notecard");
							menu_read_note.ButtonPressEvent+= new ButtonPressEventHandler(onOpenNotecard);
							 menu.Append(menu_read_note);
							
						}

						if(item is InventoryTexture || item is InventorySnapshot)
						{
				             Gtk.MenuItem menu_view_texture = new MenuItem("View texture");
							 menu_view_texture.ButtonPressEvent+= new ButtonPressEventHandler(onViewTexture);
							 menu.Append(menu_view_texture);							
						}
						
						
                        if(item is InventoryItem)
                        {
						
						    Gtk.ImageMenuItem menu_give_item = new ImageMenuItem("Give item to user");
							menu_give_item.Image=new Gtk.Image(MainClass.GetResource("ff_edit_theirs.tga"));
							
                            Gtk.ImageMenuItem menu_delete_item = new ImageMenuItem("Delete item");
							menu_delete_item.Image=new Gtk.Image(MainClass.GetResource("inv_folder_trash.tga"));

                            menu_give_item.ButtonPressEvent += new ButtonPressEventHandler(ongiveasset);
                            menu_delete_item.ButtonPressEvent += new ButtonPressEventHandler(ondeleteasset);
            
                            menu.Append(menu_give_item);
                          
                            menu.Append(menu_delete_item);
                        }
						
						if(item is InventoryAttachment || item is InventoryObject)
						{
							Gtk.MenuItem menu_attach_item = new MenuItem("Attach (default pos)");
							menu_attach_item.ButtonPressEvent += new ButtonPressEventHandler(menu_attach_item_ButtonPressEvent);
							menu.Append(menu_attach_item);
						}

						if(item is InventoryWearable)
						{
							Gtk.MenuItem menu_attach_item = new MenuItem("Wear");
							menu_attach_item.ButtonPressEvent += new ButtonPressEventHandler(menu_wear_item_ButtonPressEvent);
							menu.Append(menu_attach_item);				
						}
						
                        menu.Popup();
                        menu.ShowAll();
						
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
                      
                        
                        this.no_items = 0;
                      //  MainClass.client.Inventory.Store.read_inventory_cache(MainClass.client.Settings.TEXTURE_CACHE_DIR+"\\"+MainClass.client.Inventory.Store.RootFolder.UUID.ToString()+".osl");
                        
                        fetcherrunning = true;
                        Thread invRunner = new Thread(new ParameterizedThreadStart(fetchinventory));
                        invthreaddata itd = new invthreaddata(MainClass.client.Inventory.Store.RootFolder.UUID, "0:0", TLI, true);
                        invRunner.Start(itd);

                        /*
                        InventoryNode root;
                        MainClass.client.Inventory.Store.Items.TryGetValue(MainClass.client.Inventory.Store.RootFolder.UUID,out root);
                        TreeIter iter;

                        iter = inventory.AppendValues(folder_open, root.Data.Name, root.Data.UUID, root);
                        IEnumerator<OpenMetaverse.InventoryNode> xx = root.Nodes.Values.GetEnumerator();
                        for (int x = 0; x < root.Nodes.Count; x++)
                        {
                            xx.MoveNext();
                            recursiveaddnode(xx.Current, iter);
                        }
                         * */
                         
                    });
                }
				 
			}
		}

        void recursiveaddnode(InventoryNode nodeparent,TreeIter iterparent)
        {

            InventoryBase ibase = nodeparent.Data;
            Gdk.Pixbuf buf = this.getprettyicon(ibase);
            TreeIter iter;
            iter = inventory.AppendValues(iterparent, buf, ibase.Name, ibase.UUID, ibase);
           
            if(nodeparent.Nodes!=null)
            {
                int count = nodeparent.Nodes.Count;
                int x;
                IEnumerator<OpenMetaverse.InventoryNode> xx=nodeparent.Nodes.Values.GetEnumerator();
                for (x = 0; x < count; x++)
                {
                    xx.MoveNext();
                    recursiveaddnode(xx.Current, iter);
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

		void onRowExpanded(object o,Gtk.RowExpandedArgs args)
		{
            if (filteractive == true)
                return;
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
                    Thread invRunner = new Thread(new ParameterizedThreadStart(UpdateRow));
                    invthreaddata x = new invthreaddata(key, filter.ConvertPathToChildPath(args.Path).ToString(), iter,false);
                    invRunner.Start(x);
                }
            }
            catch
            {

            }
		}
		
		void fetchinventory(object x)
		{
            recursion++;
            invthreaddata itd = (invthreaddata)x;
 			UUID start=itd.key;
            TreeIter iter = itd.iter;
            bool cache = itd.cacheonly;
       
             List<InventoryBase> myObjects;

            if(cache || MainClass.client.Inventory.Store.GetNodeStatus(start) == OpenMetaverse.InventoryNode.CacheState.STATE_NETWORK)
                myObjects = MainClass.client.Inventory.Store.GetContents(start);
            else
 	            myObjects = MainClass.client.Inventory.FolderContents(start, MainClass.client.Self.AgentID, true, true, InventorySortOrder.ByDate, 30000);
          
            if (myObjects == null || myObjects.Count==0)
            {
                recursion--;
                return;
            }
          
			Gtk.Application.Invoke(delegate{
				this.label_fetched.Text="fetched "+this.no_items.ToString()+" items";
			});
			List<InventoryBase> folders = new List<InventoryBase>();

            // Ok we need to find and remove the previous Waiting.... it should be the first child of the current iter

            TreePath path = inventory.GetPath(iter);
            path.Down();
            TreeIter childiter;
            inventory.GetIter(out childiter, path);
            if ("Waiting..." == (string)inventory.GetValue(childiter, 1))
            {
                inventory.Remove(ref childiter);
            }
            else
            {
                // Don't do anything else from here if we already have the folder data
                recursion--;
                return;
            }

			foreach (InventoryBase item in myObjects)
            {
                if (invmap.ContainsKey(item.UUID))
                {
                    TreeIter iterx = invmap[item.UUID];
                    InventoryBase itemx = (InventoryBase)inventory.GetValue(iterx,3);
                    if (itemx is InventoryFolder)
                    {
                        invthreaddata itd2 = new invthreaddata(item.UUID, "", iterx,cache);
                        fetchinventory((object)itd2);
                    }
                    continue;
                }

                   this.no_items++;
                   Gdk.Pixbuf buf = getprettyicon(item);
				   System.Threading.AutoResetEvent ar=new System.Threading.AutoResetEvent(false);
				
				   Gtk.Application.Invoke(delegate{
					    global_thread_tree = inventory.AppendValues(iter, buf, item.Name+ " (c)", item.UUID, item);
					    if(!invmap.ContainsKey(item.UUID))
                            invmap.Add(item.UUID,global_thread_tree);
                        
                        if (!assetmap.ContainsKey(item.UUID))
                            assetmap.Add(item.UUID, global_thread_tree);
                        ar.Set();
			        });
				
				    ar.WaitOne();
				
				Gtk.TreeIter iter2=global_thread_tree;
					
                     if (item is InventoryFolder)
                     {
                     
                         System.Threading.AutoResetEvent ar2=new System.Threading.AutoResetEvent(false);
					
					Gtk.Application.Invoke(delegate{
						                       
					inventory.AppendValues(iter2, item_object, "Waiting...", item.UUID, null);
                    ar2.Set();
					     });					     
						
					   ar2.WaitOne();
						//System.Threading.Thread.Sleep(50);
				                
					    invthreaddata itd2 = new invthreaddata(((InventoryFolder)item).UUID, "", iter2,cache);
						fetchinventory((object)itd2);
                     }
			}

            recursion--;
            if (recursion == 0)
            {
                fetcherrunning = false;
                Console.WriteLine("Fetch Complete");
            }
			return;
		}
		
        void UpdateRow(object x)
        {
	        UUID key;
            //Gtk.RowExpandedArgs args;
			Gtk.TreePath path;
			invthreaddata xx=(invthreaddata)x;
            key = ((invthreaddata)x).key;
            TreeIter incommingIter = ((invthreaddata)x).iter;
            //args = ((invthreaddata)x).args;

            if (MainClass.client.Inventory.Store.GetNodeStatus(key) == OpenMetaverse.InventoryNode.CacheState.STATE_NETWORK)
            {
                Console.WriteLine("Not fetching row for " + key.ToString() + " alreay in state network");
                //return;
            }

            List<InventoryBase> myObjects = MainClass.client.Inventory.FolderContents(key, MainClass.client.Self.AgentID, true, true, InventorySortOrder.ByDate, 30000);
			
            Gtk.Application.Invoke(delegate{			

 			string paths=((invthreaddata)x).path;
			path=new Gtk.TreePath(paths);


			incomming.Add(xx,myObjects);

			Gtk.TreeIter childiter;
             
		    path.Down();
				
            if (myObjects == null)
                return;

             foreach (InventoryBase item in myObjects)
             {
				 Gdk.Pixbuf buf = getprettyicon(item);
                 string msg="";

                 if (!assetmap.ContainsKey(item.UUID))
                 {
                     lock(MainClass.client.Appearance.Wearables.Dictionary)
                         foreach( KeyValuePair <WearableType,OpenMetaverse.AppearanceManager.WearableData> kvp in MainClass.client.Appearance.Wearables.Dictionary)
                         {
                             if (kvp.Value.Item.UUID == item.UUID)
                                 msg = " (WORN) ";
                         }


                     lock( MainClass.client.Network.CurrentSim.ObjectsPrimitives.Dictionary)
                         foreach (KeyValuePair<uint, Primitive> kvp in MainClass.client.Network.CurrentSim.ObjectsPrimitives.Dictionary)
                         {
                             if((kvp.Value.ID==item.UUID))
                                 msg = " (ATTACHED) ";
                         }

                     this.no_items++;
                     Gtk.TreeIter iter2 = inventory.AppendValues(incommingIter, buf, item.Name + msg, item.UUID, item);
                     if(!invmap.ContainsKey(item.UUID))
                         invmap.Add(item.UUID, iter2);
                     if(!assetmap.ContainsKey(item.UUID))
					   assetmap.Add(item.UUID, iter2);
					else
					{
						assetmap[item.UUID]=iter2;
					}
                     if (item is InventoryFolder)
                     {
						inventory.AppendValues(iter2, item_object, "Waiting...", item.UUID, null);
                     }
                 }
                //And tidy that waiting
               if (inventory.GetIter(out childiter, path))
               {
                  if ("Waiting..." == (string)inventory.GetValue(childiter, 1))
                  inventory.Remove(ref childiter);
               }

			}
			
});
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

            if (paths.Length > 0)
            {
                if (mod.GetIter(out iter, paths[0])!=null)
                {
					InventoryBase item = (InventoryBase)mod.GetValue(iter, 3);
					this.label_name.Text=item.Name;
                    Console.WriteLine("ITEM ID" + item.UUID.ToString() + " Parent " + item.ParentUUID.ToString());
                    if(item is InventoryFolder)
                        Console.WriteLine("Version is "+((InventoryFolder)item).Version.ToString());
                    
                    Console.WriteLine(item.ToString());
                    
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
            filtered.Clear(); //*sigh*
   
            if (this.entry_search.Text == "")
            {
                filteractive = false;
                filter.Refilter();
                return;
            }
            //This is fucking shocking
            filteractive = true;
            filter.Refilter();
            filter.Refilter(); //*sigh*
            treeview_inv.ExpandAll();

		}

        protected virtual void OnButtonFetchAllInvClicked(object sender, System.EventArgs e)
        {
            if (fetcherrunning == false)
            {
                fetcherrunning = true;
                Thread invRunner = new Thread(new ParameterizedThreadStart(fetchinventory));
                invthreaddata itd = new invthreaddata(MainClass.client.Inventory.Store.RootFolder.UUID, "0:0", TLI,false);
                invRunner.Start(itd);
            }
		}                 
	}
}
