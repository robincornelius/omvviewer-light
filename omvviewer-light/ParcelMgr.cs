// ParcelMgr.cs created with MonoDevelop
// User: robin at 15:51 18/10/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using OpenMetaverse;
using System.Collections.Generic;
using System.Text;
using Gtk;

namespace omvviewerlight
{
	
	public partial class ParcelMgr : Gtk.Bin
	{
		
		Gtk.TreeStore parcels_store;
		Gtk.TreeStore parcels_access;
		Gtk.TreeStore parcels_ban;
		Gtk.TreeStore parcel_prim_owners;
        Gdk.Pixbuf parcel_map;
		
		TryGetImage getter;
		
		int sequence=0;

		Dictionary <int,Parcel> simParcelsDict=new Dictionary <int,Parcel>();
		Dictionary <uint,uint> colmaptoid=new Dictionary<uint,uint>(); // map parcelid->colindex
        uint[] colmap = { 0x00FFFFFF, 0xFF00FFFF,0xFFFF00FF,0x55FFFFFF,0xaaFFFFFF,0xFF55FFFF,0xFFaaFFFF,0xFFFF55FF,0xFFFFaaFF,0xFFFFFFFF, 0x0000FFFF, 0xFF0000FF,0x00FF00FF, 0x5555FFFF, 0xFF5555FF,0x55FF55FF, 0xaaaaFFFF, 0xFFaaaaFF,0xaaFFaaFF};
   	    int nextcol=0;

		public ParcelMgr()
		{

			this.Build();
			MainClass.client.Parcels.OnParcelInfo += new OpenMetaverse.ParcelManager.ParcelInfoCallback(onParcelInfo);
			MainClass.client.Network.OnCurrentSimChanged += new OpenMetaverse.NetworkManager.CurrentSimChangedCallback(onNewSim);
			MainClass.client.Parcels.OnSimParcelsDownloaded += new OpenMetaverse.ParcelManager.SimParcelsDownloaded(onParcelsDownloaded);
			MainClass.client.Parcels.OnParcelProperties += new OpenMetaverse.ParcelManager.ParcelPropertiesCallback(onParcelProperties);
			MainClass.client.Parcels.OnPrimOwnersListReply += new OpenMetaverse.ParcelManager.ParcelObjectOwnersListReplyCallback(onParcelObjectOwners);
			
			parcels_store=new Gtk.TreeStore (typeof(Gdk.Pixbuf),typeof(string),typeof(string),typeof(string),typeof(string),typeof(Parcel),typeof(int));
			parcels_access=new Gtk.TreeStore(typeof(string),typeof(UUID));
			parcels_ban=new Gtk.TreeStore(typeof(string),typeof(UUID));
			this.parcel_prim_owners=new Gtk.TreeStore(typeof(string),typeof(string));

			this.treeview_parcels.AppendColumn("Key",new CellRendererPixbuf(),"pixbuf",0);
			this.treeview_parcels.AppendColumn("Parcel",new Gtk.CellRendererText(),"text",1);
			this.treeview_parcels.AppendColumn("Area",new Gtk.CellRendererText(),"text",2);
			this.treeview_parcels.AppendColumn("Traffic",new Gtk.CellRendererText(),"text",3);
			this.treeview_parcels.AppendColumn("For sale",new Gtk.CellRendererText(),"text",4);
			
					
			treeview_parcels.Model=parcels_store;
			this.treeview_access.AppendColumn("Allowed Access",new Gtk.CellRendererText(),"text",0);
			this.treeview_ban.AppendColumn("Banned",new Gtk.CellRendererText(),"text",0);
			this.treeview_access.Model=this.parcels_access;
			this.treeview_ban.Model=this.parcels_ban;
			
			this.treeview_objectlist.AppendColumn("Name",new Gtk.CellRendererText(),"text",0);
			this.treeview_objectlist.AppendColumn("Prims",new Gtk.CellRendererText(),"text",1);
			this.treeview_objectlist.Model=this.parcel_prim_owners;
			
			MainClass.client.Settings.ALWAYS_REQUEST_PARCEL_ACL=true;
			MainClass.client.Settings.ALWAYS_REQUEST_PARCEL_DWELL=true;
			
			this.label_parcelgroup.Text="";
			this.label_parcelowner.Text="";

           // this.parcel_map = new Gdk.Pixbuf("trying.tga");
		//	this.image9.Pixbuf=this.parcel_map;
			
		
		}

