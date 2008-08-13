// Map.cs created with MonoDevelop
// User: robin at 23:10 12/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Net;
using libsecondlife;
using omvviewerlight;
using System.Drawing;

namespace omvviewerlight
{
	public partial class Map : Gtk.Bin
	{
		private const String MAP_IMG_URL = "http://secondlife.com/apps/mapapi/grid/map_image/";
		private const int GRID_Y_OFFSET = 1279;
		
		public Map()
		{
			this.Build();
	//		MainClass.client.Self.OnTeleport += new libsecondlife.AgentManager.TeleportCallback(onTeleport);
			MainClass.client.Network.OnLogin += new libsecondlife.NetworkManager.LoginCallback(onLogin);
			
			//this.image.SetFromPixmap
		}
		
		void onLogin(LoginStatus login, string message)
		{
			Gtk.Application.Invoke(delegate {		
			//savemap();
			
			});
		}
		
		public void savemap()
		{
			System.Drawing.Image mMapImage=getmap();
			mMapImage.Save("map.jpg");
		}
		
		System.Drawing.Image getmap()
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
				return System.Drawing.Image.FromStream(response.GetResponseStream());
				
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString(), "Error Downloading Web Map Image");
                return null;
            }

			
		}
	}
}
