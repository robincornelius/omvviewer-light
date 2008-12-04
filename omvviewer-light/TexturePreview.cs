// TexturePreview.cs created with MonoDevelop
// User: robin at 09:03 04/12/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using OpenMetaverse;
using Gdk;
using Gtk;

namespace omvviewerlight
{
	public partial class TexturePreview : Gtk.Window
	{

		UUID target_asset;
		UUID target_id;
		
		public TexturePreview(UUID texture,string title,bool isasset) : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			this.label_title.Text=title;
			if(isasset)
			{
					MainClass.client.Assets.OnAssetReceived += new OpenMetaverse.AssetManager.AssetReceivedCallback(onAsset);
		            if (MainClass.client.Inventory.Store.Contains(texture))
		            {
		                // retrieve asset from store
		                Console.WriteLine("asset in store, requesting");            
						InventoryItem ii = (InventoryItem)MainClass.client.Inventory.Store[texture];			
						target_asset=ii.AssetUUID;
						target_id=ii.UUID;
						Console.WriteLine("Asset id is "+ii.AssetUUID.ToString());
						Console.WriteLine("Id is "+ii.UUID.ToString());
//					    Gtk.Application.Invoke(delegate{
//							this.textview_notecard.Buffer.Text="Requesting asset, please wait....";
//							this.entry_title.Text=ii.Name;
//						});
						
					MainClass.client.Assets.RequestInventoryAsset(ii,true);				
				}
			}
			else
			{
				TryGetImage tg= new TryGetImage(image,texture);
			}
		}
		
		void onAsset(AssetDownload transfer,Asset asset)
        {
			Console.WriteLine("Asset retrieved id "+asset.AssetID.ToString());
			Console.WriteLine("target_asset"+this.target_asset.ToString());
			if(asset.AssetID!=target_asset)
				return;
			MainClass.client.Assets.OnAssetReceived -= new OpenMetaverse.AssetManager.AssetReceivedCallback(onAsset);
			TryGetImage tg= new TryGetImage(this.image,asset.AssetID);
		}		
	}
}
