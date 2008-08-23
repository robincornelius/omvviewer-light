// AsyncNameUpdate.cs created with MonoDevelop
// User: robin at 20:16 23/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;
using libsecondlife;
using Gtk;
using Gdk;


namespace omvviewerlight
{
	
	public class AsyncNameUpdate
	{
		LLUUID av_target;
		LLUUID group_target;
		
		public delegate void NameCallBack(string name);
        public event NameCallBack onNameCallBack;
		
		public delegate void GroupNameCallBack(string name);
        public event GroupNameCallBack onGroupNameCallBack;
		
		void AsyncNameUpdate_init()
		{
			MainClass.client.Groups.OnGroupNames += new libsecondlife.GroupManager.GroupNamesCallback(onGroupNames);
			MainClass.client.Avatars.OnAvatarNames += new libsecondlife.AvatarManager.AvatarNamesCallback(onAvatarNames);
		}
		
		void AsynNamesUpdate_deinit()
		{
			MainClass.client.Groups.OnGroupNames -= new libsecondlife.GroupManager.GroupNamesCallback(onGroupNames);
			MainClass.client.Avatars.OnAvatarNames -= new libsecondlife.AvatarManager.AvatarNamesCallback(onAvatarNames);
		}

		
		public AsyncNameUpdate(LLUUID av,LLUUID group)
		{
			av_target=av;
			group_target=group;
			AsyncNameUpdate_init();
			if(av!=null)
				try_update_name_lable(av);
			
			if(group_target!=null)
				try_update_group_lable(group);
		}
		
		void onAvatarNames(Dictionary <LLUUID,string>names)
		{
			foreach(KeyValuePair <LLUUID,string> kvp in names)
			{
				if(!MainClass.av_names.ContainsKey(kvp.Key))
				{
					MainClass.av_names.Add(kvp.Key,kvp.Value);
				}

				if(kvp.Key==av_target)
					try_update_name_lable(av_target);
			}	
		}
		
		void onGroupNames(Dictionary <LLUUID,string>groups)
	    {
			
			foreach(KeyValuePair <LLUUID,string> kvp in groups)
			{
				if(kvp.Key==group_target)
					try_update_group_lable(group_target);
			}	
		}
		
		void try_update_name_lable(LLUUID key)
		{
			string name;
			if(MainClass.av_names.TryGetValue(key,out name))
			{
				Gtk.Application.Invoke(delegate {			
					if(onNameCallBack!=null)
						onNameCallBack(name);			
					AsynNamesUpdate_deinit();
				});
			}
			else
			{		
					MainClass.client.Avatars.RequestAvatarName(key);
			}
		}
		
		void try_update_group_lable(LLUUID key)
		{
			string name;
				
			if(MainClass.client.Groups.GroupName2KeyCache.TryGetValue(key,out name))
			{
				Gtk.Application.Invoke(delegate {			
					if(onGroupNameCallBack!=null)
						onGroupNameCallBack(name);			
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
