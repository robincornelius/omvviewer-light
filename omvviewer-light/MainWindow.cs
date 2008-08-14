// MainWindow.cs created with MonoDevelop
// User: robin at 08:52 11/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
using System;
using System.Collections.Generic;
using Gtk;
using libsecondlife;
using omvviewerlight;

public partial class MainWindow: Gtk.Window
{	
	public List<libsecondlife.LLUUID>active_ims = new List<libsecondlife.LLUUID>();
	public List<libsecondlife.LLUUID>active_groups_ims = new List<libsecondlife.LLUUID>();
	
	Gtk.Label status_location;
	Gtk.HBox status_balance;
	Gtk.Label status_balance_lable;
	Gtk.Label status_parcel;		
	Gtk.HBox status_icons;
	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
		
		status_location=new Gtk.Label("Location: Unknown (0,0,0)");
		
		status_balance=new Gtk.HBox();
		status_balance_lable=new Gtk.Label("?");
		Gtk.Image balicon=new Gtk.Image("status_money.tga");
		status_balance.PackStart(balicon);
		status_balance.PackStart(status_balance_lable);
		status_balance.SetChildPacking(balicon,false,false,0,PackType.Start);
		status_balance.SetChildPacking(status_balance_lable,false,false,0,PackType.Start);
		
		status_parcel=new Gtk.Label("Parcel: Unknown");
		
		this.statusbar1.PackStart(status_location);
		this.statusbar1.PackStart(status_parcel);
		this.statusbar1.PackStart(status_balance);

		this.Title="Omvviewer-light";
	
		// Fuck stupid notebook tabs and monodeveop have to do it myself
		Search s=new Search();
		this.addtabwithicon("status_search_btn.png","Search",s);
		
		Location t=new Location();
		this.addtabwithicon("icon_place.tga","Location",t);
	
		
		
		//this.doicons();
		
		this.statusbar1.ShowAll();
		
		MainClass.client.Self.OnInstantMessage += new libsecondlife.AgentManager.InstantMessageCallback(onIM);
		
		MainClass.client.Network.OnLogin += new libsecondlife.NetworkManager.LoginCallback(onLogin);
		MainClass.client.Self.OnBalanceUpdated += new libsecondlife.AgentManager.BalanceCallback(onBalance);
		//MainClass.client.Parcels.OnSimParcelsDownloaded += new libsecondlife.ParcelManager.SimParcelsDownloaded(onParcels);
		MainClass.client.Parcels.OnParcelProperties += new libsecondlife.ParcelManager.ParcelPropertiesCallback(onParcelProperties);
		MainClass.client.Self.OnTeleport += new libsecondlife.AgentManager.TeleportCallback(onTeleport);
		MainClass.client.Network.OnDisconnected += new libsecondlife.NetworkManager.DisconnectedCallback(onDisconnect);
		
