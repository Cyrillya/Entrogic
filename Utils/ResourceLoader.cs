using Entrogic.Items.Weapons.Card.Elements;
using Entrogic.Items.Weapons.Card.Organisms;
using Entrogic.NPCs.Boss.AntaGolem;
using Entrogic.NPCs.Boss.PollutElement;
using Entrogic.NPCs.CardMerchantSystem;

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
using Terraria.ID;
using Terraria.Audio;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using Terraria.ModLoader.Assets;

using static Terraria.ModLoader.ModContent;

namespace Entrogic
{
	public static class ResourceLoader
	{
		// 如果variations不为0，则选取的文件为XXX{variations}
		public static readonly SoundStyle CGChangeTurn = new ModSoundStyle("Entrogic", "Assets/Sounds/UI/CGChangeTurn", 0);
		public static readonly SoundStyle CGHurt = new ModSoundStyle("Entrogic", "Assets/Sounds/UI/CGHurt", 0);
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

		private static void AddIntoTextureTable(string name)
		{
			Entrogic.ModTexturesTable.Add(name, Entrogic.Instance.GetTexture($"Assets/Images/{name}"));
		}
		private static void LoadTextures()
		{
			// 多次尝试无果，决定使用最笨方式
			AddIntoTextureTable("Block");
			AddIntoTextureTable("CardFightPlayer");
			AddIntoTextureTable("CardFightPlayer_Bar");
			AddIntoTextureTable("CardFightPlayer_FightPanel");
			AddIntoTextureTable("CardFightPlayer_Icon_Blue");
			AddIntoTextureTable("CardFightPlayer_Icon_Default");
			AddIntoTextureTable("CardFightPlayer_Icon_LightBlue");
			AddIntoTextureTable("CardFightPlayer_Icon_Mushroom");
			AddIntoTextureTable("CardFightPlayer_Icon_Slime");
			AddIntoTextureTable("CardFightPlayer_Surface");
			AddIntoTextureTable("Frozen");
			AddIntoTextureTable("GelSym_HeadTextures");
			AddIntoTextureTable("GelSym_Textures");
			AddIntoTextureTable("HookCursor");
			AddIntoTextureTable("Logo");
			AddIntoTextureTable("Logo2");
			AddIntoTextureTable("LogoCrimson");
			AddIntoTextureTable("PollutionArmorSetEffect");
			AddIntoTextureTable("ReadingBubble");
			AddIntoTextureTable("Slime");
			AddIntoTextureTable("Star_0");
			AddIntoTextureTable("Star_1");
			AddIntoTextureTable("Star_2");
			AddIntoTextureTable("Star_3");
			AddIntoTextureTable("Star_4");
			AddIntoTextureTable("TeleportIcon");
			AddIntoTextureTable("WhiteBackground");
			AddIntoTextureTable("凝胶安卡");
			AddIntoTextureTable("拟态魔能_1");
			AddIntoTextureTable("拟态魔能_10");
			AddIntoTextureTable("拟态魔能_11");
			AddIntoTextureTable("拟态魔能_12");
			AddIntoTextureTable("拟态魔能_2");
			AddIntoTextureTable("拟态魔能_3");
			AddIntoTextureTable("拟态魔能_4");
			AddIntoTextureTable("拟态魔能_5");
			AddIntoTextureTable("拟态魔能_6");
			AddIntoTextureTable("拟态魔能_7");
			AddIntoTextureTable("拟态魔能_8");
			AddIntoTextureTable("拟态魔能_9");

			//IDictionary<string, Texture2D> textures = (IDictionary<string, Texture2D>)typeof(Mod).GetField("textures",
			//	System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(Entrogic.Instance);
			//var names = textures.Keys.Where((name) =>
			//{
			//	return name.StartsWith("Images/");
			//});
			//foreach (var name in names)
			//	LoadTexture(name);

			// 1 加载文件
			//StreamReader sr = new StreamReader(Entrogic.Instance.GetFileStream("Images/images.txt"));
			//string line; // 2 调用StreamReader的ReadLine()函数
			//while ((line = sr.ReadLine()) != null)
			//{
			// 3 读取相应文件
			//	LoadTexture($"Images/{line.Split('.')[0]}");
			//}
			// 4 关闭流
			//sr.Close();
		}
		public static void LoadAllCardMissions()
		{
			try
			{
				Entrogic.CardQuests.Clear();
				LoadMissions();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}
		private static void LoadMissions()
		{
			// 下面这行参数从左到右分别是：任务文本, 相关Boolean, 感谢词
			Quest quest = new ZeroQuest();
			Entrogic.CardQuests.Add(quest);
			quest = new NoviceCard();
			Entrogic.CardQuests.Add(quest);
		}
		public static void LoadAllShaders()
		{
			try
			{
				WhiteBlur = null;
				LoadShaders();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}
		public static ShaderData WhiteBlur;
		private static void LoadShaders()
		{
			Filters.Scene["Entrogic:RainyDaysScreen"] = new Filter(new PollutionElementalScreenShaderData("FilterMiniTower").UseColor(0.2f, 0.2f, 0.4f).UseOpacity(0.3f), EffectPriority.VeryHigh);
			SkyManager.Instance["Entrogic:RainyDaysScreen"] = new RainyDaysScreen();
			Filters.Scene["Entrogic:GrayScreen"] = new Filter(new AthanasyScreenShaderData("FilterMiniTower").UseColor(0.2f, 0.2f, 0.2f).UseOpacity(0.7f), EffectPriority.High);
			SkyManager.Instance["Entrogic:GrayScreen"] = new GrayScreen();
			Filters.Scene["Entrogic:MagicStormScreen"] = new Filter(new ScreenShaderData("FilterBloodMoon").UseColor(-0.4f, -0.2f, 1.6f).UseOpacity(0.6f), EffectPriority.Medium);
			SkyManager.Instance["Entrogic:MagicStormScreen"] = new MagicStormScreen();
			GameShaders.Misc["ExampleMod:DeathAnimation"] = new MiscShaderData(new Ref<Effect>((Effect)Entrogic.Instance.GetEffect("Assets/Effects/ExampleEffectDeath")), "DeathAnimation").UseImage0("Images/Misc/Perlin");
			//GameShaders.Misc["Entrogic:PixelShader"] = new MiscShaderData(new Ref<Effect>(GetEffect("Effects/PixelShader")), "PixelShader");
			// First, you load in your shader file.
			// You'll have to do this regardless of what kind of shader it is,
			// and you'll have to do it for every shader file.
			// This example assumes you have screen shaders.
			Ref<Effect> screenRef = new Ref<Effect>(Entrogic.Instance.GetEffect("Assets/Effects/IceScreen").Value);
			Filters.Scene["Entrogic:IceScreen"] = new Filter(new ScreenShaderData(screenRef, "IceScreen"), EffectPriority.High);
			Filters.Scene["Entrogic:IceScreen"].Load();
			Ref<Effect> screenRef2 = new Ref<Effect>(Entrogic.Instance.GetEffect("Assets/Effects/ReallyDark").Value);
			Filters.Scene["Entrogic:ReallyDark"] = new Filter(new ScreenShaderData(screenRef2, "ReallyDark"), EffectPriority.VeryHigh);
			Filters.Scene["Entrogic:ReallyDark"].Load();
			Ref<Effect> screenRef3 = new Ref<Effect>(Entrogic.Instance.GetEffect("Assets/Effects/GooddShader").Value);
			Filters.Scene["Entrogic:GooddShader"] = new Filter(new ScreenShaderData(screenRef3, "GooddShader"), EffectPriority.VeryHigh);
			Filters.Scene["Entrogic:GooddShader"].Load();
			Filters.Scene["Entrogic:Blur"] = new Filter(new ScreenShaderData(new Ref<Effect>(Entrogic.Instance.GetEffect("Assets/Effects/Blur").Value), "Blur"), EffectPriority.VeryHigh);
			Filters.Scene["Entrogic:Blur"].Load();

			WhiteBlur = new ShaderData(new Ref<Effect>(Entrogic.Instance.GetEffect("Assets/Effects/WhiteBlur").Value), "WhiteBlur");
		}
	}
}
