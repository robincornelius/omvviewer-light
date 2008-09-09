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

// Friendshiprequest.cs created with MonoDevelop
// User: robin at 20:53 16/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using OpenMetaverse;

namespace omvviewerlight
{
	
	
	public partial class Friendshiprequest : Gtk.Window
	{
		UUID agent;
		UUID session;
		public Friendshiprequest(UUID agentID,string agentname,UUID sessionid) : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			this.label_info.Text=agentname+" has offered friendship Press \"accept\" to accept this or press \"reject\" to declin the request";
			session=sessionid;
			agent=agentID;
		}

		protected virtual void OnButtonAcceptClicked (object sender, System.EventArgs e)
		{
			MainClass.client.Friends.AcceptFriendship(agent,session);
			this.Destroy();
		}

		protected virtual void OnButton3Clicked (object sender, System.EventArgs e)
		{
			MainClass.client.Friends.DeclineFriendship(agent,session);
			this.Destroy();
		}
	}
}
