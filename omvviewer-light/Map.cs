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

// Map.cs created with MonoDevelop
// User: robin at 23:10 12/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Net;
using System.Collections.Generic;
using System.Threading;
using OpenMetaverse;
using omvviewerlight;
using System.Drawing;
using Gdk;
using Gtk;

namespace omvviewerlight
{
	public partial class Map : Gtk.Bin
	{
		private const String MAP_IMG_URL = "http://secondlife.com/apps/mapapi/grid/map_image/";
		private const int GRID_Y_OFFSET = 1279;

		private Dictionary<uint, Avatar> avs = new Dictionary<uint, Avatar>();
		
		Gtk.Image basemap;
		int rowstride;
		int channels;
		int width;
		int height;
		Gtk.Image avatar=new Gtk.Image("map_avatar_8.tga");
		Gtk.Image avatar_me=new Gtk.Image("map_avatar_me_8.tga");
		Gtk.Image avatar_above=new Gtk.Image("map_avatar_above_8.tga");
		Gtk.Image avatar_below=new Gtk.Image("map_avatar_below_8.tga");
        UUID lastsim = new UUID();
		
		public Map()
		{           
			this.Build();
			MainClass.client.Network.OnCurrentSimChanged += new OpenMetaverse.NetworkManager.CurrentSimChangedCallback(onNewSim);
			MainClass.client.Objects.OnNewAvatar += new OpenMetaverse.ObjectManager.NewAvatarCallback(onNewAvatar);
			MainClass.client.Objects.OnObjectKilled += new OpenMetaverse.ObjectManager.KillObjectCallback(onKillObject);
			MainClass.client.Objects.OnObjectUpdated += new OpenMetaverse.ObjectManager.ObjectUpdatedCallback(onUpdate);
            MainClass.client.Self.OnTeleport += new OpenMetaverse.AgentManager.TeleportCallback(onTeleport);
			//MainClass.client.Grid.OnGridLayer += new OpenMetaverse.GridManager.GridLayerCallback(onGridLayer);
			MainClass.client.Grid.OnGridRegion += new OpenMetaverse.GridManager.GridRegionCallback(onGridRegion);
							
		
		}
		
		void onGridRegion(GridRegion region)
		{
			Console.Write("Got grid layer reply, requesting texture :"+region.MapImageID.ToString()+"\n");
			Gdk.Pixbuf pb= new Gdk.Pixbuf("trying.tga");
			
			this.basemap=new Gtk.Image(pb);
			TryGetImage img=new TryGetImage(basemap,region.MapImageID,256,256);
			//getmap();
		}
				
		void onGridLayer(GridLayer layer)
	    {
			
		}

		void onTeleport(string Message, OpenMetaverse.AgentManager.TeleportStatus status,OpenMetaverse.AgentManager.TeleportFlags flags)
	    {


			if(status==OpenMetaverse.AgentManager.TeleportStatus.Finished)
			{

                if (MainClass.client.Network.CurrentSim.ID == lastsim)
                    return;
               
    		    Gtk.Application.Invoke(delegate {

                    lock (avs)
                    {
                        avs.Clear();
                    }
                    
                    drawavs();
				});

                if(MainClass.client.Network.CurrentSim !=null)
                    lastsim = MainClass.client.Network.CurrentSim.ID;
			}
	    }
				
		void onUpdate(Simulator simulator, ObjectUpdate update,ulong regionHandle, ushort timeDilation)
		{
          
			if(avs.ContainsKey(update.LocalID))
			{
                lock (avs)
                {
                    avs[update.LocalID].Position = update.Position;
                }
				drawavs();
			}
		}
		
		void onNewAvatar(Simulator simulator, Avatar avatar, ulong regionHandle, ushort timeDilation)
		{

			if(!avs.ContainsKey(avatar.LocalID))
			{
                lock (avs)
                {
                    avs.Add(avatar.LocalID, avatar);
                }
                
                drawavs();			
			}
		}

		void onKillObject(Simulator simulator, uint objectID)
		{
			if(avs.ContainsKey(objectID))
			{
                lock (avs)
                {
                    avs.Remove(objectID);
                }
                    drawavs();
			}
			
		}
		
