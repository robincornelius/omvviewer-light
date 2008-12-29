/*
omvviewerlight a Text based client to metaverses such as Linden Labs Secondlife(tm)
    Copyright (C) 2008  Robin Cornelius <robin.cornelius@gmail.com>

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation,either version 2 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

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
