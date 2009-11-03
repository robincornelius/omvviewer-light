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

            MainClass.onRegister += new MainClass.register(MainClass_onRegister);
            MainClass.onDeregister += new MainClass.deregister(MainClass_onDeregister);
            if(MainClass.client != null ) { MainClass_onRegister(); }
        }

        void MainClass_onDeregister()
        {
            MainClass.client.Avatars.UUIDNameReply -= new EventHandler<UUIDNameReplyEventArgs>(Avatars_UUIDNameReply);
        }

        void MainClass_onRegister()
        {
            MainClass.client.Avatars.UUIDNameReply += new EventHandler<UUIDNameReplyEventArgs>(Avatars_UUIDNameReply);
        }
 
        public void Dispose()
        {
            MainClass.onRegister -= new MainClass.register(MainClass_onRegister);
            MainClass.onDeregister -= new MainClass.deregister(MainClass_onDeregister);
            MainClass_onDeregister();
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

        void Avatars_UUIDNameReply(object sender, UUIDNameReplyEventArgs e)
        {
			lock(av_names)
			{
				foreach(KeyValuePair <UUID,string> kvp in e.Names)
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
		
	}	
}
