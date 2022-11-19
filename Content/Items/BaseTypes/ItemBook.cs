using Entrogic.Common.Globals.Players;
using Entrogic.Interfaces.UIElements;

namespace Entrogic.Content.Items.BaseTypes
{
    public abstract class ItemBook : ItemBase, IModifyBookContent
    {
        internal int PageMax = 1;

        public override void SetStaticDefaults() {
            SacrificeTotal = 1;
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
            TooltipLine line = new(Entrogic.Instance, Entrogic.Instance.Name, BookInfoPlayer.WarnTexts) { OverrideColor = Color.Red };
            if (Main.playerInventory)
                tooltips.Add(line);
            //if (AEntrogicConfigClient.Instance.ShowUsefulInformations) {
            line = new TooltipLine(Entrogic.Instance, Entrogic.Instance.Name, "总页数：" + PageMax) {
                OverrideColor = Color.Gray
            };
            tooltips.Add(line);
            //}
        }

        public void ModifyBookContent(Item item, Player player, ref List<BookContent> contents) {
            ModifyBookPageContent(player, player.GetModPlayer<BookInfoPlayer>().CurrentPage, ref contents);
        }

        public abstract void ModifyBookPageContent(Player player, int page, ref List<BookContent> contents);
    }
}
