// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      
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
        
        private Gtk.Button button_pay;
        
        private Gtk.Button button_profile;
        
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
            this.button_IM.CanFocus = true;
            this.button_IM.Name = "button_IM";
            this.button_IM.UseUnderline = true;
            // Container child button_IM.Gtk.Container+ContainerChild
            Gtk.Alignment w3 = new Gtk.Alignment(0.5F, 0.5F, 0F, 0F);
            // Container child GtkAlignment.Gtk.Container+ContainerChild
            Gtk.HBox w4 = new Gtk.HBox();
            w4.Spacing = 2;
            // Container child GtkHBox.Gtk.Container+ContainerChild
            Gtk.Image w5 = new Gtk.Image();
            w5.Pixbuf = MainClass.GetResource("icn_voice-groupfocus.png");
            w4.Add(w5);
            // Container child GtkHBox.Gtk.Container+ContainerChild
            Gtk.Label w7 = new Gtk.Label();
            w7.LabelProp = Mono.Unix.Catalog.GetString("IM");
            w7.UseUnderline = true;
            w4.Add(w7);
            w3.Add(w4);
            this.button_IM.Add(w3);
            this.hbox1.Add(this.button_IM);
            Gtk.Box.BoxChild w11 = ((Gtk.Box.BoxChild)(this.hbox1[this.button_IM]));
            w11.Position = 0;
            w11.Expand = false;
            w11.Fill = false;
            // Container child hbox1.Gtk.Box+BoxChild
            this.button_teleport = new Gtk.Button();
            this.button_teleport.CanFocus = true;
            this.button_teleport.Name = "button_teleport";
            this.button_teleport.UseUnderline = true;
            // Container child button_teleport.Gtk.Container+ContainerChild
            Gtk.Alignment w12 = new Gtk.Alignment(0.5F, 0.5F, 0F, 0F);
            // Container child GtkAlignment.Gtk.Container+ContainerChild
            Gtk.HBox w13 = new Gtk.HBox();
            w13.Spacing = 2;
            // Container child GtkHBox.Gtk.Container+ContainerChild
            Gtk.Image w14 = new Gtk.Image();
            w14.Pixbuf = MainClass.GetResource("icon_place.png");
            w13.Add(w14);
            // Container child GtkHBox.Gtk.Container+ContainerChild
            Gtk.Label w16 = new Gtk.Label();
            w16.LabelProp = Mono.Unix.Catalog.GetString("Teleport");
            w16.UseUnderline = true;
            w13.Add(w16);
            w12.Add(w13);
            this.button_teleport.Add(w12);
            this.hbox1.Add(this.button_teleport);
            Gtk.Box.BoxChild w20 = ((Gtk.Box.BoxChild)(this.hbox1[this.button_teleport]));
            w20.Position = 1;
            w20.Expand = false;
            w20.Fill = false;
            // Container child hbox1.Gtk.Box+BoxChild
            this.button_pay = new Gtk.Button();
            this.button_pay.CanFocus = true;
            this.button_pay.Name = "button_pay";
            this.button_pay.UseUnderline = true;
            // Container child button_pay.Gtk.Container+ContainerChild
            Gtk.Alignment w21 = new Gtk.Alignment(0.5F, 0.5F, 0F, 0F);
            // Container child GtkAlignment.Gtk.Container+ContainerChild
            Gtk.HBox w22 = new Gtk.HBox();
            w22.Spacing = 2;
            // Container child GtkHBox.Gtk.Container+ContainerChild
            Gtk.Image w23 = new Gtk.Image();
            w23.Pixbuf = MainClass.GetResource("status_money.png");
            w22.Add(w23);
            // Container child GtkHBox.Gtk.Container+ContainerChild
            Gtk.Label w25 = new Gtk.Label();
            w25.LabelProp = Mono.Unix.Catalog.GetString("Pay");
            w25.UseUnderline = true;
            w22.Add(w25);
            w21.Add(w22);
            this.button_pay.Add(w21);
            this.hbox1.Add(this.button_pay);
            Gtk.Box.BoxChild w29 = ((Gtk.Box.BoxChild)(this.hbox1[this.button_pay]));
            w29.PackType = ((Gtk.PackType)(1));
            w29.Position = 2;
            w29.Expand = false;
            w29.Fill = false;
            // Container child hbox1.Gtk.Box+BoxChild
            this.button_profile = new Gtk.Button();
            this.button_profile.CanFocus = true;
            this.button_profile.Name = "button_profile";
            this.button_profile.UseUnderline = true;
            this.button_profile.Label = Mono.Unix.Catalog.GetString("Profile");
            this.hbox1.Add(this.button_profile);
            Gtk.Box.BoxChild w30 = ((Gtk.Box.BoxChild)(this.hbox1[this.button_profile]));
            w30.PackType = ((Gtk.PackType)(1));
            w30.Position = 3;
            w30.Expand = false;
            w30.Fill = false;
            this.vbox11.Add(this.hbox1);
            Gtk.Box.BoxChild w31 = ((Gtk.Box.BoxChild)(this.vbox11[this.hbox1]));
            w31.Position = 0;
            w31.Expand = false;
            w31.Fill = false;
            this.vbox8.Add(this.vbox11);
            Gtk.Box.BoxChild w32 = ((Gtk.Box.BoxChild)(this.vbox8[this.vbox11]));
            w32.Position = 0;
            w32.Expand = false;
            w32.Fill = false;
            this.vbox7.Add(this.vbox8);
            Gtk.Box.BoxChild w33 = ((Gtk.Box.BoxChild)(this.vbox7[this.vbox8]));
            w33.Position = 1;
            w33.Expand = false;
            w33.Fill = false;
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
