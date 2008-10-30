// LogoutDlg.cs created with MonoDevelop
// User: robin at 19:02 27/10/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Threading;
using Gtk;
using Gdk;
using OpenMetaverse;

namespace omvviewerlight
{
	
	
	public partial class LogoutDlg : Gtk.Dialog
	{
		bool abort=false;
		
		public LogoutDlg()
		{
			this.Build();
			abort=false;
			Thread logoutRunner = new Thread(new ThreadStart(logout));
            logoutRunner.Start();
			Gtk.Timeout.Add(100,tick);
	
		}
		
		void logout()
		{
			MainClass.client.Network.Logout();
            Gtk.Application.Quit();
		}
		
		bool tick()
		{
			if(abort==true)
			{
				this.Destroy();
				return false;
			}

			this.progressbar1.Pulse();
			return true;
			
		}

		protected virtual void OnButtonOkClicked (object sender, System.EventArgs e)
		{
            Gtk.Application.Quit();

		}
	}
}
