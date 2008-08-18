// ScriptDialogue.cs created with MonoDevelop
// User: robin at 16:41 18/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;
using libsecondlife;

namespace omvviewerlight
{
	
	
	public partial class ScriptDialogue : Gtk.Window
	{
		
		int channel;
		public ScriptDialogue(string message,string objectName,LLUUID imageID,LLUUID objectID,string FirstName,string lastName,int chatChannel,List <string> buttons) : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			channel=chatChannel;
			
			int number=buttons.Count;
			List <Gtk.Button> xbuttons = new List<Gtk.Button>();
			xbuttons.Add(this.button6);
			xbuttons.Add(this.button7);
			xbuttons.Add(this.button8);
			xbuttons.Add(this.button9);
			xbuttons.Add(this.button10);
			xbuttons.Add(this.button11);
			xbuttons.Add(this.button12);
			xbuttons.Add(this.button13);
			xbuttons.Add(this.button14);
			xbuttons.Add(this.button15);
			xbuttons.Add(this.button16);
			xbuttons.Add(this.button17);
	
			foreach(Gtk.Button xx in xbuttons)
			{
				xx.Label="";
				xx.Hide();
				xx.Clicked += new EventHandler(ButtonClicked);
			}
			
			int x=0;
			foreach(string button in buttons)
			{
				xbuttons[x].Label=button;
				xbuttons[x].Show();				
				x++;
			}
			
		 this.label1.Text="Object :"+objectName+"\n"+"Owner :"+FirstName+" "+lastName+"\n"+message;
		}
		
		void ButtonClicked(object o, EventArgs e)
		{
			Gtk.Button thebut=(Gtk.Button)o;
			//thebut.Label;
			MainClass.client.Self.Chat(thebut.Label,channel,ChatType.Normal);
			this.Destroy();
		}
	}
}
