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
		public ProfileVIew(LLUUID key) : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			MainClass.client.Avatars.OnAvatarProperties += new libsecondlife.AvatarManager.AvatarPropertiesCallback(onAvatarProperties);
			MainClass.client.Avatars.RequestAvatarProperties(key);
			MainClass.client.Assets.OnImageReceived += new libsecondlife.AssetManager.ImageReceivedCallback(onGotImage);
			MainClass.client.Avatars.OnAvatarNames += new libsecondlife.AvatarManager.AvatarNamesCallback(on_avnames);
			resident=key;
			
			this.label_born.Text="";
			this.label_identified.Text="";
			this.label_name.Text="";
		    this.label_partner.Text="";
			this.label_pay.Text="";
			this.label_status.Text="";	
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
		
		void onGotImage(ImageDownload image,AssetTexture asset)
		{
			
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
					
					byte[] data = new byte[height*width*channels];
					
					for(int x=0;x<length;x=x+4)
					{
						data[x]=asset.Image.Red[x];
						data[x+1]=asset.Image.Green[x+1];
						data[x+2]=asset.Image.Blue[x+2];
						data[x+3]=asset.Image.Alpha[x+3];
					}
						
								
				}
				else
				{
					Console.Write("Failed to decode\n");
				}
			}
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
