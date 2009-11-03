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
		int size=150;
		int oldsize=0;
   
	
		public LocalRegion()
		{

			this.Build();

            MainClass.onRegister += new MainClass.register(MainClass_onRegister);
            MainClass.onDeregister += new MainClass.deregister(MainClass_onDeregister);
           
            maps[0] = this.map1;
            maps[1] = this.map2;
            maps[2] = this.map3;
            maps[3] = this.map4;
            maps[4] = this.map5;
            maps[5] = this.map6;
            maps[6] = this.map7;
            maps[7] = this.map8;
            maps[8] = this.map9;


            maps[0].onclickMap += delegate { mapclick(0);};
            maps[1].onclickMap += delegate { mapclick(1); };
            maps[2].onclickMap += delegate { mapclick(2); };
            maps[3].onclickMap += delegate { mapclick(3); };
            maps[4].onclickMap += delegate { mapclick(4); };
            maps[5].onclickMap += delegate { mapclick(5); };
            maps[6].onclickMap += delegate { mapclick(6); };
            maps[7].onclickMap += delegate { mapclick(7); };
            maps[8].onclickMap += delegate { mapclick(8); };

            this.SizeAllocated+=new Gtk.SizeAllocatedHandler(onResize);

            if (MainClass.client != null) { MainClass_onRegister(); }
                            
            if (MainClass.client != null)
            {
                if (MainClass.client.Network.LoginStatusCode == OpenMetaverse.LoginStatus.Success)
                {
                    requestnewgridregion();
                }
            }	
		}

        void MainClass_onDeregister()
        {
            if (MainClass.client != null)
            {
                MainClass.client.Network.SimChanged -= new EventHandler<SimChangedEventArgs>(Network_SimChanged);
                MainClass.client.Grid.GridRegion -= new EventHandler<GridRegionEventArgs>(Grid_GridRegion);
            }
        }

        void MainClass_onRegister()
        {
            requested = false;
            
            for (int x = 0; x < 9; x++)
            {
                maps[x].SetAsWater();
                regions[x] = new OpenMetaverse.GridRegion();
                regions[x].Name = "";

                Gtk.Tooltips name = new Gtk.Tooltips();
                name.SetTip(maps[x], "Empty", "");
                name.Enable();
            }

            MainClass.client.Network.SimChanged += new EventHandler<SimChangedEventArgs>(Network_SimChanged);
            MainClass.client.Grid.GridRegion += new EventHandler<GridRegionEventArgs>(Grid_GridRegion);
        }



        new public void Dispose()
        {
            Console.WriteLine("Disposing of the LocalRegion control");

            MainClass.onRegister -= new MainClass.register(MainClass_onRegister);
            MainClass.onDeregister -= new MainClass.deregister(MainClass_onDeregister);
            MainClass_onDeregister();
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
				maps[x].set_optimal_size(size);
		    }
		}


        void Grid_GridRegion(object sender, GridRegionEventArgs e)
		{

            lock (MainClass.win.grid_regions)
            {
                if (!MainClass.win.grid_regions.ContainsKey(e.Region.RegionHandle))
                {
                    MainClass.win.grid_regions.Add(e.Region.RegionHandle, e.Region);
                }
            }

			Gtk.Application.Invoke(delegate {

            if (e.Region.RegionHandle == MainClass.client.Network.CurrentSim.Handle && requested==true)
            {
                requested = false;
                cx = (uint)e.Region.X;
                cy = (uint)e.Region.Y;
                Console.WriteLine("Requesting neighbour grid");
                MainClass.client.Grid.RequestMapBlocks(GridLayerType.Objects, (ushort)(e.Region.X - 1), (ushort)(e.Region.Y - 1), (ushort)(e.Region.X + 1), (ushort)(e.Region.Y + 1), false);
            }

            int col = (int)2 - (((int)cx + (int)1) - (int)e.Region.X); //FFS
            int row = (((int)cy + (int)1) - (int)e.Region.Y);

            if (row < 0 || row > 2)
                return;
            if (col < 0 || col > 2)
                return;

            int index = (row * 3) + col;

            maps[index].SetGridRegion(UUID.Zero, e.Region.RegionHandle);
           
            Gtk.Tooltips name = new Gtk.Tooltips();
            name.SetTip(maps[index], e.Region.Name, "");
            name.Enable();		
			regions[index]=e.Region;
	
			});
		}

        void requestnewgridregion()
        {

            cx = 0;
            cy = 0;
           
            Gtk.Application.Invoke(delegate{

                
                for (int x = 0; x < 9; x++)
                {
                    regions[x] = new OpenMetaverse.GridRegion();
                    regions[x].Name = "";
                   
                    Gtk.Tooltips name = new Gtk.Tooltips();
                    name.SetTip(maps[x], "Empty", "");
                    name.Enable();
                }
                 
                Console.WriteLine("Requesting map region for current region");
                requested = true;
                MainClass.client.Grid.RequestMapRegion(MainClass.client.Network.CurrentSim.Name, GridLayerType.Objects);
               
            });           
        }

        void Network_SimChanged(object sender, SimChangedEventArgs e)
        {
            if (e.PreviousSimulator == MainClass.client.Network.CurrentSim)
                return;

            requestnewgridregion();
        }


        void mapclick(int x)
        {
            if (MainClass.win.map_widget == null)
                return;
            
            //FIXME
            //if (regions[x].Name != "")
                //MainClass.win.map_widget.changeregion(regions[x]);
        }

		
	}
}
