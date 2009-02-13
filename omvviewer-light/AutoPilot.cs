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

// AutoPilot.cs created with MonoDevelop
// User: robin at 11:59 07/12/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using OpenMetaverse;
using GLib;

namespace omvviewerlight
{
	static class AutoPilot
	{
		enum TargetType{
		TARGET_AVATAR,
		TARGET_OBJECT,
		TARGET_POS
	}
		
		static TargetType type;
		
		static bool Active;
		static UUID target_object;
        static UUID target_avatar;
		static Vector3d target_pos_global;
		static bool follow=false;


		public delegate void AutoPilotFinished();
        public static event AutoPilotFinished onAutoPilotFinished;

		public static void init()
		{
			Active=false;
		}
		
		public static void stop()
		{
			MainClass.client.Self.AutoPilotCancel();
			if(onAutoPilotFinished!=null)
				onAutoPilotFinished();
			
			Active=false;
			follow=false;
		}
		
		public static void set_target_object (UUID tobject)
		{
			 Logger.DebugLog("Autopilot start");
			type=TargetType.TARGET_OBJECT;
			target_object=tobject;
			Active=true;
			GLib.Timeout.Add(100,Think);
		}
		
		public static void set_target_avatar (UUID target,bool followon)
		{
			type=TargetType.TARGET_AVATAR;
			target_avatar=target;
			Active=true;
			GLib.Timeout.Add(100,Think);
			follow=followon;
		}
		
		public static void set_target_pos (Vector3d globalpos)
		{
			type=TargetType.TARGET_POS;
            target_pos_global = globalpos;
			Active=true;
			GLib.Timeout.Add(100,Think);
		}

        public static Avatar findavatarinsims(UUID findtarget)
        {
            Avatar target=null;

            lock (MainClass.client.Network.Simulators)
            {
                for (int i = 0; i < MainClass.client.Network.Simulators.Count; i++)
                {
                    target = MainClass.client.Network.Simulators[i].ObjectsAvatars.Find
                    (
                        delegate(Avatar avatar)
                        {
                            return avatar.ID == findtarget;
                        }
                     );
                     if(target!=null)
                        return target;

                }
            }

            return target;
        }

        static uint getlocalid(UUID id)
        {
            Avatar target = findavatarinsims(id);
            if(target!=null)
                return target.LocalID;

            return 0;
        }

		static Vector3d get_av_pos(UUID targetID,out float distance)
		{	
          lock (MainClass.client.Network.Simulators)
          {
				Vector3 targetpos;
				targetpos.X=0;
				targetpos.Y=0;
				targetpos.Z=0;
				distance=0;
				
				int i;
				for (i = 0; i < MainClass.client.Network.Simulators.Count; i++)
	            {

                    Avatar targetAv = MainClass.client.Network.Simulators[i].ObjectsAvatars.Find(
                        delegate(Avatar avatar)
                        {
                            return avatar.ID == targetID;
                        }
                    );
				
                    if(targetAv!=null)
					{
						if(targetAv.ParentID!=0)
						{
							if(!MainClass.client.Network.Simulators[i].ObjectsPrimitives.Dictionary.ContainsKey(targetAv.ParentID))
							{
								Console.WriteLine("AV is seated and i can't find the parent prim in dictionay");
								Active=false;
								Vector3 x;
								x.X=0;
								x.Y=0;
								x.Z=0;
					
								return new Vector3d(targetpos);
							}
							else
							{
								Vector3 pos;
								Primitive parent = MainClass.client.Network.Simulators[i].ObjectsPrimitives.Dictionary[targetAv.ParentID];
								targetpos=pos = Vector3.Transform(targetAv.Position, Matrix4.CreateFromQuaternion(parent.Rotation)) + parent.Position;
								distance = (float)Vector3d.Distance(new Vector3d(pos), MainClass.client.Self.GlobalPosition);
													
							}				
						}			
						else
						{
							
                                distance = (float)Vector3d.Distance(new Vector3d(targetAv.Position), MainClass.client.Self.GlobalPosition);
								targetpos=targetAv.Position;
						}
					}

                    return new Vector3d(targetpos);
				}

                return new Vector3d(targetpos);
					
			}
		}

