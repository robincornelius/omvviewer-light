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
    
    
    public partial class Radar {
        
        private Gtk.VBox vbox1;
        
        private Gtk.ScrolledWindow GtkScrolledWindow;
        
        private Gtk.TreeView treeview_radar;
        
        private Gtk.VBox vbox2;
        
        private Gtk.HBox hbox2;
        
        private Gtk.Button button_im;
        
        private Gtk.Button button_pay;
        
        private Gtk.Button button_profile;
        
        private Gtk.Button button1;
        
        private Gtk.Button button_lookat;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget omvviewerlight.Radar
            Stetic.BinContainer.Attach(this);
            this.Name = "omvviewerlight.Radar";
            // Container child omvviewerlight.Radar.Gtk.Container+ContainerChild
            this.vbox1 = new Gtk.VBox();
            this.vbox1.Name = "vbox1";
            this.vbox1.Spacing = 6;
            // Container child vbox1.Gtk.Box+BoxChild
            this.GtkScrolledWindow = new Gtk.ScrolledWindow();
            this.GtkScrolledWindow.Name = "GtkScrolledWindow";
            this.GtkScrolledWindow.ShadowType = ((Gtk.ShadowType)(1));
            // Container child GtkScrolledWindow.Gtk.Container+ContainerChild
            this.treeview_radar = new Gtk.TreeView();
            this.treeview_radar.WidthRequest = 200;
            this.treeview_radar.CanFocus = true;
            this.treeview_radar.Name = "treeview_radar";
            this.treeview_radar.HeadersClickable = true;
            this.GtkScrolledWindow.Add(this.treeview_radar);
            this.vbox1.Add(this.GtkScrolledWindow);
            Gtk.Box.BoxChild w2 = ((Gtk.Box.BoxChild)(this.vbox1[this.GtkScrolledWindow]));
            w2.Position = 0;
            // Container child vbox1.Gtk.Box+BoxChild
            this.vbox2 = new Gtk.VBox();
            this.vbox2.Name = "vbox2";
            this.vbox2.Spacing = 6;
            // Container child vbox2.Gtk.Box+BoxChild
            this.hbox2 = new Gtk.HBox();
            this.hbox2.Name = "hbox2";
            this.hbox2.Spacing = 6;
            // Container child hbox2.Gtk.Box+BoxChild
            this.button_im = new Gtk.Button();
            this.button_im.CanFocus = true;
            this.button_im.Name = "button_im";
            this.button_im.UseUnderline = true;
            // Container child button_im.Gtk.Container+ContainerChild
            Gtk.Alignment w3 = new Gtk.Alignment(0.5F, 0.5F, 0F, 0F);
            // Container child GtkAlignment.Gtk.Container+ContainerChild
            Gtk.HBox w4 = new Gtk.HBox();
            w4.Spacing = 2;
            // Container child GtkHBox.Gtk.Container+ContainerChild
            Gtk.Image w5 = new Gtk.Image();
            w5.Pixbuf = Gdk.Pixbuf.LoadFromResource("omvviewerlight.art.icn_voice-pvtfocus.tga");
            w4.Add(w5);
            // Container child GtkHBox.Gtk.Container+ContainerChild
            Gtk.Label w7 = new Gtk.Label();
            w7.LabelProp = Mono.Unix.Catalog.GetString("IM");
            w7.UseUnderline = true;
            w4.Add(w7);
            w3.Add(w4);
            this.button_im.Add(w3);
            this.hbox2.Add(this.button_im);
            Gtk.Box.BoxChild w11 = ((Gtk.Box.BoxChild)(this.hbox2[this.button_im]));
            w11.Position = 0;
            w11.Expand = false;
            w11.Fill = false;
            // Container child hbox2.Gtk.Box+BoxChild
            this.button_pay = new Gtk.Button();
            this.button_pay.CanFocus = true;
            this.button_pay.Name = "button_pay";
            this.button_pay.UseUnderline = true;
            // Container child button_pay.Gtk.Container+ContainerChild
            Gtk.Alignment w12 = new Gtk.Alignment(0.5F, 0.5F, 0F, 0F);
            // Container child GtkAlignment.Gtk.Container+ContainerChild
            Gtk.HBox w13 = new Gtk.HBox();
            w13.Spacing = 2;
            // Container child GtkHBox.Gtk.Container+ContainerChild
            Gtk.Image w14 = new Gtk.Image();
            w14.Pixbuf = Gdk.Pixbuf.LoadFromResource("omvviewerlight.art.status_money.tga");
            w13.Add(w14);
            // Container child GtkHBox.Gtk.Container+ContainerChild
            Gtk.Label w16 = new Gtk.Label();
            w16.LabelProp = Mono.Unix.Catalog.GetString("Pay");
            w16.UseUnderline = true;
            w13.Add(w16);
            w12.Add(w13);
            this.button_pay.Add(w12);
            this.hbox2.Add(this.button_pay);
            Gtk.Box.BoxChild w20 = ((Gtk.Box.BoxChild)(this.hbox2[this.button_pay]));
            w20.Position = 1;
            w20.Expand = false;
            w20.Fill = false;
            // Container child hbox2.Gtk.Box+BoxChild
            this.button_profile = new Gtk.Button();
            this.button_profile.CanFocus = true;
            this.button_profile.Name = "button_profile";
            this.button_profile.UseUnderline = true;
            this.button_profile.Label = Mono.Unix.Catalog.GetString("Profile");
            this.hbox2.Add(this.button_profile);
            Gtk.Box.BoxChild w21 = ((Gtk.Box.BoxChild)(this.hbox2[this.button_profile]));
            w21.Position = 2;
            w21.Expand = false;
            w21.Fill = false;
            // Container child hbox2.Gtk.Box+BoxChild
            this.button1 = new Gtk.Button();
            this.button1.CanFocus = true;
            this.button1.Name = "button1";
            this.button1.UseUnderline = true;
            this.button1.Label = Mono.Unix.Catalog.GetString("Follow");
            this.hbox2.Add(this.button1);
            Gtk.Box.BoxChild w22 = ((Gtk.Box.BoxChild)(this.hbox2[this.button1]));
            w22.Position = 3;
            w22.Expand = false;
            w22.Fill = false;
            // Container child hbox2.Gtk.Box+BoxChild
            this.button_lookat = new Gtk.Button();
            this.button_lookat.CanFocus = true;
            this.button_lookat.Name = "button_lookat";
            this.button_lookat.UseUnderline = true;
            this.button_lookat.Label = Mono.Unix.Catalog.GetString("Look at");
            this.hbox2.Add(this.button_lookat);
            Gtk.Box.BoxChild w23 = ((Gtk.Box.BoxChild)(this.hbox2[this.button_lookat]));
            w23.Position = 4;
            w23.Expand = false;
            w23.Fill = false;
            this.vbox2.Add(this.hbox2);
            Gtk.Box.BoxChild w24 = ((Gtk.Box.BoxChild)(this.vbox2[this.hbox2]));
            w24.Position = 0;
            w24.Expand = false;
            w24.Fill = false;
            this.vbox1.Add(this.vbox2);
            Gtk.Box.BoxChild w25 = ((Gtk.Box.BoxChild)(this.vbox1[this.vbox2]));
            w25.Position = 1;
            w25.Expand = false;
            w25.Fill = false;
            this.Add(this.vbox1);
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.Show();
            this.button_im.Clicked += new System.EventHandler(this.OnButtonImClicked);
            this.button_pay.Clicked += new System.EventHandler(this.OnButtonPayClicked);
            this.button_profile.Clicked += new System.EventHandler(this.OnButtonProfileClicked);
            this.button1.Clicked += new System.EventHandler(this.OnButton1Clicked);
            this.button_lookat.Clicked += new System.EventHandler(this.OnButtonLookatClicked);
        }
    }
}
