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

		}
		
		int sort_Vector3(Gtk.TreeModel model,Gtk.TreeIter a,Gtk.TreeIter b)
		{
            
			string distAs=(string)store.GetValue(a,2);			
			string distBs=(string)store.GetValue(b,2);			
			float distA,distB;
			
			float.TryParse(distAs,out distA);
			float.TryParse(distBs,out distB);

            //Console.Write("Testing " + distA.ToString() + " vs " + distB.ToString() + "\n");

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

				Gtk.Application.Invoke(delegate {
                    lock (store)
                    {
                        store.Clear();
                    }
                    lock (av_tree)
                    {
                       av_tree.Clear();
                    }
                });

                if (MainClass.client.Network.CurrentSim != null)
                 lastsim=MainClass.client.Network.CurrentSim.ID;
			}
	    }
		
		void onLogin(LoginStatus status,string message)
		{
			if(status==LoginStatus.Success)
				Gtk.Application.Invoke(delegate {										
				   lock(store)
                   {
                        store.Clear();
                   }
                   lock (av_tree)
                   {
                       av_tree.Clear();
                   }
              });

            if (MainClass.client.Network.CurrentSim != null)
            lastsim = MainClass.client.Network.CurrentSim.ID;
		}
		
		void onUpdate(Simulator simulator, ObjectUpdate update,ulong regionHandle, ushort timeDilation)
		{
            Gtk.Application.Invoke(delegate {
                if(this.av_tree.ContainsKey(update.LocalID))
                {
                    lock (av_tree)
                    {
                        Console.WriteLine(update.LocalID.ToString() + " is at " + update.Position.ToString()+" :: "+update.Avatar.ToString());
                        calcdistance(update.LocalID);
                    }
                }
            });
		}
		
		void onNewAvatar(Simulator simulator, Avatar avatar, ulong regionHandle, ushort timeDilation)
		{
                     Gtk.Application.Invoke(delegate
                    {
                        if (!this.av_tree.ContainsKey(avatar.LocalID))
                        {
                            // The agent *might* still be present under an old localID and we
                            // missed the kill
                            lock (av_tree)
                            {
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
                            }

                            agent theagent = new agent();
                            theagent.avatar = avatar;
                            Gtk.TreeIter iter;
                            lock (store)
                            {
                                iter = store.AppendValues("", avatar.Name, "", avatar.LocalID);
                                theagent.iter = iter;
                            }

                            lock (av_tree)
                            {
                                av_tree.Add(avatar.LocalID, theagent);
                                calcdistance(avatar.LocalID);
                            }
                       }
                    });
                
            
		}
		
		void onKillObject(Simulator simulator, uint objectID)
		{
           //  Gtk.Application.Invoke(delegate {
                 lock (av_tree)
                 {
                     if (this.av_tree.ContainsKey(objectID))
                     {
                         lock (store)
                         {
                             store.Remove(ref av_tree[objectID].iter);
                         }
                         av_tree.Remove(objectID);
                     }
                 }
            // });
		}

        void calcdistance(uint id)
        {
            if (this.av_tree.ContainsKey(id))
            {
                double dist;
                Avatar av = MainClass.client.Network.CurrentSim.ObjectsAvatars.Dictionary[id];
               
                if (MainClass.client.Network.CurrentSim.ObjectsAvatars.Dictionary[id].ParentID != 0)
                {
                    Primitive parent = MainClass.client.Network.CurrentSim.ObjectsPrimitives.Dictionary[av.ParentID];
                    Vector3 av_pos = Vector3.Transform(av.Position, Matrix4.CreateFromQuaternion(parent.Rotation)) + parent.Position;
                    dist = Vector3.Distance(MainClass.client.Self.RelativePosition, av_pos);
                }
                else
                {
                    dist = Vector3.Distance(MainClass.client.Self.RelativePosition, av.Position);
                }

                store.SetValue(av_tree[id].iter, 2, MainClass.cleandistance(dist.ToString(), 1));
            }
        }
			
		void onChat(string message, ChatAudibleLevel audible, ChatType type, ChatSourceType sourcetype,string fromName, UUID id, UUID ownerid, Vector3 position)
		{
                lock(av_typing)
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
		
	}
}
