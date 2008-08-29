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
           // Gtk.Timeout.Add(1000, dirupdate);
            MainClass.client.Objects.OnObjectUpdated += new ObjectManager.ObjectUpdatedCallback(Objects_OnObjectUpdated);
            //Gdk.Pixbuf rotated = rotatepixbuf((float)0.7, this.image_direction.Pixbuf);
            //this.image_direction.Pixbuf = rotated;
        
        }

        void Objects_OnObjectUpdated(Simulator simulator, ObjectUpdate update, ulong regionHandle, ushort timeDilation)
        {
            if (update.LocalID == MainClass.client.Self.LocalID)
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

			LLQuaternion myrotation=MainClass.client.Self.RelativeRotation;
			Console.Write(myrotation.ToString()+"\n");

			if(myrotation!=rotation)
			{
				
				float x,y,z;
				rotation=myrotation;
				LLMatrix3 xx=new LLMatrix3(rotation);
				xx.GetEulerAngles(out x,out y,out z);
				Console.Write("X :"+x.ToString()+" Y: "+y.ToString()+" Z: "+z.ToString()+"\n");
				this.spinbutton_direction.Value=(int)z*(float)(360.0/(2.0*3.1415));
               // Gdk.Pixbuf rotated = rotatepixbuf((float)1.567, this.image_direction.Pixbuf);
               // this.image_direction.Pixbuf = rotated;

			}
			
			return true;
		}

        unsafe Gdk.Pixbuf rotatepixbuf(float theta, Pixbuf buf)
        {
            Gdk.Pixbuf tempbuf = new Gdk.Pixbuf(buf, 0, 0, 100, 100);

            int srcwidth = tempbuf.Width;
            int srcheight = tempbuf.Height;
            int srcrowsstride = tempbuf.Rowstride;
            int schannels = tempbuf.NChannels;

            Console.Write("Width " + srcwidth.ToString() + " Height " + srcheight.ToString() + "\n");

            sbyte* pixels = (sbyte*)tempbuf.Pixels;
            sbyte* spixels = (sbyte*)buf.Pixels;
            sbyte* p;
            sbyte* ps;

            for (int sx = 0; sx < srcwidth; sx++)
            {
                for (int sy = 0; sy < srcheight; sy++)
                {
                   
                    
                    ps = spixels + ((sy) * srcrowsstride) + ((sx) * schannels);
                    double radius;
                    int xx = 50 - sx;
                    int yy = 50 - sy;
                    radius = Math.Sqrt((xx * xx) + (yy * yy));

                    int nx = (int) (Math.Sin(theta)*radius);
                    int ny = (int)(Math.Cos(theta)*radius);
                    nx = nx + 50;
                    ny = ny + 50;

                    Console.Write("Rotate (" + sx.ToString() + "," + sy.ToString() + ") -> (" + nx.ToString() + "," + ny.ToString() + ")\n");

                    if (nx >= 0 && nx < 100)
                        if (ny >= 0 && ny < 100)
                        {
                            p = pixels + ((ny) * srcrowsstride) + ((nx) * schannels);

                            p[0] = ps[0];
                            p[1] = ps[1];
                            p[2] = ps[2];
                        }
                   }
            }




            return tempbuf;
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
