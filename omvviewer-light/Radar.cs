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
    
    public partial class Radar : Gtk.Bin
	{
		
		Gtk.ListStore store;	
		private Dictionary<UUID, bool> av_typing = new Dictionary<UUID, bool>();
        private Dictionary<uint, agent> av_tree = new Dictionary<uint, agent>();
        UUID lastsim = new UUID();
		const float DISTANCE_BUFFER = 3f;
        uint targetLocalID = 0;
		bool Active;
		bool piloton=false;

		public Radar()
		{      
			store= new Gtk.ListStore (typeof(string),typeof(string),typeof(string),typeof(uint));
			this.Build();
			treeview_radar.AppendColumn("",new Gtk.CellRendererText(),"text",0);
			treeview_radar.AppendColumn("Name",new Gtk.CellRendererText(),"text",1);
			treeview_radar.AppendColumn("Dist.",new Gtk.CellRendererText(),"text",2);
			treeview_radar.Model=store;
	
			MainClass.client.Objects.OnNewAvatar += new OpenMetaverse.ObjectManager.NewAvatarCallback(onNewAvatar);
			MainClass.client.Objects.OnObjectKilled += new OpenMetaverse.ObjectManager.KillObjectCallback(onKillObject);
			MainClass.client.Objects.OnObjectUpdated += new OpenMetaverse.ObjectManager.ObjectUpdatedCallback(onUpdate);
		
			MainClass.client.Self.OnChat += new OpenMetaverse.AgentManager.ChatCallback(onChat);
			MainClass.client.Network.OnLogin += new OpenMetaverse.NetworkManager.LoginCallback(onLogin);
	
			MainClass.client.Self.OnTeleport += new OpenMetaverse.AgentManager.TeleportCallback(onTeleport);

			this.store.SetSortFunc(2,sort_Vector3);	
            store.SetSortColumnId(2,Gtk.SortType.Ascending);
            Gtk.Timeout.Add(10000, kickrefresh);
		}

        bool kickrefresh()
        {

			if (MainClass.client.Network.CurrentSim == null)
                return true;

            if (MainClass.client.Network.CurrentSim.ObjectsAvatars == null)
                return true;
			
			lock(MainClass.client.Network.CurrentSim.ObjectsAvatars.Dictionary)
			{
                lock(av_tree)
                {
                    foreach (KeyValuePair<uint, Avatar> kvp in MainClass.client.Network.CurrentSim.ObjectsAvatars.Dictionary)
                    {
                        //Seen this fire with some kind of null
                        if (kvp.Value == null)
                            continue;

                        if (kvp.Value.ID == UUID.Zero)
                            continue;

                        if (kvp.Value.LocalID == null)
                            continue;

                        if (!this.av_tree.ContainsKey(kvp.Value.LocalID))
                        {
                            agent theagent = new agent();
                            theagent.avatar = kvp.Value;
                            Gtk.TreeIter iter;
                            lock (av_tree)
                            {
								iter = store.AppendValues("", kvp.Value.Name, "", kvp.Value.LocalID);
                                theagent.iter = iter;

                                av_tree.Add(kvp.Value.LocalID, theagent);
                            }
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
		
		void onTeleport(string Message, OpenMetaverse.AgentManager.TeleportStatus status,OpenMetaverse.AgentManager.TeleportFlags flags)
	    {
			if(status==OpenMetaverse.AgentManager.TeleportStatus.Finished)
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
			if(status==LoginStatus.Success)
										
				Gtk.Application.Invoke(delegate
				{
				   
                   store.Clear();
				});
			
                   lock (av_tree)
                   {
                       av_tree.Clear();
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
                        if (!this.av_tree.ContainsKey(avatar.LocalID))
                        {
                            // The agent *might* still be present under an old localID and we
                            // missed the kill
                           
                                uint localid=0;
                                foreach (KeyValuePair<uint, agent> av in av_tree)
                                {
                                    if (av.Value.avatar.ID == avatar.ID)
                                    {
                                        //All ready in tree kill that old definition
                                        localid=av.Key;
                                        break;
                                        
                                    }
                                }
                                if(localid!=0)
                                    av_tree.Remove(localid);
                            

                            agent theagent = new agent();
                            theagent.avatar = avatar;
                            Gtk.TreeIter iter;
					        
			                        iter = store.AppendValues("", avatar.Name, "", avatar.LocalID);
	                                theagent.iter = iter;

	                                av_tree.Add(avatar.LocalID, theagent);
	        			calcdistance(avatar.LocalID);                            
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
						store.Remove(ref av_tree[objectID].iter);
						av_tree.Remove(objectID);
					}
				});
			}
		}

        void calcdistance(uint id)
        {
            if (this.av_tree.ContainsKey(id))
            {
                double dist;
                
                if (!MainClass.client.Network.CurrentSim.ObjectsAvatars.Dictionary.ContainsKey(id))
                    return;

                Avatar av = MainClass.client.Network.CurrentSim.ObjectsAvatars.Dictionary[id];
               
                if (MainClass.client.Network.CurrentSim.ObjectsAvatars.Dictionary[id].ParentID != 0)
                {
                    if (!MainClass.client.Network.CurrentSim.ObjectsPrimitives.Dictionary.ContainsKey(MainClass.client.Network.CurrentSim.ObjectsAvatars.Dictionary[id].ParentID))
                        return;

                    Primitive parent = MainClass.client.Network.CurrentSim.ObjectsPrimitives.Dictionary[av.ParentID];
                    Vector3 av_pos = Vector3.Transform(av.Position, Matrix4.CreateFromQuaternion(parent.Rotation)) + parent.Position;
                    dist = Vector3.Distance(MainClass.client.Self.RelativePosition, av_pos);
                }
                else
                {
                    dist = Vector3.Distance(MainClass.client.Self.RelativePosition, av.Position);
                }

				Gtk.Application.Invoke(delegate
				{
					lock (av_tree)
	                {
	                    try
	                    {
	                        if (av_tree.ContainsKey(id))
	                            store.SetValue(av_tree[id].iter, 2, MainClass.cleandistance(dist.ToString(), 1));
	                    }
	                    catch (Exception e)
	                    {
	                        Console.WriteLine("Exceptioned on store setvalue for radar");

	                    }
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
				uint localid=(uint)mod.GetValue(iter,3);
				agent avatar;
				if(av_tree.TryGetValue(localid,out avatar))
				{
					MainClass.win.startIM(avatar.avatar.ID);
				}
			}		
		}

		protected virtual void OnButtonPayClicked (object sender, System.EventArgs e)
		{

			Gtk.TreeModel mod;
			Gtk.TreeIter iter;
			
			if(treeview_radar.Selection.GetSelected(out mod,out iter))			
			{
				uint localid=(uint)mod.GetValue(iter,3);
				agent avatar;
				if(av_tree.TryGetValue(localid,out avatar))
				{
					PayWindow pay=new PayWindow(avatar.avatar.ID,0);
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
				uint localid=(uint)mod.GetValue(iter,3);
				agent avatar;
				if(av_tree.TryGetValue(localid,out avatar))
				{
					ProfileVIew p = new ProfileVIew(avatar.avatar.ID);
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
				Active=false;
				this.button1.Label="Follow";
				return;
			}
			
			if(treeview_radar.Selection.GetSelected(out mod,out iter))			
			{
				uint localid=(uint)mod.GetValue(iter,3);
				
				this.targetLocalID=localid;
			
				Active=true;
				Gtk.Timeout.Add(1000,Think);
				this.button1.Label="STOP";
			}
		}

		protected virtual void OnButtonLookatClicked (object sender, System.EventArgs e)
		{
			Gtk.TreeModel mod;
			Gtk.TreeIter iter;
			
			if(treeview_radar.Selection.GetSelected(out mod,out iter))			
			{
				uint localid=(uint)mod.GetValue(iter,3);
				agent avatar;
				if(av_tree.TryGetValue(localid,out avatar))
				{
					Vector3 pos;
					
					if(avatar.avatar.ParentID==0)
					{
						pos=avatar.avatar.Position;
						MainClass.client.Self.Movement.TurnToward(pos);					
					}					
					else
					{
						if(!MainClass.client.Network.CurrentSim.ObjectsPrimitives.Dictionary.ContainsKey(avatar.avatar.ParentID))
						{
							Console.WriteLine("AV is seated and i can't find the parent prim in dictionay");
						}
						else
						{
							Primitive parent = MainClass.client.Network.CurrentSim.ObjectsPrimitives.Dictionary[avatar.avatar.ParentID];
							pos = Vector3.Transform(avatar.avatar.Position, Matrix4.CreateFromQuaternion(parent.Rotation)) + parent.Position;
							MainClass.client.Self.Movement.TurnToward(pos);						
						}					
					}					
				}		
			}
		}
		
		bool Think()
		{
            if (Active)
            {
				
                lock (MainClass.client.Network.Simulators)
                {
                    for (int i = 0; i < MainClass.client.Network.Simulators.Count; i++)
                    {
                        Avatar targetAv;

                        if (MainClass.client.Network.Simulators[i].ObjectsAvatars.TryGetValue(targetLocalID, out targetAv))
                        {
                            float distance = 0.0f;

                            if (MainClass.client.Network.Simulators[i] == MainClass.client.Network.CurrentSim)
                            {
                                distance = Vector3.Distance(targetAv.Position, MainClass.client.Self.SimPosition);
                            }

                            if (distance > 2.5)
                            {
                                uint regionX, regionY;
                                Utils.LongToUInts(MainClass.client.Network.Simulators[i].Handle, out regionX, out regionY);

                                double xTarget = (double)targetAv.Position.X + (double)regionX;
                                double yTarget = (double)targetAv.Position.Y + (double)regionY;
                                double zTarget = targetAv.Position.Z;

                                Logger.DebugLog(String.Format("[Autopilot] {0} meters away from the target, starting autopilot to <{1},{2},{3}>",
                                    distance, xTarget, yTarget, zTarget), MainClass.client);

								piloton=true;
									
								Vector3 pos;
								pos.X=(float)xTarget;
								pos.Y=(float)yTarget;
								pos.Z=(float)zTarget;
									
								MainClass.client.Self.Movement.TurnToward(pos);			
                                MainClass.client.Self.AutoPilot(xTarget, yTarget, zTarget);
							}
                            else
                            {
								Logger.DebugLog("Stopping autopilot");
                            }
                        }
                    }
                }
				return true;
            }
			else
			{
				MainClass.client.Self.AutoPilotCancel();
				return false;
			}
		}
		
	}
}
