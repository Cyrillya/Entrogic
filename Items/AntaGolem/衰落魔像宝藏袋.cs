using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Entrogic;

namespace Entrogic.Items.AntaGolem
{
    public class 衰落魔像宝藏袋 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("宝藏袋");
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
        }

        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.consumable = true;
            item.width = 32;
            item.height = 32;
            item.rare = -12;
            item.expert = true;
        }
        public override int BossBagNPC => mod.NPCType("Antanasy");

        public override bool CanRightClick()
        {
            return true;
        }

        public override void OpenBossBag(Player player)
        {
            switch (Main.rand.Next(4))
            {
                case 0:
                    player.QuickSpawnItem(ItemType<岩石猎枪>());
                    break;
                case 1:
                    player.QuickSpawnItem(ItemType<巨石长枪>());
                    break;
                case 2:
                    player.QuickSpawnItem(ItemType<衰落之眼>());
                    break;
                default:
                    player.QuickSpawnItem(ItemType<StoneSlimeStaff>());
                    break;
            }
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(mod, mod.Name,
"本BossBGM改编自东方永夜抄五面BOSS玲仙的BGM\n" +
"原作者为Zun.原曲:狂気の瞳 ～ Invisible Full Moon\n" +
"本BGM名称也为: 狂気の瞳 - UniCyix Bootleg" +
"\nBootleg为非授权改编,只要著名原作者信息.改编就会被允许" +
"\n所以本BGM没有版权问题" +
"\n除此之外的其他BGM版权均归EntrogicMod制作组所有"
));
        }
    }
}