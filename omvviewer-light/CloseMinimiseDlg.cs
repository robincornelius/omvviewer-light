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

// CloseMinimiseDlg.cs created with MonoDevelop
// User: robin at 18:28 27/10/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace omvviewerlight
{
	
	
	public partial class CloseMinimiseDlg : Gtk.Dialog
	{
		
		bool save=false;
		public CloseMinimiseDlg()
		{
			this.Build();
		}

		protected virtual void OnCheckbutton1Clicked (object sender, System.EventArgs e)
		{
			   MainClass.appsettings.minimise=this.checkbutton1.Active;
			   save=true;
		}

		protected virtual void OnButton9Clicked (object sender, System.EventArgs e)
		{
			if(save)
			{
				  MainClass.appsettings.default_close=true;
				  MainClass.appsettings.default_minimim=false;
			}
		}

		protected virtual void OnButtonCancelClicked (object sender, System.EventArgs e)
		{
			if(save)
			{
				  MainClass.appsettings.default_close=false;
				  MainClass.appsettings.default_minimim=true;

			}		
		}
	}
}
