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
        int img_width;
        int img_height;
		
		public TryGetImage(Gtk.Image target,UUID asset,int width,int height)
		{
			if(target==null)
				return;
			
			MainClass.client.Assets.OnImageReceived += new OpenMetaverse.AssetManager.ImageReceivedCallback(onGotImage);
			MainClass.client.Assets.OnImageReceiveProgress += new OpenMetaverse.AssetManager.ImageReceiveProgressCallback(onProgress);
			
			target_asset=asset;
			target_image=target;
            img_width = width;
            img_height = height;
					
			Gdk.Pixbuf buf=new Gdk.Pixbuf("trying.tga");
			target_image.Pixbuf=buf.ScaleSimple(width,height,Gdk.InterpType.Bilinear);

			if(asset!=UUID.Zero)
					MainClass.client.Assets.RequestImage(asset,OpenMetaverse.ImageType.Normal,1013000.0f, 0,0);	
						
		}
			                                               
        void onProgress(UUID image, int recieved, int total,int lastpacket)
		{
			if(target_asset!=image)
			return;
			
//Console.WriteLine("Progress recieved "+recieved.ToString()+" of "+total.ToString());
			
            progress(target_image.Pixbuf,(float)recieved/(float)total);
			

	
	}
		
		unsafe void progress(Gdk.Pixbuf bufdest,float progress)
		{
			byte * pixels=(byte *)bufdest.Pixels;
		    int width=bufdest.Width;
			int height=bufdest.Height;
			int rowstride=bufdest.Rowstride;
			int channels=bufdest.NChannels;
			byte * p;		
			int y,x;
			
	//		Console.WriteLine("Progress is "+progress.ToString());
            int widthx=(int)((float)width*progress);
		//	Console.WriteLine("Width is  is "+widthx.ToString());

			
			if(progress>1)
                 progress=1;			
			
			for(y=(height-20);y<(height-5);y++)
			{
				for(x=0;x<((float)widthx);x++)
					{
                    
					p=pixels+((y)*rowstride)+((x)* channels);
                        p[0]=255;
						p[1]=255;
						p[2]=255;
                }	
				
			
		     }


			
			
         
			
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
					try
				{
				    Gdk.Pixbuf buf=new Gdk.Pixbuf(tgaFile);
					Console.Write("Decoded\n");
					int x;
                    if (target_image!=null) // this has managed to get set to null
                    {
                        if (target_image.Pixbuf != null)
                        {
                            x = target_image.Pixbuf.Width;
                            target_image.Pixbuf = buf.ScaleSimple(img_width, img_height, Gdk.InterpType.Bilinear);
                        }
                    }
				}
				catch(Exception e)
				{
					Console.Write("*** Image decode blew whist trying to write image into pixbuf ***\n");
					Console.WriteLine(e.Message);
				}
              });
		}	
	}
}
