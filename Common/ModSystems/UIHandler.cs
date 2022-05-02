using Entrogic.Interfaces.UI.BookUI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;

namespace Entrogic.Common.ModSystems
{
	public class UIHandler : ModSystem
	{
		public UIHandler() => Instance = this;
		internal static UIHandler Instance;
		private GameTime _lastUpdateUiGameTime;

		internal enum UI
		{
			Book
		}

		internal BookUI BookUI;
		internal UserInterface BookInterface;

		public override void Load() {
			base.Load();

			if (!Main.dedServ) {
				BookUI = new BookUI();
				BookUI.Activate();
				BookInterface = new UserInterface();
			}
		}

		public override void Unload() {
			base.Unload();

			if (BookUI != null) BookUI.Unload();
			BookUI = null;
			BookInterface = null;
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
			base.ModifyInterfaceLayers(layers);

			int MouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
			if (MouseTextIndex != -1) {
				layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
					"Entrogic: Book UI",
					delegate {
						if (_lastUpdateUiGameTime != null && BookInterface?.CurrentState != null) {
							BookInterface.Draw(Main.spriteBatch, new GameTime());
						}
						return true;
					},
					InterfaceScaleType.UI)
				);
			}

			//int CursorIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Cursor"));
			//if (CursorIndex != -1) {
			//	layers.Insert(CursorIndex, new LegacyGameInterfaceLayer(
			//		"Entrogic: Cursor Messages",
			//		delegate {
   //                     Tile t = Framing.GetTileSafely(Main.MouseWorld.ToTileCoordinates());
   //                     Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, $"slope:{t.Slope}", Main.mouseX + 4, Main.mouseY + 16, Color.White, Color.Black, Vector2.Zero);
   //                     Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, $"type:{t.type}", Main.mouseX + 4, Main.mouseY + 36, Color.White, Color.Black, Vector2.Zero);
   //                     Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, $"frameX:{t.frameX}", Main.mouseX + 4, Main.mouseY + 56, Color.White, Color.Black, Vector2.Zero);
   //                     Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, $"frameY:{t.frameY}", Main.mouseX + 4, Main.mouseY + 76, Color.White, Color.Black, Vector2.Zero);
			//			Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, $"wall:{t.wall}", Main.mouseX + 4, Main.mouseY + 96, Color.White, Color.Black, Vector2.Zero);
			//			return true;
			//		},
			//		InterfaceScaleType.UI)
			//	);
			//}
		}

		public override void UpdateUI(GameTime gameTime) {
			base.UpdateUI(gameTime);
			_lastUpdateUiGameTime = gameTime;

			if (BookInterface?.CurrentState != null) {
				BookInterface.Update(gameTime);
			}
		}

		internal void ShowUI(UI type) {
			switch (type) {
				case UI.Book:
					BookInterface?.SetState(BookUI);
					break;
			}
		}

		internal void HideUI(UI type) {
			switch (type) {
				case UI.Book:
					BookInterface?.SetState(null);
					break;
			}
		}
	}
}
