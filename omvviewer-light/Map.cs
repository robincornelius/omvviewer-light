/*
omvviewerlight a Text based client to metaverses such as Linden Labs Secondlife(tm)
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
		bool running=true;
	    Gtk.Image basemap;

		int rowstride;
		int channels;
		int width;
		int height;
		static Gtk.Image avatar=new Gtk.Image(MainClass.GetResource("map_avatar_8.tga"));
        static Gtk.Image avatar_me = new Gtk.Image(MainClass.GetResource("map_avatar_me_8.tga"));
        static Gtk.Image avatar_above = new Gtk.Image(MainClass.GetResource("map_avatar_above_8.tga"));
        static Gtk.Image avatar_below = new Gtk.Image(MainClass.GetResource("map_avatar_below_8.tga"));
        static Gtk.Image avatar_friend = new Gtk.Image(MainClass.GetResource("map_avatar_friend_8.tga"));
        static Gtk.Image avatar_friend_below = new Gtk.Image(MainClass.GetResource("map_avatar_friend_above_8.tga"));
        static Gtk.Image avatar_friend_above = new Gtk.Image(MainClass.GetResource("map_avatar_friend_below_8.tga"));
        static Gtk.Image avatar_target = new Gtk.Image(MainClass.GetResource("map_avatar_target_8.tga"));

		UUID lastsim = new UUID();
		Vector3 targetpos;
		UUID terrain_map_ID;
		UUID objects_map_ID;
		UUID forsale_map_ID;
	    Gtk.Image terrian_map;
	    Gtk.Image objects_map;
	    Gtk.Image forsale_map;
		
		public Map()
		{           
			this.Build();
			
			this.terrain_map_ID=UUID.Zero;
			this.objects_map_ID=UUID.Zero;
			this.forsale_map_ID=UUID.Zero;
			
			MainClass.client.Network.OnCurrentSimChanged += new OpenMetaverse.NetworkManager.CurrentSimChangedCallback(onNewSim);
			MainClass.client.Objects.OnNewAvatar += new OpenMetaverse.ObjectManager.NewAvatarCallback(onNewAvatar);
			MainClass.client.Objects.OnObjectUpdated += new OpenMetaverse.ObjectManager.ObjectUpdatedCallback(onUpdate);
            MainClass.client.Self.OnTeleport += new OpenMetaverse.AgentManager.TeleportCallback(onTeleport);
			MainClass.client.Grid.OnGridLayer += new OpenMetaverse.GridManager.GridLayerCallback(onGridLayer);
			MainClass.client.Grid.OnGridRegion += new OpenMetaverse.GridManager.GridRegionCallback(onGridRegion);
			MainClass.client.Grid.OnGridItems += new OpenMetaverse.GridManager.GridItemsCallback(onGridItems);
			AutoPilot.onAutoPilotFinished += new AutoPilot.AutoPilotFinished(onAutoPilotFinished);
			Gtk.Timeout.Add(10000, kickrefresh);			
			this.targetpos.X=-1;
			
			if(MainClass.client!=null)
			{
				if(MainClass.client.Network.LoginStatusCode==OpenMetaverse.LoginStatus.Success)
                {	
					    //MainClass.client.Grid.RequestMapRegion(MainClass.client.Network.CurrentSim.Name,GridLayerType.Terrain);
					    MainClass.client.Grid.RequestMapRegion(MainClass.client.Network.CurrentSim.Name,GridLayerType.Objects);
					   this.label1.Text = MainClass.client.Network.CurrentSim.Name;
       	         }
             }	
		}

		~Map()
		{
			Console.WriteLine("Map Cleaned up");
		}		
		
		new public void Dispose()
		{
			running=false;
			MainClass.client.Network.OnCurrentSimChanged -= new OpenMetaverse.NetworkManager.CurrentSimChangedCallback(onNewSim);
			MainClass.client.Objects.OnNewAvatar -= new OpenMetaverse.ObjectManager.NewAvatarCallback(onNewAvatar);
			MainClass.client.Objects.OnObjectUpdated -= new OpenMetaverse.ObjectManager.ObjectUpdatedCallback(onUpdate);
            MainClass.client.Self.OnTeleport -= new OpenMetaverse.AgentManager.TeleportCallback(onTeleport);
			MainClass.client.Grid.OnGridLayer -= new OpenMetaverse.GridManager.GridLayerCallback(onGridLayer);
			MainClass.client.Grid.OnGridRegion -= new OpenMetaverse.GridManager.GridRegionCallback(onGridRegion);
			AutoPilot.onAutoPilotFinished -= new AutoPilot.AutoPilotFinished(onAutoPilotFinished);
			
			//Finalize();
			//System.GC.SuppressFinalize(this);
		}

		
        bool kickrefresh()
        {

			if(running==false)
				return false;

            Gtk.Application.Invoke(delegate
               {
                   if (MainClass.client.Network.CurrentSim != null)
                       drawavs();
               });

            return true;

        }
		
		void onGridItems(GridItemType type, List<OpenMetaverse.GridItem> items)
		{
	
			
		}
		
		void onGridRegion(GridRegion region)
		{
			getmap();
			Console.Write("Got grid region reply, requesting texture :"+region.MapImageID.ToString()+"\n");
			
			Console.WriteLine("Assuming this is an objects overlay");
			this.objects_map_ID=region.MapImageID;
			Gdk.Pixbuf pb= MainClass.GetResource("trying.tga");
			objects_map = new Gtk.Image(pb);
			this.image.Pixbuf=pb;
			
			new TryGetImage(this.objects_map,region.MapImageID,350,350);
		//	new TryGetImage(this.objects_map,MainClass.client.Network.CurrentSim.TerrainDetail0,350,350);

			rowstride = objects_map.Pixbuf.Rowstride;
	        channels = objects_map.Pixbuf.NChannels;
	        width = objects_map.Pixbuf.Width;
	        height = objects_map.Pixbuf.Height;
		}
				
		void onGridLayer(GridLayer layer)
	    {
			//layer.ImageID
			Console.Write("Got grid layer reply, requesting texture :"+layer.ImageID.ToString()+"\n");	
		}

		void onTeleport(string Message, OpenMetaverse.AgentManager.TeleportStatus status,OpenMetaverse.AgentManager.TeleportFlags flags)
	    {
			if(status==OpenMetaverse.AgentManager.TeleportStatus.Finished)
			{

                if (MainClass.client.Network.CurrentSim.ID == lastsim)
                    return;
				
			this.terrain_map_ID=UUID.Zero;
			this.objects_map_ID=UUID.Zero;
			this.forsale_map_ID=UUID.Zero;

				MainClass.client.Grid.RequestMapRegion(MainClass.client.Network.CurrentSim.Name,GridLayerType.Objects);
      //          MainClass.client.Grid.RequestMapLayer(GridLayerType.Terrain);                
       //         MainClass.client.Grid.RequestMapLayer(GridLayerType.Objects);
				

				Gtk.Application.Invoke(delegate
                {
                    drawavs();
                });

                if(MainClass.client.Network.CurrentSim !=null)
                    lastsim = MainClass.client.Network.CurrentSim.ID;
			}
	    }
				
		void onUpdate(Simulator simulator, ObjectUpdate update,ulong regionHandle, ushort timeDilation)
		{

            Gtk.Application.Invoke(delegate
				{
				
					lock(MainClass.client.Network.CurrentSim.ObjectsAvatars.Dictionary)
                        if (MainClass.client.Network.CurrentSim.ObjectsAvatars.Dictionary.ContainsKey(update.LocalID))
                            drawavs();
                });

}
		
		void onNewAvatar(Simulator simulator, Avatar avatar, ulong regionHandle, ushort timeDilation)
		{
            Gtk.Application.Invoke(delegate
                {
                    drawavs();
                });
		}

			
		void drawavs()
		{

		  basemap=this.objects_map;
			
          if (basemap == null)
              return;

          if (basemap.Pixbuf == null)
			return;
          
          Gdk.Pixbuf buf;

			lock(basemap)
            {
                try
				{
                    buf = (Gdk.Pixbuf)basemap.Pixbuf.Clone();
                }
                catch
                {
                    Console.WriteLine("Map, caught exception cloning pixbuf\n");
                    return;
                }
                 

				int myz=(int)MainClass.client.Self.SimPosition.Z;

                lock (MainClass.client.Network.CurrentSim.ObjectsAvatars.Dictionary)
                {
					//LOCKING ISSUES HERE STILL!
					List <uint> removelist=new List<uint>();
					
					foreach (KeyValuePair<uint, Avatar> kvp in MainClass.client.Network.CurrentSim.ObjectsAvatars.Dictionary)
                    {
                        if (kvp.Value.LocalID != MainClass.client.Self.LocalID)
                        {
                            Vector3 pos;
                            if (kvp.Value.ParentID != 0)
                            {
                                if (!MainClass.client.Network.CurrentSim.ObjectsPrimitives.Dictionary.ContainsKey(kvp.Value.ParentID))
                                {
									Console.WriteLine("Could not find parent prim for AV, killing\n");
									removelist.Add(kvp.Value.LocalID);
									continue;
                                }
                                Primitive parent = MainClass.client.Network.CurrentSim.ObjectsPrimitives.Dictionary[kvp.Value.ParentID];
                                pos = Vector3.Transform(kvp.Value.Position, Matrix4.CreateFromQuaternion(parent.Rotation)) + parent.Position;
                            }
                            else
                            {
                                pos = kvp.Value.Position;
                            }

							if(MainClass.client.Friends.FriendList.Dictionary.ContainsKey(kvp.Value.ID))
							{
                            if (pos.Z - myz > 5)
                                showme(buf, avatar_friend_below.Pixbuf, pos);
                            else if (pos.Z - myz < -5)
                                showme(buf, avatar_friend_above.Pixbuf, pos);
                            else
                                showme(buf, avatar_friend.Pixbuf, pos);
								
							}
							else
							{
                            if (pos.Z - myz > 5)
                                showme(buf, avatar_below.Pixbuf, pos);
                            else if (pos.Z - myz < -5)
                                showme(buf, avatar_above.Pixbuf, pos);
                            else
                                showme(buf, avatar.Pixbuf, pos);
							}
							
                        }
                    }
					
					foreach(uint id in removelist)
					{
                          MainClass.client.Network.CurrentSim.ObjectsAvatars.Dictionary.Remove(id);
					}
                }

				//Draw me last so i am on top of the mele
				showme(buf,avatar_me.Pixbuf,MainClass.client.Self.SimPosition);				
				
				if(this.targetpos.X!=-1)
					showme(buf,avatar_target.Pixbuf,this.targetpos);				
						
             }

            lock (image)
            {
                image.Pixbuf = buf;
            }

			//		Gtk.Application.Invoke(delegate
             //   {
                    lock (image)
                    {
                        image.QueueDraw();
                    }
			//});
			
		}

		void onNewSim(Simulator lastsim)
	    {
			MainClass.win.map_widget=this;
			Console.Write("New simulator :"+MainClass.client.Network.CurrentSim.Name +" requesting grid layer for terrain \n");
		    MainClass.client.Grid.RequestMapRegion(MainClass.client.Network.CurrentSim.Name,GridLayerType.Objects);
			Gtk.Application.Invoke(delegate
                {            
                  this.label1.Text = MainClass.client.Network.CurrentSim.Name;
});
        }
	
		void showme(Gdk.Pixbuf buf,Gdk.Pixbuf src,Vector3 pos)
		{
			int tx,ty;
			tx=(int)pos.X;
			ty=(int)(256.0-pos.Y);
			
            tx=(int)(((double)tx/256.0)*(double)buf.Width);
            ty=(int)(((double)ty/256.0)*(double)buf.Height);

			tx=tx-4;
			ty=ty-4;
			
			if(tx>buf.Width-8)
				tx=buf.Width-8;
			
			if(ty>buf.Height-8)
			   ty=buf.Height-8;
			
			if(tx<8)
				tx=8;
			
			if(ty<8)
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
			
			if(x<0 || x>=width)
				return;
			
			if(y<0 || y>=height)
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
			//Gtk.Application.Invoke(delegate {		
			//Gdk.Pixbuf pb=new Pixbuf(MainClass.GetResource("trying.tga"),0,0,256,256);
          
            //lock (image)
            //{
              //  this.image.Pixbuf = pb.ScaleSimple(350,350,InterpType.Bilinear);
            //}
			//Thread mapdownload= new Thread(new ThreadStart(this.getmap_threaded));                               
			//mapdownload.Start();
			//});
			
		}
		
		void getmap_threaded()
		{
			  HttpWebRequest request = null;
              HttpWebResponse response = null;
              String imgURL = "";
	          GridRegion currRegion;			
			  MainClass.client.Grid.GetGridRegion(MainClass.client.Network.CurrentSim.Name, GridLayerType.Objects, out currRegion);
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


                if (basemap != null)
                {
                    lock (basemap)
                    {
                        lock (image)
                        {
                            basemap = new Gtk.Image(response.GetResponseStream());
                        }
			}
			}
                else
                    basemap = new Gtk.Image(response.GetResponseStream());
                
          
						lock (basemap)
						{
						lock (image)
                    {
                 
                    image.Pixbuf = (Gdk.Pixbuf)basemap.Pixbuf.ScaleSimple(350,350,InterpType.Bilinear);
                    basemap.Pixbuf=basemap.Pixbuf.ScaleSimple(350,350,InterpType.Bilinear);                                                         
                    }
                    rowstride = basemap.Pixbuf.Rowstride;
                    channels = basemap.Pixbuf.NChannels;
                    width = basemap.Pixbuf.Width;
                    height = basemap.Pixbuf.Height;
                }
                Gtk.Application.Invoke(delegate
                   {
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
			
		protected virtual void OnEventbox1ButtonPressEvent (object o, Gtk.ButtonPressEventArgs args)
		{
			Console.WriteLine("EVENT BOX CLICK"+args.Event.X.ToString()+","+args.Event.Y.ToString());
			Vector3 pos;
			pos.X=(float)(256.0*(args.Event.X/this.image.Pixbuf.Width));
			pos.Y=(float)(256.0*(args.Event.Y/this.image.Pixbuf.Height));
			pos.Z=0;

			pos.Y=255-pos.Y;

			targetpos=pos;
			
			if(MainClass.win.tp_target_widget!=null)
				MainClass.win.tp_target_widget.settarget(pos);
			
			this.drawavs();
			
		}
		
		public void showtarget(Vector3 pos)
		{
			targetpos=pos;
			this.drawavs();
		}
		
		void onAutoPilotFinished()
		{
			targetpos.X=-1;
			this.drawavs();			
		}
	}
}
