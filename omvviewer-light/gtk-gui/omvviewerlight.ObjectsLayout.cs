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
    
    
    public partial class ObjectsLayout {
        
        private Gtk.HBox hbox1;
        
        private Gtk.VBox vbox2;
        
        private Gtk.ScrolledWindow GtkScrolledWindow;
        
        private Gtk.TreeView treeview1;
        
        private Gtk.VBox vbox1;
        
        private Gtk.HBox hbox2;
        
        private Gtk.Label label1;
        
        private Gtk.Entry entry1;
        
        private Gtk.Button button_search;
        
        private Gtk.HBox hbox3;
        
        private Gtk.Button button_touch;
        
        private Gtk.Button button_siton;
        
        private Gtk.Button button_lookat;
        
        private Gtk.Button button_pay;
        
        private Gtk.HBox hbox4;
        
        private Gtk.Button button_buy;
        
        private Gtk.Button button_take;
        
        private Gtk.Button button_take_copy;
        
        private Gtk.Button button_return;
        
        private Gtk.HBox hbox5;
        
        private Gtk.Button button_moveto;
        
        private Gtk.HBox hbox6;
        
        private Gtk.VBox vbox3;
        
        private Gtk.Label label2;
        
        private Gtk.Label label3;
        
        private Gtk.Label label4;
        
        private Gtk.Label label6;
        
        private Gtk.Label label7;
        
        private Gtk.Label label9;
        
        private Gtk.Label label5;
        
        private Gtk.Label label10;
        
        private Gtk.Label label8;
        
        private Gtk.VBox vbox4;
        
        private Gtk.Label label_name;
        
        private Gtk.Label label_key;
        
        private Gtk.Label label_desc;
        
        private Gtk.Label label_owner;
        
        private Gtk.Label label_group;
        
        private Gtk.Label label_forsale;
        
        private Gtk.Label label_pos;
        
        private Gtk.Label label_distance;
        
        private Gtk.Label label_float_text;
        
        private Gtk.ScrolledWindow GtkScrolledWindow1;
        
        private Gtk.TextView textview1;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget omvviewerlight.ObjectsLayout
            Stetic.BinContainer.Attach(this);
            this.Name = "omvviewerlight.ObjectsLayout";
            // Container child omvviewerlight.ObjectsLayout.Gtk.Container+ContainerChild
            this.hbox1 = new Gtk.HBox();
            this.hbox1.Name = "hbox1";
            this.hbox1.Spacing = 6;
            // Container child hbox1.Gtk.Box+BoxChild
            this.vbox2 = new Gtk.VBox();
            this.vbox2.Name = "vbox2";
            this.vbox2.Spacing = 6;
            // Container child vbox2.Gtk.Box+BoxChild
            this.GtkScrolledWindow = new Gtk.ScrolledWindow();
            this.GtkScrolledWindow.WidthRequest = 350;
            this.GtkScrolledWindow.Name = "GtkScrolledWindow";
            this.GtkScrolledWindow.ShadowType = ((Gtk.ShadowType)(1));
            // Container child GtkScrolledWindow.Gtk.Container+ContainerChild
            this.treeview1 = new Gtk.TreeView();
            this.treeview1.CanFocus = true;
            this.treeview1.Name = "treeview1";
            this.treeview1.SearchColumn = 2;
            this.treeview1.HeadersClickable = true;
            this.GtkScrolledWindow.Add(this.treeview1);
            this.vbox2.Add(this.GtkScrolledWindow);
            Gtk.Box.BoxChild w2 = ((Gtk.Box.BoxChild)(this.vbox2[this.GtkScrolledWindow]));
            w2.Position = 0;
            this.hbox1.Add(this.vbox2);
            Gtk.Box.BoxChild w3 = ((Gtk.Box.BoxChild)(this.hbox1[this.vbox2]));
            w3.Position = 0;
            // Container child hbox1.Gtk.Box+BoxChild
            this.vbox1 = new Gtk.VBox();
            this.vbox1.Name = "vbox1";
            this.vbox1.Spacing = 6;
            // Container child vbox1.Gtk.Box+BoxChild
            this.hbox2 = new Gtk.HBox();
            this.hbox2.Name = "hbox2";
            this.hbox2.Spacing = 6;
            // Container child hbox2.Gtk.Box+BoxChild
            this.label1 = new Gtk.Label();
            this.label1.Name = "label1";
            this.label1.LabelProp = Mono.Unix.Catalog.GetString("Search radius (m)");
            this.hbox2.Add(this.label1);
            Gtk.Box.BoxChild w4 = ((Gtk.Box.BoxChild)(this.hbox2[this.label1]));
            w4.Position = 0;
            w4.Expand = false;
            w4.Fill = false;
            // Container child hbox2.Gtk.Box+BoxChild
            this.entry1 = new Gtk.Entry();
            this.entry1.CanFocus = true;
            this.entry1.Name = "entry1";
            this.entry1.Text = Mono.Unix.Catalog.GetString("25");
            this.entry1.IsEditable = true;
            this.entry1.MaxLength = 3;
            this.entry1.InvisibleChar = '●';
            this.hbox2.Add(this.entry1);
            Gtk.Box.BoxChild w5 = ((Gtk.Box.BoxChild)(this.hbox2[this.entry1]));
            w5.Position = 1;
            // Container child hbox2.Gtk.Box+BoxChild
            this.button_search = new Gtk.Button();
            this.button_search.CanFocus = true;
            this.button_search.Name = "button_search";
            this.button_search.UseUnderline = true;
            // Container child button_search.Gtk.Container+ContainerChild
            Gtk.Alignment w6 = new Gtk.Alignment(0.5F, 0.5F, 0F, 0F);
            // Container child GtkAlignment.Gtk.Container+ContainerChild
            Gtk.HBox w7 = new Gtk.HBox();
            w7.Spacing = 2;
            // Container child GtkHBox.Gtk.Container+ContainerChild
            Gtk.Image w8 = new Gtk.Image();
            w8.Pixbuf = Gdk.Pixbuf.LoadFromResource("omvviewerlight.art.status_search_btn.png");
            w7.Add(w8);
            // Container child GtkHBox.Gtk.Container+ContainerChild
            Gtk.Label w10 = new Gtk.Label();
            w10.LabelProp = Mono.Unix.Catalog.GetString("Search");
            w10.UseUnderline = true;
            w7.Add(w10);
            w6.Add(w7);
            this.button_search.Add(w6);
            this.hbox2.Add(this.button_search);
            Gtk.Box.BoxChild w14 = ((Gtk.Box.BoxChild)(this.hbox2[this.button_search]));
            w14.Position = 2;
            w14.Expand = false;
            w14.Fill = false;
            this.vbox1.Add(this.hbox2);
            Gtk.Box.BoxChild w15 = ((Gtk.Box.BoxChild)(this.vbox1[this.hbox2]));
            w15.Position = 0;
            w15.Expand = false;
            w15.Fill = false;
            // Container child vbox1.Gtk.Box+BoxChild
            this.hbox3 = new Gtk.HBox();
            this.hbox3.Name = "hbox3";
            this.hbox3.Homogeneous = true;
            this.hbox3.Spacing = 6;
            // Container child hbox3.Gtk.Box+BoxChild
            this.button_touch = new Gtk.Button();
            this.button_touch.WidthRequest = 75;
            this.button_touch.Sensitive = false;
            this.button_touch.CanFocus = true;
            this.button_touch.Name = "button_touch";
            this.button_touch.UseUnderline = true;
            this.button_touch.Label = Mono.Unix.Catalog.GetString("Touch");
            this.hbox3.Add(this.button_touch);
            Gtk.Box.BoxChild w16 = ((Gtk.Box.BoxChild)(this.hbox3[this.button_touch]));
            w16.Position = 0;
            w16.Expand = false;
            w16.Fill = false;
            // Container child hbox3.Gtk.Box+BoxChild
            this.button_siton = new Gtk.Button();
            this.button_siton.WidthRequest = 75;
            this.button_siton.Sensitive = false;
            this.button_siton.CanFocus = true;
            this.button_siton.Name = "button_siton";
            this.button_siton.UseUnderline = true;
            this.button_siton.Label = Mono.Unix.Catalog.GetString("Sit on");
            this.hbox3.Add(this.button_siton);
            Gtk.Box.BoxChild w17 = ((Gtk.Box.BoxChild)(this.hbox3[this.button_siton]));
            w17.Position = 1;
            w17.Expand = false;
            w17.Fill = false;
            // Container child hbox3.Gtk.Box+BoxChild
            this.button_lookat = new Gtk.Button();
            this.button_lookat.WidthRequest = 75;
            this.button_lookat.Sensitive = false;
            this.button_lookat.CanFocus = true;
            this.button_lookat.Name = "button_lookat";
            this.button_lookat.UseUnderline = true;
            this.button_lookat.Label = Mono.Unix.Catalog.GetString("Look at");
            this.hbox3.Add(this.button_lookat);
            Gtk.Box.BoxChild w18 = ((Gtk.Box.BoxChild)(this.hbox3[this.button_lookat]));
            w18.Position = 2;
            w18.Expand = false;
            w18.Fill = false;
            // Container child hbox3.Gtk.Box+BoxChild
            this.button_pay = new Gtk.Button();
            this.button_pay.WidthRequest = 75;
            this.button_pay.Sensitive = false;
            this.button_pay.CanFocus = true;
            this.button_pay.Name = "button_pay";
            this.button_pay.UseUnderline = true;
            // Container child button_pay.Gtk.Container+ContainerChild
            Gtk.Alignment w19 = new Gtk.Alignment(0.5F, 0.5F, 0F, 0F);
            // Container child GtkAlignment.Gtk.Container+ContainerChild
            Gtk.HBox w20 = new Gtk.HBox();
            w20.Spacing = 2;
            // Container child GtkHBox.Gtk.Container+ContainerChild
            Gtk.Image w21 = new Gtk.Image();
            w21.Pixbuf = Gdk.Pixbuf.LoadFromResource("omvviewerlight.art.status_money.tga");
            w20.Add(w21);
            // Container child GtkHBox.Gtk.Container+ContainerChild
            Gtk.Label w23 = new Gtk.Label();
            w23.LabelProp = Mono.Unix.Catalog.GetString("Pay");
            w23.UseUnderline = true;
            w20.Add(w23);
            w19.Add(w20);
            this.button_pay.Add(w19);
            this.hbox3.Add(this.button_pay);
            Gtk.Box.BoxChild w27 = ((Gtk.Box.BoxChild)(this.hbox3[this.button_pay]));
            w27.Position = 3;
            w27.Expand = false;
            w27.Fill = false;
            this.vbox1.Add(this.hbox3);
            Gtk.Box.BoxChild w28 = ((Gtk.Box.BoxChild)(this.vbox1[this.hbox3]));
            w28.Position = 1;
            w28.Expand = false;
            w28.Fill = false;
            // Container child vbox1.Gtk.Box+BoxChild
            this.hbox4 = new Gtk.HBox();
            this.hbox4.Name = "hbox4";
            this.hbox4.Homogeneous = true;
            this.hbox4.Spacing = 6;
            // Container child hbox4.Gtk.Box+BoxChild
            this.button_buy = new Gtk.Button();
            this.button_buy.WidthRequest = 75;
            this.button_buy.Sensitive = false;
            this.button_buy.CanFocus = true;
            this.button_buy.Name = "button_buy";
            this.button_buy.UseUnderline = true;
            // Container child button_buy.Gtk.Container+ContainerChild
            Gtk.Alignment w29 = new Gtk.Alignment(0.5F, 0.5F, 0F, 0F);
            // Container child GtkAlignment.Gtk.Container+ContainerChild
            Gtk.HBox w30 = new Gtk.HBox();
            w30.Spacing = 2;
            // Container child GtkHBox.Gtk.Container+ContainerChild
            Gtk.Image w31 = new Gtk.Image();
            w31.Pixbuf = Gdk.Pixbuf.LoadFromResource("omvviewerlight.art.status_money.tga");
            w30.Add(w31);
            // Container child GtkHBox.Gtk.Container+ContainerChild
            Gtk.Label w33 = new Gtk.Label();
            w33.LabelProp = Mono.Unix.Catalog.GetString("Buy");
            w33.UseUnderline = true;
            w30.Add(w33);
            w29.Add(w30);
            this.button_buy.Add(w29);
            this.hbox4.Add(this.button_buy);
            Gtk.Box.BoxChild w37 = ((Gtk.Box.BoxChild)(this.hbox4[this.button_buy]));
            w37.Position = 0;
            w37.Expand = false;
            w37.Fill = false;
            // Container child hbox4.Gtk.Box+BoxChild
            this.button_take = new Gtk.Button();
            this.button_take.WidthRequest = 75;
            this.button_take.Sensitive = false;
            this.button_take.CanFocus = true;
            this.button_take.Name = "button_take";
            this.button_take.UseUnderline = true;
            this.button_take.Label = Mono.Unix.Catalog.GetString("Take");
            this.hbox4.Add(this.button_take);
            Gtk.Box.BoxChild w38 = ((Gtk.Box.BoxChild)(this.hbox4[this.button_take]));
            w38.Position = 1;
            w38.Expand = false;
            w38.Fill = false;
            // Container child hbox4.Gtk.Box+BoxChild
            this.button_take_copy = new Gtk.Button();
            this.button_take_copy.WidthRequest = 75;
            this.button_take_copy.Sensitive = false;
            this.button_take_copy.CanFocus = true;
            this.button_take_copy.Name = "button_take_copy";
            this.button_take_copy.UseUnderline = true;
            this.button_take_copy.Label = Mono.Unix.Catalog.GetString("Take copy");
            this.hbox4.Add(this.button_take_copy);
            Gtk.Box.BoxChild w39 = ((Gtk.Box.BoxChild)(this.hbox4[this.button_take_copy]));
            w39.Position = 2;
            w39.Expand = false;
            w39.Fill = false;
            // Container child hbox4.Gtk.Box+BoxChild
            this.button_return = new Gtk.Button();
            this.button_return.WidthRequest = 75;
            this.button_return.Sensitive = false;
            this.button_return.CanFocus = true;
            this.button_return.Name = "button_return";
            this.button_return.UseUnderline = true;
            this.button_return.Label = Mono.Unix.Catalog.GetString("Return");
            this.hbox4.Add(this.button_return);
            Gtk.Box.BoxChild w40 = ((Gtk.Box.BoxChild)(this.hbox4[this.button_return]));
            w40.Position = 3;
            w40.Expand = false;
            w40.Fill = false;
            this.vbox1.Add(this.hbox4);
            Gtk.Box.BoxChild w41 = ((Gtk.Box.BoxChild)(this.vbox1[this.hbox4]));
            w41.Position = 2;
            w41.Expand = false;
            w41.Fill = false;
            // Container child vbox1.Gtk.Box+BoxChild
            this.hbox5 = new Gtk.HBox();
            this.hbox5.Name = "hbox5";
            this.hbox5.Homogeneous = true;
            this.hbox5.Spacing = 6;
            // Container child hbox5.Gtk.Box+BoxChild
            this.button_moveto = new Gtk.Button();
            this.button_moveto.WidthRequest = 75;
            this.button_moveto.Sensitive = false;
            this.button_moveto.CanFocus = true;
            this.button_moveto.Name = "button_moveto";
            this.button_moveto.UseUnderline = true;
            this.button_moveto.Label = Mono.Unix.Catalog.GetString("Move to");
            this.hbox5.Add(this.button_moveto);
            Gtk.Box.BoxChild w42 = ((Gtk.Box.BoxChild)(this.hbox5[this.button_moveto]));
            w42.Position = 0;
            w42.Expand = false;
            w42.Fill = false;
            this.vbox1.Add(this.hbox5);
            Gtk.Box.BoxChild w43 = ((Gtk.Box.BoxChild)(this.vbox1[this.hbox5]));
            w43.Position = 3;
            w43.Expand = false;
            w43.Fill = false;
            // Container child vbox1.Gtk.Box+BoxChild
            this.hbox6 = new Gtk.HBox();
            this.hbox6.Name = "hbox6";
            this.hbox6.Spacing = 6;
            // Container child hbox6.Gtk.Box+BoxChild
            this.vbox3 = new Gtk.VBox();
            this.vbox3.Name = "vbox3";
            this.vbox3.Spacing = 6;
            // Container child vbox3.Gtk.Box+BoxChild
            this.label2 = new Gtk.Label();
            this.label2.Name = "label2";
            this.label2.Xalign = 0F;
            this.label2.LabelProp = Mono.Unix.Catalog.GetString("Name");
            this.vbox3.Add(this.label2);
            Gtk.Box.BoxChild w44 = ((Gtk.Box.BoxChild)(this.vbox3[this.label2]));
            w44.Position = 0;
            w44.Expand = false;
            w44.Fill = false;
            // Container child vbox3.Gtk.Box+BoxChild
            this.label3 = new Gtk.Label();
            this.label3.Name = "label3";
            this.label3.Xalign = 0F;
            this.label3.LabelProp = Mono.Unix.Catalog.GetString("Key");
            this.vbox3.Add(this.label3);
            Gtk.Box.BoxChild w45 = ((Gtk.Box.BoxChild)(this.vbox3[this.label3]));
            w45.Position = 1;
            w45.Expand = false;
            w45.Fill = false;
            // Container child vbox3.Gtk.Box+BoxChild
            this.label4 = new Gtk.Label();
            this.label4.Name = "label4";
            this.label4.Xalign = 0F;
            this.label4.LabelProp = Mono.Unix.Catalog.GetString("Description");
            this.vbox3.Add(this.label4);
            Gtk.Box.BoxChild w46 = ((Gtk.Box.BoxChild)(this.vbox3[this.label4]));
            w46.Position = 2;
            w46.Expand = false;
            w46.Fill = false;
            // Container child vbox3.Gtk.Box+BoxChild
            this.label6 = new Gtk.Label();
            this.label6.Name = "label6";
            this.label6.Xalign = 0F;
            this.label6.LabelProp = Mono.Unix.Catalog.GetString("Owner");
            this.vbox3.Add(this.label6);
            Gtk.Box.BoxChild w47 = ((Gtk.Box.BoxChild)(this.vbox3[this.label6]));
            w47.Position = 3;
            w47.Expand = false;
            w47.Fill = false;
            // Container child vbox3.Gtk.Box+BoxChild
            this.label7 = new Gtk.Label();
            this.label7.Name = "label7";
            this.label7.Xalign = 0F;
            this.label7.LabelProp = Mono.Unix.Catalog.GetString("Group");
            this.vbox3.Add(this.label7);
            Gtk.Box.BoxChild w48 = ((Gtk.Box.BoxChild)(this.vbox3[this.label7]));
            w48.Position = 4;
            w48.Expand = false;
            w48.Fill = false;
            // Container child vbox3.Gtk.Box+BoxChild
            this.label9 = new Gtk.Label();
            this.label9.Name = "label9";
            this.label9.Xalign = 0.01F;
            this.label9.LabelProp = Mono.Unix.Catalog.GetString("For sale");
            this.vbox3.Add(this.label9);
            Gtk.Box.BoxChild w49 = ((Gtk.Box.BoxChild)(this.vbox3[this.label9]));
            w49.Position = 5;
            w49.Expand = false;
            w49.Fill = false;
            // Container child vbox3.Gtk.Box+BoxChild
            this.label5 = new Gtk.Label();
            this.label5.Name = "label5";
            this.label5.Xalign = 0F;
            this.label5.LabelProp = Mono.Unix.Catalog.GetString("Position");
            this.vbox3.Add(this.label5);
            Gtk.Box.BoxChild w50 = ((Gtk.Box.BoxChild)(this.vbox3[this.label5]));
            w50.Position = 6;
            w50.Expand = false;
            w50.Fill = false;
            // Container child vbox3.Gtk.Box+BoxChild
            this.label10 = new Gtk.Label();
            this.label10.Name = "label10";
            this.label10.Xalign = 0F;
            this.label10.LabelProp = Mono.Unix.Catalog.GetString("Distance");
            this.vbox3.Add(this.label10);
            Gtk.Box.BoxChild w51 = ((Gtk.Box.BoxChild)(this.vbox3[this.label10]));
            w51.Position = 7;
            w51.Expand = false;
            w51.Fill = false;
            // Container child vbox3.Gtk.Box+BoxChild
            this.label8 = new Gtk.Label();
            this.label8.Name = "label8";
            this.label8.Xalign = 0F;
            this.label8.LabelProp = Mono.Unix.Catalog.GetString("Text");
            this.vbox3.Add(this.label8);
            Gtk.Box.BoxChild w52 = ((Gtk.Box.BoxChild)(this.vbox3[this.label8]));
            w52.Position = 8;
            w52.Expand = false;
            w52.Fill = false;
            this.hbox6.Add(this.vbox3);
            Gtk.Box.BoxChild w53 = ((Gtk.Box.BoxChild)(this.hbox6[this.vbox3]));
            w53.Position = 0;
            w53.Expand = false;
            w53.Fill = false;
            // Container child hbox6.Gtk.Box+BoxChild
            this.vbox4 = new Gtk.VBox();
            this.vbox4.Name = "vbox4";
            this.vbox4.Spacing = 6;
            // Container child vbox4.Gtk.Box+BoxChild
            this.label_name = new Gtk.Label();
            this.label_name.Name = "label_name";
            this.label_name.Xalign = 0F;
            this.label_name.LabelProp = Mono.Unix.Catalog.GetString("label3");
            this.vbox4.Add(this.label_name);
            Gtk.Box.BoxChild w54 = ((Gtk.Box.BoxChild)(this.vbox4[this.label_name]));
            w54.Position = 0;
            w54.Expand = false;
            w54.Fill = false;
            // Container child vbox4.Gtk.Box+BoxChild
            this.label_key = new Gtk.Label();
            this.label_key.Name = "label_key";
            this.label_key.Xalign = 0F;
            this.label_key.LabelProp = Mono.Unix.Catalog.GetString("label2");
            this.vbox4.Add(this.label_key);
            Gtk.Box.BoxChild w55 = ((Gtk.Box.BoxChild)(this.vbox4[this.label_key]));
            w55.Position = 1;
            w55.Expand = false;
            w55.Fill = false;
            // Container child vbox4.Gtk.Box+BoxChild
            this.label_desc = new Gtk.Label();
            this.label_desc.Name = "label_desc";
            this.label_desc.Xalign = 0F;
            this.label_desc.LabelProp = Mono.Unix.Catalog.GetString("label4");
            this.vbox4.Add(this.label_desc);
            Gtk.Box.BoxChild w56 = ((Gtk.Box.BoxChild)(this.vbox4[this.label_desc]));
            w56.Position = 2;
            w56.Expand = false;
            w56.Fill = false;
            // Container child vbox4.Gtk.Box+BoxChild
            this.label_owner = new Gtk.Label();
            this.label_owner.Name = "label_owner";
            this.label_owner.Xalign = 0F;
            this.label_owner.LabelProp = Mono.Unix.Catalog.GetString("label6");
            this.vbox4.Add(this.label_owner);
            Gtk.Box.BoxChild w57 = ((Gtk.Box.BoxChild)(this.vbox4[this.label_owner]));
            w57.Position = 3;
            w57.Expand = false;
            w57.Fill = false;
            // Container child vbox4.Gtk.Box+BoxChild
            this.label_group = new Gtk.Label();
            this.label_group.Name = "label_group";
            this.label_group.Xalign = 0F;
            this.label_group.LabelProp = Mono.Unix.Catalog.GetString("label8");
            this.vbox4.Add(this.label_group);
            Gtk.Box.BoxChild w58 = ((Gtk.Box.BoxChild)(this.vbox4[this.label_group]));
            w58.Position = 4;
            w58.Expand = false;
            w58.Fill = false;
            // Container child vbox4.Gtk.Box+BoxChild
            this.label_forsale = new Gtk.Label();
            this.label_forsale.Name = "label_forsale";
            this.label_forsale.Xalign = 0F;
            this.label_forsale.LabelProp = Mono.Unix.Catalog.GetString("label10");
            this.vbox4.Add(this.label_forsale);
            Gtk.Box.BoxChild w59 = ((Gtk.Box.BoxChild)(this.vbox4[this.label_forsale]));
            w59.Position = 5;
            w59.Expand = false;
            w59.Fill = false;
            // Container child vbox4.Gtk.Box+BoxChild
            this.label_pos = new Gtk.Label();
            this.label_pos.Name = "label_pos";
            this.label_pos.Xalign = 0F;
            this.label_pos.LabelProp = Mono.Unix.Catalog.GetString("label4");
            this.vbox4.Add(this.label_pos);
            Gtk.Box.BoxChild w60 = ((Gtk.Box.BoxChild)(this.vbox4[this.label_pos]));
            w60.Position = 6;
            w60.Expand = false;
            w60.Fill = false;
            // Container child vbox4.Gtk.Box+BoxChild
            this.label_distance = new Gtk.Label();
            this.label_distance.Name = "label_distance";
            this.label_distance.Xalign = 0F;
            this.label_distance.LabelProp = Mono.Unix.Catalog.GetString("label6");
            this.vbox4.Add(this.label_distance);
            Gtk.Box.BoxChild w61 = ((Gtk.Box.BoxChild)(this.vbox4[this.label_distance]));
            w61.Position = 7;
            w61.Expand = false;
            w61.Fill = false;
            // Container child vbox4.Gtk.Box+BoxChild
            this.label_float_text = new Gtk.Label();
            this.label_float_text.Name = "label_float_text";
            this.label_float_text.Xalign = 0F;
            this.label_float_text.LabelProp = Mono.Unix.Catalog.GetString("label8");
            this.vbox4.Add(this.label_float_text);
            Gtk.Box.BoxChild w62 = ((Gtk.Box.BoxChild)(this.vbox4[this.label_float_text]));
            w62.Position = 8;
            w62.Expand = false;
            w62.Fill = false;
            this.hbox6.Add(this.vbox4);
            Gtk.Box.BoxChild w63 = ((Gtk.Box.BoxChild)(this.hbox6[this.vbox4]));
            w63.Position = 1;
            w63.Expand = false;
            w63.Fill = false;
            this.vbox1.Add(this.hbox6);
            Gtk.Box.BoxChild w64 = ((Gtk.Box.BoxChild)(this.vbox1[this.hbox6]));
            w64.Position = 4;
            w64.Expand = false;
            w64.Fill = false;
            // Container child vbox1.Gtk.Box+BoxChild
            this.GtkScrolledWindow1 = new Gtk.ScrolledWindow();
            this.GtkScrolledWindow1.Name = "GtkScrolledWindow1";
            this.GtkScrolledWindow1.ShadowType = ((Gtk.ShadowType)(1));
            // Container child GtkScrolledWindow1.Gtk.Container+ContainerChild
            this.textview1 = new Gtk.TextView();
            this.textview1.CanFocus = true;
            this.textview1.Name = "textview1";
            this.textview1.Editable = false;
            this.textview1.CursorVisible = false;
            this.textview1.WrapMode = ((Gtk.WrapMode)(2));
            this.GtkScrolledWindow1.Add(this.textview1);
            this.vbox1.Add(this.GtkScrolledWindow1);
            Gtk.Box.BoxChild w66 = ((Gtk.Box.BoxChild)(this.vbox1[this.GtkScrolledWindow1]));
            w66.Position = 5;
            this.hbox1.Add(this.vbox1);
            Gtk.Box.BoxChild w67 = ((Gtk.Box.BoxChild)(this.hbox1[this.vbox1]));
            w67.Position = 1;
            w67.Expand = false;
            w67.Fill = false;
            this.Add(this.hbox1);
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.Show();
            this.treeview1.CursorChanged += new System.EventHandler(this.OnTreeview1CursorChanged);
            this.treeview1.UnselectAll += new Gtk.UnselectAllHandler(this.OnTreeview1UnselectAll);
            this.button_search.Clicked += new System.EventHandler(this.OnButtonSearchClicked);
            this.button_touch.Clicked += new System.EventHandler(this.OnButtonTouchClicked);
            this.button_siton.Clicked += new System.EventHandler(this.OnButtonSitonClicked);
            this.button_lookat.Clicked += new System.EventHandler(this.OnButtonLookatClicked);
            this.button_pay.Clicked += new System.EventHandler(this.OnButtonPayClicked);
            this.button_buy.Clicked += new System.EventHandler(this.OnButtonBuyClicked);
            this.button_take.Clicked += new System.EventHandler(this.OnButtonTakeClicked);
            this.button_take_copy.Clicked += new System.EventHandler(this.OnButtonTakeCopyClicked);
            this.button_return.Clicked += new System.EventHandler(this.OnButtonReturnClicked);
            this.button_moveto.Clicked += new System.EventHandler(this.OnButtonMovetoClicked);
        }
    }
}
