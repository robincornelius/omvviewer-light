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
using OpenMetaverse;
using System.Collections.Generic;
using Gdk;
using Gtk;

namespace omvviewerlight
{

    public partial class invthreaddata
    {
        public UUID key;
        public RowExpandedArgs args;
		public string path;
		public invthreaddata(UUID keyx, RowExpandedArgs argsx,string pathx)
        {
            key = keyx;
            args = argsx;
			path = pathx;
		}
    }

	public partial class Inventory : Gtk.Bin
	{
  	
        Dictionary<invthreaddata, List<InventoryBase>> incomming = new Dictionary<invthreaddata, List<InventoryBase>>();  
		Dictionary<UUID, Gtk.TreeIter> assetmap = new Dictionary<UUID, Gtk.TreeIter>();
		String[] SearchFolders = { "" };
		//initialize our list to store the folder contents
		Gtk.TreeStore inventory = new Gtk.TreeStore (typeof(Gdk.Pixbuf),typeof (string), typeof (UUID),typeof(InventoryBase));		
		Gdk.Pixbuf folder_closed = new Gdk.Pixbuf("inv_folder_plain_closed.tga");
		Gdk.Pixbuf folder_open = new Gdk.Pixbuf("inv_folder_plain_open.tga");
		Gdk.Pixbuf item_landmark = new Gdk.Pixbuf("inv_item_landmark.tga");
		Gdk.Pixbuf item_animation = new Gdk.Pixbuf("inv_item_animation.tga");
		Gdk.Pixbuf item_clothing = new Gdk.Pixbuf("inv_item_clothing.tga");
        Gdk.Pixbuf item_object = new Gdk.Pixbuf("inv_item_object.tga");
        Gdk.Pixbuf item_gesture = new Gdk.Pixbuf("inv_item_gesture.tga");
        Gdk.Pixbuf item_notecard = new Gdk.Pixbuf("inv_item_notecard.tga");
        Gdk.Pixbuf item_script = new Gdk.Pixbuf("inv_item_script.tga");
        Gdk.Pixbuf item_snapshot = new Gdk.Pixbuf("inv_item_snapshot.tga");
        Gdk.Pixbuf item_sound = new Gdk.Pixbuf("inv_item_sound.tga");
        Gdk.Pixbuf item_callingcard = new Gdk.Pixbuf("inv_item_callingcard_offline.tga");

        Gdk.Pixbuf folder_texture = new Gdk.Pixbuf("inv_folder_texture.tga");
        Gdk.Pixbuf folder_sound = new Gdk.Pixbuf("inv_folder_sound.tga");
        Gdk.Pixbuf folder_animation = new Gdk.Pixbuf("inv_folder_animation.tga");
        Gdk.Pixbuf folder_gesture = new Gdk.Pixbuf("inv_folder_gesture.tga");
        Gdk.Pixbuf folder_snapshot = new Gdk.Pixbuf("inv_folder_snapshot.tga");
        Gdk.Pixbuf folder_trash = new Gdk.Pixbuf("inv_folder_trash.tga");
        Gdk.Pixbuf folder_notecard = new Gdk.Pixbuf("inv_folder_notecard.tga");
        Gdk.Pixbuf folder_script = new Gdk.Pixbuf("inv_folder_script.tga");
        Gdk.Pixbuf folder_lostandfound = new Gdk.Pixbuf("inv_folder_lostandfound.tga");
        Gdk.Pixbuf folder_landmark = new Gdk.Pixbuf("inv_folder_landmark.tga");
        Gdk.Pixbuf folder_bodypart = new Gdk.Pixbuf("inv_folder_bodypart.tga");
        Gdk.Pixbuf folder_callingcard = new Gdk.Pixbuf("inv_folder_callingcard.tga");
        Gdk.Pixbuf folder_clothing = new Gdk.Pixbuf("inv_folder_clothing.tga");


