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
    
    
    public partial class Inventory {
        
        private Gtk.HBox hbox1;
        
        private Gtk.VBox vbox2;
        
        private Gtk.HBox hbox7;
        
        private Gtk.Label label1;
        
        private Gtk.Image image30;
        
        private Gtk.Entry entry_search;
        
        private Gtk.Button button_expandall;
        
        private Gtk.Button button_collapse_all;
        
        private Gtk.Label label_fetched;
        
        private Gtk.ScrolledWindow GtkScrolledWindow;
        
        private Gtk.TreeView treeview_inv;
        
        private Gtk.VBox vbox1;
        
        private Gtk.VBox vbox3;
        
        private Gtk.CheckButton check_special_folders;
        
        private Gtk.HBox hbox8;
        
        private Gtk.RadioButton radiobutton2;
        
        private Gtk.RadioButton radiobutton1;
        
        private Gtk.HBox hbox5;
        
        private Gtk.VBox vbox4;
        
        private Gtk.HBox hbox6;
        
        private Gtk.VBox vbox8;
        
        private Gtk.Label label7;
        
        private Gtk.Label label8;
        
        private Gtk.Label label11;
        
        private Gtk.Label label2;
        
        private Gtk.Label label4;
        
        private Gtk.VBox vbox5;
        
        private Gtk.Label label_name;
        
        private Gtk.Label label_createdby;
        
        private Gtk.Label label_aquired;
        
        private Gtk.Label label_group;
        
        private Gtk.Label label_saleprice;
        
        private Gtk.HBox hbox4;
        
        private Gtk.HBox hbox3;
        
        private Gtk.Label label14;
        
        private Gtk.VBox vbox7;
        
        private Gtk.CheckButton checkbutton_copynext;
        
        private Gtk.CheckButton checkbutton_modnext;
        
        private Gtk.CheckButton checkbutton_transnext;
        
        private Gtk.HBox hbox2;
        
        private Gtk.Label label13;
        
        private Gtk.VBox vbox6;
        
        private Gtk.CheckButton checkbutton_copy;
        
        private Gtk.CheckButton checkbutton_mod;
        
        private Gtk.CheckButton checkbutton_trans;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget omvviewerlight.Inventory
            Stetic.BinContainer.Attach(this);
            this.Name = "omvviewerlight.Inventory";
            // Container child omvviewerlight.Inventory.Gtk.Container+ContainerChild
            this.hbox1 = new Gtk.HBox();
            this.hbox1.Name = "hbox1";
            this.hbox1.Spacing = 6;
            // Container child hbox1.Gtk.Box+BoxChild
            this.vbox2 = new Gtk.VBox();
            this.vbox2.Name = "vbox2";
            this.vbox2.Spacing = 6;
            // Container child vbox2.Gtk.Box+BoxChild
            this.hbox7 = new Gtk.HBox();
            this.hbox7.Name = "hbox7";
            this.hbox7.Spacing = 6;
            // Container child hbox7.Gtk.Box+BoxChild
            this.label1 = new Gtk.Label();
            this.label1.Name = "label1";
            this.hbox7.Add(this.label1);
            Gtk.Box.BoxChild w1 = ((Gtk.Box.BoxChild)(this.hbox7[this.label1]));
            w1.Position = 0;
            w1.Expand = false;
            w1.Fill = false;
            // Container child hbox7.Gtk.Box+BoxChild
            this.image30 = new Gtk.Image();
            this.image30.Name = "image30";
            this.image30.Pixbuf = MainClass.GetResource("status_search_btn.png");
            this.hbox7.Add(this.image30);
            Gtk.Box.BoxChild w2 = ((Gtk.Box.BoxChild)(this.hbox7[this.image30]));
            w2.Position = 1;
            w2.Expand = false;
            w2.Fill = false;
            // Container child hbox7.Gtk.Box+BoxChild
            this.entry_search = new Gtk.Entry();
            this.entry_search.CanFocus = true;
            this.entry_search.Name = "entry_search";
            this.entry_search.IsEditable = true;
            this.entry_search.InvisibleChar = '●';
            this.hbox7.Add(this.entry_search);
            Gtk.Box.BoxChild w3 = ((Gtk.Box.BoxChild)(this.hbox7[this.entry_search]));
            w3.Position = 2;
            // Container child hbox7.Gtk.Box+BoxChild
            this.button_expandall = new Gtk.Button();
            this.button_expandall.CanFocus = true;
            this.button_expandall.Name = "button_expandall";
            this.button_expandall.UseUnderline = true;
            // Container child button_expandall.Gtk.Container+ContainerChild
            Gtk.Alignment w4 = new Gtk.Alignment(0.5F, 0.5F, 0F, 0F);
            // Container child GtkAlignment.Gtk.Container+ContainerChild
            Gtk.HBox w5 = new Gtk.HBox();
            w5.Spacing = 2;
            // Container child GtkHBox.Gtk.Container+ContainerChild
            Gtk.Image w6 = new Gtk.Image();
            w6.Pixbuf = Stetic.IconLoader.LoadIcon(this, "gtk-zoom-in", Gtk.IconSize.Menu, 16);
            w5.Add(w6);
            // Container child GtkHBox.Gtk.Container+ContainerChild
            Gtk.Label w8 = new Gtk.Label();
            w5.Add(w8);
            w4.Add(w5);
            this.button_expandall.Add(w4);
            this.hbox7.Add(this.button_expandall);
            Gtk.Box.BoxChild w12 = ((Gtk.Box.BoxChild)(this.hbox7[this.button_expandall]));
            w12.Position = 3;
            w12.Expand = false;
            w12.Fill = false;
            // Container child hbox7.Gtk.Box+BoxChild
            this.button_collapse_all = new Gtk.Button();
            this.button_collapse_all.CanFocus = true;
            this.button_collapse_all.Name = "button_collapse_all";
            this.button_collapse_all.UseUnderline = true;
            // Container child button_collapse_all.Gtk.Container+ContainerChild
            Gtk.Alignment w13 = new Gtk.Alignment(0.5F, 0.5F, 0F, 0F);
            // Container child GtkAlignment.Gtk.Container+ContainerChild
            Gtk.HBox w14 = new Gtk.HBox();
            w14.Spacing = 2;
            // Container child GtkHBox.Gtk.Container+ContainerChild
            Gtk.Image w15 = new Gtk.Image();
            w15.Pixbuf = Stetic.IconLoader.LoadIcon(this, "gtk-zoom-out", Gtk.IconSize.Menu, 16);
            w14.Add(w15);
            // Container child GtkHBox.Gtk.Container+ContainerChild
            Gtk.Label w17 = new Gtk.Label();
            w14.Add(w17);
            w13.Add(w14);
            this.button_collapse_all.Add(w13);
            this.hbox7.Add(this.button_collapse_all);
            Gtk.Box.BoxChild w21 = ((Gtk.Box.BoxChild)(this.hbox7[this.button_collapse_all]));
            w21.Position = 4;
            w21.Expand = false;
            w21.Fill = false;
            this.vbox2.Add(this.hbox7);
            Gtk.Box.BoxChild w22 = ((Gtk.Box.BoxChild)(this.vbox2[this.hbox7]));
            w22.Position = 0;
            w22.Expand = false;
            w22.Fill = false;
            // Container child vbox2.Gtk.Box+BoxChild
            this.label_fetched = new Gtk.Label();
            this.label_fetched.Name = "label_fetched";
            this.label_fetched.Xalign = 0F;
            this.label_fetched.LabelProp = Mono.Unix.Catalog.GetString("Fetched 0 items");
            this.vbox2.Add(this.label_fetched);
            Gtk.Box.BoxChild w23 = ((Gtk.Box.BoxChild)(this.vbox2[this.label_fetched]));
            w23.Position = 1;
            w23.Expand = false;
            w23.Fill = false;
            // Container child vbox2.Gtk.Box+BoxChild
            this.GtkScrolledWindow = new Gtk.ScrolledWindow();
            this.GtkScrolledWindow.Name = "GtkScrolledWindow";
            this.GtkScrolledWindow.ShadowType = ((Gtk.ShadowType)(1));
            // Container child GtkScrolledWindow.Gtk.Container+ContainerChild
            this.treeview_inv = new Gtk.TreeView();
            this.treeview_inv.WidthRequest = 450;
            this.treeview_inv.CanFocus = true;
            this.treeview_inv.Name = "treeview_inv";
            this.GtkScrolledWindow.Add(this.treeview_inv);
            this.vbox2.Add(this.GtkScrolledWindow);
            Gtk.Box.BoxChild w25 = ((Gtk.Box.BoxChild)(this.vbox2[this.GtkScrolledWindow]));
            w25.Position = 2;
            this.hbox1.Add(this.vbox2);
            Gtk.Box.BoxChild w26 = ((Gtk.Box.BoxChild)(this.hbox1[this.vbox2]));
            w26.Position = 0;
            w26.Expand = false;
            w26.Fill = false;
            // Container child hbox1.Gtk.Box+BoxChild
            this.vbox1 = new Gtk.VBox();
            this.vbox1.Name = "vbox1";
            this.vbox1.Spacing = 6;
            // Container child vbox1.Gtk.Box+BoxChild
            this.vbox3 = new Gtk.VBox();
            this.vbox3.Name = "vbox3";
            this.vbox3.Spacing = 6;
            // Container child vbox3.Gtk.Box+BoxChild
            this.check_special_folders = new Gtk.CheckButton();
            this.check_special_folders.CanFocus = true;
            this.check_special_folders.Name = "check_special_folders";
            this.check_special_folders.Label = Mono.Unix.Catalog.GetString("System folders top");
            this.check_special_folders.DrawIndicator = true;
            this.check_special_folders.UseUnderline = true;
            this.vbox3.Add(this.check_special_folders);
            Gtk.Box.BoxChild w27 = ((Gtk.Box.BoxChild)(this.vbox3[this.check_special_folders]));
            w27.Position = 0;
            w27.Expand = false;
            w27.Fill = false;
            // Container child vbox3.Gtk.Box+BoxChild
            this.hbox8 = new Gtk.HBox();
            this.hbox8.Name = "hbox8";
            this.hbox8.Spacing = 6;
            // Container child hbox8.Gtk.Box+BoxChild
            this.radiobutton2 = new Gtk.RadioButton(Mono.Unix.Catalog.GetString("Sort Name"));
            this.radiobutton2.CanFocus = true;
            this.radiobutton2.Name = "radiobutton2";
            this.radiobutton2.DrawIndicator = true;
            this.radiobutton2.UseUnderline = true;
            this.radiobutton2.Group = new GLib.SList(System.IntPtr.Zero);
            this.hbox8.Add(this.radiobutton2);
            Gtk.Box.BoxChild w28 = ((Gtk.Box.BoxChild)(this.hbox8[this.radiobutton2]));
            w28.Position = 0;
            // Container child hbox8.Gtk.Box+BoxChild
            this.radiobutton1 = new Gtk.RadioButton(Mono.Unix.Catalog.GetString("Sort Date"));
            this.radiobutton1.CanFocus = true;
            this.radiobutton1.Name = "radiobutton1";
            this.radiobutton1.DrawIndicator = true;
            this.radiobutton1.UseUnderline = true;
            this.radiobutton1.Group = this.radiobutton2.Group;
            this.hbox8.Add(this.radiobutton1);
            Gtk.Box.BoxChild w29 = ((Gtk.Box.BoxChild)(this.hbox8[this.radiobutton1]));
            w29.Position = 1;
            this.vbox3.Add(this.hbox8);
            Gtk.Box.BoxChild w30 = ((Gtk.Box.BoxChild)(this.vbox3[this.hbox8]));
            w30.Position = 1;
            w30.Expand = false;
            w30.Fill = false;
            this.vbox1.Add(this.vbox3);
            Gtk.Box.BoxChild w31 = ((Gtk.Box.BoxChild)(this.vbox1[this.vbox3]));
            w31.Position = 0;
            w31.Expand = false;
            w31.Fill = false;
            // Container child vbox1.Gtk.Box+BoxChild
            this.hbox5 = new Gtk.HBox();
            this.hbox5.Name = "hbox5";
            this.hbox5.Spacing = 6;
            // Container child hbox5.Gtk.Box+BoxChild
            this.vbox4 = new Gtk.VBox();
            this.vbox4.Name = "vbox4";
            this.vbox4.Spacing = 6;
            // Container child vbox4.Gtk.Box+BoxChild
            this.hbox6 = new Gtk.HBox();
            this.hbox6.Name = "hbox6";
            this.hbox6.Spacing = 6;
            // Container child hbox6.Gtk.Box+BoxChild
            this.vbox8 = new Gtk.VBox();
            this.vbox8.WidthRequest = 81;
            this.vbox8.Name = "vbox8";
            this.vbox8.Spacing = 6;
            // Container child vbox8.Gtk.Box+BoxChild
            this.label7 = new Gtk.Label();
            this.label7.Name = "label7";
            this.label7.Xalign = 0F;
            this.label7.LabelProp = Mono.Unix.Catalog.GetString("Name");
            this.vbox8.Add(this.label7);
            Gtk.Box.BoxChild w32 = ((Gtk.Box.BoxChild)(this.vbox8[this.label7]));
            w32.Position = 0;
            w32.Expand = false;
            w32.Fill = false;
            // Container child vbox8.Gtk.Box+BoxChild
            this.label8 = new Gtk.Label();
            this.label8.Name = "label8";
            this.label8.Xalign = 0F;
            this.label8.LabelProp = Mono.Unix.Catalog.GetString("Created By");
            this.vbox8.Add(this.label8);
            Gtk.Box.BoxChild w33 = ((Gtk.Box.BoxChild)(this.vbox8[this.label8]));
            w33.Position = 1;
            w33.Expand = false;
            w33.Fill = false;
            // Container child vbox8.Gtk.Box+BoxChild
            this.label11 = new Gtk.Label();
            this.label11.Name = "label11";
            this.label11.Xalign = 0F;
            this.label11.LabelProp = Mono.Unix.Catalog.GetString("Aquired");
            this.vbox8.Add(this.label11);
            Gtk.Box.BoxChild w34 = ((Gtk.Box.BoxChild)(this.vbox8[this.label11]));
            w34.Position = 2;
            w34.Expand = false;
            w34.Fill = false;
            // Container child vbox8.Gtk.Box+BoxChild
            this.label2 = new Gtk.Label();
            this.label2.Name = "label2";
            this.label2.Xalign = 0F;
            this.label2.LabelProp = Mono.Unix.Catalog.GetString("Group");
            this.vbox8.Add(this.label2);
            Gtk.Box.BoxChild w35 = ((Gtk.Box.BoxChild)(this.vbox8[this.label2]));
            w35.Position = 3;
            w35.Expand = false;
            w35.Fill = false;
            // Container child vbox8.Gtk.Box+BoxChild
            this.label4 = new Gtk.Label();
            this.label4.Name = "label4";
            this.label4.Xalign = 0F;
            this.label4.LabelProp = Mono.Unix.Catalog.GetString("Sale price");
            this.vbox8.Add(this.label4);
            Gtk.Box.BoxChild w36 = ((Gtk.Box.BoxChild)(this.vbox8[this.label4]));
            w36.Position = 4;
            w36.Expand = false;
            w36.Fill = false;
            this.hbox6.Add(this.vbox8);
            Gtk.Box.BoxChild w37 = ((Gtk.Box.BoxChild)(this.hbox6[this.vbox8]));
            w37.Position = 0;
            w37.Expand = false;
            w37.Fill = false;
            // Container child hbox6.Gtk.Box+BoxChild
            this.vbox5 = new Gtk.VBox();
            this.vbox5.WidthRequest = 175;
            this.vbox5.Name = "vbox5";
            this.vbox5.Spacing = 6;
            // Container child vbox5.Gtk.Box+BoxChild
            this.label_name = new Gtk.Label();
            this.label_name.Name = "label_name";
            this.label_name.Xalign = 0F;
            this.label_name.LabelProp = Mono.Unix.Catalog.GetString("label9");
            this.vbox5.Add(this.label_name);
            Gtk.Box.BoxChild w38 = ((Gtk.Box.BoxChild)(this.vbox5[this.label_name]));
            w38.Position = 0;
            w38.Expand = false;
            w38.Fill = false;
            // Container child vbox5.Gtk.Box+BoxChild
            this.label_createdby = new Gtk.Label();
            this.label_createdby.Name = "label_createdby";
            this.label_createdby.Xalign = 0F;
            this.label_createdby.LabelProp = Mono.Unix.Catalog.GetString("label10");
            this.vbox5.Add(this.label_createdby);
            Gtk.Box.BoxChild w39 = ((Gtk.Box.BoxChild)(this.vbox5[this.label_createdby]));
            w39.Position = 1;
            w39.Expand = false;
            w39.Fill = false;
            // Container child vbox5.Gtk.Box+BoxChild
            this.label_aquired = new Gtk.Label();
            this.label_aquired.Name = "label_aquired";
            this.label_aquired.Xalign = 0F;
            this.label_aquired.LabelProp = Mono.Unix.Catalog.GetString("label12");
            this.vbox5.Add(this.label_aquired);
            Gtk.Box.BoxChild w40 = ((Gtk.Box.BoxChild)(this.vbox5[this.label_aquired]));
            w40.Position = 2;
            w40.Expand = false;
            w40.Fill = false;
            // Container child vbox5.Gtk.Box+BoxChild
            this.label_group = new Gtk.Label();
            this.label_group.Name = "label_group";
            this.label_group.Xalign = 0F;
            this.label_group.LabelProp = Mono.Unix.Catalog.GetString("label3");
            this.vbox5.Add(this.label_group);
            Gtk.Box.BoxChild w41 = ((Gtk.Box.BoxChild)(this.vbox5[this.label_group]));
            w41.Position = 3;
            w41.Expand = false;
            w41.Fill = false;
            // Container child vbox5.Gtk.Box+BoxChild
            this.label_saleprice = new Gtk.Label();
            this.label_saleprice.Name = "label_saleprice";
            this.label_saleprice.Xalign = 0F;
            this.label_saleprice.Yalign = 0F;
            this.vbox5.Add(this.label_saleprice);
            Gtk.Box.BoxChild w42 = ((Gtk.Box.BoxChild)(this.vbox5[this.label_saleprice]));
            w42.Position = 4;
            w42.Expand = false;
            w42.Fill = false;
            this.hbox6.Add(this.vbox5);
            Gtk.Box.BoxChild w43 = ((Gtk.Box.BoxChild)(this.hbox6[this.vbox5]));
            w43.Position = 1;
            w43.Expand = false;
            w43.Fill = false;
            this.vbox4.Add(this.hbox6);
            Gtk.Box.BoxChild w44 = ((Gtk.Box.BoxChild)(this.vbox4[this.hbox6]));
            w44.Position = 0;
            w44.Expand = false;
            w44.Fill = false;
            this.hbox5.Add(this.vbox4);
            Gtk.Box.BoxChild w45 = ((Gtk.Box.BoxChild)(this.hbox5[this.vbox4]));
            w45.Position = 0;
            w45.Expand = false;
            w45.Fill = false;
            this.vbox1.Add(this.hbox5);
            Gtk.Box.BoxChild w46 = ((Gtk.Box.BoxChild)(this.vbox1[this.hbox5]));
            w46.Position = 1;
            w46.Expand = false;
            w46.Fill = false;
            // Container child vbox1.Gtk.Box+BoxChild
            this.hbox4 = new Gtk.HBox();
            this.hbox4.Name = "hbox4";
            this.hbox4.Spacing = 6;
            this.vbox1.Add(this.hbox4);
            Gtk.Box.BoxChild w47 = ((Gtk.Box.BoxChild)(this.vbox1[this.hbox4]));
            w47.PackType = ((Gtk.PackType)(1));
            w47.Position = 2;
            // Container child vbox1.Gtk.Box+BoxChild
            this.hbox3 = new Gtk.HBox();
            this.hbox3.Name = "hbox3";
            this.hbox3.Spacing = 6;
            // Container child hbox3.Gtk.Box+BoxChild
            this.label14 = new Gtk.Label();
            this.label14.WidthRequest = 78;
            this.label14.Name = "label14";
            this.label14.Xalign = 0F;
            this.label14.LabelProp = Mono.Unix.Catalog.GetString("Next owner\npermissions");
            this.hbox3.Add(this.label14);
            Gtk.Box.BoxChild w48 = ((Gtk.Box.BoxChild)(this.hbox3[this.label14]));
            w48.Position = 0;
            w48.Expand = false;
            w48.Fill = false;
            // Container child hbox3.Gtk.Box+BoxChild
            this.vbox7 = new Gtk.VBox();
            this.vbox7.Name = "vbox7";
            this.vbox7.Spacing = -7;
            // Container child vbox7.Gtk.Box+BoxChild
            this.checkbutton_copynext = new Gtk.CheckButton();
            this.checkbutton_copynext.Name = "checkbutton_copynext";
            this.checkbutton_copynext.Label = Mono.Unix.Catalog.GetString("Copy");
            this.checkbutton_copynext.DrawIndicator = true;
            this.checkbutton_copynext.UseUnderline = true;
            this.vbox7.Add(this.checkbutton_copynext);
            Gtk.Box.BoxChild w49 = ((Gtk.Box.BoxChild)(this.vbox7[this.checkbutton_copynext]));
            w49.Position = 0;
            w49.Expand = false;
            w49.Fill = false;
            // Container child vbox7.Gtk.Box+BoxChild
            this.checkbutton_modnext = new Gtk.CheckButton();
            this.checkbutton_modnext.Name = "checkbutton_modnext";
            this.checkbutton_modnext.Label = Mono.Unix.Catalog.GetString("Modify");
            this.checkbutton_modnext.DrawIndicator = true;
            this.checkbutton_modnext.UseUnderline = true;
            this.vbox7.Add(this.checkbutton_modnext);
            Gtk.Box.BoxChild w50 = ((Gtk.Box.BoxChild)(this.vbox7[this.checkbutton_modnext]));
            w50.Position = 1;
            w50.Expand = false;
            w50.Fill = false;
            // Container child vbox7.Gtk.Box+BoxChild
            this.checkbutton_transnext = new Gtk.CheckButton();
            this.checkbutton_transnext.Name = "checkbutton_transnext";
            this.checkbutton_transnext.Label = Mono.Unix.Catalog.GetString("Transfer");
            this.checkbutton_transnext.DrawIndicator = true;
            this.checkbutton_transnext.UseUnderline = true;
            this.vbox7.Add(this.checkbutton_transnext);
            Gtk.Box.BoxChild w51 = ((Gtk.Box.BoxChild)(this.vbox7[this.checkbutton_transnext]));
            w51.Position = 2;
            w51.Expand = false;
            w51.Fill = false;
            this.hbox3.Add(this.vbox7);
            Gtk.Box.BoxChild w52 = ((Gtk.Box.BoxChild)(this.hbox3[this.vbox7]));
            w52.Position = 1;
            this.vbox1.Add(this.hbox3);
            Gtk.Box.BoxChild w53 = ((Gtk.Box.BoxChild)(this.vbox1[this.hbox3]));
            w53.PackType = ((Gtk.PackType)(1));
            w53.Position = 3;
            w53.Expand = false;
            w53.Fill = false;
            // Container child vbox1.Gtk.Box+BoxChild
            this.hbox2 = new Gtk.HBox();
            this.hbox2.Name = "hbox2";
            this.hbox2.Spacing = 6;
            // Container child hbox2.Gtk.Box+BoxChild
            this.label13 = new Gtk.Label();
            this.label13.WidthRequest = 78;
            this.label13.Name = "label13";
            this.label13.Xalign = 0F;
            this.label13.LabelProp = Mono.Unix.Catalog.GetString("My\npermissions");
            this.hbox2.Add(this.label13);
            Gtk.Box.BoxChild w54 = ((Gtk.Box.BoxChild)(this.hbox2[this.label13]));
            w54.Position = 0;
            w54.Expand = false;
            w54.Fill = false;
            // Container child hbox2.Gtk.Box+BoxChild
            this.vbox6 = new Gtk.VBox();
            this.vbox6.Name = "vbox6";
            this.vbox6.Spacing = -7;
            // Container child vbox6.Gtk.Box+BoxChild
            this.checkbutton_copy = new Gtk.CheckButton();
            this.checkbutton_copy.Sensitive = false;
            this.checkbutton_copy.Name = "checkbutton_copy";
            this.checkbutton_copy.Label = Mono.Unix.Catalog.GetString("Copy");
            this.checkbutton_copy.DrawIndicator = true;
            this.checkbutton_copy.UseUnderline = true;
            this.vbox6.Add(this.checkbutton_copy);
            Gtk.Box.BoxChild w55 = ((Gtk.Box.BoxChild)(this.vbox6[this.checkbutton_copy]));
            w55.Position = 0;
            w55.Expand = false;
            w55.Fill = false;
            // Container child vbox6.Gtk.Box+BoxChild
            this.checkbutton_mod = new Gtk.CheckButton();
            this.checkbutton_mod.Sensitive = false;
            this.checkbutton_mod.Name = "checkbutton_mod";
            this.checkbutton_mod.Label = Mono.Unix.Catalog.GetString("Modify");
            this.checkbutton_mod.DrawIndicator = true;
            this.checkbutton_mod.UseUnderline = true;
            this.vbox6.Add(this.checkbutton_mod);
            Gtk.Box.BoxChild w56 = ((Gtk.Box.BoxChild)(this.vbox6[this.checkbutton_mod]));
            w56.Position = 1;
            w56.Expand = false;
            w56.Fill = false;
            // Container child vbox6.Gtk.Box+BoxChild
            this.checkbutton_trans = new Gtk.CheckButton();
            this.checkbutton_trans.Sensitive = false;
            this.checkbutton_trans.Name = "checkbutton_trans";
            this.checkbutton_trans.Label = Mono.Unix.Catalog.GetString("Transfer");
            this.checkbutton_trans.DrawIndicator = true;
            this.checkbutton_trans.UseUnderline = true;
            this.vbox6.Add(this.checkbutton_trans);
            Gtk.Box.BoxChild w57 = ((Gtk.Box.BoxChild)(this.vbox6[this.checkbutton_trans]));
            w57.Position = 2;
            w57.Expand = false;
            w57.Fill = false;
            this.hbox2.Add(this.vbox6);
            Gtk.Box.BoxChild w58 = ((Gtk.Box.BoxChild)(this.hbox2[this.vbox6]));
            w58.Position = 1;
            this.vbox1.Add(this.hbox2);
            Gtk.Box.BoxChild w59 = ((Gtk.Box.BoxChild)(this.vbox1[this.hbox2]));
            w59.PackType = ((Gtk.PackType)(1));
            w59.Position = 4;
            w59.Expand = false;
            w59.Fill = false;
            this.hbox1.Add(this.vbox1);
            Gtk.Box.BoxChild w60 = ((Gtk.Box.BoxChild)(this.hbox1[this.vbox1]));
            w60.Position = 1;
            w60.Expand = false;
            w60.Fill = false;
            this.Add(this.hbox1);
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.Show();
            this.entry_search.Changed += new System.EventHandler(this.OnEntrySearchChanged);
            this.button_expandall.Clicked += new System.EventHandler(this.OnButtonExpandallClicked);
            this.button_collapse_all.Clicked += new System.EventHandler(this.OnButtonCollapseAllClicked);
            this.treeview_inv.CursorChanged += new System.EventHandler(this.OnTreeviewInvCursorChanged);
            this.check_special_folders.Clicked += new System.EventHandler(this.OnCheckSpecialFoldersClicked);
            this.radiobutton1.Clicked += new System.EventHandler(this.OnRadiobutton1Clicked);
        }
    }
}
