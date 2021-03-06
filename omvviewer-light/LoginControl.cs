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

// LoginControl.cs created with MonoDevelop
// User: robin at 08:56 11/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Threading;
using OpenMetaverse;
using System.IO;
using System.Collections.Generic;
using System.Collections;


namespace omvviewerlight
{	
	[System.ComponentModel.ToolboxItem(true)]
	public partial class LoginControl : Gtk.Bin
	{
		Dictionary<string,string> gridlist = new Dictionary<string,string>();
		LoginParams login;
		bool loginbut=true;
		bool trying;
        Gtk.TextTag [] logleveltag= new Gtk.TextTag[5];

		public LoginControl()
		{
			this.Build();

            build_tag_table();

            MainClass.onRegister += new MainClass.register(MainClass_onRegister);
            MainClass.onDeregister += new MainClass.deregister(MainClass_onDeregister);
            if(MainClass.client != null ) { MainClass_onRegister(); }

            this.entry_pass.Visibility=false;

            entry_first.Text =  MainClass.appsettings.FirstName;
            entry_last.Text = MainClass.appsettings.LastName;
            entry_pass.Text = MainClass.appsettings.Password;
            checkbutton_rememberpass.Active = MainClass.appsettings.remember_pass;
		   
			 StreamReader s=null;
			 if(File.Exists(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory,"gridlist.txt")))
			   s = File.OpenText(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory,"gridlist.txt"));

			if(s==null)
			   if(File.Exists("~/.gridlist.txt"))
			      s = File.OpenText("~/.gridlist.txt");		
			if(s==null)
			      if(File.Exists("gridlist.txt"))
			         s = File.OpenText("gridlist.txt");
			
			if(s==null)
				Logger.Log("Can't find a gridlist.txt",Helpers.LogLevel.Debug);
				
			try
			{
				char[] splitter  = {' '};
				string[] arInfo = new string[2];

				while(!s.EndOfStream)
				{
					string line=s.ReadLine();
					arInfo=line.Split(splitter,2);
					gridlist.Add(arInfo[0],arInfo[1]);
					this.combobox_grid.AppendText(arInfo[0]);
				}
			}
			catch(Exception e)
			{
                Logger.Log(e.ToString(),Helpers.LogLevel.Debug);
			}					
			
			int selected=MainClass.appsettings.SelectedGrid;
	
			try
			{
				combobox_grid.Active=selected;			
			}
			catch
			{
				
			}

            string target=MainClass.appsettings.LoginLocationSetting;

            if (target == "home")
                this.radiobutton1.Active = true;

            if (target == "last")
                this.radiobutton2.Active = true;

