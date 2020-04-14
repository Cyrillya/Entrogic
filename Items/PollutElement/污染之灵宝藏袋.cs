using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.PollutElement
{
    public class 污染之灵宝藏袋 : ModItem
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
        public override int BossBagNPC => mod.NPCType("污染之灵");

        public override bool CanRightClick()
        {
            return true;
        }

        public override void OpenBossBag(Player player)
        {
            player.TryGettingDevArmor();
            switch (Main.rand.Next(3))
            {
                case 0:
                    player.QuickSpawnItem(mod.ItemType("污痕长弓"));
                    break;
                case 1:
                    player.QuickSpawnItem(mod.ItemType("污秽洋流"));
                    break;
                default:
                    player.QuickSpawnItem(mod.ItemType("水元素召唤杖"));
                    break;
            }
            player.QuickSpawnItem(ItemType<风暴之瓶>());
            for (int i = 0; i < 2; i++)
            {
                string Armordrop = "污染面具";
                switch (Main.rand.Next(3))
                {
                    case 0:
                        switch (Main.rand.Next(4))
                        {
                            case 0:
                                Armordrop = "污染头盔";
                                break;
                            case 1:
                                Armordrop = "污染头饰";
                                break;
                            case 2:
                                Armordrop = "污染之冠";
                                break;
                            default:
                                Armordrop = "污染面具";
                                break;
                        }
                        break;
                    case 1:
                        Armordrop = "污染胸甲";
                        break;
                    default:
                        Armordrop = "污染护胫";
                        break;
                }
                player.QuickSpawnItem(mod.ItemType(Armordrop));
            }
        }
    }
}