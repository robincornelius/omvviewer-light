// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------



public partial class MainWindow {
    
    private Gtk.UIManager UIManager;
    
    private Gtk.Action StatusAction;
    
    private Gtk.RadioAction BusyAction;
    
    private Gtk.RadioAction AwayAction;
    
    private Gtk.RadioAction GroundSitAction;
    
    private Gtk.RadioAction CrouchAction;
    
    private Gtk.RadioAction FlyAction;
    
    private Gtk.RadioAction AvaiableAction;
    
    private Gtk.RadioAction StandingAction;
    
    private Gtk.RadioAction SittingAction;
    
    private Gtk.Action ToolsAction;
    
    private Gtk.ToggleAction ParcelAction;
    
    private Gtk.ToggleAction ObjectsAction;
    
    private Gtk.ToggleAction InventoryAction;
    
    private Gtk.ToggleAction LocationAction;
    
    private Gtk.ToggleAction GroupsAction;
    
    private Gtk.ToggleAction SearchAction;
    
    private Gtk.Action SettingsAction;
    
    private Gtk.Action PreferencesAction;
    
    private Gtk.Action VIewAction;
    
    private Gtk.Action BrowserAction;
    
    private Gtk.Action HelpAction;
    
    private Gtk.Action AboutAction;
    
    private Gtk.VBox vbox6;
    
    private Gtk.MenuBar menubar1;
    
    private Gtk.Notebook notebook;
    
    private omvviewerlight.LoginControl logincontrol5;
    
    private Gtk.Label label8;
    
    private Gtk.Statusbar statusbar1;
    
