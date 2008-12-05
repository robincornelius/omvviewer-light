/*
omvviewerlight a Text based client to metaverses such as Linden Labs Secondlife(tm)
    Copyright (C) 2008  Robin Cornelius <robin.cornelius@gmail.com>

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
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
					Console.WriteLine("Failed to create a block of name "+blockname);
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
				//Console.WriteLine("pos is "+pos.ToString()+" lastpos is "+lastpos.ToString());
				string line=block.Substring(lastpos,pos-lastpos);
				string Value;
				
				if(line.IndexOf('{')!=-1)
				{
				   int stm;
				   Console.WriteLine("New block ("+key+")");
				   EmbeddedData new_block=addblock(key);
				   lastpos=new_block.parseblock(block,pos+1,out stm)+1;
				   pos=lastpos;
				   if(lastpos==0)
				   {
						Console.WriteLine("End found true");
						endfound=true;
						startoftext=stm;
						return -1;
				   }
				   continue;
				}
				   
				if(line.IndexOf('}')!=-1)
				{      
					Console.WriteLine("End block");
					startoftext=0;
					return pos;
				}
				
				//some keys have spaces, FFS
				int seperator=line.LastIndexOf("\t");
				if(seperator==-1)
					seperator=line.LastIndexOf(" ");
								
				char [] trim={' ','\t'};
				key=line.Substring(0,seperator);
				key=key.Trim(trim);
				Value=line.Substring(seperator);
				Value=Value.Trim(trim);
				this.keys.Add(key,Value);
				lastpos=pos+1;
				Console.WriteLine(key+" ---> "+Value);
			
				if(key.IndexOf("Text length")!=-1)
				{
					Console.WriteLine("Got text length marker its the end");
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

		EmbeddedData nd;	
		
		Dictionary <int,EmbeddedInventory> embedded_inv= new Dictionary <int,EmbeddedInventory>();
		Dictionary <Gtk.TextTag,EmbeddedInventory> iter_uuid_dict= new Dictionary <Gtk.TextTag,EmbeddedInventory>();

		~NotecardReader()
		{
			Console.WriteLine("NotecardReader Cleaned up");
		}		
		
		
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
			    Gtk.Application.Invoke(delegate{
					this.textview_notecard.Buffer.Text="Requesting asset, please wait....";
					this.entry_title.Text=ii.Name;
				});
				
				MainClass.client.Assets.RequestInventoryAsset(ii,true);				
			}
			else
			{
			    Gtk.Application.Invoke(delegate{
					this.textview_notecard.Buffer.Text="Opening embedded inventory, please wait....";
				});
				
				Console.WriteLine("Asset not in store, requesting directly");				
				Console.WriteLine("Notecard id "+this.target_id.ToString());
				Console.WriteLine("Item id "+item.ToString());
				Console.WriteLine("Folder id "+MainClass.client.Inventory.FindFolderForType(AssetType.Notecard));
				MainClass.client.Inventory.RequestCopyItemFromNotecard(UUID.Zero,this.target_id,MainClass.client.Inventory.FindFolderForType(AssetType.Notecard),item,itemcopiedcallback);
			}
		}
		
		void itemcopiedcallback(InventoryBase item)
		{
			Console.WriteLine("item copied callback "+item.UUID.ToString());
			
			    InventoryItem ii = (InventoryItem)MainClass.client.Inventory.Store[item.UUID];			
				target_asset=ii.AssetUUID;
				target_id=ii.UUID;
				Console.WriteLine("Asset id is "+ii.AssetUUID.ToString());
				Console.WriteLine("Id is "+ii.UUID.ToString());
			    Gtk.Application.Invoke(delegate{
				
			        this.textview_notecard.Buffer.Text="Requesting asset, please wait....";
				    this.entry_title.Text=ii.Name;
			});
			
				MainClass.client.Assets.RequestInventoryAsset(ii,true);
		}
		
		void onAsset(AssetDownload transfer,Asset asset)
        {
			Console.WriteLine("Asset retrieved id "+asset.AssetID.ToString());
			Console.WriteLine("target_asset"+this.target_asset.ToString());
			if(asset.AssetID!=target_asset)
				return;
			MainClass.client.Assets.OnAssetReceived -= new OpenMetaverse.AssetManager.AssetReceivedCallback(onAsset);
			shownote(asset);
		}
	
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
					
				this.textview_notecard.Buffer.Text=decode;

				int link_pos=0;

				while((link_pos=this.textview_notecard.Buffer.Text.IndexOf("\xdbc0",link_pos+2))!=-1)
				{
					char []gg=this.textview_notecard.Buffer.Text.Substring(link_pos+1,1).ToCharArray();
					int idx=gg[0];
					idx=idx-0xdc00; //check here for other inventory types
					Console.WriteLine("Found embedded index "+idx.ToString());
					EmbeddedInventory inventory;
						
					if(this.embedded_inv.TryGetValue(idx,out inventory))
				    {				
						Console.WriteLine("GOt embedded item "+inventory.name);
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
			    Console.WriteLine("CLick");
				Console.WriteLine(o.ToString());
				Gtk.TextTag tag=(Gtk.TextTag)o;
				EmbeddedInventory inventory;
				if(this.iter_uuid_dict.TryGetValue(tag,out inventory))
				{	
					if(inventory.assettype==AssetType.Notecard)
					{
						Console.Write("NEW UUID "+inventory.item_id.ToString());
						NotecardReader nr=new NotecardReader(inventory.item_id,this.target_id,this.notecard_item_id);
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
	}
}
