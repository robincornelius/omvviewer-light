// LoginControl.cs created with MonoDevelop
// User: robin at 08:56 11/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Threading;
using libsecondlife;
using System.IO;

namespace omvviewerlight
{	
	public partial class LoginControl : Gtk.Bin
	{
		LoginParams login;
		
		bool trying;
		
		~LoginControl()
		{
			FileInfo f = new FileInfo("Mytext.txt");
			StreamWriter w = f.CreateText();
			w.WriteLine(entry_first.Text);
			w.WriteLine(entry_last.Text);
			if(this.checkbutton_rememberpass.Active)
			{
				w.WriteLine("store_pass");
				w.WriteLine(entry_pass.Text);
			}
			w.Close();
			
		}
		
		
		public LoginControl()
		{
			this.Build();
			MainClass.client.Network.OnConnected += new libsecondlife.NetworkManager.ConnectedCallback(onConnected);
			MainClass.client.Network.OnDisconnected += new libsecondlife.NetworkManager.DisconnectedCallback(onDisconnected);
			MainClass.client.Network.OnLogin += new libsecondlife.NetworkManager.LoginCallback(onLogin);
			MainClass.client.Network.OnEventQueueRunning += new libsecondlife.NetworkManager.EventQueueRunningCallback(onEventQueue);
			libsecondlife.Logger.OnLogMessage += new libsecondlife.Logger.LogCallback(onLogMessage);
			this.entry_pass.Visibility=false;
			
			// SUper dirty hack
			// todo WRITE A PROPER FILE HANDLIER
			// MAY BE A NICE XML FORMAT OPTION
			try
			{			
				StreamReader s = File.OpenText("Mytext.txt");			
				entry_first.Text=s.ReadLine();
				entry_last.Text=s.ReadLine();
				string x;
				x=s.ReadLine();
				if(x=="store_pass")
				{
					entry_pass.Text=s.ReadLine();
					this.checkbutton_rememberpass.Active=true;

				}
				s.Close();
			
			}
			catch(IOException e)
			{
			
			}
			
			
		}	
		
		void onEventQueue(Simulator sim)
		{
			Console.Write("Event queue runnimg\n");
			if(sim.ID==MainClass.client.Network.CurrentSim.ID)
			{
				Console.Write("FOR CURRENT SIM\n");
				this.trying=false;
				Thread loginRunner= new Thread(new ThreadStart(this.appearencethread));                               
				loginRunner.Start();
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
		
		void onConnected(object sender)
		{
			Console.Write("Connected to simulator\n");
		}
		
		void onDisconnected(libsecondlife.NetworkManager.DisconnectType reason, string message)
		{
			Gtk.Application.Invoke(delegate {
				this.button_login.Label="Login";
			    });			
			
		}
		
		/// <summary>Called any time the login status changes, will eventually
        /// return LoginStatus.Success or LoginStatus.Failure</summary>
		void onLogin(LoginStatus login, string message)
		{
			Gtk.Application.Invoke(delegate {
				this.textview_loginmsg.Buffer.Text=message;	
				this.textview_loginmsg.QueueDraw();
			});
			
			if(LoginStatus.Failed==login)
				Gtk.Application.Invoke(delegate {
					this.button_login.Label="Login";
					this.trying=false;
			    });			
	
			//This can take ages, should be threaded
			if(LoginStatus.Success==login)
			{
				Console.Write("Login status login\n");
			//	Thread loginRunner= new Thread(new ThreadStart(this.appearencethread));                               
			//	MainClass.client.Groups.RequestCurrentGroups();
			//	MainClass.client.Self.RetrieveInstantMessages();
			//	loginRunner.Start();
			}		
		}

		void onLogMessage(object obj, libsecondlife.Helpers.LogLevel level)
		{
			if(level >= libsecondlife.Helpers.LogLevel.Warning)
			{
				Gtk.Application.Invoke(delegate {
					this.textview_log.Buffer.InsertAtCursor(obj.ToString()+"\n");
					this.textview_log.ScrollMarkOnscreen(textview_log.Buffer.InsertMark);
					this.textview_log.QueueDraw();
				});			
			}				
			
		}
		
		void loginthread()
		{
			Console.Write("Login thread go\n");
			MainClass.client.Network.Login(login);
		}
		
		void appearencethread()
		{
			Console.Write("Appearence thread go\n");
			MainClass.client.Groups.RequestCurrentGroups();
			MainClass.client.Self.RetrieveInstantMessages();		
			MainClass.client.Appearance.SetPreviousAppearance(false);
		}
		
		protected virtual void OnButton1Clicked (object sender, System.EventArgs e)
		{
			if(button_login.Label=="Login")
			{
				trying=true;
				GLib.Timeout.Add(100,OnPulseProgress);
				
				this.textview_loginmsg.Buffer.Text="Connecting to login server...";
				this.textview_loginmsg.QueueDraw();
				//LoginParams login;
				login=MainClass.client.Network.DefaultLoginParams(entry_first.Text,entry_last.Text,entry_pass.Text,"omvviewer-light","1.0");
				
				//login.Start="home";
				
				this.textview_log.Buffer.Clear();
				button_login.Label="Logout";			
				if(this.combobox_grid.ActiveText=="Agni")
				      login.URI="https://login.agni.lindenlab.com/cgi-bin/login.cgi";
				
				if(this.combobox_grid.ActiveText=="Aditi")
				      login.URI="https://login.aditi.lindenlab.com/cgi-bin/login.cgi";
				                                                  			                                                  
				if(this.combobox_grid.ActiveText=="Local")
				      login.URI="http://127.0.0.1:9000";
				
				if(this.combobox_grid.ActiveText=="Custom")
				      login.URI=this.entry_loginuri.Text;
	                                                 
				Thread loginRunner= new Thread(new ThreadStart(this.loginthread));                               
				
				loginRunner.Start();

}
			else
			{
				trying=false;
				MainClass.client.Network.Logout();
			}
		}

		protected virtual void OnCheckbuttonRememberpassClicked (object sender, System.EventArgs e)
		{
		}
	}
}
