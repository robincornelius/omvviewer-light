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
    
    
    public partial class LoginControl {
        
        private Gtk.VBox vbox1;
        
        private Gtk.HBox hbox1;
        
        private Gtk.VBox vbox2;
        
        private Gtk.Label label1;
        
        private Gtk.Label label2;
        
        private Gtk.Label label3;
        
        private Gtk.VBox vbox3;
        
        private Gtk.Entry entry_first;
        
        private Gtk.Entry entry_last;
        
        private Gtk.Entry entry_pass;
        
        private Gtk.Button button_login;
        
        private Gtk.VBox vbox5;
        
        private Gtk.HBox hbox2;
        
        private Gtk.CheckButton checkbutton_rememberpass;
        
        private Gtk.HBox hbox5;
        
        private Gtk.Label label6;
        
        private Gtk.HBox hbox4;
        
        private Gtk.RadioButton radiobutton1;
        
        private Gtk.RadioButton radiobutton2;
        
        private Gtk.RadioButton radiobutton3;
        
        private Gtk.Entry entry_location;
        
        private Gtk.ProgressBar progressbar2;
        
        private Gtk.HBox hbox6;
        
        private Gtk.Label label4;
        
        private Gtk.ComboBox combobox_grid;
        
        private Gtk.Label label5;
        
        private Gtk.Entry entry_loginuri;
        
        private Gtk.Frame frame1;
        
        private Gtk.Alignment GtkAlignment1;
        
        private Gtk.TextView textview_loginmsg;
        
        private Gtk.Label GtkLabel5;
        
        private Gtk.Frame frame2;
        
        private Gtk.Alignment GtkAlignment2;
        
        private Gtk.ScrolledWindow GtkScrolledWindow1;
        
        private Gtk.TextView textview_log;
        
        private Gtk.Label GtkLabel6;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget omvviewerlight.LoginControl
            Stetic.BinContainer.Attach(this);
            this.Name = "omvviewerlight.LoginControl";
            // Container child omvviewerlight.LoginControl.Gtk.Container+ContainerChild
            this.vbox1 = new Gtk.VBox();
            this.vbox1.Name = "vbox1";
            this.vbox1.Spacing = 6;
            // Container child vbox1.Gtk.Box+BoxChild
            this.hbox1 = new Gtk.HBox();
            this.hbox1.Name = "hbox1";
            this.hbox1.Spacing = 6;
            // Container child hbox1.Gtk.Box+BoxChild
            this.vbox2 = new Gtk.VBox();
            this.vbox2.Name = "vbox2";
            this.vbox2.Homogeneous = true;
            this.vbox2.Spacing = 6;
            // Container child vbox2.Gtk.Box+BoxChild
            this.label1 = new Gtk.Label();
            this.label1.Name = "label1";
            this.label1.LabelProp = Mono.Unix.Catalog.GetString("First name");
            this.vbox2.Add(this.label1);
            Gtk.Box.BoxChild w1 = ((Gtk.Box.BoxChild)(this.vbox2[this.label1]));
            w1.Position = 0;
            w1.Expand = false;
            w1.Fill = false;
            // Container child vbox2.Gtk.Box+BoxChild
            this.label2 = new Gtk.Label();
            this.label2.Name = "label2";
            this.label2.LabelProp = Mono.Unix.Catalog.GetString("Last Name");
            this.vbox2.Add(this.label2);
            Gtk.Box.BoxChild w2 = ((Gtk.Box.BoxChild)(this.vbox2[this.label2]));
            w2.Position = 1;
            w2.Expand = false;
            w2.Fill = false;
            // Container child vbox2.Gtk.Box+BoxChild
            this.label3 = new Gtk.Label();
            this.label3.Name = "label3";
            this.label3.LabelProp = Mono.Unix.Catalog.GetString("Password");
            this.vbox2.Add(this.label3);
            Gtk.Box.BoxChild w3 = ((Gtk.Box.BoxChild)(this.vbox2[this.label3]));
            w3.Position = 2;
            w3.Expand = false;
            w3.Fill = false;
            this.hbox1.Add(this.vbox2);
            Gtk.Box.BoxChild w4 = ((Gtk.Box.BoxChild)(this.hbox1[this.vbox2]));
            w4.Position = 0;
            w4.Expand = false;
            w4.Fill = false;
            // Container child hbox1.Gtk.Box+BoxChild
            this.vbox3 = new Gtk.VBox();
            this.vbox3.Name = "vbox3";
            this.vbox3.Spacing = 6;
            // Container child vbox3.Gtk.Box+BoxChild
            this.entry_first = new Gtk.Entry();
            this.entry_first.CanFocus = true;
            this.entry_first.Name = "entry_first";
            this.entry_first.IsEditable = true;
            this.entry_first.InvisibleChar = '●';
            this.vbox3.Add(this.entry_first);
            Gtk.Box.BoxChild w5 = ((Gtk.Box.BoxChild)(this.vbox3[this.entry_first]));
            w5.Position = 0;
            w5.Expand = false;
            w5.Fill = false;
            // Container child vbox3.Gtk.Box+BoxChild
            this.entry_last = new Gtk.Entry();
            this.entry_last.CanFocus = true;
            this.entry_last.Name = "entry_last";
            this.entry_last.IsEditable = true;
            this.entry_last.InvisibleChar = '●';
            this.vbox3.Add(this.entry_last);
            Gtk.Box.BoxChild w6 = ((Gtk.Box.BoxChild)(this.vbox3[this.entry_last]));
            w6.Position = 1;
            w6.Expand = false;
            w6.Fill = false;
            // Container child vbox3.Gtk.Box+BoxChild
            this.entry_pass = new Gtk.Entry();
            this.entry_pass.TooltipMarkup = "Enter your login password here";
            this.entry_pass.CanFocus = true;
            this.entry_pass.Name = "entry_pass";
            this.entry_pass.IsEditable = true;
            this.entry_pass.InvisibleChar = '●';
            this.vbox3.Add(this.entry_pass);
            Gtk.Box.BoxChild w7 = ((Gtk.Box.BoxChild)(this.vbox3[this.entry_pass]));
            w7.Position = 2;
            w7.Expand = false;
            w7.Fill = false;
            this.hbox1.Add(this.vbox3);
            Gtk.Box.BoxChild w8 = ((Gtk.Box.BoxChild)(this.hbox1[this.vbox3]));
            w8.Position = 1;
            // Container child hbox1.Gtk.Box+BoxChild
            this.button_login = new Gtk.Button();
            this.button_login.WidthRequest = 100;
            this.button_login.HeightRequest = 50;
            this.button_login.CanFocus = true;
            this.button_login.Name = "button_login";
            this.button_login.UseUnderline = true;
            // Container child button_login.Gtk.Container+ContainerChild
            Gtk.Alignment w9 = new Gtk.Alignment(0.5F, 0.5F, 0F, 0F);
            // Container child GtkAlignment.Gtk.Container+ContainerChild
            Gtk.HBox w10 = new Gtk.HBox();
            w10.Spacing = 2;
            // Container child GtkHBox.Gtk.Container+ContainerChild
            Gtk.Image w11 = new Gtk.Image();
            w11.Pixbuf = Stetic.IconLoader.LoadIcon(this, "gtk-connect", Gtk.IconSize.Menu, 16);
            w10.Add(w11);
            // Container child GtkHBox.Gtk.Container+ContainerChild
            Gtk.Label w13 = new Gtk.Label();
            w13.LabelProp = Mono.Unix.Catalog.GetString("Login");
            w13.UseUnderline = true;
            w10.Add(w13);
            w9.Add(w10);
            this.button_login.Add(w9);
            this.hbox1.Add(this.button_login);
            Gtk.Box.BoxChild w17 = ((Gtk.Box.BoxChild)(this.hbox1[this.button_login]));
            w17.Position = 2;
            w17.Expand = false;
            w17.Fill = false;
            this.vbox1.Add(this.hbox1);
            Gtk.Box.BoxChild w18 = ((Gtk.Box.BoxChild)(this.vbox1[this.hbox1]));
            w18.Position = 0;
            w18.Expand = false;
            w18.Fill = false;
            // Container child vbox1.Gtk.Box+BoxChild
            this.vbox5 = new Gtk.VBox();
            this.vbox5.Name = "vbox5";
            this.vbox5.Spacing = 6;
            // Container child vbox5.Gtk.Box+BoxChild
            this.hbox2 = new Gtk.HBox();
            this.hbox2.Name = "hbox2";
            this.hbox2.Spacing = 6;
            // Container child hbox2.Gtk.Box+BoxChild
            this.checkbutton_rememberpass = new Gtk.CheckButton();
            this.checkbutton_rememberpass.CanFocus = true;
            this.checkbutton_rememberpass.Name = "checkbutton_rememberpass";
            this.checkbutton_rememberpass.Label = Mono.Unix.Catalog.GetString("Remember password");
            this.checkbutton_rememberpass.DrawIndicator = true;
            this.checkbutton_rememberpass.UseUnderline = true;
            this.hbox2.Add(this.checkbutton_rememberpass);
            Gtk.Box.BoxChild w19 = ((Gtk.Box.BoxChild)(this.hbox2[this.checkbutton_rememberpass]));
            w19.Position = 0;
            this.vbox5.Add(this.hbox2);
            Gtk.Box.BoxChild w20 = ((Gtk.Box.BoxChild)(this.vbox5[this.hbox2]));
            w20.Position = 0;
            w20.Expand = false;
            w20.Fill = false;
            // Container child vbox5.Gtk.Box+BoxChild
            this.hbox5 = new Gtk.HBox();
            this.hbox5.Name = "hbox5";
            this.hbox5.Spacing = 6;
            // Container child hbox5.Gtk.Box+BoxChild
            this.label6 = new Gtk.Label();
            this.label6.Name = "label6";
            this.label6.LabelProp = Mono.Unix.Catalog.GetString("Login to");
            this.hbox5.Add(this.label6);
            Gtk.Box.BoxChild w21 = ((Gtk.Box.BoxChild)(this.hbox5[this.label6]));
            w21.Position = 0;
            w21.Expand = false;
            w21.Fill = false;
            // Container child hbox5.Gtk.Box+BoxChild
            this.hbox4 = new Gtk.HBox();
            this.hbox4.Name = "hbox4";
            this.hbox4.Spacing = 5;
            // Container child hbox4.Gtk.Box+BoxChild
            this.radiobutton1 = new Gtk.RadioButton(Mono.Unix.Catalog.GetString("Home"));
            this.radiobutton1.CanFocus = true;
            this.radiobutton1.Name = "radiobutton1";
            this.radiobutton1.DrawIndicator = true;
            this.radiobutton1.UseUnderline = true;
            this.radiobutton1.Group = new GLib.SList(System.IntPtr.Zero);
            this.hbox4.Add(this.radiobutton1);
            Gtk.Box.BoxChild w22 = ((Gtk.Box.BoxChild)(this.hbox4[this.radiobutton1]));
            w22.Position = 0;
            // Container child hbox4.Gtk.Box+BoxChild
            this.radiobutton2 = new Gtk.RadioButton(Mono.Unix.Catalog.GetString("Last"));
            this.radiobutton2.CanFocus = true;
            this.radiobutton2.Name = "radiobutton2";
            this.radiobutton2.DrawIndicator = true;
            this.radiobutton2.UseUnderline = true;
            this.radiobutton2.Group = this.radiobutton1.Group;
            this.hbox4.Add(this.radiobutton2);
            Gtk.Box.BoxChild w23 = ((Gtk.Box.BoxChild)(this.hbox4[this.radiobutton2]));
            w23.Position = 1;
            // Container child hbox4.Gtk.Box+BoxChild
            this.radiobutton3 = new Gtk.RadioButton(Mono.Unix.Catalog.GetString("Location"));
            this.radiobutton3.CanFocus = true;
            this.radiobutton3.Name = "radiobutton3";
            this.radiobutton3.DrawIndicator = true;
            this.radiobutton3.UseUnderline = true;
            this.radiobutton3.Group = this.radiobutton1.Group;
            this.hbox4.Add(this.radiobutton3);
            Gtk.Box.BoxChild w24 = ((Gtk.Box.BoxChild)(this.hbox4[this.radiobutton3]));
            w24.Position = 2;
            // Container child hbox4.Gtk.Box+BoxChild
            this.entry_location = new Gtk.Entry();
            this.entry_location.CanFocus = true;
            this.entry_location.Name = "entry_location";
            this.entry_location.IsEditable = true;
            this.entry_location.InvisibleChar = '●';
            this.hbox4.Add(this.entry_location);
            Gtk.Box.BoxChild w25 = ((Gtk.Box.BoxChild)(this.hbox4[this.entry_location]));
            w25.Position = 3;
            this.hbox5.Add(this.hbox4);
            Gtk.Box.BoxChild w26 = ((Gtk.Box.BoxChild)(this.hbox5[this.hbox4]));
            w26.Position = 1;
            w26.Expand = false;
            w26.Fill = false;
            // Container child hbox5.Gtk.Box+BoxChild
            this.progressbar2 = new Gtk.ProgressBar();
            this.progressbar2.Name = "progressbar2";
            this.hbox5.Add(this.progressbar2);
            Gtk.Box.BoxChild w27 = ((Gtk.Box.BoxChild)(this.hbox5[this.progressbar2]));
            w27.PackType = ((Gtk.PackType)(1));
            w27.Position = 2;
            this.vbox5.Add(this.hbox5);
            Gtk.Box.BoxChild w28 = ((Gtk.Box.BoxChild)(this.vbox5[this.hbox5]));
            w28.Position = 1;
            w28.Expand = false;
            w28.Fill = false;
            // Container child vbox5.Gtk.Box+BoxChild
            this.hbox6 = new Gtk.HBox();
            this.hbox6.Name = "hbox6";
            this.hbox6.Spacing = 6;
            // Container child hbox6.Gtk.Box+BoxChild
            this.label4 = new Gtk.Label();
            this.label4.Name = "label4";
            this.label4.LabelProp = Mono.Unix.Catalog.GetString("Connect to grid ");
            this.hbox6.Add(this.label4);
            Gtk.Box.BoxChild w29 = ((Gtk.Box.BoxChild)(this.hbox6[this.label4]));
            w29.Position = 0;
            w29.Expand = false;
            w29.Fill = false;
            // Container child hbox6.Gtk.Box+BoxChild
            this.combobox_grid = Gtk.ComboBox.NewText();
            this.combobox_grid.WidthRequest = 140;
            this.combobox_grid.Name = "combobox_grid";
            this.hbox6.Add(this.combobox_grid);
            Gtk.Box.BoxChild w30 = ((Gtk.Box.BoxChild)(this.hbox6[this.combobox_grid]));
            w30.Position = 1;
            w30.Expand = false;
            w30.Fill = false;
            // Container child hbox6.Gtk.Box+BoxChild
            this.label5 = new Gtk.Label();
            this.label5.Name = "label5";
            this.label5.LabelProp = Mono.Unix.Catalog.GetString("URI");
            this.hbox6.Add(this.label5);
            Gtk.Box.BoxChild w31 = ((Gtk.Box.BoxChild)(this.hbox6[this.label5]));
            w31.Position = 2;
            w31.Expand = false;
            w31.Fill = false;
            // Container child hbox6.Gtk.Box+BoxChild
            this.entry_loginuri = new Gtk.Entry();
            this.entry_loginuri.CanFocus = true;
            this.entry_loginuri.Name = "entry_loginuri";
            this.entry_loginuri.IsEditable = true;
            this.entry_loginuri.InvisibleChar = '●';
            this.hbox6.Add(this.entry_loginuri);
            Gtk.Box.BoxChild w32 = ((Gtk.Box.BoxChild)(this.hbox6[this.entry_loginuri]));
            w32.Position = 3;
            this.vbox5.Add(this.hbox6);
            Gtk.Box.BoxChild w33 = ((Gtk.Box.BoxChild)(this.vbox5[this.hbox6]));
            w33.Position = 3;
            w33.Expand = false;
            w33.Fill = false;
            // Container child vbox5.Gtk.Box+BoxChild
            this.frame1 = new Gtk.Frame();
            this.frame1.Name = "frame1";
            this.frame1.ShadowType = ((Gtk.ShadowType)(0));
            // Container child frame1.Gtk.Container+ContainerChild
            this.GtkAlignment1 = new Gtk.Alignment(0F, 0F, 1F, 1F);
            this.GtkAlignment1.Name = "GtkAlignment1";
            this.GtkAlignment1.LeftPadding = ((uint)(12));
            // Container child GtkAlignment1.Gtk.Container+ContainerChild
            this.textview_loginmsg = new Gtk.TextView();
            this.textview_loginmsg.HeightRequest = 35;
            this.textview_loginmsg.CanFocus = true;
            this.textview_loginmsg.Name = "textview_loginmsg";
            this.textview_loginmsg.Editable = false;
            this.textview_loginmsg.WrapMode = ((Gtk.WrapMode)(2));
            this.GtkAlignment1.Add(this.textview_loginmsg);
            this.frame1.Add(this.GtkAlignment1);
            this.GtkLabel5 = new Gtk.Label();
            this.GtkLabel5.Name = "GtkLabel5";
            this.GtkLabel5.LabelProp = Mono.Unix.Catalog.GetString("<b>Login message</b>");
            this.GtkLabel5.UseMarkup = true;
            this.frame1.LabelWidget = this.GtkLabel5;
            this.vbox5.Add(this.frame1);
            Gtk.Box.BoxChild w36 = ((Gtk.Box.BoxChild)(this.vbox5[this.frame1]));
            w36.Position = 4;
            // Container child vbox5.Gtk.Box+BoxChild
            this.frame2 = new Gtk.Frame();
            this.frame2.Name = "frame2";
            this.frame2.ShadowType = ((Gtk.ShadowType)(0));
            // Container child frame2.Gtk.Container+ContainerChild
            this.GtkAlignment2 = new Gtk.Alignment(0F, 0F, 1F, 1F);
            this.GtkAlignment2.Name = "GtkAlignment2";
            this.GtkAlignment2.LeftPadding = ((uint)(12));
            // Container child GtkAlignment2.Gtk.Container+ContainerChild
            this.GtkScrolledWindow1 = new Gtk.ScrolledWindow();
            this.GtkScrolledWindow1.Name = "GtkScrolledWindow1";
            this.GtkScrolledWindow1.ShadowType = ((Gtk.ShadowType)(1));
            // Container child GtkScrolledWindow1.Gtk.Container+ContainerChild
            this.textview_log = new Gtk.TextView();
            this.textview_log.HeightRequest = 270;
            this.textview_log.CanFocus = true;
            this.textview_log.Name = "textview_log";
            this.textview_log.Editable = false;
            this.textview_log.WrapMode = ((Gtk.WrapMode)(2));
            this.GtkScrolledWindow1.Add(this.textview_log);
            this.GtkAlignment2.Add(this.GtkScrolledWindow1);
            this.frame2.Add(this.GtkAlignment2);
            this.GtkLabel6 = new Gtk.Label();
            this.GtkLabel6.Name = "GtkLabel6";
            this.GtkLabel6.LabelProp = Mono.Unix.Catalog.GetString("<b>Debug log</b>");
            this.GtkLabel6.UseMarkup = true;
            this.frame2.LabelWidget = this.GtkLabel6;
            this.vbox5.Add(this.frame2);
            Gtk.Box.BoxChild w40 = ((Gtk.Box.BoxChild)(this.vbox5[this.frame2]));
            w40.Position = 5;
            this.vbox1.Add(this.vbox5);
            Gtk.Box.BoxChild w41 = ((Gtk.Box.BoxChild)(this.vbox1[this.vbox5]));
            w41.Position = 1;
            this.Add(this.vbox1);
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.Show();
            this.button_login.Clicked += new System.EventHandler(this.OnButton1Clicked);
            this.checkbutton_rememberpass.Clicked += new System.EventHandler(this.OnCheckbuttonRememberpassClicked);
            this.combobox_grid.Changed += new System.EventHandler(this.OnComboboxGridChanged);
        }
    }
}
