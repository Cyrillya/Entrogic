using Entrogic.Common.Globals.Players;
using Entrogic.Common.Hooks.Items;
using Entrogic.Content.Items.BaseTypes;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Entrogic.Common.Globals.GlobalItems
{
    public class GlobalBook : GlobalItem, IModifyBookPanel, IModifyBookContent, IDrawReading
	{
		void IModifyBookPanel.ModifyBookPanel(Item item, Player player, ref Asset<Texture2D> panelImage) {
			if (player.HeldItem?.ModItem is ItemBook)
				(item.ModItem as ItemBook).ModifyBookPanel(player, player.GetModPlayer<BookInfoPlayer>().CurrentPage, ref panelImage);
		}

		void IModifyBookContent.ModifyBookContent(Item item, Player player, ref List<Interfaces.UI.BookUI.BookContent> contents) {
			if (player.HeldItem?.ModItem is ItemBook)
				(item.ModItem as ItemBook).ModifyBookContent(player, player.GetModPlayer<BookInfoPlayer>().CurrentPage, ref contents);
		}

		bool IDrawReading.DrawReading(SpriteBatch spriteBatch, Item item, Player player) {
			if (player.HeldItem?.ModItem is ItemBook)
				return (item.ModItem as ItemBook).DrawReading(spriteBatch, item, player);
			else return false;
		}
	}
}
