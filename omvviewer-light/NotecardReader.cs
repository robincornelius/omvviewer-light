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

// NotecardReader.cs created with MonoDevelop
// User: robin at 17:19 15/11/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using OpenMetaverse;
using OpenMetaverse.Assets;
using System.Collections.Generic;
using System.IO;
using Gtk;
using Gdk;

namespace omvviewerlight
{
	public class EmbeddedData
	{
		public Dictionary <string,string> keys= new Dictionary <string,string>();
		public Dictionary <string,EmbeddedData> blocks = new Dictionary <string,EmbeddedData>();
		public List <EmbeddedData> unamed_blocks = new List <EmbeddedData>();
		bool endfound=false;
		
		public EmbeddedData()
		{
			//Nothing exciting to do
		}
		
		EmbeddedData addblock(string blockname)
		{
			EmbeddedData new_block=new EmbeddedData();
			if(blockname=="count")
			{
				unamed_blocks.Add(new_block);
			}
			else
			{
				if(blocks.ContainsKey(blockname))
				{
					Logger.Log("Failed to create a block of name "+blockname,Helpers.LogLevel.Debug);
				}
				else
				{
					blocks.Add(blockname,new_block);
				}
			}			

			return new_block;
		}

		public int parsedata(string data)
		{
			int startoftext;
			parseblock(data,0,out startoftext);
			return startoftext;
		}
		
		bool getblock(string blockname, out	EmbeddedData gotblock)
		{
			EmbeddedData block;

			if(blocks.TryGetValue(blockname,out block))
			{
				gotblock=block;
				return true;
			}
			gotblock=null;
			return false;
		}

		public int parseblock(string block,int pos ,out int startoftext)
		{
			int lastpos=pos;
			string key="";
			
			
			while((pos=block.IndexOf("\n",pos+1))!=-1)
			{
				if(endfound==true)
				    break;
				
				//Find each line by looking for the newline
				//Logger.Log("pos is "+pos.ToString()+" lastpos is "+lastpos.ToString(),Helpers.LogLevel.Debug);
				string line=block.Substring(lastpos,pos-lastpos);
				string Value;
				
				if(line.IndexOf('{')!=-1)
				{
				   int stm;
				   Logger.Log("New block ("+key+")",Helpers.LogLevel.Debug);
				   EmbeddedData new_block=addblock(key);
				   lastpos=new_block.parseblock(block,pos+1,out stm)+1;
				   pos=lastpos;
				   if(lastpos==0)
				   {
						Logger.Log("End found true",Helpers.LogLevel.Debug);
						endfound=true;
						startoftext=stm;
						return -1;
				   }
				   continue;
				}
				   
				if(line.IndexOf('}')!=-1)
				{      
					Logger.Log("End block",Helpers.LogLevel.Debug);
					startoftext=0;
					return pos;
				}
				
				//some keys have spaces, FFS
				int seperator=line.LastIndexOf("\t");
				if(seperator==-1)
					seperator=line.LastIndexOf(" ");
				
				if(seperator==-1)
					continue; //opensim extra workaround
								
				char [] trim={' ','\t'};
				key=line.Substring(0,seperator);
				key=key.Trim(trim);
				Value=line.Substring(seperator);
				Value=Value.Trim(trim);
				this.keys.Add(key,Value);
				lastpos=pos+1;
				Logger.Log(key+" ---> "+Value,Helpers.LogLevel.Debug);
			
				if(key.IndexOf("Text length")!=-1)
				{
					Logger.Log("Got text length marker its the end",Helpers.LogLevel.Debug);
					endfound=true;
					startoftext=pos;
					return -1;			
				}
			}
			startoftext=0;
			return -1;
		}				
	}
	
	public class EmbeddedInventory
	{
		public UUID asset_id;
		public UUID parent_id;
		public UUID item_id;
		
		public int index;
		public string name;
		public AssetType assettype;
		public EmbeddedInventory(UUID asset_idIN,UUID item_idIN,UUID parent_idIN,int indexIN,string nameIN,AssetType typeIN)
		{
			asset_id=asset_idIN;
			parent_id=parent_idIN;
			item_id=item_idIN;
			index=indexIN;
			name=nameIN;
			assettype=typeIN;
		}
	}
	
	public partial class NotecardReader : Gtk.Window
	{
		UUID target_asset;
		UUID target_id;
		UUID notecard_item_id;
		public UUID transfer_id;
		EmbeddedData nd;
		AssetType asset_type;
		
		Dictionary <int,EmbeddedInventory> embedded_inv= new Dictionary <int,EmbeddedInventory>();
		Dictionary <Gtk.TextTag,EmbeddedInventory> iter_uuid_dict= new Dictionary <Gtk.TextTag,EmbeddedInventory>();

		~NotecardReader()
		{
			Logger.Log("NotecardReader Cleaned up",Helpers.LogLevel.Debug);
		}		
		
		
		public NotecardReader(UUID item,UUID target,UUID notecard_item_id_in) : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
						
