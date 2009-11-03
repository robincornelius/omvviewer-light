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

// ObjectsLayout.cs created with MonoDevelop
// User: robin at 13:32 14/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;
using OpenMetaverse;
using Gtk;
using Gdk;

namespace omvviewerlight
{

	[System.ComponentModel.ToolboxItem(true)]	
	public partial class ObjectsLayout : Gtk.Bin
	{

        Gdk.Pixbuf img_blank;
        Gdk.Pixbuf img_script;
        Gdk.Pixbuf img_touch;
        Gdk.Pixbuf img_phantom;
        Gdk.Pixbuf img_physical;
        Gdk.Pixbuf img_pay;
        Gdk.Pixbuf img_owner;
        Gdk.Pixbuf img_officer;
        Gdk.Pixbuf img_group;
        Gdk.Pixbuf img_move;
        Gdk.Pixbuf img_mod;
        Gdk.Pixbuf img_trans;


        bool sat = false;
		Gtk.ListStore store;	
        Dictionary<UUID, Primitive> PrimsWaiting = new Dictionary<UUID, Primitive>();
        Dictionary<UUID, Primitive> FetchedPrims = new Dictionary<UUID, Primitive>();
		
		~ObjectsLayout()
		{
			Console.WriteLine("ObjectsLayout Cleaned up");
		}		

        new public void Dispose()
        {
          
            AutoPilot.onAutoPilotFinished-=new AutoPilot.AutoPilotFinished(onAutoPilotFinished);
            MainClass.onRegister -= new MainClass.register(MainClass_onRegister);
            MainClass.onDeregister -= new MainClass.deregister(MainClass_onDeregister);
            MainClass_onDeregister();

            Gtk.Notebook p;
            p = (Gtk.Notebook)this.Parent;
            p.RemovePage(p.PageNum(this));

		}

