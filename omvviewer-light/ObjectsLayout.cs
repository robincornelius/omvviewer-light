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
	    List<uint> avs = new List<uint>();
	
		public ObjectsLayout()
		{
			this.Build();
			store= new Gtk.ListStore (typeof(string),typeof(string),typeof(string),typeof(uint));
			treeview1.AppendColumn("Name",new Gtk.CellRendererText(),"text",1);
			treeview1.AppendColumn("Dist.",new Gtk.CellRendererText(),"text",2);
			treeview1.Model=store;

			MainClass.client.Objects.OnObjectKilled += new libsecondlife.ObjectManager.KillObjectCallback(onKillObject);
			MainClass.client.Objects.OnObjectUpdated += new libsecondlife.ObjectManager.ObjectUpdatedCallback(onUpdate);
		}
		
		void onUpdate(Simulator simulator, ObjectUpdate update,ulong regionHandle, ushort timeDilation)
		{
			//ObjectUpdate update;
			if(!avs.Contains(update.LocalID))
			{
				//avs_pos[update.LocalID]=update.Position;
				// I will assume libsl has done the business here for me and the avatar contains
				// the details i need
				Gtk.Application.Invoke(delegate {										
					avs.Add(update.LocalID);
					//MainClass.client.
					//store.AppendValues(update.
					//store.Foreach(myfunc);
				});
			}
		}
		
		bool myfunc(Gtk.TreeModel mod, Gtk.TreePath path, Gtk.TreeIter iter)
		{
			uint key=(uint)store.GetValue(iter,3);			
			if(!avs.Contains(key))
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
			if(avs.Contains(objectID))
			{
				avs.Remove(objectID);
				Gtk.Application.Invoke(delegate {						
					store.Foreach(myfunc);
				});
			}
		}

	
	}
}
