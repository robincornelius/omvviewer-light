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
    
    
    public partial class ParcelMgr {
        
        private Gtk.VBox vbox1;
        
        private Gtk.HBox hbox1;
        
        private Gtk.ScrolledWindow GtkScrolledWindow;
        
        private Gtk.TreeView treeview_parcels;
        
        private Gtk.Notebook notebook1;
        
        private Gtk.HBox hbox2;
        
        private Gtk.VBox vbox8;
        
        private Gtk.Label label5;
        
        private Gtk.Label label11;
        
        private Gtk.Label label14;
        
        private Gtk.Label lable3;
        
        private Gtk.Label label15;
        
        private Gtk.VBox vbox9;
        
        private Gtk.Label label_parcelowner;
        
        private Gtk.Label label_parcelgroup;
        
        private Gtk.Label label_forsale;
        
        private Gtk.Label lable_forsaleto;
        
        private Gtk.Label label_price1;
        
        private Gtk.VBox vbox10;
        
        private Gtk.Label label12;
        
        private Gtk.Image image9;
        
        private Gtk.VBox vbox11;
        
        private Gtk.Label label13;
        
        private Gtk.Image image_parcelsnap;
        
        private Gtk.Label label1;
        
        private Gtk.HBox hbox5;
        
        private Gtk.ScrolledWindow GtkScrolledWindow1;
        
        private Gtk.TreeView treeview_access;
        
        private Gtk.ScrolledWindow GtkScrolledWindow2;
        
        private Gtk.TreeView treeview_ban;
        
        private Gtk.VBox vbox2;
        
        private Gtk.CheckButton checkbutton_publicaccess;
        
        private Gtk.CheckButton checkbox_nopayment;
        
        private Gtk.CheckButton checkbutton_noageverify;
        
        private Gtk.CheckButton checkbutton_groupaccess;
        
        private Gtk.HBox hbox3;
        
        private Gtk.VBox vbox3;
        
        private Gtk.CheckButton checkbutton_sellpasses;
        
        private Gtk.Label label_price;
        
        private Gtk.Label label3;
        
        private Gtk.VBox vbox4;
        
        private Gtk.ComboBox combobox_passes;
        
        private Gtk.Entry entry_price;
        
        private Gtk.Entry entry_time;
        
        private Gtk.Label label2;
        
        private Gtk.HBox hbox4;
        
        private Gtk.VBox vbox5;
        
        private Gtk.ScrolledWindow GtkScrolledWindow3;
        
        private Gtk.TreeView treeview_objectlist;
        
        private Gtk.HBox hbox6;
        
        private Gtk.Button button1;
        
        private Gtk.Button button_return_selected;
        
        private Gtk.VBox vbox6;
        
        private Gtk.Label label_totalobejcts;
        
        private Gtk.Label label6;
        
        private Gtk.Label label7;
        
        private Gtk.Label label8;
        
        private Gtk.Label label9;
        
        private Gtk.Label label10;
        
        private Gtk.VBox vbox7;
        
        private Gtk.Entry entry_totalprims;
        
        private Gtk.Entry entry_maxprims;
        
        private Gtk.Entry entry_bonus;
        
        private Gtk.Entry entry_primsowner;
        
        private Gtk.Entry entry_primsgroup;
        
        private Gtk.Entry entry_primsother;
        
        private Gtk.Label label4;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget omvviewerlight.ParcelMgr
            Stetic.BinContainer.Attach(this);
            this.Name = "omvviewerlight.ParcelMgr";
            // Container child omvviewerlight.ParcelMgr.Gtk.Container+ContainerChild
            this.vbox1 = new Gtk.VBox();
            this.vbox1.Name = "vbox1";
            this.vbox1.Spacing = 6;
            // Container child vbox1.Gtk.Box+BoxChild
            this.hbox1 = new Gtk.HBox();
            this.hbox1.Name = "hbox1";
            this.hbox1.Spacing = 6;
            // Container child hbox1.Gtk.Box+BoxChild
            this.GtkScrolledWindow = new Gtk.ScrolledWindow();
            this.GtkScrolledWindow.Name = "GtkScrolledWindow";
            this.GtkScrolledWindow.ShadowType = ((Gtk.ShadowType)(1));
            // Container child GtkScrolledWindow.Gtk.Container+ContainerChild
            this.treeview_parcels = new Gtk.TreeView();
            this.treeview_parcels.HeightRequest = 164;
            this.treeview_parcels.CanFocus = true;
            this.treeview_parcels.Name = "treeview_parcels";
            this.treeview_parcels.HeadersClickable = true;
            this.GtkScrolledWindow.Add(this.treeview_parcels);
            this.hbox1.Add(this.GtkScrolledWindow);
            Gtk.Box.BoxChild w2 = ((Gtk.Box.BoxChild)(this.hbox1[this.GtkScrolledWindow]));
            w2.Position = 0;
            this.vbox1.Add(this.hbox1);
            Gtk.Box.BoxChild w3 = ((Gtk.Box.BoxChild)(this.vbox1[this.hbox1]));
            w3.Position = 0;
            // Container child vbox1.Gtk.Box+BoxChild
            this.notebook1 = new Gtk.Notebook();
            this.notebook1.CanFocus = true;
            this.notebook1.CurrentPage = 0;
            // Container child notebook1.Gtk.Notebook+NotebookChild
            this.hbox2 = new Gtk.HBox();
            this.hbox2.Name = "hbox2";
            this.hbox2.Spacing = 6;
            // Container child hbox2.Gtk.Box+BoxChild
            this.vbox8 = new Gtk.VBox();
            this.vbox8.Name = "vbox8";
            this.vbox8.Spacing = 6;
            // Container child vbox8.Gtk.Box+BoxChild
            this.label5 = new Gtk.Label();
            this.label5.Name = "label5";
            this.label5.Xalign = 0F;
            this.label5.LabelProp = Mono.Unix.Catalog.GetString("Owner");
            this.vbox8.Add(this.label5);
            Gtk.Box.BoxChild w4 = ((Gtk.Box.BoxChild)(this.vbox8[this.label5]));
            w4.Position = 0;
            w4.Expand = false;
            w4.Fill = false;
            // Container child vbox8.Gtk.Box+BoxChild
            this.label11 = new Gtk.Label();
            this.label11.Name = "label11";
            this.label11.Xalign = 0F;
            this.label11.LabelProp = Mono.Unix.Catalog.GetString("Group");
            this.vbox8.Add(this.label11);
            Gtk.Box.BoxChild w5 = ((Gtk.Box.BoxChild)(this.vbox8[this.label11]));
            w5.Position = 1;
            w5.Expand = false;
            w5.Fill = false;
            // Container child vbox8.Gtk.Box+BoxChild
            this.label14 = new Gtk.Label();
            this.label14.Name = "label14";
            this.label14.Xalign = 0F;
            this.label14.LabelProp = Mono.Unix.Catalog.GetString("For sale");
            this.vbox8.Add(this.label14);
            Gtk.Box.BoxChild w6 = ((Gtk.Box.BoxChild)(this.vbox8[this.label14]));
            w6.Position = 2;
            w6.Expand = false;
            w6.Fill = false;
            // Container child vbox8.Gtk.Box+BoxChild
            this.lable3 = new Gtk.Label();
            this.lable3.Name = "lable3";
            this.lable3.Xalign = 0F;
            this.lable3.LabelProp = Mono.Unix.Catalog.GetString("To");
            this.vbox8.Add(this.lable3);
            Gtk.Box.BoxChild w7 = ((Gtk.Box.BoxChild)(this.vbox8[this.lable3]));
            w7.Position = 3;
            w7.Expand = false;
            w7.Fill = false;
            // Container child vbox8.Gtk.Box+BoxChild
            this.label15 = new Gtk.Label();
            this.label15.Name = "label15";
            this.label15.Xalign = 0F;
            this.label15.LabelProp = Mono.Unix.Catalog.GetString("Price");
            this.vbox8.Add(this.label15);
            Gtk.Box.BoxChild w8 = ((Gtk.Box.BoxChild)(this.vbox8[this.label15]));
            w8.Position = 4;
            w8.Expand = false;
            w8.Fill = false;
            this.hbox2.Add(this.vbox8);
            Gtk.Box.BoxChild w9 = ((Gtk.Box.BoxChild)(this.hbox2[this.vbox8]));
            w9.Position = 0;
            w9.Expand = false;
            w9.Fill = false;
            // Container child hbox2.Gtk.Box+BoxChild
            this.vbox9 = new Gtk.VBox();
            this.vbox9.Name = "vbox9";
            this.vbox9.Spacing = 6;
            // Container child vbox9.Gtk.Box+BoxChild
            this.label_parcelowner = new Gtk.Label();
            this.label_parcelowner.WidthRequest = 200;
            this.label_parcelowner.Name = "label_parcelowner";
            this.label_parcelowner.Xalign = 0F;
            this.label_parcelowner.LabelProp = Mono.Unix.Catalog.GetString("label4");
            this.vbox9.Add(this.label_parcelowner);
            Gtk.Box.BoxChild w10 = ((Gtk.Box.BoxChild)(this.vbox9[this.label_parcelowner]));
            w10.Position = 0;
            w10.Expand = false;
            w10.Fill = false;
            // Container child vbox9.Gtk.Box+BoxChild
            this.label_parcelgroup = new Gtk.Label();
            this.label_parcelgroup.Name = "label_parcelgroup";
            this.label_parcelgroup.Xalign = 0F;
            this.label_parcelgroup.LabelProp = "";
            this.vbox9.Add(this.label_parcelgroup);
            Gtk.Box.BoxChild w11 = ((Gtk.Box.BoxChild)(this.vbox9[this.label_parcelgroup]));
            w11.Position = 1;
            w11.Expand = false;
            w11.Fill = false;
            // Container child vbox9.Gtk.Box+BoxChild
            this.label_forsale = new Gtk.Label();
            this.label_forsale.Name = "label_forsale";
            this.label_forsale.Xalign = 0F;
            this.label_forsale.LabelProp = Mono.Unix.Catalog.GetString("label2");
            this.vbox9.Add(this.label_forsale);
            Gtk.Box.BoxChild w12 = ((Gtk.Box.BoxChild)(this.vbox9[this.label_forsale]));
            w12.Position = 2;
            w12.Expand = false;
            w12.Fill = false;
            // Container child vbox9.Gtk.Box+BoxChild
            this.lable_forsaleto = new Gtk.Label();
            this.lable_forsaleto.Name = "lable_forsaleto";
            this.lable_forsaleto.Xalign = 0F;
            this.lable_forsaleto.LabelProp = Mono.Unix.Catalog.GetString("label4");
            this.vbox9.Add(this.lable_forsaleto);
            Gtk.Box.BoxChild w13 = ((Gtk.Box.BoxChild)(this.vbox9[this.lable_forsaleto]));
            w13.Position = 3;
            w13.Expand = false;
            w13.Fill = false;
            // Container child vbox9.Gtk.Box+BoxChild
            this.label_price1 = new Gtk.Label();
            this.label_price1.Name = "label_price1";
            this.label_price1.Xalign = 0F;
            this.label_price1.LabelProp = Mono.Unix.Catalog.GetString("label6");
            this.vbox9.Add(this.label_price1);
            Gtk.Box.BoxChild w14 = ((Gtk.Box.BoxChild)(this.vbox9[this.label_price1]));
            w14.Position = 4;
            w14.Expand = false;
            w14.Fill = false;
            this.hbox2.Add(this.vbox9);
            Gtk.Box.BoxChild w15 = ((Gtk.Box.BoxChild)(this.hbox2[this.vbox9]));
            w15.Position = 1;
            w15.Expand = false;
            w15.Fill = false;
            // Container child hbox2.Gtk.Box+BoxChild
            this.vbox10 = new Gtk.VBox();
            this.vbox10.Name = "vbox10";
            this.vbox10.Spacing = 6;
            // Container child vbox10.Gtk.Box+BoxChild
            this.label12 = new Gtk.Label();
            this.label12.Name = "label12";
            this.label12.LabelProp = Mono.Unix.Catalog.GetString("Parcel map");
            this.vbox10.Add(this.label12);
            Gtk.Box.BoxChild w16 = ((Gtk.Box.BoxChild)(this.vbox10[this.label12]));
            w16.Position = 0;
            w16.Expand = false;
            w16.Fill = false;
            // Container child vbox10.Gtk.Box+BoxChild
            this.image9 = new Gtk.Image();
            this.image9.WidthRequest = 256;
            this.image9.HeightRequest = 256;
            this.image9.Name = "image9";
            this.vbox10.Add(this.image9);
            Gtk.Box.BoxChild w17 = ((Gtk.Box.BoxChild)(this.vbox10[this.image9]));
            w17.Position = 1;
            w17.Expand = false;
            w17.Fill = false;
            this.hbox2.Add(this.vbox10);
            Gtk.Box.BoxChild w18 = ((Gtk.Box.BoxChild)(this.hbox2[this.vbox10]));
            w18.Position = 2;
            w18.Expand = false;
            w18.Fill = false;
            // Container child hbox2.Gtk.Box+BoxChild
            this.vbox11 = new Gtk.VBox();
            this.vbox11.Name = "vbox11";
            this.vbox11.Spacing = 6;
            // Container child vbox11.Gtk.Box+BoxChild
            this.label13 = new Gtk.Label();
            this.label13.Name = "label13";
            this.label13.LabelProp = Mono.Unix.Catalog.GetString("Parcel snapshot");
            this.vbox11.Add(this.label13);
            Gtk.Box.BoxChild w19 = ((Gtk.Box.BoxChild)(this.vbox11[this.label13]));
            w19.Position = 0;
            w19.Expand = false;
            w19.Fill = false;
            // Container child vbox11.Gtk.Box+BoxChild
            this.image_parcelsnap = new Gtk.Image();
            this.image_parcelsnap.WidthRequest = 256;
            this.image_parcelsnap.HeightRequest = 256;
            this.image_parcelsnap.Name = "image_parcelsnap";
            this.vbox11.Add(this.image_parcelsnap);
            Gtk.Box.BoxChild w20 = ((Gtk.Box.BoxChild)(this.vbox11[this.image_parcelsnap]));
            w20.Position = 1;
            w20.Expand = false;
            w20.Fill = false;
            this.hbox2.Add(this.vbox11);
            Gtk.Box.BoxChild w21 = ((Gtk.Box.BoxChild)(this.hbox2[this.vbox11]));
            w21.Position = 3;
            w21.Expand = false;
            w21.Fill = false;
            this.notebook1.Add(this.hbox2);
            // Notebook tab
            this.label1 = new Gtk.Label();
            this.label1.Name = "label1";
            this.label1.LabelProp = Mono.Unix.Catalog.GetString("General");
            this.notebook1.SetTabLabel(this.hbox2, this.label1);
            this.label1.ShowAll();
            // Container child notebook1.Gtk.Notebook+NotebookChild
            this.hbox5 = new Gtk.HBox();
            this.hbox5.Name = "hbox5";
            this.hbox5.Spacing = 6;
            // Container child hbox5.Gtk.Box+BoxChild
            this.GtkScrolledWindow1 = new Gtk.ScrolledWindow();
            this.GtkScrolledWindow1.Name = "GtkScrolledWindow1";
            this.GtkScrolledWindow1.ShadowType = ((Gtk.ShadowType)(1));
            // Container child GtkScrolledWindow1.Gtk.Container+ContainerChild
            this.treeview_access = new Gtk.TreeView();
            this.treeview_access.CanFocus = true;
            this.treeview_access.Name = "treeview_access";
            this.treeview_access.HeadersClickable = true;
            this.GtkScrolledWindow1.Add(this.treeview_access);
            this.hbox5.Add(this.GtkScrolledWindow1);
            Gtk.Box.BoxChild w24 = ((Gtk.Box.BoxChild)(this.hbox5[this.GtkScrolledWindow1]));
            w24.Position = 0;
            // Container child hbox5.Gtk.Box+BoxChild
            this.GtkScrolledWindow2 = new Gtk.ScrolledWindow();
            this.GtkScrolledWindow2.Name = "GtkScrolledWindow2";
            this.GtkScrolledWindow2.ShadowType = ((Gtk.ShadowType)(1));
            // Container child GtkScrolledWindow2.Gtk.Container+ContainerChild
            this.treeview_ban = new Gtk.TreeView();
            this.treeview_ban.CanFocus = true;
            this.treeview_ban.Name = "treeview_ban";
            this.treeview_ban.HeadersClickable = true;
            this.GtkScrolledWindow2.Add(this.treeview_ban);
            this.hbox5.Add(this.GtkScrolledWindow2);
            Gtk.Box.BoxChild w26 = ((Gtk.Box.BoxChild)(this.hbox5[this.GtkScrolledWindow2]));
            w26.Position = 1;
            // Container child hbox5.Gtk.Box+BoxChild
            this.vbox2 = new Gtk.VBox();
            this.vbox2.Name = "vbox2";
            this.vbox2.Spacing = 6;
            // Container child vbox2.Gtk.Box+BoxChild
            this.checkbutton_publicaccess = new Gtk.CheckButton();
            this.checkbutton_publicaccess.CanFocus = true;
            this.checkbutton_publicaccess.Name = "checkbutton_publicaccess";
            this.checkbutton_publicaccess.Label = Mono.Unix.Catalog.GetString("Allow public access");
            this.checkbutton_publicaccess.DrawIndicator = true;
            this.checkbutton_publicaccess.UseUnderline = true;
            this.vbox2.Add(this.checkbutton_publicaccess);
            Gtk.Box.BoxChild w27 = ((Gtk.Box.BoxChild)(this.vbox2[this.checkbutton_publicaccess]));
            w27.Position = 0;
            w27.Expand = false;
            w27.Fill = false;
            // Container child vbox2.Gtk.Box+BoxChild
            this.checkbox_nopayment = new Gtk.CheckButton();
            this.checkbox_nopayment.CanFocus = true;
            this.checkbox_nopayment.Name = "checkbox_nopayment";
            this.checkbox_nopayment.Label = Mono.Unix.Catalog.GetString("Block no payment info");
            this.checkbox_nopayment.DrawIndicator = true;
            this.checkbox_nopayment.UseUnderline = true;
            this.vbox2.Add(this.checkbox_nopayment);
            Gtk.Box.BoxChild w28 = ((Gtk.Box.BoxChild)(this.vbox2[this.checkbox_nopayment]));
            w28.Position = 1;
            w28.Expand = false;
            w28.Fill = false;
            // Container child vbox2.Gtk.Box+BoxChild
            this.checkbutton_noageverify = new Gtk.CheckButton();
            this.checkbutton_noageverify.CanFocus = true;
            this.checkbutton_noageverify.Name = "checkbutton_noageverify";
            this.checkbutton_noageverify.Label = Mono.Unix.Catalog.GetString("Block not age verified");
            this.checkbutton_noageverify.DrawIndicator = true;
            this.checkbutton_noageverify.UseUnderline = true;
            this.vbox2.Add(this.checkbutton_noageverify);
            Gtk.Box.BoxChild w29 = ((Gtk.Box.BoxChild)(this.vbox2[this.checkbutton_noageverify]));
            w29.Position = 2;
            w29.Expand = false;
            w29.Fill = false;
            // Container child vbox2.Gtk.Box+BoxChild
            this.checkbutton_groupaccess = new Gtk.CheckButton();
            this.checkbutton_groupaccess.CanFocus = true;
            this.checkbutton_groupaccess.Name = "checkbutton_groupaccess";
            this.checkbutton_groupaccess.Label = Mono.Unix.Catalog.GetString("Allow group access");
            this.checkbutton_groupaccess.DrawIndicator = true;
            this.checkbutton_groupaccess.UseUnderline = true;
            this.vbox2.Add(this.checkbutton_groupaccess);
            Gtk.Box.BoxChild w30 = ((Gtk.Box.BoxChild)(this.vbox2[this.checkbutton_groupaccess]));
            w30.Position = 3;
            w30.Expand = false;
            w30.Fill = false;
            // Container child vbox2.Gtk.Box+BoxChild
            this.hbox3 = new Gtk.HBox();
            this.hbox3.Name = "hbox3";
            this.hbox3.Spacing = 6;
            // Container child hbox3.Gtk.Box+BoxChild
            this.vbox3 = new Gtk.VBox();
            this.vbox3.Name = "vbox3";
            this.vbox3.Homogeneous = true;
            this.vbox3.Spacing = 6;
            // Container child vbox3.Gtk.Box+BoxChild
            this.checkbutton_sellpasses = new Gtk.CheckButton();
            this.checkbutton_sellpasses.CanFocus = true;
            this.checkbutton_sellpasses.Name = "checkbutton_sellpasses";
            this.checkbutton_sellpasses.Label = Mono.Unix.Catalog.GetString("Sell passes");
            this.checkbutton_sellpasses.DrawIndicator = true;
            this.checkbutton_sellpasses.UseUnderline = true;
            this.vbox3.Add(this.checkbutton_sellpasses);
            Gtk.Box.BoxChild w31 = ((Gtk.Box.BoxChild)(this.vbox3[this.checkbutton_sellpasses]));
            w31.Position = 0;
            w31.Expand = false;
            w31.Fill = false;
            // Container child vbox3.Gtk.Box+BoxChild
            this.label_price = new Gtk.Label();
            this.label_price.Name = "label_price";
            this.label_price.Xalign = 0F;
            this.label_price.LabelProp = Mono.Unix.Catalog.GetString("Pass price $L");
            this.vbox3.Add(this.label_price);
            Gtk.Box.BoxChild w32 = ((Gtk.Box.BoxChild)(this.vbox3[this.label_price]));
            w32.Position = 1;
            w32.Expand = false;
            w32.Fill = false;
            // Container child vbox3.Gtk.Box+BoxChild
            this.label3 = new Gtk.Label();
            this.label3.Name = "label3";
            this.label3.Xalign = 0.01F;
            this.label3.LabelProp = Mono.Unix.Catalog.GetString("Pass length (time)");
            this.vbox3.Add(this.label3);
            Gtk.Box.BoxChild w33 = ((Gtk.Box.BoxChild)(this.vbox3[this.label3]));
            w33.Position = 2;
            w33.Expand = false;
            w33.Fill = false;
            this.hbox3.Add(this.vbox3);
            Gtk.Box.BoxChild w34 = ((Gtk.Box.BoxChild)(this.hbox3[this.vbox3]));
            w34.Position = 0;
            w34.Expand = false;
            w34.Fill = false;
            // Container child hbox3.Gtk.Box+BoxChild
            this.vbox4 = new Gtk.VBox();
            this.vbox4.Name = "vbox4";
            this.vbox4.Homogeneous = true;
            this.vbox4.Spacing = 6;
            // Container child vbox4.Gtk.Box+BoxChild
            this.combobox_passes = Gtk.ComboBox.NewText();
            this.combobox_passes.AppendText(Mono.Unix.Catalog.GetString("Anyone"));
            this.combobox_passes.Name = "combobox_passes";
            this.combobox_passes.Active = 0;
            this.vbox4.Add(this.combobox_passes);
            Gtk.Box.BoxChild w35 = ((Gtk.Box.BoxChild)(this.vbox4[this.combobox_passes]));
            w35.Position = 0;
            w35.Expand = false;
            w35.Fill = false;
            // Container child vbox4.Gtk.Box+BoxChild
            this.entry_price = new Gtk.Entry();
            this.entry_price.CanFocus = true;
            this.entry_price.Name = "entry_price";
            this.entry_price.IsEditable = true;
            this.entry_price.WidthChars = 4;
            this.entry_price.InvisibleChar = '●';
            this.vbox4.Add(this.entry_price);
            Gtk.Box.BoxChild w36 = ((Gtk.Box.BoxChild)(this.vbox4[this.entry_price]));
            w36.Position = 1;
            w36.Expand = false;
            w36.Fill = false;
            // Container child vbox4.Gtk.Box+BoxChild
            this.entry_time = new Gtk.Entry();
            this.entry_time.CanFocus = true;
            this.entry_time.Name = "entry_time";
            this.entry_time.IsEditable = true;
            this.entry_time.WidthChars = 4;
            this.entry_time.InvisibleChar = '●';
            this.vbox4.Add(this.entry_time);
            Gtk.Box.BoxChild w37 = ((Gtk.Box.BoxChild)(this.vbox4[this.entry_time]));
            w37.Position = 2;
            w37.Expand = false;
            w37.Fill = false;
            this.hbox3.Add(this.vbox4);
            Gtk.Box.BoxChild w38 = ((Gtk.Box.BoxChild)(this.hbox3[this.vbox4]));
            w38.Position = 1;
            w38.Expand = false;
            w38.Fill = false;
            this.vbox2.Add(this.hbox3);
            Gtk.Box.BoxChild w39 = ((Gtk.Box.BoxChild)(this.vbox2[this.hbox3]));
            w39.Position = 4;
            w39.Expand = false;
            w39.Fill = false;
            this.hbox5.Add(this.vbox2);
            Gtk.Box.BoxChild w40 = ((Gtk.Box.BoxChild)(this.hbox5[this.vbox2]));
            w40.Position = 2;
            w40.Expand = false;
            w40.Fill = false;
            this.notebook1.Add(this.hbox5);
            Gtk.Notebook.NotebookChild w41 = ((Gtk.Notebook.NotebookChild)(this.notebook1[this.hbox5]));
            w41.Position = 1;
            // Notebook tab
            this.label2 = new Gtk.Label();
            this.label2.Name = "label2";
            this.label2.LabelProp = Mono.Unix.Catalog.GetString("Access");
            this.notebook1.SetTabLabel(this.hbox5, this.label2);
            this.label2.ShowAll();
            // Container child notebook1.Gtk.Notebook+NotebookChild
            this.hbox4 = new Gtk.HBox();
            this.hbox4.Name = "hbox4";
            this.hbox4.Spacing = 6;
            // Container child hbox4.Gtk.Box+BoxChild
            this.vbox5 = new Gtk.VBox();
            this.vbox5.Name = "vbox5";
            this.vbox5.Spacing = 6;
            // Container child vbox5.Gtk.Box+BoxChild
            this.GtkScrolledWindow3 = new Gtk.ScrolledWindow();
            this.GtkScrolledWindow3.Name = "GtkScrolledWindow3";
            this.GtkScrolledWindow3.ShadowType = ((Gtk.ShadowType)(1));
            // Container child GtkScrolledWindow3.Gtk.Container+ContainerChild
            this.treeview_objectlist = new Gtk.TreeView();
            this.treeview_objectlist.WidthRequest = 540;
            this.treeview_objectlist.CanFocus = true;
            this.treeview_objectlist.Name = "treeview_objectlist";
            this.treeview_objectlist.HeadersClickable = true;
            this.GtkScrolledWindow3.Add(this.treeview_objectlist);
            this.vbox5.Add(this.GtkScrolledWindow3);
            Gtk.Box.BoxChild w43 = ((Gtk.Box.BoxChild)(this.vbox5[this.GtkScrolledWindow3]));
            w43.Position = 0;
            // Container child vbox5.Gtk.Box+BoxChild
            this.hbox6 = new Gtk.HBox();
            this.hbox6.Name = "hbox6";
            this.hbox6.Spacing = 6;
            // Container child hbox6.Gtk.Box+BoxChild
            this.button1 = new Gtk.Button();
            this.button1.CanFocus = true;
            this.button1.Name = "button1";
            this.button1.UseUnderline = true;
            this.button1.Label = Mono.Unix.Catalog.GetString("Fetch object list");
            this.hbox6.Add(this.button1);
            Gtk.Box.BoxChild w44 = ((Gtk.Box.BoxChild)(this.hbox6[this.button1]));
            w44.Position = 0;
            w44.Expand = false;
            w44.Fill = false;
            // Container child hbox6.Gtk.Box+BoxChild
            this.button_return_selected = new Gtk.Button();
            this.button_return_selected.Sensitive = false;
            this.button_return_selected.CanFocus = true;
            this.button_return_selected.Name = "button_return_selected";
            this.button_return_selected.UseUnderline = true;
            this.button_return_selected.Label = Mono.Unix.Catalog.GetString("Return selected");
            this.hbox6.Add(this.button_return_selected);
            Gtk.Box.BoxChild w45 = ((Gtk.Box.BoxChild)(this.hbox6[this.button_return_selected]));
            w45.Position = 1;
            w45.Expand = false;
            w45.Fill = false;
            this.vbox5.Add(this.hbox6);
            Gtk.Box.BoxChild w46 = ((Gtk.Box.BoxChild)(this.vbox5[this.hbox6]));
            w46.Position = 1;
            w46.Expand = false;
            w46.Fill = false;
            this.hbox4.Add(this.vbox5);
            Gtk.Box.BoxChild w47 = ((Gtk.Box.BoxChild)(this.hbox4[this.vbox5]));
            w47.Position = 0;
            // Container child hbox4.Gtk.Box+BoxChild
            this.vbox6 = new Gtk.VBox();
            this.vbox6.Name = "vbox6";
            this.vbox6.Homogeneous = true;
            this.vbox6.Spacing = 6;
            // Container child vbox6.Gtk.Box+BoxChild
            this.label_totalobejcts = new Gtk.Label();
            this.label_totalobejcts.Name = "label_totalobejcts";
            this.label_totalobejcts.Yalign = 1F;
            this.label_totalobejcts.LabelProp = Mono.Unix.Catalog.GetString("Total objects on parcel");
            this.vbox6.Add(this.label_totalobejcts);
            Gtk.Box.BoxChild w48 = ((Gtk.Box.BoxChild)(this.vbox6[this.label_totalobejcts]));
            w48.Position = 0;
            w48.Expand = false;
            w48.Fill = false;
            // Container child vbox6.Gtk.Box+BoxChild
            this.label6 = new Gtk.Label();
            this.label6.Name = "label6";
            this.label6.Xalign = 0F;
            this.label6.Yalign = 1F;
            this.label6.LabelProp = Mono.Unix.Catalog.GetString("Max objects");
            this.vbox6.Add(this.label6);
            Gtk.Box.BoxChild w49 = ((Gtk.Box.BoxChild)(this.vbox6[this.label6]));
            w49.Position = 1;
            w49.Expand = false;
            w49.Fill = false;
            // Container child vbox6.Gtk.Box+BoxChild
            this.label7 = new Gtk.Label();
            this.label7.Name = "label7";
            this.label7.Xalign = 0F;
            this.label7.LabelProp = Mono.Unix.Catalog.GetString("Region bonus factor");
            this.vbox6.Add(this.label7);
            Gtk.Box.BoxChild w50 = ((Gtk.Box.BoxChild)(this.vbox6[this.label7]));
            w50.Position = 2;
            w50.Expand = false;
            w50.Fill = false;
            // Container child vbox6.Gtk.Box+BoxChild
            this.label8 = new Gtk.Label();
            this.label8.Name = "label8";
            this.label8.Xalign = 0F;
            this.label8.LabelProp = Mono.Unix.Catalog.GetString("Owned by parcel owner");
            this.vbox6.Add(this.label8);
            Gtk.Box.BoxChild w51 = ((Gtk.Box.BoxChild)(this.vbox6[this.label8]));
            w51.Position = 3;
            w51.Expand = false;
            w51.Fill = false;
            // Container child vbox6.Gtk.Box+BoxChild
            this.label9 = new Gtk.Label();
            this.label9.Name = "label9";
            this.label9.Xalign = 0F;
            this.label9.LabelProp = Mono.Unix.Catalog.GetString("Set To group");
            this.vbox6.Add(this.label9);
            Gtk.Box.BoxChild w52 = ((Gtk.Box.BoxChild)(this.vbox6[this.label9]));
            w52.Position = 4;
            w52.Expand = false;
            w52.Fill = false;
            // Container child vbox6.Gtk.Box+BoxChild
            this.label10 = new Gtk.Label();
            this.label10.Name = "label10";
            this.label10.Xalign = 0F;
            this.label10.LabelProp = Mono.Unix.Catalog.GetString("Others");
            this.vbox6.Add(this.label10);
            Gtk.Box.BoxChild w53 = ((Gtk.Box.BoxChild)(this.vbox6[this.label10]));
            w53.Position = 5;
            w53.Expand = false;
            w53.Fill = false;
            this.hbox4.Add(this.vbox6);
            Gtk.Box.BoxChild w54 = ((Gtk.Box.BoxChild)(this.hbox4[this.vbox6]));
            w54.Position = 1;
            w54.Expand = false;
            w54.Fill = false;
            // Container child hbox4.Gtk.Box+BoxChild
            this.vbox7 = new Gtk.VBox();
            this.vbox7.Name = "vbox7";
            this.vbox7.Homogeneous = true;
            this.vbox7.Spacing = 6;
            // Container child vbox7.Gtk.Box+BoxChild
            this.entry_totalprims = new Gtk.Entry();
            this.entry_totalprims.CanFocus = true;
            this.entry_totalprims.Name = "entry_totalprims";
            this.entry_totalprims.IsEditable = false;
            this.entry_totalprims.WidthChars = 5;
            this.entry_totalprims.MaxLength = 5;
            this.entry_totalprims.InvisibleChar = '●';
            this.vbox7.Add(this.entry_totalprims);
            Gtk.Box.BoxChild w55 = ((Gtk.Box.BoxChild)(this.vbox7[this.entry_totalprims]));
            w55.Position = 0;
            w55.Expand = false;
            w55.Fill = false;
            // Container child vbox7.Gtk.Box+BoxChild
            this.entry_maxprims = new Gtk.Entry();
            this.entry_maxprims.CanFocus = true;
            this.entry_maxprims.Name = "entry_maxprims";
            this.entry_maxprims.IsEditable = false;
            this.entry_maxprims.WidthChars = 5;
            this.entry_maxprims.MaxLength = 5;
            this.entry_maxprims.InvisibleChar = '●';
            this.vbox7.Add(this.entry_maxprims);
            Gtk.Box.BoxChild w56 = ((Gtk.Box.BoxChild)(this.vbox7[this.entry_maxprims]));
            w56.Position = 1;
            w56.Expand = false;
            w56.Fill = false;
            // Container child vbox7.Gtk.Box+BoxChild
            this.entry_bonus = new Gtk.Entry();
            this.entry_bonus.CanFocus = true;
            this.entry_bonus.Name = "entry_bonus";
            this.entry_bonus.IsEditable = false;
            this.entry_bonus.WidthChars = 5;
            this.entry_bonus.MaxLength = 5;
            this.entry_bonus.InvisibleChar = '●';
            this.vbox7.Add(this.entry_bonus);
            Gtk.Box.BoxChild w57 = ((Gtk.Box.BoxChild)(this.vbox7[this.entry_bonus]));
            w57.Position = 2;
            w57.Expand = false;
            w57.Fill = false;
            // Container child vbox7.Gtk.Box+BoxChild
            this.entry_primsowner = new Gtk.Entry();
            this.entry_primsowner.CanFocus = true;
            this.entry_primsowner.Name = "entry_primsowner";
            this.entry_primsowner.IsEditable = false;
            this.entry_primsowner.WidthChars = 5;
            this.entry_primsowner.InvisibleChar = '●';
            this.vbox7.Add(this.entry_primsowner);
            Gtk.Box.BoxChild w58 = ((Gtk.Box.BoxChild)(this.vbox7[this.entry_primsowner]));
            w58.Position = 3;
            w58.Expand = false;
            w58.Fill = false;
            // Container child vbox7.Gtk.Box+BoxChild
            this.entry_primsgroup = new Gtk.Entry();
            this.entry_primsgroup.CanFocus = true;
            this.entry_primsgroup.Name = "entry_primsgroup";
            this.entry_primsgroup.IsEditable = false;
            this.entry_primsgroup.WidthChars = 5;
            this.entry_primsgroup.InvisibleChar = '●';
            this.vbox7.Add(this.entry_primsgroup);
            Gtk.Box.BoxChild w59 = ((Gtk.Box.BoxChild)(this.vbox7[this.entry_primsgroup]));
            w59.Position = 4;
            w59.Expand = false;
            w59.Fill = false;
            // Container child vbox7.Gtk.Box+BoxChild
            this.entry_primsother = new Gtk.Entry();
            this.entry_primsother.CanFocus = true;
            this.entry_primsother.Name = "entry_primsother";
            this.entry_primsother.IsEditable = false;
            this.entry_primsother.WidthChars = 5;
            this.entry_primsother.InvisibleChar = '●';
            this.vbox7.Add(this.entry_primsother);
            Gtk.Box.BoxChild w60 = ((Gtk.Box.BoxChild)(this.vbox7[this.entry_primsother]));
            w60.Position = 5;
            w60.Expand = false;
            w60.Fill = false;
            this.hbox4.Add(this.vbox7);
            Gtk.Box.BoxChild w61 = ((Gtk.Box.BoxChild)(this.hbox4[this.vbox7]));
            w61.Position = 2;
            this.notebook1.Add(this.hbox4);
            Gtk.Notebook.NotebookChild w62 = ((Gtk.Notebook.NotebookChild)(this.notebook1[this.hbox4]));
            w62.Position = 2;
            // Notebook tab
            this.label4 = new Gtk.Label();
            this.label4.Name = "label4";
            this.label4.LabelProp = Mono.Unix.Catalog.GetString("Objects");
            this.notebook1.SetTabLabel(this.hbox4, this.label4);
            this.label4.ShowAll();
            this.vbox1.Add(this.notebook1);
            Gtk.Box.BoxChild w63 = ((Gtk.Box.BoxChild)(this.vbox1[this.notebook1]));
            w63.Position = 1;
            this.Add(this.vbox1);
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.Show();
            this.treeview_parcels.CursorChanged += new System.EventHandler(this.OnTreeviewParcelsCursorChanged);
            this.button1.Activated += new System.EventHandler(this.OnButton1Activated);
            this.button1.Clicked += new System.EventHandler(this.OnButton1Clicked);
            this.button_return_selected.Clicked += new System.EventHandler(this.OnButtonReturnSelectedClicked);
        }
    }
}