        public void kill()
        {
            this.treeview_inv.RowExpanded -= new Gtk.RowExpandedHandler(onRowExpanded);
            this.treeview_inv.RowCollapsed -= new Gtk.RowCollapsedHandler(onRowCollapsed);
            MainClass.client.Network.OnLogin -= new OpenMetaverse.NetworkManager.LoginCallback(onLogin);
            this.treeview_inv.ButtonPressEvent -= new ButtonPressEventHandler(treeview_inv_ButtonPressEvent);
            MainClass.client.Inventory.OnObjectOffered -= new InventoryManager.ObjectOfferedCallback(Inventory_OnObjectOffered);
    
            Gtk.Notebook p;
            p = (Gtk.Notebook)this.Parent;
            p.RemovePage(p.PageNum(this));
        }

		public Inventory()
		{
			this.Build();		
			treeview_inv.AppendColumn("",new CellRendererPixbuf(),"pixbuf",0);
			treeview_inv.AppendColumn("Name",new  Gtk.CellRendererText(),"text",1);
			treeview_inv.Model=inventory;
			this.treeview_inv.RowExpanded += new Gtk.RowExpandedHandler(onRowExpanded);
			this.treeview_inv.RowCollapsed += new Gtk.RowCollapsedHandler(onRowCollapsed);
			MainClass.client.Network.OnLogin += new OpenMetaverse.NetworkManager.LoginCallback(onLogin);
            this.treeview_inv.ButtonPressEvent += new ButtonPressEventHandler(treeview_inv_ButtonPressEvent);

            MainClass.client.Inventory.OnObjectOffered += new InventoryManager.ObjectOfferedCallback(Inventory_OnObjectOffered);
          
			this.label_aquired.Text="";
			this.label_createdby.Text="";
			this.label_name.Text="";
			this.label_group.Text="";
			this.label_saleprice.Text="";

            if (MainClass.client != null)
            {
                if (MainClass.client.Network.LoginStatusCode == OpenMetaverse.LoginStatus.Success)
                {
                    inventory.Clear();
                    Gtk.TreeIter iter = inventory.AppendValues(folder_closed, "Inventory", MainClass.client.Inventory.Store.RootFolder.UUID);
                    inventory.AppendValues(iter, folder_closed, "Waiting...", MainClass.client.Inventory.Store.RootFolder.UUID, null);
                }
            }	
		}

        void menu_ware_ButtonPressEvent(object o, ButtonPressEventArgs args)
        {
            Gtk.TreeModel mod;
            Gtk.TreeIter iter;
            this.treeview_inv.Selection.GetSelected(out mod, out iter);
            InventoryBase item = (InventoryBase)mod.GetValue(iter, 3);

            if (item is InventoryFolder)
            {
                MainClass.client.Appearance.WearOutfit(item.UUID,true);
            }

           // MainClass.client.Appearance.Attach(

        }
		
		void ondeleteasset(object o, ButtonPressEventArgs args)
		{
			Gtk.TreeModel mod;
            Gtk.TreeIter iter;
            this.treeview_inv.Selection.GetSelected(out mod, out iter);
            InventoryBase item = (InventoryBase)mod.GetValue(iter, 3);
			
			if(item is InventoryItem)
			{			
				MessageDialog md = new MessageDialog(MainClass.win,DialogFlags.Modal,MessageType.Question,ButtonsType.YesNo,"Are you sure you wish to delete\n"+((InventoryItem)item).Name+"to ");
				ResponseType result=(ResponseType)md.Run();	
				if(result==ResponseType.Yes)
				{
					md.Destroy();
					MainClass.client.Inventory.RemoveItem(item.UUID);
					return;
				}
					md.Destroy();				
			}
		}
		
		void ongiveasset(object o, ButtonPressEventArgs args)
		{
			Gtk.TreeModel mod;
            Gtk.TreeIter iter;
            this.treeview_inv.Selection.GetSelected(out mod, out iter);
            InventoryBase item = (InventoryBase)mod.GetValue(iter, 3);
			NamePicker np = new NamePicker();
			
			if(item is InventoryItem)
				np.item_name=((InventoryItem)item).Name;
					
			np.asset=item.UUID;
			
			np.UserSel += new NamePicker.UserSelected(ongiveasset2);
			np.Show();
			
		}
		