        public static Primitive findobject(UUID findtarget,out Simulator sim)
        {
            Primitive target=null;

            lock (MainClass.client.Network.Simulators)
            {
                for (int i = 0; i < MainClass.client.Network.Simulators.Count; i++)
                {
                    
                    target = MainClass.client.Network.Simulators[i].ObjectsPrimitives.Find
                    (
                        delegate(Primitive prim)
                        {
                            return prim.ID == findtarget;
                        }
                     );
                     if(target!=null)
                     {
                        sim=MainClass.client.Network.Simulators[i];
                        return target;
                     }

                }
            }

            sim=null;
            return target;

        }
		
		public static Vector3d localtoglobalpos(Vector3 targetpos,ulong sim_handle)
		{
			
			 uint regionX, regionY;
             Utils.LongToUInts(sim_handle, out regionX, out regionY);
						
             double xTarget = (double)targetpos.X + (double)regionX;
             double yTarget = (double)targetpos.Y + (double)regionY;

			 double zTarget = targetpos.Z;
            
			 Vector3d pos;
			 pos.X=xTarget;
			 pos.Y=yTarget;
			 pos.Z=zTarget;
			 return pos;
		}
		
		static bool Think()
		{
            if (Active)
            {               
			    Simulator sim=MainClass.client.Network.CurrentSim;
                float distance = 0.0f;	
				Vector3d targetpos;

                switch (type)
                {
                    case TargetType.TARGET_AVATAR:
                        targetpos = get_av_pos(target_avatar, out distance);
                        distance = (float)Vector3d.Distance(targetpos, new Vector3d(MainClass.client.Self.SimPosition));
                        //Console.WriteLine("Avatar Target at "+targetpos.ToString());
                        //Console.WriteLine("I'm at "+MainClass.client.Self.SimPosition.ToString());
                        //Console.WriteLine("Distance is "+distance.ToString());
                        break;
                    case TargetType.TARGET_OBJECT:
                        Simulator obj_sim;
                        Primitive prim = findobject(target_object, out obj_sim);
                        if (prim != null)
                        {
                            targetpos = localtoglobalpos(prim.Position, obj_sim.Handle);
                        }
                        else
                        {
                            targetpos = MainClass.client.Self.GlobalPosition;
                        }
                
                        break;
                    case TargetType.TARGET_POS:
                        targetpos=target_pos_global;
					    targetpos.Z=MainClass.client.Self.SimPosition.Z;
                        distance = (float)Vector3d.Distance(targetpos, new Vector3d(MainClass.client.Self.GlobalPosition));					              
                        break;
                    default:
                        targetpos = new Vector3d(MainClass.client.Self.GlobalPosition);
                        break;
                }

                if (distance > 2.5)
				{
                    
					//Console.WriteLine("Autopilot think");
                    //Console.WriteLine("Target at " + targetpos.ToString());
                    //Console.WriteLine("I'm at Global" + MainClass.client.Self.GlobalPosition.ToString());
                    //Console.WriteLine("I'm at Local" + MainClass.client.Self.SimPosition.ToString());
                    //Console.WriteLine("Distance is " + distance.ToString());
                    //Console.WriteLine("Local vector is "+(new Vector3(targetpos)-new Vector3(MainClass.client.Self.GlobalPosition)).ToString());
                    Vector3 heading=new Vector3(targetpos)-new Vector3(MainClass.client.Self.GlobalPosition);
                    heading.Normalize();
                    heading = MainClass.client.Self.SimPosition + heading;
	                MainClass.client.Self.Movement.TurnToward(heading);
					MainClass.client.Self.Movement.AtPos=true;
					MainClass.client.Self.Movement.SendUpdate();
				}
                 else if (follow==false)
				 {
					 Active=false;
					 MainClass.client.Self.Movement.AtPos=false;
					 MainClass.client.Self.Movement.SendUpdate();
					 if(onAutoPilotFinished!=null)
						onAutoPilotFinished();
				     Console.WriteLine("Stopping autopilot");
					return false;
                 }
                        
				return true;
            }
			else
			{
				Console.WriteLine("NOT ACTIVE Stopping autopilot");
				MainClass.client.Self.Movement.AtPos=false;				
				MainClass.client.Self.Movement.SendUpdate();			
				Active=false;
				if(onAutoPilotFinished!=null)
					onAutoPilotFinished();
				return false;
			}
			
		}	
	}
}
