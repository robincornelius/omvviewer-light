// LocalRegion.cs created with MonoDevelop
// User: robin at 19:07 09/02/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using OpenMetaverse;
using omvviewerlight;
using Gdk;
using Gtk;

namespace omvviewerlight
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class LocalRegion : Gtk.Bin
	{
        

		uint cy;
		uint cx;
		bool requested=false;
		GridRegion[] regions=new GridRegion[9];
        Map[] maps = new Map[9];
        //Gtk.Image[] baseimages = new Gtk.Image[9];
		int size=150;
		int oldsize=0;
		
		public LocalRegion()
		{

            MainClass.client.Network.OnCurrentSimChanged += new OpenMetaverse.NetworkManager.CurrentSimChangedCallback(onNewSim);
            MainClass.client.Grid.OnGridRegion += new OpenMetaverse.GridManager.GridRegionCallback(onGridRegion);
            
			this.Build();
            maps[0] = this.map1;
            maps[1] = this.map2;
            maps[2] = this.map3;
            maps[3] = this.map4;
            maps[4] = this.map5;
            maps[5] = this.map6;
            maps[6] = this.map7;
            maps[7] = this.map8;
            maps[8] = this.map9;
				
            this.SizeAllocated+=new Gtk.SizeAllocatedHandler(onResize);
			                                                
			requested=true;
			
		}
		
		void onResize(object o,SizeAllocatedArgs args)
		{
			
		 	size=args.Allocation.Width<args.Allocation.Height?args.Allocation.Width:args.Allocation.Height;
			size=size/3;
			if(oldsize==size)
				return;
			
			oldsize=size;

		    for(int x = 0; x < 9; x++)
            {
				maps[x].optimal_width=size;
		    }
		}
		
		void onGridRegion(GridRegion region)
		{

            lock (MainClass.win.grid_regions)
            {
                if (!MainClass.win.grid_regions.ContainsKey(region.RegionHandle))
                {
                    MainClass.win.grid_regions.Add(region.RegionHandle, region);
                }
            }

			Gtk.Application.Invoke(delegate {

            if (region.RegionHandle == MainClass.client.Network.CurrentSim.Handle && requested==true)
            {
                requested = false;
                cx = (uint)region.X;
                cy = (uint)region.Y;
                Console.WriteLine("Requesting neighbour grid");
                MainClass.client.Grid.RequestMapBlocks(GridLayerType.Objects, (ushort)(region.X - 1), (ushort)(region.Y - 1), (ushort)(region.X + 1), (ushort)(region.Y + 1), false);
            }

            int col = (int)2 - (((int)cx + (int)1) - (int)region.X); //FFS
            int row = (((int)cy + (int)1) - (int)region.Y);

            if (row < 0 || row > 2)
                return;
            if (col < 0 || col > 2)
                return;

            int index = (row * 3) + col;

            maps[index].SetGridRegion(UUID.Zero, region.RegionHandle);
           
            Gtk.Tooltips name = new Gtk.Tooltips();
            name.SetTip(maps[index], region.Name, "");
            name.Enable();		
			regions[index]=region;
	
			});
		}
		
		void onNewSim(Simulator lastsim)
	    {	
			requested=true;
            cx = 0;
            cy = 0;


            MainClass.client.Grid.RequestMapRegion(MainClass.client.Network.CurrentSim.Name, GridLayerType.Objects);

            Gtk.Application.Invoke(delegate{

                /*
                for (int x = 0; x < 9; x++)
                {
                    regions[x] = new OpenMetaverse.GridRegion();
                    regions[x].Name = "";
                    images[x].Clear();
					images[x].Pixbuf=MainClass.GetResource("water.png");
					baseimages[x] = new Gtk.Image(MainClass.GetResource("water.png"));
                    Gtk.Tooltips name = new Gtk.Tooltips();
                    name.SetTip(images[x], "Empty", "");
                    name.Enable();
                }
                 */

     
                Console.WriteLine("Requesting map region for current region");            });           
        }

	

		protected virtual void OnEventbox1ButtonPressEvent (object o, Gtk.ButtonPressEventArgs args)
		{
			if(MainClass.win.map_widget==null)
				return;
			if(regions[0].Name!="")
			MainClass.win.map_widget.changeregion(regions[0]);
		}

		protected virtual void OnEventbox2ButtonPressEvent (object o, Gtk.ButtonPressEventArgs args)
		{
			if(MainClass.win.map_widget==null)
				return;

			if(regions[1].Name!="")
			MainClass.win.map_widget.changeregion(regions[1]);
		}

		protected virtual void OnEventbox3ButtonPressEvent (object o, Gtk.ButtonPressEventArgs args)
		{
			if(MainClass.win.map_widget==null)
				return;
			
			if(regions[2].Name!="")
			MainClass.win.map_widget.changeregion(regions[2]);
		}

		protected virtual void OnEventbox4ButtonPressEvent (object o, Gtk.ButtonPressEventArgs args)
		{
			if(MainClass.win.map_widget==null)
				return;

			if(regions[3].Name!="")
			MainClass.win.map_widget.changeregion(regions[3]);
		}

		protected virtual void OnEventbox5ButtonPressEvent (object o, Gtk.ButtonPressEventArgs args)
		{
			if(MainClass.win.map_widget==null)
				return;
			
			if(regions[4].Name!="")
			MainClass.win.map_widget.changeregion(regions[4]);
		}

		protected virtual void OnEventbox6ButtonPressEvent (object o, Gtk.ButtonPressEventArgs args)
		{
			if(MainClass.win.map_widget==null)
				return;

			if(regions[5].Name!="")
			MainClass.win.map_widget.changeregion(regions[5]);
		}

		protected virtual void OnEventbox7ButtonPressEvent (object o, Gtk.ButtonPressEventArgs args)
		{
			if(MainClass.win.map_widget==null)
				return;
			
			if(regions[6].Name!="")
			MainClass.win.map_widget.changeregion(regions[6]);
		}

		protected virtual void OnEventbox8ButtonPressEvent (object o, Gtk.ButtonPressEventArgs args)
		{
			if(MainClass.win.map_widget==null)
				return;

			if(regions[7].Name!="")
			MainClass.win.map_widget.changeregion(regions[7]);
		}

		protected virtual void OnEventbox9ButtonPressEvent (object o, Gtk.ButtonPressEventArgs args)
		{
			if(MainClass.win.map_widget==null)
				return;
			
			if(regions[8].Name!="")
			MainClass.win.map_widget.changeregion(regions[8]);
		}
		
	}
}
