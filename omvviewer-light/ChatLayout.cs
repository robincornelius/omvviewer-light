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

// ChatLayout.cs created with MonoDevelop
// User: robin at 13:29 14/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Gtk;

namespace omvviewerlight
{
	public partial class ChatLayout : Gtk.Bin
	{		
		public ChatLayout()
		{
			this.Build();
		}

        public void passontablable(Gtk.Label tablable)
        {
            this.chatconsole1.tabLabel = tablable;
        }

        public void onSwitchPage(object o, SwitchPageArgs args)
        {
            this.chatconsole1.onSwitchPage(o, args);

        }
	}
}
