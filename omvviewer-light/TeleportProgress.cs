// TeleportProgress.cs created with MonoDevelop
// User: robin at 19:42 16/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using libsecondlife;
using System.Threading;
using Gdk;

namespace omvviewerlight
{
	
	
	public partial class TeleportProgress : Gtk.Window
	{
		
		string tpsim;
		LLVector3 tppos;
		
		public TeleportProgress() : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			MainClass.client.Self.OnTeleport += new libsecondlife.AgentManager.TeleportCallback(onTeleport);
		}
		
		public void teleporthome()
		{
			MainClass.client.Self.GoHome();
			
		}

		public void teleport(string sim,LLVector3 pos)
		{
			Gtk.Application.Invoke(delegate {						
				this.label_sim.Text=sim;
				this.label_loc.Text=pos.ToString();
				this.tppos=pos;
				this.tpsim=sim;
				this.QueueDraw();
				Thread tpRunner= new Thread(new ThreadStart(this.tpthread));   			
				tpRunner.Start();
			});
	
			
		}
		
		void tpthread()
		{
			MainClass.client.Self.Teleport(tpsim,tppos);				
		}
		
		void onTeleport(string Message, libsecondlife.AgentManager.TeleportStatus status,libsecondlife.AgentManager.TeleportFlags flags)
		{
		Gtk.Application.Invoke(delegate {						
						
			Console.Write("TP update "+status.ToString()+"\n");
			
			this.label_info.Text=Message;
				
			if(libsecondlife.AgentManager.TeleportStatus.Start==status)
				progressbar1.Fraction=0.2;
				
			if(libsecondlife.AgentManager.TeleportStatus.Progress==status)
			{
			     Console.Write("Progress\n");
				progressbar1.Fraction+=0.2;
			}	
			
			if(libsecondlife.AgentManager.TeleportStatus.Finished==status)
			{
					progressbar1.Fraction=1.0;
					GLib.Timeout.Add(1000,closewindow);
					this.button_close.Sensitive=true;
			}

			if(libsecondlife.AgentManager.TeleportStatus.Cancelled==status)
			{
					progressbar1.Fraction=1.0;
					this.button_close.Sensitive=true;
					this.label_info.Text="Teleport Cancelled";
			}

			if(libsecondlife.AgentManager.TeleportStatus.Failed==status)
			{
					progressbar1.Fraction=1.0;
					this.button_close.Sensitive=true;
					this.label_info.Text="Teleport FAILED, sorry";
			}

				
			this.QueueDraw();
			
			});
		}
		
		bool closewindow()
		{
			this.Destroy();
			return false;
		}

		protected virtual void OnButtonAbortClicked (object sender, System.EventArgs e)
		{
			//MainClass.client.Network.tel
		}

		protected virtual void OnButtonCloseClicked (object sender, System.EventArgs e)
		{
			closewindow();
		}
	}
}
