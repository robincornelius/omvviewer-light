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
	public partial class Radar : Gtk.Bin
	{
		
		Gtk.ListStore store;	
		private Dictionary<uint, Avatar> avs = new Dictionary<uint, Avatar>();
		private Dictionary<LLUUID, bool> av_typing = new Dictionary<LLUUID, bool>();
			
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
		
			MainClass.client.Self.OnChat += new libsecondlife.AgentManager.ChatCallback(onChat);
			MainClass.client.Network.OnLogin += new libsecondlife.NetworkManager.LoginCallback(onLogin);
			store.SetSortColumnId(2,Gtk.SortType.Ascending);
	
			MainClass.client.Self.OnTeleport += new libsecondlife.AgentManager.TeleportCallback(onTeleport);

			this.store.SetSortFunc(2,sort_llvector3);
            GLib.Timeout.Add(1000, onTimeout);
		
		}

        bool onTimeout()
        {
            lock(store)
            {
                store.Foreach(myfunc);
            }

            return true;
        }
			
		int sort_llvector3(Gtk.TreeModel model,Gtk.TreeIter a,Gtk.TreeIter b)
		{

			string distAs=(string)store.GetValue(a,2);			
			string distBs=(string)store.GetValue(b,2);			
			float distA,distB;
			
			float.TryParse(distAs,out distA);
			float.TryParse(distBs,out distB);

			if(distA>distB)
				return 1;
			
			if(distA<distB)
				return 0;
			
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
                   }
              });			
		}
		
		void onUpdate(Simulator simulator, ObjectUpdate update,ulong regionHandle, ushort timeDilation)
		{
		//	if(!avs.ContainsKey(update.LocalID))
		//	{
		//		Gtk.Application.Invoke(delegate {										
		//			store.Foreach(myfunc);
		//		});
		//	}
		}
		
		void onNewAvatar(Simulator simulator, Avatar avatar, ulong regionHandle, ushort timeDilation)
		{
            Avatar av;

            lock(avs)
            {
                if(avs.ContainsKey(avatar.LocalID))
			    {
                    return;
                }
                avs.Add(avatar.LocalID,avatar);
                av=avs[avatar.LocalID];
            }
 
            Gtk.Application.Invoke(delegate 
            {		
			    if(!avs.ContainsKey(avatar.LocalID))
			    {
				    LLVector3 pos;
			    	pos=MainClass.client.Self.RelativePosition-avatar.Position;
				    double dist;
				    dist=Math.Sqrt(pos.X*pos.X+pos.Y+pos.Y+pos.Z+pos.Z);						
				    store.AppendValues("",av.Name,MainClass.cleandistance(dist.ToString(),1),avatar.LocalID);
			    }
            });
		}
		
		void onKillObject(Simulator simulator, uint objectID)
		{
            lock (avs)
            {
                if(avs.ContainsKey(objectID))
			        avs.Remove(objectID);
            }
		}
		
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
