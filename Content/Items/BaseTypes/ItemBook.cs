using Entrogic.Common.Globals.Players;
using Entrogic.Common.ModSystems;
using Entrogic.Interfaces.UI.BookUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Entrogic.Content.Items.BaseTypes
{
    public abstract class ItemBook : ItemBase
    {
        internal int PageMax = 1;

        public override void SetStaticDefaults() {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public virtual void SetDefaultsBook() { }
        public sealed override void SetDefaults() {
            Item.value = Item.sellPrice(0, 0, 20);
            Item.width = 10;
            Item.height = 10;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = 1000;
            Item.scale = 0.75f;
            Item.rare = ItemRarityID.Quest;
            base.SetDefaults();
            SetDefaultsBook();
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            TooltipLine line = new TooltipLine(Entrogic.Instance, Entrogic.Instance.Name, BookInfoPlayer.WarnTexts) { overrideColor = Color.Red };
            if (Main.playerInventory)
                tooltips.Add(line);
            //if (AEntrogicConfigClient.Instance.ShowUsefulInformations) {
            line = new TooltipLine(Entrogic.Instance, Entrogic.Instance.Name, "总页数：" + PageMax) {
                overrideColor = Color.Gray
            };
            tooltips.Add(line);
            //}
        }

        public virtual void ModifyBookPanel(Player player, int page, ref Asset<Texture2D> panelImage) { }
        public virtual void ModifyBookContent(Player player, int page, ref List<BookContent> contents) { }
        public virtual bool DrawReading(SpriteBatch spriteBatch, Item item, Player player) { return true; }
    }
}