		void onParcelObjectOwners(Simulator sim,List <OpenMetaverse.ParcelManager.ParcelPrimOwners> primOwners)
		{
			for(int i = 0; i < primOwners.Count; i++)
			{
				Console.WriteLine(primOwners[i].ToString());        
				Gtk.TreeIter iter2=parcel_prim_owners.AppendValues("Waiting...",primOwners[i].Count.ToString());			
				AsyncNameUpdate ud=new AsyncNameUpdate(primOwners[i].OwnerID,false);  
				ud.addparameters(iter2);
				ud.onNameCallBack += delegate(string namex,object[] values){ Gtk.TreeIter iterx=(Gtk.TreeIter)values[0]; parcel_prim_owners.SetValue(iterx,0,namex);};				
				
			}
						
		}
		
	 unsafe   void onParcelProperties(Simulator Sim,Parcel parcel, ParcelResult result, int selectedprims,int sequenceID, bool snapSelection)
		{		
			
			if(!simParcelsDict.ContainsKey(parcel.LocalID))
			{	
				this.simParcelsDict.Add(parcel.LocalID,parcel);
				int thiscol=0;
				
				if(!colmaptoid.ContainsKey((uint)parcel.LocalID))
				{
						
					colmaptoid.Add((uint)parcel.LocalID,colmap[nextcol]);
					thiscol=nextcol;
					Console.WriteLine("ID "+parcel.LocalID.ToString()+"Setting col to "+nextcol.ToString());
						
					nextcol++;
					if(nextcol>=colmap.Length)
						nextcol=colmap.Length-1;
						
					byte[] data= new byte[4*32*16];
					uint col=colmap[thiscol];
					
					Gdk.Pixbuf pb=new Gdk.Pixbuf("parcelindex.tga");	
					sbyte * ps;		
					ps=(sbyte *)pb.Pixels;
					
					for (int x=0;x<4*32*16;x=x+4)
					{
						
						ps[x+0]=(sbyte)((0xFF000000&col)>>24);
						ps[x+1]=(sbyte)((0x00FF0000&col)>>16);
						ps[x+2]=(sbyte)((0x0000FF00&col)>>8);
						ps[x+3]=(sbyte)((0x000000FF&col)>>0);
					}
					
					string saleinfo;
					if((parcel.Flags & Parcel.ParcelFlags.ForSale) == Parcel.ParcelFlags.ForSale)
					{
						if(parcel.AuthBuyerID!=UUID.Zero)
						{
							saleinfo="Single AV";	
						}
						else
						{
							saleinfo=parcel.SalePrice.ToString();
							
						}
					}
					
					saleinfo="";
					
					parcels_store.AppendValues(pb,parcel.Name,parcel.Area.ToString(),parcel.Dwell.ToString(),saleinfo,parcel,parcel.LocalID);
					
					
					
				}		
				
					
			}			
		}
		
