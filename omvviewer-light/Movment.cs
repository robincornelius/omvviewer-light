// Movment.cs created with MonoDevelop
// User: robin at 15:33 27/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using libsecondlife;
using Gdk;
using Gtk;

namespace omvviewerlight
{
	
	public partial class Movment : Gtk.Bin
	{
		
		LLQuaternion rotation;
		
		public Movment()
		{
			this.Build();			
			this.image_direction.Pixbuf=new Pixbuf("arrow.tga");
            Gtk.Timeout.Add(1000, dirupdate);
            MainClass.client.Objects.OnObjectUpdated += new ObjectManager.ObjectUpdatedCallback(Objects_OnObjectUpdated);
        }

        void Objects_OnObjectUpdated(Simulator simulator, ObjectUpdate update, ulong regionHandle, ushort timeDilation)
        {
            if (update.LocalID == MainClass.client.Self.LocalID)
            {
                Console.Write("UPDATE ME\n");
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

			LLQuaternion myrotation=MainClass.client.Self.RelativeRotation;
			Console.Write(myrotation.ToString()+"\n");

			if(myrotation!=rotation)
			{
				
				float x,y,z;
				rotation=myrotation;
				LLMatrix3 xx=new LLMatrix3(rotation);
				xx.GetEulerAngles(out x,out y,out z);
				Console.Write("X :"+x.ToString()+" Y: "+y.ToString()+" Z: "+z.ToString()+"\n");
				z=(float)z*(float)(360.0/(2.0*3.1415));
				this.spinbutton_direction.Value=(int)z;
				//Gdk.PixbufRotation angle=new Gdk.PixbufRotation(z);
					
			}
			
			return true;
		}

		protected virtual void OnSpinbuttonDirectionChangeValue (object o, Gtk.ChangeValueArgs args)
		{
		 Console.Write("CHANGEVALUE\n");

		}

		protected virtual void OnSpinbuttonDirectionValueChanged (object sender, System.EventArgs e)
		{
		 Console.Write("VALUECHANGE\n");

		}

		protected virtual void OnButton1Pressed (object sender, System.EventArgs e)
		{
			MainClass.client.Self.Movement.TurnLeft=true;
            MainClass.client.Self.Movement.SendUpdate(true, MainClass.client.Network.CurrentSim);
		}

		protected virtual void OnButton1Released (object sender, System.EventArgs e)
		{
			MainClass.client.Self.Movement.TurnLeft=false;
            MainClass.client.Self.Movement.SendUpdate(true, MainClass.client.Network.CurrentSim);
        }

		protected virtual void OnButton2Pressed (object sender, System.EventArgs e)
		{
			MainClass.client.Self.Movement.TurnRight=true;
            MainClass.client.Self.Movement.SendUpdate(true, MainClass.client.Network.CurrentSim);
		}

		protected virtual void OnButton2Released (object sender, System.EventArgs e)
		{
			MainClass.client.Self.Movement.TurnRight=false;
            MainClass.client.Self.Movement.SendUpdate(true, MainClass.client.Network.CurrentSim);	
		}
		
		protected virtual void OnButtonFwdPressed (object sender, System.EventArgs e)
		{
			MainClass.client.Self.Movement.AtPos=true;
            MainClass.client.Self.Movement.SendUpdate(true, MainClass.client.Network.CurrentSim);	

		}

		protected virtual void OnButtonFwdReleased (object sender, System.EventArgs e)
		{
			MainClass.client.Self.Movement.AtNeg=true;
            MainClass.client.Self.Movement.SendUpdate(true, MainClass.client.Network.CurrentSim);	

		}

	}
}
