using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Terraria.ID;

using Entrogic.NPCs.Boss.AntaGolem;
using static Entrogic.Entrogic;

namespace Entrogic.Items.AntaGolem
{
    public class TitansOrder : ModItem
    {
<<<<<<< HEAD
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }
=======
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 32;
            item.maxStack = 20;
            item.rare = RareID.LV4;
            item.useAnimation = 45;
            item.useTime = 45;
<<<<<<< HEAD
            item.useStyle = ItemUseStyleID.HoldUp;
=======
            item.useStyle = ItemUseStyleID.HoldingUp;
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
            item.consumable = true;
        }
        public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(NPCType<Antanasy>());
        }

        public override bool UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer && CanUseItem(player))
            {
<<<<<<< HEAD
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Roar, player.Center, 0);
=======
                Main.PlaySound(SoundID.Roar, player.Center, 0);
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
                NPCSpawnOnPlayer(player, NPCType<Antanasy>());
                return true;
            }
            return false;
        }
        public static void NPCSpawnOnPlayer(Player player, int type)
        {
<<<<<<< HEAD
            if (Main.netMode != NetmodeID.MultiplayerClient)
=======
            if (Main.netMode != 1)
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
            {
                NPC.SpawnOnPlayer(player.whoAmI, type);
                return;
            }
<<<<<<< HEAD
            ModPacket packet = Instance.GetPacket();
=======
            ModPacket packet = Entrogic.Instance.GetPacket();
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
            packet.Write((byte)EntrogicModMessageType.NPCSpawnOnPlayerAction);
            packet.Write((byte)player.whoAmI);
            packet.Write((short)type);
            packet.Send(-1, -1);
        }
    }
}
