// Main.cs created with MonoDevelop
// User: robin at 08:52 11/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
using System;
using Gtk;
using libsecondlife;

namespace omvviewerlight
{
	class MainClass
	{

		public static SecondLife client;
		public static MainWindow win;
		
		public static void Main (string[] args)
		{
			// Now boot the libsecondlife layer so it is global to our namespace
			client = new SecondLife();
			
			Application.Init ();
			win = new MainWindow ();
			win.Show ();
			Application.Run ();
			
			if(client.Network.Connected)
				client.Network.Logout();
		}
	}
}