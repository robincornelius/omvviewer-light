// ProfileVIew.cs created with MonoDevelop
// User: robin at 15:15 15/08/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;
using libsecondlife;

namespace omvviewerlight
{
	
	
	public partial class ProfileVIew : Gtk.Window
	{
		
		LLUUID profile_pic;
		LLUUID firstlife_pic;
		LLUUID partner_key;
		LLUUID resident;
		
		Gtk.ListStore store;		
		
		
		public ProfileVIew(LLUUID key) : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
					
			MainClass.client.Avatars.OnAvatarProperties += new libsecondlife.AvatarManager.AvatarPropertiesCallback(onAvatarProperties);
			MainClass.client.Avatars.RequestAvatarProperties(key);
			MainClass.client.Assets.OnImageReceived += new libsecondlife.AssetManager.ImageReceivedCallback(onGotImage);
			MainClass.client.Avatars.OnAvatarNames += new libsecondlife.AvatarManager.AvatarNamesCallback(on_avnames);
			MainClass.client.Avatars.OnAvatarPicks += new libsecondlife.AvatarManager.AvatarPicksCallback(onPicks);
			MainClass.client.Avatars.OnPickInfo += new libsecondlife.AvatarManager.PickInfoCallback(onPickInfo);
			MainClass.client.Avatars.RequestAvatarPicks(key);
			resident=key;
			
			this.label_born.Text="";
			this.label_identified.Text="";
			this.label_name.Text="";
		    this.label_partner.Text="";
			this.label_pay.Text="";
			this.label_status.Text="";	
		}
	
		void onPickInfo(LLUUID pick,ProfilePick info)
		{
			
			//Arrrrrrrrgggggg
			aPick tpick= new aPick(info.SnapshotID,info.Name,info.Desc,info.Name,info.SimName,info.PosGlobal);
			Gtk.Label lable=new Gtk.Label(info.Name.Substring(0,10));
			this.ShowAll();
			
			this.notebook_picks.InsertPage(tpick,lable,-1);
			this.notebook_picks.ShowAll();

		}
		
		void onPicks(LLUUID avatar, Dictionary<LLUUID,string> picks)
	    {
			Gtk.Application.Invoke(delegate {	
				foreach(KeyValuePair<LLUUID,string> pick in picks)
				{	
					//this.notebook_picks.InsertPage(
					MainClass.client.Avatars.RequestPickInfo(resident,pick.Key);
				}
			});
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
				if(MainClass.av_names.ContainsKey(resident))
				{
					this.label_name.Text=MainClass.av_names[resident];
				}
				else
				{
					this.label_name.Text="Waiting....";
				}
				this.QueueDraw();
			});
			
		
			Gtk.Application.Invoke(delegate {	
				if(MainClass.av_names.ContainsKey(partner_key))
				{
					this.label_partner.Text=MainClass.av_names[partner_key];
				}
				else
				{
					this.label_partner.Text="Waiting....";
				}
				this.QueueDraw();
			});
				
		}		
		
		unsafe void onGotImage(ImageDownload image,AssetTexture asset)
		{
	
		Gtk.Application.Invoke(delegate {	
				
			if(asset.AssetID==this.firstlife_pic)
				Console.Write("Downloaded first life pic\n");
				              
			if(asset.AssetID==this.profile_pic)
			{
				Console.Write("Downloaded profile pic\n");
				if(asset.Decode())
				{
					
					Console.Write("Decoded\n");
					Console.Write("Channels : "+asset.Image.Channels.ToString()+"\n");
					//Console.Write("Length "+asset.Image.Blue.GetLength().ToString()+"\n");
					int height =asset.Image.Height;
					int width = asset.Image.Width;
					int channels=4;
					int rowstride=width*channels;
					int length = asset.Image.Red.Length;

					//AssetTexture asset;
					//asset.Image.
						
					byte[] data = asset.Image.ExportRaw();
					
					Console.Write("W "+width.ToString()+" H "+height.ToString()+"\n");
			
					//Gdk.Pixbuf buf=new Gdk.Pixbuf(data,true,8,width,height,rowstride);
					Gdk.Pixbuf buf = new Gdk.Pixbuf(Gdk.Colorspace.Rgb,true,8,width,height);
					
					sbyte * pixels=(sbyte *)buf.Pixels;
					
						int dest=0;
						
					for(int x=0;x<(width*height*4);x=x+4)
					{
						pixels[x]=(sbyte)data[x];
						pixels[x+1]=(sbyte)data[x];
						pixels[x+2]=(sbyte)data[x];
						pixels[x+3]=(sbyte)data[x];							
						}

					this.image7.Pixbuf=buf;
				}
				else
				{
					Console.Write("Failed to decode\n");
				}			
			}
			});
				
		}
		
		void onAvatarProperties(LLUUID id,libsecondlife.Avatar.AvatarProperties props)
		{
			//libsecondlife.Avatar.AvatarProperties props;
		Gtk.Application.Invoke(delegate {
				
			this.label_born.Text=props.BornOn;
			//this.label_partner.Text=props.Partner;
			partner_key=props.Partner;
			
			if(props.Online)
				this.label_status.Text="Online";
			else
				this.label_status.Text="Offline";
			
			if(props.Transacted)
				this.label_pay.Text="Pay info on file";
			else
				this.label_pay.Text="No";
			
			if(props.Identified)
				this.label_identified.Text="Yes";
			else
				this.label_identified.Text="No";
			
			this.textview2.Buffer.Text=props.AboutText;
				
			    this.textview3.Buffer.Text=props.FirstLifeText;
				
				profile_pic=props.ProfileImage;
				firstlife_pic=props.FirstLifeImage;
						
				if(profile_pic!=LLUUID.Zero)
					MainClass.client.Assets.RequestImage(profile_pic,ImageType.Normal);	
				
				if(firstlife_pic!=LLUUID.Zero)
					MainClass.client.Assets.RequestImage(firstlife_pic,ImageType.Normal);
				
				if(MainClass.av_names.ContainsKey(id))
				{
					this.label_name.Text=MainClass.av_names[id];
				}
				else
				{
					MainClass.client.Avatars.RequestAvatarName(id);
					this.label_name.Text="Waiting....";
				}
						
				if(props.Partner!=LLUUID.Zero)
				{	
				    if(MainClass.av_names.ContainsKey(partner_key))
					{
						this.label_partner.Text=MainClass.av_names[partner_key];
					}
					else
					{
						MainClass.client.Avatars.RequestAvatarName(partner_key);
						this.label_partner.Text="Waiting....";
					}
				}					
										
			});
			}	
	}
}