		unsafe void onParcelsDownloaded(Simulator sim,InternalDictionary <int,Parcel> simParcels,int[,] parcelmap)
		{
			
			Console.WriteLine("All Parcels download");
			
		

            uint[,] pm = new uint[64,64];

            int x = 0;
            int y = 0;

			
			
			sbyte * spixels=(sbyte *)this.parcel_map.Pixels;		
			sbyte * ps;			
						
			            uint[] colmap = { 0x00FFFFFF, 0xFF00FFFF,0xFFFF00FF,0x55FFFFFF,0xaaFFFFFF,0xFF55FFFF,0xFFaaFFFF,0xFFFF55FF,0xFFFFaaFF,0xFFFFFFFF, 0x0000FFFF, 0xFF0000FF,0x00FF00FF, 0x5555FFFF, 0xFF5555FF,0x55FF55FF, 0xaaaaFFFF, 0xFFaaaaFF,0xaaFFaaFF};
			int nextcol=0;

			int srcwidth=parcel_map.Width;
			int srcheight=parcel_map.Height;
			int srcrowsstride=parcel_map.Rowstride;
			int schannels=parcel_map.NChannels;
			
			
			
			for(int sx=0;sx<srcwidth;sx++)
			{
				for(int sy=0;sy<srcheight;sy++)
				{	
				 	
				    x=(int)(((double)sx/(double)srcwidth)*64.0);
					y=(int)(((float)sy/(float)srcheight)*64.0);
		
   				    uint id=(uint)parcelmap[x,y];
					uint col;
					
					if(colmaptoid.ContainsKey(id))
					{
						col=colmaptoid[id];
						ps=spixels+(sy*srcrowsstride)+(sx* schannels);
						ps[0]=(sbyte)((0xFF000000&col)>>24);
						ps[1]=(sbyte)((0x00FF0000&col)>>16);
						ps[2]=(sbyte)((0x0000FF00&col)>>8);
						ps[3]=(sbyte)((0x000000FF&col)>>0);
					}
					
				}
			}	

			Gtk.Application.Invoke(delegate {		
				this.image9.QueueDraw();
			});

           
		}

        void onParcelInfo(ParcelInfo pinfo)
		{
		
		}
		
		void onNewSim(Simulator lastsim)
	    {
			this.parcels_store.Clear();
			this.parcels_access.Clear();
			this.parcels_ban.Clear();
			this.colmaptoid.Clear();
			nextcol=0;
			Gtk.Application.Invoke(delegate
            {
				this.parcel_map = new Gdk.Pixbuf("trying.tga");
				this.image9.Pixbuf=this.parcel_map;
			});

			Console.WriteLine("Requesting parcel info for sim:"+MainClass.client.Network.CurrentSim.Name);
			MainClass.client.Parcels.RequestAllSimParcels(MainClass.client.Network.CurrentSim);
		}

