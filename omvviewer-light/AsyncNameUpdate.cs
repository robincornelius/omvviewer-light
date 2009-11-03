/*
omvviewerlight a Text based client to metaverses such as Linden Labs Secondlife(tm)
    Copyright (C) 2008,2009  Robin Cornelius <robin.cornelius@gmail.com>

    Redistribution and use in source and binary forms, with or without
    modification, are permitted provided that the following conditions
    are met:
    1. Redistributions of source code must retain the above copyright
        notice, this list of conditions and the following disclaimer.
    2. Redistributions in binary form must reproduce the above copyright
        notice, this list of conditions and the following disclaimer in the
        documentation and/or other materials provided with the distribution.
    3. The name of the author may not be used to endorse or promote products
        derived from this software without specific prior written permission.

    THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
    IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
    OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
    IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
    INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
    NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
    DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
    THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
    (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
    THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
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
                try_update_name_label(av_target);

            if (group_target!= UUID.Zero)
                try_update_group_lable(group_target);

        }

		public void addparameters(params object[] values)
		{
			callbackvalues1=values;
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

            MainClass.onRegister += new MainClass.register(MainClass_onRegister);
            MainClass.onDeregister += new MainClass.deregister(MainClass_onDeregister);
            if(MainClass.client != null ) { MainClass_onRegister(); }

		}

        void MainClass_onDeregister()
        {
            if (MainClass.client != null)
            {
            MainClass.client.Groups.GroupNamesReply -= new EventHandler<GroupNamesEventArgs>(Groups_GroupNamesReply);
            MainClass.client.Avatars.UUIDNameReply -= new EventHandler<UUIDNameReplyEventArgs>(Avatars_UUIDNameReply);
            }
        }

        void MainClass_onRegister()
        {
            MainClass.client.Groups.GroupNamesReply += new EventHandler<GroupNamesEventArgs>(Groups_GroupNamesReply);
            MainClass.client.Avatars.UUIDNameReply += new EventHandler<UUIDNameReplyEventArgs>(Avatars_UUIDNameReply);
        }
   		
	    void Avatars_UUIDNameReply(object sender, UUIDNameReplyEventArgs e)
      	{
			if(e.Names.ContainsKey(av_target))
			   try_update_name_label(av_target);
		}

        void Groups_GroupNamesReply(object sender, GroupNamesEventArgs e)
        {
			if(e.GroupNames.ContainsKey(group_target))
			   try_update_group_lable(group_target);	   
		}
		
		void try_update_name_label(UUID key)
		{
			string name;
			if(MainClass.name_cache.av_names.TryGetValue(key,out name))
			{
				Gtk.Application.Invoke(delegate {
					if(onNameCallBack!=null)
						onNameCallBack(name,this.callbackvalues1);
                    MainClass_onDeregister();
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
                    MainClass_onDeregister();
				});	
			}
			else
			{	
				MainClass.client.Groups.RequestGroupName(key);
			}
		}
	}
}
