// TeleportTo.cs created with MonoDevelop
// User: robin at 16:23 12/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using libsecondlife;
using Gtk;


namespace omvviewerlight
{
	
	public partial class TeleportTo : Gtk.Bin
	{
		
		public TeleportTo()
		{
			this.Build();		
		}

		protected virtual void OnButtonTeleportActivated (object sender, System.EventArgs e)
		{
		}

		protected virtual void OnButtonTeleportClicked (object sender, System.EventArgs e)
		{
			LLVector3 pos;
			pos=new LLVector3();
			pos.X=(float)this.spinbutton_x.Value;
			pos.Y=(float)this.spinbutton_y.Value;
			pos.Z=(float)this.spinbutton_z.Value;
			
			MainClass.client.Self.Teleport(entry_simname.Text,pos);	
		
		}
	}
}
