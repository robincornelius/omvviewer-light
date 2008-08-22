/*
omvviewer-light a Text based client to metaverses such as Linden Labs Secondlife(tm)
    Copyright (C) 2008  Robin Cornelius <robin.cornelius@gmail.com>

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

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
	public Gtk.Label chat_tab_lable;// Uber lazy fudge
	
	public uint currentpage=0;
	public StatusIcon trayIcon;

    ~MainWindow()
    {
        this.trayIcon.Visible = false;
        this.trayIcon.Dispose();
    }

	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
        Build();
       
		trayIcon = new StatusIcon(new Gdk.Pixbuf("viewericon.xpm"));
		trayIcon.Visible=true;
		trayIcon.Tooltip="Hello World";
		trayIcon.Activate+= delegate{Visible=!Visible;};

        trayIcon.Activate += delegate { trayIcon.Blinking = false; this.UrgencyHint = false; };
				
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
		this.SetIconFromFile("viewericon.xpm");
		
		// Fuck stupid notebook tabs and monodeveop have to do it myself
		ChatLayout c=new ChatLayout();
        chat_tab_lable=this.addtabwithicon("icn_voice-pvtfocus.tga","Chat",c);
		
		Location t=new Location();
		this.addtabwithicon("icon_place.tga","Location",t);
			
		Search s=new Search();
		this.addtabwithicon("status_search_btn.png","Search",s);

		ObjectsLayout o=new ObjectsLayout();
		this.addtabwithicon("item_object.tga","Objects",o);
		
		Groups g = new Groups();
		this.addtabwithicon("icn_voice-groupfocus.tga","Groups",g);
		//this.doicons();
				
		omvviewerlight.Inventory i = new omvviewerlight.Inventory();
		this.addtabwithicon("inv_folder_plain_open.tga","Inventory",i);
		//this.doicons();
		
		this.statusbar1.ShowAll();
		
		MainClass.client.Self.OnInstantMessage += new libsecondlife.AgentManager.InstantMessageCallback(onIM);
		
		MainClass.client.Network.OnLogin += new libsecondlife.NetworkManager.LoginCallback(onLogin);
		MainClass.client.Self.OnBalanceUpdated += new libsecondlife.AgentManager.BalanceCallback(onBalance);
		MainClass.client.Parcels.OnParcelProperties += new libsecondlife.ParcelManager.ParcelPropertiesCallback(onParcelProperties);
		MainClass.client.Self.OnTeleport += new libsecondlife.AgentManager.TeleportCallback(onTeleport);
		MainClass.client.Network.OnDisconnected += new libsecondlife.NetworkManager.DisconnectedCallback(onDisconnect);
		
		MainClass.client.Friends.OnFriendshipOffered += new libsecondlife.FriendsManager.FriendshipOfferedEvent(onFriendship);
		MainClass.client.Self.OnAlertMessage += new libsecondlife.AgentManager.AlertMessage(onAlertMessage);
		MainClass.client.Self.OnScriptQuestion += new libsecondlife.AgentManager.ScriptQuestionCallback(onScriptCallback);
		MainClass.client.Self.OnScriptDialog +=new libsecondlife.AgentManager.ScriptDialogCallback(onScriptDialogue);
		
        this.WindowStateEvent += delegate { if (this.Visible) { trayIcon.Blinking = false; this.UrgencyHint = false; };};
         

		GLib.Timeout.Add(10000,OnUpdateStatus); 
	}

    
    
	void onScriptDialogue(string message,string objectName,LLUUID imageID,LLUUID objectID,string FirstName,string lastName,int chatChannel,List <string> buttons)
	{
        Gtk.Application.Invoke(delegate
        {
            ScriptDialogue d = new ScriptDialogue(message, objectName, imageID, objectID, FirstName, lastName, chatChannel, buttons);
            d.Show();
        });
	}
	
	void onAlertMessage(string message)
	{
		Gtk.Application.Invoke(delegate {						
			string msg;
			msg="<b>ALERT FROM SECONDLIFE<b>\n"+message;
			MessageDialog md= new Gtk.MessageDialog(this,DialogFlags.Modal,MessageType.Info,ButtonsType.Close,true,msg);
			ResponseType result=(ResponseType)md.Run();			
		});	
	}
	
	void onScriptCallback(Simulator sim,LLUUID taskID,LLUUID itemID,string objectName,string objectOwner,libsecondlife.ScriptPermission questions)
	{
		string message;
	
		ScriptPermission x;
		
		switch(questions)
		{
		case ScriptPermission.Attach:
			message="Attach to you";
			break;
		case ScriptPermission.ChangeJoints:
			message="Change joints";
			break;
		case ScriptPermission.ChangeLinks:
			message="Change links";
			break;
		case ScriptPermission.ChangePermissions:
			message="<b>Change permissions<b>";
			break;
		case ScriptPermission.ControlCamera:
			message="Control your camera";
			break;
		case ScriptPermission.Debit:
			message="<b>BE ABLE TO TAKE YOUR MONEY<b>";
			break;
		case ScriptPermission.ReleaseOwnership:
			message="Release ownership";
			break;
		case ScriptPermission.RemapControls:
			message="Remap controls";
			break;
		case ScriptPermission.TakeControls:
			message="Take controls";
			break;
		case ScriptPermission.TrackCamera:
			message="Track camera";
			break;
		case ScriptPermission.TriggerAnimation:
			message="Trigger animations";
			break;
		default:
			message="I HAVE NO IDEA";
			return;
			break;
		}

		Gtk.Application.Invoke(delegate {						
			string msg;
			msg="The object : "+objectName+"Owner by :"+objectOwner+"Would like to \n"+message+"\n Would you like to allow this?";
			MessageDialog md= new Gtk.MessageDialog(this,DialogFlags.Modal,MessageType.Question,ButtonsType.YesNo,true,msg);
			ResponseType result=(ResponseType)md.Run();
			if(result==ResponseType.Yes)
			{
				MainClass.client.Self.ScriptQuestionReply(sim,itemID,taskID,questions);
			}
			else
			{
				MainClass.client.Self.ScriptQuestionReply(sim,itemID,taskID,ScriptPermission.None);
			}
			md.Destroy();
			
		});
	}
	
	public Gtk.Notebook getnotebook()
	{
		return this.notebook;
	}
	
	void onFriendship(LLUUID agentID,string agentname,LLUUID sessionid)
	{
		Gtk.Application.Invoke(delegate {						
		
			string msg;
			msg="You have recieved a friendship request from "+agentname+"\n They would like to become your friend \n do you want to accept?";
			MessageDialog md= new Gtk.MessageDialog(this,DialogFlags.Modal,MessageType.Question,ButtonsType.YesNo,true,msg);
			ResponseType result=(ResponseType)md.Run();
			if(result==ResponseType.Yes)
			{
				MainClass.client.Friends.AcceptFriendship(agentID,sessionid);
			}
			else
			{
				MainClass.client.Friends.DeclineFriendship(agentID,sessionid);
			}
			md.Destroy();
		});			
	}
			
	Gtk.Label addtabwithicon(string filename,string label,Gtk.Widget contents)
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
        return lable;		
	}
	
	void onDisconnect(libsecondlife.NetworkManager.DisconnectType Reason,string msg)	                                       
    {
		Gtk.Application.Invoke(delegate {						
			if(status_icons!=null)
				status_icons.Destroy();

			status_location.Text="Location: Unknown (0,0,0)";
			status_balance_lable.Text="?";
			status_parcel.Text="Parcel: Unknown";

		});
	}
			
	void onTeleport(string Message, libsecondlife.AgentManager.TeleportStatus status,libsecondlife.AgentManager.TeleportFlags flags)
    {		
		Gtk.Application.Invoke(delegate {						
			status_location.Text="Location: "+MainClass.client.Network.CurrentSim.Name+MainClass.prettyvector(MainClass.client.Self.SimPosition,2);	
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
        string tooltext;        
		tooltext=
				parcel.Name
					+"\nOwner :"+owner
					+"\nGroup :"+group
					+"\nSize: "+size.ToString()	
				    +"\nPrims :"+prims.ToString()
					+"\nTraffic: "+parcel.Dwell.ToString()
					+"\nArea: "+parcel.Area.ToString();

        Tooltips tooltips1 = new Tooltips();
        tooltips1.SetTip(this.statusbar1, tooltext, null);
        tooltips1.Enable();
       
      
			doicons(parcel);
		});
	}
			                                                
	void onBalance(int balance)
	{
		Gtk.Application.Invoke(delegate {
			status_balance_lable.Text=MainClass.client.Self.Balance.ToString();
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
			status_location.Text="Location: "+MainClass.client.Network.CurrentSim.Name+MainClass.prettyvector(MainClass.client.Self.SimPosition,2);	
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

	
	void makeimwindow(string name,ChatConsole cs,bool group)
	{
		Gtk.Image image=new Gtk.Image("closebox.tga");
		image.HeightRequest=16;
		image.WidthRequest=16;
		
		Gtk.Image icon;
		
		if(group)
			icon=new Gtk.Image("icon_group.tga");
		else
			icon=new Gtk.Image("icn_voice-groupfocus.tga");
		
		image.SetSizeRequest(16,16);
		Gtk.Label lable=new Gtk.Label(name);
		Gtk.Button button=new Gtk.Button(image);
        button.SetSizeRequest(16,16);
		    Gtk.HBox box=new Gtk.HBox();
			box.PackStart(icon);
			box.PackStart(lable);
		    box.PackStart(button);
		    box.SetChildPacking(image,false,false,0,PackType.Start);
		
		    box.ShowAll();
		    notebook.InsertPage(cs,box,-1);
		    notebook.ShowAll();
		cs.tabLabel=lable;
		cs.kicknames();
		button.Clicked += new EventHandler(cs.clickclosed);
		this.notebook.SwitchPage += new SwitchPageHandler(cs.onSwitchPage);
		

	}
	
	
	
	public void startGroupIM(LLUUID id)
	{
		if(!active_ims.Contains(id))
		{
			ChatConsole imc=new ChatConsole(id,true);
			string lable;
					
			if(!MainClass.client.Groups.GroupName2KeyCache.TryGetValue(id,out lable))
				lable="Waiting...";
							
			makeimwindow(lable,imc,true);
	
			active_ims.Add(id);
		
		}
	}
	
	public void startIM(LLUUID target)
	{
		if(!active_ims.Contains(target))
		{
			
			Gtk.Application.Invoke(delegate {						
				ChatConsole imc=new ChatConsole(target);
				makeimwindow(MainClass.av_names[target],imc,false);
				active_ims.Add(target);
			});
		}		
				
	}
	   	
	void onIM(InstantMessage im, Simulator sim)
	{	
		Gtk.Application.Invoke(delegate {	
		
		if(!this.Visible)
		{
			trayIcon.Blinking=true;
			this.UrgencyHint=true;
		}
	        });

        if (im.IMSessionID == LLUUID.Zero)
            return; //Its an object Im, chat weill grab this for us
			
		if(im.GroupIM==true)
		{		
			if(!active_ims.Contains(im.IMSessionID))
			{
				Gtk.Application.Invoke(delegate {	
					ChatConsole imc=new ChatConsole(im);
					string lable;
					
					if(!MainClass.client.Groups.GroupName2KeyCache.TryGetValue(im.IMSessionID,out lable))
						lable="Waiting...";
							
					   makeimwindow(lable,imc,true);
	
					active_ims.Add(im.IMSessionID);
				});
			}
			return;
		}
		
		if(!active_ims.Contains(im.FromAgentID) && !active_ims.Contains(im.IMSessionID))
		{
			Gtk.Application.Invoke(delegate {						
				ChatConsole imc=new ChatConsole(im);
				string name;

				if(!MainClass.av_names.TryGetValue(im.FromAgentID,out name))
					name="Waiting...";
				
			
				makeimwindow(name,imc,false);
				active_ims.Add(im.FromAgentID);
			});
		}
		
	}	

}