		public ObjectsLayout()
		{
			this.Build();
            store = new Gtk.ListStore(typeof(string), typeof(string), typeof(string), typeof(UUID), typeof(ulong), typeof(Gdk.Pixbuf), typeof(Gdk.Pixbuf), typeof(Gdk.Pixbuf), typeof(Gdk.Pixbuf), typeof(Gdk.Pixbuf), typeof(Gdk.Pixbuf), typeof(Gdk.Pixbuf), typeof(Gdk.Pixbuf), typeof(Gdk.Pixbuf));
        
            MyTreeViewColumn mycol;
            mycol = new MyTreeViewColumn("Name",new Gtk.CellRendererText(), "text", 0,true);
            mycol.setmodel(store);
            treeview1.AppendColumn(mycol);

            mycol = new MyTreeViewColumn("Desc", new Gtk.CellRendererText(), "text", 1,false);
            mycol.setmodel(store);
            treeview1.AppendColumn(mycol);

            mycol = new MyTreeViewColumn("Distance", new Gtk.CellRendererText(), "text", 2,false);
            mycol.setmodel(store);
            treeview1.AppendColumn(mycol);

            mycol = new MyTreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 5, false);
            mycol.setmodel(store);
            treeview1.AppendColumn(mycol);
            mycol = new MyTreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 6, false);
            mycol.setmodel(store);
            treeview1.AppendColumn(mycol);
            mycol = new MyTreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 7, false);
            mycol.setmodel(store);
            treeview1.AppendColumn(mycol);
            mycol = new MyTreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 8, false);
            mycol.setmodel(store);
            treeview1.AppendColumn(mycol);
            mycol = new MyTreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 9, false);
            mycol.setmodel(store);
            treeview1.AppendColumn(mycol);
            mycol = new MyTreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 10, false);
            mycol.setmodel(store);
            treeview1.AppendColumn(mycol);
            mycol = new MyTreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 11, false);
            mycol.setmodel(store);
            treeview1.AppendColumn(mycol);
            mycol = new MyTreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 12, false);
            mycol.setmodel(store);
            treeview1.AppendColumn(mycol);

            mycol = new MyTreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 13, false);
            mycol.setmodel(store);
            treeview1.AppendColumn(mycol);

            
            treeview1.Model=store;
            this.store.SetSortFunc(2, sort_Vector3);
            store.SetSortColumnId(2, Gtk.SortType.Ascending);

		    this.label_desc.Text="";
			this.label_forsale.Text="";
			this.label_group.Text="";
			this.label_name.Text="";
			this.label_owner.Text="";
		    this.label_key.Text="";
			this.label_distance.Text="";
			this.label_pos.Text="";
		    this.label_float_text.Text="";
            treeview1.HeadersClickable = true;

            img_blank = MainClass.GetResource("blank_arrow.png");
            img_script = MainClass.GetResource("inv_item_script.png");
            img_touch = MainClass.GetResource("touch.png");
            img_pay= MainClass.GetResource("status_money.png");
            img_phantom = MainClass.GetResource("phantom.png");
            img_physical = MainClass.GetResource("inv_item_object.png");
            img_owner = MainClass.GetResource("icn_voice-pvtfocus.png");
            img_officer = MainClass.GetResource("icn_voice-groupfocus2.png");
            img_group = MainClass.GetResource("icn_voice-groupfocus.png");

            img_move = MainClass.GetResource("move.png");
            img_mod = MainClass.GetResource("ff_edit_mine.png");
            img_trans = MainClass.GetResource("ff_edit_theirs.png");

          			
	    
            AutoPilot.onAutoPilotFinished += new AutoPilot.AutoPilotFinished(onAutoPilotFinished);
        
            MainClass.onRegister += new MainClass.register(MainClass_onRegister);
            MainClass.onDeregister += new MainClass.deregister(MainClass_onDeregister);
            if(MainClass.client != null ) { MainClass_onRegister(); }

        
        }

        void MainClass_onDeregister()
        {
            store.Clear();
            PrimsWaiting.Clear();
            FetchedPrims.Clear();
            if (MainClass.client != null)
            {
                MainClass.client.Objects.ObjectProperties -= new EventHandler<ObjectPropertiesEventArgs>(Objects_ObjectProperties);
                MainClass.client.Groups.GroupNamesReply -= new EventHandler<GroupNamesEventArgs>(Groups_GroupNamesReply);
                MainClass.client.Self.AvatarSitResponse -= new EventHandler<AvatarSitResponseEventArgs>(Self_AvatarSitResponse);
                MainClass.client.Objects.ObjectUpdate -= new EventHandler<PrimEventArgs>(Objects_ObjectUpdate);
            }
        }

        void MainClass_onRegister()
        {
            MainClass.client.Objects.ObjectProperties += new EventHandler<ObjectPropertiesEventArgs>(Objects_ObjectProperties);
            MainClass.client.Groups.GroupNamesReply += new EventHandler<GroupNamesEventArgs>(Groups_GroupNamesReply);
            MainClass.client.Self.AvatarSitResponse += new EventHandler<AvatarSitResponseEventArgs>(Self_AvatarSitResponse);
            MainClass.client.Objects.ObjectUpdate += new EventHandler<PrimEventArgs>(Objects_ObjectUpdate);
        }

        void Objects_ObjectUpdate(object sender, PrimEventArgs e)
        {
            if (e.Prim.LocalID == MainClass.client.Self.LocalID)
            {
                if (MainClass.client.Network.CurrentSim.ObjectsAvatars[MainClass.client.Self.LocalID].ParentID == 0)
                {
                    //Console.WriteLine("** Update is " + update.ToString() + " \nbyes are " + update.State.ToString() + "\n parent is " + MainClass.client.Network.CurrentSim.ObjectsAvatars[MainClass.client.Self.LocalID].ParentID);
                    if (sat == true)
                    {
                        sat = false;
                        Gtk.Application.Invoke(delegate
                        {
                            this.button_siton.Label = "Sit";
                        });
                    }
                }
            }
        }

        void Self_AvatarSitResponse(object sender, AvatarSitResponseEventArgs e)
        {
            Gtk.Application.Invoke(delegate
            {
                this.button_siton.Label = "Stand";
                sat = true;
            });
        }

        int sort_Vector3(Gtk.TreeModel model, Gtk.TreeIter a, Gtk.TreeIter b)
        {

            string distAs = (string)store.GetValue(a, 2);
            string distBs = (string)store.GetValue(b, 2);
            float distA, distB;

            float.TryParse(distAs, out distA);
            float.TryParse(distBs, out distB);

            //Console.Write("Testing " + distA.ToString() + " vs " + distB.ToString() + "\n");

            if (distAs == distBs)
                return 0;

            if (distAs == "NaN")
                return 1;

            if (distBs == "NaN")
                return -1;


            if (distA > distB)
                return 1;

            if (distA < distB)
                return -1;

            return 0;
        }

        void setupsort(int selcol)
        {

            int col;
            Gtk.SortType order;

            this.store.GetSortColumnId(out col, out order);
            if(col==selcol)
            {
                if(order==Gtk.SortType.Ascending)
                    this.store.SetSortColumnId(selcol,Gtk.SortType.Descending);
                else
                    this.store.SetSortColumnId(selcol,Gtk.SortType.Ascending);

                return;
            }

            this.store.SetSortColumnId(selcol, Gtk.SortType.Ascending);

        }

        void name_col_Clicked(object sender, EventArgs e)
        {
            setupsort(0);
        }

        void dist_col_Clicked(object sender, EventArgs e)
        {
            setupsort(2);
        }

        void desc_col_Clicked(object sender, EventArgs e)
        {
            setupsort(1);
        }
			
        void Groups_GroupNamesReply(object sender, GroupNamesEventArgs e)
	    {
			Gtk.TreeModel mod;
			Gtk.TreeIter iter;
			
			if(treeview1.Selection.GetSelected(out mod,out iter))			
			{
				UUID id=(UUID)mod.GetValue(iter,3);
				Primitive prim;
				
				if(FetchedPrims.TryGetValue(id,out prim))
				{
			
					string group;;
					if(MainClass.client.Groups.GroupName2KeyCache.TryGetValue(prim.Properties.GroupID,out group))
					{
						Gtk.Application.Invoke(delegate {						
							this.label_group.Text=group;
						});
					}					
				}
			}	
		}

		protected virtual void OnButtonSearchClicked (object sender, System.EventArgs e)
		{
			int radius;
			int.TryParse(this.entry1.Text,out radius);
			store.Clear();
			// *** get current location ***
            Vector3d location = MainClass.client.Self.GlobalPosition;
            List<Primitive> prims=null;


            lock (PrimsWaiting)
            {
                PrimsWaiting.Clear();
                FetchedPrims.Clear();
            }
            // *** find all objects in radius ***
            lock (MainClass.client.Network.Simulators)
            {
                foreach (Simulator sim in MainClass.client.Network.Simulators)
                {
                    prims = sim.ObjectsPrimitives.FindAll(
                        delegate(Primitive prim)
                        {
                            Vector3d pos=AutoPilot.localtoglobalpos(prim.Position, sim.Handle);
                            return ((prim.ParentID == 0) && (pos != Vector3d.Zero) && (Vector3d.Distance(pos, location) < radius));
                        }
                    );

                    if (prims != null)
					{
						Console.WriteLine("Requesting properties for "+prims.Count.ToString()+" objects");
                        RequestObjectProperties(prims, 250,sim);
					}
                }
            }

           
			
		}

        private void RequestObjectProperties(List<Primitive> objects, int msPerRequest, Simulator sim)
        {
            // Create an array of the local IDs of all the prims we are requesting properties for
            uint[] localids = new uint[objects.Count];
			lock (FetchedPrims) 
			{

				lock (PrimsWaiting)
				{
                  	for (int i = 0; i < objects.Count; ++i) {
						localids[i] = objects[i].LocalID;
                        PrimsWaiting[objects[i].ID]=objects[i];
 					}
				}
			}

            MainClass.client.Objects.SelectObjects(sim, localids);

            //return AllPropertiesReceived.WaitOne(2000 + msPerRequest * objects.Count, false);
        }

        void Objects_ObjectProperties(object sender, ObjectPropertiesEventArgs e)
        {
            try
            {
                lock (PrimsWaiting)
                {
                    Primitive prim;
                    if (PrimsWaiting.TryGetValue(e.Properties.ObjectID, out prim))
                    {
                        prim.Properties = e.Properties;
                        Gtk.Application.Invoke(delegate
                        {
                            Vector3 self_pos;
                            lock (MainClass.client.Network.CurrentSim.ObjectsAvatars)
                            {
                                // Cope if *we* are sitting on someting
                                if (MainClass.client.Network.CurrentSim.ObjectsAvatars[MainClass.client.Self.LocalID].ParentID != 0)
                                {
                                    Primitive parent = MainClass.client.Network.CurrentSim.ObjectsPrimitives[MainClass.client.Network.CurrentSim.ObjectsAvatars[MainClass.client.Self.LocalID].ParentID];
                                    self_pos = Vector3.Transform(MainClass.client.Self.RelativePosition, Matrix4.CreateFromQuaternion(parent.Rotation)) + parent.Position;
                                }
                                else
                                {
                                    self_pos = MainClass.client.Self.RelativePosition;
                                }
                            }

                            PrimsWaiting.Remove(e.Properties.ObjectID);
                            if (FetchedPrims.ContainsKey(e.Properties.ObjectID))
                            {
                                Console.WriteLine("Trying to add a duplicate prim to FetchedPrims WTF? " + e.Properties.ObjectID.ToString());
                                return;
                            }
                            else
                            {
                                FetchedPrims.Add(e.Properties.ObjectID, prim);
                            }
                            Gtk.Application.Invoke(delegate
                            {
                                Gdk.Pixbuf owner = this.img_blank;

                                if ((prim.Flags & PrimFlags.ObjectYouOwner) == PrimFlags.ObjectYouOwner)
                                    owner = this.img_owner;
                                if ((prim.Flags & PrimFlags.ObjectYouOfficer) == PrimFlags.ObjectYouOfficer)
                                    owner = this.img_officer;
                                if ((prim.Flags & PrimFlags.ObjectGroupOwned) == PrimFlags.ObjectGroupOwned)
                                    owner = this.img_group;
                               

                                store.AppendValues(prim.Properties.Name, prim.Properties.Description, MainClass.cleandistance(Vector3d.Distance(AutoPilot.localtoglobalpos(prim.Position, e.Simulator.Handle), AutoPilot.localtoglobalpos(self_pos, MainClass.client.Network.CurrentSim.Handle)).ToString(),2), prim.Properties.ObjectID, e.Simulator.Handle,
                                    owner,
                                    (prim.Flags & PrimFlags.Money) == PrimFlags.Money ? this.img_pay : this.img_blank,
                                    (prim.Flags & PrimFlags.Touch) == PrimFlags.Touch ? this.img_touch : this.img_blank,
                                    (prim.Flags & PrimFlags.Scripted) == PrimFlags.Scripted ? this.img_script : this.img_blank,
                                    (prim.Flags & PrimFlags.Phantom) == PrimFlags.Phantom ? this.img_phantom : this.img_blank,
                                    (prim.Flags & PrimFlags.Physics) == PrimFlags.Physics ? this.img_physical : this.img_blank,
                                    (prim.Flags & PrimFlags.ObjectMove) == PrimFlags.ObjectMove ? this.img_move : this.img_blank,
                                    (prim.Flags & PrimFlags.ObjectModify) == PrimFlags.ObjectModify ? this.img_mod : this.img_blank,
                                    (prim.Flags & PrimFlags.ObjectTransfer) == PrimFlags.ObjectTransfer ? this.img_trans : this.img_blank
                           
                                    );
                            });
                        });

                    }
                }
            }
            catch(Exception ee)
            {
                // General messups, we can get here if we try to scan for objects too soon after login and ourself is not
                // in the dictionary yet. Can't be bothered to test *every* object property if we are valid so just catch the exception
                Logger.Log("Error: Scanning for object triggered exception: "+ee.Message, Helpers.LogLevel.Error);
            }
        }

		protected virtual void OnTreeview1CursorChanged (object sender, System.EventArgs e)
		{
			Gtk.TreeModel mod;
			Gtk.TreeIter iter;

			this.button_lookat.Sensitive=true;
			this.button_siton.Sensitive=true;
						
			if(treeview1.Selection.GetSelected(out mod,out iter))			
			{
				UUID id=(UUID)mod.GetValue(iter,3);
                ulong handle=(ulong)mod.GetValue(iter,4);
		
				Primitive prim;
                
				if(FetchedPrims.TryGetValue(id,out prim))
				{
				Console.WriteLine(prim.ToString());

					this.label_name.Text=prim.Properties.Name;
					this.label_desc.Text=prim.Properties.Description;
									
					AsyncNameUpdate ud=new AsyncNameUpdate(prim.Properties.OwnerID,false);  
					ud.onNameCallBack += delegate(string namex,object[] values){label_owner.Text=namex;};
					ud.go();
					
                    string group;
					if(!MainClass.client.Groups.GroupName2KeyCache.TryGetValue(prim.Properties.GroupID,out group))
					{
						MainClass.client.Groups.RequestGroupName(prim.Properties.GroupID);
						group="Waiting...";	
					}
					
					this.label_group.Text=group;
					
					switch(prim.Properties.SaleType)
					{
					case OpenMetaverse.SaleType.Not: 
						this.label_forsale.Text="Not for sale";
						break;
				
					case OpenMetaverse.SaleType.Contents: 
						this.label_forsale.Text="Contents for $L"+prim.Properties.SalePrice.ToString();
						break;

					case OpenMetaverse.SaleType.Copy: 
						this.label_forsale.Text="Copy for $L"+prim.Properties.SalePrice.ToString();	
						break;
					
					case OpenMetaverse.SaleType.Original: 
						this.label_forsale.Text="Original for $L"+prim.Properties.SalePrice.ToString();	
						break;	
					}

                    this.button_lookat.Sensitive = true;

					if(prim.Properties.SaleType == OpenMetaverse.SaleType.Not)
					{
						this.button_buy.Sensitive=false;
					}
					else
					{
						this.button_buy.Sensitive=true;						
					}

                    if ((prim.Flags & PrimFlags.Touch) == PrimFlags.Touch)
					{
						this.button_touch.Sensitive=true;
					}
					else
					{
						this.button_touch.Sensitive=false;
					}

                    if ((prim.Flags & PrimFlags.Money) == PrimFlags.Money)
					{
						this.button_pay.Sensitive=true;
					}
					else
					{
						this.button_pay.Sensitive=false;
					}
					
					this.label_key.Text=prim.ID.ToString();
				
					this.label_pos.Text=prim.Position.ToString();
				
					
					this.label_distance.Text=Vector3d.Distance(MainClass.client.Self.GlobalPosition,AutoPilot.localtoglobalpos(prim.Position,handle)).ToString()+" m";
				
					
					this.button_take_copy.Sensitive=((prim.Flags & PrimFlags.ObjectCopy)==PrimFlags.ObjectCopy);
					
					Console.WriteLine(prim.Flags.ToString());
					this.textview1.Buffer.SetText(prim.Flags.ToString());
				
					if((prim.Flags & PrimFlags.ObjectYouOwner)==PrimFlags.ObjectYouOwner)
					{
						this.button_take.Sensitive=true;
					}
					else
					{
						this.button_take.Sensitive=false;
					}
					
					this.label_float_text.Text=prim.Text;
					
					this.button_moveto.Sensitive=true;
					
					
					
					//MainClass.client.Parcels.ReturnObjects(MainClass.client.Network.CurrentSim,,ObjectReturnType.Other,
				
					bool allowed=false;
				
					uint x,y;
					x=(uint)(64*(prim.Position.X/256));
					y=(uint)(64*(prim.Position.Y/256));
							
					int parcelid=MainClass.client.Network.CurrentSim.ParcelMap[y,x];

					//If avatar owns the parcel they are allowed.
					//If they are in the group that owns the parcel AND have the correct group permissions AND have the group tag they are allowed
					if(MainClass.client.Network.CurrentSim.Parcels.ContainsKey(parcelid))
					{
						Parcel parcel;
						parcel=MainClass.client.Network.CurrentSim.Parcels[parcelid];
					
						if(parcel.OwnerID==MainClass.client.Self.AgentID)
							allowed=true;
						
						if (parcel.OwnerID == MainClass.client.Self.ActiveGroup || parcel.GroupID==MainClass.client.Self.ActiveGroup)
						{
							 if(prim.GroupID==parcel.GroupID)
								if((MainClass.client.Self.ActiveGroupPowers & GroupPowers.ReturnGroupSet)==GroupPowers.ReturnGroupOwned)
									allowed=true;
							
							 if(prim.OwnerID==parcel.GroupID)
								if((MainClass.client.Self.ActiveGroupPowers & GroupPowers.ReturnGroupOwned)==GroupPowers.ReturnGroupOwned)
									allowed=true;
							
							if(prim.OwnerID!=parcel.GroupID && prim.GroupID!=parcel.GroupID)
								if((MainClass.client.Self.ActiveGroupPowers & GroupPowers.ReturnNonGroup)==GroupPowers.ReturnNonGroup)
									allowed=true;
								
						}
					}
					
				}
			
			}
		}

		protected virtual void OnButtonPayClicked (object sender, System.EventArgs e)
		{

			Gtk.TreeModel mod;
			Gtk.TreeIter iter;
			
			if(treeview1.Selection.GetSelected(out mod,out iter))			
			{
				UUID id=(UUID)mod.GetValue(iter,3);
				Primitive prim;
				
				if(FetchedPrims.TryGetValue(id,out prim))
				{
					PayWindow pay=new PayWindow(prim,0);
					pay.Modal=true;
					pay.Show();
				}
				
			}
		}

		protected virtual void OnButtonTouchClicked (object sender, System.EventArgs e)
		{
			Gtk.TreeModel mod;
			Gtk.TreeIter iter;
			
			if(treeview1.Selection.GetSelected(out mod,out iter))			
			{
				UUID id=(UUID)mod.GetValue(iter,3);
				Primitive prim;
				
				if(FetchedPrims.TryGetValue(id,out prim))
				{
					Console.WriteLine("Touching prim "+prim.LocalID.ToString());
					MainClass.client.Self.Touch(prim.LocalID);
				}
			}
		}

		protected virtual void OnButtonSitonClicked (object sender, System.EventArgs e)
		{

            if (this.button_siton.Label == "Stand")
            {
                this.button_siton.Label = "Sit";
                MainClass.win.stand();
                return;
            }

			Gtk.TreeModel mod;
			Gtk.TreeIter iter;
			
			if(treeview1.Selection.GetSelected(out mod,out iter))			
			{
				UUID id=(UUID)mod.GetValue(iter,3);
				Primitive prim;
				
				if(FetchedPrims.TryGetValue(id,out prim))
				{
					MainClass.client.Self.RequestSit(prim.ID,Vector3.Zero);		
				}
			}
		
		}

		protected virtual void OnButtonLookatClicked (object sender, System.EventArgs e)
		{
			Gtk.TreeModel mod;
			Gtk.TreeIter iter;
			
			if(treeview1.Selection.GetSelected(out mod,out iter))			
			{
				UUID id=(UUID)mod.GetValue(iter,3);
				Primitive prim;
				
				if(FetchedPrims.TryGetValue(id,out prim))
				{
                    MainClass.client.Self.LookAtEffect(UUID.Zero, prim.ID, Vector3d.Zero, LookAtType.Idle, UUID.Zero);
				    // We may actualy just want to turn around in this general direction
                    //MainClass.client.Self.Movement.BodyRotation.SetQuaternion(
                    MainClass.client.Self.Movement.TurnToward(prim.Position);
                }
			}
		}

		protected virtual void OnTreeview1UnselectAll (object o, Gtk.UnselectAllArgs args)
		{
			Console.Write("UNSELECT\n");
			//Might need to clean these on a timer
			this.button_lookat.Sensitive=false;
			this.button_siton.Sensitive=false;

		}

		protected virtual void OnButtonTakeClicked (object sender, System.EventArgs e)
		{
			Gtk.TreeModel mod;
			Gtk.TreeIter iter;
			
			if(treeview1.Selection.GetSelected(out mod,out iter))			
			{
				UUID id=(UUID)mod.GetValue(iter,3);
				Primitive prim;
				
				if(FetchedPrims.TryGetValue(id,out prim))
				{
					MainClass.client.Inventory.RequestDeRezToInventory(prim.LocalID);
				}
			}
		}

		protected virtual void OnButtonTakeCopyClicked (object sender, System.EventArgs e)
		{
			
			Gtk.TreeModel mod;
			Gtk.TreeIter iter;
			
			if(treeview1.Selection.GetSelected(out mod,out iter))			
			{
				UUID id=(UUID)mod.GetValue(iter,3);
				Primitive prim;
				
				if(FetchedPrims.TryGetValue(id,out prim))
				{
					MainClass.client.Inventory.RequestDeRezToInventory(prim.LocalID,DeRezDestination.AgentInventoryCopy,MainClass.client.Inventory.FindFolderForType(AssetType.Object),UUID.Random());
				}
			}
		}

		protected virtual void OnButtonBuyClicked (object sender, System.EventArgs e)
		{
			Gtk.TreeModel mod;
			Gtk.TreeIter iter;
			
			if(treeview1.Selection.GetSelected(out mod,out iter))			
			{
				UUID id=(UUID)mod.GetValue(iter,3);
				Primitive prim;
				
				if(FetchedPrims.TryGetValue(id,out prim))
				{
					SaleType st;
					st=prim.Properties.SaleType;
					string salemsg="";
					UUID target=UUID.Zero;
					if(st==SaleType.Contents)
					{   
						target=MainClass.client.Inventory.Store.RootFolder.UUID;
						salemsg="the CONTENTS of";
					}
					if(st==SaleType.Copy)
						salemsg="a COPY of";
					if(st==SaleType.Original)
						salemsg="the ORIGINAL of";

					salemsg="Are you sure you would like to\nbuy "+salemsg+"\n"+prim.Properties.Name+"\n for L$"+prim.Properties.SalePrice.ToString();
				    MessageDialog md2 = new Gtk.MessageDialog(MainClass.win, DialogFlags.DestroyWithParent, MessageType.Question, ButtonsType.YesNo, true, salemsg);
					ResponseType result2 = (ResponseType)md2.Run();
					if(result2==ResponseType.Yes)
					{
						MainClass.client.Objects.BuyObject(MainClass.client.Network.CurrentSim,prim.LocalID,st,prim.Properties.SalePrice,MainClass.client.Self.ActiveGroup,target);
						
					}
					
					md2.Destroy();	

				}
			}
		}


		protected virtual void OnButtonReturnClicked (object sender, System.EventArgs e)
		{
			Gtk.TreeModel mod;
			Gtk.TreeIter iter;
			
			if(treeview1.Selection.GetSelected(out mod,out iter))			
			{
				UUID id=(UUID)mod.GetValue(iter,3);
				Primitive prim;
				
				if(FetchedPrims.TryGetValue(id,out prim))
				{
					uint localid=prim.LocalID;
					MainClass.client.Inventory.RequestDeRezToInventory(localid,DeRezDestination.ReturnToOwner,UUID.Zero,UUID.Random());
				}
				
				
			}
			
		}

		protected virtual void OnButtonMovetoClicked (object sender, System.EventArgs e)
		{
			Gtk.TreeModel mod;
			Gtk.TreeIter iter;
		
			if(this.button_moveto.Label=="Stop")
			{
				AutoPilot.stop();
				return;
			}
			
			if(treeview1.Selection.GetSelected(out mod,out iter))			
			{
				UUID id=(UUID)mod.GetValue(iter,3);
				Primitive prim;
					
				if(FetchedPrims.TryGetValue(id,out prim))
				{
                    AutoPilot.set_target_pos(AutoPilot.localtoglobalpos(prim.Position,MainClass.client.Network.CurrentSim.Handle));
					this.button_moveto.Label="Stop";				
				}
			}
			
		}

		void onAutoPilotFinished()
		{
			this.button_moveto.Label="Move to";
		}
	}
}
