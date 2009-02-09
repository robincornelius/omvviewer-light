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
		UUID av_target=UUID.Zero;
		UUID group_target=UUID.Zero;
	    object[] callbackvalues1;
       		
		public delegate void NameCallBack(string name, object[] values);
        public event NameCallBack onNameCallBack;
		
		public delegate void GroupNameCallBack(string name,object[] values);
        public event GroupNameCallBack onGroupNameCallBack;

        public void go()
        {
            if (av_target!=UUID.Zero)
                try_update_name_lable(av_target);

            if (group_target!= UUID.Zero)
                try_update_group_lable(group_target);

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

            MainClass.client.Groups.OnGroupNames += new OpenMetaverse.GroupManager.GroupNamesCallback(onGroupNames);
            MainClass.client.Avatars.OnAvatarNames += new OpenMetaverse.AvatarManager.AvatarNamesCallback(onAvatarNames);

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
			string name;
			if(MainClass.name_cache.av_names.TryGetValue(key,out name))
			{
				Gtk.Application.Invoke(delegate {
					if(onNameCallBack!=null)
						onNameCallBack(name,this.callbackvalues1);			
					AsynNamesUpdate_deinit();
				});
			}
			else
			{
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
