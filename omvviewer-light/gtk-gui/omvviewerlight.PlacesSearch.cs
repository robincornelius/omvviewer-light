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
    
    
    public partial class PlacesSearch {
        
        private Gtk.VBox vbox1;
        
        private Gtk.HBox hbox1;
        
        private Gtk.Label label1;
        
        private Gtk.Entry entry1;
        
        private Gtk.Button button_search;
        
        private Gtk.HBox hbox2;
        
        private Gtk.ScrolledWindow GtkScrolledWindow;
        
        private Gtk.TreeView treeview1;
        
        private Gtk.VBox vbox2;
        
        private Gtk.CheckButton checkbutton_mature;
        
        private Gtk.HBox hbox3;
        
        private Gtk.Button button_TP;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget omvviewerlight.PlacesSearch
            Stetic.BinContainer.Attach(this);
            this.Name = "omvviewerlight.PlacesSearch";
            // Container child omvviewerlight.PlacesSearch.Gtk.Container+ContainerChild
            this.vbox1 = new Gtk.VBox();
            this.vbox1.Name = "vbox1";
            this.vbox1.Spacing = 6;
            // Container child vbox1.Gtk.Box+BoxChild
            this.hbox1 = new Gtk.HBox();
            this.hbox1.Name = "hbox1";
            this.hbox1.Spacing = 6;
            // Container child hbox1.Gtk.Box+BoxChild
            this.label1 = new Gtk.Label();
            this.label1.Name = "label1";
            this.label1.LabelProp = Mono.Unix.Catalog.GetString("Find");
            this.hbox1.Add(this.label1);
            Gtk.Box.BoxChild w1 = ((Gtk.Box.BoxChild)(this.hbox1[this.label1]));
            w1.Position = 0;
            w1.Expand = false;
            w1.Fill = false;
            // Container child hbox1.Gtk.Box+BoxChild
            this.entry1 = new Gtk.Entry();
            this.entry1.CanFocus = true;
            this.entry1.Name = "entry1";
            this.entry1.IsEditable = true;
            this.entry1.InvisibleChar = '●';
            this.hbox1.Add(this.entry1);
            Gtk.Box.BoxChild w2 = ((Gtk.Box.BoxChild)(this.hbox1[this.entry1]));
            w2.Position = 1;
            // Container child hbox1.Gtk.Box+BoxChild
            this.button_search = new Gtk.Button();
            this.button_search.CanFocus = true;
            this.button_search.Name = "button_search";
            this.button_search.UseUnderline = true;
            this.button_search.Label = Mono.Unix.Catalog.GetString("Search");
            this.hbox1.Add(this.button_search);
            Gtk.Box.BoxChild w3 = ((Gtk.Box.BoxChild)(this.hbox1[this.button_search]));
            w3.Position = 2;
            w3.Expand = false;
            w3.Fill = false;
            this.vbox1.Add(this.hbox1);
            Gtk.Box.BoxChild w4 = ((Gtk.Box.BoxChild)(this.vbox1[this.hbox1]));
            w4.Position = 0;
            w4.Expand = false;
            w4.Fill = false;
            // Container child vbox1.Gtk.Box+BoxChild
            this.hbox2 = new Gtk.HBox();
            this.hbox2.Name = "hbox2";
            this.hbox2.Spacing = 6;
            // Container child hbox2.Gtk.Box+BoxChild
            this.GtkScrolledWindow = new Gtk.ScrolledWindow();
            this.GtkScrolledWindow.Name = "GtkScrolledWindow";
            this.GtkScrolledWindow.ShadowType = ((Gtk.ShadowType)(1));
            // Container child GtkScrolledWindow.Gtk.Container+ContainerChild
            this.treeview1 = new Gtk.TreeView();
            this.treeview1.WidthRequest = 400;
            this.treeview1.CanFocus = true;
            this.treeview1.Name = "treeview1";
            this.treeview1.HeadersClickable = true;
            this.GtkScrolledWindow.Add(this.treeview1);
            this.hbox2.Add(this.GtkScrolledWindow);
            Gtk.Box.BoxChild w6 = ((Gtk.Box.BoxChild)(this.hbox2[this.GtkScrolledWindow]));
            w6.Position = 0;
            // Container child hbox2.Gtk.Box+BoxChild
            this.vbox2 = new Gtk.VBox();
            this.vbox2.Spacing = 6;
            // Container child vbox2.Gtk.Box+BoxChild
            this.checkbutton_mature = new Gtk.CheckButton();
            this.checkbutton_mature.CanFocus = true;
            this.checkbutton_mature.Name = "checkbutton_mature";
            this.checkbutton_mature.Label = Mono.Unix.Catalog.GetString("Enable Mature");
            this.checkbutton_mature.Active = true;
            this.checkbutton_mature.DrawIndicator = true;
            this.checkbutton_mature.UseUnderline = true;
            this.vbox2.Add(this.checkbutton_mature);
            Gtk.Box.BoxChild w7 = ((Gtk.Box.BoxChild)(this.vbox2[this.checkbutton_mature]));
            w7.Position = 0;
            w7.Expand = false;
            w7.Fill = false;
            this.hbox2.Add(this.vbox2);
            Gtk.Box.BoxChild w8 = ((Gtk.Box.BoxChild)(this.hbox2[this.vbox2]));
            w8.Position = 1;
            this.vbox1.Add(this.hbox2);
            Gtk.Box.BoxChild w9 = ((Gtk.Box.BoxChild)(this.vbox1[this.hbox2]));
            w9.Position = 1;
            // Container child vbox1.Gtk.Box+BoxChild
            this.hbox3 = new Gtk.HBox();
            this.hbox3.Name = "hbox3";
            this.hbox3.Spacing = 6;
            // Container child hbox3.Gtk.Box+BoxChild
            this.button_TP = new Gtk.Button();
            this.button_TP.CanFocus = true;
            this.button_TP.Name = "button_TP";
            this.button_TP.UseUnderline = true;
            this.button_TP.Label = Mono.Unix.Catalog.GetString("Teleport");
            this.hbox3.Add(this.button_TP);
            Gtk.Box.BoxChild w10 = ((Gtk.Box.BoxChild)(this.hbox3[this.button_TP]));
            w10.Position = 0;
            w10.Expand = false;
            w10.Fill = false;
            this.vbox1.Add(this.hbox3);
            Gtk.Box.BoxChild w11 = ((Gtk.Box.BoxChild)(this.vbox1[this.hbox3]));
            w11.Position = 2;
            w11.Expand = false;
            w11.Fill = false;
            this.Add(this.vbox1);
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.Show();
            this.button_search.Clicked += new System.EventHandler(this.OnButtonSearchClicked);
            this.button_TP.Clicked += new System.EventHandler(this.OnButtonTPClicked);
        }
    }
}
