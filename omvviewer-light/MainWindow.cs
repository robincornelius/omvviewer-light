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
	Gtk.Label status_balance;
	Gtk.Label status_parcel;		
	Gtk.HBox status_icons;
	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
		
		status_location=new Gtk.Label("Location: Unknown (0,0,0)");
		status_balance=new Gtk.Label("$L?");
		status_parcel=new Gtk.Label("Parcel: Unknown");
		
		this.statusbar1.PackStart(status_location);
		this.statusbar1.PackStart(status_parcel);
		this.statusbar1.PackStart(status_balance);
		
		//this.doicons();
		
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
		}
	
		if((parcel.Flags & libsecondlife.Parcel.ParcelFlags.RestrictPushObject)==libsecondlife.Parcel.ParcelFlags.RestrictPushObject)
		{
			Gtk.Image myimage=new Gtk.Image("status_no_push.tga");
			status_icons.PackStart(myimage);				
		}

		if((parcel.Flags & libsecondlife.Parcel.ParcelFlags.AllowOtherScripts)!=libsecondlife.Parcel.ParcelFlags.AllowOtherScripts)
		{
			Gtk.Image myimage=new Gtk.Image("status_no_scripts.tga");
			status_icons.PackStart(myimage);				
		}

		if((parcel.Flags &libsecondlife.Parcel.ParcelFlags.CreateObjects)==libsecondlife.Parcel.ParcelFlags.CreateObjects)
		{
			Gtk.Image myimage=new Gtk.Image("status_no_build.tga");
			status_icons.PackStart(myimage);				
		}
		
		
		status_icons.ShowAll();
		
	}
	
	void onParcelProperties(Parcel parcel, ParcelManager.ParcelResult result, int sequenceID, bool snapSelection)
	{
		Gtk.Application.Invoke(delegate {						

		string owner;
		
		//if(!MainClass.av_names.TryGetValue(parcel.OwnerID,out owner))
		   owner="Unknown";
		
		string size;
		size=parcel.Area.ToString();
		
		int primscount=parcel.OwnerPrims+parcel.OtherPrims+parcel.GroupPrims;
		string prims;
		prims=primscount.ToString()+ "of "+	parcel.MaxPrims;
					
		status_parcel.Text=parcel.Name;
		status_parcel.TooltipText=
				parcel.Name
					+"\nOwner :"+owner
					+"\nGroup :"+parcel.GroupID.ToString()
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
			status_balance.Text="L$"+MainClass.client.Self.Balance.ToString();
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

	public void startIM(LLUUID target)
	{
		if(!active_ims.Contains(target))
		{
			Widget lable=new Gtk.Label(MainClass.av_names[target]);
			ChatConsole imc=new ChatConsole(target);
			Gtk.Application.Invoke(delegate {						
				notebook.InsertPage(imc,lable,notebook.Page);
				active_ims.Add(target);
			});
		}		
				
	}
	   	
	void onIM(InstantMessage im, Simulator sim)
	{		
			Console.Write("Session is :"+im.IMSessionID.ToString()+"\n");
			Console.Write("Group is :"+im.GroupIM.ToString()+"\n");
			Console.Write("ID is :"+im.FromAgentID.ToString()+"\n");
			
		if(im.GroupIM==true)
		{		
		if(!active_ims.Contains(im.IMSessionID))
		{

			Gtk.Application.Invoke(delegate {						

		    Gtk.Image image=new Gtk.Image("close.xpm");
			image.SetSizeRequest(16,16);
			Gtk.Label lable=new Gtk.Label(im.FromAgentName);
			Gtk.Button button=new Gtk.Button(image);
			Gtk.HBox box=new Gtk.HBox();
			box.PackStart(button);
			box.PackStart(lable);
				
			box.ShowAll();
	
			button.Clicked += new EventHandler(clickclosed);
			
			ChatConsole imc=new ChatConsole(im);
						
				notebook.InsertPage(imc,(Gtk.Widget)box,-1); 
				notebook.ShowAll();
				active_ims.Add(im.IMSessionID);
			});
			}
			return;
		}
		
		if(!active_ims.Contains(im.FromAgentID) && !active_ims.Contains(im.IMSessionID))
		{
			Gtk.Application.Invoke(delegate {						

		    Gtk.Image image=new Gtk.Image("close.xpm");
			image.SetSizeRequest(16,16);
			Gtk.Label lable=new Gtk.Label(im.FromAgentName);
			Gtk.Button button=new Gtk.Button(image);
			Gtk.HBox box=new Gtk.HBox();
			box.PackStart(button);
			box.PackStart(lable);
				
			box.ShowAll();
	
				
			button.Clicked += new EventHandler(clickclosed);
			
			ChatConsole imc=new ChatConsole(im);
						
				notebook.InsertPage(imc,(Gtk.Widget)box,-1); 
				notebook.ShowAll();
				active_ims.Add(im.FromAgentID);
			});
		}
		
	}	

		void clickclosed(object obj, EventArgs args)
		{
		    /// Urrrg
		    // The button is in a hbox in a tab of a notebook page in a notebook in a chatwidget
		    int pageno=1;
		    pageno=notebook.PageNum((Gtk.Widget)obj);
		    ChatConsole cs=(ChatConsole)notebook.GetNthPage(pageno);

		if(cs.im_key!=libsecondlife.LLUUID.Zero)
				if(MainClass.win.active_ims.Contains(cs.im_key))
					MainClass.win.active_ims.Remove(cs.im_key);	
		
			notebook.RemovePage(pageno);
		}


}