// Friendshiprequest.cs created with MonoDevelop
// User: robin at 20:53 16/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using libsecondlife;

namespace omvviewerlight
{
	
	
	public partial class Friendshiprequest : Gtk.Window
	{
		string name;
		LLUUID agent;
		LLUUID session;
		public Friendshiprequest(LLUUID agentID,string agentname,LLUUID sessionid) : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			this.label_info.Text=agentname+" has offered friendship Press \"accept\" to accept this or press \"reject\" to declin the request";
			name=agentname;
			session=sessionid;
			agent=agentID;
		}

		protected virtual void OnButtonAcceptClicked (object sender, System.EventArgs e)
		{
			MainClass.client.Friends.AcceptFriendship(agent,session);
			this.Destroy();
		}

		protected virtual void OnButton3Clicked (object sender, System.EventArgs e)
		{
			MainClass.client.Friends.DeclineFriendship(agent,session);
			this.Destroy();
		}
	}
}