			this.target_id=target;
			this.notecard_item_id=notecard_item_id_in;
			
            //FIX ME
			//MainClass.client.Assets.OnAssetReceived += new OpenMetaverse.AssetManager.AssetReceivedCallback(onAsset);
		

			// verify asset is loaded in store;
            if (MainClass.client.Inventory.Store.Contains(item))
            {
                // retrieve asset from store
                Logger.Log("asset in store, requesting",Helpers.LogLevel.Debug);            
				InventoryItem ii = (InventoryItem)MainClass.client.Inventory.Store[item];
				
				if(ii is InventoryLSL)
					asset_type=AssetType.LSLText;
				if(ii is InventoryNotecard)
					asset_type=AssetType.Notecard;
				
				target_asset=ii.AssetUUID;
				target_id=ii.UUID;
				Logger.Log("Asset id is "+ii.AssetUUID.ToString(),Helpers.LogLevel.Debug);
				Logger.Log("Id is "+ii.UUID.ToString(),Helpers.LogLevel.Debug);
			    Gtk.Application.Invoke(delegate{
					this.textview_notecard.Buffer.Text="Requesting asset, please wait....";
					this.entry_title.Text=ii.Name;
				});
				
                MainClass.client.Assets.RequestInventoryAsset(ii,true,new AssetManager.AssetReceivedCallback(this.asset_recieved));
				Logger.Log("transfer Id is "+transfer_id,Helpers.LogLevel.Debug);
			}
			else
			{
			    Gtk.Application.Invoke(delegate{
					this.textview_notecard.Buffer.Text="Opening embedded inventory, please wait....";
				});
				
				Logger.Log("Asset not in store, requesting directly",Helpers.LogLevel.Debug);				
				Logger.Log("Notecard id "+this.target_id.ToString(),Helpers.LogLevel.Debug);
				Logger.Log("Item id "+item.ToString(),Helpers.LogLevel.Debug);
				Logger.Log("Folder id "+MainClass.client.Inventory.FindFolderForType(AssetType.Notecard),Helpers.LogLevel.Debug);
				MainClass.client.Inventory.RequestCopyItemFromNotecard(UUID.Zero,this.target_id,MainClass.client.Inventory.FindFolderForType(AssetType.Notecard),item,itemcopiedcallback);
			}
		}
		
		void itemcopiedcallback(InventoryBase item)
		{
			Logger.Log("item copied callback "+item.UUID.ToString(),Helpers.LogLevel.Debug);
			
			    InventoryItem ii = (InventoryItem)MainClass.client.Inventory.Store[item.UUID];			
				target_asset=ii.AssetUUID;
				target_id=ii.UUID;
				Logger.Log("Asset id is "+ii.AssetUUID.ToString(),Helpers.LogLevel.Debug);
				Logger.Log("Id is "+ii.UUID.ToString(),Helpers.LogLevel.Debug);
			    Gtk.Application.Invoke(delegate{
				
			        this.textview_notecard.Buffer.Text="Requesting asset, please wait....";
				    this.entry_title.Text=ii.Name;
			});
			
                MainClass.client.Assets.RequestInventoryAsset(ii, true, new AssetManager.AssetReceivedCallback(this.asset_recieved));
	
        }
		
        void asset_recieved(AssetDownload transfer, Asset asset)
        {
            Logger.Log("Asset retrieved id " + asset.AssetID.ToString(),Helpers.LogLevel.Debug);
            Logger.Log("target_asset" + this.target_asset.ToString(),Helpers.LogLevel.Debug);
            if (transfer_id != transfer.ID)
                return;
            shownote(asset);

        }
	
		void shownote(Asset asset)
		{
			Gtk.Application.Invoke(delegate{

				if(asset==null || asset.AssetData==null)
					{
						this.textview_notecard.Buffer.Text="Asset transfer failed";
						Logger.Log("No asset data",Helpers.LogLevel.Debug);
						return;
					}
					
				if(target_asset==UUID.Zero)
				{
					this.textview_notecard.Buffer.Text=Utils.BytesToString(asset.AssetData);
					return;
				}
				
				
				string decode=decodenote(Utils.BytesToString(asset.AssetData));
					
				
				this.textview_notecard.Buffer.Text=decode;

				int link_pos=0;

				while((link_pos=this.textview_notecard.Buffer.Text.IndexOf("\xdbc0",link_pos+2))!=-1)
				{
					char []gg=this.textview_notecard.Buffer.Text.Substring(link_pos+1,1).ToCharArray();
					int idx=gg[0];
					idx=idx-0xdc00; //check here for other inventory types
					Logger.Log("Found embedded index "+idx.ToString(),Helpers.LogLevel.Debug);
					EmbeddedInventory inventory;
						
					if(this.embedded_inv.TryGetValue(idx,out inventory))
				    {				
						Logger.Log("GOt embedded item "+inventory.name,Helpers.LogLevel.Debug);
						Gtk.TextTag link_tag=new Gtk.TextTag("link"+idx.ToString());
						link_tag.ForegroundGdk=new Gdk.Color(0,0,255);
						link_tag.Underline=Pango.Underline.Single;
						link_tag.TextEvent+=new TextEventHandler(onTextEvent);
						this.textview_notecard.Buffer.TagTable.Add(link_tag);
							
						TextIter start=this.textview_notecard.Buffer.GetIterAtOffset(link_pos);
						TextIter end=this.textview_notecard.Buffer.GetIterAtOffset(link_pos+1);
						TextMark markstart = textview_notecard.Buffer.CreateMark("start", start, true);
						TextMark markend = textview_notecard.Buffer.CreateMark("end", end, true);

						this.textview_notecard.Buffer.Delete(start,end);
						
						start=this.textview_notecard.Buffer.GetIterAtMark(markstart);
						end=this.textview_notecard.Buffer.GetIterAtMark(markend);
							
						this.textview_notecard.Buffer.InsertWithTags(start,inventory.name+" ",link_tag);
						start=this.textview_notecard.Buffer.GetIterAtMark(markstart);
						end=this.textview_notecard.Buffer.GetIterAtMark(markend);
							
						this.textview_notecard.Buffer.ApplyTag(link_tag,start,end);	

						this.iter_uuid_dict.Add(link_tag,inventory);
					
					}
				}
								
			});
		}
		
