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

// TeleportProgress.cs created with MonoDevelop
// User: robin at 19:42 16/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using OpenMetaverse;
using System.Threading;
using Gdk;

namespace omvviewerlight
{
	
	
	public partial class TeleportProgress : Gtk.Window
	{
		
		string tpsim;
		Vector3 tppos;
		UUID landmark;
 
		
		public TeleportProgress() : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
	        MainClass.client.Self.TeleportProgress += new EventHandler<TeleportEventArgs>(Self_TeleportProgress);
        }

    	
		public void teleportassetid(UUID asset,string name)
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

		public void teleport(string sim,Vector3 pos)
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
			if(!MainClass.client.Grid.GetGridRegion(tpsim,OpenMetaverse.GridLayerType.Objects, out region))
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

        void Self_TeleportProgress(object sender, TeleportEventArgs e)
        {
		    Gtk.Application.Invoke(delegate {						
						
			Console.Write("TP update "+e.Status.ToString()+"\n");
			
			this.label_info.Text=e.Message;
				
			if(OpenMetaverse.TeleportStatus.Start==e.Status)
				progressbar1.Fraction=0.2;
				
			if(OpenMetaverse.TeleportStatus.Progress==e.Status)
			{
			     Console.Write("Progress\n");
				 progressbar1.Fraction+=0.2;
			}	
			
			if(OpenMetaverse.TeleportStatus.Finished==e.Status)
			{
					progressbar1.Fraction=1.0;
					GLib.Timeout.Add(1000,closewindow);
					this.button_close.Sensitive=true;
			}

			if(OpenMetaverse.TeleportStatus.Cancelled==e.Status)
			{
					progressbar1.Fraction=1.0;
					this.button_close.Sensitive=true;
					this.label_info.Text="Teleport Cancelled";
			}

			if(OpenMetaverse.TeleportStatus.Failed==e.Status)
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
            MainClass.client.Self.TeleportProgress -= new EventHandler<TeleportEventArgs>(Self_TeleportProgress);
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
