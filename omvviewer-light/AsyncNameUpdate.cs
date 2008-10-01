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

// AsyncNameUpdate.cs created with MonoDevelop
// User: robin at 20:16 23/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;
using OpenMetaverse;
using Gtk;
using Gdk;


namespace omvviewerlight
{
	public class AsyncNameUpdate
	{
		UUID av_target;
		UUID group_target;
	    object[] callbackvalues1;
       		
		public delegate void NameCallBack(string name, object[] values);
        public event NameCallBack onNameCallBack;
		
		public delegate void GroupNameCallBack(string name,object[] values);
        public event GroupNameCallBack onGroupNameCallBack;
		
		void AsyncNameUpdate_init()
		{
			MainClass.client.Groups.OnGroupNames += new OpenMetaverse.GroupManager.GroupNamesCallback(onGroupNames);
			MainClass.client.Avatars.OnAvatarNames += new OpenMetaverse.AvatarManager.AvatarNamesCallback(onAvatarNames);
           // GLib.Timeout.Add(1000, onTimeout);
        }

		public void addparameters(params object[] values)
		{
			callbackvalues1=values;
		}
		
		void AsynNamesUpdate_deinit()
		{
			MainClass.client.Groups.OnGroupNames -= new OpenMetaverse.GroupManager.GroupNamesCallback(onGroupNames);
			MainClass.client.Avatars.OnAvatarNames -= new OpenMetaverse.AvatarManager.AvatarNamesCallback(onAvatarNames);
		}
		
		public AsyncNameUpdate(UUID key,bool group)
		{

            if (key == UUID.Zero)
                return;
			
			if(group==true)
			{
				group_target=key;
				av_target=UUID.Zero;
			}			
			else
			{
				av_target=key;
				group_target=UUID.Zero;
			}
						
			AsyncNameUpdate_init();

			if(!group)
				try_update_name_lable(key);
			
			if(group)
				try_update_group_lable(key);
		}
		
		void onAvatarNames(Dictionary <UUID,string>names)
		{
			if(names.ContainsKey(av_target))
			   try_update_name_lable(av_target);
		}
		
		void onGroupNames(Dictionary <UUID,string>groups)
	    {
			if(groups.ContainsKey(group_target))
			   try_update_group_lable(group_target);	   
		}
		
		void try_update_name_lable(UUID key)
		{
           // Console.Write("Try update KEY " + key.ToString() + "\n");

			string name;
			if(MainClass.name_cache.av_names.TryGetValue(key,out name))
			{
				Gtk.Application.Invoke(delegate {
                   // Console.Write("Success! trying to throw callback\n");
					if(onNameCallBack!=null)
						onNameCallBack(name,this.callbackvalues1);			
					AsynNamesUpdate_deinit();
				});
			}
			else
			{
                //Console.Write("Failed! kicking requester\n");
				MainClass.name_cache.reqname(key);			
			}
		}
		
		void try_update_group_lable(UUID key)
		{
			string name;
				
			if(MainClass.client.Groups.GroupName2KeyCache.TryGetValue(key,out name))
			{
				Gtk.Application.Invoke(delegate {			
					if(onGroupNameCallBack!=null)
						onGroupNameCallBack(name,this.callbackvalues1);			
					AsynNamesUpdate_deinit();
				});
				
			}
			else
			{	
				
				MainClass.client.Groups.RequestGroupName(key);
			}
		}
	}
}
