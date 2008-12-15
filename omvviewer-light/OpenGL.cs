/*

  This file is based on the code for PrimWorkshop, part of libomv.
  Copyright (c) 2007-2008, openmetaverse.org
  All rights reserved.
 
  Additional work to this file :-
  Copyright (c) 2008 Robin Cornelius <robin.cornelius@gmail.com>
 
  - Redistribution and use in source and binary forms, with or without 
    modification, are permitted provided that the following conditions are met:
 
  - Redistributions of source code must retain the above copyright notice, this
    list of conditions and the following disclaimer.
  - Neither the name of the openmetaverse.org nor the names 
    of its contributors may be used to endorse or promote products derived from
    this software without specific prior written permission.
 
  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
  AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
  IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
  ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
  LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
  CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
  SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
  INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
  CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
  ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
  POSSIBILITY OF SUCH DAMAGE.

*/

// OpenGL.cs created with MonoDevelop
// User: robin at 15:05 08/12/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//


using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Threading;

using Tao.FreeGlut;
using Tao.OpenGl;
using OpenMetaverse;
using OpenMetaverse.StructuredData;
using OpenMetaverse.Imaging;
using OpenMetaverse.Rendering;

namespace omvviewerlight
{
	
	public partial class OpenGL : Gtk.Bin
	{		
		const float DEG_TO_RAD = 0.0174532925f;
        const uint TERRAIN_START = (uint)Int32.MaxValue + 1;

        sCamera Camera;
        Dictionary<uint, Primitive> RenderFoliageList = new Dictionary<uint, Primitive>();
        Dictionary<uint, RenderablePrim> RenderPrimList = new Dictionary<uint, RenderablePrim>();

        EventHandler IdleEvent;

		int specialkey=0;
		
        System.Timers.Timer ProgressTimer;
        int TotalPrims;

        // Textures
        TexturePipeline TextureDownloader;
        Dictionary<UUID, TextureInfo> Textures = new Dictionary<UUID, TextureInfo>();

        // Terrain
        float MaxHeight = 0.1f;
        OpenMetaverse.TerrainPatch[,] Heightmap;
        HeightmapLookupValue[] LookupHeightTable;

        // Picking globals
        bool Clicked = false;
        int ClickX = 0;
        int ClickY = 0;
        uint LastHit = 0;

        //
        Vector3 PivotPosition = Vector3.Zero;
        bool Pivoting = false;
        Point LastPivot;

		int winno;
		bool abort=false;
		
        //
        const int SELECT_BUFSIZE = 512;
        int[] SelectBuffer = new int[SELECT_BUFSIZE];

		public delegate void dotextures();
		public event dotextures ondotextures;

		
    public static class Render
    {
        public static IRendering Plugin;
    }
		
		
 public struct FaceData
    {
        public float[] Vertices;
        public ushort[] Indices;
        public float[] TexCoords;
        public int TexturePointer;
        public System.Drawing.Image Texture;
        // TODO: Normals / binormals?
    }		
		
		public OpenGL()
		{
			 Thread loginRunner = new Thread(new ThreadStart(this.run));
             loginRunner.Start();	
		}

		void run()			
		{
			
			List<string> renderers = RenderingLoader.ListRenderers(System.AppDomain.CurrentDomain.BaseDirectory);			
			Render.Plugin = RenderingLoader.LoadRenderer(renderers[0]);
			MainGL();
		}
				
        public void SampleDisplay()
        {		
			
            Gl.glClearColor(0f, 0.5f, 1f, 1f);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
			RenderScene();
            Glut.glutSwapBuffers();
		}

        void SampleIdle()
		{ 
	
			if(this.ondotextures!=null)
			{
				this.ondotextures();
				this.ondotextures=null;
			}
			
			Gl.glClearColor(0f, 0.5f, 1f, 1f);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
			RenderScene();
            Glut.glutSwapBuffers();			
			}

        void SampleReshape(int nWidth, int nHeight)
        {						

			Gl.glClearColor(0.39f, 0.58f, 0.93f, 1.0f);

            Gl.glViewport(0, 0,  nWidth, nHeight);

            Gl.glPushMatrix();
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();

            SetPerspective();

            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glPopMatrix();

            // Set the center of the glControl as the default pivot point
           // LastPivot = glControl.PointToScreen(new Point(nWidth/2, nHeight/2));
           LastPivot = new Point(nWidth/2, nHeight/2);
				
			
		}

        void SampleKeyboard(byte cChar, int nMouseX, int nMouseY)
        {
			Console.WriteLine("key "+cChar.ToString());
        }

        void keyup(byte cChar, int x, int y)
        {

            Console.WriteLine("up key " + cChar.ToString());

        }

        void specialkeyup(int key, int x, int y)
        {

            Console.WriteLine("special up key " + key.ToString());

            switch (key)
            {
                case 101:
                    MainClass.client.Self.Movement.AtPos = false;
                    break;
                case 102:
                    MainClass.client.Self.Movement.TurnLeft = false;
                    break;
                case 103:
                    MainClass.client.Self.Movement.AtNeg = false;
                    break;
                case 104:
                    MainClass.client.Self.Movement.TurnRight = false;
                    break;
                default:
                    break;

            }

            MainClass.client.Self.Movement.SendUpdate();
   
        }

