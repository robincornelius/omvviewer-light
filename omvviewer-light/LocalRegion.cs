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
	public partial class LocalRegion : Gtk.Bin
	{
		uint cy;
		uint cx;
		bool requested=false;
		GridRegion[] regions=new GridRegion[9];
		public LocalRegion()
		{
			this.Build();
			MainClass.client.Network.OnCurrentSimChanged += new OpenMetaverse.NetworkManager.CurrentSimChangedCallback(onNewSim);
		    MainClass.client.Grid.OnGridRegion += new OpenMetaverse.GridManager.GridRegionCallback(onGridRegion);
			requested=true;
		}
		
		void onGridRegion(GridRegion region)
		{
			
			Console.WriteLine("!!!!!!!!!!! GRID REGION cx is "+region.X.ToString()+" cy is "+region.Y.ToString());
			Gtk.Application.Invoke(delegate {
		
			if(region.X==cx-1 && region.Y==cy+1)
			{
				this.image1.Pixbuf= MainClass.GetResource("trying.tga");
				new TryGetImage(this.image1,region.MapImageID,100,100,false);	
				regions[0]=region;
			}

			if(region.X==cx && region.Y==cy+1)
			{
				this.image2.Pixbuf= MainClass.GetResource("trying.tga");
				new TryGetImage(this.image2,region.MapImageID,100,100,false);	
				regions[1]=region;
			}
			
			if(region.X==cx+1 && region.Y==cy+1)
			{
				this.image3.Pixbuf= MainClass.GetResource("trying.tga");
				new TryGetImage(this.image3,region.MapImageID,100,100,false);
				regions[2]=region;
			}
			if(region.X==cx-1 && region.Y==cy)
			{
                Console.WriteLine("Updaing map for cell 4 (3)");
				this.image4.Pixbuf= MainClass.GetResource("trying.tga");
				new TryGetImage(this.image4,region.MapImageID,100,100,false);
                regions[3] = region;
			}

				if(region.X==cx+1 && region.Y==cy)
			{
				this.image6.Pixbuf= MainClass.GetResource("trying.tga");
				new TryGetImage(this.image6,region.MapImageID,100,100,false);
				regions[5]=region;
			}

				
			if(region.X==cx-1 && region.Y==cy-1)
			{
				this.image7.Pixbuf= MainClass.GetResource("trying.tga");
				new TryGetImage(this.image7,region.MapImageID,100,100,false);
				regions[6]=region;
			}

			if(region.X==cx && region.Y==cy-1)
			{
				this.image8.Pixbuf= MainClass.GetResource("trying.tga");
				new TryGetImage(this.image8,region.MapImageID,100,100,false);
				regions[7]=region;
			}
			
			if(region.X==cx+1 && region.Y==cy-1)
			{
				this.image9.Pixbuf= MainClass.GetResource("trying.tga");
				new TryGetImage(this.image9,region.MapImageID,100,100,false);
				regions[8]=region;
			}
				
			if(region.RegionHandle==MainClass.client.Network.CurrentSim.Handle)
			{
				if(requested==true)
					{
						requested=false;
						this.image5.Pixbuf= MainClass.GetResource("trying.tga");
						new TryGetImage(this.image5,region.MapImageID,100,100,false);				
						cx=(uint)region.X;
						cy=(uint)region.Y;
						Console.WriteLine("Requesting neighbour grid");
						MainClass.client.Grid.RequestMapBlocks(GridLayerType.Objects,(ushort)(region.X-1),(ushort)(region.Y-1),(ushort)(region.X+1),(ushort)(region.Y+1),false);
						regions[4]=region;
					}
				}
				
			});
		}
		
		void onNewSim(Simulator lastsim)
	    {	
			requested=true;
			for(int x=0;x<9;x++)
			{
				regions[x]=new OpenMetaverse.GridRegion();
				regions[x].Name="";
			}

            Gtk.Application.Invoke(delegate{
                image1.Clear();
                image2.Clear();
                image3.Clear();
                image4.Clear();
                image5.Clear();
                image6.Clear();
                image7.Clear();
                image8.Clear();
                image9.Clear();
                requested = true;
                Console.WriteLine("Requesting map region for current region");
                MainClass.client.Grid.RequestMapRegion(MainClass.client.Network.CurrentSim.Name, GridLayerType.Objects);
            });           
        }

	

		protected virtual void OnEventbox1ButtonPressEvent (object o, Gtk.ButtonPressEventArgs args)
		{
			if(regions[0].Name!="")
			MainClass.win.map_widget.changeregion(regions[0]);
		}

		protected virtual void OnEventbox2ButtonPressEvent (object o, Gtk.ButtonPressEventArgs args)
		{
			if(regions[1].Name!="")
			MainClass.win.map_widget.changeregion(regions[1]);
		}

		protected virtual void OnEventbox3ButtonPressEvent (object o, Gtk.ButtonPressEventArgs args)
		{
			if(regions[2].Name!="")
			MainClass.win.map_widget.changeregion(regions[2]);
		}

		protected virtual void OnEventbox4ButtonPressEvent (object o, Gtk.ButtonPressEventArgs args)
		{
			if(regions[3].Name!="")
			MainClass.win.map_widget.changeregion(regions[3]);
		}

		protected virtual void OnEventbox5ButtonPressEvent (object o, Gtk.ButtonPressEventArgs args)
		{
			if(regions[4].Name!="")
			MainClass.win.map_widget.changeregion(regions[4]);
		}

		protected virtual void OnEventbox6ButtonPressEvent (object o, Gtk.ButtonPressEventArgs args)
		{
			if(regions[5].Name!="")
			MainClass.win.map_widget.changeregion(regions[5]);
		}

		protected virtual void OnEventbox7ButtonPressEvent (object o, Gtk.ButtonPressEventArgs args)
		{
			if(regions[6].Name!="")
			MainClass.win.map_widget.changeregion(regions[6]);
		}

		protected virtual void OnEventbox8ButtonPressEvent (object o, Gtk.ButtonPressEventArgs args)
		{
			if(regions[7].Name!="")
			MainClass.win.map_widget.changeregion(regions[7]);
		}

		protected virtual void OnEventbox9ButtonPressEvent (object o, Gtk.ButtonPressEventArgs args)
		{
			if(regions[8].Name!="")
			MainClass.win.map_widget.changeregion(regions[8]);
		}
		
	}
}
