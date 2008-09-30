// ParcelMgr.cs created with MonoDevelop
// User: robin at 21:16 30/09/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;
using OpenMetaverse;

namespace omvviewerlight
{
	public class ParcelMgr
	{
		public ParcelMgr()
		{
			MainClass.client.Parcels.OnParcelInfo += new OpenMetaverse.ParcelManager.ParcelInfoCallback(onParcelInfo);
		}
		
		void onParcelInfo(ParcelInfo info)
		{
			
			
		}
	}
}