        void SampleSpecial(int nSpecial, int nMouseX, int nMouseY)
		{
            Console.WriteLine("Special key "+nSpecial.ToString());
			specialkey=nSpecial;

            switch (nSpecial)
            {
                case 101:
                    MainClass.client.Self.Movement.AtPos = true;
                    break;
                case 102: 
                    MainClass.client.Self.Movement.TurnLeft = true;
                    break;
                case 103:
                    MainClass.client.Self.Movement.AtNeg = true;
                    break;
                case 104:
                    MainClass.client.Self.Movement.TurnRight = true;
                    break;
                default:
                    break;
            }

            MainClass.client.Self.Movement.SendUpdate();
        
	    }
		
		void zoom(float Delta)
			{
			
           if (Delta != 0)
            {
                // Calculate the distance to move to/away
                float dist = (float)(Delta / 120) * 10.0f;
				
				Console.WriteLine("Zoom");

                if (Vector3.Distance(Camera.Position, Camera.FocalPoint) > dist)
                {
                    // Move closer or further away from the focal point
                    Vector3 toFocal = Camera.FocalPoint - Camera.Position;
                    toFocal.Normalize();

                    toFocal = toFocal * dist;

                    Camera.Position += toFocal;
                    UpdateCamera();
                }
            }
 			
		
}		

		void MouseCallback(int button, int state,int x, int y)
		{
            Console.WriteLine("Mouse callback "+button.ToString()+" at "+x.ToString()+":"+y.ToString());
						
			if ((specialkey==Glut.GLUT_KEY_F1))
            {
                // Alt is held down and we have a valid target
                Pivoting = true;
                PivotPosition = Camera.FocalPoint;

                LastPivot = new Point(x, y);
            }		
	}
		
		void MotionCallback(int X,int Y)
			{
         Console.WriteLine("Moting callback at "+X.ToString()+":"+Y.ToString());
	
            if (Pivoting)
            {
                float a,x,y,z;

                Point mouse = new Point(X, Y);

                // Calculate the deltas from the center of the control to the current position
                int deltaX = (int)((mouse.X - LastPivot.X) * -0.5d);
				int deltaY = (int)((mouse.Y - LastPivot.Y) * -0.5d);

Console.WriteLine("Motion callback");
                // Translate so the focal point is the origin
                Vector3 altered = Camera.Position - Camera.FocalPoint;

                // Rotate the translated point by deltaX
                a = (float)deltaX * DEG_TO_RAD;
                x = (float)((altered.X * Math.Cos(a)) - (altered.Y * Math.Sin(a)));
                y = (float)((altered.X * Math.Sin(a)) + (altered.Y * Math.Cos(a)));

                altered.X = x;
                altered.Y = y;

                // Rotate the translated point by deltaY
                a = (float)deltaY * DEG_TO_RAD;
                y = (float)((altered.Y * Math.Cos(a)) - (altered.Z * Math.Sin(a)));
                z = (float)((altered.Y * Math.Sin(a)) + (altered.Z * Math.Cos(a)));

                altered.Y = y;
                altered.Z = z;

                // Translate back to world space
                altered += Camera.FocalPoint;

                // Update the camera
                Camera.Position = altered;
                UpdateCamera();

                // Update the pivot point
                LastPivot = mouse;
            }
       }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public void MainGL()
		{
			int menuID, subMenuA, subMenuB;
		
            Glut.glutInitDisplayString("rgb double samples");
			Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH);
            Glut.glutInitWindowPosition(100, 100);


			Glut.glutInit();
            Glut.glutSetOption(Glut.GLUT_ACTION_ON_WINDOW_CLOSE, Glut.GLUT_ACTION_CONTINUE_EXECUTION);
  	
			winno=Glut.glutCreateWindow("3D Browser");
			Glut.glutWMCloseFunc(new Glut.WindowCloseCallback(this.close));
            Glut.glutCloseFunc(new Glut.CloseCallback(this.close2));
         
            Glut.glutDisplayFunc(new Glut.DisplayCallback(this.SampleDisplay));
			Glut.glutReshapeFunc(new Glut.ReshapeCallback(this.SampleReshape));
			Glut.glutKeyboardFunc(new Glut.KeyboardCallback(this.SampleKeyboard));
			Glut.glutSpecialFunc(new Glut.SpecialCallback(this.SampleSpecial));
            Glut.glutKeyboardUpFunc(new Glut.KeyboardUpCallback(this.keyup));
            Glut.glutSpecialUpFunc(new Glut.SpecialUpCallback(this.specialkeyup));

			Glut.glutIdleFunc(new Glut.IdleCallback(this.SampleIdle));
			Glut.glutMouseFunc(new Glut.MouseCallback(this.MouseCallback));			
			Glut.glutMotionFunc(new Glut.MotionCallback(this.MotionCallback));
			
	        InitializeObjects();
            InitHeightmap(0);
			InitOpenGL();
            InitCamera();			


	        try
            {
			 	Glut.glutMainLoop();
                Console.WriteLine("Main Loop exit");
			}
			catch
			{
			}
            Console.WriteLine("glutMainLoop() termination works fine!\n");

            if (this.TextureDownloader != null)
                this.TextureDownloader.Shutdown();
	}
		
			private void close2()
			{
                Console.WriteLine("Close2");
			    abort=true;
              //  Glut.glutDestroyWindow(winno);
               
            }
            private void close()
            {
                Console.WriteLine("Close");
                abort = true;
            }

	
       private void InitLists()
			
        {
            TotalPrims = 0;

            lock (Textures)
            {
                foreach (TextureInfo tex in Textures.Values)
                {
                    int id = tex.ID;
                    Gl.glDeleteTextures(1, ref id);
                }

                Textures.Clear();
            }

            lock (RenderPrimList) RenderPrimList.Clear();
            lock (RenderFoliageList) RenderFoliageList.Clear();
        }

