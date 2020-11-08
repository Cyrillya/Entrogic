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
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }
        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 32;
            item.maxStack = 20;
            item.rare = RareID.LV4;
            item.useAnimation = 45;
            item.useTime = 45;
            item.useStyle = ItemUseStyleID.HoldUp;
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
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Roar, player.Center, 0);
                NPCSpawnOnPlayer(player, NPCType<Antanasy>());
                return true;
            }
            return false;
        }
        public static void NPCSpawnOnPlayer(Player player, int type)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.SpawnOnPlayer(player.whoAmI, type);
                return;
            }
            ModPacket packet = Instance.GetPacket();
            packet.Write((byte)EntrogicModMessageType.NPCSpawnOnPlayerAction);
            packet.Write((byte)player.whoAmI);
            packet.Write((short)type);
            packet.Send(-1, -1);
        }
    }
}
