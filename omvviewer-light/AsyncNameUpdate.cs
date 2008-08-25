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
		List <LLUUID>getting;
	    object[] callbackvalues1;
		
		public delegate void NameCallBack(string name, object[] values);
        public event NameCallBack onNameCallBack;
		
		public delegate void GroupNameCallBack(string name,object[] values);
        public event GroupNameCallBack onGroupNameCallBack;
		
		void AsyncNameUpdate_init()
		{
			MainClass.client.Groups.OnGroupNames += new libsecondlife.GroupManager.GroupNamesCallback(onGroupNames);
			MainClass.client.Avatars.OnAvatarNames += new libsecondlife.AvatarManager.AvatarNamesCallback(onAvatarNames);
		}
		
		public void addparameters(params object[] values)
		{
			callbackvalues1=values;
		}
		
		void AsynNamesUpdate_deinit()
		{
			MainClass.client.Groups.OnGroupNames -= new libsecondlife.GroupManager.GroupNamesCallback(onGroupNames);
			MainClass.client.Avatars.OnAvatarNames -= new libsecondlife.AvatarManager.AvatarNamesCallback(onAvatarNames);
		}
		
		public AsyncNameUpdate(LLUUID key,bool group)
		{
			
			//Console.Write("New AsyncName Update for "+key.ToString()_" group mode = "+group.ToString()+"\n");
			
			if(group==true)
			{
				group_target=key;
				av_target=null;
			}			
			else
			{
				av_target=key;
				group_target=null;
			}
			
			AsyncNameUpdate_init();

			if(!group)
				try_update_name_lable(key);
			
			if(group)
				try_update_group_lable(key);
		}
		
		void onAvatarNames(Dictionary <LLUUID,string>names)
		{
			if(names.ContainsKey(av_target))
			   try_update_name_lable(av_target);
		}
		
		void onGroupNames(Dictionary <LLUUID,string>groups)
	    {
			if(groups.ContainsKey(group_target))
			   try_update_group_lable(group_target);	   
		}
		
		void try_update_name_lable(LLUUID key)
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
		
		void try_update_group_lable(LLUUID key)
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