        private void InitializeObjects()
        {
            InitLists();
            // Initialize the SL MainClass.client

			MainClass.client.Settings.ALWAYS_DECODE_OBJECTS = true;
            MainClass.client.Settings.ALWAYS_REQUEST_OBJECTS = true;

            MainClass.client.Network.OnCurrentSimChanged += new NetworkManager.CurrentSimChangedCallback(Network_OnCurrentSimChanged);
            MainClass.client.Network.OnEventQueueRunning += new NetworkManager.EventQueueRunningCallback(Network_OnEventQueueRunning);
            //MainClass.client.Objects.OnNewPrim += new ObjectManager.NewPrimCallback(Objects_OnNewPrim);
            //MainClass.client.Objects.OnNewFoliage += new ObjectManager.NewFoliageCallback(Objects_OnNewFoliage);
            //MainClass.client.Objects.OnObjectKilled += new ObjectManager.KillObjectCallback(Objects_OnObjectKilled);
			MainClass.client.Terrain.OnLandPatch += new TerrainManager.LandPatchCallback(Terrain_OnLandPatch);
		//	MainClass.client.Parcels.OnSimParcelsDownloaded += new ParcelManager.SimParcelsDownloaded(Parcels_OnSimParcelsDownloaded);
            //MainClass.client.Objects.OnObjectUpdated += new OpenMetaverse.ObjectManager.ObjectUpdatedCallback(onUpdate);

            // Initialize the texture download pipeline
            if (TextureDownloader != null)
                TextureDownloader.Shutdown();
            TextureDownloader = new TexturePipeline(MainClass.client);
            TextureDownloader.OnDownloadFinished += new TexturePipeline.DownloadFinishedCallback(TextureDownloader_OnDownloadFinished);
            TextureDownloader.OnDownloadProgress += new TexturePipeline.DownloadProgressCallback(TextureDownloader_OnDownloadProgress);

            // Initialize the camera object
            InitCamera();

            // Setup the libsl camera to match our Camera struct
            UpdateCamera();
	}
		
			void onUpdate(Simulator simulator, ObjectUpdate update, ulong regionHandle, ushort timeDilation)
			{
				if (update.LocalID == MainClass.client.Self.LocalID)
				{
		//		Vector3 far;
      //          Vector3 displacment;

    //            displacment = MainClass.client.Self.RelativePosition;
  //              displacment.X +=40;

//                displacment = (displacment - MainClass.client.Self.RelativePosition);

            //    Quaternion rot = MainClass.client.Self.RelativeRotation;
            //    displacment = displacment * rot;

            //    far = MainClass.client.Self.RelativePosition + displacment;

             //   this.Camera.Position=MainClass.client.Self.RelativePosition;
             //   this.Camera.FocalPoint=far;

               // Console.WriteLine("Position is now " + MainClass.client.Self.RelativePosition.ToString() + " rotation is " + MainClass.client.Self.RelativeRotation.ToString() + " Camera target is " + far.ToString());
                }
                       
             }

        private void InitOpenGL()
        {
            Gl.glShadeModel(Gl.GL_SMOOTH);

            Gl.glClearDepth(1.0f);
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glDepthMask(true);
            Gl.glDepthFunc(Gl.GL_LEQUAL);
            Gl.glHint(Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST);
        }

        void InitHeightmap(int xxx)
        {
            // Initialize the heightmap
            Heightmap = new OpenMetaverse.TerrainPatch[16, 16];
            for (int y = 0; y < 16; y++)
            {
                for (int x = 0; x < 16; x++)
                {
                    Heightmap[y, x] = new OpenMetaverse.TerrainPatch();
                    Heightmap[y, x].Data = new float[16 * 16];
                }
            }

            // Speed up terrain exports with a lookup table
            LookupHeightTable = new HeightmapLookupValue[256 * 256];
            for (int i = 0; i < 256; i++)
            {
                for (int j = 0; j < 256; j++)
                {
                    LookupHeightTable[i + (j * 256)] = new HeightmapLookupValue(i + (j * 256), ((float)i * ((float)j / 127.0f)));
                }
            }
            Array.Sort<HeightmapLookupValue>(LookupHeightTable);
        }

			private void InitCamera()
        {
            Console.WriteLine("Init camera");
            Camera = new sCamera();
            Camera.Position = new Vector3(128f, -192f, 90f);
            Camera.FocalPoint = new Vector3(128f, 128f, 0f);
            Camera.Zoom = 1.0d;
            Camera.Far = 512.0d;
        }

        private void UpdateCamera()
        {
            if (MainClass.client != null)
            {
                MainClass.client.Self.Movement.Camera.LookAt(Camera.Position, Camera.FocalPoint);
                MainClass.client.Self.Movement.Camera.Far = (float)Camera.Far;
            }

            Gl.glPushMatrix();
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();

            SetPerspective();

            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glPopMatrix();
        }
		
        private void SetPerspective()
        {
            Glu.gluPerspective(50.0d * Camera.Zoom, 1.0d, 0.1d, Camera.Far);
        }

        private void StartPicking(int cursorX, int cursorY)
        {
            int[] viewport = new int[4];

            Gl.glSelectBuffer(SELECT_BUFSIZE, SelectBuffer);
            Gl.glRenderMode(Gl.GL_SELECT);

            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glPushMatrix();
            Gl.glLoadIdentity();

            Gl.glGetIntegerv(Gl.GL_VIEWPORT, viewport);
            Glu.gluPickMatrix(cursorX, viewport[3] - cursorY, 5, 5, viewport);

            SetPerspective();

            Gl.glMatrixMode(Gl.GL_MODELVIEW);

            Gl.glInitNames();
        }

