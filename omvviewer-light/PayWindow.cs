/*
omvviewerlight a Text based client to metaverses such as Linden Labs Secondlife(tm)
    Copyright (C) 2008,2009  Robin Cornelius <robin.cornelius@gmail.com>

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 2 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

// PayWindow.cs created with MonoDevelop
// User: robin at 12:42 15/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;
using OpenMetaverse;

namespace omvviewerlight
{
	
	public partial class PayWindow : Gtk.Window
	{
		string resident;
		UUID resident_key;
		UUID object_key;
		string object_name;
		bool is_object;
		int amountpay;
		
		
		public PayWindow(Primitive prim,int amount) : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			is_object=true;
			amountpay=amount;
            resident_key=prim.Properties.OwnerID;					
			object_key=prim.Properties.ObjectID;
			object_name=prim.Properties.Name;
								
		}

	
	public PayWindow(UUID target,int amount) : 
				base(Gtk.WindowType.Toplevel)
		{
			is_object=false;
			amountpay=amount;
            resident_key = target;

            resident = "Waiting...";
            AsyncNameUpdate ud = new AsyncNameUpdate(target, false);
            ud.onNameCallBack += new AsyncNameUpdate.NameCallBack(ud_onNameCallBack);
            ud.go();             

            this.Build();
		}

        void ud_onNameCallBack(string name, object[] values)
        {
            this.entry1.Text = amountpay.ToString();

            if (is_object)
            {
                this.label_payinfo.Text = "Pay\nResident :" + name + "\nVia Object :" + object_name;
            }
            else
            {
                this.label_payinfo.Text = "Pay\nResident :" + name;
            }

        }	
	
		protected virtual void OnButtonCancelClicked (object sender, System.EventArgs e)
		{
            this.Destroy();
		}

		protected virtual void OnButtonPayClicked (object sender, System.EventArgs e)
		{
			int amount;
			
			if(!int.TryParse(this.entry1.Text,out amount))
			{
				entry1.Text="Error";
				return;				
			}
			
			if(is_object)
				MainClass.client.Self.GiveObjectMoney(object_key,amount,object_name);
			else
				MainClass.client.Self.GiveAvatarMoney(this.resident_key,amount);


			this.Destroy();
		}
	}
}