    protected virtual void Build() {
        Stetic.Gui.Initialize(this);
        // Widget MainWindow
        this.UIManager = new Gtk.UIManager();
        Gtk.ActionGroup w1 = new Gtk.ActionGroup("Default");
        this.StatusAction = new Gtk.Action("StatusAction", Mono.Unix.Catalog.GetString("Status"), null, null);
        this.StatusAction.ShortLabel = Mono.Unix.Catalog.GetString("Status");
        w1.Add(this.StatusAction, null);
        this.BusyAction = new Gtk.RadioAction("BusyAction", Mono.Unix.Catalog.GetString("Busy"), null, "gtk-dialog-error", 0);
        this.BusyAction.Group = new GLib.SList(System.IntPtr.Zero);
        this.BusyAction.Sensitive = false;
        this.BusyAction.ShortLabel = Mono.Unix.Catalog.GetString("Busy");
        w1.Add(this.BusyAction, null);
        this.AwayAction = new Gtk.RadioAction("AwayAction", Mono.Unix.Catalog.GetString("Away"), null, "gtk-redo", 0);
        this.AwayAction.Group = this.BusyAction.Group;
        this.AwayAction.ShortLabel = Mono.Unix.Catalog.GetString("Away");
        w1.Add(this.AwayAction, null);
        this.GroundSitAction = new Gtk.RadioAction("GroundSitAction", Mono.Unix.Catalog.GetString("Ground Sit"), null, null, 0);
        this.GroundSitAction.Group = new GLib.SList(System.IntPtr.Zero);
        this.GroundSitAction.ShortLabel = Mono.Unix.Catalog.GetString("Ground Sit");
        w1.Add(this.GroundSitAction, null);
        this.CrouchAction = new Gtk.RadioAction("CrouchAction", Mono.Unix.Catalog.GetString("Crouch"), null, null, 0);
        this.CrouchAction.Group = this.GroundSitAction.Group;
        this.CrouchAction.ShortLabel = Mono.Unix.Catalog.GetString("Crouch");
        w1.Add(this.CrouchAction, null);
        this.FlyAction = new Gtk.RadioAction("FlyAction", Mono.Unix.Catalog.GetString("Fly"), null, null, 0);
        this.FlyAction.Group = this.GroundSitAction.Group;
        this.FlyAction.ShortLabel = Mono.Unix.Catalog.GetString("Fly");
        w1.Add(this.FlyAction, null);
        this.AvaiableAction = new Gtk.RadioAction("AvaiableAction", Mono.Unix.Catalog.GetString("Avaiable"), null, "gtk-yes", 0);
        this.AvaiableAction.Group = this.BusyAction.Group;
        this.AvaiableAction.ShortLabel = Mono.Unix.Catalog.GetString("Avaiable");
        w1.Add(this.AvaiableAction, null);
        this.StandingAction = new Gtk.RadioAction("StandingAction", Mono.Unix.Catalog.GetString("Standing"), null, null, 0);
        this.StandingAction.Group = this.FlyAction.Group;
        this.StandingAction.ShortLabel = Mono.Unix.Catalog.GetString("Standing");
        w1.Add(this.StandingAction, null);
        this.SittingAction = new Gtk.RadioAction("SittingAction", Mono.Unix.Catalog.GetString("Sitting"), null, null, 0);
        this.SittingAction.Group = this.StandingAction.Group;
        this.SittingAction.Sensitive = false;
        this.SittingAction.ShortLabel = Mono.Unix.Catalog.GetString("Sitting");
        w1.Add(this.SittingAction, null);
        this.ToolsAction = new Gtk.Action("ToolsAction", Mono.Unix.Catalog.GetString("Tools"), null, "gtk-execute");
        this.ToolsAction.ShortLabel = Mono.Unix.Catalog.GetString("Tools");
        w1.Add(this.ToolsAction, null);
        this.ParcelAction = new Gtk.ToggleAction("ParcelAction", Mono.Unix.Catalog.GetString("Parcel"), null, null);
        this.ParcelAction.ShortLabel = Mono.Unix.Catalog.GetString("Parcel");
        w1.Add(this.ParcelAction, null);
        this.ObjectsAction = new Gtk.ToggleAction("ObjectsAction", Mono.Unix.Catalog.GetString("Objects"), null, null);
        this.ObjectsAction.ShortLabel = Mono.Unix.Catalog.GetString("Objects");
        w1.Add(this.ObjectsAction, null);
        this.InventoryAction = new Gtk.ToggleAction("InventoryAction", Mono.Unix.Catalog.GetString("Inventory"), null, null);
        this.InventoryAction.ShortLabel = Mono.Unix.Catalog.GetString("Inventory");
        w1.Add(this.InventoryAction, null);
        this.LocationAction = new Gtk.ToggleAction("LocationAction", Mono.Unix.Catalog.GetString("Location"), null, null);
        this.LocationAction.ShortLabel = Mono.Unix.Catalog.GetString("Location");
        w1.Add(this.LocationAction, null);
        this.GroupsAction = new Gtk.ToggleAction("GroupsAction", Mono.Unix.Catalog.GetString("Groups"), null, "Groups");
        this.GroupsAction.ShortLabel = Mono.Unix.Catalog.GetString("Groups");
        w1.Add(this.GroupsAction, null);
        this.SearchAction = new Gtk.ToggleAction("SearchAction", Mono.Unix.Catalog.GetString("Search"), null, null);
        this.SearchAction.ShortLabel = Mono.Unix.Catalog.GetString("Search");
        w1.Add(this.SearchAction, null);
        this.SettingsAction = new Gtk.Action("SettingsAction", Mono.Unix.Catalog.GetString("Settings"), null, null);
        this.SettingsAction.ShortLabel = Mono.Unix.Catalog.GetString("Settings");
        w1.Add(this.SettingsAction, null);
        this.PreferencesAction = new Gtk.Action("PreferencesAction", Mono.Unix.Catalog.GetString("Preferences"), null, "gtk-properties");
        this.PreferencesAction.ShortLabel = Mono.Unix.Catalog.GetString("Preferences");
        w1.Add(this.PreferencesAction, null);
        this.VIewAction = new Gtk.Action("VIewAction", Mono.Unix.Catalog.GetString("VIew"), null, null);
        this.VIewAction.ShortLabel = Mono.Unix.Catalog.GetString("VIew");
        w1.Add(this.VIewAction, null);
        this.BrowserAction = new Gtk.Action("BrowserAction", Mono.Unix.Catalog.GetString("Browser"), null, null);
        this.BrowserAction.ShortLabel = Mono.Unix.Catalog.GetString("Browser");
        w1.Add(this.BrowserAction, null);
        this.HelpAction = new Gtk.Action("HelpAction", Mono.Unix.Catalog.GetString("Help"), null, "gtk-help");
        this.HelpAction.ShortLabel = Mono.Unix.Catalog.GetString("Help");
        w1.Add(this.HelpAction, null);
        this.AboutAction = new Gtk.Action("AboutAction", Mono.Unix.Catalog.GetString("About"), null, "gtk-about");
        this.AboutAction.ShortLabel = Mono.Unix.Catalog.GetString("About");
        w1.Add(this.AboutAction, null);
        this.UIManager.InsertActionGroup(w1, 0);
        this.AddAccelGroup(this.UIManager.AccelGroup);
        this.WidthRequest = 800;
        this.HeightRequest = 600;
        this.Name = "MainWindow";
        this.Title = Mono.Unix.Catalog.GetString("MainWindow");
        this.WindowPosition = ((Gtk.WindowPosition)(4));
        this.AllowShrink = true;
        // Container child MainWindow.Gtk.Container+ContainerChild
        this.vbox6 = new Gtk.VBox();
        this.vbox6.Name = "vbox6";
        this.vbox6.Spacing = 6;
        // Container child vbox6.Gtk.Box+BoxChild
        this.UIManager.AddUiFromString("<ui><menubar name='menubar1'><menu name='StatusAction' action='StatusAction'><menuitem name='AvaiableAction' action='AvaiableAction'/><menuitem name='BusyAction' action='BusyAction'/><menuitem name='AwayAction' action='AwayAction'/><separator/><menuitem name='StandingAction' action='StandingAction'/><menuitem name='GroundSitAction' action='GroundSitAction'/><menuitem name='CrouchAction' action='CrouchAction'/><menuitem name='FlyAction' action='FlyAction'/><menuitem name='SittingAction' action='SittingAction'/></menu><menu name='ToolsAction' action='ToolsAction'><menuitem name='ParcelAction' action='ParcelAction'/><menuitem name='ObjectsAction' action='ObjectsAction'/><menuitem name='InventoryAction' action='InventoryAction'/><menuitem name='LocationAction' action='LocationAction'/><menuitem name='GroupsAction' action='GroupsAction'/><menuitem name='SearchAction' action='SearchAction'/></menu><menu name='SettingsAction' action='SettingsAction'><menuitem name='PreferencesAction' action='PreferencesAction'/></menu><menu name='HelpAction' action='HelpAction'><menuitem name='AboutAction' action='AboutAction'/></menu></menubar></ui>");
        this.menubar1 = ((Gtk.MenuBar)(this.UIManager.GetWidget("/menubar1")));
        this.menubar1.Name = "menubar1";
        this.vbox6.Add(this.menubar1);
        Gtk.Box.BoxChild w2 = ((Gtk.Box.BoxChild)(this.vbox6[this.menubar1]));
        w2.Position = 0;
        w2.Expand = false;
        w2.Fill = false;
        // Container child vbox6.Gtk.Box+BoxChild
        this.notebook = new Gtk.Notebook();
        this.notebook.CanFocus = true;
        this.notebook.Name = "notebook";
        this.notebook.CurrentPage = 0;
        this.notebook.Scrollable = true;
        // Container child notebook.Gtk.Notebook+NotebookChild
        this.logincontrol5 = new omvviewerlight.LoginControl();
        this.logincontrol5.Events = ((Gdk.EventMask)(256));
        this.logincontrol5.Name = "logincontrol5";
        this.notebook.Add(this.logincontrol5);
        // Notebook tab
        this.label8 = new Gtk.Label();
        this.label8.Name = "label8";
        this.label8.LabelProp = Mono.Unix.Catalog.GetString("Login");
        this.notebook.SetTabLabel(this.logincontrol5, this.label8);
        this.label8.ShowAll();
        this.vbox6.Add(this.notebook);
        Gtk.Box.BoxChild w4 = ((Gtk.Box.BoxChild)(this.vbox6[this.notebook]));
        w4.Position = 1;
        // Container child vbox6.Gtk.Box+BoxChild
        this.statusbar1 = new Gtk.Statusbar();
        this.statusbar1.HeightRequest = 20;
        this.statusbar1.Name = "statusbar1";
        this.statusbar1.Spacing = 6;
        this.vbox6.Add(this.statusbar1);
        Gtk.Box.BoxChild w5 = ((Gtk.Box.BoxChild)(this.vbox6[this.statusbar1]));
        w5.Position = 2;
        w5.Expand = false;
        w5.Fill = false;
        this.Add(this.vbox6);
        if ((this.Child != null)) {
            this.Child.ShowAll();
        }
        this.DefaultWidth = 812;
        this.DefaultHeight = 629;
        this.Show();
        this.DeleteEvent += new Gtk.DeleteEventHandler(this.OnDeleteEvent);
        this.BusyAction.Activated += new System.EventHandler(this.OnBusyActionActivated);
        this.AwayAction.Activated += new System.EventHandler(this.OnAwayActionActivated);
        this.GroundSitAction.Activated += new System.EventHandler(this.OnGroundSitActionActivated);
        this.CrouchAction.Activated += new System.EventHandler(this.OnCrouchActionActivated);
        this.FlyAction.Activated += new System.EventHandler(this.OnFlyActionActivated);
        this.AvaiableAction.Activated += new System.EventHandler(this.OnAvaiableActionActivated);
        this.StandingAction.Activated += new System.EventHandler(this.OnStandingActionActivated);
        this.ParcelAction.Toggled += new System.EventHandler(this.OnParcelActionToggled);
        this.ObjectsAction.Toggled += new System.EventHandler(this.OnObjectsActionToggled);
        this.InventoryAction.Toggled += new System.EventHandler(this.OnInventoryActionToggled);
        this.LocationAction.Toggled += new System.EventHandler(this.OnLocationActionToggled);
        this.GroupsAction.Toggled += new System.EventHandler(this.OnGroupsActionToggled);
        this.SearchAction.Toggled += new System.EventHandler(this.OnSearchActionToggled);
        this.PreferencesAction.Activated += new System.EventHandler(this.OnPreferencesActionActivated);
        this.BrowserAction.Activated += new System.EventHandler(this.OnBrowserActionActivated);
        this.AboutAction.Activated += new System.EventHandler(this.OnAboutActionActivated);
    }
}
