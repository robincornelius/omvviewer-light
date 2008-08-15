// PayWindow.cs created with MonoDevelop
// User: robin at 12:42 15/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;
using libsecondlife;

namespace omvviewerlight
{
	
	public partial class PayWindow : Gtk.Window
	{
		string resident;
		LLUUID resident_key;
		LLUUID object_key;
		string object_name;
		bool is_object;
		int amountpay;
		
		
		public PayWindow(Primitive prim,int amount) : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			is_object=true;
			amountpay=amount;
			MainClass.client.Avatars.OnAvatarNames += new libsecondlife.AvatarManager.AvatarNamesCallback(on_avnames);
			resident_key=prim.Properties.OwnerID;
			request_name(prim.Properties.OwnerID);
					
			object_key=prim.Properties.ObjectID;
			object_name=prim.Properties.Name;
								
			refresh();
		}
	
	public PayWindow(LLUUID target,int amount) : 
				base(Gtk.WindowType.Toplevel)
		{
			is_object=false;
			amountpay=amount;
			MainClass.client.Avatars.OnAvatarNames += new libsecondlife.AvatarManager.AvatarNamesCallback(on_avnames);
			resident_key=target;	
			request_name(target);
			this.Build();
			refresh();
		}	
	
		void request_name(LLUUID id)
		{
			Console.Write("Requesting name for ID : "+id.ToString()+"\n");
			if(!MainClass.av_names.ContainsKey(id))
			{
				Console.Write("Not found sending to server\n");
				MainClass.client.Avatars.RequestAvatarName(id);			
			}
			else
			{
				Console.Write("Already known :"+MainClass.av_names[id]+"\n");
			}
		}
		
		void refresh()
		{
			this.entry1.Text=amountpay.ToString();
			
			if(!MainClass.av_names.TryGetValue(resident_key, out resident))
			{
				resident="Waiting......";
			}
			
			if(is_object)
			{
				this.label_payinfo.Text="Pay\nResident :"+resident+"\nVia Object :"+object_name;								
			}
			else
			{
				this.label_payinfo.Text="Pay\nResident :"+resident;		
			}
			
		}
					
		void on_avnames(Dictionary<LLUUID, string> names)
	    {
			//what the hell, lets cache them to the program store if we find them
			//Possible to do, move this type of stuff more global
			Console.Write("Got new names \n");
			
			foreach(KeyValuePair<LLUUID,string> name in names)
			{
				if(!MainClass.av_names.ContainsKey(name.Key))
					MainClass.av_names.Add(name.Key,name.Value);		
			
				Console.Write("Names = "+name.Value+"\n");
			}
			
			Gtk.Application.Invoke(delegate {	
				refresh();
				this.QueueDraw();
			});
			
	    }

		protected virtual void OnButtonCancelClicked (object sender, System.EventArgs e)
		{
			MainClass.client.Avatars.OnAvatarNames -= new libsecondlife.AvatarManager.AvatarNamesCallback(on_avnames);
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
