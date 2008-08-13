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
		
		Gtk.Image mMapImage;		
		Gtk.Image basemap;
		int rowstride;
		int channels;
		int width;
		int height;
		
		public Map()
		{
			this.Build();
			MainClass.client.Network.OnLogin += new libsecondlife.NetworkManager.LoginCallback(onLogin);
			MainClass.client.Network.OnCurrentSimChanged += new libsecondlife.NetworkManager.CurrentSimChangedCallback(onNewSim);
			MainClass.client.Objects.OnNewAvatar += new libsecondlife.ObjectManager.NewAvatarCallback(onNewAvatar);
			MainClass.client.Objects.OnObjectKilled += new libsecondlife.ObjectManager.KillObjectCallback(onKillObject);
			MainClass.client.Objects.OnObjectUpdated += new libsecondlife.ObjectManager.ObjectUpdatedCallback(onUpdate);

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
			Gtk.Application.Invoke(delegate {						

			
			Gdk.Pixbuf buf=(Gdk.Pixbuf)basemap.Pixbuf.Clone();
			
			showme(buf,MainClass.client.Self.SimPosition,255,0,0);				
			
			foreach(KeyValuePair<uint, Avatar> kvp in avs)
			{
					if(kvp.Value.LocalID!=MainClass.client.Self.LocalID)
						showme(buf,kvp.Value.Position,0,255,0);
			}

				image.Pixbuf=buf;
				image.QueueDraw();
			});
			
		}
		

		
		void onNewSim(Simulator last)
	    {
			Gtk.Application.Invoke(delegate {		

			if(MainClass.client.Network.LoginStatusCode==LoginStatus.Success)
				{
					getmap();
					drawavs();
				}				
			});
		}
			
		unsafe void showme(Gdk.Pixbuf buf,LLVector3 pos,int R,int G,int B)
		{
			int tx,ty;
			tx=(int)pos.X;
			ty=(int)(255.0-pos.Y);
			
			tx=tx-3;
			ty=ty-3;
			
			if(tx>251)
				tx=251;
			
			if(ty>251)
			   ty=251;
			
			if(tx<0)
				tx=0;
			
			if(ty<0)
				ty=0;
			
			for(int x=tx;x<tx+5;x++)
			{
				for(int y=ty;y<ty+5;y++)
				{	
					drawxy(buf,x,y,R,G,B);	
				}
				
			}
				
			
		}
		
		unsafe void drawxy(Gdk.Pixbuf buf,int x,int y,int R,int G,int B)
		{
			
			sbyte * pixels=(sbyte *)buf.Pixels;
			sbyte * p;			
			
			if(x<0 || x>width)
				return;
			
			if(y<0 || y>height)
				return;
			
			p=pixels+(y*rowstride)+(x* channels);
		
			p[0]=(sbyte)R;
			p[1]=(sbyte)G;
			p[2]=(sbyte)B;
		
		}
		
		void onLogin(LoginStatus login, string message)
		{
			Gtk.Application.Invoke(delegate {		
				
				if(login==LoginStatus.Success)
				{
					getmap();
					drawavs();
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
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString(), "Error Downloading Web Map Image");
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
