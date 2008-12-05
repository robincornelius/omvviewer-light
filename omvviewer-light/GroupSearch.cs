// GroupSearch.cs created with MonoDevelop
// User: robin at 08:56 14/11/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using OpenMetaverse;
using System.Collections.Generic;

namespace omvviewerlight
{
	
	public partial class GroupSearch : Gtk.Bin
	{
		Gtk.ListStore store;
		UUID queryid;
		
		public GroupSearch()
		{
			this.Build();
			MainClass.client.Directory.OnDirGroupsReply+=new OpenMetaverse.DirectoryManager.DirGroupsReplyCallback(onGroupReply);
		    store= new Gtk.ListStore (typeof(string),typeof(string),typeof(UUID));

            MyTreeViewColumn mycol;
            mycol = new MyTreeViewColumn("Name", new Gtk.CellRendererText(), "text", 0);
            mycol.setmodel(store);
            treeview1.AppendColumn(mycol);

            mycol = new MyTreeViewColumn("Members", new Gtk.CellRendererText(), "text", 1);
            mycol.setmodel(store);
            store.SetSortFunc(2, numericsort);
            treeview1.AppendColumn(mycol);

			treeview1.Model=store;

		}

		new public void Dispose()
		{
			
			MainClass.client.Directory.OnDirGroupsReply-=new OpenMetaverse.DirectoryManager.DirGroupsReplyCallback(onGroupReply);
			
			//Finalize();
			//System.GC.SuppressFinalize(this);
		}
		

        int numericsort(Gtk.TreeModel model, Gtk.TreeIter a, Gtk.TreeIter b)
        {

            float Pa;
            float.TryParse((string)store.GetValue(a, 2), out Pa);
            float Pb;
            float.TryParse((string)store.GetValue(b, 2), out Pb);

            int tSortColumnId;
            Gtk.SortType order;

            store.GetSortColumnId(out tSortColumnId, out order);

            if (Pa == Pb)
                return 0;

            if (order == Gtk.SortType.Ascending)
            {
                if (Pa > Pb)
                    return -1;
                else
                    return 1;
            }
            else
            {
                if (Pa > Pb)
                    return 1;
                else
                    return -1;
            }
        }

		void onGroupReply(UUID thisqueryid,List <OpenMetaverse.DirectoryManager.GroupSearchData> MatchedGroups)
	    {
			if(queryid!=thisqueryid)
				return;

			Gtk.Application.Invoke(delegate{
				
				this.label_search_progress.Text="Returned "+MatchedGroups.Count.ToString()+" results";
				foreach (OpenMetaverse.DirectoryManager.GroupSearchData agroup in MatchedGroups)
				{
					store.AppendValues(agroup.GroupName,agroup.Members.ToString(),agroup.GroupID);
				
				}			
			});
			
	    }
		
		protected virtual void OnButton1Clicked (object sender, System.EventArgs e)
		{
			this.label_search_progress.Text="searching....";
			queryid=MainClass.client.Directory.StartGroupSearch(OpenMetaverse.DirectoryManager.DirFindFlags.Groups,this.entry_search.Text,0);
		}

		protected virtual void OnButtonViewGroupProfileClicked (object sender, System.EventArgs e)
		{
						Gtk.TreeModel mod;
			Gtk.TreeIter iter;
			
			if(treeview1.Selection.GetSelected(out mod,out iter))			
			{
				UUID id=(UUID)mod.GetValue(iter,2);			
				GroupInfo gi=new GroupInfo(id,false);
				
			}
				
		}
	}
}
