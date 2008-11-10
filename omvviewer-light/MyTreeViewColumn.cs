using System;
using System.Collections.Generic;
using System.Text;
using Gtk;

namespace omvviewerlight
{
	public class MyTreeViewColumn : Gtk.TreeViewColumn
	{
        Gtk.Image col_icon;
        int model_col;
        Gtk.ListStore model;
        Gdk.Pixbuf up_arrow;
        Gdk.Pixbuf down_arrow;
        Gdk.Pixbuf blank_arrow;


        public MyTreeViewColumn(string title, Gtk.CellRenderer cell,string prop,int col) : base (title,cell,prop,col)
        {
            up_arrow = MainClass.GetResource("up_arrow.tga");
            down_arrow = MainClass.GetResource("down_arrow.tga");
            blank_arrow = MainClass.GetResource("blank_arrow.tga");
            model_col = col;
            col_icon = new Gtk.Image(up_arrow);
            Gtk.HBox hb = new Gtk.HBox();
            Gtk.Label lb = new Label(title);
            hb.PackEnd(col_icon);
            hb.PackEnd(lb);
            this.Widget = hb;
            hb.ShowAll();
           
            base.Clicked += new EventHandler(col_clicked);
            
        }

        public void setmodel(Gtk.ListStore m)
        {
            model = m;
            m.SortColumnChanged += new EventHandler(m_SortColumnChanged);
        }

        void m_SortColumnChanged(object sender, EventArgs e)
        {

            int tSortColumnId;
            SortType order;
            model.GetSortColumnId(out tSortColumnId, out order);
            if(tSortColumnId!=model_col)
                col_icon.Pixbuf = blank_arrow;
        }

        void col_clicked(object sender, EventArgs e)
        {
            if (model == null)
                return;

            int tSortColumnId;
            SortType order;

            model.GetSortColumnId(out tSortColumnId, out order);

            if (tSortColumnId == model_col)
            {
                if (order == Gtk.SortType.Ascending)
                {
                    model.SetSortColumnId(model_col, Gtk.SortType.Descending);
                    col_icon.Pixbuf = down_arrow;
                }
                else
                {
                    model.SetSortColumnId(model_col, Gtk.SortType.Ascending);
                    col_icon.Pixbuf = up_arrow;
                }

                return;
            }

            model.SetSortColumnId(model_col, Gtk.SortType.Ascending);
            col_icon.Pixbuf = up_arrow;
        }

	}
}
