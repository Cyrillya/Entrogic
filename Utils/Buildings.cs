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
                    if ((ts[i, j].type != 0 && ts[i,j].active()) || useAir)
                    {
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
							from.type = (ushort)Entrogic.Instance.TileType(str);
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
	}
}