            if (target == "location")
            {
                this.radiobutton1.Active = true;
                this.entry_location.Text=MainClass.appsettings.LoginLocation;

            }
		}


        void MainClass_onDeregister()
        {

            MainClass.client.Network.SimConnected -= new EventHandler<SimConnectedEventArgs>(Network_SimConnected);
            MainClass.client.Network.Disconnected -= new EventHandler<DisconnectedEventArgs>(Network_Disconnected);
            MainClass.client.Network.LoginProgress -= new EventHandler<LoginProgressEventArgs>(Network_LoginProgress);
            MainClass.client.Network.EventQueueRunning -= new EventHandler<EventQueueRunningEventArgs>(Network_EventQueueRunning);
            OpenMetaverse.Logger.OnLogMessage -= new OpenMetaverse.Logger.LogCallback(onLogMessage);
        }

        void MainClass_onRegister()
        {

       
            MainClass.client.Network.SimConnected += new EventHandler<SimConnectedEventArgs>(Network_SimConnected);
            MainClass.client.Network.Disconnected += new EventHandler<DisconnectedEventArgs>(Network_Disconnected);
            MainClass.client.Network.LoginProgress += new EventHandler<LoginProgressEventArgs>(Network_LoginProgress);
            MainClass.client.Network.EventQueueRunning += new EventHandler<EventQueueRunningEventArgs>(Network_EventQueueRunning);

            OpenMetaverse.Settings.LOG_LEVEL = OpenMetaverse.Helpers.LogLevel.Debug;
            OpenMetaverse.Logger.OnLogMessage += new OpenMetaverse.Logger.LogCallback(onLogMessage);

        }


        new public void Dispose()
        {
            Logger.Log("Disposing of the login control",Helpers.LogLevel.Debug);

            MainClass.onRegister -= new MainClass.register(MainClass_onRegister);
            MainClass.onDeregister -= new MainClass.deregister(MainClass_onDeregister);
            MainClass_onDeregister();
        }

        void oncleanuptime()
        {
			
			MainClass.appsettings.FirstName=entry_first.Text;
		    MainClass.appsettings.LastName= entry_last.Text;
           
            if(this.checkbutton_rememberpass.Active)
            {
               MainClass.appsettings.Password=entry_pass.Text;
            }
            else
            {
                MainClass.appsettings.Password="";
            }
			
			MainClass.appsettings.Save();
            MainClass.appsettings.remember_pass=this.checkbutton_rememberpass.Active;
			MainClass.appsettings.SelectedGrid=combobox_grid.Active;
		
		}



        void Network_EventQueueRunning(object sender, EventQueueRunningEventArgs e)
		{
			if(e.Simulator.ID==MainClass.client.Network.CurrentSim.ID)
			{
				this.trying=false;
				MainClass.client.Self.Movement.Flags=0;
				MainClass.client.Self.Movement.SendUpdate();	
			}	
		}
		
		bool OnPulseProgress()
		{

			if(trying==true)
			{
				this.progressbar2.Pulse();
				progressbar2.QueueDraw();
			}			
			else
			{
				this.progressbar2.Fraction=1.0;
				return false;
			}	
			
			return true;
		}



        void Network_SimConnected(object sender, SimConnectedEventArgs e)
		{
			Console.Write("Connected to simulator\n");
		}
		
        void Network_Disconnected(object sender, DisconnectedEventArgs e)
		{
			Logger.Log("on disconnected",Helpers.LogLevel.Debug);
			Gtk.Application.Invoke(delegate {
				this.button_login.Label="Login";
				this.loginbut=true;
				this.trying=false;
                progressbar2.Fraction = 0.0;
				this.enablebuttons();
                if (!MainClass.win.Visible)
                {
                    MainClass.win.trayIcon.Blinking = true;
                    MainClass.win.UrgencyHint = true;
                    MainClass.win.trayIcon.Blinking = true;      
                }

                if (MainClass.userlogout == false)
                {
                    Gtk.MessageDialog md = new Gtk.MessageDialog(MainClass.win, Gtk.DialogFlags.Modal, Gtk.MessageType.Warning, Gtk.ButtonsType.Close, false, "You have been disconnected from the current simulator");
                    md.Run();
                    md.Destroy();
                }
                else
                {


                }

                //MainClass.userlogout = false;
                MainClass.killclient();

			});
		}
		
		/// <summary>Called any time the login status changes, will eventually
        /// return LoginStatus.Success or LoginStatus.Failure</summary>
        void Network_LoginProgress(object sender, LoginProgressEventArgs e)
        {
			Gtk.Application.Invoke(delegate {
				this.textview_loginmsg.Buffer.Text=e.Message;	
				this.textview_loginmsg.QueueDraw();
			});
			
			if(LoginStatus.Failed==e.Status)
				Gtk.Application.Invoke(delegate {
					this.button_login.Label="Login";
					this.loginbut=true;
				    button_login.Image=new Gtk.Image(Stetic.IconLoader.LoadIcon(this, "gtk-connect", Gtk.IconSize.Menu, 16));
					this.trying=false;
					this.enablebuttons();
                    MainClass.killclient();
			    });			
	
			if(LoginStatus.Success==e.Status)
			{
				Console.Write("Login status login\n");
                Thread.Sleep(5000);
    

				MainClass.client.Groups.RequestCurrentGroups();
				MainClass.client.Self.RetrieveInstantMessages();
				
				MainClass.client.Throttle.Cloud = MainClass.appsettings.ThrottleCloud;
			    MainClass.client.Throttle.Wind = MainClass.appsettings.ThrottleWind;
			    MainClass.client.Throttle.Land =  MainClass.appsettings.ThrottleLand;
				MainClass.client.Throttle.Asset =  MainClass.appsettings.ThrottleAsset;
				MainClass.client.Throttle.Resend =  MainClass.appsettings.ThrottleResend;
				MainClass.client.Throttle.Task=  MainClass.appsettings.ThrottleTask;
				MainClass.client.Throttle.Texture= MainClass.appsettings.ThrottleTexture;	
                MainClass.userlogout = false;          
              }
		}

		void onLogMessage(object obj, OpenMetaverse.Helpers.LogLevel level)
		{

			{
				Gtk.Application.Invoke(delegate {

                    Gtk.TextTag tag;

                    switch (level)
                    {
                        case Helpers.LogLevel.Debug:
                            tag = logleveltag[0];
                            break;
                        case Helpers.LogLevel.None:
                            tag = logleveltag[1];
                            break;
                        case Helpers.LogLevel.Info:
                            tag = logleveltag[2];
                            break;
                        case Helpers.LogLevel.Warning:
                            tag = logleveltag[3];
                            break;
                        case Helpers.LogLevel.Error:
                            tag = logleveltag[4];
                            break;
                        default:
                            tag = logleveltag[1];
                            break;
                    }

                    textview_log.Buffer.InsertWithTags(textview_log.Buffer.EndIter,obj.ToString()+"\n",tag);
                    textview_log.ScrollToIter(textview_log.Buffer.EndIter, 0, false, 0, 0);
                });			
			}				
			
		}
		
		void loginthread()
		{
			Console.Write("Login thread go\n");
            try
            {
                MainClass.client.Network.Login(login);
            }
            catch (Exception e)
            {
                Console.Write("Login throw an exception\n");
                Console.Write(e.ToString()+"\n");
                Gtk.Application.Invoke(delegate
                {
                    this.button_login.Label = "Login";
					this.loginbut=true;
					this.trying=false;
                    this.enablebuttons();
                });

                try
                {
					Logger.Log("Trying to logout after login exception",Helpers.LogLevel.Debug);
                    MainClass.client.Network.Logout();
                }
                catch (Exception ee)
                {
                    Console.Write("Additional exception cleaning up logout\n");
                    Console.Write(ee.ToString()+"\n");
                }

                this.trying = false;
            }
        }
				
		protected virtual void OnButton1Clicked (object sender, System.EventArgs e)
		{
            MainClass.userlogout = true;

			oncleanuptime();
			Logger.Log("button 1 clicked "+button_login.Label,Helpers.LogLevel.Debug);
			if(this.loginbut==true)
			{
				this.button_login.Sensitive=false;
				GLib.Timeout.Add(1000,debounce);

				this.disablebuttons();
				trying=true;
				GLib.Timeout.Add(100,OnPulseProgress);
				
				this.textview_loginmsg.Buffer.Text="Connecting to login server...";
				this.textview_loginmsg.QueueDraw();
				//LoginParams login;

                MainClass.getMeANewClient();

				login=MainClass.client.Network.DefaultLoginParams(entry_first.Text,entry_last.Text,entry_pass.Text,"omvviewer","2.0");
				StreamReader s=null;
              
				 if(File.Exists(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory,"MyMac.txt")))
				     s = File.OpenText(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory,"MyMac.txt"));
				 if(s==null)
					if(File.Exists("~/.MyMac.txt"))
						s = File.OpenText("~/.MyMac.txt");	  
				  if(s==null)
					if(File.Exists("MyMac.txt"))
					    s = File.OpenText("MyMac.txt");
				
				if(s!=null)
				{
					try
	                {
	                    login.MAC = s.ReadLine();
	                }
	                catch(Exception ee)
	                {
	                    Logger.Log(ee.ToString(),Helpers.LogLevel.Debug);
	                }
				}

                if (this.radiobutton1.Active)
                {
                    login.Start = "home";
					MainClass.appsettings.LoginLocationSetting="home";
                }
                if (this.radiobutton2.Active)
                {
                    login.Start = "last";
                    MainClass.appsettings.LoginLocationSetting="last";
                }

                if (this.radiobutton3.Active)
                {
                    login.Start = "uri:"+this.entry_location.Text+"&128&128&0";
					MainClass.appsettings.LoginLocationSetting="location";
					MainClass.appsettings.LoginLocation=entry_location.Text;
                }
				
				this.textview_log.Buffer.Clear();
				button_login.Label="Logout";
				this.loginbut=false;
			    button_login.Image=new Gtk.Image(Stetic.IconLoader.LoadIcon(this, "gtk-disconnect", Gtk.IconSize.Menu, 16));
				
				login.URI=entry_loginuri.Text;

				Thread loginRunner= new Thread(new ThreadStart(this.loginthread));                               		
				loginRunner.Start();

            }
			else
			{
                Logger.Log("Trying to logout user request",Helpers.LogLevel.Debug);
                this.button_login.Sensitive = false;
                trying = true;
                progressbar2.Fraction=0.1;
                GLib.Timeout.Add(100, OnPulseProgress);
                
                Logger.Log("Running logout tasks first",Helpers.LogLevel.Debug);
                if (MainClass.client.Inventory.Store != null)
                    MainClass.client.Inventory.Store.SaveToDisk(MainClass.client.Settings.ASSET_CACHE_DIR + System.IO.Path.DirectorySeparatorChar + MainClass.client.Inventory.Store.RootFolder.UUID.ToString() + ".osl");
               
                Logger.Log("Done",Helpers.LogLevel.Debug);
                MainClass.userlogout = true;
				MainClass.client.Network.Logout();
				this.trying=false;
			}
		}

		protected virtual void OnCheckbuttonRememberpassClicked (object sender, System.EventArgs e)
		{
		}

		void disablebuttons()
		{
			this.entry_first.Sensitive=false;
			this.entry_last.Sensitive=false;
			this.entry_loginuri.Sensitive=false;
			this.entry_pass.Sensitive=false;
			this.combobox_grid.Sensitive=false;
			this.checkbutton_rememberpass.Sensitive=false;
			this.radiobutton1.Sensitive=false;
			this.radiobutton2.Sensitive=false;
			this.radiobutton3.Sensitive=false;
			this.entry_location.Sensitive=false;
			
		}
		
		void enablebuttons()
		{			
			this.entry_first.Sensitive=true;
			this.entry_last.Sensitive=true;
			this.entry_loginuri.Sensitive=true;
			this.entry_pass.Sensitive=true;
			this.combobox_grid.Sensitive=true;	
			this.checkbutton_rememberpass.Sensitive=true;
			this.radiobutton1.Sensitive=true;
			this.radiobutton2.Sensitive=true;
			this.radiobutton3.Sensitive=true;
			this.entry_location.Sensitive=true;
            this.button_login.Sensitive = true;

		}

		protected virtual void OnComboboxGridChanged (object sender, System.EventArgs e)
		{
			string grid=this.combobox_grid.ActiveText;
			string uri;

            if (grid == null)
                return;

			if(gridlist.TryGetValue(grid,out uri))
		        this.entry_loginuri.Text=uri;
		}
		
		bool debounce()
		{
     			this.button_login.Sensitive=true;
			    return false;
		}

        void build_tag_table()
        {
            logleveltag[0] = new Gtk.TextTag("Debug");
            logleveltag[1] = new Gtk.TextTag("Info");
            logleveltag[2] = new Gtk.TextTag("None");
            logleveltag[3] = new Gtk.TextTag("Warning");
            logleveltag[4] = new Gtk.TextTag("Error");

            logleveltag[0].ForegroundGdk = new Gdk.Color(0,0,0);
            logleveltag[1].ForegroundGdk = new Gdk.Color(38,34,232);
            logleveltag[2].ForegroundGdk = new Gdk.Color(232,134,192);
            logleveltag[3].ForegroundGdk = new Gdk.Color(243,147,10);
            logleveltag[4].ForegroundGdk = new Gdk.Color(255, 0, 0);

        }
	
	}
}
