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

// Location.cs created with MonoDevelop
// User: robin at 13:11 14/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Gtk;
using Gdk;

namespace omvviewerlight
{
	
	[System.ComponentModel.ToolboxItem(true)]	
	public partial class Location : Gtk.Bin
	{
        bool requested = false;
		
		public Location()
		{
			this.Build();

            MainClass.onRegister += new MainClass.register(MainClass_onRegister);
            MainClass.onDeregister += new MainClass.deregister(MainClass_onDeregister);
            if(MainClass.client != null ) { MainClass_onRegister(); }

            this.SizeAllocated += new Gtk.SizeAllocatedHandler(onResize);

            if (MainClass.client != null)
            {
                if (MainClass.client.Network.LoginStatusCode == OpenMetaverse.LoginStatus.Success)
                {
                    requested = true;
                }
            }

        }

        void MainClass_onDeregister()
        {
            if(MainClass.client!=null)
                MainClass.client.Network.EventQueueRunning -= new EventHandler<OpenMetaverse.EventQueueRunningEventArgs>(Network_EventQueueRunning);
        }

        void MainClass_onRegister()
        {
            MainClass.client.Network.EventQueueRunning += new EventHandler<OpenMetaverse.EventQueueRunningEventArgs>(Network_EventQueueRunning);
        }


        void onResize(object o, SizeAllocatedArgs args)
        {

            MainClass.win.setmapwidget(map1);

            if (requested == true)
            {
                if (args.Allocation.Width > 100)
                {
                    requested = false;
                    map1.SetAsWater();
                  //   map1.set_optimal_size(args.Allocation.Width);
                     map1.SetGridRegion(MainClass.client.Network.CurrentSim.RegionID, MainClass.client.Network.CurrentSim.Handle);
                }
            }
        }

        void Network_EventQueueRunning(object sender, OpenMetaverse.EventQueueRunningEventArgs e)
        {
            this.map1.SetGridRegion(MainClass.client.Network.CurrentSim.RegionID, MainClass.client.Network.CurrentSim.Handle);
        }

        new public void Dispose()
        {
            Console.WriteLine("Disposing of the Location control");

            MainClass.onRegister -= new MainClass.register(MainClass_onRegister);
            MainClass.onDeregister -= new MainClass.deregister(MainClass_onDeregister);
            MainClass_onDeregister();

            Gtk.Notebook p;
            p = (Gtk.Notebook)this.Parent;
            p.RemovePage(p.PageNum(this));

            this.map1.Dispose();
            this.radar1.Dispose();
            this.teleportto1.Dispose();
        }
	}
}
