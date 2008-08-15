// aPick.cs created with MonoDevelop
// User: robin at 19:17 15/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using libsecondlife;

namespace omvviewerlight
{

	
	public partial class aPick : Gtk.Bin
	{
		LLVector3d picpos;
		string sim;
		
		public aPick(LLUUID image,string name,string desc,string info,string simname,LLVector3d pos)
		{
			this.Build();
			this.label_sim.Text=name;
			this.label_info.Text=simname+" @ "+pos.ToString();
			this.textview1.Buffer.Text=desc;
			sim=simname;
			picpos=pos;
		}

		protected virtual void OnButtonTeleportClicked (object sender, System.EventArgs e)
		{
			LLVector3 tp=new LLVector3();
			tp.X=(int)(picpos.X)&0x0000FF;
			tp.Y=(int)(picpos.Y)&0x0000FF;
			tp.Z=(int)(picpos.Z)&0x0000FF;
			
			MainClass.client.Self.Teleport(sim,tp);
			
		}
	}
}
