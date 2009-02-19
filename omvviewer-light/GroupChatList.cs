// GroupChatList.cs created with MonoDevelop
// User: robin at 15:21 12/02/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using OpenMetaverse;

namespace omvviewerlight
{
	public partial class GroupChatList : Gtk.Bin
	{
		UUID session;
		Gtk.ListStore store;	

		public GroupChatList()
		{
			this.Build();
			store= new Gtk.ListStore (typeof(string),typeof(UUID));
			Gtk.TreeViewColumn tvc;
			tvc=treeview_members.AppendColumn("Name",new Gtk.CellRendererText(),"text",0);
			//tvc.Resizable=true;
			tvc.Sizing=Gtk.TreeViewColumnSizing.Autosize;			
			treeview_members.Model=store;
	
			MainClass.client.Self.OnChatSessionMemberAdded += new OpenMetaverse.AgentManager.ChatSessionMemberAddedCallback(onGroupChatMemberAdded);
			MainClass.client.Self.OnChatSessionMemberLeft += new OpenMetaverse.AgentManager.ChatSessionMemberLeftCallback(onGroupChatMemberLeft);
		}
		
		public void setsession(UUID id)
		{
			session=id;			
		}
		
		void onGroupChatMemberAdded(UUID thissession, UUID key)
		{
			if(session!=thissession)
			return;
			
            Gtk.Application.Invoke(delegate{
			lock(store)
			{
			Gtk.TreeIter iter = store.AppendValues("Waiting...",key);
		     AsyncNameUpdate ud=new AsyncNameUpdate(key,false);  
			 ud.addparameters(iter);
           			
				
			 ud.onNameCallBack += delegate(string namex,object[] values){Gtk.TreeIter iterx=(Gtk.TreeIter)values[0]; lock(store){store.SetValue(iterx,0,namex);}};
			ud.go();
			}	 
            });
         }

		void onGroupChatMemberLeft(UUID thissession, UUID key)
		{
			if(session!=thissession)
			return;
            Gtk.Application.Invoke(delegate{
			lock(store)
            {
			store.Foreach(delegate(Gtk.TreeModel mod, Gtk.TreePath path, Gtk.TreeIter iter)
            {
				UUID id=(UUID)store.GetValue(iter,1);
 				if(id==key)
                {					
				    store.Remove(ref iter);
			        return false; //???????? CHECK ME
                }
			return true;	//?????????????? CHECK ME
			});	
			}	
            });	
			
		}


	}
}
