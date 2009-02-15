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
			this.checkbutton_showtimestamps.Active=MainClass.appsettings.timestamps;			
			this.checkbutton_hideminimise.Active=MainClass.appsettings.minimise;
            this.DeleteEvent += new DeleteEventHandler(OnDeleteEvent);

            if (MainClass.appsettings.default_minimim)
            {
                this.radiobutton1.Active=true;
            }
            else
            {
                this.radiobutton2.Active=true;
            }
						
			this.hscale_asset.Value=     MainClass.client.Throttle.Asset;
			this.hscale_cloud.Value=    MainClass.client.Throttle.Cloud;
			this.hscale_land.Value=     MainClass.client.Throttle.Land;
			this.hscale_resend.Value=    MainClass.client.Throttle.Resend;
			this.hscale_task.Value=      MainClass.client.Throttle.Task;
			this.hscale_texture.Value=     MainClass.client.Throttle.Texture;
			this.hscale_wind.Value=   MainClass.client.Throttle.Wind;
			
			long col=MainClass.appsettings.color_chat;
			this.colorbutton_normal.Color=new Gdk.Color((byte)(col>>16 & 0x0000FF),(byte)(col>>8 & 0x0000FF),(byte)(col>>0 & 0x0000FF));
     
			col=MainClass.appsettings.color_chat_object;
			this.colorbutton_object.Color=new Gdk.Color((byte)(col>>16 & 0x0000FF),(byte)(col>>8 & 0x0000FF),(byte)(col>>0 & 0x0000FF));

			col=MainClass.appsettings.color_chat_object_owner;
			this.colorbutton_ownerim.Color=new Gdk.Color((byte)(col>>16 & 0x0000FF),(byte)(col>>8 & 0x0000FF),(byte)(col>>0 & 0x0000FF));

		//	col=MainClass.appsettings.color_chat_online;
		//	this.col.Color=new Gdk.Color((byte)(col>>16 & 0x0000FF),(byte)(col>>8 & 0x0000FF),(byte)(col>>0 & 0x0000FF));
     
			col=MainClass.appsettings.color_chat_system;
			this.colorbutton_system.Color=new Gdk.Color((byte)(col>>16 & 0x0000FF),(byte)(col>>8 & 0x0000FF),(byte)(col>>0 & 0x0000FF));
    		
			col=MainClass.appsettings.color_chat_typing;
			this.colorbutton_typing.Color=new Gdk.Color((byte)(col>>16 & 0x0000FF),(byte)(col>>8 & 0x0000FF),(byte)(col>>0 & 0x0000FF));
     
			col=MainClass.appsettings.color_chat_online;
			this.colorbutton_online.Color=new Gdk.Color((byte)(col>>16 & 0x0000FF),(byte)(col>>8 & 0x0000FF),(byte)(col>>0 & 0x0000FF));
     
			
			
		}

				protected virtual void OnCheckbuttonShowtimestampsClicked (object sender, System.EventArgs e)
				{
			          MainClass.appsettings.timestamps=checkbutton_showtimestamps.Active;
                     
			         
		        }

				protected virtual void OnCheckbuttonHideminimiseClicked (object sender, System.EventArgs e)
				{
                     MainClass.appsettings.minimise=this.checkbutton_hideminimise.Active;
               
			        
		        }

				protected virtual void OnRadiobutton1Activated (object sender, System.EventArgs e)
				{
			          MainClass.appsettings.default_minimim=true;
			          MainClass.appsettings.default_close=false;
                    
			         
		        }

				protected virtual void OnRadiobutton2Activated (object sender, System.EventArgs e)
				{
			          MainClass.appsettings.default_minimim=false;
			          MainClass.appsettings.default_close=true;
                   
			         
		        }

				protected virtual void OnButtonApplythrottleClicked (object sender, System.EventArgs e)
				{

			       MainClass.appsettings.ThrottleAsset=(float)this.hscale_asset.Value;
			       MainClass.appsettings.ThrottleCloud=(float)this.hscale_cloud.Value;
			       MainClass.appsettings.ThrottleLand=(float)this.hscale_land.Value;
			       MainClass.appsettings.ThrottleResend=(float)this.hscale_resend.Value;
			       MainClass.appsettings.ThrottleTask=(float)this.hscale_task.Value;
			       MainClass.appsettings.ThrottleTexture=(float)this.hscale_texture.Value;
			       MainClass.appsettings.ThrottleWind=(float)this.hscale_wind.Value;
			
			MainClass.client.Throttle.Asset=(float)this.hscale_asset.Value;
			MainClass.client.Throttle.Cloud=(float)this.hscale_cloud.Value; 
		    MainClass.client.Throttle.Land=(float)this.hscale_land.Value; 
			MainClass.client.Throttle.Resend=(float)this.hscale_resend.Value; 
			MainClass.client.Throttle.Task=(float)this.hscale_task.Value; 
			MainClass.client.Throttle.Texture=(float)this.hscale_texture.Value;  
			MainClass.client.Throttle.Wind=(float)this.hscale_wind.Value;
			
	
				
				}


                [GLib.ConnectBefore]
                void OnDeleteEvent(object o, DeleteEventArgs args)
                {

                    MainClass.appsettings.default_minimim = this.radiobutton1.Active;
                    MainClass.appsettings.default_close = this.radiobutton2.Active;
                    MainClass.appsettings.Save();

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

	}


    
}
