// LoginControl.cs created with MonoDevelop
// User: robin at 08:56 11/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using libsecondlife;

namespace omvviewerlight
{	
	public partial class LoginControl : Gtk.Bin
	{
		
		public LoginControl()
		{
			this.Build();
			MainClass.client.Network.OnConnected += new libsecondlife.NetworkManager.ConnectedCallback(onConnected);
			MainClass.client.Network.OnDisconnected += new libsecondlife.NetworkManager.DisconnectedCallback(onDisconnected);
			MainClass.client.Network.OnLogin += new libsecondlife.NetworkManager.LoginCallback(onLogin);
			//libsecondlife.Logger.OnLogMessage += new libsecondlife.Logger.LogCallback(onLogMessage);
			
		}  
		
		void onConnected(object sender)
		{
			
			
		}
		
		void onDisconnected(libsecondlife.NetworkManager.DisconnectType reason, string message)
		{
			this.button_login.Label="Login";
		}
		
		/// <summary>Called any time the login status changes, will eventually
        /// return LoginStatus.Success or LoginStatus.Failure</summary>
		void onLogin(LoginStatus login, string message)
		{
			this.textview_loginmsg.Buffer.Text=message;	
			if(LoginStatus.Failed==login)
				this.button_login.Label="Login";		
			
			if(LoginStatus.Success==login)
				MainClass.client.Appearance.SetPreviousAppearance(false);
		
		}

		void onLogMessage(object obj, libsecondlife.Helpers.LogLevel level)
		{
			if(level >= libsecondlife.Helpers.LogLevel.Warning)
				this.textview_log.Buffer.InsertAtCursor(obj.ToString()+"\n");
		}
		
		protected virtual void OnButton1Clicked (object sender, System.EventArgs e)
		{
			if(button_login.Label=="Login")
			{
				this.textview_log.Buffer.Clear();
				button_login.Label="Logout";
				MainClass.client.Network.Login(this.entry_first.Text, this.entry_last.Text, this.entry_pass.Text, "My First Bot", "Your name");
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
