// Main.cs created with MonoDevelop
// User: robin at 08:52 11/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
using System;
using System.Collections.Generic;
using Gtk;
using libsecondlife;

namespace omvviewerlight
{
	class MainClass
	{

		public static SecondLife client;
		public static MainWindow win;
		public static Dictionary<LLUUID, string> av_names;		

		
		public static void Main (string[] args)
		{
			// Now boot the libsecondlife layer so it is global to our namespace
			client = new SecondLife();
			av_names=new Dictionary<LLUUID, string>(); 
			Application.Init ();
			win = new MainWindow ();
			win.Show ();
			Application.Run ();
			
			if(client.Network.Connected)
				client.Network.Logout();
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
		
		public static string prettyvector(LLVector3 vector,int dp)
		{
			LLVector3d vec = new LLVector3d(vector.X,vector.Y,vector.Z);
			return prettyvector(vec,dp);
		}
		
		public static string prettyvector(LLVector3d vector,int dp)
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