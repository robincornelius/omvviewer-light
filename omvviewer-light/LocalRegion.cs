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
        bool needdata = false;
	
			
	
		public LocalRegion()
		{

            
			this.Build();

            MainClass.onRegister += new MainClass.register(MainClass_onRegister);
            MainClass.onDeregister += new MainClass.deregister(MainClass_onDeregister);
            if(MainClass.client != null ) { MainClass_onRegister(); }
		
			
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
			                                                
			requested=true;

            if (MainClass.client != null)
            {
                if (MainClass.client.Network.LoginStatusCode == OpenMetaverse.LoginStatus.Success)
                {
                    needdata = true;

                    //if(MainClass.client.Network.CurrentSim!=null && MainClass.client.Network.CurrentSim.Name!=null)
                        //MainClass.client.Grid.RequestMapRegion(MainClass.client.Network.CurrentSim.Name, GridLayerType.Objects);
                }
            }	
		}

        void MainClass_onDeregister()
        {
            MainClass.client.Network.OnCurrentSimChanged -= new OpenMetaverse.NetworkManager.CurrentSimChangedCallback(onNewSim);
            MainClass.client.Grid.OnGridRegion -= new OpenMetaverse.GridManager.GridRegionCallback(onGridRegion);
 
        }

        void MainClass_onRegister()
        {
            requested = false;
            needdata = true;
            MainClass.client.Network.OnCurrentSimChanged += new OpenMetaverse.NetworkManager.CurrentSimChangedCallback(onNewSim);
            MainClass.client.Grid.OnGridRegion += new OpenMetaverse.GridManager.GridRegionCallback(onGridRegion);
 
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

            if (needdata == true)
            {
                needdata = false;
                if(MainClass.client.Network.CurrentSim!=null && MainClass.client.Network.CurrentSim.Name!=null)
                    MainClass.client.Grid.RequestMapRegion(MainClass.client.Network.CurrentSim.Name, GridLayerType.Objects);
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
            if (lastsim == MainClass.client.Network.CurrentSim)
                return;

			requested=true;
            cx = 0;
            cy = 0;
            for (int x = 0; x < 9; x++)
            {
                maps[x].SetAsWater();
            }
  
            Gtk.Application.Invoke(delegate{

                
                for (int x = 0; x < 9; x++)
                {
                    regions[x] = new OpenMetaverse.GridRegion();
                    regions[x].Name = "";
                   
                    Gtk.Tooltips name = new Gtk.Tooltips();
                    name.SetTip(maps[x], "Empty", "");
                    name.Enable();
                }
                 

                MainClass.client.Grid.RequestMapRegion(MainClass.client.Network.CurrentSim.Name, GridLayerType.Objects);

                Console.WriteLine("Requesting map region for current region");            });           
        }


        void mapclick(int x)
        {
            if (MainClass.win.map_widget == null)
                return;
            if (regions[x].Name != "")
                MainClass.win.map_widget.changeregion(regions[x]);
        }

		
	}
}
