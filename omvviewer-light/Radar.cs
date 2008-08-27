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
using libsecondlife;

namespace omvviewerlight
{

    public class agent 
    {
        public libsecondlife.Avatar avatar;
        public uint localid;
        public Gtk.TreeIter iter;
    }
    
    
    public partial class Radar : Gtk.Bin
	{
		
		Gtk.ListStore store;	
		private Dictionary<uint, Avatar> avs = new Dictionary<uint, Avatar>();
		private Dictionary<LLUUID, bool> av_typing = new Dictionary<LLUUID, bool>();
        private Dictionary<uint, agent> av_tree = new Dictionary<uint, agent>();	


		public Radar()
		{
       
			store= new Gtk.ListStore (typeof(string),typeof(string),typeof(string),typeof(uint));
			this.Build();
			treeview_radar.AppendColumn("",new Gtk.CellRendererText(),"text",0);
			treeview_radar.AppendColumn("Name",new Gtk.CellRendererText(),"text",1);
			treeview_radar.AppendColumn("Dist.",new Gtk.CellRendererText(),"text",2);
			treeview_radar.Model=store;
	
			MainClass.client.Objects.OnNewAvatar += new libsecondlife.ObjectManager.NewAvatarCallback(onNewAvatar);
			MainClass.client.Objects.OnObjectKilled += new libsecondlife.ObjectManager.KillObjectCallback(onKillObject);
			MainClass.client.Objects.OnObjectUpdated += new libsecondlife.ObjectManager.ObjectUpdatedCallback(onUpdate);
		
	//		MainClass.client.Self.OnChat += new libsecondlife.AgentManager.ChatCallback(onChat);
			MainClass.client.Network.OnLogin += new libsecondlife.NetworkManager.LoginCallback(onLogin);
			
	
			MainClass.client.Self.OnTeleport += new libsecondlife.AgentManager.TeleportCallback(onTeleport);

			this.store.SetSortFunc(2,sort_llvector3);	
            store.SetSortColumnId(2,Gtk.SortType.Ascending);

		}
		
		int sort_llvector3(Gtk.TreeModel model,Gtk.TreeIter a,Gtk.TreeIter b)
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
		
		void onTeleport(string Message, libsecondlife.AgentManager.TeleportStatus status,libsecondlife.AgentManager.TeleportFlags flags)
	    {
			if(status==libsecondlife.AgentManager.TeleportStatus.Finished)
			{
				Gtk.Application.Invoke(delegate {
					lock (store)
                    {
					    store.Clear();
                        av_tree.Clear();
                    }
                });
			}
	    }
		
		void onLogin(LoginStatus status,string message)
		{
			if(status==LoginStatus.Success)
				Gtk.Application.Invoke(delegate {										
				   lock(store)
                   {
                        store.Clear();
                        av_tree.Clear();
                   }
              });			
		}
		
		void onUpdate(Simulator simulator, ObjectUpdate update,ulong regionHandle, ushort timeDilation)
		{
            if(this.av_tree.ContainsKey(update.LocalID))
            {
                Gtk.Application.Invoke(delegate {
                    calcdistance(update.LocalID);
                });
            }
		}
		
		void onNewAvatar(Simulator simulator, Avatar avatar, ulong regionHandle, ushort timeDilation)
		{
            Avatar av;

              
                    Gtk.Application.Invoke(delegate
                    {
                        if (!this.av_tree.ContainsKey(avatar.LocalID))
                        {
                        agent theagent = new agent();
                        theagent.avatar = avatar;
                        Gtk.TreeIter iter;
                        iter = store.AppendValues("", avatar.Name, "", avatar.LocalID);
                        theagent.iter = iter;
                        av_tree.Add(avatar.LocalID, theagent);
                        calcdistance(avatar.LocalID);
                        }
                    });
                
            
		}
		
		void onKillObject(Simulator simulator, uint objectID)
		{
           if(this.av_tree.ContainsKey(objectID))
            {
                Gtk.Application.Invoke(delegate {	
                    store.Remove(ref av_tree[objectID].iter);
                    av_tree.Remove(objectID);
                });
            }
		}

        void calcdistance(uint id)
        {
            if (this.av_tree.ContainsKey(id))
            {
                LLVector3 pos;
                pos = MainClass.client.Self.RelativePosition - (LLVector3)av_tree[id].avatar.Position;
                double dist;
                dist = Math.Sqrt(pos.X * pos.X + pos.Y + pos.Y + pos.Z + pos.Z);
                store.SetValue(av_tree[id].iter, 2, MainClass.cleandistance(dist.ToString(), 1));
           
            }
        }
/*		
		bool myfunc(Gtk.TreeModel mod, Gtk.TreePath path, Gtk.TreeIter iter)
		{
			uint key=(uint)store.GetValue(iter,3);
    
            lock(avs)
            {
			    if(!avs.ContainsKey(key))
			    {      
                        store.Remove(ref iter);
                }
			    else
			    {
				    LLVector3 pos;
				    pos=MainClass.client.Self.RelativePosition-(LLVector3)avs[key].Position;
				    double dist;
				    dist=Math.Sqrt(pos.X*pos.X+pos.Y+pos.Y+pos.Z+pos.Z);
				    store.SetValue(iter,2,MainClass.cleandistance(dist.ToString(),1));
    		
                    lock(av_typing)
                    {
				        if(av_typing.ContainsKey(avs[key].ID))
				        {
					        store.SetValue(iter,0,"*");
				        }
				        else
				        {
					        store.SetValue(iter,0," ");
				        }
                    }
			    }
            }

			return true;
		}
*/
/*		
		void onChat(string message, ChatAudibleLevel audible, ChatType type, ChatSourceType sourcetype,string fromName, LLUUID id, LLUUID ownerid, LLVector3 position)
		{
                lock(av_typing)
                {
                    if (type == ChatType.StartTyping)
                    {

                        if (!av_typing.ContainsKey(id))
                            av_typing.Add(id, true);
                    }

                    if (type == ChatType.StopTyping)
                    {
                        if (av_typing.ContainsKey(id))
                            av_typing.Remove(id);
                    }
                }	
		}
*/
		protected virtual void OnButtonImClicked (object sender, System.EventArgs e)
		{
			//beter work out who we have selected
			Gtk.TreeModel mod;
			Gtk.TreeIter iter;			
			
			if(this.treeview_radar.Selection.GetSelected(out mod,out iter))			
			{
				uint localid=(uint)mod.GetValue(iter,3);
				Avatar avatar;
				if(avs.TryGetValue(localid,out avatar))
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
				uint localid=(uint)mod.GetValue(iter,3);
				Avatar avatar;
				if(avs.TryGetValue(localid,out avatar))
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
				uint localid=(uint)mod.GetValue(iter,3);
				Avatar avatar;
				if(avs.TryGetValue(localid,out avatar))
				{
					ProfileVIew p = new ProfileVIew(avatar.ID);
					p.Show();
				}
						
			}		

		}
		
	}
}
