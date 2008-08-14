// ObjectsLayout.cs created with MonoDevelop
// User: robin at 13:32 14/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;
using libsecondlife;

namespace omvviewerlight
{
	
	public partial class ObjectsLayout : Gtk.Bin
	{
	
		Gtk.ListStore store;	
	    //List<uint> avs = new List<uint>();
		Dictionary<uint,Primitive> objects= new Dictionary<uint,Primitive>(); 
			
		public ObjectsLayout()
		{
			this.Build();
			store= new Gtk.ListStore (typeof(string),typeof(string),typeof(uint));
			treeview1.AppendColumn("Name",new Gtk.CellRendererText(),"text",0);
			treeview1.AppendColumn("Desc.",new Gtk.CellRendererText(),"text",1);
			treeview1.AppendColumn("ID",new Gtk.CellRendererText(),"text",2);
			treeview1.Model=store;

			MainClass.client.Objects.OnObjectKilled += new libsecondlife.ObjectManager.KillObjectCallback(onKillObject);
			MainClass.client.Objects.OnObjectUpdated += new libsecondlife.ObjectManager.ObjectUpdatedCallback(onUpdate);
			MainClass.client.Objects.OnNewPrim += new libsecondlife.ObjectManager.NewPrimCallback(onNewPrim);
			
			
			
		}
		
		
		void onNewPrim(Simulator sim, Primitive prim,ulong RegionHandle,ushort timedilation)
	    {
			if(prim.ParentID!=0)
				return;
			
		Gtk.Application.Invoke(delegate {										
		
			if(!objects.ContainsKey(prim.LocalID))
			{
				store.AppendValues(prim.PropertiesFamily.Name,prim.PropertiesFamily.Description,prim.LocalID);
				objects.Add(prim.LocalID,prim);
				}
			});		
		}
		
		void onUpdate(Simulator simulator, ObjectUpdate update,ulong regionHandle, ushort timeDilation)
		{
			//ObjectUpdate update;
			if(objects.ContainsKey(update.LocalID))
			{
				//avs_pos[update.LocalID]=update.Position;
				// I will assume libsl has done the business here for me and the avatar contains
				// the details i need
				Gtk.Application.Invoke(delegate {										
					//avs.Add(update.LocalID);
					//MainClass.client.
					//store.AppendValues(update.
					//store.Foreach(myfunc);
				});
			}
		}
		
		bool myfunc(Gtk.TreeModel mod, Gtk.TreePath path, Gtk.TreeIter iter)
		{
			uint key=(uint)store.GetValue(iter,3);			
			if(!objects.ContainsKey(key))
			{
				store.Remove(ref iter);
			}
			else
			{
				//Update stuff here	
			}
			
			return true;
		}
		
		void onKillObject(Simulator simulator, uint objectID)
		{
			if(objects.ContainsKey(objectID))
			{
				objects.Remove(objectID);
				Gtk.Application.Invoke(delegate {						
					store.Foreach(myfunc);
				});
			}
		}

	
	}
}
