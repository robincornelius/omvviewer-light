/*
omvviewer-light a Text based client to metaverses such as Linden Labs Secondlife(tm)
    Copyright (C) 2008  Robin Cornelius <robin.cornelius@gmail.com>

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

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
		LLUUID landmark;
 
		
		public TeleportProgress() : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			MainClass.client.Self.OnTeleport += new libsecondlife.AgentManager.TeleportCallback(onTeleport);
		}
		
		public void teleportassetid(LLUUID asset,string name)
		{
   
			landmark=asset;
            tpsim = name;
            this.label_sim.Text = name;
            this.label_loc.Text = "";
			Thread tpRunner= new Thread(new ThreadStart(this.tptolandmark));   			
			tpRunner.Start();
		}
		                                      
		
		public void teleporthome()
		{
            this.label_sim.Text = "Home";
            this.label_loc.Text = "";
			Thread tpRunner= new Thread(new ThreadStart(this.gohomethread));   			
 			tpRunner.Start();		
		}

		public void teleport(string sim,LLVector3 pos)
		{
			Gtk.Application.Invoke(delegate {						
				this.label_sim.Text=sim;
				this.label_loc.Text=pos.ToString();
				this.tppos=pos;
				this.tpsim=sim;
				this.QueueDraw();
			});
				
			    Thread tpRunner= new Thread(new ThreadStart(this.tpthread));   			
				tpRunner.Start();
		}
		
		void tpthread()
		{
			Thread.Sleep(1000);
			GridRegion region;
			if(!MainClass.client.Grid.GetGridRegion(tpsim,libsecondlife.GridLayerType.Objects, out region))
			{
				Gtk.Application.Invoke(delegate {
					this.button_close.Sensitive=true;
					this.label_info.Text="No such region :"+tpsim;
					this.progressbar1.Fraction=1.0;
					return;
				});
				
			}
			//GetGridRegion
			MainClass.client.Self.Teleport(tpsim,tppos);				
		}
		
		void tptolandmark()		
		{
        
			   Thread.Sleep(1000);
			   MainClass.client.Self.Teleport(landmark);
			
		}
		
		void gohomethread()
		{
			Thread.Sleep(1000);
		    MainClass.client.Self.GoHome();
			
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
					//GLib.Timeout.Add(1000,closewindow);
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