        private void StopPicking()
        {
            int hits;

            // Resotre the original projection matrix
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glPopMatrix();
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glFlush();

            // Return to normal rendering mode
            hits = Gl.glRenderMode(Gl.GL_RENDER);

            // If there are hits process them
            if (hits != 0)
            {
                ProcessHits(hits, SelectBuffer);
            }
            else
            {
                LastHit = 0;
                //glControl.ContextMenu = null;
            }
        }

        private void ProcessHits(int hits, int[] selectBuffer)
        {
            uint names = 0;
            uint numNames = 0;
            uint minZ = 0xffffffff;
            uint ptr = 0;
            uint ptrNames = 0;

            for (int i = 0; i < hits; i++)
            {
                names = (uint)selectBuffer[ptr];
                ++ptr;
                if (selectBuffer[ptr] < minZ)
                {
                    numNames = (uint)names;
                    minZ = (uint)selectBuffer[ptr];
                    ptrNames = ptr + 2;
                }

                ptr += names + 2;
            }

            ptr = ptrNames;

            for (uint i = 0; i < numNames; i++, ptr++)
            {
                LastHit = (uint)selectBuffer[ptr];
            }

            if (LastHit >= TERRAIN_START)
            {
                // Terrain was clicked on, turn off the context menu
                //glControl.ContextMenu = ExportTerrainMenu;
            }
            else
            {
                RenderablePrim render;
                if (RenderPrimList.TryGetValue(LastHit, out render))
                {
                    if (render.Prim.ParentID == 0)
                    {
                        Camera.FocalPoint = render.Prim.Position;
                        UpdateCamera();
                    }
                    else
                    {
                        // See if we have the parent
                        RenderablePrim renderParent;
                        if (RenderPrimList.TryGetValue(render.Prim.ParentID, out renderParent))
                        {
                            // Turn on the context menu
                            //glControl.ContextMenu = ExportPrimMenu;

                            // Change the clicked on prim to the parent. Camera position stays on the
                            // clicked child but the highlighting is applied to all the children
                            LastHit = renderParent.Prim.LocalID;

                            Camera.FocalPoint = renderParent.Prim.Position + render.Prim.Position;
                            UpdateCamera();
                        }
                        else
                        {
                            Console.WriteLine("Clicked on a child prim with no parent!");
                            LastHit = 0;
                        }
                    }
                }
            }
        }

			private void Objects_OnNewPrim(Simulator simulator, Primitive prim, ulong regionHandle, ushort timeDilation)
			{

			//Console.WriteLine("New prim");
			
            RenderablePrim render = new RenderablePrim();
            render.Prim = prim;
            render.Mesh = Render.Plugin.GenerateFacetedMesh(prim, DetailLevel.Highest);

            // Create a FaceData struct for each face that stores the 3D data
            // in a Tao.OpenGL friendly format
            for (int j = 0; j < render.Mesh.Faces.Count; j++)
            {
                Face face = render.Mesh.Faces[j];
                FaceData data = new FaceData();

                // Vertices for this face
                data.Vertices = new float[face.Vertices.Count * 3];
                for (int k = 0; k < face.Vertices.Count; k++)
                {
                    data.Vertices[k * 3 + 0] = face.Vertices[k].Position.X;
                    data.Vertices[k * 3 + 1] = face.Vertices[k].Position.Y;
                    data.Vertices[k * 3 + 2] = face.Vertices[k].Position.Z;
                }

                // Indices for this face
                data.Indices = face.Indices.ToArray();

                // Texture transform for this face
                Primitive.TextureEntryFace teFace = prim.Textures.GetFace((uint)j);
                Render.Plugin.TransformTexCoords(face.Vertices, face.Center, teFace);

                // Texcoords for this face
                data.TexCoords = new float[face.Vertices.Count * 2];
                for (int k = 0; k < face.Vertices.Count; k++)
                {
                    data.TexCoords[k * 2 + 0] = face.Vertices[k].TexCoord.X;
                    data.TexCoords[k * 2 + 1] = face.Vertices[k].TexCoord.Y;
                }

                // Texture for this face
                if (teFace.TextureID != UUID.Zero &&
                    teFace.TextureID != Primitive.TextureEntry.WHITE_TEXTURE)
                { 
                    lock (Textures)
                    {
                        if (!Textures.ContainsKey(teFace.TextureID))
                        {
                            // We haven't constructed this image in OpenGL yet, get ahold of it
                            TextureDownloader.RequestTexture(teFace.TextureID);
                        }
                    }
                }

                // Set the UserData for this face to our FaceData struct
                face.UserData = data;
                render.Mesh.Faces[j] = face;
            }

            lock (RenderPrimList) RenderPrimList[prim.LocalID] = render;
        }

        private void Objects_OnNewFoliage(Simulator simulator, Primitive foliage, ulong regionHandle, ushort timeDilation)
        {
            lock (RenderFoliageList)
                RenderFoliageList[foliage.LocalID] = foliage;
        }

        private void Objects_OnObjectKilled(Simulator simulator, uint objectID)
        {
            
        }

        private void Parcels_OnSimParcelsDownloaded(Simulator simulator, InternalDictionary<int, Parcel> simParcels, int[,] parcelMap)
        {
            TotalPrims = 0;

            simParcels.ForEach(
                delegate(Parcel parcel)
                {
                    TotalPrims += parcel.TotalPrims;
                });
        }

