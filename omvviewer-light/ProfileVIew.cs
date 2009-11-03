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

// ProfileVIew.cs created with MonoDevelop
// User: robin at 15:15 15/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;
using System.IO;
using OpenMetaverse;
using Gtk;


namespace omvviewerlight
{

	public partial class ProfileVIew : Gtk.Window
	{
		
		UUID profile_pic;
		UUID firstlife_pic;
		UUID partner_key;
		UUID resident;
		
		List <UUID>picks_waiting;
		
		public ProfileVIew(UUID key) : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
	
			picks_waiting=new List<UUID>();
			resident=key;

            MainClass.client.Avatars.AvatarPropertiesReply += new EventHandler<AvatarPropertiesReplyEventArgs>(Avatars_AvatarPropertiesReply);
            MainClass.client.Avatars.UUIDNameReply += new EventHandler<UUIDNameReplyEventArgs>(Avatars_UUIDNameReply);
            MainClass.client.Avatars.PickInfoReply += new EventHandler<PickInfoReplyEventArgs>(Avatars_PickInfoReply);
            MainClass.client.Avatars.AvatarPicksReply += new EventHandler<AvatarPicksReplyEventArgs>(Avatars_AvatarPicksReply);

   			MainClass.client.Avatars.RequestAvatarProperties(key);
	
			MainClass.client.Avatars.RequestAvatarPicks(key);
            this.DeleteEvent += new DeleteEventHandler(OnDeleteEvent);
			
			this.label_born.Text="";
			this.label_identified.Text="";
			this.label_name.Text="";
		    this.label_partner.Text="";
			this.label_pay.Text="";
			this.label_status.Text="";	
			Gdk.Pixbuf buf=MainClass.GetResource("trying.png");
			this.image3.Pixbuf=buf.ScaleSimple(128,128,Gdk.InterpType.Bilinear);
			
			this.image7.Pixbuf=buf.ScaleSimple(128,128,Gdk.InterpType.Bilinear);
			
		}


        ~ProfileVIew()
		{
			Console.WriteLine("ProfileView Cleaned up");
		}		
		
		
        [GLib.ConnectBefore]
        void OnDeleteEvent(object o, DeleteEventArgs args)
        {
            picks_waiting.Clear();
            MainClass.client.Avatars.AvatarPropertiesReply -= new EventHandler<AvatarPropertiesReplyEventArgs>(Avatars_AvatarPropertiesReply);
            MainClass.client.Avatars.UUIDNameReply -= new EventHandler<UUIDNameReplyEventArgs>(Avatars_UUIDNameReply);
            MainClass.client.Avatars.PickInfoReply -= new EventHandler<PickInfoReplyEventArgs>(Avatars_PickInfoReply);
            MainClass.client.Avatars.AvatarPicksReply -= new EventHandler<AvatarPicksReplyEventArgs>(Avatars_AvatarPicksReply);
            this.DeleteEvent -= new DeleteEventHandler(OnDeleteEvent);
            Console.WriteLine("Profile view go bye bye");
			this.Destroy();	
			//Finalize();
			//System.GC.SuppressFinalize(this);

        }
			



        void Avatars_PickInfoReply(object sender, PickInfoReplyEventArgs e)
		{				
			if(!this.picks_waiting.Contains(e.PickID))
				return;
			
			picks_waiting.Remove(e.PickID);
			   
			Gtk.Application.Invoke(delegate {	
			
				aPick tpick= new aPick(e.Pick.SnapshotID,e.Pick.Name,e.Pick.Desc,e.Pick.Name,e.Pick.SimName,e.Pick.PosGlobal);
				Gtk.Label lable=new Gtk.Label(e.Pick.Name.Substring(0,e.Pick.Name.Length>10?10:e.Pick.Name.Length));
				this.ShowAll();
				
				this.notebook_picks.InsertPage(tpick,lable,-1);
				this.notebook_picks.ShowAll();
			});
		}



