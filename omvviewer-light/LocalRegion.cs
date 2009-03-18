// LocalRegion.cs created with MonoDevelop
// User: robin at 19:07 09/02/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using OpenMetaverse;
using omvviewerlight;

namespace omvviewerlight
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class LocalRegion : Gtk.Bin
	{
		uint cy;
		uint cx;
		bool requested=false;
		GridRegion[] regions=new GridRegion[9];
        Gtk.Image[] images = new Gtk.Image[9];
		public LocalRegion()
		{
			this.Build();
            images[0] = this.image1;
            images[1] = this.image2;
            images[2] = this.image3;
            images[3] = this.image4;
            images[4] = this.image5;
            images[5] = this.image6;
            images[6] = this.image7;
            images[7] = this.image8;
            images[8] = this.image9;
			
			for (int x = 0; x < 9; x++)
            {
				images[x].HeightRequest=150;
				images[x].WidthRequest=150;
			}
			MainClass.client.Network.OnCurrentSimChanged += new OpenMetaverse.NetworkManager.CurrentSimChangedCallback(onNewSim);
		    MainClass.client.Grid.OnGridRegion += new OpenMetaverse.GridManager.GridRegionCallback(onGridRegion);
			requested=true;
		}
		
		void onGridRegion(GridRegion region)
		{
			
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

            images[index].Pixbuf = MainClass.GetResource("trying.tga");
            Gtk.Tooltips name = new Gtk.Tooltips();
            name.SetTip(images[index], region.Name,"");
            name.Enable();
            new TryGetImage(images[index], region.MapImageID, 150, 150, false);	
			regions[index]=region;
	
			});
		}
		
		void onNewSim(Simulator lastsim)
	    {	
			requested=true;
            cx = 0;
            cy = 0;

            Gtk.Application.Invoke(delegate{

                for (int x = 0; x < 9; x++)
                {
                    regions[x] = new OpenMetaverse.GridRegion();
                    regions[x].Name = "";
                    images[x].Clear();
					images[x].Pixbuf=MainClass.GetResource("water.png");
                    Gtk.Tooltips name = new Gtk.Tooltips();
                    name.SetTip(images[x], "Empty", "");
                    name.Enable();
                }

     
                Console.WriteLine("Requesting map region for current region");
                MainClass.client.Grid.RequestMapRegion(MainClass.client.Network.CurrentSim.Name, GridLayerType.Objects);
            });           
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
