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
		static uint target_avatar;
		static Vector3 target_pos;
		
		public static void init()
		{
			Active=false;
		}
		
		public static void stop()
		{
			MainClass.client.Self.AutoPilotCancel();
			Active=false;			
		}
		
		public static void set_target_object (UUID tobject)
		{
			 Logger.DebugLog("Autopilot start");
			type=TargetType.TARGET_OBJECT;
			target_object=tobject;
			Active=true;
			GLib.Timeout.Add(1000,Think);
		}
		
		public static void set_target_avatar (uint localid)
		{
			type=TargetType.TARGET_AVATAR;
			target_avatar=localid;
			Active=true;
			GLib.Timeout.Add(1000,Think);

		}
		
		public static void set_target_pos (Vector3 pos)
		{
			type=TargetType.TARGET_POS;
			target_pos=pos;
			Active=true;
			GLib.Timeout.Add(1000,Think);
		}
		
		static Vector3 get_av_pos(uint targetLocalID,out float distance,out Simulator sim)
		{	
          lock (MainClass.client.Network.Simulators)
          {
					
				Avatar targetAv;
				Vector3 targetpos;
				targetpos.X=0;
				targetpos.Y=0;
				targetpos.Z=0;
				distance=0;
				sim=MainClass.client.Network.CurrentSim;
				
				for (int i = 0; i < MainClass.client.Network.Simulators.Count; i++)
	            {
				
					if (MainClass.client.Network.Simulators[i].ObjectsAvatars.TryGetValue(targetLocalID, out targetAv))
					{
						if(targetAv.ParentID!=0)
						{
						
							if(!MainClass.client.Network.CurrentSim.ObjectsPrimitives.Dictionary.ContainsKey(targetAv.ParentID))
							{
								Console.WriteLine("AV is seated and i can't find the parent prim in dictionay");
								Active=false;
								Vector3 x;
								x.X=0;
								x.Y=0;
								x.Z=0;
					
								return targetpos;
							}
							else
							{
								Vector3 pos;
								Primitive parent = MainClass.client.Network.CurrentSim.ObjectsPrimitives.Dictionary[targetAv.ParentID];
								targetpos=pos = Vector3.Transform(targetAv.Position, Matrix4.CreateFromQuaternion(parent.Rotation)) + parent.Position;
								if (MainClass.client.Network.Simulators[i] == MainClass.client.Network.CurrentSim)
								{
									sim=MainClass.client.Network.Simulators[i];
									distance = Vector3.Distance(pos, MainClass.client.Self.SimPosition);
								}					
							}				
						}			
						else
						{
							if (MainClass.client.Network.Simulators[i] == MainClass.client.Network.CurrentSim)
							{
								sim=MainClass.client.Network.Simulators[i];
								distance = Vector3.Distance(targetAv.Position, MainClass.client.Self.SimPosition);
								targetpos=targetAv.Position;
							}
						}
					}			
						
					return targetpos;
				}
				
				return targetpos;
					
			}
		}
		
		static Vector3 localtoglobalpos(Vector3 targetpos,Simulator sim)
		{
			
			 uint regionX, regionY;
             Utils.LongToUInts(sim.Handle, out regionX, out regionY);
						
             double xTarget = (double)targetpos.X + (double)regionX;
             double yTarget = (double)targetpos.Y + (double)regionY;
             double zTarget = targetpos.Z;
            
			 Vector3 pos;
			 pos.X=(float)xTarget;
			 pos.Y=(float)yTarget;
			 pos.Z=(float)zTarget;
			 return pos;
		}
		
		static bool Think()
		{
            if (Active)
            {               
				Avatar targetAv;
			    Simulator sim=MainClass.client.Network.CurrentSim;
                float distance = 0.0f;	
				Vector3 targetpos;
				
				if(type==TargetType.TARGET_AVATAR)
				{
					targetpos=get_av_pos(target_avatar,out distance,out sim);
				}
				else				
				{
					targetpos=target_pos;
					targetpos.Z=MainClass.client.Self.SimPosition.Z;
					distance = Vector3.Distance(targetpos, MainClass.client.Self.SimPosition);					
					               
				}
				
                if (distance > 2.5)
				{
					Logger.DebugLog("Autopilot think");
				    Vector3 targetglobal=localtoglobalpos(targetpos,sim);
                    MainClass.client.Self.Movement.TurnToward(targetpos);			
                    MainClass.client.Self.AutoPilot(targetglobal.X, targetglobal.Y, targetglobal.Z);
				 }
                 else
				 {
					 Active=false;
					 return false;
				     Logger.DebugLog("Stopping autopilot");
                 }
                        
				return true;
            }
			else
			{
				 Logger.DebugLog("NOT ACTIVE Stopping autopilot");
				Active=false;
	            return false;
			}
			
		}	
	}
}
