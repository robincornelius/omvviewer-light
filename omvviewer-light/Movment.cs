/*
omvviewerlight a Text based client to metaverses such as Linden Labs Secondlife(tm)
    Copyright (C) 2008,2009  Robin Cornelius <robin.cornelius@gmail.com>

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 2 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

// Movment.cs created with MonoDevelop
// User: robin at 15:33 27/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using OpenMetaverse;
using Gdk;
using Gtk;

namespace omvviewerlight
{
	
	[System.ComponentModel.ToolboxItem(true)]	
	public partial class Movment : Gtk.Bin
	{

        Quaternion rotation;
		~Movment()
		{
			Console.WriteLine("Movement Cleaned up");
		}		

		
		public Movment()
		{
			this.Build();			

            //Gtk.Timeout.Add(1000, dirupdate);
            MainClass.client.Objects.ObjectUpdate += new EventHandler<PrimEventArgs>(Objects_ObjectUpdate);  
        }

        void Objects_ObjectUpdate(object sender, PrimEventArgs e)
        {
            if (e.Prim.LocalID == MainClass.client.Self.LocalID)
            {
                Gtk.Application.Invoke(delegate
                {
                    dirupdate();
                });
            }
        }
	
		bool dirupdate()
		{
			if(MainClass.client.Network.LoginStatusCode!=LoginStatus.Success)
				return true;

			Quaternion myrotation=MainClass.client.Self.RelativeRotation;
			Console.Write(myrotation.ToString()+"\n");

			if(myrotation!=rotation)
			{
				
				float x,y,z;
			    rotation=myrotation;
                rotation.GetEulerAngles(out x, out y, out z);
				this.spinbutton_direction.Value=(int)z*(float)(360.0/(2.0*3.1415));    
			}
			
			return true;
		}

		protected virtual void OnButton1Pressed (object sender, System.EventArgs e)
		{
			MainClass.client.Self.Movement.TurnLeft=true;
         MainClass.client.Self.Movement.SendUpdate(false);
		}

		protected virtual void OnButton1Released (object sender, System.EventArgs e)
		{
			MainClass.client.Self.Movement.TurnLeft=false;
       MainClass.client.Self.Movement.SendUpdate(false);
        }

		protected virtual void OnButton2Pressed (object sender, System.EventArgs e)
		{
			MainClass.client.Self.Movement.TurnRight=true;
      MainClass.client.Self.Movement.SendUpdate(false);
		}

		protected virtual void OnButton2Released (object sender, System.EventArgs e)
		{
			MainClass.client.Self.Movement.TurnRight=false;
         MainClass.client.Self.Movement.SendUpdate(false);
		}
		
		protected virtual void OnButtonFwdPressed (object sender, System.EventArgs e)
		{
		
			MainClass.client.Self.Movement.AtPos=true;
            MainClass.client.Self.Movement.SendUpdate(false);
		}

		protected virtual void OnButtonFwdReleased (object sender, System.EventArgs e)
		{
			MainClass.client.Self.Movement.AtNeg=true;
            MainClass.client.Self.Movement.SendUpdate(false);

		}

		protected virtual void OnSpinbuttonDirectionChangeValue (object o, Gtk.ChangeValueArgs args)
		{
		}

		protected virtual void OnSpinbuttonDirectionValueChanged (object sender, System.EventArgs e)
		{
		}

	}
}
