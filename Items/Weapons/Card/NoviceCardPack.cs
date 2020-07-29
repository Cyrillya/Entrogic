using Entrogic.Items.Books.卡牌入门手册;
using Entrogic.Items.Weapons.Card.Elements;
using Entrogic.Items.Weapons.Card.Gloves;

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Card
{
    public class NoviceCardPack : ModItem
    {
        public List<int> GetCard = new List<int>()
        {
            ItemType<ArcaneMissle>(),
            ItemType<Glove>(),
            ItemType<CardBasicManual>()
        };
        public List<int> GetCardStack = new List<int>() { 3, 1, 1 };

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
        }

        public override void SetDefaults()
        {
            item.maxStack = 30;
            item.consumable = true;
            item.width = 32;
            item.height = 40;
            item.rare = RareID.LV3;
            item.value = Item.buyPrice(0, 15);
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void RightClick(Player player)
        {
            player.QuickSpawnItem(ItemType<ArcaneMissle>(), 3);
            player.QuickSpawnItem(ItemType<Glove>());
            player.QuickSpawnItem(ItemType<CardBasicManual>());
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine line2 = new TooltipLine(mod, mod.Name, "新手专用！买不了吃亏买不了上当！")
            {
                overrideColor = Color.Red
            };
            tooltips.Add(line2);
            TooltipLine line = new TooltipLine(mod, mod.Name, "内含：")
            {
                overrideColor = Color.LightSlateGray
            };
            tooltips.Add(line);
            foreach (int card in GetCard)
            {
                Item i = new Item();
                i.SetDefaults(card);
                ModItem modItem = i.modItem;
                line = new TooltipLine(mod, mod.Name, i.Name + "×" + GetCardStack[GetCard.IndexOf(card)])
                {
                    overrideColor = Color.SlateGray
                };
                tooltips.Add(line);
            }
        }
    }
}