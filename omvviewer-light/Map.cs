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
	[System.ComponentModel.ToolboxItem(true)]	
	public partial class Map : Gtk.Bin
	{
		bool running=true;
	    Gtk.Image basemap;
		Gtk.Image scalemap;
		GridRegion current_region;

        int optimal_width = 0;

		static Gtk.Image avatar=new Gtk.Image(MainClass.GetResource("map_avatar_8.png"));
        static Gtk.Image avatar_me = new Gtk.Image(MainClass.GetResource("map_avatar_me_8.png"));
        static Gtk.Image avatar_above = new Gtk.Image(MainClass.GetResource("map_avatar_above_8.png"));
        static Gtk.Image avatar_below = new Gtk.Image(MainClass.GetResource("map_avatar_below_8.png"));
        static Gtk.Image avatar_friend = new Gtk.Image(MainClass.GetResource("map_avatar_friend_8.png"));
        static Gtk.Image avatar_friend_below = new Gtk.Image(MainClass.GetResource("map_avatar_friend_above_8.png"));
        static Gtk.Image avatar_friend_above = new Gtk.Image(MainClass.GetResource("map_avatar_friend_below_8.png"));
        static Gtk.Image avatar_target = new Gtk.Image(MainClass.GetResource("map_avatar_target_8.png"));

		UUID lastsim = new UUID();
		
        static Vector3 targetpos;
        static ulong targetID;

        UUID terrain_map_ID;
		UUID objects_map_ID;
		UUID forsale_map_ID;
	    Gtk.Image objects_map;
		int width,height;
		int lastwidth,lastheight;

        Simulator this_maps_sim = null;
        UUID this_maps_regionID;
        ulong this_maps_region_handle=0;

        bool requested = false;

        public event ClickMap onclickMap;
        public delegate void ClickMap();
		
		public void set_optimal_size(int size)
		{
            if (size < 25)
                size = 25;
			optimal_width=size;
			if(this.scalemap!=null && this.scalemap.Pixbuf!=null)
			{
				this.scalemap.Pixbuf = this.objects_map.Pixbuf.ScaleSimple(size,size, InterpType.Bilinear);
          		drawavs();
			}
		}

        public void SetGridRegion(UUID regionID,ulong region_handle)
        {
            this_maps_regionID = regionID;
            Simulator sim;
            sim=MainClass.client.Network.Simulators.Find(delegate (Simulator dsim){ return (dsim.Handle == region_handle); });

            if (sim != null)
            {
                SetMapSim(sim);     
            }

            this_maps_region_handle = region_handle;
            update_map_for_region(region_handle);
        }

        public void SetAsWater()
        {
            this_maps_sim = null;
            this_maps_regionID = UUID.Zero;
            objects_map_ID = UUID.Zero;
            terrain_map_ID = UUID.Zero;
            this_maps_region_handle = 0;
            current_region.Name = "Empty";

            objects_map = new Gtk.Image(MainClass.GetResource("water.png"));

            if (this.scalemap != null)
            {
                if(height>25 && width>25)
                    this.scalemap.Pixbuf = this.objects_map.Pixbuf.ScaleSimple(height, width, InterpType.Bilinear);
            }
        }

        void SetMapSim(Simulator sim)
        {
            this_maps_sim=sim;
       
        }
		
		public void setsize(int size)
		{
			optimal_width=size;
			height=size;
			width=size;
			this.scalemap.Pixbuf = this.objects_map.Pixbuf.ScaleSimple(size,size, InterpType.Bilinear);
		}

		public Map()
		{           
			this.Build();
            objects_map = new Gtk.Image(MainClass.GetResource("water.png"));
         
			lastwidth=-1;
			lastheight=-1;
			
				
			this.eventbox1.SizeAllocated += delegate(object o, SizeAllocatedArgs args) {
			height=args.Allocation.Height;	
			width=args.Allocation.Width;
			
					
				
                Gtk.Application.Invoke(delegate
                {
                        if (objects_map != null && objects_map.Pixbuf != null && (lastheight != height || lastwidth != width))
                        {
                            this.scalemap = new Gtk.Image();

                            int size;
                        
							if(optimal_width==0)
                            	size = height < width ? height : width;
							else
                        		size=optimal_width;

                            if (size < 25)
                                size = 25;

                            this.scalemap.Pixbuf = this.objects_map.Pixbuf.ScaleSimple(size,size, InterpType.Bilinear);
                            lastheight = height;
                            lastwidth = width;
                            drawavs();
                       }	
                });
			};
			
			
			
			this.terrain_map_ID=UUID.Zero;
			this.objects_map_ID=UUID.Zero;
			this.forsale_map_ID=UUID.Zero;
			


            MainClass.onRegister += new MainClass.register(MainClass_onRegister);
            MainClass.onDeregister += new MainClass.deregister(MainClass_onDeregister);
            if(MainClass.client != null ) { MainClass_onRegister(); }


            GLib.Timeout.Add(10000, kickrefresh);			
			targetpos.X=-1;
			
			if(MainClass.client!=null)
			{
				if(MainClass.client.Network.LoginStatusCode==OpenMetaverse.LoginStatus.Success)
                {
                       //  MainClass.client.Grid.RequestMapRegion(this_maps_sim.Name, GridLayerType.Objects);
                       //  this.label1.Text = this_maps_sim.Name;
					   //	MainClass.win.map_widget=this;
       	         }
             }	
		}

        void MainClass_onDeregister()
        {

            running = false;
            if (MainClass.client != null)
            {
                MainClass.client.Network.SimChanged -= new EventHandler<SimChangedEventArgs>(Network_SimChanged);
                MainClass.client.Self.TeleportProgress -= new EventHandler<TeleportEventArgs>(Self_TeleportProgress);
                MainClass.client.Grid.GridRegion -= new EventHandler<GridRegionEventArgs>(Grid_GridRegion);
                MainClass.client.Grid.CoarseLocationUpdate -= new EventHandler<CoarseLocationUpdateEventArgs>(Grid_CoarseLocationUpdate);
                
             }
            AutoPilot.onAutoPilotFinished -= new AutoPilot.AutoPilotFinished(onAutoPilotFinished);

        }

        void MainClass_onRegister()
        {

            lastsim = UUID.Zero;
            requested = false;

            MainClass.client.Network.SimChanged += new EventHandler<SimChangedEventArgs>(Network_SimChanged);
            MainClass.client.Self.TeleportProgress += new EventHandler<TeleportEventArgs>(Self_TeleportProgress);
            MainClass.client.Grid.GridRegion += new EventHandler<GridRegionEventArgs>(Grid_GridRegion);
            MainClass.client.Grid.CoarseLocationUpdate += new EventHandler<CoarseLocationUpdateEventArgs>(Grid_CoarseLocationUpdate);

            AutoPilot.onAutoPilotFinished += new AutoPilot.AutoPilotFinished(onAutoPilotFinished);
       
        }







        void Grid_CoarseLocationUpdate(object sender, CoarseLocationUpdateEventArgs e)
        {
            Gtk.Application.Invoke(delegate
            {
                drawavs();
            });
        }

    
		~Map()
		{
			Console.WriteLine("Map Cleaned up");
		}		
		
        new public void Dispose()
        {
            Console.WriteLine("Disposing of the map control");

            running = false;

            MainClass.onRegister -= new MainClass.register(MainClass_onRegister);
            MainClass.onDeregister -= new MainClass.deregister(MainClass_onDeregister);
            MainClass_onDeregister();

            AutoPilot.onAutoPilotFinished -= new AutoPilot.AutoPilotFinished(onAutoPilotFinished);
	
        }

		
        bool kickrefresh()
        {

			if(running==false)
				return false;

            if (this_maps_sim != null)
	            drawavs();
         	
            return true;

        }
		
        void Grid_GridRegion(object sender, GridRegionEventArgs e)
		{
            
            if (current_region.RegionHandle == e.Region.RegionHandle)
            {
                //Nothing to do;
                return;
            }

			current_region=e.Region;
			this.objects_map_ID=e.Region.MapImageID;
			Gdk.Pixbuf pb= MainClass.GetResource("trying.png");
			objects_map = new Gtk.Image(pb);
			this.image.Pixbuf=pb;	

			TryGetImage tgi=new TryGetImage(this.objects_map,e.Region.MapImageID,350,350,true);
            tgi.OnDecodeComplete += new TryGetImage.Decodecomplete(delegate()
			{
				  Gtk.Application.Invoke(delegate
            {
				lastheight=-1;
				lastwidth=-1;
                drawavs();
            });
				
			});
            tgi.go();
		}
		
		void onGridRegion(GridRegion region)
		{
            lock (MainClass.win.grid_regions)
            {
                if (!MainClass.win.grid_regions.ContainsKey(region.RegionHandle))
                {
                    MainClass.win.grid_regions.Add(region.RegionHandle, region);
                }

                update_map_for_region(region.RegionHandle);
            }
            
		}

        void update_map_for_region(ulong regionID)
        {


            if (regionID != this_maps_region_handle)
                return;

            if (requested == true)
                return; 

            GridRegion region;
            if(MainClass.win.grid_regions.TryGetValue(regionID,out region))
            {
                current_region=region;
                requested = true;
				
				Console.Write("Got grid region reply, requesting texture :"+region.MapImageID.ToString()+"\n");
				
				Console.WriteLine("Assuming this is an objects overlay");
				this.objects_map_ID=region.MapImageID;
				Gdk.Pixbuf pb= MainClass.GetResource("trying.png");
				objects_map = new Gtk.Image(pb);

				this.image.Pixbuf=pb;
                TryGetImage tgi=new TryGetImage(this.objects_map, region.MapImageID, 350, 350, true);
                tgi.OnDecodeComplete += delegate
                {
                    Gtk.Application.Invoke(delegate
                    {
                        this.scalemap = new Gtk.Image();
                        int size = height < width ? height : width;
                        if (size < 25)
                            size = 25; //meh!

                        this.scalemap.Pixbuf = this.objects_map.Pixbuf.ScaleSimple(size, size, InterpType.Bilinear);
                        drawavs();
                    });
                };
  
                tgi.go();
            }
        }

        void Self_TeleportProgress(object sender, TeleportEventArgs e)
	    {
			if(e.Status==OpenMetaverse.TeleportStatus.Finished)
			{

                if (lastsim != null && this_maps_sim!=null)
                    if (this_maps_sim.ID == lastsim)
                        return;
				
			this.terrain_map_ID=UUID.Zero;
			this.objects_map_ID=UUID.Zero;
			this.forsale_map_ID=UUID.Zero;
            requested = false;

            // I can nill reference exception, and why is this even done here, this should only be
            // done on a new simulator not just on any old teleport.
            //MainClass.client.Grid.RequestMapRegion(this_maps_sim.Name, GridLayerType.Objects);

			Gtk.Application.Invoke(delegate
            {
                 drawavs();
            });

                if (this_maps_sim != null)
                    lastsim = this_maps_sim.ID;
			}
	    }
			
		void drawavs()
		{
				
		    if(MainClass.client==null || this.scalemap==null || this.scalemap.Pixbuf==null)
			    return;

          
		    basemap=this.scalemap;
                  
			Gdk.Pixbuf buf;
			Simulator draw_sim=null;


            draw_sim=MainClass.client.Network.Simulators.Find(delegate(Simulator sim)
            {
                return sim.Name == current_region.Name;
            });



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
                 
                    if (draw_sim != null)

                    draw_sim.AvatarPositions.ForEach(delegate(KeyValuePair<UUID, Vector3> kvp)
                    {

                        //Don't draw us now, we want to be on top
                        if (kvp.Key != MainClass.client.Self.AgentID)
                            if (MainClass.client.Friends.FriendList.ContainsKey(kvp.Key))
                            {
                                if (kvp.Value.Z - MainClass.client.Self.SimPosition.Z > 5)
                                    showme(buf, avatar_friend_above.Pixbuf, kvp.Value);
                                else if (kvp.Value.Z - MainClass.client.Self.SimPosition.Z < -5)
                                    showme(buf, avatar_friend_below.Pixbuf, kvp.Value);
                                else
                                    showme(buf, avatar_friend.Pixbuf, kvp.Value);

                            }
                            else
                            {
                                if (kvp.Value.Z - MainClass.client.Self.SimPosition.Z > 5)
                                    showme(buf, avatar_above.Pixbuf, kvp.Value);
                                else if (kvp.Value.Z - MainClass.client.Self.SimPosition.Z < -5)
                                    showme(buf, avatar_below.Pixbuf, kvp.Value);
                                else
                                    showme(buf, avatar.Pixbuf, kvp.Value);
                            }

                    });

                    if (this_maps_sim != null && MainClass.client.Network.CurrentSim.Handle == this_maps_sim.Handle)
                        if ( this_maps_sim.Handle == current_region.RegionHandle)
                            showme(buf, avatar_me.Pixbuf, MainClass.client.Self.SimPosition);
                        
                if (targetpos.X!=-1 && targetID==current_region.RegionHandle)
					  showme(buf,avatar_target.Pixbuf,targetpos);				
						
             }

            lock (image)
            {
                image.Pixbuf = buf;
                image.QueueDraw();
			}
		}

        void Network_SimChanged(object sender, SimChangedEventArgs e)
        {
			MainClass.win.map_widget=this;
            Gtk.Application.Invoke(delegate
            {
                basemap = new Gtk.Image(MainClass.GetResource("water.png"));
            });
        }
	
		void showme(Gdk.Pixbuf buf,Gdk.Pixbuf src,Vector3 pos)
		{
            try
            {
                if (pos.X < 0 || pos.X > 255 || pos.Y < 0 || pos.Y > 255) //don't plot child AVs of sims
                    return;
                
                int tx, ty;
                tx = (int)pos.X;
                ty = (int)(256.0 - pos.Y);

                tx = (int)(((double)tx / 256.0) * (double)buf.Width);
                ty = (int)(((double)ty / 256.0) * (double)buf.Height);

                tx = tx - 4;
                ty = ty - 4;

                if (tx > buf.Width - 8)
                    tx = buf.Width - 8;

                if (ty > buf.Height - 8)
                    ty = buf.Height - 8;

                if (tx < 8)
                    tx = 8;

                if (ty < 8)
                    ty = 8;

                mergedrawxy(buf, src, tx, ty);
            }
            catch (Exception e)
            {

                Logger.Log("Exception in map! " + e.Message, Helpers.LogLevel.Error);
            }
						
		}

		unsafe void mergedrawxy(Gdk.Pixbuf bufdest,Gdk.Pixbuf src,int x,int y)
		{

            try
            {

                sbyte* pixels = (sbyte*)bufdest.Pixels;
                sbyte* spixels = (sbyte*)src.Pixels;
                sbyte* p;
                sbyte* ps;

                int srcwidth = src.Width;
                int srcheight = src.Height;
                int srcrowsstride = src.Rowstride;
                int schannels = src.NChannels;

                if (x < 0 || x >= bufdest.Width)
                    return;

                if (y < 0 || y >= bufdest.Height)
                    return;

                for (int sx = 0; sx < srcwidth; sx++)
                {
                    for (int sy = 0; sy < srcheight; sy++)
                    {
                        ps = spixels + ((sy) * srcrowsstride) + ((sx) * schannels);
                        p = pixels + ((sy + y) * bufdest.Rowstride) + ((sx + x) * bufdest.NChannels);

                        if (ps[3] != 0) //Alpha merge
                        {
                            p[0] = ps[0];
                            p[1] = ps[1];
                            p[2] = ps[2];
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Log("Exception in map! " + e.Message, Helpers.LogLevel.Error);
            }
		}

		protected virtual void OnEventbox1ButtonPressEvent (object o, Gtk.ButtonPressEventArgs args)
		{
			
			
		    if(this.image==null || this.image.Pixbuf==null)
				return;
			
			// The event box and map are likely to be different sizes.
			
			int marginX=width-image.Pixbuf.Width;
			int marginY=height-image.Pixbuf.Height;
			
			marginX /=2;
			marginY /=2;
			
			Vector3 pos;
			if(args.Event.X>marginX && args.Event.X<(width-marginX) && args.Event.Y>marginY && args.Event.Y<(height-marginY)) 
			{
				Console.WriteLine("In the box");	
				pos.X=(float)(256.0*((args.Event.X-marginX)/this.image.Pixbuf.Width));
				pos.Y=(float)(256.0*((args.Event.Y-marginY)/this.image.Pixbuf.Height));
		}
				else{
			Console.Write("Not in box");	
			return;
            }
			
			pos.Z=0;

			pos.Y=255-pos.Y;

			targetpos=pos;
            targetID = current_region.RegionHandle;

			if(MainClass.win.tp_target_widget!=null)
				MainClass.win.tp_target_widget.settarget(pos,current_region);

			this.drawavs();

            if (onclickMap!=null)
            {
                try
                {
                    onclickMap();
                }
                catch (Exception e)
                {

                }
            }

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
