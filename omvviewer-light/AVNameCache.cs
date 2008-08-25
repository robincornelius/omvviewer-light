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
