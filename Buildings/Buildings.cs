using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace Entrogic
{
	public static class Buildings
	{
		static SortedDictionary<string, byte[]> _cache = new SortedDictionary<string, byte[]>();
		public static void Cache(params string[] names)
		{
			foreach (var item in names)
			{
				if (_cache.ContainsKey(item))
					_cache.Remove(item);
				_cache.Add(item, Entrogic.Instance.GetFileBytes(item));
			}
		}
		public static Chest FindAndCreateChest(Rectangle searchRange, int tileType)
		{
			var r = searchRange;
			for (int i = 0; i < r.Width; i++)
				for (int j = 0; j < r.Height; j++)
					if (Main.tile[r.X + i, r.Y + j].type == tileType)
						return Main.chest[Chest.CreateChest(r.X + i, r.Y + j)];
			return null;
		}

		public static Sign FindAndCreateSign(Rectangle searchRange, int tileType)
		{
			var r = searchRange;
			for (int i = 0; i < r.Width; i++)
				for (int j = 0; j < r.Height; j++)
					if (Main.tile[r.X + i, r.Y + j].type == tileType)
						return Main.sign[Sign.ReadSign(r.X + i, r.Y + j)];
			return null;
		}
		public static string BuildingSavePath => ModLoader.ModPath + "/TileFiles/";
		public static void Export(Vector2 start, Vector2 end)
		{
			Point starttc = start.ToTileCoordinates();
			Point endtc = end.ToTileCoordinates();
			Tile[,] toexport = new Tile[endtc.X - starttc.X + 1, endtc.Y - starttc.Y + 1];
			for (int i = 0; i < toexport.GetLength(0); i++)
			{
				for (int j = 0; j < toexport.GetLength(1); j++)
				{
					int wi = starttc.X + i;
					int wj = starttc.Y + j;
					toexport[i, j] = new Tile();
					toexport[i, j].CopyFrom(Main.tile[wi, wj]);
				}
			}
			using (FileStream fs = File.OpenWrite(BuildingSavePath + "1.buildings"))
			using (DeflateStream ds = new DeflateStream(fs, CompressionMode.Compress))
			using (BinaryWriter bw = new BinaryWriter(ds))
			{
				bw.Write(toexport.GetLength(0));
				bw.Write(toexport.GetLength(1));
				for (int i = 0; i < toexport.GetLength(0); i++)
				{
					for (int j = 0; j < toexport.GetLength(1); j++)
					{
						var from = toexport[i, j];
						bw.Write(from.type);
						if (TileLoader.GetTile(toexport[i, j].type) != null)
						{
							bw.Write(TileLoader.GetTile(toexport[i, j].type).Name);
						}
						else
						{
							bw.Write("");
						}
						bw.Write(from.wall);
						bw.Write(from.liquid);
						bw.Write(from.sTileHeader);
						bw.Write(from.bTileHeader);
						bw.Write(from.bTileHeader2);
						bw.Write(from.bTileHeader3);
						bw.Write(from.frameX);
						bw.Write(from.frameY);
					}
				}
			}
		}
		public static Rectangle Build(string name, Vector2 position, bool useAir = true)
		{
			using (MemoryStream stream = new MemoryStream(_cache[name]))
				return Build(stream, position, useAir);
		}
		public static Tile[,] Import(string name)
		{
			using (MemoryStream stream = new MemoryStream(_cache[name]))
				return Import(stream);
		}
		public static Rectangle Build(Stream fs, Vector2 position, bool useAir = true)
		{
			Tile[,] ts = Import(fs);
			Point starttc = position.ToTileCoordinates();
			for (int i = 0; i < ts.GetLength(0); i++)
			{
				for (int j = 0; j < ts.GetLength(1); j++)
				{
					int wi = starttc.X + i;
					int wj = starttc.Y + j;
					if ((ts[i, j].type != 0 && ts[i, j].active()) || useAir)
					{
<<<<<<< HEAD
						if (Main.tile[wi, wj] == null)
							Main.tile[wi, wj] = new Tile();
=======
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
						Main.tile[wi, wj].CopyFrom(ts[i, j]);
					}
				}
			}
			return new Rectangle(starttc.X, starttc.Y, ts.GetLength(0), ts.GetLength(1));
		}
		public static Tile[,] Import(Stream fs)
		{
			using (DeflateStream ds = new DeflateStream(fs, CompressionMode.Decompress))
			using (BinaryReader br = new BinaryReader(ds))
			{
				int x, y;
				x = br.ReadInt32();
				y = br.ReadInt32();
				Tile[,] ts = new Tile[x, y];
				for (int i = 0; i < x; i++)
				{
					for (int j = 0; j < y; j++)
					{
						var from = new Tile();
						from.type = br.ReadUInt16();
						var str = br.ReadString();
						if (str != "")
<<<<<<< HEAD
							from.type = (ushort)TileType(str);
=======
							from.type = (ushort)Entrogic.Instance.TileType(str);
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
						from.wall = br.ReadUInt16();
						from.liquid = br.ReadByte();
						from.sTileHeader = br.ReadUInt16();
						from.bTileHeader = br.ReadByte();
						from.bTileHeader2 = br.ReadByte();
						from.bTileHeader3 = br.ReadByte();
						from.frameX = br.ReadInt16();
						from.frameY = br.ReadInt16();
						ts[i, j] = from;
					}
				}
				return ts;
			}
		}
<<<<<<< HEAD

        private static ushort TileType(string str)
        {
			return Entrogic.ModTiles.Find(s => s.Name == str).Type;
		}
    }
=======
	}
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
}
