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
	
		GLib.Timeout.Add(10000,OnUpdateStatus);
	}
	
	
	bool OnUpdateStatus()
	{
		if(MainClass.client.Network.Connected)
		{
			status_location.Text="Location: "+MainClass.client.Network.CurrentSim.Name+MainClass.client.Self.SimPosition.ToString();	
			status_location.TooltipText="Here we\ngo";
			status_balance.Text="L$"+MainClass.client.Self.Balance.ToString();
			//status_parcel.Text="Parcel :"+MainClass.client
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