			private void Terrain_OnLandPatch(Simulator simulator, int x, int y, int width, float[] data)
			{

           // Console.WriteLine("OnLandPatch");
            if (MainClass.client != null && MainClass.client.Network.CurrentSim == simulator)
            {
                Heightmap[y, x].Data = data;
            }

            // Find the new max height
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] > MaxHeight)
                    MaxHeight = data[i];
            }
        }

        private void Network_OnCurrentSimChanged(Simulator PreviousSimulator)
        {
            Console.WriteLine("CurrentSim set to " + MainClass.client.Network.CurrentSim + ", downloading parcel information");

            InitHeightmap(0);
            InitLists();     
        }

        private void Network_OnEventQueueRunning(Simulator simulator)
        {
          // Now seems like a good time to start requesting parcel information
			MainClass.client.Parcels.RequestAllSimParcels(MainClass.client.Network.CurrentSim, false, 100); 
        }

        private void RenderScene()
        {
            try
            {
                Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
                Gl.glLoadIdentity();
                Gl.glEnableClientState(Gl.GL_VERTEX_ARRAY);
                Gl.glEnableClientState(Gl.GL_TEXTURE_COORD_ARRAY);

//                if (Clicked)
//                    StartPicking(ClickX, ClickY);

                // Setup wireframe or solid fill drawing mode
                Gl.glPolygonMode(Gl.GL_FRONT, Gl.GL_LINE);

                // Position the camera
                Glu.gluLookAt(
                    Camera.Position.X, Camera.Position.Y, Camera.Position.Z,
                    Camera.FocalPoint.X, Camera.FocalPoint.Y, Camera.FocalPoint.Z,
                    0f, 0f, 1f);

                RenderSkybox();

                // Push the world matrix
                Gl.glPushMatrix();

                RenderTerrain();
              //  RenderPrims();
                //RenderAvatars();

                Gl.glDisableClientState(Gl.GL_TEXTURE_COORD_ARRAY);
                Gl.glDisableClientState(Gl.GL_VERTEX_ARRAY);

//                if (Clicked)
//                {
//                    Clicked = false;
//                    StopPicking();
//                }

                // Pop the world matrix
                Gl.glPopMatrix();
                Gl.glFlush();

//                glControl.Invalidate();
            }
            catch (Exception)
            {
            }
        }

        static readonly Vector3[] SkyboxVerts = new Vector3[]
        {
	        // Right side
	        new Vector3(	 10.0f,		10.0f,		-10.0f	), //Top left
	        new Vector3(	 10.0f,		10.0f,		10.0f	), //Top right
	        new Vector3(	 10.0f,		-10.0f,		10.0f	), //Bottom right
	        new Vector3(	 10.0f,		-10.0f,		-10.0f	), //Bottom left
	        // Left side
	        new Vector3(	-10.0f,		10.0f,		10.0f	), //Top left
	        new Vector3(	-10.0f,		10.0f,		-10.0f	), //Top right
	        new Vector3(	-10.0f,		-10.0f,		-10.0f	), //Bottom right
	        new Vector3(	-10.0f,		-10.0f,		10.0f	), //Bottom left
	        // Top side
	        new Vector3(	-10.0f,		10.0f,		10.0f	), //Top left
	        new Vector3(	 10.0f,		10.0f,		10.0f	), //Top right
	        new Vector3(	 10.0f,		10.0f,		-10.0f	), //Bottom right
	        new Vector3(	-10.0f,		10.0f,		-10.0f	), //Bottom left
	        // Bottom side
	        new Vector3(	-10.0f,		-10.0f,		-10.0f	), //Top left
	        new Vector3(	 10.0f,		-10.0f,		-10.0f	), //Top right
	        new Vector3(	 10.0f,		-10.0f,		10.0f	), //Bottom right
	        new Vector3(	-10.0f,		-10.0f,		10.0f	), //Bottom left
	        // Front side
	        new Vector3(	-10.0f,		10.0f,		-10.0f	), //Top left
	        new Vector3(	 10.0f,		10.0f,		-10.0f	), //Top right
	        new Vector3(	 10.0f,		-10.0f,		-10.0f	), //Bottom right
	        new Vector3(	-10.0f,		-10.0f,		-10.0f	), //Bottom left
	        // Back side
	        new Vector3(	10.0f,		10.0f,		10.0f	), //Top left
	        new Vector3(	-10.0f,		10.0f,		10.0f	), //Top right
	        new Vector3(	-10.0f,		-10.0f,		10.0f	), //Bottom right
	        new Vector3(	 10.0f,		-10.0f,		10.0f	), //Bottom left
        };

        private void RenderSkybox()
        {
            //Gl.glTranslatef(0f, 0f, 0f);
        }

        private void RenderTerrain()
        {
            if (Heightmap != null)
            {
				int i = 0;
				
				Gl.glEnable(Gl.GL_TEXTURE_2D);

				    UUID id=MainClass.client.Network.CurrentSim.TerrainDetail1;
					if(Textures.ContainsKey(id))
					{
                        Gl.glBindTexture(Gl.GL_TEXTURE_2D, Textures[id].ID);
					}
					else
					{
                        //Console.WriteLine("No texture this time");
				        Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
				        // No texture
			        }
				// select modulate to mix texture with color for shading
				Gl.glTexEnvf( Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_MODULATE );

				// when texture area is small, bilinear filter the closest mipmap
				Gl.glTexParameterf( Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER,
				                 Gl.GL_LINEAR_MIPMAP_NEAREST );
				// when texture area is large, bilinear filter the original
				Gl.glTexParameterf( Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR );

				// the texture wraps over at the edges (repeat)
			    Gl.glTexParameterf( Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_TEXTURE_WRAP_S );
				Gl.glTexParameterf( Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_TEXTURE_WRAP_T);
//
				
				float height;
				int lxx,lyy;
				float red;
                bool selected=false;
                float color;

				Gl.glBegin(Gl.GL_TRIANGLE_STRIP);
				
				
                for (int yy = 0; yy < 16*15; yy++)
				{
					
						for (int xx = 0; xx < 16*15; xx++)
						{
                         lxx=xx;
						 lyy=yy;
						
						
						height= Heightmap[(int)lyy/16, (int)lxx/16].Data[(lyy%16) * 16 + (lxx%16)];
                        color = height / (MaxHeight/2.0f);
                        red = (selected) ? 1f : color;
                        Gl.glColor3f(red, color, color);
                        Gl.glTexCoord2f(0f, 0f);
						Gl.glVertex3f(lxx, lyy, height);
						lxx++;
						
						height= Heightmap[(int)lyy/16, (int)lxx/16].Data[(lyy%16) * 16 + (lxx%16)];
                        color = height / (MaxHeight/2.0f);
                        red = (selected) ? 1f : color;
                        Gl.glColor3f(red, color, color);
                        Gl.glTexCoord2f(1f, 0f);
						Gl.glVertex3f(lxx, lyy, height);
                        lyy++;
                        lxx--;
						
						height= Heightmap[(int)lyy/16, (int)lxx/16].Data[(lyy%16) * 16 + (lxx%16)];
                        color = height / (MaxHeight/2.0f);
                        red = (selected) ? 1f : color;
                        Gl.glColor3f(red, color, color);	
                        Gl.glTexCoord2f(1f, 1f);
						Gl.glVertex3f(lxx, lyy, height);
						lyy--;
						lxx++;
						
						height= Heightmap[(int)lyy/16, (int)lxx/16].Data[(lyy%16) * 16 + (lxx%16)];
                        color = height / (MaxHeight/2.0f);
                        red = (selected) ? 1f : color;
                        Gl.glTexCoord2f(0f, 1f);
						Gl.glVertex3f(lxx, lyy, height);
      
                }

			}
				
    Gl.glEnd();

             }
        }

        int[] CubeMapDefines = new int[]
        {
            Gl.GL_TEXTURE_CUBE_MAP_POSITIVE_X_ARB,
            Gl.GL_TEXTURE_CUBE_MAP_NEGATIVE_X_ARB,
            Gl.GL_TEXTURE_CUBE_MAP_POSITIVE_Y_ARB,
            Gl.GL_TEXTURE_CUBE_MAP_NEGATIVE_Y_ARB,
            Gl.GL_TEXTURE_CUBE_MAP_POSITIVE_Z_ARB,
            Gl.GL_TEXTURE_CUBE_MAP_NEGATIVE_Z_ARB
        };

        private void RenderPrims()
        {
            if (RenderPrimList != null && RenderPrimList.Count > 0)
            {
                Gl.glEnable(Gl.GL_TEXTURE_2D);

                lock (RenderPrimList)
                {
                    bool firstPass = true;
                    Gl.glDisable(Gl.GL_BLEND);
                    Gl.glEnable(Gl.GL_DEPTH_TEST);

StartRender:

                    foreach (RenderablePrim render in RenderPrimList.Values)
                    {
                        RenderablePrim parentRender = RenderablePrim.Empty;
                        Primitive prim = render.Prim;

                        if (prim.ParentID != 0)
                        {
                            // Get the parent reference
                            if (!RenderPrimList.TryGetValue(prim.ParentID, out parentRender))
                            {
                                // Can't render a child with no parent prim, skip it
                                continue;
                            }
                        }

                        Gl.glPushName((int)prim.LocalID);
                        Gl.glPushMatrix();

                        if (prim.ParentID != 0)
                        {
                            // Child prim
                            Primitive parent = parentRender.Prim;

                            // Apply parent translation and rotation
                            Gl.glMultMatrixf(Math3D.CreateTranslationMatrix(parent.Position));
                            Gl.glMultMatrixf(Math3D.CreateRotationMatrix(parent.Rotation));
                        }

                        // Apply prim translation and rotation
                        Gl.glMultMatrixf(Math3D.CreateTranslationMatrix(prim.Position));
                        Gl.glMultMatrixf(Math3D.CreateRotationMatrix(prim.Rotation));

                        // Scale the prim
                        Gl.glScalef(prim.Scale.X, prim.Scale.Y, prim.Scale.Z);

                        // Draw the prim faces
                        for (int j = 0; j < render.Mesh.Faces.Count; j++)
                        {
                            Face face = render.Mesh.Faces[j];
                            FaceData data = (FaceData)face.UserData;
                            Color4 color = face.TextureFace.RGBA;
                            bool alpha = false;
                            int textureID = 0;

                            if (color.A < 1.0f)
                                alpha = true;

                            #region Texturing

                            TextureInfo info;
                            if (Textures.TryGetValue(face.TextureFace.TextureID, out info))
                            {
                                if (info.Alpha)
                                    alpha = true;

                                textureID = info.ID;

                                // Enable texturing for this face
                                Gl.glPolygonMode(Gl.GL_FRONT_AND_BACK, Gl.GL_FILL);
                            }
                            else
                            {
                                if (face.TextureFace.TextureID == Primitive.TextureEntry.WHITE_TEXTURE ||
                                    face.TextureFace.TextureID == UUID.Zero)
                                {
                                    Gl.glPolygonMode(Gl.GL_FRONT, Gl.GL_FILL);
                                }
                                else
                                {
                                    Gl.glPolygonMode(Gl.GL_FRONT, Gl.GL_LINE);
                                }
                            }

                            if (firstPass && !alpha || !firstPass && alpha)
                            {
                                // Color this prim differently based on whether it is selected or not
                                if (LastHit == prim.LocalID || (LastHit != 0 && LastHit == prim.ParentID))
                                {
                                    Gl.glColor4f(1f, color.G * 0.3f, color.B * 0.3f, color.A);
                                }
                                else
                                {
                                    Gl.glColor4f(color.R, color.G, color.B, color.A);
                                }

                                // Bind the texture
                                Gl.glBindTexture(Gl.GL_TEXTURE_2D, textureID);

                                Gl.glTexCoordPointer(2, Gl.GL_FLOAT, 0, data.TexCoords);
                                Gl.glVertexPointer(3, Gl.GL_FLOAT, 0, data.Vertices);
                                Gl.glDrawElements(Gl.GL_TRIANGLES, data.Indices.Length, Gl.GL_UNSIGNED_SHORT, data.Indices);
                            }

                            #endregion Texturing
                        }

                        Gl.glPopMatrix();
                        Gl.glPopName();
                    }

                    if (firstPass)
                    {
                        firstPass = false;
                        Gl.glEnable(Gl.GL_BLEND);
                        Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
                        //Gl.glDisable(Gl.GL_DEPTH_TEST);

                        goto StartRender;
                    }
                }

                Gl.glEnable(Gl.GL_DEPTH_TEST);
                Gl.glDisable(Gl.GL_TEXTURE_2D);
            }
        }

        private void RenderAvatars()
        {
            if (MainClass.client != null && MainClass.client.Network.CurrentSim != null)
            {
                Gl.glColor3f(0f, 1f, 0f);

                MainClass.client.Network.CurrentSim.ObjectsAvatars.ForEach(
                    delegate(Avatar avatar)
                    {
                        Gl.glPushMatrix();
                        Gl.glTranslatef(avatar.Position.X, avatar.Position.Y, avatar.Position.Z);

                        Glu.GLUquadric quad = Glu.gluNewQuadric();
                        Glu.gluSphere(quad, 1.0d, 10, 10);
                        Glu.gluDeleteQuadric(quad);

                        Gl.glPopMatrix();
                    }
                );
                
                Gl.glColor3f(1f, 1f, 1f);
            }
        }

        #region Texture Downloading

        private void TextureDownloader_OnDownloadFinished(UUID id, bool success)
        {
            bool alpha = false;
            ManagedImage imgData = null;
            byte[] raw = null;

            try
            {
                // Load the image off the disk
                if (success)
                {
                    ImageDownload download = TextureDownloader.GetTextureToRender(id);
                    if (OpenJPEG.DecodeToImage(download.AssetData, out imgData))
                    {
                        raw = imgData.ExportRaw();

                        if ((imgData.Channels & ManagedImage.ImageChannels.Alpha) != 0)
                            alpha = true;
                    }
                    else
                    {
                        success = false;
                        Console.WriteLine("Failed to decode texture");
                    }
                }

				// Make sure the OpenGL commands run on the main thread
				// Gtk.Application.Invoke(delegate {
					
                         //Yay for invokes
                         this.ondotextures+=delegate
                          {
                           if (success)
                           {
                               int textureID = 0;

                               try
                               {
                                   Gl.glGenTextures(1, out textureID);
                                   Gl.glBindTexture(Gl.GL_TEXTURE_2D, textureID);

                                   Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR_MIPMAP_NEAREST); //Gl.GL_NEAREST);
                                   Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
                                   Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_REPEAT);
                                   Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_REPEAT);
                                   Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_GENERATE_MIPMAP, Gl.GL_TRUE); //Gl.GL_FALSE);

                                   //Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA, bitmap.Width, bitmap.Height, 0, Gl.GL_BGRA, Gl.GL_UNSIGNED_BYTE,
                                   //    bitmapData.Scan0);
                                   //int error = Gl.glGetError();

                                   int error = Glu.gluBuild2DMipmaps(Gl.GL_TEXTURE_2D, Gl.GL_RGBA, imgData.Width, imgData.Height, Gl.GL_BGRA,
                                       Gl.GL_UNSIGNED_BYTE, raw);

                                   if (error == 0)
                                   {
                                       Textures[id] = new TextureInfo(textureID, alpha);
                                       Console.WriteLine("Created OpenGL texture for " + id.ToString());
                                   }
                                   else
                                   {
                                       Textures[id] = new TextureInfo(0, false);
                                       Console.WriteLine("Error creating OpenGL texture: " + error);
                                   }
                               }
                               catch (Exception ex)
							   {
                                   Console.WriteLine(ex);
				               }
                           }
					};
