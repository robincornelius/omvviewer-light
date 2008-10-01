/*
omvviewer-light a Text based client to metaverses such as Linden Labs Secondlife(tm)
    Copyright (C) 2008  Robin Cornelius <robin.cornelius@gmail.com>

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

// TeleportTo.cs created with MonoDevelop
// User: robin at 16:23 12/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using OpenMetaverse;
using Gtk;

namespace omvviewerlight
{
	
	public partial class TeleportTo : Gtk.Bin
	{
		
		public TeleportTo()
		{
			this.Build();		
            MainClass.client.Network.OnCurrentSimChanged += new OpenMetaverse.NetworkManager.CurrentSimChangedCallback(onNewSim);
		}

        void onNewSim(Simulator lastsim)
        {
            this.spinbutton_x.Value = MainClass.client.Self.SimPosition.X;
            this.spinbutton_y.Value = MainClass.client.Self.SimPosition.Y;
            this.spinbutton_z.Value = MainClass.client.Self.SimPosition.Z;
        }

	    bool OnTimeout()
		{
			if(MainClass.client.Network.LoginStatusCode==OpenMetaverse.LoginStatus.Success)
			{
				this.label_current.Text="Current Location: "+MainClass.client.Network.CurrentSim.Name+" "+MainClass.client.Self.SimPosition;
			}
			
			return true;
		}
		
		protected virtual void OnButtonTeleportActivated (object sender, System.EventArgs e)
		{
		}

		protected virtual void OnButtonTeleportClicked (object sender, System.EventArgs e)
		{
			Vector3 pos;
			pos=new Vector3();
			pos.X=(float)this.spinbutton_x.Value;
			pos.Y=(float)this.spinbutton_y.Value;
			pos.Z=(float)this.spinbutton_z.Value;
			TeleportProgress tp = new TeleportProgress();
			tp.Show();
			tp.teleport(entry_simname.Text,pos);
		
		}

		protected virtual void OnButtonTphomeClicked (object sender, System.EventArgs e)
		{
			TeleportProgress tp = new TeleportProgress();
			tp.Show();
			tp.teleporthome();
		}
	}
}
