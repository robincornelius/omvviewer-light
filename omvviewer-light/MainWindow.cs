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

// MainWindow.cs created with MonoDevelop
// User: robin at 08:52 11/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using Gtk;
using Gdk;
using OpenMetaverse;
using omvviewerlight;


public partial class MainWindow: Gtk.Window
{	
	public List<OpenMetaverse.UUID>active_ims = new List<OpenMetaverse.UUID>();
	//public List<OpenMetaverse.UUID>active_groups_ims = new List<OpenMetaverse.UUID>();

    public delegate void Cleanuptime();
    public event Cleanuptime oncleanuptime;

    public delegate void LogoutWork();
    public event LogoutWork onLogoutWork;
    AutoResetEvent appearancesetting= new AutoResetEvent(true);

    public delegate void InventoryAccepted(AssetType type, UUID objectID);
    public static event InventoryAccepted OnInventoryAccepted;

	Search tab_search;
	Location tab_location;
    Groups tab_groups;
    ObjectsLayout tab_objects;
    omvviewerlight.Inventory tab_inventory;
    ParcelMgr tab_parcels;

    ChatLayout chat;
			
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
    int current_parcel_dwell;

    string parcel_owner_name;
    string parcel_group;
    Tooltips tooltips1;
    int lastparcelid = 0;
	
	public TeleportTo tp_target_widget=null;
    public Map map_widget=null;
	
	public List<AvatarGroup> avatarGroups=new List<AvatarGroup>();

    public List<InstantMessage> im_queue = new List<InstantMessage>();
    public Dictionary<UUID, ChatConsole> im_windows = new Dictionary<UUID, ChatConsole>();
    public List<UUID> im_registering = new List<UUID>();

    public Dictionary<ulong, GridRegion> grid_regions = new Dictionary<ulong, GridRegion>();
	
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

 void menu_quit_fn(object o, ButtonPressEventArgs args)
	{
		MainClass.userlogout = true;
        //if (onLogoutWork != null)
        {
            Console.WriteLine("Running logout tasks first");
            if(MainClass.client.Inventory.Store!=null)
                MainClass.client.Inventory.Store.SaveToDisk(MainClass.client.Settings.ASSET_CACHE_DIR + System.IO.Path.DirectorySeparatorChar + MainClass.client.Inventory.Store.RootFolder.UUID.ToString() + ".osl");
            Console.WriteLine("Done");
        }
     
        LogoutDlg ld = new LogoutDlg();
        ld.Modal = false;
	    ld.Run();
		//Gtk.Application.Quit();
	}
 void menu_hide_fn(object o, ButtonPressEventArgs args)
	{
		Visible=false;
	}
 void menu_restore_fn(object o, ButtonPressEventArgs args)
	{
		Visible=true;
	}

	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
        Build();
   		
		trayIcon = new StatusIcon(MainClass.GetResource("omvviewer-light.xpm"));
		trayIcon.Visible=true;
		trayIcon.Tooltip="Disconnected";
		trayIcon.Activate+= delegate{
            Visible=!Visible;
            this.Deiconify();
        };
        trayIcon.Activate += delegate { trayIcon.Blinking = false; this.UrgencyHint = false; };

		trayIcon.PopupMenu += delegate { 
			Gtk.Menu menu = new Gtk.Menu(); 
			    Gtk.ImageMenuItem menu_hide = new ImageMenuItem("Minimse");
			  Gtk.ImageMenuItem menu_restore = new ImageMenuItem("Restore");
			AccelGroup ag=new AccelGroup();
              Gtk.ImageMenuItem menu_quit = new ImageMenuItem("gtk-quit",ag);
			
			  menu_quit.ButtonPressEvent += new ButtonPressEventHandler(menu_quit_fn);
			  menu_restore.ButtonPressEvent  += new ButtonPressEventHandler(menu_restore_fn);
			  menu_hide.ButtonPressEvent  += new ButtonPressEventHandler(menu_hide_fn);
			  if(MainClass.win.Visible)
			      menu.Append(menu_hide);
			  else				
				menu.Append(menu_restore);
              menu.Append(menu_quit);
              menu.ShowAll();
              menu.Popup();
		};
				
		this.Icon=MainClass.GetResource("omvviewer-light.xpm");
		status_location=new Gtk.Label("Location: Unknown (0,0,0)");

        appearancesetting.Set();

		status_balance=new Gtk.HBox();
		status_balance_lable=new Gtk.Label("?");
		Gtk.Image balicon=new Gtk.Image();
		balicon.Pixbuf = MainClass.GetResource("status_money.png");
		status_balance.PackStart(balicon);
		status_balance.PackStart(status_balance_lable);
		status_balance.SetChildPacking(balicon,false,false,0,PackType.Start);
		status_balance.SetChildPacking(status_balance_lable,false,false,0,PackType.Start);
		
		status_parcel=new Gtk.Label("Parcel: Unknown");
		
		this.statusbar1.PackStart(status_location);
		this.statusbar1.PackStart(status_parcel);
		this.statusbar1.PackStart(status_balance);

		this.Title="omvviewer light v0.48.1";
		
