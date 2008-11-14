// Preferences.cs created with MonoDevelop
// User: robin at 21:00 14/11/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
namespace omvviewerlight
{
	
	public partial class Preferences : Gtk.Window
	{
		
		public Preferences() : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			bool result;
			bool.TryParse(MainClass.ReadSetting("timestamps"),out result);
			this.checkbutton_showtimestamps.Active=result;
			bool.TryParse(MainClass.ReadSetting("hideminimse"),out result);
			this.checkbutton_hideminimise.Active=result;
			bool.TryParse(MainClass.ReadSetting("defaultminimise"),out result);
			this.radiobutton1.Active=result;
			bool.TryParse(MainClass.ReadSetting("defaultclose"),out result);
			this.radiobutton2.Active=result; 		 
		}

				protected virtual void OnCheckbuttonShowtimestampsClicked (object sender, System.EventArgs e)
				{
                      MainClass.WriteSetting("timestamps",this.checkbutton_showtimestamps.Active.ToString());
			          MainClass.kick_preferences();
		        }

				protected virtual void OnCheckbuttonHideminimiseClicked (object sender, System.EventArgs e)
				{
                     MainClass.WriteSetting("hideminimse",this.checkbutton_hideminimise.Active.ToString());
			         MainClass.kick_preferences();
		        }

				protected virtual void OnRadiobutton1Activated (object sender, System.EventArgs e)
				{
			          MainClass.WriteSetting("defaultminimise",this.radiobutton1.Active.ToString());
			          MainClass.WriteSetting("defaultclose",this.radiobutton2.Active.ToString());
			          MainClass.kick_preferences();
		        }

				protected virtual void OnRadiobutton2Activated (object sender, System.EventArgs e)
				{
			          MainClass.WriteSetting("defaultminimise",this.radiobutton1.Active.ToString());
			          MainClass.WriteSetting("defaultclose",this.radiobutton2.Active.ToString());
			          MainClass.kick_preferences();
		}
	}
}
