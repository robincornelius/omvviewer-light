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

// ObjectsLayout.cs created with MonoDevelop
// User: robin at 13:32 14/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;
using OpenMetaverse;

namespace omvviewerlight
{
	
	public partial class ObjectsLayout : Gtk.Bin
	{
	
		Gtk.ListStore store;	
        Dictionary<UUID, Primitive> PrimsWaiting = new Dictionary<UUID, Primitive>();

		public ObjectsLayout()
		{
			this.Build();
			store= new Gtk.ListStore (typeof(string),typeof(string),typeof(string),typeof(UUID));
            Gtk.TreeViewColumn col;

            col = new Gtk.TreeViewColumn("Name", new Gtk.CellRendererText(), "text", 0);
            col.Clicked += new EventHandler(name_col_Clicked);
            treeview1.AppendColumn(col);

            col = new Gtk.TreeViewColumn("Desc.",new Gtk.CellRendererText(),"text",1);
            col.Clicked += new EventHandler(desc_col_Clicked);
            treeview1.AppendColumn(col);

            col = new Gtk.TreeViewColumn("Distance",new Gtk.CellRendererText(),"text",2);
            col.Clicked += new EventHandler(dist_col_Clicked);
            treeview1.AppendColumn(col);
            
            treeview1.Model=store;
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
          
			
			MainClass.client.Objects.OnObjectProperties += new OpenMetaverse.ObjectManager.ObjectPropertiesCallback(Objects_OnObjectProperties);
			MainClass.client.Groups.OnGroupNames += new OpenMetaverse.GroupManager.GroupNamesCallback(onGroupNames);

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
			
		void onGroupNames(Dictionary <UUID,string>groups)
	    {
			Gtk.TreeModel mod;
			Gtk.TreeIter iter;
			
			if(treeview1.Selection.GetSelected(out mod,out iter))			
			{
				UUID id=(UUID)mod.GetValue(iter,3);
				Primitive prim;
				
				if(PrimsWaiting.TryGetValue(id,out prim))
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
						
		bool myfunc(Gtk.TreeModel mod, Gtk.TreePath path, Gtk.TreeIter iter)
		{
			UUID key=(UUID)store.GetValue(iter,3);			
			if(PrimsWaiting.ContainsKey(key))
			{
				store.SetValue(iter,0,PrimsWaiting[key].Properties.Name);
				store.SetValue(iter,1,PrimsWaiting[key].Properties.Description);
				store.SetValue(iter,2,Vector3.Distance(PrimsWaiting[key].Position,MainClass.client.Self.RelativePosition).ToString());
				store.SetValue(iter,3,PrimsWaiting[key].Properties.ObjectID.ToString());
			}
			return true;
		}

		protected virtual void OnButtonSearchClicked (object sender, System.EventArgs e)
		{
			int radius;
			int.TryParse(this.entry1.Text,out radius);
			store.Clear();
			// *** get current location ***
            Vector3 location = MainClass.client.Self.SimPosition;

            // *** find all objects in radius ***
            List<Primitive> prims = MainClass.client.Network.CurrentSim.ObjectsPrimitives.FindAll(
                delegate(Primitive prim) {
                    Vector3 pos = prim.Position;
                    return ((prim.ParentID == 0) && (pos != Vector3.Zero) && (Vector3.Distance(pos, location) < radius));
                }
            );

           	RequestObjectProperties(prims, 250);
			
		}
		
        private void RequestObjectProperties(List<Primitive> objects, int msPerRequest)
        {
            // Create an array of the local IDs of all the prims we are requesting properties for
            uint[] localids = new uint[objects.Count];

            lock (PrimsWaiting) {
                PrimsWaiting.Clear();

                for (int i = 0; i < objects.Count; ++i) {
                    localids[i] = objects[i].LocalID;
                    PrimsWaiting.Add(objects[i].ID, objects[i]);
                }
            }

            MainClass.client.Objects.SelectObjects(MainClass.client.Network.CurrentSim, localids);

            //return AllPropertiesReceived.WaitOne(2000 + msPerRequest * objects.Count, false);
        }

		void Objects_OnObjectProperties(Simulator simulator, Primitive.ObjectProperties properties)
        {
            lock (PrimsWaiting) {
                Primitive prim;
                if (PrimsWaiting.TryGetValue(properties.ObjectID, out prim)) {
                    prim.Properties = properties;
					store.AppendValues(prim.Properties.Name,prim.Properties.Description,Vector3.Distance(prim.Position,MainClass.client.Self.RelativePosition).ToString(),prim.Properties.ObjectID);
					Gtk.Application.Invoke(delegate {										
						store.Foreach(myfunc);
				});
				
				}
              //  PrimsWaiting.Remove(properties.ObjectID);

                //if (PrimsWaiting.Count == 0)
                   // AllPropertiesReceived.Set();
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
		
				Primitive prim;
				if(PrimsWaiting.TryGetValue(id,out prim))
				{
				Console.WriteLine(prim.ToString());

					this.label_name.Text=prim.Properties.Name;
					this.label_desc.Text=prim.Properties.Description;
									
					AsyncNameUpdate ud=new AsyncNameUpdate(prim.Properties.OwnerID,false);  
					ud.onNameCallBack += delegate(string namex,object[] values){label_owner.Text=namex;};
					
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
				
					
					this.label_distance.Text=Vector3.Distance(MainClass.client.Self.RelativePosition,prim.Position).ToString()+" m";
				
					
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
				
				if(PrimsWaiting.TryGetValue(id,out prim))
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
				
				if(PrimsWaiting.TryGetValue(id,out prim))
				{
					MainClass.client.Self.Touch(prim.LocalID);
				}
			}
		}

		protected virtual void OnButtonSitonClicked (object sender, System.EventArgs e)
		{
		Gtk.TreeModel mod;
			Gtk.TreeIter iter;
			
			if(treeview1.Selection.GetSelected(out mod,out iter))			
			{
				UUID id=(UUID)mod.GetValue(iter,3);
				Primitive prim;
				
				if(PrimsWaiting.TryGetValue(id,out prim))
				{
					//MainClass.client.Self.sit
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
				
				if(PrimsWaiting.TryGetValue(id,out prim))
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
				
				if(PrimsWaiting.TryGetValue(id,out prim))
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
				
				if(PrimsWaiting.TryGetValue(id,out prim))
				{
					Gtk.MessageDialog md=new Gtk.MessageDialog(MainClass.win,Gtk.DialogFlags.DestroyWithParent,Gtk.MessageType.Info,Gtk.ButtonsType.Ok,false,"Sorry that is not yet implemented");
					md.Run();
					md.Destroy();
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
				
				if(PrimsWaiting.TryGetValue(id,out prim))
				{
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
				
				if(PrimsWaiting.TryGetValue(id,out prim))
				{
					//MainClass.client.Parcels
				}
			}
		}
	
	}
}