	       chat=new ChatLayout();
           chat_tab_lable=this.addtabwithicon("icn_voice-pvtfocus.png","Chat",chat);        
	       chat.passontablable(chat_tab_lable);
           this.notebook.SwitchPage += new SwitchPageHandler(chat.onSwitchPage);

            this.LocationAction.Active = MainClass.appsettings.tab_location;

            this.SearchAction.Active = MainClass.appsettings.tab_search;

            this.GroupsAction.Active = MainClass.appsettings.tab_groups;
    
            this.InventoryAction.Active = MainClass.appsettings.tab_inv;

            this.ObjectsAction.Active = MainClass.appsettings.tab_objects;
    
            this.ParcelAction.Active = MainClass.appsettings.tab_parcel;
	
		
		this.statusbar1.ShowAll();
		


        MainClass.onRegister += new MainClass.register(MainClass_onRegister);
        MainClass.onDeregister += new MainClass.deregister(MainClass_onDeregister);
        if(MainClass.client != null ) { MainClass_onRegister(); }

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
        

        this.DeleteEvent += new DeleteEventHandler(MainWindow_DeleteEvent);

        GLib.Timeout.Add(1000,OnUpdateStatus);

        tooltips1 = new Tooltips();
        this.statusbar1.Push(1, "Logged out");

	}


    void MainClass_onDeregister()
    {
        MainClass.client.Self.IM -= new EventHandler<InstantMessageEventArgs>(Self_IM);
        MainClass.client.Network.LoginProgress -= new EventHandler<LoginProgressEventArgs>(Network_LoginProgress);
        MainClass.client.Self.MoneyBalanceReply -= new EventHandler<MoneyBalanceReplyEventArgs>(Self_MoneyBalanceReply);
        MainClass.client.Self.TeleportProgress -= new EventHandler<TeleportEventArgs>(Self_TeleportProgress);
        MainClass.client.Network.Disconnected -= new EventHandler<DisconnectedEventArgs>(Network_Disconnected);
        MainClass.client.Friends.FriendshipOffered -= new EventHandler<FriendshipOfferedEventArgs>(Friends_FriendshipOffered);
        MainClass.client.Self.AlertMessage -= new EventHandler<AlertMessageEventArgs>(Self_AlertMessage);
        MainClass.client.Self.ScriptQuestion -= new EventHandler<ScriptQuestionEventArgs>(Self_ScriptQuestion);
        MainClass.client.Self.ScriptDialog -= new EventHandler<ScriptDialogEventArgs>(Self_ScriptDialog);
        MainClass.client.Self.GroupChatLeft -= new EventHandler<GroupChatLeftEventArgs>(Self_GroupChatLeft);
        MainClass.client.Friends.FriendshipResponse -= new EventHandler<FriendshipResponseEventArgs>(Friends_FriendshipResponse);
        MainClass.client.Friends.FriendshipTerminated -= new EventHandler<FriendshipTerminatedEventArgs>(Friends_FriendshipTerminated);
        MainClass.client.Parcels.ParcelDwellReply -= new EventHandler<ParcelDwellReplyEventArgs>(Parcels_ParcelDwellReply);
        MainClass.client.Inventory.InventoryObjectOffered -= new EventHandler<InventoryObjectOfferedEventArgs>(Inventory_InventoryObjectOffered);
        MainClass.client.Network.SimChanged -= new EventHandler<SimChangedEventArgs>(Network_SimChanged);
        MainClass.client.Grid.GridRegion -= new EventHandler<GridRegionEventArgs>(Grid_GridRegion);
        MainClass.client.Self.AvatarSitResponse -= new EventHandler<AvatarSitResponseEventArgs>(Self_AvatarSitResponse);
        MainClass.client.Groups.CurrentGroups -= new EventHandler<CurrentGroupsEventArgs>(Groups_CurrentGroups);
     
    }

    void MainClass_onRegister()
    {

        MainClass.client.Self.IM += new EventHandler<InstantMessageEventArgs>(Self_IM);
        MainClass.client.Network.LoginProgress += new EventHandler<LoginProgressEventArgs>(Network_LoginProgress);
        MainClass.client.Self.MoneyBalanceReply += new EventHandler<MoneyBalanceReplyEventArgs>(Self_MoneyBalanceReply);
        MainClass.client.Self.TeleportProgress += new EventHandler<TeleportEventArgs>(Self_TeleportProgress);
        MainClass.client.Network.Disconnected += new EventHandler<DisconnectedEventArgs>(Network_Disconnected);
        MainClass.client.Friends.FriendshipOffered += new EventHandler<FriendshipOfferedEventArgs>(Friends_FriendshipOffered);
        MainClass.client.Self.AlertMessage += new EventHandler<AlertMessageEventArgs>(Self_AlertMessage);
        MainClass.client.Self.ScriptQuestion += new EventHandler<ScriptQuestionEventArgs>(Self_ScriptQuestion);
        MainClass.client.Self.ScriptDialog += new EventHandler<ScriptDialogEventArgs>(Self_ScriptDialog);
        MainClass.client.Self.GroupChatLeft += new EventHandler<GroupChatLeftEventArgs>(Self_GroupChatLeft);
        MainClass.client.Friends.FriendshipResponse += new EventHandler<FriendshipResponseEventArgs>(Friends_FriendshipResponse);
        MainClass.client.Friends.FriendshipTerminated += new EventHandler<FriendshipTerminatedEventArgs>(Friends_FriendshipTerminated);
        MainClass.client.Parcels.ParcelDwellReply += new EventHandler<ParcelDwellReplyEventArgs>(Parcels_ParcelDwellReply);
        MainClass.client.Inventory.InventoryObjectOffered += new EventHandler<InventoryObjectOfferedEventArgs>(Inventory_InventoryObjectOffered);
        MainClass.client.Network.SimChanged += new EventHandler<SimChangedEventArgs>(Network_SimChanged);
        MainClass.client.Grid.GridRegion += new EventHandler<GridRegionEventArgs>(Grid_GridRegion);
        MainClass.client.Self.AvatarSitResponse += new EventHandler<AvatarSitResponseEventArgs>(Self_AvatarSitResponse);
        MainClass.client.Groups.CurrentGroups += new EventHandler<CurrentGroupsEventArgs>(Groups_CurrentGroups);
     
    
    }

   

    new public void Dispose()
    {
        MainClass.onRegister -= new MainClass.register(MainClass_onRegister);
        MainClass.onDeregister -= new MainClass.deregister(MainClass_onDeregister);
        MainClass_onDeregister();
    }



    void Grid_GridRegion(object sender, GridRegionEventArgs e)
{
    lock (MainClass.win.grid_regions)
    {
        if(!grid_regions.ContainsKey(e.Region.RegionHandle))
            grid_regions.Add(e.Region.RegionHandle, e.Region);
    }

}

    void Network_SimChanged(object sender, SimChangedEventArgs e)
    {
        if (MainClass.client.Network.LoginStatusCode == LoginStatus.Success)
        {
            //Console.WriteLine("Changed Sim, forcing a rebake");
            //Thread app=new Thread(new ThreadStart(setappearance));
            //app.Start();
        }
    }

    void setappearance()
    {

        appearancesetting.WaitOne(30000, false);
        appearancesetting.Reset();
        Console.WriteLine("Setting appearance");
        MainClass.client.Appearance.SetPreviousAppearance(true);
        appearancesetting.Set();
    }

    void Inventory_InventoryObjectOffered(object sender, InventoryObjectOfferedEventArgs e)
	{
        //FIXME
        /*
			Gtk.Application.Invoke(delegate {
                MessageDialog md = new MessageDialog(MainClass.win, DialogFlags.DestroyWithParent, MessageType.Info, ButtonsType.YesNo, false,details.FromAgentName + " has offered you\n"+details.Message+"\n Would you like to accept");
                md.Response += delegate { md.Destroy(); };
                md.ShowAll();
			});
        */
			
	}


    void Parcels_ParcelDwellReply(object sender, ParcelDwellReplyEventArgs e)
    {
        if (this.current_parcelid == e.LocalID)
        {
            Gtk.Application.Invoke(delegate
            {
                current_parcel_dwell = (int)e.Dwell;
                this.updatestatusinfo(true);
            });
        }
   }


