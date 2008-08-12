// MainWindow.cs created with MonoDevelop
// User: robin at 08:52 11/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
using System;
using System.Collections.Generic;
using Gtk;
using libsecondlife;
using omvviewerlight;

public partial class MainWindow: Gtk.Window
{	
	public List<libsecondlife.LLUUID>active_ims = new List<libsecondlife.LLUUID>();
	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
		MainClass.client.Self.OnInstantMessage += new libsecondlife.AgentManager.InstantMessageCallback(onIM);
	}
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	public void addnotetab(string name)
	{
	}		

	void onIM(InstantMessage im, Simulator sim)
	{
		
		// don't do this yet
		if(im.GroupIM==true)
			return;
		
		if(!active_ims.Contains(im.FromAgentID))
		{
			Widget lable=new Gtk.Label(im.FromAgentName);
			ChatConsole imc=new ChatConsole(im);
			notebook.InsertPage(imc,lable,notebook.Page);
			active_ims.Add(im.FromAgentID);
		}		
		}

}