/*
                           // Remove this image from the download listbox
                           lock (DownloadList)
                           {
                               GlacialComponents.Controls.GLItem item;
                               if (DownloadList.TryGetValue(id, out item))
                               {
                                   DownloadList.Remove(id);
                                   try { lstDownloads.Items.Remove(item); }
                                   catch (Exception) { }
                                   lstDownloads.Invalidate();
                               }
                           }
	*/				
					
//                       });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void TextureDownloader_OnDownloadProgress(UUID image, int recieved, int total)
		{

        }

        #endregion Texture Downloading

  
/*
        private void glControl_MouseClick(object sender, MouseEventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Alt) == 0 && e.Button == MouseButtons.Left)
            {
                // Only allow clicking if alt is not being held down
                ClickX = e.X;
                ClickY = e.Y;
                Clicked = true;
            }
        }
*/
		
/*
        private void glControl_MouseDown(object sender, MouseEventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Alt) != 0 && LastHit > 0)
            {
                // Alt is held down and we have a valid target
                Pivoting = true;
                PivotPosition = Camera.FocalPoint;

                Control control = (Control)sender;
                LastPivot = control.PointToScreen(new Point(e.X, e.Y));
            }
        }
		*/
/*
        private void glControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (Pivoting)
            {
                float a,x,y,z;

                Control control = (Control)sender;
                Point mouse = control.PointToScreen(new Point(e.X, e.Y));

                // Calculate the deltas from the center of the control to the current position
                int deltaX = (int)((mouse.X - LastPivot.X) * -0.5d);
                int deltaY = (int)((mouse.Y - LastPivot.Y) * -0.5d);

                // Translate so the focal point is the origin
                Vector3 altered = Camera.Position - Camera.FocalPoint;

                // Rotate the translated point by deltaX
                a = (float)deltaX * DEG_TO_RAD;
                x = (float)((altered.X * Math.Cos(a)) - (altered.Y * Math.Sin(a)));
                y = (float)((altered.X * Math.Sin(a)) + (altered.Y * Math.Cos(a)));

                altered.X = x;
                altered.Y = y;

                // Rotate the translated point by deltaY
                a = (float)deltaY * DEG_TO_RAD;
                y = (float)((altered.Y * Math.Cos(a)) - (altered.Z * Math.Sin(a)));
                z = (float)((altered.Y * Math.Sin(a)) + (altered.Z * Math.Cos(a)));

                altered.Y = y;
                altered.Z = z;

                // Translate back to world space
                altered += Camera.FocalPoint;

                // Update the camera
                Camera.Position = altered;
                UpdateCamera();

                // Update the pivot point
                LastPivot = mouse;
            }
        }
		*/
		
