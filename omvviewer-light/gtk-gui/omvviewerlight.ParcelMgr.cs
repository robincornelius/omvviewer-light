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
        
        private Gtk.VBox vbox9;
        
        private Gtk.Label label_parcelowner;
        
        private Gtk.Label label_parcelgroup;
        
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
        
        private Gtk.Button button1;
        
        private Gtk.ScrolledWindow GtkScrolledWindow3;
        
        private Gtk.TreeView treeview_objectlist;
        
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
            this.notebook1.Name = "notebook1";
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
            this.hbox2.Add(this.vbox8);
            Gtk.Box.BoxChild w6 = ((Gtk.Box.BoxChild)(this.hbox2[this.vbox8]));
            w6.Position = 0;
            w6.Expand = false;
            w6.Fill = false;
            // Container child hbox2.Gtk.Box+BoxChild
            this.vbox9 = new Gtk.VBox();
            this.vbox9.Name = "vbox9";
            this.vbox9.Spacing = 6;
            // Container child vbox9.Gtk.Box+BoxChild
            this.label_parcelowner = new Gtk.Label();
            this.label_parcelowner.Name = "label_parcelowner";
            this.label_parcelowner.LabelProp = Mono.Unix.Catalog.GetString("label4");
            this.vbox9.Add(this.label_parcelowner);
            Gtk.Box.BoxChild w7 = ((Gtk.Box.BoxChild)(this.vbox9[this.label_parcelowner]));
            w7.Position = 0;
            w7.Expand = false;
            w7.Fill = false;
            // Container child vbox9.Gtk.Box+BoxChild
            this.label_parcelgroup = new Gtk.Label();
            this.label_parcelgroup.Name = "label_parcelgroup";
            this.label_parcelgroup.LabelProp = Mono.Unix.Catalog.GetString("label5");
            this.vbox9.Add(this.label_parcelgroup);
            Gtk.Box.BoxChild w8 = ((Gtk.Box.BoxChild)(this.vbox9[this.label_parcelgroup]));
            w8.Position = 1;
            w8.Expand = false;
            w8.Fill = false;
            this.hbox2.Add(this.vbox9);
            Gtk.Box.BoxChild w9 = ((Gtk.Box.BoxChild)(this.hbox2[this.vbox9]));
            w9.Position = 1;
            w9.Expand = false;
            w9.Fill = false;
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
            Gtk.Box.BoxChild w12 = ((Gtk.Box.BoxChild)(this.hbox5[this.GtkScrolledWindow1]));
            w12.Position = 0;
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
            Gtk.Box.BoxChild w14 = ((Gtk.Box.BoxChild)(this.hbox5[this.GtkScrolledWindow2]));
            w14.Position = 1;
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
            Gtk.Box.BoxChild w15 = ((Gtk.Box.BoxChild)(this.vbox2[this.checkbutton_publicaccess]));
            w15.Position = 0;
            w15.Expand = false;
            w15.Fill = false;
            // Container child vbox2.Gtk.Box+BoxChild
            this.checkbox_nopayment = new Gtk.CheckButton();
            this.checkbox_nopayment.CanFocus = true;
            this.checkbox_nopayment.Name = "checkbox_nopayment";
            this.checkbox_nopayment.Label = Mono.Unix.Catalog.GetString("Block no payment info");
            this.checkbox_nopayment.DrawIndicator = true;
            this.checkbox_nopayment.UseUnderline = true;
            this.vbox2.Add(this.checkbox_nopayment);
            Gtk.Box.BoxChild w16 = ((Gtk.Box.BoxChild)(this.vbox2[this.checkbox_nopayment]));
            w16.Position = 1;
            w16.Expand = false;
            w16.Fill = false;
            // Container child vbox2.Gtk.Box+BoxChild
            this.checkbutton_noageverify = new Gtk.CheckButton();
            this.checkbutton_noageverify.CanFocus = true;
            this.checkbutton_noageverify.Name = "checkbutton_noageverify";
            this.checkbutton_noageverify.Label = Mono.Unix.Catalog.GetString("Block not age verified");
            this.checkbutton_noageverify.DrawIndicator = true;
            this.checkbutton_noageverify.UseUnderline = true;
            this.vbox2.Add(this.checkbutton_noageverify);
            Gtk.Box.BoxChild w17 = ((Gtk.Box.BoxChild)(this.vbox2[this.checkbutton_noageverify]));
            w17.Position = 2;
            w17.Expand = false;
            w17.Fill = false;
            // Container child vbox2.Gtk.Box+BoxChild
            this.checkbutton_groupaccess = new Gtk.CheckButton();
            this.checkbutton_groupaccess.CanFocus = true;
            this.checkbutton_groupaccess.Name = "checkbutton_groupaccess";
            this.checkbutton_groupaccess.Label = Mono.Unix.Catalog.GetString("Allow group access");
            this.checkbutton_groupaccess.DrawIndicator = true;
            this.checkbutton_groupaccess.UseUnderline = true;
            this.vbox2.Add(this.checkbutton_groupaccess);
            Gtk.Box.BoxChild w18 = ((Gtk.Box.BoxChild)(this.vbox2[this.checkbutton_groupaccess]));
            w18.Position = 3;
            w18.Expand = false;
            w18.Fill = false;
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
            Gtk.Box.BoxChild w19 = ((Gtk.Box.BoxChild)(this.vbox3[this.checkbutton_sellpasses]));
            w19.Position = 0;
            w19.Expand = false;
            w19.Fill = false;
            // Container child vbox3.Gtk.Box+BoxChild
            this.label_price = new Gtk.Label();
            this.label_price.Name = "label_price";
            this.label_price.Xalign = 0F;
            this.label_price.LabelProp = Mono.Unix.Catalog.GetString("Pass price $L");
            this.vbox3.Add(this.label_price);
            Gtk.Box.BoxChild w20 = ((Gtk.Box.BoxChild)(this.vbox3[this.label_price]));
            w20.Position = 1;
            w20.Expand = false;
            w20.Fill = false;
            // Container child vbox3.Gtk.Box+BoxChild
            this.label3 = new Gtk.Label();
            this.label3.Name = "label3";
            this.label3.Xalign = 0.01F;
            this.label3.LabelProp = Mono.Unix.Catalog.GetString("Pass length (time)");
            this.vbox3.Add(this.label3);
            Gtk.Box.BoxChild w21 = ((Gtk.Box.BoxChild)(this.vbox3[this.label3]));
            w21.Position = 2;
            w21.Expand = false;
            w21.Fill = false;
            this.hbox3.Add(this.vbox3);
            Gtk.Box.BoxChild w22 = ((Gtk.Box.BoxChild)(this.hbox3[this.vbox3]));
            w22.Position = 0;
            w22.Expand = false;
            w22.Fill = false;
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
            Gtk.Box.BoxChild w23 = ((Gtk.Box.BoxChild)(this.vbox4[this.combobox_passes]));
            w23.Position = 0;
            w23.Expand = false;
            w23.Fill = false;
            // Container child vbox4.Gtk.Box+BoxChild
            this.entry_price = new Gtk.Entry();
            this.entry_price.CanFocus = true;
            this.entry_price.Name = "entry_price";
            this.entry_price.IsEditable = true;
            this.entry_price.WidthChars = 4;
            this.entry_price.InvisibleChar = '●';
            this.vbox4.Add(this.entry_price);
            Gtk.Box.BoxChild w24 = ((Gtk.Box.BoxChild)(this.vbox4[this.entry_price]));
            w24.Position = 1;
            w24.Expand = false;
            w24.Fill = false;
            // Container child vbox4.Gtk.Box+BoxChild
            this.entry_time = new Gtk.Entry();
            this.entry_time.CanFocus = true;
            this.entry_time.Name = "entry_time";
            this.entry_time.IsEditable = true;
            this.entry_time.WidthChars = 4;
            this.entry_time.InvisibleChar = '●';
            this.vbox4.Add(this.entry_time);
            Gtk.Box.BoxChild w25 = ((Gtk.Box.BoxChild)(this.vbox4[this.entry_time]));
            w25.Position = 2;
            w25.Expand = false;
            w25.Fill = false;
            this.hbox3.Add(this.vbox4);
            Gtk.Box.BoxChild w26 = ((Gtk.Box.BoxChild)(this.hbox3[this.vbox4]));
            w26.Position = 1;
            w26.Expand = false;
            w26.Fill = false;
            this.vbox2.Add(this.hbox3);
            Gtk.Box.BoxChild w27 = ((Gtk.Box.BoxChild)(this.vbox2[this.hbox3]));
            w27.Position = 4;
            w27.Expand = false;
            w27.Fill = false;
            this.hbox5.Add(this.vbox2);
            Gtk.Box.BoxChild w28 = ((Gtk.Box.BoxChild)(this.hbox5[this.vbox2]));
            w28.Position = 2;
            w28.Expand = false;
            w28.Fill = false;
            this.notebook1.Add(this.hbox5);
            Gtk.Notebook.NotebookChild w29 = ((Gtk.Notebook.NotebookChild)(this.notebook1[this.hbox5]));
            w29.Position = 1;
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
            this.button1 = new Gtk.Button();
            this.button1.CanFocus = true;
            this.button1.Name = "button1";
            this.button1.UseUnderline = true;
            this.button1.Label = Mono.Unix.Catalog.GetString("Fetch object list");
            this.vbox5.Add(this.button1);
            Gtk.Box.BoxChild w30 = ((Gtk.Box.BoxChild)(this.vbox5[this.button1]));
            w30.Position = 0;
            w30.Expand = false;
            w30.Fill = false;
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
            Gtk.Box.BoxChild w32 = ((Gtk.Box.BoxChild)(this.vbox5[this.GtkScrolledWindow3]));
            w32.Position = 1;
            this.hbox4.Add(this.vbox5);
            Gtk.Box.BoxChild w33 = ((Gtk.Box.BoxChild)(this.hbox4[this.vbox5]));
            w33.Position = 0;
            w33.Expand = false;
            w33.Fill = false;
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
            Gtk.Box.BoxChild w34 = ((Gtk.Box.BoxChild)(this.vbox6[this.label_totalobejcts]));
            w34.Position = 0;
            w34.Expand = false;
            w34.Fill = false;
            // Container child vbox6.Gtk.Box+BoxChild
            this.label6 = new Gtk.Label();
            this.label6.Name = "label6";
            this.label6.Xalign = 0F;
            this.label6.Yalign = 1F;
            this.label6.LabelProp = Mono.Unix.Catalog.GetString("Max objects");
            this.vbox6.Add(this.label6);
            Gtk.Box.BoxChild w35 = ((Gtk.Box.BoxChild)(this.vbox6[this.label6]));
            w35.Position = 1;
            w35.Expand = false;
            w35.Fill = false;
            // Container child vbox6.Gtk.Box+BoxChild
            this.label7 = new Gtk.Label();
            this.label7.Name = "label7";
            this.label7.Xalign = 0F;
            this.label7.LabelProp = Mono.Unix.Catalog.GetString("Region bonus factor");
            this.vbox6.Add(this.label7);
            Gtk.Box.BoxChild w36 = ((Gtk.Box.BoxChild)(this.vbox6[this.label7]));
            w36.Position = 2;
            w36.Expand = false;
            w36.Fill = false;
            // Container child vbox6.Gtk.Box+BoxChild
            this.label8 = new Gtk.Label();
            this.label8.Name = "label8";
            this.label8.Xalign = 0F;
            this.label8.LabelProp = Mono.Unix.Catalog.GetString("Owned by parcel owner");
            this.vbox6.Add(this.label8);
            Gtk.Box.BoxChild w37 = ((Gtk.Box.BoxChild)(this.vbox6[this.label8]));
            w37.Position = 3;
            w37.Expand = false;
            w37.Fill = false;
            // Container child vbox6.Gtk.Box+BoxChild
            this.label9 = new Gtk.Label();
            this.label9.Name = "label9";
            this.label9.Xalign = 0F;
            this.label9.LabelProp = Mono.Unix.Catalog.GetString("Set To group");
            this.vbox6.Add(this.label9);
            Gtk.Box.BoxChild w38 = ((Gtk.Box.BoxChild)(this.vbox6[this.label9]));
            w38.Position = 4;
            w38.Expand = false;
            w38.Fill = false;
            // Container child vbox6.Gtk.Box+BoxChild
            this.label10 = new Gtk.Label();
            this.label10.Name = "label10";
            this.label10.Xalign = 0F;
            this.label10.LabelProp = Mono.Unix.Catalog.GetString("Others");
            this.vbox6.Add(this.label10);
            Gtk.Box.BoxChild w39 = ((Gtk.Box.BoxChild)(this.vbox6[this.label10]));
            w39.Position = 5;
            w39.Expand = false;
            w39.Fill = false;
            this.hbox4.Add(this.vbox6);
            Gtk.Box.BoxChild w40 = ((Gtk.Box.BoxChild)(this.hbox4[this.vbox6]));
            w40.Position = 1;
            w40.Expand = false;
            w40.Fill = false;
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
            Gtk.Box.BoxChild w41 = ((Gtk.Box.BoxChild)(this.vbox7[this.entry_totalprims]));
            w41.Position = 0;
            w41.Expand = false;
            w41.Fill = false;
            // Container child vbox7.Gtk.Box+BoxChild
            this.entry_maxprims = new Gtk.Entry();
            this.entry_maxprims.CanFocus = true;
            this.entry_maxprims.Name = "entry_maxprims";
            this.entry_maxprims.IsEditable = false;
            this.entry_maxprims.WidthChars = 5;
            this.entry_maxprims.MaxLength = 5;
            this.entry_maxprims.InvisibleChar = '●';
            this.vbox7.Add(this.entry_maxprims);
            Gtk.Box.BoxChild w42 = ((Gtk.Box.BoxChild)(this.vbox7[this.entry_maxprims]));
            w42.Position = 1;
            w42.Expand = false;
            w42.Fill = false;
            // Container child vbox7.Gtk.Box+BoxChild
            this.entry_bonus = new Gtk.Entry();
            this.entry_bonus.CanFocus = true;
            this.entry_bonus.Name = "entry_bonus";
            this.entry_bonus.IsEditable = false;
            this.entry_bonus.WidthChars = 5;
            this.entry_bonus.MaxLength = 5;
            this.entry_bonus.InvisibleChar = '●';
            this.vbox7.Add(this.entry_bonus);
            Gtk.Box.BoxChild w43 = ((Gtk.Box.BoxChild)(this.vbox7[this.entry_bonus]));
            w43.Position = 2;
            w43.Expand = false;
            w43.Fill = false;
            // Container child vbox7.Gtk.Box+BoxChild
            this.entry_primsowner = new Gtk.Entry();
            this.entry_primsowner.CanFocus = true;
            this.entry_primsowner.Name = "entry_primsowner";
            this.entry_primsowner.IsEditable = false;
            this.entry_primsowner.WidthChars = 5;
            this.entry_primsowner.InvisibleChar = '●';
            this.vbox7.Add(this.entry_primsowner);
            Gtk.Box.BoxChild w44 = ((Gtk.Box.BoxChild)(this.vbox7[this.entry_primsowner]));
            w44.Position = 3;
            w44.Expand = false;
            w44.Fill = false;
            // Container child vbox7.Gtk.Box+BoxChild
            this.entry_primsgroup = new Gtk.Entry();
            this.entry_primsgroup.CanFocus = true;
            this.entry_primsgroup.Name = "entry_primsgroup";
            this.entry_primsgroup.IsEditable = false;
            this.entry_primsgroup.WidthChars = 5;
            this.entry_primsgroup.InvisibleChar = '●';
            this.vbox7.Add(this.entry_primsgroup);
            Gtk.Box.BoxChild w45 = ((Gtk.Box.BoxChild)(this.vbox7[this.entry_primsgroup]));
            w45.Position = 4;
            w45.Expand = false;
            w45.Fill = false;
            // Container child vbox7.Gtk.Box+BoxChild
            this.entry_primsother = new Gtk.Entry();
            this.entry_primsother.CanFocus = true;
            this.entry_primsother.Name = "entry_primsother";
            this.entry_primsother.IsEditable = false;
            this.entry_primsother.WidthChars = 5;
            this.entry_primsother.InvisibleChar = '●';
            this.vbox7.Add(this.entry_primsother);
            Gtk.Box.BoxChild w46 = ((Gtk.Box.BoxChild)(this.vbox7[this.entry_primsother]));
            w46.Position = 5;
            w46.Expand = false;
            w46.Fill = false;
            this.hbox4.Add(this.vbox7);
            Gtk.Box.BoxChild w47 = ((Gtk.Box.BoxChild)(this.hbox4[this.vbox7]));
            w47.Position = 2;
            this.notebook1.Add(this.hbox4);
            Gtk.Notebook.NotebookChild w48 = ((Gtk.Notebook.NotebookChild)(this.notebook1[this.hbox4]));
            w48.Position = 2;
            // Notebook tab
            this.label4 = new Gtk.Label();
            this.label4.Name = "label4";
            this.label4.LabelProp = Mono.Unix.Catalog.GetString("Objects");
            this.notebook1.SetTabLabel(this.hbox4, this.label4);
            this.label4.ShowAll();
            this.vbox1.Add(this.notebook1);
            Gtk.Box.BoxChild w49 = ((Gtk.Box.BoxChild)(this.vbox1[this.notebook1]));
            w49.Position = 1;
            this.Add(this.vbox1);
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.Show();
            this.treeview_parcels.CursorChanged += new System.EventHandler(this.OnTreeviewParcelsCursorChanged);
            this.button1.Activated += new System.EventHandler(this.OnButton1Activated);
            this.button1.Clicked += new System.EventHandler(this.OnButton1Clicked);
        }
    }
}
