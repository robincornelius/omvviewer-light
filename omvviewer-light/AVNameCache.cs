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

// AVNameCache.cs created with MonoDevelop
// User: robin at 10:06 24/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;
using libsecondlife;

namespace omvviewerlight
{
	public class AVNameCache
	{
		List <LLUUID>getting;
		public Dictionary<LLUUID, string> av_names;
		
		public AVNameCache()
		{
			av_names = new Dictionary<LLUUID, string>();
			getting = new List<LLUUID>();
			MainClass.client.Groups.OnGroupNames += new libsecondlife.GroupManager.GroupNamesCallback(onGroupNames);
			MainClass.client.Avatars.OnAvatarNames += new libsecondlife.AvatarManager.AvatarNamesCallback(onAvatarNames);
		}
		
		public void reqname(LLUUID name)
		{
			if(!getting.Contains(name)&& !MainClass.name_cache.av_names.ContainsKey(name))
			{
				getting.Add(name);
				MainClass.client.Avatars.RequestAvatarName(name);
			}			
		}
		
		public void reqnames(List <LLUUID> names)
		{
			
			List <LLUUID> request=new List <LLUUID>();
			
			foreach(LLUUID name in names)
			{
				if(!this.getting.Contains(name))
				{
					getting.Add(name);
				}
				if(!av_names.ContainsKey(name))
				{
					request.Add(name);
				}
				
			}
				
			if(request.Count>0)
				MainClass.client.Avatars.RequestAvatarNames(request);
			
			
		}

		void onAvatarNames(Dictionary <LLUUID,string>names)
		{
			foreach(KeyValuePair <LLUUID,string> kvp in names)
			{
				if(getting.Contains(kvp.Key))
			        getting.Remove(kvp.Key);
								   
				if(!av_names.ContainsKey(kvp.Key))
				{
					av_names.Add(kvp.Key,kvp.Value);
				}
			}	
		
		}
		
		void onGroupNames(Dictionary <LLUUID,string>groups)
	    {
			
		}
	}	
}
