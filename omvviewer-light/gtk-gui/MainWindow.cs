// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 2.0.50727.42
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------



public partial class MainWindow {
    
    private Gtk.Action StatusAction;
    
    private Gtk.RadioAction BusyAction;
    
    private Gtk.RadioAction AwayAction;
    
    private Gtk.RadioAction GroundSitAction;
    
    private Gtk.RadioAction CrouchAction;
    
    private Gtk.RadioAction FlyAction;
    
    private Gtk.RadioAction AvaiableAction;
    
    private Gtk.RadioAction StandingAction;
    
    private Gtk.RadioAction SittingAction;
    
    private Gtk.VBox vbox6;
    
    private Gtk.MenuBar menubar1;
    
    private Gtk.Notebook notebook;
    
    private omvviewerlight.LoginControl logincontrol5;
    
    private Gtk.Label label8;
    
    private Gtk.Statusbar statusbar1;
    
    protected virtual void Build() {
        Stetic.Gui.Initialize(this);
        // Widget MainWindow
        Gtk.UIManager w1 = new Gtk.UIManager();
        Gtk.ActionGroup w2 = new Gtk.ActionGroup("Default");
        this.StatusAction = new Gtk.Action("StatusAction", Mono.Unix.Catalog.GetString("Status"), null, null);
        this.StatusAction.ShortLabel = Mono.Unix.Catalog.GetString("Status");
        w2.Add(this.StatusAction, null);
        this.BusyAction = new Gtk.RadioAction("BusyAction", Mono.Unix.Catalog.GetString("Busy"), null, null, 0);
        this.BusyAction.Group = new GLib.SList(System.IntPtr.Zero);
        this.BusyAction.Sensitive = false;
        this.BusyAction.ShortLabel = Mono.Unix.Catalog.GetString("Busy");
        w2.Add(this.BusyAction, null);
        this.AwayAction = new Gtk.RadioAction("AwayAction", Mono.Unix.Catalog.GetString("Away"), null, null, 0);
        this.AwayAction.Group = this.BusyAction.Group;
        this.AwayAction.ShortLabel = Mono.Unix.Catalog.GetString("Away");
        w2.Add(this.AwayAction, null);
        this.GroundSitAction = new Gtk.RadioAction("GroundSitAction", Mono.Unix.Catalog.GetString("Ground Sit"), null, null, 0);
        this.GroundSitAction.Group = new GLib.SList(System.IntPtr.Zero);
        this.GroundSitAction.ShortLabel = Mono.Unix.Catalog.GetString("Ground Sit");
        w2.Add(this.GroundSitAction, null);
        this.CrouchAction = new Gtk.RadioAction("CrouchAction", Mono.Unix.Catalog.GetString("Crouch"), null, null, 0);
        this.CrouchAction.Group = this.GroundSitAction.Group;
        this.CrouchAction.ShortLabel = Mono.Unix.Catalog.GetString("Crouch");
        w2.Add(this.CrouchAction, null);
        this.FlyAction = new Gtk.RadioAction("FlyAction", Mono.Unix.Catalog.GetString("Fly"), null, null, 0);
        this.FlyAction.Group = this.CrouchAction.Group;
        this.FlyAction.ShortLabel = Mono.Unix.Catalog.GetString("Fly");
        w2.Add(this.FlyAction, null);
        this.AvaiableAction = new Gtk.RadioAction("AvaiableAction", Mono.Unix.Catalog.GetString("Avaiable"), null, null, 0);
        this.AvaiableAction.Group = this.AwayAction.Group;
        this.AvaiableAction.ShortLabel = Mono.Unix.Catalog.GetString("Avaiable");
        w2.Add(this.AvaiableAction, null);
        this.StandingAction = new Gtk.RadioAction("StandingAction", Mono.Unix.Catalog.GetString("Standing"), null, null, 0);
        this.StandingAction.Group = this.CrouchAction.Group;
        this.StandingAction.ShortLabel = Mono.Unix.Catalog.GetString("Standing");
        w2.Add(this.StandingAction, null);
        this.SittingAction = new Gtk.RadioAction("SittingAction", Mono.Unix.Catalog.GetString("Sitting"), null, null, 0);
        this.SittingAction.Group = this.StandingAction.Group;
        this.SittingAction.Sensitive = false;
        this.SittingAction.ShortLabel = Mono.Unix.Catalog.GetString("Sitting");
        w2.Add(this.SittingAction, null);
        w1.InsertActionGroup(w2, 0);
        this.AddAccelGroup(w1.AccelGroup);
        this.WidthRequest = 800;
        this.HeightRequest = 600;
        this.Name = "MainWindow";
        this.Title = Mono.Unix.Catalog.GetString("MainWindow");
        this.WindowPosition = ((Gtk.WindowPosition)(4));
        // Container child MainWindow.Gtk.Container+ContainerChild
        this.vbox6 = new Gtk.VBox();
        this.vbox6.Name = "vbox6";
        this.vbox6.Spacing = 6;
        // Container child vbox6.Gtk.Box+BoxChild
        w1.AddUiFromString("<ui><menubar name='menubar1'><menu action='StatusAction'><menuitem action='AvaiableAction'/><menuitem action='BusyAction'/><menuitem action='AwayAction'/><separator/><menuitem action='StandingAction'/><menuitem action='GroundSitAction'/><menuitem action='CrouchAction'/><menuitem action='FlyAction'/><menuitem action='SittingAction'/></menu></menubar></ui>");
        this.menubar1 = ((Gtk.MenuBar)(w1.GetWidget("/menubar1")));
        this.menubar1.Name = "menubar1";
        this.vbox6.Add(this.menubar1);
        Gtk.Box.BoxChild w3 = ((Gtk.Box.BoxChild)(this.vbox6[this.menubar1]));
        w3.Position = 0;
        w3.Expand = false;
        w3.Fill = false;
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
        Gtk.Box.BoxChild w5 = ((Gtk.Box.BoxChild)(this.vbox6[this.notebook]));
        w5.Position = 1;
        // Container child vbox6.Gtk.Box+BoxChild
        this.statusbar1 = new Gtk.Statusbar();
        this.statusbar1.HeightRequest = 20;
        this.statusbar1.Name = "statusbar1";
        this.statusbar1.Spacing = 6;
        this.vbox6.Add(this.statusbar1);
        Gtk.Box.BoxChild w6 = ((Gtk.Box.BoxChild)(this.vbox6[this.statusbar1]));
        w6.Position = 3;
        w6.Expand = false;
        w6.Fill = false;
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
    }
}
