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
			   MainClass.WriteSetting("hideminimse",this.checkbutton1.Active.ToString());
			   save=true;
		}

		protected virtual void OnButton9Clicked (object sender, System.EventArgs e)
		{
			if(save)
			{
				  MainClass.WriteSetting("defaultclose",save.ToString());
				  MainClass.WriteSetting("defaultminimise",false.ToString());

			}
		}

		protected virtual void OnButtonCancelClicked (object sender, System.EventArgs e)
		{
			if(save)
			{
				  MainClass.WriteSetting("defaultminimise",save.ToString());
				  MainClass.WriteSetting("defaultclose",false.ToString());

			}		
		}
	}
}