//FIXME??
	void onAvatarGroups(UUID avatarID, List<AvatarGroup> avatarGroupsi)
	{
		Console.WriteLine("On Avatar groups");
		// Only interested in self here;
		if(avatarID!=MainClass.client.Self.AgentID)
			return;
		
		Console.WriteLine("GOt list for self");
		avatarGroups.AddRange(avatarGroupsi);
		
	}
	
   [GLib.ConnectBefore]
    void MainWindow_DeleteEvent(object o, DeleteEventArgs args)
    {

		bool hidewindow=MainClass.appsettings.minimise;
		ResponseType result=ResponseType.Close;
		CloseMinimiseDlg cmd=new CloseMinimiseDlg();
		if(!hidewindow)
		{
		    result = (ResponseType)cmd.Run();			
		}
		else
		{
			bool minimise=MainClass.appsettings.default_minimim;
			if(minimise==true)
				result=ResponseType.Accept;		
		}
		
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
                MainClass.userlogout = true;
				LogoutDlg ld = new LogoutDlg();
                ld.Modal = false;
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


   	void Self_AvatarSitResponse(object sender, AvatarSitResponseEventArgs e)
    {
		Gtk.Application.Invoke(delegate{
	        // we sat down
	        togglesat();
		});
    }

    public void stand()
    {
        this.StandingAction.Active = true;
    }
	
	public void togglesat()
	{
		if(this.SittingAction.Sensitive==false)
		{
            this.SittingAction.Sensitive=true;			
            this.SittingAction.Activate();
		    this.SittingAction.Sensitive=false;
		}
		else
			this.SittingAction.Activate();		
    }	


    void Friends_FriendshipTerminated(object sender, FriendshipTerminatedEventArgs e)
    {
        Gtk.Application.Invoke(delegate
        {
            MessageDialog md = new Gtk.MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Info, ButtonsType.Close, true, e.AgentName+" has terminated your friendship");
            ResponseType result = (ResponseType)md.Run();
            md.Destroy();	
        });
    }

    void Friends_FriendshipResponse(object sender, FriendshipResponseEventArgs e)
    {
        Gtk.Application.Invoke(delegate
        {
            string msg = "";
            if (e.Accepted)
            {
                msg = e.AgentName + " accepted your friendship request";
            }
            else
            {
                msg = e.AgentName + " declined your friendship request";
            }

            MessageDialog md = new Gtk.MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Info, ButtonsType.Close, true, msg);
            ResponseType result = (ResponseType)md.Run();
            md.Destroy();	
        });
    }	




    void Self_GroupChatLeft(object sender, GroupChatLeftEventArgs e)
  	{
		Console.Write("Left group chat for session "+e.SessionID.ToString()+"\n");
		if(MainClass.win.active_ims.Contains(e.SessionID))
		   MainClass.win.active_ims.Remove(e.SessionID);
	}

    void Self_ScriptDialog(object sender, ScriptDialogEventArgs e)
	{
        Gtk.Application.Invoke(delegate
        {
            ScriptDialogue d = new ScriptDialogue(e.Message, e.ObjectName, e.ImageID, e.ObjectID, e.FirstName, e.LastName, e.Channel, e.ButtonLabels);
            d.Show();
        });
	}


    void Self_AlertMessage(object sender, AlertMessageEventArgs e)
	{
		if(e.Message=="Autopilot canceled")
		{
			Console.WriteLine("Autopilot cancled");
			return;
		}
		
		Gtk.Application.Invoke(delegate {						
			string msg;
			msg="<b>ALERT FROM SECONDLIFE</b>\n"+e.Message;
			MessageDialog md= new Gtk.MessageDialog(this,DialogFlags.Modal,MessageType.Info,ButtonsType.Close,true,msg);
            md.Response += delegate { md.Destroy(); };
            md.ShowAll();
		});	
	}


	void Self_ScriptQuestion(object sender, ScriptQuestionEventArgs e)
	{
		string message;
		
		switch(e.Questions)
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
			msg="The object : "+e.ObjectName+"Owner by :"+e.ObjectOwnerName+"Would like to \n"+message+"\n Would you like to allow this?";
			MessageDialog md= new Gtk.MessageDialog(this,DialogFlags.DestroyWithParent,MessageType.Question,ButtonsType.YesNo,true,msg);
            md.Response += delegate(object o, ResponseArgs args)
            {
                if (args.ResponseId == ResponseType.Yes)
                {
                    MainClass.client.Self.ScriptQuestionReply(e.Simulator, e.ItemID, e.TaskID, e.Questions);
                }
                else
                {
                    MainClass.client.Self.ScriptQuestionReply(e.Simulator, e.ItemID, e.TaskID, ScriptPermission.None);
                }
                md.Destroy();
            };

            md.ShowAll();	
		});
	}
	
	public Gtk.Notebook getnotebook()
	{
		return this.notebook;
	}

    void Friends_FriendshipOffered(object sender, FriendshipOfferedEventArgs e)
	{
		Gtk.Application.Invoke(delegate {						
		
			string msg;
			msg="You have recieved a friendship request from "+e.AgentName+"\n They would like to become your friend \n do you want to accept?";
			MessageDialog md= new Gtk.MessageDialog(this,DialogFlags.DestroyWithParent,MessageType.Question,ButtonsType.YesNo,true,msg);

            md.Response += delegate(object o, ResponseArgs args)
            {
                if (args.ResponseId == ResponseType.Yes)
                {
                    MainClass.client.Friends.AcceptFriendship(e.AgentID, e.SessionID);
                }
                else
                {
                    MainClass.client.Friends.DeclineFriendship(e.AgentID, e.SessionID);
                }

                md.Destroy();
            };
            md.ShowAll();	
		});			
	}
			
	Gtk.Label addtabwithicon(string filename,string label,Gtk.Widget contents)
	{
		Gtk.Image image=new Gtk.Image();
		image.Pixbuf=MainClass.GetResource(""+filename);
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



    void Network_Disconnected(object sender, DisconnectedEventArgs e)
    {
		Gtk.Application.Invoke(delegate {						
			if(status_icons!=null)
				status_icons.Destroy();

			status_location.Text="Location: Unknown (0,0,0)";
			status_balance_lable.Text="?";
			status_parcel.Text="Parcel: Unknown";

		});
	}


		
    void Self_TeleportProgress(object sender, TeleportEventArgs e)
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
		if((parcel.Flags & OpenMetaverse.ParcelFlags.AllowFly) != OpenMetaverse.ParcelFlags.AllowFly )
		{
			Gtk.Image myimage=new Gtk.Image(MainClass.GetResource("status_no_fly.png"));
			status_icons.PackStart(myimage);
			status_icons.SetChildPacking(myimage,false,false,0,PackType.Start);
		}
		if((parcel.Flags & OpenMetaverse.ParcelFlags.RestrictPushObject)==OpenMetaverse.ParcelFlags.RestrictPushObject)
		{
			Gtk.Image myimage=new Gtk.Image(MainClass.GetResource("status_no_push.png"));
			status_icons.PackStart(myimage);				
			status_icons.SetChildPacking(myimage,false,false,0,PackType.Start);

		}

		if((parcel.Flags & OpenMetaverse.ParcelFlags.AllowOtherScripts)!=OpenMetaverse.ParcelFlags.AllowOtherScripts)
		{
			Gtk.Image myimage=new Gtk.Image(MainClass.GetResource("status_no_scripts.png"));
			status_icons.PackStart(myimage);				
			status_icons.SetChildPacking(myimage,false,false,0,PackType.Start);
		
		}

		if((parcel.Flags & OpenMetaverse.ParcelFlags.CreateObjects)!=OpenMetaverse.ParcelFlags.CreateObjects)
		{
			Gtk.Image myimage=new Gtk.Image(MainClass.GetResource("status_no_build.png"));
			status_icons.PackStart(myimage);				
			status_icons.SetChildPacking(myimage,false,false,0,PackType.Start);

		}

		status_icons.ShowAll();	
	}

    void updatestatusinfo(bool callback)
    {
        Parcel parcel;
        Vector3 pos = MainClass.client.Self.RelativePosition;
        
        //Clamp values, sim crossings can produce values outside the expected range my +-30m
        if (pos.Y > 255)
            pos.Y = 255;
        if (pos.X > 255)
            pos.X = 255;
        if (pos.Y < 0)
            pos.Y = 0;
        if (pos.X < 0)
            pos.X = 0;

        int parcelid = MainClass.client.Network.CurrentSim.ParcelMap[(int)(64.0 * (pos.Y/256.0)), (int)(64.0 * (pos.X/256))];
        
        if (!callback && (parcelid == lastparcelid))
            return;

        if (!MainClass.client.Network.CurrentSim.Parcels.TryGetValue(parcelid, out parcel))
            return;
				
		if(lastparcelid != parcelid)
		{
			this.parcel_owner_name = "Waiting....";
			this.parcel_group = "Waiting....";
			this.current_parcelid=parcel.LocalID;			
			this.is_parcel_owner=(parcel.OwnerID==MainClass.client.Self.AgentID);

			if (parcel.IsGroupOwned == false)
	        {
	            AsyncNameUpdate an;
	            an = new AsyncNameUpdate(parcel.OwnerID, false);
	            an.onNameCallBack += delegate(string namex, object[] values) { this.parcel_owner_name = namex; updatestatusinfo(true); };
	            an.go();
	        }
	        else
	        {
	            this.parcel_owner_name = "(group)";
	        }
	         
	        AsyncNameUpdate an2 = new AsyncNameUpdate(parcel.GroupID, true);
	        an2.onGroupNameCallBack += delegate(string namex, object[] values) { this.parcel_group = namex; updatestatusinfo(true); };
	        an2.go();

			MainClass.client.Parcels.RequestDwell(MainClass.client.Network.CurrentSim,parcel.LocalID);
            

			lastparcelid = parcelid;

			Gtk.Application.Invoke(delegate {		
				doicons(parcel);
			});
		
		}
		
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
                    + "\nTraffic : " + this.current_parcel_dwell.ToString()
                    + "\nArea : " + parcel.Area.ToString();

        tooltips1.SetTip(this.statusbar1, tooltext, null);
        tooltips1.Enable();
 
    }



    void Self_MoneyBalanceReply(object sender, MoneyBalanceReplyEventArgs e)
    {
		Gtk.Application.Invoke(delegate {
			status_balance_lable.Text=e.Balance.ToString();
		});
	}
	
    void Network_LoginProgress(object sender, LoginProgressEventArgs e)
	{
        if (e.Status == LoginStatus.Success)
        {
            MainClass.client.Self.RequestBalance();
			MainClass.client.Avatars.RequestAvatarProperties(MainClass.client.Self.AgentID);
            Thread app = new Thread(new ThreadStart(setappearance));
            app.Start();

			Gtk.Application.Invoke(delegate
            {
                OnUpdateStatus();
                this.AvaiableAction.Sensitive = true;
                this.AwayAction.Sensitive = true;
                this.BusyAction.Sensitive = false;
                this.StandingAction.Sensitive = true;
                this.CrouchAction.Sensitive = true;
                this.FlyAction.Sensitive = true;
                this.GroundSitAction.Sensitive = true;
                this.SittingAction.Sensitive = false;

                MainClass.client.Groups.RequestCurrentGroups();
                trayIcon.Tooltip = "Logged in as\n" + MainClass.client.Self.Name;

            });
        }
        else
        {
            Gtk.Application.Invoke(delegate
            {
                trayIcon.Tooltip = "Status: " + e.Status.ToString();
               
            });
        }
        Gtk.Application.Invoke(delegate
        {
            this.statusbar1.Pop(1);
            this.statusbar1.Push(1, e.Status.ToString());
        });
	}

    
	bool OnUpdateStatus()
	{

        if (MainClass.client == null)
            return true;

		if(MainClass.client.Network.LoginStatusCode==LoginStatus.Success)
		{
			status_location.Text="Location: "+MainClass.client.Network.CurrentSim.Name+MainClass.prettyvector(MainClass.client.Self.SimPosition,2);
            updatestatusinfo(false);
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
		Gtk.Image image=new Gtk.Image(MainClass.GetResource("closebox.png"));
		image.HeightRequest=16;
		image.WidthRequest=16;
		
		Gtk.Image icon;
		
		if(group)
			icon=new Gtk.Image(MainClass.GetResource("icon_group.png"));
		else
			icon=new Gtk.Image(MainClass.GetResource("icn_voice-groupfocus.png"));
		
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

		if(target!=UUID.Zero)
		{
			if(group)
				ud=new AsyncNameUpdate(target,true);
			else		
				ud=new AsyncNameUpdate(target,false);
			
			ud.onNameCallBack += delegate(string namex,object [] values){cs.tabLabel.Text=namex;};
	        ud.go();
		}

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
				makeimwindow("Waiting...",imc,false,target);

				active_ims.Add(target);
			});
		}					
    }
	
    public void startConfrenceIM(List<UUID> targets)
	{
			Gtk.Application.Invoke(delegate {						
				ChatConsole imc=new ChatConsole(targets);
				makeimwindow("Confrence",imc,true,UUID.Zero);
			});
	}
	
 public static string BytesToString(byte[] bytes)
        {
            if (bytes.Length > 0 && bytes[bytes.Length - 1] == 0x00)
                return System.Text.UTF8Encoding.UTF8.GetString(bytes, 0, bytes.Length - 1);
            else
                return System.Text.UTF8Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }


 void Self_IM(object sender, InstantMessageEventArgs e)
 {	
        // Note to self, if i do implement a dispatcher here these two need to be dispatched but not open 
        // new IM tabs so don't treat the same as the rest below.
		if(e.IM.Dialog==OpenMetaverse.InstantMessageDialog.StartTyping)
			return;

        if (e.IM.Dialog == OpenMetaverse.InstantMessageDialog.StopTyping)
            return;

		if(e.IM.Dialog==OpenMetaverse.InstantMessageDialog.GroupNoticeRequested)
			return;

        if (e.IM.Dialog == OpenMetaverse.InstantMessageDialog.InventoryOffered)
            return;
    
		if(e.IM.Dialog==OpenMetaverse.InstantMessageDialog.TaskInventoryOffered)
		    return;
		
        if(e.IM.Dialog==OpenMetaverse.InstantMessageDialog.FriendshipOffered)		
            return;

		if(e.IM.Dialog==OpenMetaverse.InstantMessageDialog.InventoryAccepted)
		{
			Gtk.Application.Invoke(delegate {
                MessageDialog md = new MessageDialog(MainClass.win, DialogFlags.DestroyWithParent, MessageType.Info, ButtonsType.Ok, false,e.IM.FromAgentName + " accepted your inventory offer");
                md.Response += delegate { md.Destroy(); };
                md.ShowAll();
			});
			return;
		}
		
		if(e.IM.Dialog==OpenMetaverse.InstantMessageDialog.GroupNotice)
		{
			//Hmm need to handle this differently than a standard IM
			Gtk.Application.Invoke(delegate {	
				MessageDialog md = new MessageDialog(MainClass.win,DialogFlags.DestroyWithParent,MessageType.Info,ButtonsType.Ok,false,"GROUP NOTICE\nFrom:"+e.IM.FromAgentName+"\n"+e.IM.Message);
                md.Response += delegate { md.Destroy(); };
                md.ShowAll();	
			});
			return;
		}
		
		if(e.IM.Dialog==OpenMetaverse.InstantMessageDialog.GroupInvitation)
		{
			Gtk.Application.Invoke(delegate {	
				MessageDialog md = new MessageDialog(MainClass.win,DialogFlags.DestroyWithParent,MessageType.Question,ButtonsType.YesNo,false,e.IM.FromAgentName+" has invited you to join a group\n"+e.IM.Message+"\nPress yes to accept or no to decline");
				md.Response += delegate(object o,ResponseArgs args) 
				{
					if(args.ResponseId==ResponseType.Yes)
					{
	                    MainClass.client.Self.InstantMessage(MainClass.client.Self.Name,e.IM.FromAgentID,"",e.IM.IMSessionID,InstantMessageDialog.GroupInvitationAccept,InstantMessageOnline.Offline,MainClass.client.Self.RelativePosition,MainClass.client.Network.CurrentSim.ID,null);
				    }
					else
					{
	                    MainClass.client.Self.InstantMessage(MainClass.client.Self.Name,e.IM.FromAgentID,"",e.IM.IMSessionID,InstantMessageDialog.GroupInvitationDecline,InstantMessageOnline.Offline,MainClass.client.Self.RelativePosition,MainClass.client.Network.CurrentSim.ID,null); 	
	                }
					md.Destroy();	
         		};	
				md.ShowAll();	

                return;				
            });			
        }		
		
		if(e.IM.Dialog==OpenMetaverse.InstantMessageDialog.RequestTeleport)
		{
			//Hmm need to handle this differently than a standard IM
			Gtk.Application.Invoke(delegate {
                MessageDialog md = new MessageDialog(MainClass.win, DialogFlags.DestroyWithParent, MessageType.Question, ButtonsType.YesNo, false,e.IM.FromAgentName + " would like you to join them\n" + e.IM.Message + "\nPress yes to teleport or no to ignore");
				md.Response += delegate(object o,ResponseArgs args) 
                {
					if(args.ResponseId==ResponseType.Yes)
					{
						MainClass.client.Self.TeleportLureRespond(e.IM.FromAgentID,true);
					}
					else
					{
						MainClass.client.Self.TeleportLureRespond(e.IM.FromAgentID,false);
					}
					md.Destroy();
			     };
	             md.ShowAll();	
			});
			
			return;
			
		}
		
        if (e.IM.Dialog==InstantMessageDialog.MessageFromObject)
            return; //Its an object Im, chat will grab this for us

        if (im_windows.ContainsKey(e.IM.IMSessionID))
		    return; // Do nothing handler is registered
		
		if(active_ims.Contains(e.IM.IMSessionID))
           return;

        //We only want the following stuff here for a new window create
        if (e.IM.Dialog != OpenMetaverse.InstantMessageDialog.MessageFromAgent &&
            e.IM.Dialog != OpenMetaverse.InstantMessageDialog.SessionSend &&
            e.IM.Dialog != OpenMetaverse.InstantMessageDialog.SessionGroupStart &&
            e.IM.Dialog != OpenMetaverse.InstantMessageDialog.BusyAutoResponse &&
            e.IM.Dialog != OpenMetaverse.InstantMessageDialog.SessionAdd
            )
        {
            return;
        }

        if (e.IM.BinaryBucket.Length>1)
        {           
              Gtk.Application.Invoke(delegate {	
					ChatConsole imc=new ChatConsole(e.IM);
					string lable;
	                lable=BytesToString(e.IM.BinaryBucket);
                    makeimwindow(lable, imc, true, e.IM.IMSessionID);
                    active_ims.Add(e.IM.IMSessionID);
            });
            return;
  		}		
  
            lock (MainClass.win.im_queue)
            {
                if (im_windows.ContainsKey(e.IM.FromAgentID))
                    return; // Do nothing handler is registered

                im_queue.Add(e.IM);

                if (im_registering.Contains(e.IM.FromAgentID))
                    Console.WriteLine("Got 2nd IM when we are still processing window");
                else
                {
                    im_registering.Add(e.IM.FromAgentID);
                    Gtk.Application.Invoke(delegate
                    {
                        ChatConsole imc = new ChatConsole(e.IM);

                        makeimwindow("Waiting...", imc, false, e.IM.FromAgentID);
                        active_ims.Add(e.IM.FromAgentID);
                    });
                }
            }
	}

	protected virtual void OnAvaiableActionActivated (object sender, System.EventArgs e)
	{
        if (MainClass.client != null)
        {
            MainClass.client.Self.Movement.Away = false;
            MainClass.client.Self.Movement.SendUpdate(true);
        }
	}

	protected virtual void OnBusyActionActivated (object sender, System.EventArgs e)
	{
	   	
	}

	protected virtual void OnAwayActionActivated (object sender, System.EventArgs e)
	{
        if (MainClass.client != null)
        {
            MainClass.client.Self.Movement.Away = true;
            MainClass.client.Self.Movement.SendUpdate(true);
        }
		
	}

	protected virtual void OnStandingActionActivated (object sender, System.EventArgs e)
	{
        if (MainClass.client != null && this.StandingAction.Active == true)
        { 
			MainClass.client.Self.Stand();
	        MainClass.client.Self.Fly(false);			
			MainClass.client.Self.Crouch(false); 
        }
	}

	protected virtual void OnGroundSitActionActivated (object sender, System.EventArgs e)
	{
        if (MainClass.client != null && this.GroundSitAction.Active == true)		
             MainClass.client.Self.SitOnGround();
	}

	protected virtual void OnCrouchActionActivated (object sender, System.EventArgs e)
	{
        if (MainClass.client != null && this.CrouchAction.Active == true)		
             MainClass.client.Self.Crouch(true); 
	}

	protected virtual void OnFlyActionActivated (object sender, System.EventArgs e)
	{
        if (MainClass.client != null && this.FlyAction.Active == true)
	          MainClass.client.Self.Fly(true);	
    }

    void Groups_CurrentGroups(object sender, CurrentGroupsEventArgs e)
    {
			
			Gtk.Application.Invoke(delegate {
				
				foreach(KeyValuePair <UUID,Group> group in e.Groups)
				{
					if(!this.current_groups.Contains(group.Value))
					{
						this.current_groups.Add(group.Value);
					}
				}
			});

		}
	
	protected virtual void OnParcelActionToggled (object sender, System.EventArgs e)
	{
        if (this.ParcelAction.Active == true)
        {
			if(tab_parcels!=null)
		    {
                tab_parcels.Dispose();
			    tab_parcels=null;	
            }
			
            tab_parcels = new ParcelMgr();
            this.addtabwithicon("parcel.png", "Parcel", tab_parcels);
            this.ParcelAction.Active = true;
            MainClass.appsettings.tab_parcel=true;

        }
        else
		{
			if(tab_parcels!=null)
		    {
                tab_parcels.Dispose();
			    tab_parcels=null;	
            }
            MainClass.appsettings.tab_parcel=false;
        }

	}

	protected virtual void OnObjectsActionToggled (object sender, System.EventArgs e)
	{
        if (this.ObjectsAction.Active == true)
        {
			if(tab_objects!=null)
		    {
                tab_objects.Dispose();
			    tab_objects=null;	
            }
			
            tab_objects = new ObjectsLayout();
			this.addtabwithicon("item_object.png", "Objects", tab_objects);
			this.ObjectsAction.Active=true;
            MainClass.appsettings.tab_objects=true;

        }
        else
        {
			if(tab_objects!=null)
		    {
                tab_objects.Dispose();
			    tab_objects=null;	
            }
            MainClass.appsettings.tab_objects=false;
        }	   
	}

	protected virtual void OnInventoryActionToggled (object sender, System.EventArgs e)
	{

        if (this.InventoryAction.Active == true)
		{
			if(tab_inventory!=null)
			{
				tab_inventory.Dispose();
                tab_inventory=null;
            }
            tab_inventory = new omvviewerlight.Inventory();
            this.addtabwithicon("inv_folder_plain_open.png", "Inventory", tab_inventory);
            this.InventoryAction.Active = true;
            MainClass.appsettings.tab_inv=true;

        }
        else
        {
            tab_inventory.Dispose();
            MainClass.appsettings.tab_inv=false;
            tab_inventory=null;
        }	  
  
	}

	protected virtual void OnLocationActionToggled (object sender, System.EventArgs e)
	{	
        if(this.LocationAction.Active==true)
        {
			if(tab_location!=null)
			{
				tab_location.Dispose();
                tab_location=null;
            }

 		    tab_location=new Location();
		    this.addtabwithicon("icon_place.png","Location",tab_location);
		    this.LocationAction.Active=true;
              MainClass.appsettings.tab_location=true;

	    }
		else
		{
		    if(tab_location!=null)
			{
				tab_location.Dispose();
                tab_location=null;
             }
             MainClass.appsettings.tab_location=false;
        }	
	
	}

	protected virtual void OnGroupsActionToggled (object sender, System.EventArgs e)
	{
        if (this.GroupsAction.Active == true)
		{
			if(tab_groups!=null)
			{
				tab_groups.Dispose();
                tab_groups=null;
            }

            tab_groups = new Groups();
            this.addtabwithicon("icn_voice-groupfocus.png", "Groups", tab_groups);
            this.GroupsAction.Active = true;
             MainClass.appsettings.tab_groups=true;
        }
        else
        {
			if(tab_groups!=null)
			{
				tab_groups.Dispose();
                tab_groups=null;
            }
            MainClass.appsettings.tab_groups=false;
        }	
	}

	protected virtual void OnSearchActionToggled (object sender, System.EventArgs e)
    {	
		if(this.SearchAction.Active==true)
        {
			if(tab_search!=null)
			{
				tab_search.Dispose();
                tab_search=null;
            }
			
	        tab_search=new Search();
			this.addtabwithicon("status_search_btn.png","Search",tab_search);
		    this.SearchAction.Active=true;
             MainClass.appsettings.tab_search=true;
	    }
		else
		{
			if(tab_search!=null)
			{
				tab_search.Dispose();
                tab_search=null;
            }
            MainClass.appsettings.tab_search=false;
        }	
	}

	protected virtual void OnPreferencesActionActivated (object sender, System.EventArgs e)
	{
		Preferences p=new Preferences();
        p.Show();
	}
	
        bool Inventory_OnObjectOffered(InstantMessage offerDetails, AssetType type, UUID objectID, bool fromTask)
        {
			
			AutoResetEvent ObjectOfferEvent = new AutoResetEvent(false);
			ResponseType object_offer_result=ResponseType.Yes;

            string msg = "";
			ResponseType result;
            if (!fromTask)
                msg = "The user "+offerDetails.FromAgentName + " has offered you\n" + offerDetails.Message + "\n Which is a " + type.ToString() + "\nPress Yes to accept or no to decline";
            else
                msg = "The object "+offerDetails.FromAgentName + " has offered you\n" + offerDetails.Message + "\n Which is a " + type.ToString() + "\nPress Yes to accept or no to decline";

			
			Application.Invoke(delegate {			
					ObjectOfferEvent.Reset();

					Gtk.MessageDialog md = new MessageDialog(MainClass.win, DialogFlags.Modal, MessageType.Other, ButtonsType.YesNo, false, msg);
				
					result = (ResponseType)md.Run();
					object_offer_result=result;
				    md.Destroy();
					ObjectOfferEvent.Set();
			});
			

		    ObjectOfferEvent.WaitOne(1000*3600,false);
	
           if (object_offer_result == ResponseType.Yes)
		   {
               if(OnInventoryAccepted!=null)
               {
                  OnInventoryAccepted(type,objectID);
               }
				return true;
		   }
		   else
		{
			    return false;
			}			
        }

        protected virtual void OnBrowserActionActivated (object sender, System.EventArgs e)
		{
		
        //OpenGL mgl=new OpenGL();
              
        }	

        public void setmapwidget(omvviewerlight.Map Mapwidget)
        {
            this.map_widget = Mapwidget;
	}
		protected virtual void OnAboutActionActivated (object sender, System.EventArgs e)
		{
		About ab = new About();
        	
        	ab.Run();
        	ab.Destroy();
        
        }



}