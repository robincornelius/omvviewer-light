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
      public partial class Radar : Gtk.Bin
	{
		
		Gtk.ListStore store;
        List<uint> localids = new List<uint>();
        List<Gtk.TreeIter> removelist = new List<Gtk.TreeIter>();
        UUID lastsim = new UUID();
		const float DISTANCE_BUFFER = 3f;

		bool running=true;
		~Radar()
		{
			Console.WriteLine("Radar Cleaned up");
		}		
		
		
		public Radar()
		{
            store = new Gtk.ListStore(typeof(string), typeof(string), typeof(string), typeof(OpenMetaverse.Avatar));
			this.Build();
			Gtk.TreeViewColumn tvc;
            treeview_radar.AppendColumn("",new Gtk.CellRendererText(),"text",0);

			//tvc=treeview_radar.AppendColumn("Name",new Gtk.CellRendererText(),"text",1);
			//tvc.Sizing=Gtk.TreeViewColumnSizing.Autosize;

            MyTreeViewColumn mycol;
            mycol = new MyTreeViewColumn("Name", new Gtk.CellRendererText(), "text", 1, false);
            mycol.Sizing = Gtk.TreeViewColumnSizing.Autosize;
            treeview_radar.AppendColumn(mycol);

            mycol = new MyTreeViewColumn("Dist.", new Gtk.CellRendererText(), "text", 2, false);
            mycol.Sizing = Gtk.TreeViewColumnSizing.Autosize;
            treeview_radar.AppendColumn(mycol);
			
			//treeview_radar.AppendColumn("Dist.",new Gtk.CellRendererText(),"text",2);
			treeview_radar.Model=store;
            treeview_radar.HeadersClickable = true;
	
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
            Gtk.Timeout.Add(2500, kickrefresh);
		}

        void Network_OnSimDisconnected(Simulator simulator, NetworkManager.DisconnectType reason)
        {
            lock (simulator.ObjectsAvatars.Dictionary)
            {
                lock (store)
                {
                    store.Foreach(delegate(Gtk.TreeModel mod, Gtk.TreePath path, Gtk.TreeIter iter)
                    {
                        OpenMetaverse.Avatar avatar = (OpenMetaverse.Avatar)mod.GetValue(iter, 3);
                        if (simulator.ObjectsAvatars.Dictionary.ContainsKey(avatar.LocalID))
                            removelist.Add(iter);

                        return false;
 
                    });
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
               
           lock (store)
           {
               store.Foreach(delegate(Gtk.TreeModel mod, Gtk.TreePath path, Gtk.TreeIter iter)
               {
                   calcdistance(iter);
                   return false;
               });

               foreach (Gtk.TreeIter iter in removelist)
               {
                   Gtk.TreeIter refiter = iter;
                   store.Remove(ref refiter);
               }

               removelist.Clear();
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
                       localids.Clear();
				});
			}		

            if (MainClass.client.Network.CurrentSim != null)
            lastsim = MainClass.client.Network.CurrentSim.ID;
		}
		
		void onUpdate(Simulator simulator, ObjectUpdate update,ulong regionHandle, ushort timeDilation)
		{
            return;
            if(update.Avatar)
            if (localids.Contains(update.LocalID))
            {
                Gtk.Application.Invoke(delegate
                {
                    lock (store)
                    {
                        store.Foreach(delegate(Gtk.TreeModel mod, Gtk.TreePath path, Gtk.TreeIter iter)
                        {
                            OpenMetaverse.Avatar avatar = (OpenMetaverse.Avatar)mod.GetValue(iter, 3);
                            if (avatar.LocalID == update.LocalID && avatar.RegionHandle == simulator.Handle)
                            {
                                calcdistance(iter);
                                return true;
                            }
                            return false;
                        });

                        foreach (Gtk.TreeIter iter in removelist)
                        {
                            Gtk.TreeIter refiter=iter;
                            store.Remove(ref refiter);
                        }
                        removelist.Clear();
                    }
                });
            }
		}
		
		void onNewAvatar(Simulator simulator, Avatar avatar, ulong regionHandle, ushort timeDilation)
		{

            if(!localids.Contains(avatar.LocalID))
            {      
                localids.Add(avatar.LocalID);
                Logger.Log("*** New Avatar " + avatar.Name + " from sim " + simulator.Name + " region handle " + regionHandle.ToString() + " local id " + avatar.LocalID.ToString() + " avatar position " + avatar.Position.ToString(), Helpers.LogLevel.Warning);
                lock (store)
                {
                    store.Foreach(delegate(Gtk.TreeModel mod, Gtk.TreePath path, Gtk.TreeIter iter)
                    {
                        OpenMetaverse.Avatar avatarstore = (OpenMetaverse.Avatar)mod.GetValue(iter, 3);
                        if (avatar.Name == avatarstore.Name)
                        {
                            removelist.Add(iter);
                            localids.Remove(avatarstore.LocalID);
                            return true;
                        }
                        return false;
                    });
                }
               
                 
                
			    Gtk.Application.Invoke(delegate
			    {  
                    Gtk.TreeIter iter=store.AppendValues("", avatar.Name + " ("+simulator.Name+")", "", avatar);
	                calcdistance(iter);
                });    
            }
	    }
		
		void onKillObject(Simulator simulator, uint objectID)
		{
            if(localids.Contains(objectID))
            {
                Gtk.Application.Invoke(delegate
                {
                    lock (store)
                    {
                        store.Foreach(delegate(Gtk.TreeModel mod, Gtk.TreePath path, Gtk.TreeIter iter)
                        {

                            OpenMetaverse.Avatar avatar = (OpenMetaverse.Avatar)mod.GetValue(iter, 3);
                            if (avatar.LocalID == objectID)
                            {
                                  Logger.Log("*** Kill Avatar " + avatar.Name + " from sim " + simulator.Name + " region handle " + avatar.RegionHandle.ToString(), Helpers.LogLevel.Warning);
                                  store.Remove(ref iter);
                                  localids.Remove(objectID);
                                  return true;
                            }
                            return false;
                        });
                    }
              });
          }
		}

        void calcdistance(Gtk.TreeIter iter)
        {

                double dist;
                Vector3 self_pos;
                Avatar avatar = (Avatar)store.GetValue(iter, 3);
                uint id = avatar.LocalID;

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
                    localids.Remove(id);
                    removelist.Add(iter);	
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

	             store.SetValue(iter, 2, MainClass.cleandistance(dist.ToString(), 1));	
        }
			
		void onChat(string message, ChatAudibleLevel audible, ChatType type, ChatSourceType sourcetype,string fromName, UUID id, UUID ownerid, Vector3 position)
		{ 
			Gtk.Application.Invoke(delegate
			{
                    if (type == ChatType.StartTyping)
                    {
                        lock (store)
                        {
                            store.Foreach(delegate(Gtk.TreeModel mod, Gtk.TreePath path, Gtk.TreeIter iter)
                            {
                                Avatar avatar = (Avatar)store.GetValue(iter, 3);
                                if (avatar.ID == id)
                                {
                                    store.SetValue(iter, 0, "*");
                                    return true;
                                }
                                return false;
                            });
                        }
                    }

                    if (type == ChatType.StopTyping)
                    {
                        lock (store)
                        {
                            store.Foreach(delegate(Gtk.TreeModel mod, Gtk.TreePath path, Gtk.TreeIter iter)
                            {
                                Avatar avatar = (Avatar)store.GetValue(iter, 3);
                                if (avatar.ID == id)
                                {
                                    store.SetValue(iter, 0, " ");
                                    return true;
                                }
                                return false;
                            });
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
                OpenMetaverse.Avatar avatar = (OpenMetaverse.Avatar)(mod.GetValue(iter, 3));
			    MainClass.win.startIM(avatar.ID);
				
			}		
		}

		protected virtual void OnButtonPayClicked (object sender, System.EventArgs e)
		{

			Gtk.TreeModel mod;
			Gtk.TreeIter iter;
			
			if(treeview_radar.Selection.GetSelected(out mod,out iter))			
			{
                OpenMetaverse.Avatar avatar = (OpenMetaverse.Avatar)mod.GetValue(iter, 3);
				PayWindow pay=new PayWindow(avatar.ID,0);
				pay.Show();
				
			}		
		}

		protected virtual void OnButtonProfileClicked (object sender, System.EventArgs e)
		{
			Gtk.TreeModel mod;
			Gtk.TreeIter iter;
			
			if(treeview_radar.Selection.GetSelected(out mod,out iter))			
			{
                Avatar avatar = (Avatar)mod.GetValue(iter, 3);
             
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
                OpenMetaverse.Avatar avatar = (OpenMetaverse.Avatar)mod.GetValue(iter, 3);		
				AutoPilot.set_target_avatar(avatar.ID,true);
				this.button1.Label="STOP";
			}
		}

		protected virtual void OnButtonLookatClicked (object sender, System.EventArgs e)
		{
			Gtk.TreeModel mod;
			Gtk.TreeIter iter;
			
			if(treeview_radar.Selection.GetSelected(out mod,out iter))			
			{
                OpenMetaverse.Avatar avatar = (OpenMetaverse.Avatar)mod.GetValue(iter, 3);
     
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
