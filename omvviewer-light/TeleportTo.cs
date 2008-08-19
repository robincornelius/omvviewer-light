// TeleportTo.cs created with MonoDevelop
// User: robin at 16:23 12/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using libsecondlife;
using Gtk;


namespace omvviewerlight
{
	
	public partial class TeleportTo : Gtk.Bin
	{
		
		public TeleportTo()
		{
			this.Build();		
			GLib.Timeout.Add(1000,OnTimeout);
			MainClass.client.Self.OnTeleport += new libsecondlife.AgentManager.TeleportCallback(onTeleport);
			MainClass.client.Network.OnLogin += new libsecondlife.NetworkManager.LoginCallback(onLogin);

		}

		void onLogin(LoginStatus login, string message)
		{
            Gtk.Application.Invoke(delegate
            {

                if (login == libsecondlife.LoginStatus.Success)
                {
                    this.spinbutton_x.Value = MainClass.client.Self.SimPosition.X;
                    this.spinbutton_y.Value = MainClass.client.Self.SimPosition.Y;
                    this.spinbutton_z.Value = MainClass.client.Self.SimPosition.Z;
                    this.entry_simname.Text = MainClass.client.Network.CurrentSim.Name;
                }
            });
		}
		
		void onTeleport(string Message, libsecondlife.AgentManager.TeleportStatus status,libsecondlife.AgentManager.TeleportFlags flags)
		{
            Gtk.Application.Invoke(delegate
            {
                this.spinbutton_x.Value = MainClass.client.Self.SimPosition.X;
                this.spinbutton_y.Value = MainClass.client.Self.SimPosition.Y;
                this.spinbutton_z.Value = MainClass.client.Self.SimPosition.Z;
                this.entry_simname.Text = MainClass.client.Network.CurrentSim.Name;
            });
            }
		
	    bool OnTimeout()
		{
			if(MainClass.client.Network.LoginStatusCode==libsecondlife.LoginStatus.Success)
			{
				this.label_current.Text="Current Location: "+MainClass.client.Network.CurrentSim.Name+" "+MainClass.client.Self.SimPosition;
			}
			
			return true;
		}
		
		protected virtual void OnButtonTeleportActivated (object sender, System.EventArgs e)
		{
		}

		protected virtual void OnButtonTeleportClicked (object sender, System.EventArgs e)
		{
			LLVector3 pos;
			pos=new LLVector3();
			pos.X=(float)this.spinbutton_x.Value;
			pos.Y=(float)this.spinbutton_y.Value;
			pos.Z=(float)this.spinbutton_z.Value;
			TeleportProgress tp = new TeleportProgress();
			tp.Show();
			tp.teleport(entry_simname.Text,pos);
			
			//MainClass.client.Self.Teleport(entry_simname.Text,pos);	
		
		}

		protected virtual void OnButtonTphomeClicked (object sender, System.EventArgs e)
		{
			//MainClass.client.Self.GoHome();
			TeleportProgress tp = new TeleportProgress();
			tp.Show();
			tp.teleporthome();
		}
	}
}
