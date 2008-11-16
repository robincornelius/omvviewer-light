// NotecardReader.cs created with MonoDevelop
// User: robin at 17:19 15/11/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using OpenMetaverse;
using System.Collections.Generic;
using System.IO;
using Gtk;
using Gdk;

namespace omvviewerlight
{
	public class EmbeddedInventory
	{
		public UUID asset_id;
		public UUID parent_id;
		public UUID item_id;
		int index;
		public EmbeddedInventory(UUID asset_idIN,UUID item_idIN,UUID parent_idIN,int indexIN)
		{
			asset_id=asset_idIN;
			parent_id=parent_idIN;
			item_id=item_idIN;
			index=indexIN;
		}
	}
	
	public partial class NotecardReader : Gtk.Window
	{
		UUID target_asset;
		UUID target_id;
		UUID notecard_item_id;
		Dictionary <Gtk.TextTag,UUID> iter_uuid_dict= new Dictionary <Gtk.TextTag,UUID>();
				
		public NotecardReader(UUID item,UUID target,UUID notecard_item_id_in) : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
						
			this.target_id=target;
			this.notecard_item_id=notecard_item_id_in;
			
			MainClass.client.Assets.OnAssetReceived += new OpenMetaverse.AssetManager.AssetReceivedCallback(onAsset);
			
			// verify asset is loaded in store;
            if (MainClass.client.Inventory.Store.Contains(item))
            {
                // retrieve asset from store
                Console.WriteLine("asset in store, requesting");            
				InventoryItem ii = (InventoryItem)MainClass.client.Inventory.Store[item];			
				target_asset=ii.AssetUUID;
				target_id=ii.UUID;
				Console.WriteLine("Asset id is "+ii.AssetUUID.ToString());
				Console.WriteLine("Id is "+ii.UUID.ToString());
				this.textview_notecard.Buffer.Text="Requesting asset, please wait....";
				
				MainClass.client.Assets.RequestInventoryAsset(ii,true);
			}
			else
			{
				this.textview_notecard.Buffer.Text="Requesting asset, please wait....";
				Console.WriteLine("Asset not in store, requesting directly");				
				Console.WriteLine("Notecard id "+this.target_id.ToString());
				Console.WriteLine("Item id "+item.ToString());
				Console.WriteLine("Folder id "+MainClass.client.Inventory.FindFolderForType(AssetType.Notecard));
				MainClass.client.Inventory.RequestCopyItemFromNotecard(UUID.Zero,this.target_id,MainClass.client.Inventory.FindFolderForType(AssetType.Notecard),item);
			}
		}
			
		void onAsset(AssetDownload transfer,Asset asset)
        {
			Console.WriteLine("Asset retrieved id "+asset.AssetID.ToString());
			Console.WriteLine("Transfer ID "+transfer.ID.ToString());
			Console.WriteLine("Transfer AssetID "+transfer.AssetID.ToString());
			
		
			//if(asset.AssetID!=target_asset);
				//   return;
			Console.WriteLine("Showing note");

			shownote(asset);
		}
	
//		void shownote(string decode)
		void shownote(Asset asset)
		{
			Gtk.Application.Invoke(delegate{

			if(asset==null || asset.AssetData==null)
				{
					this.textview_notecard.Buffer.Text="Asset transfer failed";
					Console.WriteLine("No asset data");
					return;
				}
				
			string decode=decodenote(Utils.BytesToString(asset.AssetData));
				
			Console.WriteLine("Displaying note");
				
			this.textview_notecard.Buffer.Text=decode;

			Console.WriteLine("Doing buffer find replace");

			//	Gtk.TextTag
			int link_pos=0;
			int counter=0;
			while((link_pos=this.textview_notecard.Buffer.Text.IndexOf("notecard://",link_pos+1))!=-1)
			{
				int mid_link_pos=this.textview_notecard.Buffer.Text.IndexOf("\":",link_pos);					
				int end_link_pos=this.textview_notecard.Buffer.Text.IndexOf(" ",mid_link_pos);
				int namestart=link_pos+12;
				int nameend=mid_link_pos;
					
				string name=this.textview_notecard.Buffer.Text.Substring(namestart,nameend-namestart);
				string sUUID=this.textview_notecard.Buffer.Text.Substring(nameend+2,36);
				UUID theuuid=new UUID(sUUID);
					
				Console.WriteLine("Name is "+name+" UUID is "+sUUID);
				Console.WriteLine("link at "+link_pos.ToString()+" "+mid_link_pos.ToString()+" "+end_link_pos.ToString());	
					
				Gtk.TextTag link_tag=new Gtk.TextTag("link"+counter.ToString());
				link_tag.ForegroundGdk=new Gdk.Color(0,0,255);
				link_tag.Underline=Pango.Underline.Single;
				link_tag.TextEvent+=new TextEventHandler(onTextEvent);
				this.textview_notecard.Buffer.TagTable.Add(link_tag);
					
				TextIter start=this.textview_notecard.Buffer.GetIterAtOffset(link_pos);
				TextIter end=this.textview_notecard.Buffer.GetIterAtOffset(end_link_pos);
				TextMark markstart = textview_notecard.Buffer.CreateMark("xyz", start, true);
				TextMark markend = textview_notecard.Buffer.CreateMark("xyz2", end, true);
					
				this.textview_notecard.Buffer.Delete(start,end);
				
				start=this.textview_notecard.Buffer.GetIterAtMark(markstart);
				end=this.textview_notecard.Buffer.GetIterAtMark(markend);
					
				this.textview_notecard.Buffer.InsertWithTags(start,name,link_tag);
				start=this.textview_notecard.Buffer.GetIterAtMark(markstart);
				end=this.textview_notecard.Buffer.GetIterAtMark(markend);
					
				this.textview_notecard.Buffer.ApplyTag(link_tag,start,end);	
			
				this.iter_uuid_dict.Add(link_tag,theuuid);
				counter++;
			}
			});
		}
		
