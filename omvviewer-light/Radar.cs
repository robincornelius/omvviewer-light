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

// Radar.cs created with MonoDevelop
// User: robin at 21:37 11/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;
using OpenMetaverse;

namespace omvviewerlight
{

	[System.ComponentModel.ToolboxItem(true)]	
    public partial class Radar : Gtk.Bin
	{
		
		Gtk.ListStore store;	
		private Dictionary<UUID, bool> av_typing = new Dictionary<UUID, bool>();
        private Dictionary<UUID, Gtk.TreeIter> av_tree = new Dictionary<UUID, Gtk.TreeIter>();
        UUID lastsim = new UUID();
		const float DISTANCE_BUFFER = 3f;

		bool running=true;
		~Radar()
		{
			Console.WriteLine("Radar Cleaned up");
		}		
		
		public Radar()
		{      
			store= new Gtk.ListStore (typeof(string),typeof(string),typeof(string),typeof(UUID));
			this.Build();
			Gtk.TreeViewColumn tvc;
			treeview_radar.AppendColumn("",new Gtk.CellRendererText(),"text",0);
			tvc=treeview_radar.AppendColumn("Name",new Gtk.CellRendererText(),"text",1);
			//tvc.Resizable=true;
			tvc.Sizing=Gtk.TreeViewColumnSizing.Autosize;
			
			treeview_radar.AppendColumn("Dist.",new Gtk.CellRendererText(),"text",2);
			treeview_radar.Model=store;

            MainClass.client.Grid.OnCoarseLocationUpdate += new GridManager.CoarseLocationUpdateCallback(Grid_OnCoarseLocationUpdate);
			MainClass.client.Self.OnChat += new OpenMetaverse.AgentManager.ChatCallback(onChat);
			MainClass.client.Network.OnLogin += new OpenMetaverse.NetworkManager.LoginCallback(onLogin);
	
			MainClass.client.Self.OnTeleport += new OpenMetaverse.AgentManager.TeleportCallback(onTeleport);
            
			AutoPilot.onAutoPilotFinished+=new AutoPilot.AutoPilotFinished(onAutoPilotFinished);

            MainClass.client.Network.OnSimDisconnected += new NetworkManager.SimDisconnectedCallback(Network_OnSimDisconnected);
			
			this.store.SetSortFunc(2,sort_Vector3);	
            store.SetSortColumnId(2,Gtk.SortType.Ascending);
            Gtk.Timeout.Add(10000, kickrefresh);
		}

        void Network_OnSimDisconnected(Simulator simulator, NetworkManager.DisconnectType reason)
        {
            lock (simulator.ObjectsAvatars)  
            {
                simulator.AvatarPositions.ForEach(delegate (KeyValuePair <UUID,Vector3> kvp)
                {
                    if (kvp.Value != null && kvp.Key != UUID.Zero)
                    {
                        lock (av_tree)
                        {
                            if (av_tree.ContainsKey(kvp.Key))
                            {
                                Gtk.TreeIter iter = av_tree[kvp.Key];
                                store.Remove(ref iter);
                                av_tree.Remove(kvp.Key);
                            }
                        }
                    }
                });
            }
        }

        void Grid_OnCoarseLocationUpdate(Simulator sim, List<UUID> newEntries, List<UUID> removedEntries)
        {
            Gtk.Application.Invoke(delegate
            {
                lock(av_tree)
                {
                    foreach (UUID id in removedEntries)
                    {
                        if(av_tree.ContainsKey(id))
                        {
                            Gtk.TreeIter iter = av_tree[id];
                            store.Remove(ref iter);
                            av_tree.Remove(id);
                        }
                    }
                }

                /*
                foreach (UUID id in newEntries)
                {
                    agent theagent = new agent();
                    Gtk.TreeIter iter;
                   
                    iter = store.AppendValues("", "Waiting...", "", id);

                    AsyncNameUpdate ud = new AsyncNameUpdate(id, false);

                    ud.onNameCallBack += delegate(string name, object[] values) { store.SetValue(iter, 1, name); };
                    ud.go();


                 
                    theagent.iter = iter;
                    av_tree.Add(id, theagent);
                }
                 
                 */

                calcdistance();
            });
        }

		new public void Dispose()
		{
			running=false;
			MainClass.client.Self.OnChat -= new OpenMetaverse.AgentManager.ChatCallback(onChat);
			MainClass.client.Network.OnLogin -= new OpenMetaverse.NetworkManager.LoginCallback(onLogin);
			MainClass.client.Self.OnTeleport -= new OpenMetaverse.AgentManager.TeleportCallback(onTeleport);
			AutoPilot.onAutoPilotFinished-=new AutoPilot.AutoPilotFinished(onAutoPilotFinished);
			
			//Finalize();
			//System.GC.SuppressFinalize(this);
		}
		
