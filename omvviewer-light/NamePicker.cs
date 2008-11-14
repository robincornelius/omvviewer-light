// NamePicker.cs created with MonoDevelop
// User: robin at 19:37 22/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;
using OpenMetaverse;
using Gdk;
using Gtk;

namespace omvviewerlight
{
	
	
	public partial class NamePicker : Gtk.Window
	{
		
		Gtk.ListStore store;
		
		public delegate void UserSelected(UUID id,UUID asset,string item_name,string user_name);
 
        // Define an Event based on the above Delegate
        public event UserSelected UserSel;
		public UUID asset;
		public string item_name;
		public string user_name;
			
		public NamePicker() : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			store= new Gtk.ListStore (typeof(string),typeof(UUID));
			treeview1.AppendColumn("Name",new Gtk.CellRendererText(),"text",0);		
			treeview1.Model=store;
			store.SetSortColumnId(0,Gtk.SortType.Ascending);
			
			lock(MainClass.name_cache.av_names)
			{
				foreach(KeyValuePair<UUID,string> name in MainClass.name_cache.av_names)
				{
					store.AppendValues(name.Value,name.Key);
					
				}
			}
		}
		
		protected virtual void OnButtonSelectClicked (object sender, System.EventArgs e)
		{
			Gtk.TreeModel mod;
			Gtk.TreeIter iter;
			
			if(treeview1.Selection.GetSelected(out mod,out iter))			
			{
				UUID id=(UUID)mod.GetValue(iter,1);
				if(UserSel!=null)
					UserSel(id,asset,item_name,user_name);
			}
			
			this.Destroy();
				
		}

		protected virtual void OnButtonCancelClicked (object sender, System.EventArgs e)
		{
			
			this.Destroy();
		}
		
				
		
	}
}