		void Avatars_AvatarPicksReply(object sender, AvatarPicksReplyEventArgs e)
	    {
			if(e.AvatarID!=	resident)
				return;
		
			Gtk.Application.Invoke(delegate {	
				foreach(KeyValuePair<UUID,string> pick in e.Picks)
				{	
					//this.notebook_picks.InsertPage(
					this.picks_waiting.Add(pick.Key);
					MainClass.client.Avatars.RequestPickInfo(resident,pick.Key);
				}
			});
		}


	                                                   
		    void Avatars_UUIDNameReply(object sender, UUIDNameReplyEventArgs e)
			{
			//what the hell, lets cache them to the program store if we find them
			//Possible to do, move this type of stuff more global
			Console.Write("Got new names \n");
			
			foreach(KeyValuePair<UUID,string> name in e.Names)
			{
				//if(!MainClass.av_names.ContainsKey(name.Key))
					//MainClass.av_names.Add(name.Key,name.Value);		
			
				Console.Write("Names = "+name.Value+"\n");
			}

			Gtk.Application.Invoke(delegate {	
				if(MainClass.name_cache.av_names.ContainsKey(resident))
				{
					this.label_name.Text=MainClass.name_cache.av_names[resident];
				}
				else
				{
					this.label_name.Text="Waiting....";
				}
				this.QueueDraw();
			});
			
		
			Gtk.Application.Invoke(delegate {	
				if(MainClass.name_cache.av_names.ContainsKey(partner_key))
				{
					this.label_partner.Text=MainClass.name_cache.av_names[partner_key];
				}
				else
				{
					this.label_partner.Text="Waiting....";
				}
				this.QueueDraw();
			});
				
		}

        void Avatars_AvatarPropertiesReply(object sender, AvatarPropertiesReplyEventArgs e)
            {
			if(e.AvatarID!=	resident)
				return;

			Gtk.Application.Invoke(delegate {
				
			this.label_born.Text=e.Properties.BornOn;

			partner_key=e.Properties.Partner;
			
			if(e.Properties.Online)
				this.label_status.Text="Online";
			else
				this.label_status.Text="Offline";
			
			if(e.Properties.Transacted)
				this.label_pay.Text="Pay info on file";
			else
				this.label_pay.Text="No";
			
			if(e.Properties.Identified)
				this.label_identified.Text="Yes";
			else
				this.label_identified.Text="No";
			
			this.textview2.Buffer.Text=e.Properties.AboutText;
				
			this.textview3.Buffer.Text=e.Properties.FirstLifeText;
				
			profile_pic=e.Properties.ProfileImage;
			firstlife_pic=e.Properties.FirstLifeImage;

			TryGetImage getter= new TryGetImage(this.image7,profile_pic,128,128,false);
			TryGetImage getter2= new TryGetImage(this.image3,firstlife_pic,128,128,false);
							
			if(MainClass.name_cache.av_names.ContainsKey(e.AvatarID))
			{
				this.label_name.Text=MainClass.name_cache.av_names[e.AvatarID];
			}
			else
			{
				MainClass.client.Avatars.RequestAvatarName(e.AvatarID);
				this.label_name.Text="Waiting....";
			}
						
			if(e.Properties.Partner!=UUID.Zero)
			{	
				if(MainClass.name_cache.av_names.ContainsKey(partner_key))
				{
					this.label_partner.Text=MainClass.name_cache.av_names[partner_key];
				}
				else
				{
					MainClass.client.Avatars.RequestAvatarName(partner_key);
					this.label_partner.Text="Waiting....";
				}
			}														
		});
		}

		protected virtual void OnButtonAddfriendClicked (object sender, System.EventArgs e)
		{
		 MainClass.client.Friends.OfferFriendship(resident);
		}

		protected virtual void OnEventbox1ButtonPressEvent (object o, Gtk.ButtonPressEventArgs args)
		{
			if(this.profile_pic==UUID.Zero)
				return;

			TexturePreview tp= new TexturePreview(this.profile_pic,this.label_name.Text,false);
			tp.ShowAll();
		}

		protected virtual void OnEventbox2ButtonPressEvent (object o, Gtk.ButtonPressEventArgs args)
		{
			if(this.firstlife_pic==UUID.Zero)
				return;
			
			TexturePreview tp= new TexturePreview(this.firstlife_pic,this.label_name.Text,false);
			tp.ShowAll();

		}
	
	}
}
