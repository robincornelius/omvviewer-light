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
    
    
    public partial class ProfileVIew {
        
        private Gtk.Notebook notebook1;
        
        private Gtk.VBox vbox1;
        
        private Gtk.Label label_name;
        
        private Gtk.HBox hbox1;
        
        private Gtk.Image image7;
        
        private Gtk.HBox hbox3;
        
        private Gtk.VBox vbox3;
        
        private Gtk.Label label6;
        
        private Gtk.Label label8;
        
        private Gtk.Label label10;
        
        private Gtk.Label label12;
        
        private Gtk.Label label14;
        
        private Gtk.VBox vbox4;
        
        private Gtk.Label label_status;
        
        private Gtk.Label label_born;
        
        private Gtk.Label label_pay;
        
        private Gtk.Label label_partner;
        
        private Gtk.Label label_identified;
        
        private Gtk.Frame frame2;
        
        private Gtk.Alignment GtkAlignment1;
        
        private Gtk.ScrolledWindow GtkScrolledWindow1;
        
        private Gtk.TextView textview2;
        
        private Gtk.Label GtkLabel1;
        
        private Gtk.HBox hbox4;
        
        private Gtk.Button button_IM;
        
        private Gtk.Button button_pay;
        
        private Gtk.Button button_teleport;
        
        private Gtk.Button button_addfriend;
        
        private Gtk.Label label2;
        
        private Gtk.VBox vbox2;
        
        private Gtk.Image image3;
        
        private Gtk.ScrolledWindow GtkScrolledWindow2;
        
        private Gtk.TextView textview3;
        
        private Gtk.Label label3;
        
        private Gtk.VBox vbox5;
        
        private Gtk.Notebook notebook_picks;
        
        private Gtk.HBox hbox5;
        
        private Gtk.Label label4;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget omvviewerlight.ProfileVIew
            this.WidthRequest = 410;
            this.HeightRequest = 450;
            this.Name = "omvviewerlight.ProfileVIew";
            this.Title = Mono.Unix.Catalog.GetString("ProfileVIew");
            this.WindowPosition = ((Gtk.WindowPosition)(4));
            // Container child omvviewerlight.ProfileVIew.Gtk.Container+ContainerChild
            this.notebook1 = new Gtk.Notebook();
            this.notebook1.WidthRequest = 160;
            this.notebook1.CanFocus = true;
            this.notebook1.Name = "notebook1";
            this.notebook1.CurrentPage = 0;
            // Container child notebook1.Gtk.Notebook+NotebookChild
            this.vbox1 = new Gtk.VBox();
            this.vbox1.Name = "vbox1";
            this.vbox1.Spacing = 6;
            // Container child vbox1.Gtk.Box+BoxChild
            this.label_name = new Gtk.Label();
            this.label_name.Name = "label_name";
            this.label_name.LabelProp = Mono.Unix.Catalog.GetString("Name Here");
            this.label_name.UseMarkup = true;
            this.vbox1.Add(this.label_name);
            Gtk.Box.BoxChild w1 = ((Gtk.Box.BoxChild)(this.vbox1[this.label_name]));
            w1.Position = 0;
            w1.Expand = false;
            w1.Fill = false;
            // Container child vbox1.Gtk.Box+BoxChild
            this.hbox1 = new Gtk.HBox();
            this.hbox1.Name = "hbox1";
            this.hbox1.Spacing = 6;
            // Container child hbox1.Gtk.Box+BoxChild
            this.image7 = new Gtk.Image();
            this.image7.WidthRequest = 128;
            this.image7.HeightRequest = 128;
            this.image7.Name = "image7";
            this.hbox1.Add(this.image7);
            Gtk.Box.BoxChild w2 = ((Gtk.Box.BoxChild)(this.hbox1[this.image7]));
            w2.Position = 0;
            w2.Expand = false;
            w2.Fill = false;
            // Container child hbox1.Gtk.Box+BoxChild
            this.hbox3 = new Gtk.HBox();
            this.hbox3.Name = "hbox3";
            this.hbox3.Spacing = 6;
            // Container child hbox3.Gtk.Box+BoxChild
            this.vbox3 = new Gtk.VBox();
            this.vbox3.Name = "vbox3";
            this.vbox3.Spacing = 6;
            // Container child vbox3.Gtk.Box+BoxChild
            this.label6 = new Gtk.Label();
            this.label6.Name = "label6";
            this.label6.Xalign = 0F;
            this.label6.LabelProp = Mono.Unix.Catalog.GetString("<b>Status</b>");
            this.label6.UseMarkup = true;
            this.vbox3.Add(this.label6);
            Gtk.Box.BoxChild w3 = ((Gtk.Box.BoxChild)(this.vbox3[this.label6]));
            w3.Position = 0;
            w3.Expand = false;
            w3.Fill = false;
            // Container child vbox3.Gtk.Box+BoxChild
            this.label8 = new Gtk.Label();
            this.label8.Name = "label8";
            this.label8.Xalign = 0F;
            this.label8.LabelProp = Mono.Unix.Catalog.GetString("<b>Born</b>");
            this.label8.UseMarkup = true;
            this.vbox3.Add(this.label8);
            Gtk.Box.BoxChild w4 = ((Gtk.Box.BoxChild)(this.vbox3[this.label8]));
            w4.Position = 1;
            w4.Expand = false;
            w4.Fill = false;
            // Container child vbox3.Gtk.Box+BoxChild
            this.label10 = new Gtk.Label();
            this.label10.Name = "label10";
            this.label10.Xalign = 0F;
            this.label10.LabelProp = Mono.Unix.Catalog.GetString("<b>Pay Info</b>");
            this.label10.UseMarkup = true;
            this.vbox3.Add(this.label10);
            Gtk.Box.BoxChild w5 = ((Gtk.Box.BoxChild)(this.vbox3[this.label10]));
            w5.Position = 2;
            w5.Expand = false;
            w5.Fill = false;
            // Container child vbox3.Gtk.Box+BoxChild
            this.label12 = new Gtk.Label();
            this.label12.Name = "label12";
            this.label12.Xalign = 0F;
            this.label12.LabelProp = Mono.Unix.Catalog.GetString("<b>Partner</b>");
            this.label12.UseMarkup = true;
            this.vbox3.Add(this.label12);
            Gtk.Box.BoxChild w6 = ((Gtk.Box.BoxChild)(this.vbox3[this.label12]));
            w6.Position = 3;
            w6.Expand = false;
            w6.Fill = false;
            // Container child vbox3.Gtk.Box+BoxChild
            this.label14 = new Gtk.Label();
            this.label14.Name = "label14";
            this.label14.Xalign = 0F;
            this.label14.LabelProp = Mono.Unix.Catalog.GetString("<b>Identified</b>");
            this.label14.UseMarkup = true;
            this.vbox3.Add(this.label14);
            Gtk.Box.BoxChild w7 = ((Gtk.Box.BoxChild)(this.vbox3[this.label14]));
            w7.Position = 4;
            w7.Expand = false;
            w7.Fill = false;
            this.hbox3.Add(this.vbox3);
            Gtk.Box.BoxChild w8 = ((Gtk.Box.BoxChild)(this.hbox3[this.vbox3]));
            w8.Position = 0;
            w8.Expand = false;
            w8.Fill = false;
            // Container child hbox3.Gtk.Box+BoxChild
            this.vbox4 = new Gtk.VBox();
            this.vbox4.Name = "vbox4";
            this.vbox4.Spacing = 6;
            // Container child vbox4.Gtk.Box+BoxChild
            this.label_status = new Gtk.Label();
            this.label_status.Name = "label_status";
            this.label_status.Xalign = 0F;
            this.label_status.LabelProp = Mono.Unix.Catalog.GetString("label7");
            this.vbox4.Add(this.label_status);
            Gtk.Box.BoxChild w9 = ((Gtk.Box.BoxChild)(this.vbox4[this.label_status]));
            w9.Position = 0;
            w9.Expand = false;
            w9.Fill = false;
            // Container child vbox4.Gtk.Box+BoxChild
            this.label_born = new Gtk.Label();
            this.label_born.Name = "label_born";
            this.label_born.Xalign = 0F;
            this.label_born.LabelProp = Mono.Unix.Catalog.GetString("label9");
            this.vbox4.Add(this.label_born);
            Gtk.Box.BoxChild w10 = ((Gtk.Box.BoxChild)(this.vbox4[this.label_born]));
            w10.Position = 1;
            w10.Expand = false;
            w10.Fill = false;
            // Container child vbox4.Gtk.Box+BoxChild
            this.label_pay = new Gtk.Label();
            this.label_pay.Name = "label_pay";
            this.label_pay.Xalign = 0F;
            this.label_pay.LabelProp = Mono.Unix.Catalog.GetString("label11");
            this.vbox4.Add(this.label_pay);
            Gtk.Box.BoxChild w11 = ((Gtk.Box.BoxChild)(this.vbox4[this.label_pay]));
            w11.Position = 2;
            w11.Expand = false;
            w11.Fill = false;
            // Container child vbox4.Gtk.Box+BoxChild
            this.label_partner = new Gtk.Label();
            this.label_partner.Name = "label_partner";
            this.label_partner.Xalign = 0F;
            this.label_partner.LabelProp = Mono.Unix.Catalog.GetString("label13");
            this.vbox4.Add(this.label_partner);
            Gtk.Box.BoxChild w12 = ((Gtk.Box.BoxChild)(this.vbox4[this.label_partner]));
            w12.Position = 3;
            w12.Expand = false;
            w12.Fill = false;
            // Container child vbox4.Gtk.Box+BoxChild
            this.label_identified = new Gtk.Label();
            this.label_identified.Name = "label_identified";
            this.label_identified.Xalign = 0F;
            this.label_identified.LabelProp = Mono.Unix.Catalog.GetString("label15");
            this.vbox4.Add(this.label_identified);
            Gtk.Box.BoxChild w13 = ((Gtk.Box.BoxChild)(this.vbox4[this.label_identified]));
            w13.Position = 4;
            w13.Expand = false;
            w13.Fill = false;
            this.hbox3.Add(this.vbox4);
            Gtk.Box.BoxChild w14 = ((Gtk.Box.BoxChild)(this.hbox3[this.vbox4]));
            w14.Position = 1;
            w14.Expand = false;
            w14.Fill = false;
            this.hbox1.Add(this.hbox3);
            Gtk.Box.BoxChild w15 = ((Gtk.Box.BoxChild)(this.hbox1[this.hbox3]));
            w15.Position = 1;
            w15.Expand = false;
            w15.Fill = false;
            this.vbox1.Add(this.hbox1);
            Gtk.Box.BoxChild w16 = ((Gtk.Box.BoxChild)(this.vbox1[this.hbox1]));
            w16.Position = 1;
            w16.Expand = false;
            w16.Fill = false;
            // Container child vbox1.Gtk.Box+BoxChild
            this.frame2 = new Gtk.Frame();
            this.frame2.Name = "frame2";
            this.frame2.ShadowType = ((Gtk.ShadowType)(0));
            // Container child frame2.Gtk.Container+ContainerChild
            this.GtkAlignment1 = new Gtk.Alignment(0F, 0F, 1F, 1F);
            this.GtkAlignment1.Name = "GtkAlignment1";
            this.GtkAlignment1.LeftPadding = ((uint)(12));
            // Container child GtkAlignment1.Gtk.Container+ContainerChild
            this.GtkScrolledWindow1 = new Gtk.ScrolledWindow();
            this.GtkScrolledWindow1.Name = "GtkScrolledWindow1";
            this.GtkScrolledWindow1.ShadowType = ((Gtk.ShadowType)(1));
            // Container child GtkScrolledWindow1.Gtk.Container+ContainerChild
            this.textview2 = new Gtk.TextView();
            this.textview2.CanFocus = true;
            this.textview2.Name = "textview2";
            this.textview2.Editable = false;
            this.textview2.WrapMode = ((Gtk.WrapMode)(2));
            this.GtkScrolledWindow1.Add(this.textview2);
            this.GtkAlignment1.Add(this.GtkScrolledWindow1);
            this.frame2.Add(this.GtkAlignment1);
            this.GtkLabel1 = new Gtk.Label();
            this.GtkLabel1.Name = "GtkLabel1";
            this.GtkLabel1.LabelProp = Mono.Unix.Catalog.GetString("<b>About</b>");
            this.GtkLabel1.UseMarkup = true;
            this.frame2.LabelWidget = this.GtkLabel1;
            this.vbox1.Add(this.frame2);
            Gtk.Box.BoxChild w20 = ((Gtk.Box.BoxChild)(this.vbox1[this.frame2]));
            w20.Position = 2;
            // Container child vbox1.Gtk.Box+BoxChild
            this.hbox4 = new Gtk.HBox();
            this.hbox4.Name = "hbox4";
            this.hbox4.Spacing = 6;
            // Container child hbox4.Gtk.Box+BoxChild
            this.button_IM = new Gtk.Button();
            this.button_IM.CanFocus = true;
            this.button_IM.Name = "button_IM";
            this.button_IM.UseUnderline = true;
            // Container child button_IM.Gtk.Container+ContainerChild
            Gtk.Alignment w21 = new Gtk.Alignment(0.5F, 0.5F, 0F, 0F);
            // Container child GtkAlignment.Gtk.Container+ContainerChild
            Gtk.HBox w22 = new Gtk.HBox();
            w22.Spacing = 2;
            // Container child GtkHBox.Gtk.Container+ContainerChild
            Gtk.Image w23 = new Gtk.Image();
            w23.Pixbuf = MainClass.GetResource("icn_voice-groupfocus.tga");
            w22.Add(w23);
            // Container child GtkHBox.Gtk.Container+ContainerChild
            Gtk.Label w25 = new Gtk.Label();
            w25.LabelProp = Mono.Unix.Catalog.GetString("IM");
            w25.UseUnderline = true;
            w22.Add(w25);
            w21.Add(w22);
            this.button_IM.Add(w21);
            this.hbox4.Add(this.button_IM);
            Gtk.Box.BoxChild w29 = ((Gtk.Box.BoxChild)(this.hbox4[this.button_IM]));
            w29.Position = 0;
            w29.Expand = false;
            w29.Fill = false;
            // Container child hbox4.Gtk.Box+BoxChild
            this.button_pay = new Gtk.Button();
            this.button_pay.CanFocus = true;
            this.button_pay.Name = "button_pay";
            this.button_pay.UseUnderline = true;
            // Container child button_pay.Gtk.Container+ContainerChild
            Gtk.Alignment w30 = new Gtk.Alignment(0.5F, 0.5F, 0F, 0F);
            // Container child GtkAlignment2.Gtk.Container+ContainerChild
            Gtk.HBox w31 = new Gtk.HBox();
            w31.Spacing = 2;
            // Container child GtkHBox1.Gtk.Container+ContainerChild
            Gtk.Image w32 = new Gtk.Image();
            w32.Pixbuf = MainClass.GetResource("status_money.tga");
            w31.Add(w32);
            // Container child GtkHBox1.Gtk.Container+ContainerChild
            Gtk.Label w34 = new Gtk.Label();
            w34.LabelProp = Mono.Unix.Catalog.GetString("Pay");
            w34.UseUnderline = true;
            w31.Add(w34);
            w30.Add(w31);
            this.button_pay.Add(w30);
            this.hbox4.Add(this.button_pay);
            Gtk.Box.BoxChild w38 = ((Gtk.Box.BoxChild)(this.hbox4[this.button_pay]));
            w38.Position = 1;
            w38.Expand = false;
            w38.Fill = false;
            // Container child hbox4.Gtk.Box+BoxChild
            this.button_teleport = new Gtk.Button();
            this.button_teleport.CanFocus = true;
            this.button_teleport.Name = "button_teleport";
            this.button_teleport.UseUnderline = true;
            // Container child button_teleport.Gtk.Container+ContainerChild
            Gtk.Alignment w39 = new Gtk.Alignment(0.5F, 0.5F, 0F, 0F);
            // Container child GtkAlignment3.Gtk.Container+ContainerChild
            Gtk.HBox w40 = new Gtk.HBox();
            w40.Spacing = 2;
            // Container child GtkHBox2.Gtk.Container+ContainerChild
            Gtk.Image w41 = new Gtk.Image();
            w41.Pixbuf = MainClass.GetResource("icon_place.tga");
            w40.Add(w41);
            // Container child GtkHBox2.Gtk.Container+ContainerChild
            Gtk.Label w43 = new Gtk.Label();
            w43.LabelProp = Mono.Unix.Catalog.GetString("Teleport");
            w43.UseUnderline = true;
            w40.Add(w43);
            w39.Add(w40);
            this.button_teleport.Add(w39);
            this.hbox4.Add(this.button_teleport);
            Gtk.Box.BoxChild w47 = ((Gtk.Box.BoxChild)(this.hbox4[this.button_teleport]));
            w47.Position = 2;
            w47.Expand = false;
            w47.Fill = false;
            // Container child hbox4.Gtk.Box+BoxChild
            this.button_addfriend = new Gtk.Button();
            this.button_addfriend.CanFocus = true;
            this.button_addfriend.Name = "button_addfriend";
            this.button_addfriend.UseUnderline = true;
            this.button_addfriend.Label = Mono.Unix.Catalog.GetString("Add friend");
            this.hbox4.Add(this.button_addfriend);
            Gtk.Box.BoxChild w48 = ((Gtk.Box.BoxChild)(this.hbox4[this.button_addfriend]));
            w48.Position = 3;
            w48.Expand = false;
            w48.Fill = false;
            this.vbox1.Add(this.hbox4);
            Gtk.Box.BoxChild w49 = ((Gtk.Box.BoxChild)(this.vbox1[this.hbox4]));
            w49.Position = 3;
            w49.Expand = false;
            w49.Fill = false;
            this.notebook1.Add(this.vbox1);
            // Notebook tab
            this.label2 = new Gtk.Label();
            this.label2.Name = "label2";
            this.label2.LabelProp = Mono.Unix.Catalog.GetString("2nd Life");
            this.notebook1.SetTabLabel(this.vbox1, this.label2);
            this.label2.ShowAll();
            // Container child notebook1.Gtk.Notebook+NotebookChild
            this.vbox2 = new Gtk.VBox();
            this.vbox2.Name = "vbox2";
            this.vbox2.Spacing = 6;
            // Container child vbox2.Gtk.Box+BoxChild
            this.image3 = new Gtk.Image();
            this.image3.WidthRequest = 128;
            this.image3.HeightRequest = 128;
            this.image3.Name = "image3";
            this.vbox2.Add(this.image3);
            Gtk.Box.BoxChild w51 = ((Gtk.Box.BoxChild)(this.vbox2[this.image3]));
            w51.Position = 0;
            w51.Expand = false;
            w51.Fill = false;
            // Container child vbox2.Gtk.Box+BoxChild
            this.GtkScrolledWindow2 = new Gtk.ScrolledWindow();
            this.GtkScrolledWindow2.Name = "GtkScrolledWindow2";
            this.GtkScrolledWindow2.ShadowType = ((Gtk.ShadowType)(1));
            // Container child GtkScrolledWindow2.Gtk.Container+ContainerChild
            this.textview3 = new Gtk.TextView();
            this.textview3.CanFocus = true;
            this.textview3.Name = "textview3";
            this.textview3.Editable = false;
            this.textview3.WrapMode = ((Gtk.WrapMode)(2));
            this.GtkScrolledWindow2.Add(this.textview3);
            this.vbox2.Add(this.GtkScrolledWindow2);
            Gtk.Box.BoxChild w53 = ((Gtk.Box.BoxChild)(this.vbox2[this.GtkScrolledWindow2]));
            w53.Position = 1;
            this.notebook1.Add(this.vbox2);
            Gtk.Notebook.NotebookChild w54 = ((Gtk.Notebook.NotebookChild)(this.notebook1[this.vbox2]));
            w54.Position = 1;
            // Notebook tab
            this.label3 = new Gtk.Label();
            this.label3.Name = "label3";
            this.label3.LabelProp = Mono.Unix.Catalog.GetString("1st Life");
            this.notebook1.SetTabLabel(this.vbox2, this.label3);
            this.label3.ShowAll();
            // Container child notebook1.Gtk.Notebook+NotebookChild
            this.vbox5 = new Gtk.VBox();
            this.vbox5.Name = "vbox5";
            this.vbox5.Spacing = 6;
            // Container child vbox5.Gtk.Box+BoxChild
            this.notebook_picks = new Gtk.Notebook();
            this.notebook_picks.CanFocus = true;
            this.notebook_picks.Name = "notebook_picks";
            this.notebook_picks.CurrentPage = -1;
            this.notebook_picks.TabPos = ((Gtk.PositionType)(0));
            this.vbox5.Add(this.notebook_picks);
            Gtk.Box.BoxChild w55 = ((Gtk.Box.BoxChild)(this.vbox5[this.notebook_picks]));
            w55.Position = 0;
            w55.Expand = false;
            w55.Fill = false;
            // Container child vbox5.Gtk.Box+BoxChild
            this.hbox5 = new Gtk.HBox();
            this.hbox5.Name = "hbox5";
            this.hbox5.Spacing = 6;
            this.vbox5.Add(this.hbox5);
            Gtk.Box.BoxChild w56 = ((Gtk.Box.BoxChild)(this.vbox5[this.hbox5]));
            w56.Position = 1;
            this.notebook1.Add(this.vbox5);
            Gtk.Notebook.NotebookChild w57 = ((Gtk.Notebook.NotebookChild)(this.notebook1[this.vbox5]));
            w57.Position = 2;
            // Notebook tab
            this.label4 = new Gtk.Label();
            this.label4.Name = "label4";
            this.label4.LabelProp = Mono.Unix.Catalog.GetString("Picks");
            this.notebook1.SetTabLabel(this.vbox5, this.label4);
            this.label4.ShowAll();
            this.Add(this.notebook1);
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.DefaultWidth = 427;
            this.DefaultHeight = 491;
            this.Show();
            this.button_addfriend.Clicked += new System.EventHandler(this.OnButtonAddfriendClicked);
        }
    }
}
