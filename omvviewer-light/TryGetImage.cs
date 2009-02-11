/*
omvviewerlight a Text based client to metaverses such as Linden Labs Secondlife(tm)
    Copyright (C) 2008,2009  Robin Cornelius <robin.cornelius@gmail.com>

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 2 of the License, or
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
using System.Threading;
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
        ImageDownload this_image;
        AssetTexture this_asset;
		bool scale=false;
      
        public delegate void Decodecomplete();
        public event Decodecomplete OnDecodeComplete;

        public TryGetImage(Gtk.Image target, UUID asset, bool asynccallback)
		{
			TryGetImageWork(target,asset,true,256,256,false);
		}
		
		public TryGetImage(Gtk.Image target,UUID asset,int width,int height,bool asynccallback)
		{
			TryGetImageWork(target,asset,false,width,height,false);
		}

        public void TryGetImageWork(Gtk.Image target, UUID asset, bool auto, int width, int height, bool asynccallback)
		{
			if(target==null)
				return;
			
			MainClass.client.Assets.OnImageReceived += new OpenMetaverse.AssetManager.ImageReceivedCallback(onGotImage);
			MainClass.client.Assets.OnImageReceiveProgress += new OpenMetaverse.AssetManager.ImageReceiveProgressCallback(onProgress);
			scale=auto;
			
			target_asset=asset;
			target_image=target;
            img_width = width;
            img_height = height;

            if (asynccallback)
            {
                return;
            }

            dowork();	
	    }

        public void go()
        {
            dowork();
        }

        void dowork()
        {
            Gdk.Pixbuf buf = MainClass.GetResource("trying.tga");

            if (scale)
                target_image.Pixbuf = buf;
            else
                target_image.Pixbuf = buf.ScaleSimple(img_width, img_height, Gdk.InterpType.Bilinear);

            if (target_asset != UUID.Zero)
                MainClass.client.Assets.RequestImage(target_asset, OpenMetaverse.ImageType.Normal, 99999000.0f, 0, 0);	

        }
		
		public void abort()
	   {
			MainClass.client.Assets.OnImageReceived -= new OpenMetaverse.AssetManager.ImageReceivedCallback(onGotImage);
			MainClass.client.Assets.OnImageReceiveProgress -= new OpenMetaverse.AssetManager.ImageReceiveProgressCallback(onProgress);
       }
			                                               
        void onProgress(UUID image, int recieved, int total,int lastpacket)
		{
			if(target_asset!=image)
			    return;

            progress(target_image.Pixbuf,(float)total/(float)lastpacket);
	}
		
		unsafe void progress(Gdk.Pixbuf bufdest,float progress)
		{
            if (bufdest == null)
                return;

			byte * pixels=(byte *)bufdest.Pixels;
		    int width=bufdest.Width;
			int height=bufdest.Height;
			int rowstride=bufdest.Rowstride;
			int channels=bufdest.NChannels;
			byte * p;		
			int y,x;
			
			//Console.WriteLine("Progress is "+progress.ToString());
			int widthx=(int)((float)width*progress);
		    //Console.WriteLine("Width is  is "+widthx.ToString());
	
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
					p[3]=255;
                }	
		    }
            Gtk.Application.Invoke(delegate
            {
                target_image.QueueDraw();
            });		
        }

        void decodethread()
        {

            if (!this_image.Success)
            {
                Console.Write("Failed to download image\n");
                return;
            }

            if (this_asset.AssetID != target_asset)
                return;

            MainClass.client.Assets.OnImageReceived -= new OpenMetaverse.AssetManager.ImageReceivedCallback(onGotImage);
            MainClass.client.Assets.OnImageReceiveProgress -= new OpenMetaverse.AssetManager.ImageReceiveProgressCallback(onProgress);

            Console.Write("Downloaded asset " + this_asset.AssetID.ToString() + "\n");
            byte[] tgaFile = null;
            try
            {
                ManagedImage imgData;
                OpenJPEG.DecodeToImage(this_image.AssetData, out imgData);
                tgaFile = imgData.ExportTGA();
            }
            catch (Exception e)
            {
                Console.Write("\n*****************\n" + e.Message + "\n");
            }
           
			Gdk.Pixbuf buf;
				
			if(this.scale)
				buf = new Gdk.Pixbuf(tgaFile);
			else
				buf = new Gdk.Pixbuf(tgaFile).ScaleSimple(img_width, img_height, Gdk.InterpType.Bilinear);;
					
            Console.Write("Decoded\n");

            Gtk.Application.Invoke(delegate
            {
                try
                {
                    if (target_image != null) // this has managed to get set to null
                    {
                        if (target_image.Pixbuf != null)
                        {
                            target_image.Pixbuf = buf;
                            Console.WriteLine("TryGetImage:: Image Done queuing for a redraw");
                            target_image.QueueDraw();
                            if (OnDecodeComplete!=null)
                            {
                                try
                                {
                                    OnDecodeComplete();
                                    OnDecodeComplete = null;
                                }
                                catch
                                {

                                }
                            }
						}
                    }
                }
                catch (Exception e)
                {
                    Console.Write("*** Image decode blew whist trying to write image into pixbuf ***\n");
                    Console.WriteLine(e.Message);
                }
            });
        }
		                                  
		void onGotImage(ImageDownload image,AssetTexture asset)
		{
            if (asset == null || image == null)
            {
                Console.WriteLine("Try get image got a null asset");
                return;
            }
            this_image = image;
            this_asset = asset;
            Thread decode = new Thread(new ThreadStart(this.decodethread));
            Console.WriteLine("Begining a decode thread for asset "+asset.AssetID.ToString());
            decode.Start();
		}	
	}
}
