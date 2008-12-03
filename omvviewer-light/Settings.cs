// Settings.cs created with MonoDevelop
// User: robin at 19:42 02/12/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Configuration;

namespace omvviewerlight
{
	
	sealed class MySettings : ApplicationSettingsBase
	{
		
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

		
	}
	
}
		
