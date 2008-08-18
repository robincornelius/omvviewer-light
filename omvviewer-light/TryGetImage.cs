// TryGetImage.cs created with MonoDevelop
// User: robin at 14:44 18/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using libsecondlife;
using libsecondlife.Imaging;
using Gdk;

namespace omvviewerlight
{
	public class TryGetImage
	{
		
		LLUUID target_asset;
		Gtk.Image target_image;
		
		public TryGetImage(Gtk.Image target,LLUUID asset)
		{
			if(target==null)
				return;
			
			MainClass.client.Assets.OnImageReceived += new libsecondlife.AssetManager.ImageReceivedCallback(onGotImage);
			
			target_asset=asset;
			target_image=target;
					
			Gdk.Pixbuf buf=new Gdk.Pixbuf("trying.tga");
			target_image.Pixbuf=buf.ScaleSimple(128,128,Gdk.InterpType.Bilinear);

			if(asset!=LLUUID.Zero)
					MainClass.client.Assets.RequestImage(asset,libsecondlife.ImageType.Normal,1013000.0f, 0);	
						
		}
		
		void onGotImage(ImageDownload image,AssetTexture asset)
		{
	
			if(!image.Success)
			{
				Console.Write("Failed to download image\n");
				return;
			}
			
			if(asset.AssetID!=target_asset)
				return;
			
			Gtk.Application.Invoke(delegate {	
				
				Console.Write("Downloaded asset "+asset.AssetID.ToString()+"\n");
				try
				{	    
					ManagedImage imgData;
					OpenJPEG.DecodeToImage(image.AssetData, out imgData);
					byte[] tgaFile = imgData.ExportTGA();
    				
					Gdk.Pixbuf buf=new Gdk.Pixbuf(tgaFile);
					Console.Write("Decoded\n");
					int x,y;
					x=target_image.Pixbuf.Width;
					
					target_image.Pixbuf=buf.ScaleSimple(128,128,Gdk.InterpType.Bilinear);

				}
				catch(Exception e)
				{
					Console.Write("\n*****************\n"+e.Message+"\n");	
				}				

			});
		}	
	}
}
