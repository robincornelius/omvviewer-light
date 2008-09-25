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
                    //Console.Write("NO requesting\n");
                    getting.Add(name);
                    MainClass.client.Avatars.RequestAvatarName(name);
                }
                else
                {
                    // Console.Write("Already have or already getting\n");
                }
            }
		}
		
		public void reqnames(List <UUID> names)
		{

            lock (getting)
            {
                List<UUID> request = new List<UUID>();
                foreach (UUID name in names)
                {
                    //Console.Write("LIST Do we have " + name.ToString() + "\n");
                    if (!getting.Contains(name) && !MainClass.name_cache.av_names.ContainsKey(name))
                    {
                        // Console.Write("LIST NO requesting\n");
                        getting.Add(name);
                        request.Add(name);
                        //MainClass.client.Avatars.RequestAvatarName(name);
                    }
                    else
                    {
                        // Console.Write("LIST Already have or already getting\n");
                    }

                }
            
                // Possible libomv bug, dont request too many names in a single shot
                for (int x = 0; x < request.Count; x = x + 100)
                {
                    List<UUID> request_temp = request.GetRange(x, (x + 100) < (request.Count - 1) ? 100 : 100-((x + 100)-(request.Count - 1)));
                    MainClass.client.Avatars.RequestAvatarNames(request_temp); 
              
                }


                
            }

            
			
			
		}

		void onAvatarNames(Dictionary <UUID,string>names)
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
                    //Console.Write("GOT "+kvp.Key.ToString()+" = "+kvp.Value+"\n");
					av_names.Add(kvp.Key,kvp.Value);
				}
			}	
		
		}
		
		void onGroupNames(Dictionary <UUID,string>groups)
	    {
			
		}
	}	
}