		void onTextEvent(object o, TextEventArgs args)
		{
			if(args.Event.Type==Gdk.EventType.ButtonPress)
			{
			    Logger.Log("CLick",Helpers.LogLevel.Debug);
				Logger.Log(o.ToString(),Helpers.LogLevel.Debug);
				Gtk.TextTag tag=(Gtk.TextTag)o;
				EmbeddedInventory inventory;
				if(this.iter_uuid_dict.TryGetValue(tag,out inventory))
				{	
					if(inventory.assettype==AssetType.Notecard)
					{
						Console.Write("NEW UUID "+inventory.item_id.ToString());
						new NotecardReader(inventory.item_id,this.target_id,this.notecard_item_id);
					}
					if(inventory.assettype==AssetType.Landmark)
					{
						Console.Write("New landmark");
						TeleportProgress tp=new TeleportProgress();
						tp.teleportassetid(inventory.asset_id,inventory.name);
					}
					
				}
			}			
		}
		
		string decodenote(string note)
		{
			nd=new EmbeddedData();
			int stm=nd.parsedata(note);				
			int index=0;
		
			//NOTE opensim brokenness here
			if(nd.blocks.ContainsKey("Linden text version"))
			{
			foreach(EmbeddedData data in nd.blocks["Linden text version"].blocks["LLEmbeddedItems version"].unamed_blocks)
			{
					UUID id=new UUID(data.blocks["inv_item"].keys["item_id"]);
					UUID asset_id=new UUID(data.blocks["inv_item"].keys["asset_id"]);
				    char [] trim={'|'};
					string name=" "+data.blocks["inv_item"].keys["name"].TrimEnd(trim);
				    string assettype=data.blocks["inv_item"].keys["type"];
				    AssetType type=AssetType.Unknown;
				    if(assettype=="notecard")
					    type=AssetType.Notecard;
				    if(assettype=="landmark")
					    type=AssetType.Landmark;
								
					EmbeddedInventory item=new EmbeddedInventory(asset_id,id,UUID.Zero,index,name,type);			
					embedded_inv.Add(index,item);		
					index++;
			}

			return(note.Substring(stm,(note.Length-stm)-2)); //loose 2 bytes to remove the closing }
			}
			else
			{
				return note;
			}
		}		
		protected virtual void OnButtonSaveClicked (object sender, System.EventArgs e)
		{
			button_save.Sensitive=false;

			if(asset_type==AssetType.Notecard)
			{
                OpenMetaverse.Assets.AssetNotecard nd = new OpenMetaverse.Assets.AssetNotecard(this.textview_notecard.Buffer.Text);
				nd.Encode();	
				MainClass.client.Inventory.RequestUploadNotecardAsset(nd.AssetData,this.target_id,new InventoryManager.InventoryUploadedAssetCallback(OnNoteUpdated));
			}
			
			if(asset_type==AssetType.LSLText)
			{
                OpenMetaverse.Assets.AssetScriptText nd = new OpenMetaverse.Assets.AssetScriptText(this.textview_notecard.Buffer.Text);
				nd.Encode();
                MainClass.client.Inventory.RequestUploadNotecardAsset(nd.AssetData, this.target_id, new InventoryManager.InventoryUploadedAssetCallback(OnNoteUpdated));				
			}
			
		}
			
		void OnNoteUpdated(bool success,string status,UUID item_uuid, UUID asset_uuid)
		{
			Logger.Log("Notecard uploaded",Helpers.LogLevel.Debug);
			Logger.Log("BOOL = "+success.ToString()+" status = "+status+" item ID is "+item_uuid.ToString()+" Asset UUID is "+asset_uuid.ToString(),Helpers.LogLevel.Debug);
			button_save.Sensitive=true;

		}
	}
}
