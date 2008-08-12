// MainWindow.cs created with MonoDevelop
// User: robin at 08:52 11/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
using System;
using System.Collections.Generic;
using Gtk;
using libsecondlife;
using libsecondlife;
using omvviewerlight;

public partial class MainWindow: Gtk.Window
{	
	public List<libsecondlife.LLUUID>active_ims = new List<libsecondlife.LLUUID>();
	public List<libsecondlife.LLUUID>active_groups_ims = new List<libsecondlife.LLUUID>();
	
	Gtk.Label status_location;
	Gtk.Label status_balance;
	Gtk.Label status_parcel;
	string status_parcel_tool_text;
	string status_parcel_text;
		
	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
		
		status_location=new Gtk.Label("Location: Unknown (0,0,0)");
		status_balance=new Gtk.Label("$L?");
		status_parcel=new Gtk.Label("Parcel: Unknown");
		
		this.statusbar1.PackStart(status_location);
		this.statusbar1.PackStart(status_parcel);
		this.statusbar1.PackStart(status_balance);

		this.statusbar1.ShowAll();
		
		MainClass.client.Self.OnInstantMessage += new libsecondlife.AgentManager.InstantMessageCallback(onIM);
		MainClass.client.Network.OnLogin += new libsecondlife.NetworkManager.LoginCallback(onLogin);
		MainClass.client.Self.OnBalanceUpdated += new libsecondlife.AgentManager.BalanceCallback(onBalance);
		//MainClass.client.Parcels.OnSimParcelsDownloaded += new libsecondlife.ParcelManager.SimParcelsDownloaded(onParcels);
		MainClass.client.Parcels.OnParcelProperties += new libsecondlife.ParcelManager.ParcelPropertiesCallback(onParcelProperties);
		MainClass.client.Self.OnTeleport += new libsecondlife.AgentManager.TeleportCallback(onTeleport);
		GLib.Timeout.Add(10000,OnUpdateStatus);
	}
	
	
	void onTeleport(string Message, libsecondlife.AgentManager.TeleportStatus status,libsecondlife.AgentManager.TeleportFlags flags)
    {
		
			
	}
			
	
	void onParcelProperties(Parcel parcel, ParcelManager.ParcelResult result, int sequenceID, bool snapSelection)
	{
	
		string owner;
		
		//if(!MainClass.av_names.TryGetValue(parcel.OwnerID,out owner))
		   owner="Unknown";
		
		string size;
		size=parcel.Area.ToString();
		
		int primscount=parcel.OwnerPrims+parcel.OtherPrims+parcel.GroupPrims;
		string prims;
		prims=primscount.ToString()+" of "+parcel.TotalPrims.ToString();
		
		status_parcel_text="Parcel :"+parcel.Name;
		status_parcel_tool_text=parcel.Name+"\nOwner :"+owner+"\nSize: "+size+"\nPrims :"+prims;
	}
	
	void onParcels(Simulator sim, InternalDictionary<int,Parcel>sim_parcels,int [,] ParcelMap)
	{
			
	}
		                                                
	void onBalance(int balance)
	{
			status_balance.Text="L$"+MainClass.client.Self.Balance.ToString();
			//status_location.QueueDraw();	
	}
	
	void onLogin(LoginStatus login, string message)
	{
		if(login==LoginStatus.Success)
		{			
			MainClass.client.Self.RequestBalance();
			//MainClass.client.Parcels.RequestAllSimParcels(MainClass.client.Network.CurrentSim);
			OnUpdateStatus();
		}
	}
	
	bool OnUpdateStatus()
	{
		if(MainClass.client.Network.Connected)
		{
			status_location.Text="Location: "+MainClass.client.Network.CurrentSim.Name+MainClass.client.Self.SimPosition.ToString();	
			status_location.TooltipText="Here we\ngo";
			status_balance.Text="L$"+MainClass.client.Self.Balance.ToString();
			status_parcel.Text=status_parcel_text;
			status_parcel.TooltipText=status_parcel_tool_text;
			status_location.QueueDraw();
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

	public void startIM(LLUUID target)
	{
		if(!active_ims.Contains(target))
		{
			Widget lable=new Gtk.Label(MainClass.av_names[target]);
			ChatConsole imc=new ChatConsole(target);
			notebook.InsertPage(imc,lable,notebook.Page);
			active_ims.Add(target);
		}		
				
	}
	             
	void onIM(InstantMessage im, Simulator sim)
	{		
		// don't do this yet
		if(im.GroupIM==true)
		{
			if(!active_ims.Contains(im.IMSessionID))
			{
				Widget lable=new Gtk.Label("Group: "+im.FromAgentName);
				ChatConsole imc=new ChatConsole(im.IMSessionID);
				notebook.InsertPage(imc,lable,notebook.Page);
				active_ims.Add(im.IMSessionID);				
			}
			
			return;
		}
		
		
		if(!active_ims.Contains(im.FromAgentID))
		{
			Widget lable=new Gtk.Label(im.FromAgentName);
			ChatConsole imc=new ChatConsole(im);
			notebook.InsertPage(imc,lable,notebook.Page);
			active_ims.Add(im.FromAgentID);
		}		
		}

}