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
    public class agent 
    {
        public OpenMetaverse.Avatar avatar;
        public uint localid;
        public Gtk.TreeIter iter;
    }
    
	[System.ComponentModel.ToolboxItem(true)]	
    public partial class Radar : Gtk.Bin
	{
		
		Gtk.ListStore store;	
		private Dictionary<UUID, bool> av_typing = new Dictionary<UUID, bool>();
        private Dictionary<uint, agent> av_tree = new Dictionary<uint, agent>();
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
	
			MainClass.client.Objects.OnNewAvatar += new OpenMetaverse.ObjectManager.NewAvatarCallback(onNewAvatar);
			MainClass.client.Objects.OnObjectKilled += new OpenMetaverse.ObjectManager.KillObjectCallback(onKillObject);
			MainClass.client.Objects.OnObjectUpdated += new OpenMetaverse.ObjectManager.ObjectUpdatedCallback(onUpdate);

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
            lock (simulator.ObjectsAvatars.Dictionary)  
            {
                foreach (KeyValuePair<uint, Avatar> kvp in simulator.ObjectsAvatars.Dictionary)
                {
                    if (kvp.Value == null)
                        continue;

                    if (kvp.Value.ID == UUID.Zero)
                        continue;
                    lock (av_tree)
                    {
                        if (av_tree.ContainsKey(kvp.Value.LocalID))
                        {
                            store.Remove(ref av_tree[kvp.Value.LocalID].iter);
                            av_tree.Remove(kvp.Value.LocalID);
                        }
                    }
                }
            }
        }

		new public void Dispose()
		{
			running=false;
			MainClass.client.Objects.OnNewAvatar -= new OpenMetaverse.ObjectManager.NewAvatarCallback(onNewAvatar);
			MainClass.client.Objects.OnObjectKilled -= new OpenMetaverse.ObjectManager.KillObjectCallback(onKillObject);
			MainClass.client.Objects.OnObjectUpdated -= new OpenMetaverse.ObjectManager.ObjectUpdatedCallback(onUpdate);			
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

                lock (sim.ObjectsAvatars.Dictionary)
                {

                    foreach (KeyValuePair<uint, Avatar> kvp in sim.ObjectsAvatars.Dictionary)
                    {
                        //Seen this fire with some kind of null
                        if (kvp.Value == null)
                            continue;

                        if (kvp.Value.ID == UUID.Zero)
                            continue;

                        if (!this.av_tree.ContainsKey(kvp.Value.LocalID))
                        {
                            agent theagent = new agent();
                            theagent.avatar = kvp.Value;
                            Gtk.TreeIter iter;
                            iter = store.AppendValues("", kvp.Value.Name, "", kvp.Value.ID);
                            theagent.iter = iter;
                            av_tree.Add(kvp.Value.LocalID, theagent);
                            calcdistance(kvp.Value.LocalID);
                        }
                        else
                        {
                            calcdistance(kvp.Value.LocalID);
                        }
                    }
                }
            }

            
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
		
		void onUpdate(Simulator simulator, ObjectUpdate update,ulong regionHandle, ushort timeDilation)
		{
            lock (av_tree)
            {
                if (this.av_tree.ContainsKey(update.LocalID))
                {
                    calcdistance(update.LocalID); 
                }
            }
		}
		
		void onNewAvatar(Simulator simulator, Avatar avatar, ulong regionHandle, ushort timeDilation)
		{
			Gtk.Application.Invoke(delegate
			{  
                lock(av_tree)
			    {
				    lock(simulator.ObjectsAvatars.Dictionary)
				    {
                        if (!this.av_tree.ContainsKey(avatar.LocalID))
                        {
                            // The agent *might* still be present under an old localID and we
                            // missed the kill or have just handed off to a new sim
							    List <uint> removelist=new List <uint>();
                                foreach (KeyValuePair<uint, agent> av in av_tree)
                                {
                                    if (av.Value.avatar.ID == avatar.ID)
                                    {
                                        simulator.ObjectsAvatars.Dictionary.Remove(av.Key);
                                        removelist.Add(av.Key);
                                    }
						}

							foreach(uint id in removelist)
                            {
								store.Remove(ref av_tree[id].iter);	
	                           	av_tree.Remove(id); 
	                        }
							
                            agent theagent = new agent();
                            theagent.avatar = avatar;
                            Gtk.TreeIter iter;
					        
			                iter = store.AppendValues("", avatar.Name , "", avatar.ID);
	                        theagent.iter = iter;

	                        av_tree.Add(avatar.LocalID, theagent);
						    calcdistance(avatar.LocalID);
					    }
                    }
		        }
            });    
	    }
		
		void onKillObject(Simulator simulator, uint objectID)
		{
            if (this.av_tree.ContainsKey(objectID))
            {    
				Gtk.Application.Invoke(delegate
				{
					lock(av_tree)
					{
                            if (this.av_tree.ContainsKey(objectID)) //check again we are invoked so not constrained by the last test
                            {
						        store.Remove(ref av_tree[objectID].iter);
						        av_tree.Remove(objectID);
                            }
					}
				});
			}
		}

        void calcdistance(uint id)
        {
            if (this.av_tree.ContainsKey(id))
            {
                double dist;
              
				Vector3 self_pos;
				if(MainClass.client.Network.CurrentSim==null)
  				     return;  //opensim protection

                lock (MainClass.client.Network.CurrentSim.ObjectsAvatars.Dictionary)
                {
                    // Cope if *we* are sitting on someting
                    if (!MainClass.client.Network.CurrentSim.ObjectsAvatars.Dictionary.ContainsKey(MainClass.client.Self.LocalID))
                        return; //bollocks, we are not in the Dictionary yet

                    if (MainClass.client.Network.CurrentSim.ObjectsAvatars.Dictionary[MainClass.client.Self.LocalID].ParentID != 0)
                    {
                        Primitive parent = MainClass.client.Network.CurrentSim.ObjectsPrimitives.Dictionary[MainClass.client.Network.CurrentSim.ObjectsAvatars.Dictionary[MainClass.client.Self.LocalID].ParentID];
                        self_pos = Vector3.Transform(MainClass.client.Self.RelativePosition, Matrix4.CreateFromQuaternion(parent.Rotation)) + parent.Position;
                    }
                    else
                    {
                        self_pos = MainClass.client.Self.RelativePosition;
                    }
                }

                uint regionX, regionY;
                Utils.LongToUInts(MainClass.client.Network.CurrentSim.Handle, out regionX, out regionY);
                self_pos.X = self_pos.X + regionX;
                self_pos.Y = self_pos.Y + regionY;
                
				Simulator target_sim=null;
				foreach(Simulator sim in MainClass.client.Network.Simulators)
				{
						if(sim.ObjectsAvatars.Dictionary.ContainsKey(id))
						{
							target_sim=sim;
							break;						
                         }		
				}
					
				if(target_sim==null)
				{
                    Console.WriteLine("NO SIM FOR AV?");						
				    return;
                }

                Avatar av = target_sim.ObjectsAvatars.Dictionary[id];
                Vector3 av_pos;
                //Cope if *they* are sitting on something
                if (target_sim.ObjectsAvatars.Dictionary[id].ParentID != 0)
                {
                    if (!target_sim.ObjectsPrimitives.Dictionary.ContainsKey(target_sim.ObjectsAvatars.Dictionary[id].ParentID))
                    {
                        Console.WriteLine("Avatar is sitting but can't find parent prim");
                        return;
                    }

                    Primitive parent = target_sim.ObjectsPrimitives.Dictionary[av.ParentID];
                    av_pos = Vector3.Transform(av.Position, Matrix4.CreateFromQuaternion(parent.Rotation)) + parent.Position;
                }
                else
                {
                    av_pos = av.Position;
                }

                Utils.LongToUInts(target_sim.Handle, out regionX, out regionY);
                av_pos.X = av_pos.X + regionX;
                av_pos.Y = av_pos.Y + regionY;
                dist = Vector3.Distance(self_pos, av_pos);

				Gtk.Application.Invoke(delegate
				{
	                try
	                {
	                    if (av_tree.ContainsKey(id))
	                        store.SetValue(av_tree[id].iter, 2, MainClass.cleandistance(dist.ToString(), 1));
	                }
	                catch
	                {
	                    Console.WriteLine("Exceptioned on store setvalue for radar");

	                }
				});
			}
        }
			
		void onChat(string message, ChatAudibleLevel audible, ChatType type, ChatSourceType sourcetype,string fromName, UUID id, UUID ownerid, Vector3 position)
		{
			Gtk.Application.Invoke(delegate
			{

                lock(av_tree)
                {
                    if (type == ChatType.StartTyping)
                    {
                        foreach (KeyValuePair<uint, agent> kvp in av_tree)
                        {
                            if (kvp.Value.avatar.ID == id)
                            {
                                store.SetValue(kvp.Value.iter, 0, "*");
                                return;
                            }
                        }
                    }

                    if (type == ChatType.StopTyping)
                    {
                        foreach (KeyValuePair<uint, agent> kvp in av_tree)
                        {
                            if (kvp.Value.avatar.ID == id)
                            {
                                store.SetValue(kvp.Value.iter, 0, " ");
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
						if(!MainClass.client.Network.CurrentSim.ObjectsPrimitives.Dictionary.ContainsKey(avatar.ParentID))
						{
							Console.WriteLine("AV is seated and i can't find the parent prim in dictionay");
						}
						else
						{
							Primitive parent = MainClass.client.Network.CurrentSim.ObjectsPrimitives.Dictionary[avatar.ParentID];
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