        bool kickrefresh()
        {

			if(running==false)
				return false;
			
			if (MainClass.client.Network.CurrentSim == null)
                return true;

            if (MainClass.client.Network.CurrentSim.ObjectsAvatars == null)
                return true;



            foreach (Simulator sim in MainClass.client.Network.Simulators)
            {
                sim.AvatarPositions.ForEach(delegate(KeyValuePair<UUID, Vector3> kvp)
                {
                    if (kvp.Key != MainClass.client.Self.AgentID)
                    {
                        if (!this.av_tree.ContainsKey(kvp.Key))
                        {
                            Gtk.TreeIter iter;
                            iter = store.AppendValues("", "Waiting...", "", kvp.Key);
                            av_tree.Add(kvp.Key, iter);

                            AsyncNameUpdate ud = new AsyncNameUpdate(kvp.Key, false);

                            ud.onNameCallBack += delegate(string name, object[] values) { store.SetValue(iter, 1, name); };
                            ud.go();

                        }
                    }
                });
            }

            calcdistance();
            return true;

        }

		int sort_Vector3(Gtk.TreeModel model,Gtk.TreeIter a,Gtk.TreeIter b)
		{
            
			string distAs=(string)store.GetValue(a,2);			
			string distBs=(string)store.GetValue(b,2);			
			float distA,distB;
			
			float.TryParse(distAs,out distA);
			float.TryParse(distBs,out distB);

            if (distAs == distBs)
                return 0;

            if (distAs == "NaN")
                return 1;

            if (distBs == "NaN")
                return -1;

			if(distA>distB)
				return 1;
			
			if(distA<distB)
				return -1;
			
			return 0;
		}
		
		void onTeleport(string Message, OpenMetaverse.TeleportStatus status,OpenMetaverse.TeleportFlags flags)
	    {
			if(status==OpenMetaverse.TeleportStatus.Finished)
			{

                if(MainClass.client.Network.CurrentSim.ID == lastsim)
                    return;

				Gtk.Application.Invoke(delegate
				{                   
                    store.Clear();
				});
				
                lock (av_tree)
                {
                   av_tree.Clear();
                }

                if (MainClass.client.Network.CurrentSim != null)
                 lastsim=MainClass.client.Network.CurrentSim.ID;
			}
	    }
		
		void onLogin(LoginStatus status,string message)
		{
			if(status==LoginStatus.ConnectingToSim)
			{					
				Gtk.Application.Invoke(delegate
				{
					   Console.WriteLine("Clearing all radar lists");
	                   store.Clear();
					   av_tree.Clear();
					   av_typing.Clear();
				});
			}		

            if (MainClass.client.Network.CurrentSim != null)
            lastsim = MainClass.client.Network.CurrentSim.ID;
		}

        void calcdistance()
        {
           Gtk.Application.Invoke(delegate
		   {
              // if (this.av_tree.ContainsKey(id))
               {

                   double dist;

                   Vector3 self_pos;
                   if (MainClass.client.Network.CurrentSim == null)
                       return;  //opensim protection
          
                   self_pos = MainClass.client.Self.RelativePosition;

                   uint regionX, regionY;
                   Utils.LongToUInts(MainClass.client.Network.CurrentSim.Handle, out regionX, out regionY);
                   self_pos.X = self_pos.X + regionX;
                   self_pos.Y = self_pos.Y + regionY;

                   Simulator target_sim = null;

                   MainClass.client.Network.Simulators.ForEach(delegate(Simulator sim)
                   {
                       //foreach (Simulator sim in MainClass.client.Network.Simulators)


                       sim.AvatarPositions.ForEach(delegate(KeyValuePair<UUID, Vector3> kvp)
                       {

                           if (kvp.Key != MainClass.client.Self.AgentID)
                           {
                               Utils.LongToUInts(sim.Handle, out regionX, out regionY);

                               try
                               {
                                   Vector3 target_pos = kvp.Value;

                                   target_pos.X = target_pos.X + regionX;
                                   target_pos.Y = target_pos.Y + regionY;
                                   dist = Vector3.Distance(target_pos, self_pos);

                                   if (av_tree.ContainsKey(kvp.Key))
                                   {
                                       store.SetValue(av_tree[kvp.Key], 2, MainClass.cleandistance(dist.ToString(), 1));
                                   }
                               }
                               catch
                               {
                                   Console.WriteLine("Exceptioned on store setvalue for radar");
                               }
                           }

                       });

                   });

               }
           });
        }
			
