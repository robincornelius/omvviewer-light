/*
omvviewerlight a Text based client to metaverses such as Linden Labs Secondlife(tm)
    Copyright (C) 2008,2009  Robin Cornelius <robin.cornelius@gmail.com>

    Redistribution and use in source and binary forms, with or without
    modification, are permitted provided that the following conditions
    are met:
    1. Redistributions of source code must retain the above copyright
        notice, this list of conditions and the following disclaimer.
    2. Redistributions in binary form must reproduce the above copyright
        notice, this list of conditions and the following disclaimer in the
        documentation and/or other materials provided with the distribution.
    3. The name of the author may not be used to endorse or promote products
        derived from this software without specific prior written permission.

    THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
    IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
    OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
    IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
    INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
    NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
    DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
    THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
    (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
    THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

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
            Logger.Log("Disposing of the LocalRegion control",Helpers.LogLevel.Debug);

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

#region libomv_events

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

            Logger.Log("Got grid region for :"+e.Region.Name+" ("+e.Region.RegionHandle.ToString()+")", Helpers.LogLevel.Debug);
       
            if (e.Region.RegionHandle == MainClass.client.Network.CurrentSim.Handle && requested==true)
            {
                requested = false;
                cx = (uint)e.Region.X;
                cy = (uint)e.Region.Y;
                Logger.Log("Requesting neighbour grid",Helpers.LogLevel.Debug);
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

        void Network_SimChanged(object sender, SimChangedEventArgs e)
        {
            if (e.PreviousSimulator == MainClass.client.Network.CurrentSim)
                return;

            requestnewgridregion();
        }

#endregion

        void requestnewgridregion()
        {

            cx = 0;
            cy = 0;
           
            Gtk.Application.Invoke(delegate{
         
                for (int x = 0; x < 9; x++)
                {
                    regions[x] = new OpenMetaverse.GridRegion();
                    regions[x].Name = "";
                    maps[x].SetAsWater();
                    Gtk.Tooltips name = new Gtk.Tooltips();
                    name.SetTip(maps[x], "Empty", "");
                    name.Enable();
                }
                 
                Logger.Log("Requesting map region for current region",Helpers.LogLevel.Debug);
                requested = true;
                MainClass.client.Grid.RequestMapRegion(MainClass.client.Network.CurrentSim.Name, GridLayerType.Objects);
               
            });           
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