		void onTextEvent(object o, TextEventArgs args)
		{
			//Console.WriteLine("TextEvent " +args.Event.Type.ToString());
			if(args.Event.Type==Gdk.EventType.ButtonPress)
			{
			    Console.WriteLine("CLick");
				Console.WriteLine(o.ToString());
				Gtk.TextTag tag=(Gtk.TextTag)o;
				UUID theuuid;
				if(this.iter_uuid_dict.TryGetValue(tag,out theuuid))
				{
					Console.Write("NEW UUID "+theuuid.ToString());
					NotecardReader nr=new NotecardReader(theuuid,this.target_id,this.notecard_item_id);
				}
			}			
		}
		
		string decodenote(string note)
		{
			int depth=0;
			bool firstfound=true;
			int openingbck=note.IndexOf("{");
			CharEnumerator cn=note.GetEnumerator();
			int pos=0;
			
			List <int> opentoken=new List <int>();
			List <int> closetoken=new List <int>();
			
			while(cn.MoveNext()==true)
			{
				if(cn.Current=='{')
				{
//					Console.WriteLine("Found { at "+pos.ToString());
					opentoken.Add(pos);
					depth++;
				}
				
				if(cn.Current=='}')
				{
//					Console.WriteLine("Found } at "+pos.ToString());
					closetoken.Add(pos);
					depth--;
				}								
				pos++;
			}
			
			string version=note.Substring(0,opentoken[0]);
			
//			Console.WriteLine("Close -2 is "+closetoken[closetoken.Count-2].ToString());
//			Console.WriteLine("Close -1 is "+closetoken[closetoken.Count-1].ToString());
//			Console.WriteLine("Diff is "+(note.Length-closetoken[closetoken.Count-1]).ToString());
			
			string embedded=note.Substring(0,closetoken[closetoken.Count-2]);
			
			string bodyx=note.Substring(closetoken[closetoken.Count-2]+2);
			
//			Console.WriteLine("Version is "+version);
			
//			Console.WriteLine("end of length is "+(bodyx.Substring(2,bodyx.IndexOf('\n')).ToString()));
			string length=bodyx.Substring(0,bodyx.IndexOf('\n'));
			string body=bodyx.Substring(bodyx.IndexOf('\n'),(bodyx.Length-bodyx.IndexOf('\n'))-2);
//			Console.WriteLine("Length string is "+length);
//			Console.WriteLine("Body is "+body);
			
			pos=0;
			
			List <string> embedded_names=new List<string>();
			List <UUID> embedded_asset=new List<UUID>();
			List <int> embedded_asset_index=new List <int>();
			//List <int> embedded_asset_pos
			
			// Now find the embedded notes;
			while((pos=embedded.IndexOf("ext char index",pos+1))!=-1)
			{
				
//				Console.WriteLine("POS IS "+pos.ToString());
				int nl=embedded.IndexOf("\n",pos);
				string indexer=embedded.Substring(pos,nl-pos);
//				Console.WriteLine("Line is "+indexer+" Length is "+indexer.Length.ToString());
				indexer=indexer.Substring(14);
				int index;
				int.TryParse(indexer,out index);
//				Console.WriteLine("Index is "+index.ToString());
				
				//Now find the name and assetid of that
				int namepos=embedded.IndexOf("name",pos);
				int nameend=embedded.IndexOf("|",namepos);
//				Console.WriteLine("namestart "+namepos.ToString()+" nameend is "+nameend.ToString());
				string name=embedded.Substring(namepos+5,(nameend-namepos)-5);
//				Console.WriteLine("Name is "+name+" Index is "+index.ToString());

				int assetpos=embedded.IndexOf("item_id",pos);
				int assetposend=embedded.IndexOf("\n",assetpos);
				string asset=embedded.Substring(assetpos+8,(assetposend-assetpos)-8);

				
//			    int assetpos=embedded.IndexOf("asset_id",pos);
//				int assetposend=embedded.IndexOf("\n",assetpos);
//				string asset=embedded.Substring(assetpos+9,(assetposend-assetpos)-9);
				
				embedded_names.Add(name);
				UUID id=new UUID(asset);
				embedded_asset.Add(id);
				
			}
			
			// Now find replace the binary markers
			// F4 80 80 80 is marker 0  // 80 80 81 is marker 1
			Console.WriteLine("There are "+embedded_asset.Count.ToString()+" markers");
			int posx=0;
			
			for(int x=0;x<embedded_asset.Count;x++)
			{
				char g='\xdbc0'; //next is dc00 + index
				posx=body.IndexOf(g,posx+1);
				CharEnumerator nc=body.GetEnumerator();
				char []gg=body.Substring(posx+1,1).ToCharArray();
				int idx=gg[0];
				int idx2=idx-0xdc00;
//				Console.WriteLine("Pos is at "+posx.ToString()+ "index is "+idx2.ToString());
				embedded_asset_index.Add(idx2);
				string find=new string(gg,0,1);
				body=body.Replace(find," notecard://\""+embedded_names[idx2]+"\":"+embedded_asset[idx2]+" ");
			}

			    body=body.Replace("\xdbc0","");

			return body;
		}
	}
}