/*
        private void glControl_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta != 0)
            {
                // Calculate the distance to move to/away
                float dist = (float)(e.Delta / 120) * 10.0f;

                if (Vector3.Distance(Camera.Position, Camera.FocalPoint) > dist)
                {
                    // Move closer or further away from the focal point
                    Vector3 toFocal = Camera.FocalPoint - Camera.Position;
                    toFocal.Normalize();

                    toFocal = toFocal * dist;

                    Camera.Position += toFocal;
                    UpdateCamera();
                }
            }
        }
		*/
		
/*
        private void glControl_MouseUp(object sender, MouseEventArgs e)
        {
            // Stop pivoting if we were previously
            Pivoting = false;
        }
*/
        private void txtLogin_Enter(object sender, EventArgs e)
        {
            TextBox input = (TextBox)sender;
            input.SelectAll();
	}

    public struct TextureInfo
    {
        /// <summary>OpenGL Texture ID</summary>
        public int ID;
        /// <summary>True if this texture has an alpha component</summary>
        public bool Alpha;

        public TextureInfo(int id, bool alpha)
        {
            ID = id;
            Alpha = alpha;
        }
    }

    public struct HeightmapLookupValue : IComparable<HeightmapLookupValue>
    {
        public int Index;
        public float Value;

        public HeightmapLookupValue(int index, float value)
        {
            Index = index;
            Value = value;
        }

        public int CompareTo(HeightmapLookupValue val)
        {
            return Value.CompareTo(val.Value);
        }
    }

    public struct RenderablePrim
    {
        public Primitive Prim;
        public FacetedMesh Mesh;

        public readonly static RenderablePrim Empty = new RenderablePrim();
    }

    public struct sCamera
    {
        public Vector3 Position;
        public Vector3 FocalPoint;
        public double Zoom;
        public double Far;
    }
	
    public static class Math3D
    {
        // Column-major:
        // |  0  4  8 12 |
        // |  1  5  9 13 |
        // |  2  6 10 14 |
        // |  3  7 11 15 |

				public static float[] CreateTranslationMatrix(Vector3 v)
        {
            float[] mat = new float[16];

            mat[12] = v.X;
            mat[13] = v.Y;
            mat[14] = v.Z;
            mat[0] = mat[5] = mat[10] = mat[15] = 1;

            return mat;
        }

        public static float[] CreateRotationMatrix(Quaternion q)
        {
            float[] mat = new float[16];

            // Transpose the quaternion (don't ask me why)
            q.X = q.X * -1f;
            q.Y = q.Y * -1f;
            q.Z = q.Z * -1f;

            float x2 = q.X + q.X;
            float y2 = q.Y + q.Y;
            float z2 = q.Z + q.Z;
            float xx = q.X * x2;
            float xy = q.X * y2;
            float xz = q.X * z2;
            float yy = q.Y * y2;
            float yz = q.Y * z2;
            float zz = q.Z * z2;
            float wx = q.W * x2;
            float wy = q.W * y2;
            float wz = q.W * z2;

            mat[0] = 1.0f - (yy + zz);
            mat[1] = xy - wz;
            mat[2] = xz + wy;
            mat[3] = 0.0f;

            mat[4] = xy + wz;
            mat[5] = 1.0f - (xx + zz);
            mat[6] = yz - wx;
            mat[7] = 0.0f;

            mat[8] = xz - wy;
            mat[9] = yz + wx;
            mat[10] = 1.0f - (xx + yy);
            mat[11] = 0.0f;

            mat[12] = 0.0f;
            mat[13] = 0.0f;
            mat[14] = 0.0f;
            mat[15] = 1.0f;

            return mat;
        }

				public static float[] CreateScaleMatrix(Vector3 v)
        {
            float[] mat = new float[16];

            mat[0] = v.X;
            mat[5] = v.Y;
            mat[10] = v.Z;
            mat[15] = 1;

            return mat;
        }
		
		
}
}
}
