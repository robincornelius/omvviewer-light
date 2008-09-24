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

// TryGetImage.cs created with MonoDevelop
// User: robin at 14:44 18/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using OpenMetaverse;
using OpenMetaverse.Imaging;
using Gdk;

namespace omvviewerlight
{
	public class TryGetImage
	{
		
		UUID target_asset;
		Gtk.Image target_image;
		
		public TryGetImage(Gtk.Image target,UUID asset)
		{
			if(target==null)
				return;
			
			MainClass.client.Assets.OnImageReceived += new OpenMetaverse.AssetManager.ImageReceivedCallback(onGotImage);
			
			target_asset=asset;
			target_image=target;
					
			Gdk.Pixbuf buf=new Gdk.Pixbuf("trying.tga");
			target_image.Pixbuf=buf.ScaleSimple(128,128,Gdk.InterpType.Bilinear);

			if(asset!=UUID.Zero)
					MainClass.client.Assets.RequestImage(asset,OpenMetaverse.ImageType.Normal,1013000.0f, 0,0);	
						
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

            MainClass.client.Assets.OnImageReceived -= new OpenMetaverse.AssetManager.ImageReceivedCallback(onGotImage);

				
				Console.Write("Downloaded asset "+asset.AssetID.ToString()+"\n");
                byte[] tgaFile=null;
				try
				{	    
					ManagedImage imgData;
					OpenJPEG.DecodeToImage(image.AssetData, out imgData);
					tgaFile = imgData.ExportTGA();
                }
                catch (Exception e)
                {
                    Console.Write("\n*****************\n" + e.Message + "\n");
                }

                Gtk.Application.Invoke(delegate
                {	
					Gdk.Pixbuf buf=new Gdk.Pixbuf(tgaFile);
					Console.Write("Decoded\n");
					int x,y;
                    if (target_image!=null) // this has managed to get set to null
                    {
                        if (target_image.Pixbuf != null)
                        {
                            x = target_image.Pixbuf.Width;
                            target_image.Pixbuf = buf.ScaleSimple(128, 128, Gdk.InterpType.Bilinear);
                        }
                    }
              });
		}	
	}
}
