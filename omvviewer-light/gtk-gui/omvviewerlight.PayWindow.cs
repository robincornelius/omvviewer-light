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
    
    
    public partial class PayWindow {
        
        private Gtk.VBox vbox3;
        
        private Gtk.Label label_payinfo;
        
        private Gtk.VBox vbox4;
        
        private Gtk.HBox hbox5;
        
        private Gtk.Label label11;
        
        private Gtk.Entry entry1;
        
        private Gtk.Label label12;
        
        private Gtk.HBox hbox4;
        
        private Gtk.Button button_pay;
        
        private Gtk.Button button_cancel;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget omvviewerlight.PayWindow
            this.Name = "omvviewerlight.PayWindow";
            this.Title = Mono.Unix.Catalog.GetString("Pay");
            this.Icon = new Gdk.Pixbuf(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "status_money.tga"));
            this.WindowPosition = ((Gtk.WindowPosition)(4));
            this.Modal = true;
            // Container child omvviewerlight.PayWindow.Gtk.Container+ContainerChild
            this.vbox3 = new Gtk.VBox();
            this.vbox3.Name = "vbox3";
            this.vbox3.Spacing = 6;
            // Container child vbox3.Gtk.Box+BoxChild
            this.label_payinfo = new Gtk.Label();
            this.label_payinfo.Name = "label_payinfo";
            this.label_payinfo.LabelProp = Mono.Unix.Catalog.GetString("Pay \nResident Michelle2 Zenovka\n");
            this.label_payinfo.Justify = ((Gtk.Justification)(2));
            this.vbox3.Add(this.label_payinfo);
            Gtk.Box.BoxChild w1 = ((Gtk.Box.BoxChild)(this.vbox3[this.label_payinfo]));
            w1.Position = 0;
            w1.Expand = false;
            w1.Fill = false;
            // Container child vbox3.Gtk.Box+BoxChild
            this.vbox4 = new Gtk.VBox();
            this.vbox4.Name = "vbox4";
            this.vbox4.Spacing = 6;
            // Container child vbox4.Gtk.Box+BoxChild
            this.hbox5 = new Gtk.HBox();
            this.hbox5.Name = "hbox5";
            this.hbox5.Spacing = 6;
            // Container child hbox5.Gtk.Box+BoxChild
            this.label11 = new Gtk.Label();
            this.label11.Name = "label11";
            this.label11.LabelProp = Mono.Unix.Catalog.GetString("Amount");
            this.hbox5.Add(this.label11);
            Gtk.Box.BoxChild w2 = ((Gtk.Box.BoxChild)(this.hbox5[this.label11]));
            w2.Position = 0;
            w2.Expand = false;
            w2.Fill = false;
            // Container child hbox5.Gtk.Box+BoxChild
            this.entry1 = new Gtk.Entry();
            this.entry1.WidthRequest = 50;
            this.entry1.CanFocus = true;
            this.entry1.Name = "entry1";
            this.entry1.IsEditable = true;
            this.entry1.InvisibleChar = '●';
            this.hbox5.Add(this.entry1);
            Gtk.Box.BoxChild w3 = ((Gtk.Box.BoxChild)(this.hbox5[this.entry1]));
            w3.Position = 1;
            // Container child hbox5.Gtk.Box+BoxChild
            this.label12 = new Gtk.Label();
            this.label12.Name = "label12";
            this.label12.LabelProp = Mono.Unix.Catalog.GetString("$L");
            this.hbox5.Add(this.label12);
            Gtk.Box.BoxChild w4 = ((Gtk.Box.BoxChild)(this.hbox5[this.label12]));
            w4.Position = 2;
            w4.Expand = false;
            w4.Fill = false;
            this.vbox4.Add(this.hbox5);
            Gtk.Box.BoxChild w5 = ((Gtk.Box.BoxChild)(this.vbox4[this.hbox5]));
            w5.Position = 0;
            w5.Expand = false;
            w5.Fill = false;
            this.vbox3.Add(this.vbox4);
            Gtk.Box.BoxChild w6 = ((Gtk.Box.BoxChild)(this.vbox3[this.vbox4]));
            w6.Position = 1;
            w6.Expand = false;
            w6.Fill = false;
            // Container child vbox3.Gtk.Box+BoxChild
            this.hbox4 = new Gtk.HBox();
            this.hbox4.Name = "hbox4";
            this.hbox4.Homogeneous = true;
            this.hbox4.Spacing = 6;
            // Container child hbox4.Gtk.Box+BoxChild
            this.button_pay = new Gtk.Button();
            this.button_pay.CanFocus = true;
            this.button_pay.Name = "button_pay";
            this.button_pay.UseUnderline = true;
            this.button_pay.Label = Mono.Unix.Catalog.GetString("Pay");
            this.hbox4.Add(this.button_pay);
            Gtk.Box.BoxChild w7 = ((Gtk.Box.BoxChild)(this.hbox4[this.button_pay]));
            w7.Position = 0;
            w7.Expand = false;
            w7.Fill = false;
            // Container child hbox4.Gtk.Box+BoxChild
            this.button_cancel = new Gtk.Button();
            this.button_cancel.CanFocus = true;
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.UseUnderline = true;
            this.button_cancel.Label = Mono.Unix.Catalog.GetString("Cancel");
            this.hbox4.Add(this.button_cancel);
            Gtk.Box.BoxChild w8 = ((Gtk.Box.BoxChild)(this.hbox4[this.button_cancel]));
            w8.Position = 1;
            w8.Expand = false;
            w8.Fill = false;
            this.vbox3.Add(this.hbox4);
            Gtk.Box.BoxChild w9 = ((Gtk.Box.BoxChild)(this.vbox3[this.hbox4]));
            w9.Position = 2;
            w9.Expand = false;
            w9.Fill = false;
            this.Add(this.vbox3);
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.DefaultWidth = 400;
            this.DefaultHeight = 155;
            this.Show();
            this.button_pay.Clicked += new System.EventHandler(this.OnButtonPayClicked);
            this.button_cancel.Clicked += new System.EventHandler(this.OnButtonCancelClicked);
        }
    }
}
