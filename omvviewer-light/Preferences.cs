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
			
			this.hscale_asset.Value=  MainClass.client.Throttle.Asset;
			this.hscale_cloud.Value= MainClass.client.Throttle.Cloud;
			this.hscale_land.Value=  MainClass.client.Throttle.Land;
			this.hscale_resend.Value= MainClass.client.Throttle.Resend;
			this.hscale_task.Value=   MainClass.client.Throttle.Task;
			this.hscale_texture.Value=  MainClass.client.Throttle.Texture;
			this.hscale_wind.Value=MainClass.client.Throttle.Wind;
			
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

				protected virtual void OnButtonApplythrottleClicked (object sender, System.EventArgs e)
				{

			
			       MainClass.WriteSetting("throttle_asset",this.hscale_asset.Value.ToString());
			       MainClass.WriteSetting("throttle_cloud",this.hscale_cloud.Value.ToString());
			       MainClass.WriteSetting("throttle_land",this.hscale_land.Value.ToString());
			       MainClass.WriteSetting("throttle_resend",this.hscale_resend.Value.ToString());
			       MainClass.WriteSetting("throttle_task",this.hscale_task.Value.ToString());
			       MainClass.WriteSetting("throttle_texture",this.hscale_texture.Value.ToString());
			       MainClass.WriteSetting("throttle_wind",this.hscale_wind.Value.ToString());
				
				}
	}
}
