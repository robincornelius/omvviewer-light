/*
omvviewerlight a Text based client to metaverses such as Linden Labs Secondlife(tm)
    Copyright (C) 2008  Robin Cornelius <robin.cornelius@gmail.com>

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

// aPick.cs created with MonoDevelop
// User: robin at 19:17 15/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using OpenMetaverse;

namespace omvviewerlight
{

	public partial class aPick : Gtk.Bin
	{
		Vector3d picpos;
		string sim;
		UUID picimage;
		
		public aPick(UUID image,string name,string desc,string info,string simname,Vector3d pos)
		{
			this.Build();
			this.label_sim.Text=name;
			this.label_info.Text=simname+" @ "+pos.ToString();
			this.textview1.Buffer.Text=desc;
			sim=simname;
			picpos=pos;
			picimage=image;
			new TryGetImage(this.image2,image,128,128);
		}

		protected virtual void OnButtonTeleportClicked (object sender, System.EventArgs e)
		{
			Vector3 pos=new Vector3();
			pos.X=(int)(picpos.X)&0x0000FF;
			pos.Y=(int)(picpos.Y)&0x0000FF;
			pos.Z=(int)(picpos.Z)&0x0000FF;
			
			TeleportProgress tp = new TeleportProgress();
			tp.Show();
			tp.teleport(sim,pos);
					
		}

		protected virtual void OnEventbox3ButtonPressEvent (object o, Gtk.ButtonPressEventArgs args)
		{
			if(this.picimage==UUID.Zero)
				return;

			TexturePreview tp= new TexturePreview(this.picimage,label_sim.Text,false);
			tp.ShowAll();

		}
	}
}
