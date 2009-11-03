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

// Preferences.cs created with MonoDevelop
// User: robin at 21:00 14/11/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Gtk;
using Gdk;

namespace omvviewerlight
{
	public partial class Preferences : Gtk.Window
	{
		
		public Preferences() : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			
            if (MainClass.appsettings.default_minimim)
                this.radiobutton1.Active=true;
            else
                this.radiobutton2.Active=true;

            this.checkbutton_hideminimise.Active = MainClass.appsettings.minimise;
            this.checkbutton_showtimestamps.Active = MainClass.appsettings.timestamps;

            if (MainClass.client != null)
            {

                this.hscale_asset.Value = MainClass.client.Throttle.Asset;
                this.hscale_cloud.Value = MainClass.client.Throttle.Cloud;
                this.hscale_land.Value = MainClass.client.Throttle.Land;
                this.hscale_resend.Value = MainClass.client.Throttle.Resend;
                this.hscale_task.Value = MainClass.client.Throttle.Task;
                this.hscale_texture.Value = MainClass.client.Throttle.Texture;
                this.hscale_wind.Value = MainClass.client.Throttle.Wind;
            }

            this.colorbutton_normal.Color = MainClass.appsettings.convertfromsetting(MainClass.appsettings.color_chat);
            this.colorbutton_object.Color = MainClass.appsettings.convertfromsetting(MainClass.appsettings.color_chat_object);
            this.colorbutton_ownerim.Color =MainClass.appsettings.convertfromsetting(MainClass.appsettings.color_chat_object_owner);

            this.colorbutton_system.Color = MainClass.appsettings.convertfromsetting(MainClass.appsettings.color_chat_system);
            this.colorbutton_typing.Color =MainClass.appsettings.convertfromsetting(MainClass.appsettings.color_chat_typing);
			this.colorbutton_online.Color=MainClass.appsettings.convertfromsetting(MainClass.appsettings.color_chat_online);

			this.checkbutton_notifychat.Active=MainClass.appsettings.notify_chat;
			this.checkbutton_notifyobjectchat.Active=MainClass.appsettings.notify_object_chat;
			this.checkbutton_notifyGroupIM.Active=MainClass.appsettings.notify_group_IM;
			this.checkbutton_notifyIM.Active=MainClass.appsettings.notify_IM;
						
		}

        void applysettings()
        {

            MainClass.appsettings.timestamps = checkbutton_showtimestamps.Active;
            MainClass.appsettings.minimise = this.checkbutton_hideminimise.Active;

            MainClass.appsettings.default_minimim = this.radiobutton1.Active;
            MainClass.appsettings.default_close = this.radiobutton2.Active;

            MainClass.appsettings.ThrottleAsset = (float)this.hscale_asset.Value;
            MainClass.appsettings.ThrottleCloud = (float)this.hscale_cloud.Value;
            MainClass.appsettings.ThrottleLand = (float)this.hscale_land.Value;
            MainClass.appsettings.ThrottleResend = (float)this.hscale_resend.Value;
            MainClass.appsettings.ThrottleTask = (float)this.hscale_task.Value;
            MainClass.appsettings.ThrottleTexture = (float)this.hscale_texture.Value;
            MainClass.appsettings.ThrottleWind = (float)this.hscale_wind.Value;

            if (MainClass.client != null)
            {
                MainClass.client.Throttle.Asset = (float)this.hscale_asset.Value;
                MainClass.client.Throttle.Cloud = (float)this.hscale_cloud.Value;
                MainClass.client.Throttle.Land = (float)this.hscale_land.Value;
                MainClass.client.Throttle.Resend = (float)this.hscale_resend.Value;
                MainClass.client.Throttle.Task = (float)this.hscale_task.Value;
                MainClass.client.Throttle.Texture = (float)this.hscale_texture.Value;
                MainClass.client.Throttle.Wind = (float)this.hscale_wind.Value;
            }

            MainClass.appsettings.color_chat = MainClass.appsettings.converttosetting(this.colorbutton_normal.Color);
            MainClass.appsettings.color_chat_object = MainClass.appsettings.converttosetting(this.colorbutton_object.Color);
            MainClass.appsettings.color_chat_object_owner = MainClass.appsettings.converttosetting(this.colorbutton_ownerim.Color);
            MainClass.appsettings.color_chat_online = MainClass.appsettings.converttosetting(this.colorbutton_online.Color);
            MainClass.appsettings.color_chat_system = MainClass.appsettings.converttosetting(this.colorbutton_system.Color);
            MainClass.appsettings.color_chat_typing = MainClass.appsettings.converttosetting(this.colorbutton_typing.Color);

			MainClass.appsettings.notify_chat=this.checkbutton_notifychat.Active;
			MainClass.appsettings.notify_object_chat=this.checkbutton_notifyobjectchat.Active;
			MainClass.appsettings.notify_group_IM=this.checkbutton_notifyGroupIM.Active;
			MainClass.appsettings.notify_IM=this.checkbutton_notifyIM.Active;
		
            MainClass.appsettings.Save();
		    MainClass.appsettings.notify();
        }


				protected virtual void OnCheckbuttonShowtimestampsClicked (object sender, System.EventArgs e)
				{
			        
                     
			         
		        }

				protected virtual void OnCheckbuttonHideminimiseClicked (object sender, System.EventArgs e)
				{
                   
               
			        
		        }

				protected virtual void OnRadiobutton1Activated (object sender, System.EventArgs e)
				{
			          
                    
			         
		        }

				protected virtual void OnRadiobutton2Activated (object sender, System.EventArgs e)
				{
			       
                   
			         
		        }

				protected virtual void OnButtonApplythrottleClicked (object sender, System.EventArgs e)
				{

			     		
				}

                protected virtual void OnColorbuttonNormalClicked (object sender, System.EventArgs e)
                {
                }

                protected virtual void OnColorbuttonObjectClicked (object sender, System.EventArgs e)
                {
                }

                protected virtual void OnColorbuttonOwnerimClicked (object sender, System.EventArgs e)
                {
                }

                protected virtual void OnColorbuttonObjectimClicked (object sender, System.EventArgs e)
                {
                }

                protected virtual void OnColorbuttonSystemClicked (object sender, System.EventArgs e)
                {
                }

                protected virtual void OnButtonCancelClicked (object sender, System.EventArgs e)
                {
                    this.Destroy();
                }

                protected virtual void OnButtonApplyClicked (object sender, System.EventArgs e)
                {
                    applysettings();
                }

                protected virtual void OnButtonOkClicked (object sender, System.EventArgs e)
                {
                    applysettings();
                    this.Destroy();
                }

	}


    
}
