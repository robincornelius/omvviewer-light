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

		string logbuffer;
		bool newdata;
		LoginParams login;
		
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
		void onConnected(object sender)
		{
			
			
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
			    });			
	
			//This can take ages, should be threaded
			if(LoginStatus.Success==login)
				MainClass.client.Appearance.SetPreviousAppearance(false);
		
		}

		void onLogMessage(object obj, libsecondlife.Helpers.LogLevel level)
		{
			if(level >= libsecondlife.Helpers.LogLevel.Warning)
			{
				Gtk.Application.Invoke(delegate {
					this.textview_log.Buffer.InsertAtCursor(obj.ToString()+"\n");
					this.textview_log.ScrollMarkOnscreen(textview_log.Buffer.InsertMark);
				});			
			}				
			
		}
		
		void loginthread()
		{
				MainClass.client.Network.Login(login);
		}
		
		protected virtual void OnButton1Clicked (object sender, System.EventArgs e)
		{
			if(button_login.Label=="Login")
			{
				this.textview_loginmsg.Buffer.SetText("Connecting to login server...");
				this.textview_loginmsg.QueueDraw();
				//LoginParams login;
				login=MainClass.client.Network.DefaultLoginParams(entry_first.Text,entry_last.Text,entry_pass.Text,"omvviewer-light","1.0");
				
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
				MainClass.client.Network.Logout();
			}
		}

		protected virtual void OnCheckbuttonRememberpassClicked (object sender, System.EventArgs e)
		{
		}
	}
}
