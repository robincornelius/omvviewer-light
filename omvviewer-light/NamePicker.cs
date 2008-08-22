// NamePicker.cs created with MonoDevelop
// User: robin at 19:37 22/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;
using libsecondlife;
using Gdk;
using Gtk;

namespace omvviewerlight
{
	
	
	public partial class NamePicker : Gtk.Window
	{
		
		Gtk.ListStore store;
		
		public delegate void UserSelected(LLUUID id,LLUUID asset,string item_name);
 
        // Define an Event based on the above Delegate
        public event UserSelected UserSel;
		public LLUUID asset;
		public string item_name;
		public string user_name;
		
		
		public NamePicker() : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			store= new Gtk.ListStore (typeof(string),typeof(LLUUID));
			treeview1.AppendColumn("Name",new Gtk.CellRendererText(),"text",0);		
			treeview1.Model=store;
			store.SetSortColumnId(0,Gtk.SortType.Ascending);
			
			foreach(KeyValuePair<LLUUID,string> name in MainClass.av_names)
			{
				store.AppendValues(name.Value,name.Key);
				
			}
			
		}
		
		protected virtual void OnButtonSelectClicked (object sender, System.EventArgs e)
		{
			Gtk.TreeModel mod;
			Gtk.TreeIter iter;
			
			MessageDialog md = new MessageDialog(MainClass.win,DialogFlags.Modal,MessageType.Question,ButtonsType.YesNo,"Are you sure you wish to give\n"+item_name+"to "+user_name);
			ResponseType result=(ResponseType)md.Run();	
			if(result==ResponseType.No)
			{
				this.Destroy();
				return;
			}
			
			if(treeview1.Selection.GetSelected(out mod,out iter))			
			{
				LLUUID id=(LLUUID)mod.GetValue(iter,1);
				if(UserSel!=null)
					UserSel(id,asset,item_name);
			}
			
			this.Destroy();
				
		}

		protected virtual void OnButtonCancelClicked (object sender, System.EventArgs e)
		{
			
			this.Destroy();
		}
		
				
		
	}
}
