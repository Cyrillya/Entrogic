using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Terraria.ID;

using Entrogic.NPCs.Boss.PollutElement;
using static Entrogic.Entrogic;

namespace Entrogic.Items.PollutElement
{
    public class ContaminatedLiquor : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 32;
            item.maxStack = 20;
            item.rare = RareID.LV8;
            item.useAnimation = 45;
            item.useTime = 45;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.consumable = true;
            item.value = Item.sellPrice(0, 0, 20);
        }
        public override bool CanUseItem(Player player) => (player.ZoneBeach || EntrogicWorld.IsDownedPollutionElemental) && !NPC.AnyNPCs(NPCType<PollutionElemental>());

        public override bool UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer && CanUseItem(player))
            {
                Main.PlaySound(SoundID.Roar, player.Center, 0);
                NPCSpawnOnPlayer(player, NPCType<PollutionElemental>());
                return true;
            }
            return false;
        }
        public static void NPCSpawnOnPlayer(Player player, int type)
        {
            if (Main.netMode != 1)
            {
                NPC.SpawnOnPlayer(player.whoAmI, type);
                return;
            }
            ModPacket packet = Entrogic.Instance.GetPacket();
            packet.Write((byte)EntrogicModMessageType.NPCSpawnOnPlayerAction);
            packet.Write((byte)player.whoAmI);
            packet.Write((short)type);
            packet.Send(-1, -1);
        }

        public override void AddRecipes()
        {
            ModRecipe modRecipe = BasicIngredient;
            modRecipe.AddIngredient(ItemID.Starfish, 3);
            modRecipe.AddTile(TileID.AlchemyTable);
            modRecipe.SetResult(this);
            modRecipe.AddRecipe();

            modRecipe = BasicIngredient;
            modRecipe.AddIngredient(ItemID.Seashell, 3);
            modRecipe.AddTile(TileID.AlchemyTable);
            modRecipe.SetResult(this);
            modRecipe.AddRecipe();

            modRecipe = BasicIngredient;
            modRecipe.AddIngredient(ItemID.Coral, 3);
            modRecipe.AddTile(TileID.AlchemyTable);
            modRecipe.SetResult(this);
            modRecipe.AddRecipe();
        }
        private ModRecipe BasicIngredient
        {
            get
            {
                ModRecipe modRecipe = new ModRecipe(mod);
                modRecipe.AddIngredient(ItemID.SharkFin, 3);
                modRecipe.AddIngredient(ItemID.BottledWater, 5);
                return modRecipe;
            }
        }
    }
}
