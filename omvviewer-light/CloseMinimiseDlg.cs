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
		
		bool save=false;
		public CloseMinimiseDlg()
		{
			this.Build();
		}

		protected virtual void OnCheckbutton1Clicked (object sender, System.EventArgs e)
		{
			   MainClass.appsettings.minimise=this.checkbutton1.Active;
			   save=true;
		}

		protected virtual void OnButton9Clicked (object sender, System.EventArgs e)
		{
			if(save)
			{
				  MainClass.appsettings.default_close=true;
				  MainClass.appsettings.default_minimim=false;
			}
		}

		protected virtual void OnButtonCancelClicked (object sender, System.EventArgs e)
		{
			if(save)
			{
				  MainClass.appsettings.default_close=false;
				  MainClass.appsettings.default_minimim=true;

			}		
		}
	}
}
