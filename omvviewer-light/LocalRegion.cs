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
		public LocalRegion()
		{
			this.Build();
			MainClass.client.Network.OnCurrentSimChanged += new OpenMetaverse.NetworkManager.CurrentSimChangedCallback(onNewSim);
		    MainClass.client.Grid.OnGridRegion += new OpenMetaverse.GridManager.GridRegionCallback(onGridRegion);
			MainClass.client.Grid.OnGridLayer += new OpenMetaverse.GridManager.GridLayerCallback(onGridLayer);
		
		}
		
		void onGridRegion(GridRegion region)
		{
			Console.WriteLine("!!!!!!!!!!! GRID REGION cx is "+region.X.ToString()+" cy is "+region.Y.ToString());
			Gtk.Application.Invoke(delegate {
		
			if(region.X==cx-1 && region.Y==cy+1)
			{
				this.image1.Pixbuf= MainClass.GetResource("trying.tga");
				new TryGetImage(this.image1,region.MapImageID,100,100);				
			}

			if(region.X==cx && region.Y==cy+1)
			{
				this.image2.Pixbuf= MainClass.GetResource("trying.tga");
				new TryGetImage(this.image2,region.MapImageID,100,100);				
			}
			
			if(region.X==cx+1 && region.Y==cy+1)
			{
				this.image3.Pixbuf= MainClass.GetResource("trying.tga");
				new TryGetImage(this.image3,region.MapImageID,100,100);				
			}

			if(region.X==cx-1 && region.Y==cy)
			{
				this.image4.Pixbuf= MainClass.GetResource("trying.tga");
				new TryGetImage(this.image4,region.MapImageID,100,100);				
			}

				if(region.X==cx+1 && region.Y==cy)
			{
				this.image6.Pixbuf= MainClass.GetResource("trying.tga");
				new TryGetImage(this.image6,region.MapImageID,100,100);				
			}

				
			if(region.X==cx-1 && region.Y==cy-1)
			{
				this.image7.Pixbuf= MainClass.GetResource("trying.tga");
				new TryGetImage(this.image7,region.MapImageID,100,100);				
			}

			if(region.X==cx && region.Y==cy-1)
			{
				this.image8.Pixbuf= MainClass.GetResource("trying.tga");
				new TryGetImage(this.image8,region.MapImageID,100,100);				
			}
			
			if(region.X==cx+1 && region.Y==cy-1)
			{
				this.image9.Pixbuf= MainClass.GetResource("trying.tga");
				new TryGetImage(this.image9,region.MapImageID,100,100);				
			}
				
				
			if(region.RegionHandle==MainClass.client.Network.CurrentSim.Handle)
			{
				if(requested==true)
					{
						requested=false;
						this.image5.Pixbuf= MainClass.GetResource("trying.tga");
				new TryGetImage(this.image5,region.MapImageID,100,100);				
				cx=(uint)region.X;
				cy=(uint)region.Y;
				Console.WriteLine("Requesting neighbout grid");
				MainClass.client.Grid.RequestMapBlocks(GridLayerType.Objects,(ushort)(region.X-1),(ushort)(region.Y-1),(ushort)(region.X+1),(ushort)(region.Y+1),false);
					
					}
				}
				
			});
		}
		
		void onNewSim(Simulator lastsim)
	    {			
			requested=true;
        }
		
		void onGridLayer(GridLayer layer)
	    {
			//layer.ImageID
			//Console.Write("Got grid layer reply, requesting texture :"+layer.ImageID.ToString()+"\n");	
		}

	}
}
