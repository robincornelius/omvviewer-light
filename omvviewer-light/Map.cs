// Map.cs created with MonoDevelop
// User: robin at 23:10 12/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Net;
using System.Collections.Generic;
using libsecondlife;
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

		public Map()
		{
			this.Build();
			MainClass.client.Network.OnLogin += new libsecondlife.NetworkManager.LoginCallback(onLogin);
			MainClass.client.Network.OnCurrentSimChanged += new libsecondlife.NetworkManager.CurrentSimChangedCallback(onNewSim);
			MainClass.client.Objects.OnNewAvatar += new libsecondlife.ObjectManager.NewAvatarCallback(onNewAvatar);
			MainClass.client.Objects.OnObjectKilled += new libsecondlife.ObjectManager.KillObjectCallback(onKillObject);
			MainClass.client.Objects.OnObjectUpdated += new libsecondlife.ObjectManager.ObjectUpdatedCallback(onUpdate);
			MainClass.client.Self.OnTeleport += new libsecondlife.AgentManager.TeleportCallback(onTeleport);

		}

		void onTeleport(string Message, libsecondlife.AgentManager.TeleportStatus status,libsecondlife.AgentManager.TeleportFlags flags)
	    {
			if(status==libsecondlife.AgentManager.TeleportStatus.Finished)
			{
				Gtk.Application.Invoke(delegate {										
					avs.Clear();
				});
			}
	    }
		
		
		void onUpdate(Simulator simulator, ObjectUpdate update,ulong regionHandle, ushort timeDilation)
		{
			if(avs.ContainsKey(update.LocalID))
			{
				drawavs();
			}
		}
		
		void onNewAvatar(Simulator simulator, Avatar avatar, ulong regionHandle, ushort timeDilation)
		{
			if(!avs.ContainsKey(avatar.LocalID))
			{
				avs.Add(avatar.LocalID,avatar);	
				drawavs();			
			}
		}

		void onKillObject(Simulator simulator, uint objectID)
		{
			if(avs.ContainsKey(objectID))
			{
				avs.Remove(objectID);
				drawavs();
			}
			
		}
		
		void drawavs()
		{
			if(basemap==null)
					return;
			
			Gtk.Application.Invoke(delegate {						
				
			Gdk.Pixbuf buf=(Gdk.Pixbuf)basemap.Pixbuf.Clone();
				
		//	Console.Write("Drawing me\n");
			showme(buf,avatar_me.Pixbuf,MainClass.client.Self.SimPosition);				
				
			int myz=(int)MainClass.client.Self.SimPosition.Z;
				
				
			foreach(KeyValuePair<uint, Avatar> kvp in avs)
			{
					if(kvp.Value.LocalID!=MainClass.client.Self.LocalID)
					{

						//showme(buf,avatar_me.Pixbuf,kvp.Value.Position);				
						
						if(kvp.Value.Position.Z-myz>5)
						{	
							//Console.Write("Drawing above\n");

							showme(buf,avatar_above.Pixbuf,kvp.Value.Position);
						
						}						
						else if(kvp.Value.Position.Z-myz<-5)
						{	
						//Console.Write("Drawing below\n");
		
							showme(buf,avatar_below.Pixbuf,kvp.Value.Position);
				
						}						
						else
						{
						//				Console.Write("Drawing lelve\n");

							showme(buf,avatar.Pixbuf,kvp.Value.Position);
						}								
					}
			}

	//			Console.Write("Update donw refresh\n");
				image.Pixbuf=buf;
				image.QueueDraw();
//				Console.Write("Finished\n");
			});
			
		}
		

		
		void onNewSim(Simulator last)
	    {
			Gtk.Application.Invoke(delegate {		

			if(MainClass.client.Network.LoginStatusCode==LoginStatus.Success)
				{
					getmap();
					drawavs();
					this.label1.Text=MainClass.client.Network.CurrentSim.Name;
				}
				else
				{
					this.label1.Text="No location";
					
				}
			});
		}
			
		unsafe void showme(Gdk.Pixbuf buf,Gdk.Pixbuf src,LLVector3 pos)
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
		
		void onLogin(LoginStatus login, string message)
		{
			Gtk.Application.Invoke(delegate {		
				
				if(login==LoginStatus.Success)
				{
					getmap();
					drawavs();
					this.label1.Text=MainClass.client.Network.CurrentSim.Name;

				}
			});
		}
				
		void getmap()
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
				basemap=new Gtk.Image(response.GetResponseStream());

				image.Pixbuf=(Gdk.Pixbuf)basemap.Pixbuf.Clone();

				rowstride=basemap.Pixbuf.Rowstride;
				channels=basemap.Pixbuf.NChannels;
				width=basemap.Pixbuf.Width;
				height=basemap.Pixbuf.Height;
				
				
				//return System.Drawing.Image.FromStream(response.GetResponseStream());
				
            }
            catch (Exception e)
            {
				Gtk.MessageDialog msg = new Gtk.MessageDialog(MainClass.win,DialogFlags.Modal,MessageType.Error,ButtonsType.Ok,"Error Downloading Web Map Image");
				msg.Show();
				return;
            }

			
		}

		protected virtual void OnImageButtonPressEvent (object o, Gtk.ButtonPressEventArgs args)
		{

			Console.Write("Click "+args.Event.X.ToString()+","+args.Event.Y.ToString()+"\n");
			// Ah fuck it it does not get the events
			
		
		}

		protected virtual void OnEventbox1ButtonPressEvent (object o, Gtk.ButtonPressEventArgs args)
		{
			//this stupid thing gets events but autosizes even though i said no
		}
	}
}
