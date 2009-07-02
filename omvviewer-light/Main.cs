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

// Main.cs created with MonoDevelop
// User: robin at 08:52 11/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
using System;
using System.Collections.Generic;
using Gtk;
using OpenMetaverse;
using System.Runtime.InteropServices;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Collections;
using System.Text;
using System.Configuration;
using System.Configuration.Assemblies;
using System.Resources;
using System.ComponentModel;
using System.Reflection;
using System.IO;
using Gdk;
using System.Windows.Forms;

namespace omvviewerlight
{
  	class MainClass
	{
        [DllImport("Kernel32")]
        public extern static bool SetDllDirectory(string path);


		public static bool userlogout = false;

    	public static GridClient client=null;
		public static MainWindow win;

		public static AVNameCache name_cache;
		public static MySettings appsettings;

        public delegate void register();
        public static event register onRegister;

        public delegate void deregister();
        public static event deregister onDeregister;

        static void doregister()
        {
            if (onRegister != null)
                onRegister();
        }

        static void doderegister()
        {
            if (onDeregister != null && MainClass.client !=null)
                onDeregister();
        }

        public static void killclient()
        {
            if (client != null)
            {
                Console.WriteLine("** Kill network manager ***");

                client.Network.Shutdown(NetworkManager.DisconnectType.ClientInitiated);
                doderegister();
            }

            client = null;

        }

        public static void getMeANewClient()
        {
            Console.WriteLine("** TRYING TO GET A NEW CLIENT ***");

            if (client != null)
            {
                Console.WriteLine("** Kill network manager ***");

                client.Network.Shutdown(NetworkManager.DisconnectType.ClientInitiated);
                doderegister();
                client = null;
            }


            Console.WriteLine("** NEW CLIENT ***");

            client=new GridClient();

            client.Settings.USE_TEXTURE_CACHE = true;
            // client.Settings.USE_LLSD_LOGIN = true;

            string res_dir = System.AppDomain.CurrentDomain.BaseDirectory + System.IO.Path.DirectorySeparatorChar + "openmetaverse_data";

            OpenMetaverse.Settings.RESOURCE_DIR = res_dir;

            Console.WriteLine("Setting resource dir to " + res_dir);

            string cache = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + System.IO.Path.DirectorySeparatorChar + "omvviewer-light" + System.IO.Path.DirectorySeparatorChar + "omvviewer_cache";

            Console.WriteLine("Setting texture cache to :" + cache);
            client.Settings.TEXTURE_CACHE_DIR = cache;

            doregister();
        }


        static bool monodevelop = false;

		public static string art_location=System.AppDomain.CurrentDomain.BaseDirectory;

        // This is stupid
        public static Gdk.Pixbuf GetResource(string name)
        {
    
            Gdk.Pixbuf buf=null;

                try
                {
                    if (!monodevelop)
                        buf = Pixbuf.LoadFromResource(name);
                    else
                        buf = Pixbuf.LoadFromResource("omvviewerlight.art." + name);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error loading pixbuf");
                }

                return buf;

        }

        private static bool GtkProbe()
        {
            try
            {
                Console.WriteLine("Probing for GTK");
                Gtk.Application.Init();
                Console.WriteLine("GTK found");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("GTK not found");
                return false;
            }


        }


		public static void Main (string[] args)
		{

            bool IsRunningOnMono = (Type.GetType("Mono.Runtime") != null);

            if (!IsRunningOnMono)
            {
                bool gtkfound = false;
                //Gtk probe just try to sort out path issues with the native dlls here.
                //if we still have problems then we need to try to sort our paths for the dot net dlls as well
                // but these SHOULD be in the GAC unless install really fucked up. But path is a sensitive issue
                // and some one may want to kill the path because of Gtk+ issues with other apps.

                gtkfound=GtkProbe();

                if (gtkfound == false)
                {
                    SetDllDirectory("C:\\Program Files\\GtkSharp\\2.12\\bin");
                    gtkfound = GtkProbe();
                }
            }

            try
            {


				Assembly a = Assembly.GetExecutingAssembly();
                appsettings= new MySettings();

                // get a list of resource names from the manifest
                string[] resNames = a.GetManifestResourceNames();

                string check = resNames[0];
                if (check.Contains("omvviewerlight.art."))
                    monodevelop = true;

                name_cache = new AVNameCache();
                Gtk.Application.Init();

                Gtk.Window.DefaultIcon = MainClass.GetResource("omvviewer-light.xpm");
				
                win = new MainWindow();

                win.Show();
				Gtk.Application.Run();
                appsettings.Save();

            }
            catch (Exception e)
            {
                string extra="";
                if (e.Message.Contains("Unable to load DLL 'libglib-"))
                    extra = "\n\nPlease ensure Gtk# (GtkSharp) is installed as per the instructions at \nhttp://www.byteme.org.uk/omvviewer-light.html\nIf this error persists please contact Robin Cornelius <robin.cornelius@gmail.com> for help\n\n"; 
            

                MessageBox.Show("Message :"+e.Message +extra+ "\nSource:" + e.Source+"\n\nBacktrace:\n"+e.StackTrace, "Omvviewer-light - Critical Error");
                Console.Write("The application died in a big heap\n This is the debug i caught :-");
                Console.Write("-----------------------------------------------\n");
                Console.Write(e.Message + "\n");
                Console.Write(e.Source + "\n");
                Console.Write(e.StackTrace + "\n");
                Console.Write(e.TargetSite + "\n");
                Exception ee;
                ee = e.InnerException;

                while (ee != null)
                {
                    Console.Write("-----------------------------------------------\n");
                    Console.Write(ee.Message + "\n");
                    Console.Write(ee.Source + "\n");
                    Console.Write(ee.StackTrace + "\n");
                    Console.Write(ee.TargetSite + "\n");
                    ee = ee.InnerException;
                }
            }
			
		}

		public static string cleandistance(string dist,int dp)
		{			
			int pos=dist.IndexOf(".");
			dp++;
			
			if(pos==-1)
				return dist;
			
			if((pos+dp)>dist.Length)
				return dist;
			
			return dist.Substring(0,pos+dp);		
		}		
		
		public static string prettyvector(Vector3 vector,int dp)
		{
			Vector3d vec = new Vector3d(vector.X,vector.Y,vector.Z);
			return prettyvector(vec,dp);
		}
		
		public static string prettyvector(Vector3d vector,int dp)
		{
			string ret;
			
			string x,y,z;
			x=cleandistance(vector.X.ToString(),dp);
			y=cleandistance(vector.Y.ToString(),dp);
			z=cleandistance(vector.Z.ToString(),dp);
			
			ret="<"+x+","+y+","+z+">";
			
			return ret;
		}

	}
}
