// GroupChatList.cs created with MonoDevelop
// User: robin at 15:21 12/02/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using OpenMetaverse;
using Gtk;
using System.Collections.Generic;

namespace omvviewerlight
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class GroupChatList : Gtk.Bin
	{
		UUID session;
		Gtk.ListStore store;	

		public GroupChatList()
		{
			this.Build();
			store= new Gtk.ListStore (typeof(string),typeof(UUID));
			MyTreeViewColumn tvc;
			tvc=new MyTreeViewColumn("Name",new Gtk.CellRendererText(),"text",0,true);			
			tvc.Sizing=Gtk.TreeViewColumnSizing.Autosize;
			tvc.setmodel(store);
			this.treeview_members.AppendColumn(tvc);
			this.treeview_members.HeadersClickable=true;
			store.SetSortColumnId(0,SortType.Ascending);
			
			
			
			Dictionary <UUID,TreeIter> memberstree= new Dictionary<UUID,TreeIter>();			
			treeview_members.Model=store;
		
        }
		
		public void setsession(UUID id)
		{
			session=id;		
			
                Gtk.Application.Invoke(delegate{			
				this.store.Clear();
				//lock(MainClass.client.Self.GroupChatSessions.Dictionary) //FIX ME i need to lock this but its private
				// libomv update required to use a better dictionary internally
					if(MainClass.client.Self.GroupChatSessions.ContainsKey(session))
						foreach(OpenMetaverse.ChatSessionMember member in MainClass.client.Self.GroupChatSessions[session])
						{
		                    string extra= member.IsModerator==true?" (moderator)":"";
							Gtk.TreeIter iter = store.AppendValues("Waiting...",member.AvatarKey);
				            AsyncNameUpdate ud=new AsyncNameUpdate(member.AvatarKey,false);  
					        ud.addparameters(iter);
			           			
					        ud.onNameCallBack += delegate(string namex,object[] values){Gtk.TreeIter iterx=(Gtk.TreeIter)values[0]; lock(store){store.SetValue(iterx,0,namex+extra);}};
				 	        ud.go();						
		  			    }	
				
	        MainClass.client.Self.OnChatSessionMemberAdded += new OpenMetaverse.AgentManager.ChatSessionMemberAddedCallback(onGroupChatMemberAdded);
			MainClass.client.Self.OnChatSessionMemberLeft += new OpenMetaverse.AgentManager.ChatSessionMemberLeftCallback(onGroupChatMemberLeft);	

            });			

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
