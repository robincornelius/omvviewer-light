// CloseMinimiseDlg.cs created with MonoDevelop
// User: robin at 18:28 27/10/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace omvviewerlight
{
	
	
	public partial class CloseMinimiseDlg : Gtk.Dialog
	{
		
		public CloseMinimiseDlg()
		{
			this.Build();
		}

		protected virtual void OnCheckbutton1Clicked (object sender, System.EventArgs e)
		{
			if(this.checkbutton1.Active)
				MainClass.WriteSetting("Remember_close","true");
			else
				MainClass.WriteSetting("Remember_close","false");
		}
	}
}
