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
        Dictionary<LLUUID, Primitive> PrimsWaiting = new Dictionary<LLUUID, Primitive>();

		// The issue is that newprim, killand update all run from the simulator
		// and are indexed by (sim local) localIDs
		// GetObjectProperties is asset info and this comes from the aset server
		// referenced by (global) ObjectIDs

		// Its useful to keep an index of localids to Primitives and a map between localIDs and ObjectIDs
		Dictionary<uint,Primitive> objects= new Dictionary<uint,Primitive>(); 
		Dictionary<uint,LLUUID> objects_index= new Dictionary<uint,LLUUID>(); 
		Dictionary<LLUUID,uint> objects_index_rev= new Dictionary<LLUUID,uint>(); 

		public ObjectsLayout()
		{
			this.Build();
			store= new Gtk.ListStore (typeof(string),typeof(string),typeof(uint));
			treeview1.AppendColumn("Name",new Gtk.CellRendererText(),"text",0);
			treeview1.AppendColumn("Desc.",new Gtk.CellRendererText(),"text",1);
			treeview1.AppendColumn("ID",new Gtk.CellRendererText(),"text",2);
			treeview1.Model=store;

			MainClass.client.Objects.OnObjectProperties += new libsecondlife.ObjectManager.ObjectPropertiesCallback(Objects_OnObjectProperties);
		}

		bool myfunc(Gtk.TreeModel mod, Gtk.TreePath path, Gtk.TreeIter iter)
		{
			uint key=(uint)store.GetValue(iter,2);			
			if(objects.ContainsKey(key))
			{
				store.SetValue(iter,0,objects[key].Properties.Name);
				store.SetValue(iter,1,objects[key].Properties.Description);
				store.SetValue(iter,2,objects[key].Properties.ObjectID.ToString());
			}
			return true;
		}

		protected virtual void OnButtonSearchClicked (object sender, System.EventArgs e)
		{
			int radius;
			int.TryParse(this.entry1.Text,out radius);
			
			// *** get current location ***
            LLVector3 location = MainClass.client.Self.SimPosition;

            // *** find all objects in radius ***
            List<Primitive> prims = MainClass.client.Network.CurrentSim.ObjectsPrimitives.FindAll(
                delegate(Primitive prim) {
                    LLVector3 pos = prim.Position;
                    return ((prim.ParentID == 0) && (pos != LLVector3.Zero) && (LLVector3.Dist(pos, location) < radius));
                }
            );
			
			RequestObjectProperties(prims, 250);
			
		}
		
        private void RequestObjectProperties(List<Primitive> objects, int msPerRequest)
        {
            // Create an array of the local IDs of all the prims we are requesting properties for
            uint[] localids = new uint[objects.Count];

            lock (PrimsWaiting) {
                PrimsWaiting.Clear();

                for (int i = 0; i < objects.Count; ++i) {
                    localids[i] = objects[i].LocalID;
                    PrimsWaiting.Add(objects[i].ID, objects[i]);
                }
            }

            MainClass.client.Objects.SelectObjects(MainClass.client.Network.CurrentSim, localids);

            //return AllPropertiesReceived.WaitOne(2000 + msPerRequest * objects.Count, false);
        }

		void Objects_OnObjectProperties(Simulator simulator, LLObject.ObjectProperties properties)
        {
            lock (PrimsWaiting) {
                Primitive prim;
                if (PrimsWaiting.TryGetValue(properties.ObjectID, out prim)) {
                    prim.Properties = properties;
					store.AppendValues(prim.Properties.Name,prim.Properties.Description,prim.LocalID);
					Gtk.Application.Invoke(delegate {										
						store.Foreach(myfunc);
				});
				
				}
              //  PrimsWaiting.Remove(properties.ObjectID);

                //if (PrimsWaiting.Count == 0)
                   // AllPropertiesReceived.Set();
            }
        }

		
	}
}
