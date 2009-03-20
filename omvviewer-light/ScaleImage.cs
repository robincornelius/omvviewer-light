// 
//  Copyright (C) 2009 robin
// 
//  This program is free software; you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation; either version 2 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU General Public License for more details.
//  
//  You should have received a copy of the GNU General Public License
//  along with this program; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
// 

using System;
using Gdk;
using Gtk;

namespace omvviewerlight
{
	
	
	[System.ComponentModel.ToolboxItem(true)]
	public partial class ScaleImage : Gtk.Bin
	{
		
		public Gtk.Image baseimage=new Gtk.Image();
		int width;
		int height;
        int oldwidth = -1;
        int oldheight = -1;

		
		public ScaleImage()
		{		
			this.Build();
			this.SizeAllocated += HandleSizeAllocated;
           
		}		
		
		public void clear()
		{
			
		}
		
		public void setimage()
		{	
			if(height>0 && width>0)
			{
				dispimage.Pixbuf=baseimage.Pixbuf.ScaleSimple(width,height,InterpType.Bilinear);			
			}
			else
			{
				dispimage.Pixbuf=(Gdk.Pixbuf)baseimage.Pixbuf.Clone();	
			}
		}

		void HandleSizeAllocated(object o, SizeAllocatedArgs args)
		{

			width=args.Allocation.Width;
			height=args.Allocation.Height;

            if (oldwidth == width && oldheight == height)
                return;

            oldwidth = width;
            oldheight = height;

			if(baseimage.Pixbuf==null)
				return;
			dispimage.Pixbuf=baseimage.Pixbuf.ScaleSimple(args.Allocation.Width,args.Allocation.Height,InterpType.Bilinear);			
		
		}

	}
}
