// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 2.0.50727.42
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace omvviewerlight {
    
    
    public partial class FriendsList {
        
        private Gtk.VBox vbox7;
        
        private Gtk.ScrolledWindow GtkScrolledWindow;
        
        private Gtk.TreeView treeview_friends;
        
        private Gtk.VBox vbox8;
        
        private Gtk.VBox vbox11;
        
        private Gtk.HBox hbox1;
        
        private Gtk.Button button_IM;
        
        private Gtk.Button button_teleport;
        
        private Gtk.HBox hbox3;
        
        private Gtk.Button button_profile;
        
        private Gtk.Button button_pay;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget omvviewerlight.FriendsList
            Stetic.BinContainer.Attach(this);
            this.Name = "omvviewerlight.FriendsList";
            // Container child omvviewerlight.FriendsList.Gtk.Container+ContainerChild
            this.vbox7 = new Gtk.VBox();
            this.vbox7.Name = "vbox7";
            this.vbox7.Spacing = 6;
            // Container child vbox7.Gtk.Box+BoxChild
            this.GtkScrolledWindow = new Gtk.ScrolledWindow();
            this.GtkScrolledWindow.Name = "GtkScrolledWindow";
            this.GtkScrolledWindow.ShadowType = ((Gtk.ShadowType)(1));
            // Container child GtkScrolledWindow.Gtk.Container+ContainerChild
            this.treeview_friends = new Gtk.TreeView();
            this.treeview_friends.WidthRequest = 225;
            this.treeview_friends.CanFocus = true;
            this.treeview_friends.Name = "treeview_friends";
            this.treeview_friends.Reorderable = true;
            this.treeview_friends.HeadersClickable = true;
            this.GtkScrolledWindow.Add(this.treeview_friends);
            this.vbox7.Add(this.GtkScrolledWindow);
            Gtk.Box.BoxChild w2 = ((Gtk.Box.BoxChild)(this.vbox7[this.GtkScrolledWindow]));
            w2.Position = 0;
            // Container child vbox7.Gtk.Box+BoxChild
            this.vbox8 = new Gtk.VBox();
            this.vbox8.Name = "vbox8";
            this.vbox8.Spacing = 6;
            // Container child vbox8.Gtk.Box+BoxChild
            this.vbox11 = new Gtk.VBox();
            this.vbox11.Name = "vbox11";
            this.vbox11.Spacing = 6;
            // Container child vbox11.Gtk.Box+BoxChild
            this.hbox1 = new Gtk.HBox();
            this.hbox1.Name = "hbox1";
            this.hbox1.Homogeneous = true;
            this.hbox1.Spacing = 6;
            // Container child hbox1.Gtk.Box+BoxChild
            this.button_IM = new Gtk.Button();
            this.button_IM.WidthRequest = 70;
            this.button_IM.CanFocus = true;
            this.button_IM.Name = "button_IM";
            this.button_IM.UseUnderline = true;
            this.button_IM.Label = Mono.Unix.Catalog.GetString("IM");
            this.hbox1.Add(this.button_IM);
            Gtk.Box.BoxChild w3 = ((Gtk.Box.BoxChild)(this.hbox1[this.button_IM]));
            w3.Position = 0;
            w3.Expand = false;
            w3.Fill = false;
            // Container child hbox1.Gtk.Box+BoxChild
            this.button_teleport = new Gtk.Button();
            this.button_teleport.WidthRequest = 80;
            this.button_teleport.CanFocus = true;
            this.button_teleport.Name = "button_teleport";
            this.button_teleport.UseUnderline = true;
            this.button_teleport.Label = Mono.Unix.Catalog.GetString("Teleport");
            this.hbox1.Add(this.button_teleport);
            Gtk.Box.BoxChild w4 = ((Gtk.Box.BoxChild)(this.hbox1[this.button_teleport]));
            w4.Position = 1;
            w4.Expand = false;
            w4.Fill = false;
            this.vbox11.Add(this.hbox1);
            Gtk.Box.BoxChild w5 = ((Gtk.Box.BoxChild)(this.vbox11[this.hbox1]));
            w5.Position = 0;
            w5.Expand = false;
            w5.Fill = false;
            this.vbox8.Add(this.vbox11);
            Gtk.Box.BoxChild w6 = ((Gtk.Box.BoxChild)(this.vbox8[this.vbox11]));
            w6.Position = 0;
            w6.Expand = false;
            w6.Fill = false;
            // Container child vbox8.Gtk.Box+BoxChild
            this.hbox3 = new Gtk.HBox();
            this.hbox3.Name = "hbox3";
            this.hbox3.Homogeneous = true;
            this.hbox3.Spacing = 6;
            // Container child hbox3.Gtk.Box+BoxChild
            this.button_profile = new Gtk.Button();
            this.button_profile.WidthRequest = 70;
            this.button_profile.CanFocus = true;
            this.button_profile.Name = "button_profile";
            this.button_profile.UseUnderline = true;
            this.button_profile.Label = Mono.Unix.Catalog.GetString("Profile");
            this.hbox3.Add(this.button_profile);
            Gtk.Box.BoxChild w7 = ((Gtk.Box.BoxChild)(this.hbox3[this.button_profile]));
            w7.Position = 0;
            w7.Expand = false;
            w7.Fill = false;
            // Container child hbox3.Gtk.Box+BoxChild
            this.button_pay = new Gtk.Button();
            this.button_pay.WidthRequest = 80;
            this.button_pay.CanFocus = true;
            this.button_pay.Name = "button_pay";
            this.button_pay.UseUnderline = true;
            this.button_pay.Label = Mono.Unix.Catalog.GetString("Pay");
            this.hbox3.Add(this.button_pay);
            Gtk.Box.BoxChild w8 = ((Gtk.Box.BoxChild)(this.hbox3[this.button_pay]));
            w8.Position = 1;
            w8.Expand = false;
            w8.Fill = false;
            this.vbox8.Add(this.hbox3);
            Gtk.Box.BoxChild w9 = ((Gtk.Box.BoxChild)(this.vbox8[this.hbox3]));
            w9.Position = 1;
            w9.Expand = false;
            w9.Fill = false;
            this.vbox7.Add(this.vbox8);
            Gtk.Box.BoxChild w10 = ((Gtk.Box.BoxChild)(this.vbox7[this.vbox8]));
            w10.Position = 1;
            w10.Expand = false;
            w10.Fill = false;
            this.Add(this.vbox7);
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.Show();
            this.treeview_friends.ButtonPressEvent += new Gtk.ButtonPressEventHandler(this.OnTreeviewFriendsButtonPressEvent);
            this.button_IM.Clicked += new System.EventHandler(this.OnButtonIMClicked);
            this.button_teleport.Clicked += new System.EventHandler(this.OnButtonTeleportClicked);
            this.button_profile.Clicked += new System.EventHandler(this.OnButtonProfileClicked);
            this.button_pay.Clicked += new System.EventHandler(this.OnButtonPayClicked);
        }
    }
}