		void onChat(string message, ChatAudibleLevel audible, ChatType type, ChatSourceType sourcetype,string fromName, UUID id, UUID ownerid, Vector3 position)
		{
            
			Gtk.Application.Invoke(delegate
			{

                lock(av_tree)
                {
                    if (type == ChatType.StartTyping)
                    {
                        foreach (KeyValuePair<UUID, Gtk.TreeIter> kvp in av_tree)
                        {
                            if (kvp.Key == id)
                            {
                                store.SetValue(kvp.Value, 0, "*");
                                return;
                            }
                        }
                    }

                    if (type == ChatType.StopTyping)
                    {
                        foreach (KeyValuePair<UUID, Gtk.TreeIter> kvp in av_tree)
                        {
                            if (kvp.Key == id)
                            {
                                store.SetValue(kvp.Value, 0, " ");
                                return;
                            }
                        } 
                    }
                }
			});
             
		}

		protected virtual void OnButtonImClicked (object sender, System.EventArgs e)
		{
			//beter work out who we have selected
			Gtk.TreeModel mod;
			Gtk.TreeIter iter;			
			
			if(this.treeview_radar.Selection.GetSelected(out mod,out iter))			
			{
                UUID id=(UUID)mod.GetValue(iter,3);
                Avatar avatar = AutoPilot.findavatarinsims(id);
				if(avatar!=null)
				{
					MainClass.win.startIM(avatar.ID);
				}
			}		
		}

		protected virtual void OnButtonPayClicked (object sender, System.EventArgs e)
		{

			Gtk.TreeModel mod;
			Gtk.TreeIter iter;
			
			if(treeview_radar.Selection.GetSelected(out mod,out iter))			
			{
                UUID id=(UUID)mod.GetValue(iter,3);
                Avatar avatar = AutoPilot.findavatarinsims(id);
				if(avatar!=null)
				{
					PayWindow pay=new PayWindow(avatar.ID,0);
					pay.Show();
				}	
			}		
		}

		protected virtual void OnButtonProfileClicked (object sender, System.EventArgs e)
		{
			Gtk.TreeModel mod;
			Gtk.TreeIter iter;
			
			if(treeview_radar.Selection.GetSelected(out mod,out iter))			
			{
                UUID id = (UUID)mod.GetValue(iter, 3);
                Avatar avatar = AutoPilot.findavatarinsims(id);
                if (avatar != null)
				{
					ProfileVIew p = new ProfileVIew(avatar.ID);
					p.Show();
				}						
			}		
		}

		protected virtual void OnButton1Clicked (object sender, System.EventArgs e)
		{
			Gtk.TreeModel mod;
			Gtk.TreeIter iter;
			
			if(this.button1.Label=="STOP")
			{
				AutoPilot.stop();
				return;
			}
			
			if(treeview_radar.Selection.GetSelected(out mod,out iter))			
			{
				UUID id=(UUID)mod.GetValue(iter,3);		
				AutoPilot.set_target_avatar(id,true);
				this.button1.Label="STOP";
			}
		}

		protected virtual void OnButtonLookatClicked (object sender, System.EventArgs e)
		{
			Gtk.TreeModel mod;
			Gtk.TreeIter iter;
			
			if(treeview_radar.Selection.GetSelected(out mod,out iter))			
			{
				UUID id=(UUID)mod.GetValue(iter,3);
                Avatar avatar = AutoPilot.findavatarinsims(id);
				if(avatar!=null)
				{
					Vector3 pos;
					
					if(avatar.ParentID==0)
					{
						pos=avatar.Position;
						MainClass.client.Self.Movement.TurnToward(pos);					
					}					
					else
					{
						if(!MainClass.client.Network.CurrentSim.ObjectsPrimitives.ContainsKey(avatar.ParentID))
						{
							Console.WriteLine("AV is seated and i can't find the parent prim in dictionay");
						}
						else
						{
							Primitive parent = MainClass.client.Network.CurrentSim.ObjectsPrimitives[avatar.ParentID];
							pos = Vector3.Transform(avatar.Position, Matrix4.CreateFromQuaternion(parent.Rotation)) + parent.Position;
							MainClass.client.Self.Movement.TurnToward(pos);						
						}					
					}					
				}		
			}
		}

		void onAutoPilotFinished()
		{
			this.button1.Label="Follow";
		}
	}
}
