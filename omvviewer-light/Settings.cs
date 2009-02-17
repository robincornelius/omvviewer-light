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

// Settings.cs created with MonoDevelop
// User: robin at 19:42 02/12/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Configuration;
using Gdk;

namespace omvviewerlight
{
	
	sealed class MySettings : ApplicationSettingsBase
	{
	
		public delegate void SettingsUpdate();
        public event SettingsUpdate onSettingsUpdate;
		
		public void notify()
		{
			if(onSettingsUpdate!=null)
			{
				try
				{
					onSettingsUpdate();
				}
				catch
				{
					
					
				}
			}
		}
		
		[UserScopedSettingAttribute()]
		public String LastName
		{
			get { try{return (String)this["LastName"];} catch{return "";} }
			set { this["LastName"] = value; }
		}
		[UserScopedSettingAttribute()]
		
		public String FirstName
		{
			get { try{return (String)this["FirstName"];} catch{return "";}  }
			set { this["FirstName"] = value; }
		}
		
		[UserScopedSettingAttribute()]
		public String Password
		{
			get { try{return (String)this["Password"]; } catch{return "";} }
			set { this["Password"] = value; }
		}
		
		[UserScopedSettingAttribute()]
		public bool remember_pass
		{
			get { try{return (bool)this["remember_pass"];} catch{return false;}  }
			set { this["remember_pass"] = value; }
		}
		
		[UserScopedSettingAttribute()]
		public int SelectedGrid
		{
			get { try{return (int)this["SelectedGrid"];} catch{return 0;}  }
			set { this["SelectedGrid"] = value; }
		}
		
		[UserScopedSettingAttribute()]
		public String LoginLocation
		{
			get { try{return (String)this["LoginLocation"]; } catch{return "";} }
			set { this["LoginLocation"] = value; }
		}
		
		[UserScopedSettingAttribute()]
		public String LoginLocationSetting
		{
			get { try{return (String)this["LoginLocationSetting"];} catch{return "";}  }
			set { this["LoginLocationSetting"] = value; }
		}
	
		
		[UserScopedSettingAttribute()]
		public float ThrottleAsset
		{
			get { try{return (float)this["ThrottleAsset"];} catch{return (float)1536000*0.484f / 3f;}  }
			set { this["ThrottleAsset"] = value; }
		}
		
		[UserScopedSettingAttribute()]
		public float ThrottleResend
		{
			get { try{return (float)this["ThrottleResend"];} catch{return (float)1536000* 0.1f;}  }
			set { this["ThrottleResend"] = value; }
		}
		
		[UserScopedSettingAttribute()]
		public float ThrottleLand
		{
			get { try{return (float)this["ThrottleLand"];} catch{return (float)1536000*0.52f / 3f;}  }
			set { this["ThrottleLand"] = value; }
		}
		
		[UserScopedSettingAttribute()]
		public float ThrottleWind
		{
			get { try{return (float)this["ThrottleWind"];} catch{return (float)1536000*0.05f;}  }
			set { this["ThrottleWind"] = value; }
		}
		
			[UserScopedSettingAttribute()]
		public float ThrottleCloud
		{
			get { try{return (float)this["ThrottleCloud"];} catch{return (float)1536000* 0.05f;}  }
			set { this["ThrottleCloud"] = value; }
		}
		
		[UserScopedSettingAttribute()]
		public float ThrottleTask
		{
			get { try{return (float)this["ThrottleTask"];} catch{return (float)1536000* 0.704f / 3f;}  }
			set { this["ThrottleTask"] = value; }
		}

		[UserScopedSettingAttribute()]
		public float ThrottleTexture
		{
			get { try{return (float)this["ThrottleTexture"];} catch{return (float)1536000* 0.704f / 3f;}  }
			set { this["ThrottleTexture"] = value; }
		}
		
		[UserScopedSettingAttribute()]
		public bool timestamps
		{
			get { try{return (bool)this["timestamps"];} catch{return true;}  }
			set { this["timestamps"] = value; }
		}
		
		[UserScopedSettingAttribute()]
		public bool minimise
		{
			get { try{return (bool)this["minimise"];} catch{return true;}  }
			set { this["minimise"] = value; }
		}
		
		[UserScopedSettingAttribute()]
		public bool default_minimim
		{
            get { try { return (bool)this["default_minimim"]; } catch { return true; } }
			set { this["default_minimim"] = value; }
		}
		