		GLib.Timeout.Add(10000,OnUpdateStatus);
	}
	
	void addtabwithicon(string filename,string label,Gtk.Widget contents)
	{
			Gtk.Image image=new Gtk.Image(filename);
		    image.SetSizeRequest(16,16);
			Gtk.Label lable=new Gtk.Label(label);
		    Gtk.HBox box=new Gtk.HBox();
			box.PackStart(image);
			box.PackStart(lable);
			box.SetChildPacking(image,false,false,0,PackType.Start);
		    box.ShowAll();
		    notebook.InsertPage(contents,box,-1);
		    notebook.ShowAll();

		
	}
	
	void onDisconnect(libsecondlife.NetworkManager.DisconnectType Reason,string msg)	                                       
    {
		Gtk.Application.Invoke(delegate {						
			if(status_icons!=null)
				status_icons.Destroy();

			status_location.Text="Location: Unknown (0,0,0)";
			status_balance_lable.Text="$L?";
			status_parcel.Text="Parcel: Unknown";
			
		
		});
	}
			
	void onTeleport(string Message, libsecondlife.AgentManager.TeleportStatus status,libsecondlife.AgentManager.TeleportFlags flags)
    {		
		Gtk.Application.Invoke(delegate {						
			status_location.Text="Location: "+MainClass.client.Network.CurrentSim.Name+MainClass.client.Self.SimPosition.ToString();	
		});
	}
			
	void doicons(Parcel parcel)
	{
		
		if(status_icons!=null)
			status_icons.Destroy();

		status_icons=new Gtk.HBox();		
		this.statusbar1.PackStart(status_icons);
		
		if((parcel.Flags & libsecondlife.Parcel.ParcelFlags.AllowFly) != libsecondlife.Parcel.ParcelFlags.AllowFly )
		{
			Gtk.Image myimage=new Gtk.Image("status_no_fly.tga");
			status_icons.PackStart(myimage);
			status_icons.SetChildPacking(myimage,false,false,0,PackType.Start);
		}
	
		if((parcel.Flags & libsecondlife.Parcel.ParcelFlags.RestrictPushObject)==libsecondlife.Parcel.ParcelFlags.RestrictPushObject)
		{
			Gtk.Image myimage=new Gtk.Image("status_no_push.tga");
			status_icons.PackStart(myimage);				
			status_icons.SetChildPacking(myimage,false,false,0,PackType.Start);

		}

		if((parcel.Flags & libsecondlife.Parcel.ParcelFlags.AllowOtherScripts)!=libsecondlife.Parcel.ParcelFlags.AllowOtherScripts)
		{
			Gtk.Image myimage=new Gtk.Image("status_no_scripts.tga");
			status_icons.PackStart(myimage);				
			status_icons.SetChildPacking(myimage,false,false,0,PackType.Start);
		
		}

		if((parcel.Flags & libsecondlife.Parcel.ParcelFlags.CreateObjects)!=libsecondlife.Parcel.ParcelFlags.CreateObjects)
		{
			Gtk.Image myimage=new Gtk.Image("status_no_build.tga");
			status_icons.PackStart(myimage);				
			status_icons.SetChildPacking(myimage,false,false,0,PackType.Start);

		}
		
		
		status_icons.ShowAll();
		
	}
	
	void onParcelProperties(Parcel parcel, ParcelManager.ParcelResult result, int sequenceID, bool snapSelection)
	{
		Gtk.Application.Invoke(delegate {						

		string owner="Unknown";
		if(!MainClass.av_names.TryGetValue(parcel.OwnerID,out owner))
				owner="Unknown";
		
		string group="Unknown";
		if(!MainClass.av_names.TryGetValue(parcel.GroupID,out group))
			group="Unknown";	
				
		string size;
		size=parcel.Area.ToString();
		
		int primscount=parcel.OwnerPrims+parcel.OtherPrims+parcel.GroupPrims;
		string prims;
		prims=primscount.ToString()+ "of "+	parcel.MaxPrims;
					
		status_parcel.Text=parcel.Name;
		status_parcel.TooltipText=
				parcel.Name
					+"\nOwner :"+owner
					+"\nGroup :"+group
					+"\nSize: "+size.ToString()	
					+"\nPrims :"+prims.ToString()
					+"\nTraffic: "+parcel.Dwell.ToString()
					+"\nArea: "+parcel.Area.ToString();
	
		
			doicons(parcel);
		});
	}
	
	void onParcels(Simulator sim, InternalDictionary<int,Parcel>sim_parcels,int [,] ParcelMap)
	{
			
	}
		                                                
	void onBalance(int balance)
	{
			Gtk.Application.Invoke(delegate {
			status_balance_lable.Text="L$"+MainClass.client.Self.Balance.ToString();
		});
	}
	
	void onLogin(LoginStatus login, string message)
	{
		if(login==LoginStatus.Success)
		{			
			MainClass.client.Self.RequestBalance();
			//MainClass.client.Parcels.RequestAllSimParcels(MainClass.client.Network.CurrentSim);
			Gtk.Application.Invoke(delegate {						
				OnUpdateStatus();
			});
		}
	}
	
	bool OnUpdateStatus()
	{
		if(MainClass.client.Network.LoginStatusCode==LoginStatus.Success)
		{
			status_location.Text="Location: "+MainClass.client.Network.CurrentSim.Name+MainClass.client.Self.SimPosition.ToString();	
		}		
		return true;
	}
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	public void addnotetab(string name)
	{
	}		

	
	void makeimwindow(string name,ChatConsole cs)
	{
		    Gtk.Image image=new Gtk.Image("closebox.tga");
		    image.SetSizeRequest(16,16);
			Gtk.Label lable=new Gtk.Label(name);
			Gtk.Button button=new Gtk.Button(image);
            button.SetSizeRequest(16,16);
		    Gtk.HBox box=new Gtk.HBox();
			box.PackStart(button);
			box.PackStart(lable);
			box.SetChildPacking(image,false,false,0,PackType.Start);
		
		    box.ShowAll();
		    notebook.InsertPage(cs,box,-1);
		    notebook.ShowAll();
		
		button.Clicked += new EventHandler(cs.clickclosed);

	}
	
	
	public void startIM(LLUUID target)
	{
		if(!active_ims.Contains(target))
		{
			
			Gtk.Application.Invoke(delegate {		
				ChatConsole imc=new ChatConsole(target);
				makeimwindow(MainClass.av_names[target],imc);
				active_ims.Add(target);
			});
		}		
				
	}
	   	
	void onIM(InstantMessage im, Simulator sim)
	{		
			Console.Write("Session is :"+im.IMSessionID.ToString()+"\n");
			Console.Write("Group is :"+im.GroupIM.ToString()+"\n");
			Console.Write("ID is :"+im.FromAgentID.ToString()+"\n");
		
		
		//this.UrgencyHint=true;
		
		if(im.GroupIM==true)
		{		
			if(!active_ims.Contains(im.IMSessionID))
			{
				Gtk.Application.Invoke(delegate {	
					ChatConsole imc=new ChatConsole(im);
					LLUUID key;
					string lable="Unknown";
					
					MainClass.av_names.TryGetValue(im.IMSessionID,out lable);
					makeimwindow(lable,imc);
	
					active_ims.Add(im.IMSessionID);
				});
			}
			return;
		}
		
		if(!active_ims.Contains(im.FromAgentID) && !active_ims.Contains(im.IMSessionID))
		{
			Gtk.Application.Invoke(delegate {						
				ChatConsole imc=new ChatConsole(im);
				makeimwindow("Group :"+im.FromAgentName,imc);
				active_ims.Add(im.FromAgentID);
			});
		}
		
	}	

}