using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Terraria;
using Terraria.ModLoader;

namespace Entrogic
{
	public static class ResourceLoader
	{
		public static int OldGlowMasksLength;

		public static void LoadAllTextures()
		{
			try
			{
				Entrogic.ModTexturesTable.Clear();
				LoadTextures();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}
        private static void LoadTexture(string name)
        {
            Entrogic.ModTexturesTable.Add(name.Substring(7), Entrogic.Instance.GetTexture(name));

            //string directPath = $"{Entrogic.ModFolder}TextureLoad{Path.DirectorySeparatorChar}";
            //Directory.CreateDirectory(directPath);
            //string path = $"{directPath}{name.Substring(7)}.xnb";
            //File.Create(path);
        }
		private static void LoadTextures()
		{
			IDictionary<string, Texture2D> textures = (IDictionary<string, Texture2D>)typeof(Mod).GetField("textures",
				System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(Entrogic.Instance);

			var names = textures.Keys.Where((name) =>
			{
				return name.StartsWith("Images/");
			});
			foreach (var name in names)
				LoadTexture(name);
		}
	}
}