		[UserScopedSettingAttribute()]
		public bool default_close
		{
			get { try{return (bool)this["default_close"];} catch{return false;}  }
            set { this["default_close"] = value; }
		}
		
		
		[UserScopedSettingAttribute()]
		public bool tab_location
		{
			get { try{return (bool)this["tab_location"];} catch{return false;}  }
			set { this["tab_location"] = value; }
		}
	[UserScopedSettingAttribute()]
		public bool tab_search
		{
			get { try{return (bool)this["tab_search"];} catch{return false;}  }
			set { this["tab_search"] = value; }
		}
	[UserScopedSettingAttribute()]
		public bool tab_groups
		{
			get { try{return (bool)this["tab_groups"];} catch{return false;}  }
			set { this["tab_groups"] = value; }
		}
	[UserScopedSettingAttribute()]
		public bool tab_inv
		{
			get { try{return (bool)this["tab_inv"];} catch{return false;}  }
			set { this["tab_inv"] = value; }
		}
	[UserScopedSettingAttribute()]
		public bool tab_objects
		{
			get { try{return (bool)this["tab_objects"];} catch{return false;}  }
			set { this["tab_objects"] = value; }
		}
	[UserScopedSettingAttribute()]
		public bool tab_parcel
		{
			get { try{return (bool)this["tab_parcel"];} catch{return false;}  }
			set { this["tab_parcel"] = value; }
		}

		[UserScopedSettingAttribute()]
		public long color_chat
		{
			get { try{return (long)this["color_chat"];} catch{return (((0<<8)+0)<<8)+0;}  }
			set { this["color_chat"] = value; }
		}	

		[UserScopedSettingAttribute()]
		public long color_chat_object
		{
			get { try{return (long)this["color_chat_object"];} catch{return (((0<<8)+255)<<8)+0;}  }
			set { this["color_chat_object"] = value; }
		}	
		
		[UserScopedSettingAttribute()]
		public long color_chat_object_owner
		{
			get { try{return (long)this["color_chat_object_owner"];} catch{return (((0<<8)+0)<<8)+255;}  }
			set { this["color_chat_object_owner"] = value; }
		}	
		
		[UserScopedSettingAttribute()]
		public long color_chat_system
		{
			get { try{return (long)this["color_chat_system"];} catch{return (((255<<8)+0)<<8)+0;}  }
			set { this["color_chat_system"] = value; }
		}	
		
		[UserScopedSettingAttribute()]
		public long color_chat_typing
		{
			get { try{return (long)this["color_chat_typing"];} catch{return (((0<<8)+255)<<8)+0;}  }
			set { this["color_chat_typing"] = value; }
		}	
		
		[UserScopedSettingAttribute()]
		public long color_chat_online
		{
			get { try{return (long)this["color_chat_online"];} catch{return (((0<<8)+255)<<8)+255;}  }
			set { this["color_chat_online"] = value; }
		}

        public Gdk.Color convertfromsetting(long col)
        {			
			return new Gdk.Color((byte)((col & 0xFFFF00000000) >> 32 ), (byte)((col & 0x0000FFFF00000 )>> 16), (byte)(col & 0x00000000FFFF));
        }

        public long converttosetting(Gdk.Color col)
        {
			return (long)((long)(col.Red<<32)+(long)(col.Green<<16)+(long)(col.Blue));
        }
	    
		[UserScopedSettingAttribute()]
		public bool notify_chat
		{
			get { try{return (bool)this["notify_chat"];} catch{return true;}  }
			set { this["notify_chat"] = value; }
		}
		[UserScopedSettingAttribute()]
		public bool notify_object_chat
		{
			get { try{return (bool)this["notify_object_chat"];} catch{return true;}  }
			set { this["notify_object_chat"] = value; }
		}
		[UserScopedSettingAttribute()]
		public bool notify_IM
		{
			get { try{return (bool)this["notify_IM"];} catch{return true;}  }
			set { this["notify_IM"] = value; }
		}
		[UserScopedSettingAttribute()]
		public bool notify_group_IM
		{
			get { try{return (bool)this["notify_group_IM"];} catch{return true;}  }
			set { this["notify_group_IM"] = value; }
		}

	
	}
	
}
		