		protected virtual void OnTreeviewParcelsCursorChanged (object sender, System.EventArgs e)
		{
			
			Gtk.TreeModel mod;
			Gtk.TreeIter iter;
			
			if(this.treeview_parcels.Selection.GetSelected(out mod,out iter))			
			{
				int id=(int)mod.GetValue(iter,6);
				//this.image_parcel.Pixbuf=new Gdk.Pixbuf(parcel.Bitmap);
				parcels_access.Clear();
				parcels_ban.Clear();
					
				Parcel parcel;
				
				
				if(MainClass.client.Network.CurrentSim.Parcels.TryGetValue(id, out parcel))
				{						
					foreach(OpenMetaverse.ParcelManager.ParcelAccessEntry entry in parcel.AccessWhiteList)
					{
						
						Console.WriteLine(parcel.Flags.ToString());
						
						this.checkbox_nopayment.Active=(OpenMetaverse.Parcel.ParcelFlags.DenyAnonymous==(parcel.Flags& OpenMetaverse.Parcel.ParcelFlags.DenyAnonymous));
						this.checkbutton_noageverify.Active=(OpenMetaverse.Parcel.ParcelFlags.DenyAgeUnverified==(parcel.Flags& OpenMetaverse.Parcel.ParcelFlags.DenyAgeUnverified));
						this.entry_time.Text=parcel.PassHours.ToString();
						this.entry_price.Text=parcel.PassPrice.ToString();
						this.checkbutton_publicaccess.Active=!(OpenMetaverse.Parcel.ParcelFlags.UseAccessList==(parcel.Flags& OpenMetaverse.Parcel.ParcelFlags.UseAccessList));
						//this.checkbutton_sellpasses;
						this.checkbutton_groupaccess.Active=(OpenMetaverse.Parcel.ParcelFlags.UseAccessGroup==(parcel.Flags& OpenMetaverse.Parcel.ParcelFlags.UseAccessGroup));
						
						this.entry_maxprims.Text=parcel.MaxPrims.ToString();
						this.entry_primsgroup.Text=parcel.GroupPrims.ToString();
						this.entry_bonus.Text=parcel.ParcelPrimBonus.ToString();
						this.entry_primsowner.Text=parcel.OwnerPrims.ToString();
						this.entry_primsother.Text=parcel.OtherPrims.ToString();
						this.entry_totalprims.Text=parcel.TotalPrims.ToString();
						
						
						
						if(parcel.SnapshotID!=UUID.Zero)
						{
							if(getter!=null)
								getter.abort();
							
							TryGetImage i = new TryGetImage(this.image_parcelsnap,parcel.SnapshotID,256,256);
							getter=i;
							
						}
					
						AsyncNameUpdate ud;
						
						this.label_parcelowner.Text="Waiting...";
						ud=new AsyncNameUpdate(parcel.OwnerID,false);  
						ud.onNameCallBack += delegate(string namex,object[] values){this.label_parcelowner.Text=namex;};				
											
						this.label_parcelowner.Text="Waiting...";
						ud=new AsyncNameUpdate(parcel.GroupID,true);  
						ud.onNameCallBack += delegate(string namex,object[] values){this.label_parcelowner.Text=namex;};				

						
						if(entry.AgentID==UUID.Zero)
							continue;
						
						    Console.WriteLine(entry.AgentID.ToString()+" Flags = "+entry.Flags.ToString());
							Gtk.TreeIter iter2=this.parcels_access.AppendValues("Waiting...");			
							ud=new AsyncNameUpdate(entry.AgentID,false);  
							ud.addparameters(iter2);
							ud.onNameCallBack += delegate(string namex,object[] values){ Gtk.TreeIter iterx=(Gtk.TreeIter)values[0]; this.parcels_access.SetValue(iterx,0,namex);};				
					}
					
					foreach(OpenMetaverse.ParcelManager.ParcelAccessEntry entry in parcel.AccessBlackList)
					{
						if(entry.AgentID==UUID.Zero)
							continue;
										
						Console.WriteLine(entry.AgentID.ToString()+" Flags = "+entry.Flags.ToString());
							Gtk.TreeIter iter2=this.parcels_ban.AppendValues("Waiting...");			
							AsyncNameUpdate ud=new AsyncNameUpdate(entry.AgentID,false);  
							ud.addparameters(iter2);
							ud.onNameCallBack += delegate(string namex,object[] values){ Gtk.TreeIter iterx=(Gtk.TreeIter)values[0]; this.parcels_ban.SetValue(iterx,0,namex);};				
						}			
				}
				else			
				{
						Console.WriteLine("No parcel in dictionary for id "+id.ToString()+"\n");
				
				}
					
		}
			
		}

		protected virtual void OnButton1Activated (object sender, System.EventArgs e)
		{
			
		}

		protected virtual void OnButton1Clicked (object sender, System.EventArgs e)
		{
			
			Gtk.TreeModel mod;
			Gtk.TreeIter iter;
			
			this.parcel_prim_owners.Clear();
			
			if(this.treeview_parcels.Selection.GetSelected(out mod,out iter))			
			{
				int id=(int)mod.GetValue(iter,6);
				Console.WriteLine("Requesting parcel prim owners for sim "+MainClass.client.Network.CurrentSim.Name+" parcel :"+id.ToString());
				MainClass.client.Parcels.ObjectOwnersRequest(MainClass.client.Network.CurrentSim,id);
			}
		}
			
	}
}