		void ongiveasset2(UUID id,UUID asset,string item_name,string user_name)
		{
			MessageDialog md = new MessageDialog(MainClass.win,DialogFlags.Modal,MessageType.Question,ButtonsType.YesNo,"Are you sure you wish to give\n"+item_name+"to "+user_name);
			ResponseType result=(ResponseType)md.Run();	
			md.Destroy();
			
			if(result==ResponseType.Yes)
			{
				MainClass.client.Inventory.GiveItem(asset,item_name,AssetType.Landmark,id,false);
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

        void Inventory_OnFolderUpdated(UUID folderID)
        {
            
        }

        bool Inventory_OnObjectOffered(InstantMessage offerDetails, AssetType type, UUID objectID, bool fromTask)
        {
			
			AutoResetEvent ObjectOfferEvent = new AutoResetEvent(false);
			ResponseType object_offer_result=ResponseType.Yes;

            string msg = "";
			ResponseType result;
            if (!fromTask)
                msg = "The user "+offerDetails.FromAgentName + " has offered you\n" + offerDetails.Message + "\n Which is a " + type.ToString() + "\nPress Yes to accept or no to decline";
            else
                msg = "The object "+offerDetails.FromAgentName + " has offered you\n" + offerDetails.Message + "\n Which is a " + type.ToString() + "\nPress Yes to accept or no to decline";

			
			Application.Invoke(delegate {			
					ObjectOfferEvent.Reset();

					Gtk.MessageDialog md = new MessageDialog(MainClass.win, DialogFlags.Modal, MessageType.Other, ButtonsType.YesNo, false, msg);
				
					result = (ResponseType)md.Run();
					object_offer_result=result;
				    md.Destroy();
					ObjectOfferEvent.Set();
			});
			
			
		    ObjectOfferEvent.WaitOne(1000*3600,false);
	
           if (object_offer_result == ResponseType.Yes)
		   {
				return true;
		   }
		   else
		{
			    return false;
			}			
        }

        void Teleporttolandmark(object o, ButtonPressEventArgs args)
        {
            Gtk.TreeModel mod;
            Gtk.TreeIter iter;
            this.treeview_inv.Selection.GetSelected(out mod, out iter);
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

               
                if (this.treeview_inv.Selection.GetSelected(out mod, out iter))
                {   
					if(mod.GetValue(iter,3)!=null)
                    {
                        Gtk.Menu menu = new Gtk.Menu();
    
                        InventoryBase item = (InventoryBase)mod.GetValue(iter, 3);

                        if(item is InventoryLandmark)
                        {
							Gtk.ImageMenuItem menu_tp_lm = new ImageMenuItem("Teleport to Landmark");
							menu_tp_lm.Image=new Gtk.Image("icon_place.tga");
							menu_tp_lm.ButtonPressEvent += new ButtonPressEventHandler(Teleporttolandmark);
                            menu.Append(menu_tp_lm);
                        }
                       
                        if (item is InventoryFolder)
                        {
                            Gtk.MenuItem menu_wear_folder = new MenuItem("Wear folder contents");
                            Gtk.ImageMenuItem menu_give_folder = new ImageMenuItem("Give folder to user");
							menu_give_folder.Image=new Gtk.Image("ff_edit_theirs.tga");
                            Gtk.ImageMenuItem menu_delete_folder = new ImageMenuItem("Delete Folder");
							menu_delete_folder.Image=new Gtk.Image("inv_folder_trash.tga");

							
                            menu_delete_folder.ButtonPressEvent += new ButtonPressEventHandler(ondeleteasset);
                            menu_give_folder.ButtonPressEvent += new ButtonPressEventHandler(ongiveasset);
                            menu_wear_folder.ButtonPressEvent += new ButtonPressEventHandler(menu_ware_ButtonPressEvent);
                            
							Gtk.Label x=new Gtk.Label("Folder Item");
							
							menu.Append(menu_wear_folder);
                            menu.Append(menu_give_folder);
                            menu.Append(menu_delete_folder);
                        }

                        if(item is InventoryItem)
                        {
						
						    Gtk.ImageMenuItem menu_give_item = new ImageMenuItem("Give item to user");
							menu_give_item.Image=new Gtk.Image("ff_edit_theirs.tga");
							
                            Gtk.ImageMenuItem menu_delete_item = new ImageMenuItem("Delete item");
							menu_delete_item.Image=new Gtk.Image("inv_folder_trash.tga");

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

                        menu.Popup();
                        menu.ShowAll();
						
                    }
                }
            }
        }

        void menu_attach_item_ButtonPressEvent(object o, ButtonPressEventArgs args)
        {
            Gtk.TreeModel mod;
            Gtk.TreeIter iter;
            this.treeview_inv.Selection.GetSelected(out mod, out iter);
            InventoryBase item = (InventoryBase)mod.GetValue(iter, 3);
            MainClass.client.Appearance.Attach((InventoryItem)item, AttachmentPoint.Default);
        }

        void onLogin(LoginStatus status, string message)
		{
			if(LoginStatus.Success==status)
			{
				Gtk.Application.Invoke(delegate {
					inventory.Clear();
					Gtk.TreeIter iter = inventory.AppendValues(folder_closed,"Inventory", MainClass.client.Inventory.Store.RootFolder.UUID);
					inventory.AppendValues(iter,folder_closed, "Waiting...", MainClass.client.Inventory.Store.RootFolder.UUID,null);		
				});
			}
		}
				
		void onRowCollapsed(object o,Gtk.RowCollapsedArgs args)
		{
			Gdk.Pixbuf image=folder_closed;
			
			UUID key=(UUID)this.inventory.GetValue(args.Iter,2);
			InventoryBase item=(InventoryBase)this.inventory.GetValue(args.Iter,3);
            if (item == null)
                return;
			if(item is InventoryFolder)
			{
				image=getprettyfoldericon((InventoryFolder)item);	
			}
			
			inventory.SetValue(args.Iter,0,item);
		}

		void onRowExpanded(object o,Gtk.RowExpandedArgs args)
		{
			UUID key=(UUID)this.inventory.GetValue(args.Iter,2);
			if(inventory.GetValue(args.Iter,0)==folder_closed)
                inventory.SetValue(args.Iter,0,folder_open);
 
            Console.Write("Trying to get child iter\n");

           // this.treeview_inv.QueueDraw();
            Thread invRunner = new Thread(new ParameterizedThreadStart(UpdateRow));
            invthreaddata x = new invthreaddata(key,args,args.Path.ToString());

		    Console.Write("Key is NOW "+key.ToString()+"\n");
		    Console.Write("Path is NOW "+args.Path.ToString()+"\n");
			Console.Write("ARgs is now"+args.ToString()+"\n"+"\n");        
			
            MainClass.client.Inventory.RequestFolderContents(key, MainClass.client.Self.AgentID, true, true, InventorySortOrder.ByDate);

			invRunner.Start(x);
   		
		}

        void UpdateRow(object x)
        {
	        UUID key;
            Gtk.RowExpandedArgs args;
			Gtk.TreePath path;
			invthreaddata xx=(invthreaddata)x;
    
            key = ((invthreaddata)x).key;
            args = ((invthreaddata)x).args;
			string paths=((invthreaddata)x).path;
			path=new Gtk.TreePath(paths);

	        List<InventoryBase> myObjects = MainClass.client.Inventory.FolderContents(key, MainClass.client.Self.AgentID, true, true, InventorySortOrder.ByDate, 30000);

			incomming.Add(xx,myObjects);

			Gtk.TreeIter childiter;
             
            Console.Write("Path is "+path.ToString()+"\n");
		    
		    path.Down();
	        Console.Write("Path down is "+path.ToString()+"\n");
				

             if (myObjects == null)
                return;

             foreach (InventoryBase item in myObjects)
             {
				 Console.Write("Adding item "+item.ToString()+"\n");
				 Gdk.Pixbuf buf = getprettyicon(item);
				
                 if (!assetmap.ContainsKey(item.UUID))
                 {
                     Gtk.TreeIter iter2 = inventory.AppendValues(args.Iter, buf, item.Name, item.UUID, item);
                     assetmap.Add(item.UUID, iter2);
                     if (item is InventoryFolder)
                     {
                          inventory.AppendValues(iter2, item_object, "Waiting...", UUID.Zero, null);
                     }
                 }
                //And tidy that waiting
               if (inventory.GetIter(out childiter, path))
               {
                  Console.Write("We got a childiter for that\n");
                  Console.Write("Value is =" + (string)inventory.GetValue(childiter, 1) + "\n");
                  if ("Waiting..." == (string)inventory.GetValue(childiter, 1))

                  inventory.Remove(ref childiter);
               }

			 }
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
				return this.item_clothing;

            if (item is OpenMetaverse.InventoryGesture)
                return this.item_gesture;

            if (item is OpenMetaverse.InventoryNotecard)
                return this.item_notecard;

            if (item is OpenMetaverse.InventoryLSL)
                return this.item_script;

            if (item is OpenMetaverse.InventorySnapshot)
                return this.item_snapshot;

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
			
			if(this.treeview_inv.Selection.GetSelected(out mod,out iter))			
			{
				 if(mod.GetValue(iter,3)!=null)
                 {
					InventoryBase item = (InventoryBase)mod.GetValue(iter, 3);
					this.label_name.Text=item.Name;
					
					if(item is InventoryItem)
					{
						this.label_createdby.Text=((InventoryItem)item).CreatorID.ToString();
						this.label_aquired.Text=((InventoryItem)item).CreationDate.ToString();
						this.checkbutton_copy.Active=OpenMetaverse.PermissionMask.Copy==(((InventoryItem)item).Permissions.OwnerMask&OpenMetaverse.PermissionMask.Copy);
						this.checkbutton_mod.Active=OpenMetaverse.PermissionMask.Modify==(((InventoryItem)item).Permissions.OwnerMask&OpenMetaverse.PermissionMask.Modify);
						this.checkbutton_trans.Active=OpenMetaverse.PermissionMask.Transfer==(((InventoryItem)item).Permissions.OwnerMask&OpenMetaverse.PermissionMask.Transfer);
					
						this.checkbutton_copynext.Active=OpenMetaverse.PermissionMask.Copy==(((InventoryItem)item).Permissions.NextOwnerMask&OpenMetaverse.PermissionMask.Copy);
						this.checkbutton_modnext.Active=OpenMetaverse.PermissionMask.Modify==(((InventoryItem)item).Permissions.NextOwnerMask&OpenMetaverse.PermissionMask.Modify);
						this.checkbutton_transnext.Active=OpenMetaverse.PermissionMask.Transfer==(((InventoryItem)item).Permissions.NextOwnerMask&OpenMetaverse.PermissionMask.Transfer);
					
						AsyncNameUpdate ud=new AsyncNameUpdate((UUID)((InventoryItem)item).CreatorID.ToString(),false);
						ud.onNameCallBack += delegate(string name,object[] values){this.label_createdby.Text=name;};
                        ud.go();

						AsyncNameUpdate ud2=new AsyncNameUpdate(((InventoryItem)item).GroupID,true);
						ud2.onNameCallBack += delegate(string name,object[] values){this.label_group.Text=name;};
                        ud2.go();

						this.label_saleprice.Text=((InventoryItem)item).SalePrice.ToString();
						
			
					}
		
				 }
				
				
			}
		}
		
		
		
	}
}
