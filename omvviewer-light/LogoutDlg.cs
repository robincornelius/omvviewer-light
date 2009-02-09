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
			GLib.Timeout.Add(100,tick);
	
		}
		
		void logout()
		{
			MainClass.client.Network.Logout();
            GLib.Timeout.Add(5000, killme);
            
		}

        bool killme()
        {
            abort = true;
            Gtk.Application.Quit();
            return false;
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
