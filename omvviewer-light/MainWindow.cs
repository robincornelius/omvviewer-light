/*
omvviewer-light a Text based client to metaverses such as Linden Labs Secondlife(tm)
    Copyright (C) 2008  Robin Cornelius <robin.cornelius@gmail.com>

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in thOe hope that it will be useful,
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
using System.Threading;
using Gtk;
using Gdk;
using OpenMetaverse;
using omvviewerlight;

public partial class MainWindow: Gtk.Window
{	
	public List<OpenMetaverse.UUID>active_ims = new List<OpenMetaverse.UUID>();
	public List<OpenMetaverse.UUID>active_groups_ims = new List<OpenMetaverse.UUID>();

    public delegate void Cleanuptime();
    public event Cleanuptime oncleanuptime;

	Search tab_search;
	Location tab_location;
    Groups tab_groups;
    ObjectsLayout tab_objects;
    omvviewerlight.Inventory tab_inventory;
    ParcelMgr tab_parcels;
		
	
	Gtk.Label status_location;
	Gtk.HBox status_balance;
	Gtk.Label status_balance_lable;
	Gtk.Label status_parcel;		
	Gtk.HBox status_icons;
	public Gtk.Label chat_tab_lable;// Uber lazy fudge
	
	public uint currentpage=0;
	public StatusIcon trayIcon;
	
	public int current_parcelid;
	bool is_parcel_owner;
	public List<Group> current_groups=new List<Group>();

    bool loggedout = false;
    string parcel_owner_name;
    string parcel_group;
    Tooltips tooltips1;
    int lastparcelid = 0;
   
    ~MainWindow()
    {
        if (trayIcon != null)
        {
            try
            {
                this.trayIcon.Visible = false;
                this.trayIcon.Dispose();
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }
        }
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

		this.Title="Omvviewer-light v0.40 SVN";
		this.SetIconFromFile("viewericon.xpm");
		
		// Fuck stupid notebook tabs and monodeveop have to do it myself
		ChatLayout c=new ChatLayout();
        chat_tab_lable=this.addtabwithicon("icn_voice-pvtfocus.tga","Chat",c);        
		c.passontablable(chat_tab_lable);
        this.notebook.SwitchPage += new SwitchPageHandler(c.onSwitchPage);

        if (MainClass.ReadSetting("tab_location") == "active")
            this.LocationAction.Active = true;

        if (MainClass.ReadSetting("tab_search") == "active")
            this.SearchAction.Active = true;

        if (MainClass.ReadSetting("tab_groups") == "active")
            this.GroupsAction.Active = true;
    
        if (MainClass.ReadSetting("tab_inv") == "active")
            this.InventoryAction.Active = true;

        if (MainClass.ReadSetting("tab_objects") == "active")
            this.ObjectsAction.Active = true;
    
        if (MainClass.ReadSetting("tab_parcel") == "active")
            this.ParcelAction.Active = true;
	
		
		this.statusbar1.ShowAll();
		
		MainClass.client.Self.OnInstantMessage += new OpenMetaverse.AgentManager.InstantMessageCallback(onIM);
		
		MainClass.client.Network.OnLogin += new OpenMetaverse.NetworkManager.LoginCallback(onLogin);
		MainClass.client.Self.OnBalanceUpdated += new OpenMetaverse.AgentManager.BalanceCallback(onBalance);
		MainClass.client.Parcels.OnParcelProperties += new OpenMetaverse.ParcelManager.ParcelPropertiesCallback(onParcelProperties);
		MainClass.client.Self.OnTeleport += new OpenMetaverse.AgentManager.TeleportCallback(onTeleport);
		MainClass.client.Network.OnDisconnected += new OpenMetaverse.NetworkManager.DisconnectedCallback(onDisconnect);
		
		MainClass.client.Friends.OnFriendshipOffered += new OpenMetaverse.FriendsManager.FriendshipOfferedEvent(onFriendship);
		MainClass.client.Self.OnAlertMessage += new OpenMetaverse.AgentManager.AlertMessageCallback(onAlertMessage);
		MainClass.client.Self.OnScriptQuestion += new OpenMetaverse.AgentManager.ScriptQuestionCallback(onScriptCallback);
		MainClass.client.Self.OnScriptDialog +=new OpenMetaverse.AgentManager.ScriptDialogCallback(onScriptDialogue);
		MainClass.client.Self.OnGroupChatLeft += new OpenMetaverse.AgentManager.GroupChatLeftCallback(onLeaveGroupChat);
        MainClass.client.Friends.OnFriendshipResponse += new FriendsManager.FriendshipResponseEvent(Friends_OnFriendshipResponse);
        MainClass.client.Friends.OnFriendshipTerminated += new FriendsManager.FriendshipTerminatedEvent(Friends_OnFriendshipTerminated);

		//this.menubar1.get
		
		this.AvaiableAction.Activate();
		this.StandingAction.Activate();
		
		this.AvaiableAction.Sensitive=false;
		this.AwayAction.Sensitive=false;
		this.BusyAction.Sensitive=false;
		this.StandingAction.Sensitive=false;
		this.CrouchAction.Sensitive=false;
		this.FlyAction.Sensitive=false;
		this.GroundSitAction.Sensitive=false;
		this.SittingAction.Sensitive=false;
		
		this.WindowStateEvent += delegate { if (this.Visible) { trayIcon.Blinking = false; this.UrgencyHint = false; };};
        MainClass.client.Self.OnAvatarSitResponse += new AgentManager.AvatarSitResponseCallback(Self_OnAvatarSitResponse);
    
        this.DeleteEvent += new DeleteEventHandler(MainWindow_DeleteEvent);

        GLib.Timeout.Add(1000,OnUpdateStatus);

        tooltips1 = new Tooltips();
	}

   [GLib.ConnectBefore]
    void MainWindow_DeleteEvent(object o, DeleteEventArgs args)
    {
		
		CloseMinimiseDlg cmd = new CloseMinimiseDlg();
		ResponseType result = (ResponseType)cmd.Run();
		
		if(result==ResponseType.Cancel)
		{
			args.RetVal = true;
			cmd.Destroy();
            return;
		}
		
		if(result==ResponseType.Close)
		{
			 if (oncleanuptime != null)
				oncleanuptime();		

			args.RetVal = false;
			cmd.Destroy();
			
			
			if(MainClass.client.Network.Connected)
			{
				LogoutDlg ld = new LogoutDlg();
				ld.Run();
			}
				
			return;
		}
		
		if(result==ResponseType.Accept)
		{
			cmd.Destroy();
			args.RetVal = true;
            this.Visible = false;
            return;
		}
		
        args.RetVal = true;
    }   
	
/*	
	void MainWindow_DeleteEvent(object o, DeleteEventArgs args)
    {
        Gtk.MessageDialog md = new MessageDialog(this, DialogFlags.Modal, MessageType.Question, ButtonsType.YesNo, false, "Do you really want to close (YES)\n Or minimse (NO)");
        ResponseType result = (ResponseType)md.Run();
        md.Destroy();

        if (result == ResponseType.No)
        {
            args.RetVal = true;
            this.Visible = false;
            return;
        }

		if(MainClass.client.Network.Connected)
		{
			Gtk.MessageDialog md2=new MessageDialog(this,DialogFlags.DestroyWithParent,MessageType.Info,ButtonsType.None,true,"The system is trying to log you out now\n please wait\n This may take a few seconds and the\napplication may report not responding");
            md2.ShowAll();
            GLib.Timeout.Add(500, closewindowpoll);
            Thread logoutRunner = new Thread(new ThreadStart(logout));
            logoutRunner.Start();
            args.RetVal = true;
            return;
		}
		
        if (oncleanuptime != null)
            oncleanuptime();		

        args.RetVal = false;
    }
*/
	
   void logout()
   {
       MainClass.client.Network.Logout();
   }

   bool closewindowpoll()
   {
       if (MainClass.client.Network.Connected)
       {
           return true;
       }
       else
       {
           Application.Quit();
           return false;
       }
   }



    void Self_OnAvatarSitResponse(UUID objectID, bool autoPilot, Vector3 cameraAtOffset, Vector3 cameraEyeOffset, bool forceMouselook, Vector3 sitPosition, Quaternion sitRotation)
    {
        // we sat down
        togglesat();
    }
	
	public void togglesat()
	{
		if(this.SittingAction.Sensitive==false)
		{
            this.SittingAction.Sensitive=true;			
            this.SittingAction.Activate();
		    this.SittingAction.Sensitive=false;			
		}
        this.SittingAction.Activate();		
    }	

    void Friends_OnFriendshipTerminated(UUID agentID, string agentName)
    {
        Gtk.Application.Invoke(delegate
        {
            MessageDialog md = new Gtk.MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Info, ButtonsType.Close, true, agentName+" has terminated your friendship");
            ResponseType result = (ResponseType)md.Run();
            md.Destroy();	
        });
    }

    void Friends_OnFriendshipResponse(UUID agentID, string agentName, bool accepted)
    {
        Gtk.Application.Invoke(delegate
        {
            string msg = "";
            if (accepted)
            {
                msg = agentName + " accepted your friendship request";
            }
            else
            {
                msg = agentName + " declined your friendship request";
            }

            MessageDialog md = new Gtk.MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Info, ButtonsType.Close, true, msg);
            ResponseType result = (ResponseType)md.Run();
            md.Destroy();	
        });
    }	

	void onLeaveGroupChat(UUID session_id)
	{
		Console.Write("Left group chat for session "+session_id.ToString()+"\n");
		if(MainClass.win.active_groups_ims.Contains(session_id))
		   MainClass.win.active_groups_ims.Remove(session_id);
		
	}
	
	void onScriptDialogue(string message,string objectName,UUID imageID,UUID objectID,string FirstName,string lastName,int chatChannel,List <string> buttons)
	{
        Gtk.Application.Invoke(delegate
        {
            ScriptDialogue d = new ScriptDialogue(message, objectName, imageID, objectID, FirstName, lastName, chatChannel, buttons);
            d.Show();
        });
	}
	
	void onAlertMessage(string message)
	{
		if(message=="Autopilot canceled")
			return;
		
		Gtk.Application.Invoke(delegate {						
			string msg;
			msg="<b>ALERT FROM SECONDLIFE</b>\n"+message;
			MessageDialog md= new Gtk.MessageDialog(this,DialogFlags.Modal,MessageType.Info,ButtonsType.Close,true,msg);
			ResponseType result=(ResponseType)md.Run();
            md.Destroy();
		});	
	}
	
	void onScriptCallback(Simulator sim,UUID taskID,UUID itemID,string objectName,string objectOwner,OpenMetaverse.ScriptPermission questions)
	{
		string message;
		
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
			message="<b>BE ABLE TO TAKE YOUR MONEY<\b>";
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
		}

		Gtk.Application.Invoke(delegate {						
			string msg;
			msg="The object : "+objectName+"Owner by :"+objectOwner+"Would like to \n"+message+"\n Would you like to allow this?";
			MessageDialog md= new Gtk.MessageDialog(this,DialogFlags.DestroyWithParent,MessageType.Question,ButtonsType.YesNo,true,msg);
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
	
	void onFriendship(UUID agentID,string agentname,UUID sessionid)
	{
		Gtk.Application.Invoke(delegate {						
		
			string msg;
			msg="You have recieved a friendship request from "+agentname+"\n They would like to become your friend \n do you want to accept?";
			MessageDialog md= new Gtk.MessageDialog(this,DialogFlags.DestroyWithParent,MessageType.Question,ButtonsType.YesNo,true,msg);
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
	
	void onDisconnect(OpenMetaverse.NetworkManager.DisconnectType Reason,string msg)	                                       
    {
		Gtk.Application.Invoke(delegate {						
			if(status_icons!=null)
				status_icons.Destroy();

			status_location.Text="Location: Unknown (0,0,0)";
			status_balance_lable.Text="?";
			status_parcel.Text="Parcel: Unknown";

		});
	}
			
	void onTeleport(string Message, OpenMetaverse.AgentManager.TeleportStatus status,OpenMetaverse.AgentManager.TeleportFlags flags)
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
		
		if((parcel.Flags & OpenMetaverse.Parcel.ParcelFlags.AllowFly) != OpenMetaverse.Parcel.ParcelFlags.AllowFly )
		{
			Gtk.Image myimage=new Gtk.Image("status_no_fly.tga");
			status_icons.PackStart(myimage);
			status_icons.SetChildPacking(myimage,false,false,0,PackType.Start);
		}
	
		if((parcel.Flags & OpenMetaverse.Parcel.ParcelFlags.RestrictPushObject)==OpenMetaverse.Parcel.ParcelFlags.RestrictPushObject)
		{
			Gtk.Image myimage=new Gtk.Image("status_no_push.tga");
			status_icons.PackStart(myimage);				
			status_icons.SetChildPacking(myimage,false,false,0,PackType.Start);

		}

		if((parcel.Flags & OpenMetaverse.Parcel.ParcelFlags.AllowOtherScripts)!=OpenMetaverse.Parcel.ParcelFlags.AllowOtherScripts)
		{
			Gtk.Image myimage=new Gtk.Image("status_no_scripts.tga");
			status_icons.PackStart(myimage);				
			status_icons.SetChildPacking(myimage,false,false,0,PackType.Start);
		
		}

		if((parcel.Flags & OpenMetaverse.Parcel.ParcelFlags.CreateObjects)!=OpenMetaverse.Parcel.ParcelFlags.CreateObjects)
		{
			Gtk.Image myimage=new Gtk.Image("status_no_build.tga");
			status_icons.PackStart(myimage);				
			status_icons.SetChildPacking(myimage,false,false,0,PackType.Start);

		}

		status_icons.ShowAll();	
	}

    void updatestatusinfo()
    {
        Parcel parcel;
        Vector3 pos = MainClass.client.Self.RelativePosition;
        int parcelid = MainClass.client.Network.CurrentSim.ParcelMap[(int)(64.0 * (pos.Y/256.0)), (int)(64.0 * (pos.X/256))];
        if (parcelid == lastparcelid)
            return;

        if (!MainClass.client.Network.CurrentSim.Parcels.TryGetValue(parcelid, out parcel))
            return;

        lastparcelid = parcelid;

        string size;
        size = parcel.Area.ToString();

        int primscount = parcel.OwnerPrims + parcel.OtherPrims + parcel.GroupPrims;
        string prims;
        prims = primscount.ToString() + " of " + parcel.MaxPrims;

        status_parcel.Text = parcel.Name;
        string tooltext;
        tooltext =
                parcel.Name
                    + "\nOwner : " + this.parcel_owner_name
                    + "\nGroup : " + this.parcel_group
                    + "\nSize : " + size.ToString()
                    + "\nPrims : " + prims.ToString()
                    + "\nTraffic : " + parcel.Dwell.ToString()
                    + "\nArea : " + parcel.Area.ToString();

        tooltips1.SetTip(this.statusbar1, tooltext, null);
        tooltips1.Enable();
    }

	void onParcelProperties(Simulator Sim,Parcel parcel, ParcelResult result, int selectedprims,int sequenceID, bool snapSelection)
	{

		//yuck very very hacky
		if(sequenceID==int.MaxValue)
			   return;
			
		this.current_parcelid=parcel.LocalID;
			
		this.is_parcel_owner=(parcel.OwnerID==MainClass.client.Self.AgentID);

        this.parcel_owner_name = "Waiting....";
        this.parcel_group = "Waiting....";

        AsyncNameUpdate an = new AsyncNameUpdate(parcel.OwnerID, false);
        an.onNameCallBack += delegate(string namex, object[] values) { this.parcel_owner_name = namex; updatestatusinfo(); };

        AsyncNameUpdate an2 = new AsyncNameUpdate(parcel.GroupID, true);
        an2.onGroupNameCallBack += delegate(string namex, object[] values) { this.parcel_group = namex; updatestatusinfo(); };
				
		Gtk.Application.Invoke(delegate {		
            updatestatusinfo();
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
                this.AvaiableAction.Sensitive = true;
                this.AwayAction.Sensitive = true;
                this.BusyAction.Sensitive = false;
                this.StandingAction.Sensitive = true;
                this.CrouchAction.Sensitive = true;
                this.FlyAction.Sensitive = true;
                this.GroundSitAction.Sensitive = true;
				this.SittingAction.Sensitive = false;
				
                MainClass.client.Groups.OnCurrentGroups += new OpenMetaverse.GroupManager.CurrentGroupsCallback(onGroups);
				MainClass.client.Groups.RequestCurrentGroups();

			});
		}
	}
	
	bool OnUpdateStatus()
	{
		if(MainClass.client.Network.LoginStatusCode==LoginStatus.Success)
		{
			status_location.Text="Location: "+MainClass.client.Network.CurrentSim.Name+MainClass.prettyvector(MainClass.client.Self.SimPosition,2);
            updatestatusinfo();
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

	
	void makeimwindow(string name,ChatConsole cs,bool group,UUID target)
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
		AsyncNameUpdate ud;
		
		if(group)
			ud=new AsyncNameUpdate(target,true);
		else		
			ud=new AsyncNameUpdate(target,false);
		
		ud.onNameCallBack += delegate(string namex,object [] values){cs.tabLabel.Text=namex;};
		
		button.Clicked += new EventHandler(cs.clickclosed);
		this.notebook.SwitchPage += new SwitchPageHandler(cs.onSwitchPage);
		

	}
	
	
	
	public void startGroupIM(UUID id)
	{
		if(!active_ims.Contains(id))
		{
			ChatConsole imc=new ChatConsole(id,true);
			string lable;
					
			if(!MainClass.client.Groups.GroupName2KeyCache.TryGetValue(id,out lable))
				lable="Waiting...";
							
			makeimwindow(lable,imc,true,id);
	
			active_ims.Add(id);
		
		}
	}
	
	public void startIM(UUID target)
	{
		if(!active_ims.Contains(target))
		{
			
			Gtk.Application.Invoke(delegate {						
				ChatConsole imc=new ChatConsole(target);
				//makeimwindow(MainClass.av_names[target],imc,false,target);
				makeimwindow("Waiting...",imc,false,target);

				active_ims.Add(target);
			});
		}		
				
	}
	   	
	void onIM(InstantMessage im, Simulator sim)
	{	
		
        // Note to self, if i do implement a dispatcher here these two need to be dispatched but not open 
        // new IM tabs so don't treat the same as the rest below.
		if(im.Dialog==OpenMetaverse.InstantMessageDialog.StartTyping)
			return;

        if (im.Dialog == OpenMetaverse.InstantMessageDialog.StopTyping)
            return;

		if(im.Dialog==OpenMetaverse.InstantMessageDialog.GroupNoticeRequested)
			return;
		
		if(im.Dialog==OpenMetaverse.InstantMessageDialog.InventoryOffered)
			return;
			
		if(im.Dialog==OpenMetaverse.InstantMessageDialog.TaskInventoryOffered)
			return;
		
		if(im.Dialog==OpenMetaverse.InstantMessageDialog.InventoryAccepted)
		{
			Gtk.Application.Invoke(delegate {	
				MessageDialog md = new MessageDialog(MainClass.win,DialogFlags.Modal,MessageType.Info,ButtonsType.Ok,im.FromAgentName+" accepted your inventory offer");
				ResponseType result=(ResponseType)md.Run();
				md.Destroy();
			});
			return;
		}
		
		if(im.Dialog==OpenMetaverse.InstantMessageDialog.InventoryAccepted)
		{
			Gtk.Application.Invoke(delegate {	
				MessageDialog md = new MessageDialog(MainClass.win,DialogFlags.Modal,MessageType.Info,ButtonsType.Ok,im.FromAgentName+" accepted your inventory offer");
				ResponseType result=(ResponseType)md.Run();
				md.Destroy();
			});
			return;
		}

		if(im.Dialog==OpenMetaverse.InstantMessageDialog.GroupNotice)
		{
			//Hmm need to handle this differently than a standard IM
			Gtk.Application.Invoke(delegate {	
				MessageDialog md = new MessageDialog(MainClass.win,DialogFlags.DestroyWithParent,MessageType.Info,ButtonsType.Ok,"GROUP NOTICE\nFrom:"+im.FromAgentName+"\n"+im.Message);
				ResponseType result=(ResponseType)md.Run();
				md.Destroy();
				
			});
			return;
		}
		
		if(im.Dialog==OpenMetaverse.InstantMessageDialog.GroupInvitation)
		{
			Gtk.Application.Invoke(delegate {	
				MessageDialog md = new MessageDialog(MainClass.win,DialogFlags.DestroyWithParent,MessageType.Question,ButtonsType.YesNo,im.FromAgentName+" has invited you to join a group\n"+im.Message+"\nPress yes to accept or no to decline");
                ResponseType result=(ResponseType)md.Run();
				if(result==ResponseType.Yes)
				{
                    MainClass.client.Self.InstantMessage(MainClass.client.Self.Name,im.FromAgentID,"",im.IMSessionID,InstantMessageDialog.GroupInvitationAccept,InstantMessageOnline.Offline,MainClass.client.Self.RelativePosition,MainClass.client.Network.CurrentSim.ID,null);
			    }
				else
				{
                    MainClass.client.Self.InstantMessage(MainClass.client.Self.Name,im.FromAgentID,"",im.IMSessionID,InstantMessageDialog.GroupInvitationDecline,InstantMessageOnline.Offline,MainClass.client.Self.RelativePosition,MainClass.client.Network.CurrentSim.ID,null);
		     	
                }
				md.Destroy();
                return;				
            });			
        }		
		
		if(im.Dialog==OpenMetaverse.InstantMessageDialog.RequestTeleport)
		{
			//Hmm need to handle this differently than a standard IM
			Gtk.Application.Invoke(delegate {	
				MessageDialog md = new MessageDialog(MainClass.win,DialogFlags.DestroyWithParent,MessageType.Question,ButtonsType.YesNo,im.FromAgentName+" would like you to join them\n"+im.Message+"\nPress yes to teleport or no to ignore");
				ResponseType result=(ResponseType)md.Run();
				if(result==ResponseType.Yes)
				{
					MainClass.client.Self.TeleportLureRespond(im.FromAgentID,true);
				}
				else
				{
					MainClass.client.Self.TeleportLureRespond(im.FromAgentID,false);
				}
				md.Destroy();
				
			});
			
			return;
			
		}
		
		Gtk.Application.Invoke(delegate {	
		
		if(!this.Visible)
		{
			trayIcon.Blinking=true;
			this.UrgencyHint=true;
		}
	        });

        if (im.IMSessionID == UUID.Zero)
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
							
					   makeimwindow(lable,imc,true,im.IMSessionID);
	
					active_ims.Add(im.IMSessionID);
				});
			}
			return;
		}
		
		if(!active_ims.Contains(im.FromAgentID) && !active_ims.Contains(im.IMSessionID))
		{
            AutoResetEvent ChatSetup = new AutoResetEvent(false);

			Gtk.Application.Invoke(delegate {						
				ChatConsole imc=new ChatConsole(im);
				
                makeimwindow("Waiting...",imc,false,im.FromAgentID);
				active_ims.Add(im.FromAgentID);
                ChatSetup.Set();
			});

            // Block here untill chat window is set up or else we can get multiple IMs stacking up in new windows
            // from same user as set up code is run as an invoke

            ChatSetup.WaitOne(5000, false);

		}
		
	}

	protected virtual void OnAvaiableActionActivated (object sender, System.EventArgs e)
	{
		MainClass.client.Self.Movement.Away=false;
        MainClass.client.Self.Movement.SendUpdate(true);
	}

	protected virtual void OnBusyActionActivated (object sender, System.EventArgs e)
	{
	   	
	}

	protected virtual void OnAwayActionActivated (object sender, System.EventArgs e)
	{
		MainClass.client.Self.Movement.Away=true;
        MainClass.client.Self.Movement.SendUpdate(true);
		
	}

	protected virtual void OnStandingActionActivated (object sender, System.EventArgs e)
	{
		MainClass.client.Self.Stand();
        MainClass.client.Self.Fly(false);			
        MainClass.client.Self.Crouch(false); 
	}

	protected virtual void OnGroundSitActionActivated (object sender, System.EventArgs e)
	{
         MainClass.client.Self.SitOnGround();
	}

	protected virtual void OnCrouchActionActivated (object sender, System.EventArgs e)
	{
         MainClass.client.Self.Crouch(true); 
	}

	protected virtual void OnFlyActionActivated (object sender, System.EventArgs e)
	{
	      MainClass.client.Self.Fly(true);	
}
	
    void onGroups(Dictionary<UUID,Group> groups)
		{
			
			Gtk.Application.Invoke(delegate {
				
				foreach(KeyValuePair <UUID,Group> group in groups)
				{
					if(!this.current_groups.Contains(group.Value))
					{
						this.current_groups.Add(group.Value);
					}
				}
			});

		}
	
	bool parcelallowed(Simulator sim,int parcelid)
	{
	   if(sim.Parcels.Dictionary.ContainsKey(parcelid))
	    {
		   if(sim.Parcels.Dictionary[parcelid].OwnerID==MainClass.client.Self.AgentID)
			  return true;	
			
			//   if(this.current_groups.Contains(sim.Parcels.Dictionary[parcelid].GroupID))
			   {
 				     //Avatar is in the group of this land but do they have permission todo stuff?
			         //assume yes for the moment
			 //        return true; 
   			   }
		}
		
           return false;
    }

	protected virtual void OnParcelActionToggled (object sender, System.EventArgs e)
	{
        if (this.ParcelAction.Active == true)
        {
            tab_parcels = new ParcelMgr();
            this.addtabwithicon("parcel.tga", "Parcel", tab_parcels);
            this.ParcelAction.Active = true;
            MainClass.WriteSetting("tab_parcel", "active");

        }
        else
        {
            tab_parcels.kill();
            MainClass.WriteSetting("tab_parcel", "off");
        }

	}

	protected virtual void OnObjectsActionToggled (object sender, System.EventArgs e)
	{
        if (this.ObjectsAction.Active == true)
        {
            tab_objects = new ObjectsLayout();
            this.addtabwithicon("item_object.tga", "Objects", tab_objects);
            this.ObjectsAction.Active = true;
            MainClass.WriteSetting("tab_objects", "active");

	
        }
        else
        {
            tab_objects.kill();
            MainClass.WriteSetting("tab_objects", "off");

        }	   
	}

	protected virtual void OnInventoryActionToggled (object sender, System.EventArgs e)
	{

        if (this.InventoryAction.Active == true)
        {
            tab_inventory = new omvviewerlight.Inventory();
            this.addtabwithicon("inv_folder_plain_open.tga", "Inventory", tab_inventory);
            this.InventoryAction.Active = true;
            MainClass.WriteSetting("tab_inv", "active");

        }
        else
        {
            tab_inventory.kill();
            MainClass.WriteSetting("tab_inv", "off");

        }	  
  
	}

	protected virtual void OnLocationActionToggled (object sender, System.EventArgs e)
		{
		
         if(this.LocationAction.Active==true)
        {
			tab_location=new Location();
		    this.addtabwithicon("icon_place.tga","Location",tab_location);
		    this.LocationAction.Active=true;
            MainClass.WriteSetting("tab_location", "active");

	    }
		else
		{
           tab_location.kill();
           MainClass.WriteSetting("tab_location", "off");
        }	
	
	}

	protected virtual void OnGroupsActionToggled (object sender, System.EventArgs e)
	{
        if (this.GroupsAction.Active == true)
        {
            tab_groups = new Groups();
            this.addtabwithicon("icn_voice-groupfocus.tga", "Groups", tab_groups);
            this.GroupsAction.Active = true;
            MainClass.WriteSetting("tab_groups", "active");
        }
        else
        {
            tab_groups.kill();
            MainClass.WriteSetting("tab_groups", "off");
        }	
	}

	protected virtual void OnSearchActionToggled (object sender, System.EventArgs e)
    {	
		if(this.SearchAction.Active==true)
        {
	        tab_search=new Search();
			this.addtabwithicon("status_search_btn.png","Search",tab_search);
		    this.SearchAction.Active=true;
            MainClass.WriteSetting("tab_search", "active");
	    }
		else
		{
           tab_search.kill();
           MainClass.WriteSetting("tab_search", "off");
        }	
	}
	
}