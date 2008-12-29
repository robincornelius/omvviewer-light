/*
omvviewerlight a Text based client to metaverses such as Linden Labs Secondlife(tm)
    Copyright (C) 2008  Robin Cornelius <robin.cornelius@gmail.com>

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

// AVNameCache.cs created with MonoDevelop
// User: robin at 10:06 24/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;
using OpenMetaverse;

namespace omvviewerlight
{
	public class AVNameCache
	{
		List <UUID>getting;
		public Dictionary<UUID, string> av_names;
		
		public AVNameCache()
		{
			av_names = new Dictionary<UUID, string>();
			getting = new List<UUID>();
			MainClass.client.Groups.OnGroupNames += new OpenMetaverse.GroupManager.GroupNamesCallback(onGroupNames);
			MainClass.client.Avatars.OnAvatarNames += new OpenMetaverse.AvatarManager.AvatarNamesCallback(onAvatarNames);
		}
		
		public void reqname(UUID name)
		{

            lock (getting)
            {
                if (!getting.Contains(name) && !MainClass.name_cache.av_names.ContainsKey(name))
                {
                    getting.Add(name);
                    MainClass.client.Avatars.RequestAvatarName(name);
                }
            }
		}
		
		public void reqnames(List <UUID> names)
		{
            List<UUID> request = new List<UUID>();

            lock (MainClass.name_cache.av_names)
            {
                lock (getting)
                {
                    lock (MainClass.name_cache.av_names)
                    {
                        foreach (UUID name in names)
                        {
                            if (!getting.Contains(name) && !MainClass.name_cache.av_names.ContainsKey(name))
                            {
                                getting.Add(name);
                                request.Add(name);
                            }
                        }
                    }
                }
            }
                
            // Possible libomv bug, dont request too many names in a single shot
            for (int x = 0; x < request.Count; x = x + 100)
            {
                List<UUID> request_temp = request.GetRange(x, (x + 100) < (request.Count - 1) ? 100 : 100-((x + 100)-(request.Count - 1)));
                MainClass.client.Avatars.RequestAvatarNames(request_temp); 
            }  
		}

		void onAvatarNames(Dictionary <UUID,string>names)
		{
			lock(av_names)
			{
				foreach(KeyValuePair <UUID,string> kvp in names)
				{
	                lock(getting)
	                {
					    if(getting.Contains(kvp.Key))
				            getting.Remove(kvp.Key);
					}
					   
					if(!av_names.ContainsKey(kvp.Key))
					{
						av_names.Add(kvp.Key,kvp.Value);
					}
				}	
			}
		
		}
		
		void onGroupNames(Dictionary <UUID,string>groups)
	    {
			
		}
	}	
}