		void drawavs()
		{
			if(basemap==null)
					return;

            if (basemap.Pixbuf == null)
                return;

				Gdk.Pixbuf buf=(Gdk.Pixbuf)basemap.Pixbuf.Clone();
                Gtk.Application.Invoke(delegate
                {		
				    showme(buf,avatar_me.Pixbuf,MainClass.client.Self.SimPosition);				
				});

				int myz=(int)MainClass.client.Self.SimPosition.Z;

                lock (avs)
                {
                    foreach (KeyValuePair<uint, Avatar> kvp in avs)
                    {
                        if (kvp.Value.LocalID != MainClass.client.Self.LocalID)
                        {
                            Gtk.Application.Invoke(delegate
                            {
                                if (kvp.Value.Position.Z - myz > 5)
                                    showme(buf, avatar_above.Pixbuf, kvp.Value.Position);
                                else if (kvp.Value.Position.Z - myz < -5)
                                    showme(buf, avatar_below.Pixbuf, kvp.Value.Position);
                                else
                                    showme(buf, avatar.Pixbuf, kvp.Value.Position);
                            });
                        }
                    }
                }
                Gtk.Application.Invoke(delegate
                {
				    image.Pixbuf=buf;
				    image.QueueDraw();
                 });
			
		}
		

		void onNewSim(Simulator lastsim)
	    {
			Console.Write("New simulator :"+MainClass.client.Network.CurrentSim.Name +" requesting grid layer for terrain \n");
			MainClass.client.Grid.RequestMapRegion(MainClass.client.Network.CurrentSim.Name,GridLayerType.Terrain);
     
		}
	
		void showme(Gdk.Pixbuf buf,Gdk.Pixbuf src,Vector3 pos)
		{
			int tx,ty;
			tx=(int)pos.X;
			ty=(int)(255.0-pos.Y);
			
			tx=tx-4;
			ty=ty-4;
			
			if(tx>245)
				tx=247;
			
			if(ty>245)
			   ty=247;
			
			if(tx<0)
				tx=0;
			
			if(ty<0)
				ty=8;
			
			mergedrawxy(buf,src,tx,ty);
						
		}

		unsafe void mergedrawxy(Gdk.Pixbuf bufdest,Gdk.Pixbuf src,int x,int y)
		{
    
			sbyte * pixels=(sbyte *)bufdest.Pixels;
			sbyte * spixels=(sbyte *)src.Pixels;
			sbyte * p;			
			sbyte * ps;			
			
			int srcwidth=src.Width;
			int srcheight=src.Height;
			int srcrowsstride=src.Rowstride;
			int schannels=src.NChannels;
			
			if(x<0 || x>width)
				return;
			
			if(y<0 || y>height)
				return;
					
			for(int sx=0;sx<srcwidth;sx++)
			{
				for(int sy=0;sy<srcheight;sy++)
				{				
					ps=spixels+((sy)*srcrowsstride)+((sx)* schannels);
					p=pixels+((sy+y)*rowstride)+((sx+x)* channels);
					
					if(ps[3]!=0) //Alpha merge
					{
						p[0]=ps[0];
						p[1]=ps[1];
						p[2]=ps[2];
					}
				}
			}	
		}
				
		void getmap()
		{
			Gtk.Application.Invoke(delegate {		
			Gdk.Pixbuf pb= new Gdk.Pixbuf("trying.tga");
			this.image.Pixbuf=pb;
			Thread mapdownload= new Thread(new ThreadStart(this.getmap_threaded));                               
			mapdownload.Start();
			});
			
		}
		
		void getmap_threaded()
		{
			  HttpWebRequest request = null;
              HttpWebResponse response = null;
              String imgURL = "";
	          GridRegion currRegion;			
			  MainClass.client.Grid.GetGridRegion(MainClass.client.Network.CurrentSim.Name, GridLayerType.Terrain, out currRegion);
		try
            {
                //Form the URL using the sim coordinates
                imgURL = MAP_IMG_URL + currRegion.X.ToString() + "-" +
                        (GRID_Y_OFFSET - currRegion.Y).ToString() + "-1-0.jpg";
                //Make the http request
                request = (HttpWebRequest)HttpWebRequest.Create(imgURL);
                request.Timeout = 5000;
                request.ReadWriteTimeout = 20000;
				response = (HttpWebResponse)request.GetResponse();

				Gtk.Application.Invoke(delegate {		
					
				    basemap=new Gtk.Image(response.GetResponseStream());
				    image.Pixbuf=(Gdk.Pixbuf)basemap.Pixbuf.Clone();
				    rowstride=basemap.Pixbuf.Rowstride;
				    channels=basemap.Pixbuf.NChannels;
				    width=basemap.Pixbuf.Width;
				    height=basemap.Pixbuf.Height;		
				

                drawavs(); //already deligated inside	
                });
                return;
					
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());

                Gtk.Application.Invoke(delegate
                {
                    Gtk.MessageDialog msg = new Gtk.MessageDialog(MainClass.win, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "Error Downloading Web Map Image");
                    msg.Run();
                    msg.Destroy();
                });
				return ;
            }	
		}
	}
}
