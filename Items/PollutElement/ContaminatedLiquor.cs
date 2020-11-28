using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Terraria.ID;

using Entrogic.NPCs.Boss.PollutElement;
using static Entrogic.Entrogic;
using Terraria.GameContent.Creative;

namespace Entrogic.Items.PollutElement
{
    public class ContaminatedLiquor : ModItem
    {
<<<<<<< HEAD:Items/PollutElement/ContaminatedLiquor.cs
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }
=======
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96:Items/PollutElement/污染水源.cs
        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 32;
            item.maxStack = 20;
            item.rare = RareID.LV8;
            item.useAnimation = 45;
            item.useTime = 45;
<<<<<<< HEAD:Items/PollutElement/ContaminatedLiquor.cs
            item.useStyle = ItemUseStyleID.Shoot;
            item.consumable = true;
            item.value = Item.sellPrice(0, 0, 20);
        }
        public override bool CanUseItem(Player player) => (player.ZoneBeach || EntrogicWorld.IsDownedPollutionElemental) && !NPC.AnyNPCs(NPCType<PollutionElemental>());
=======
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.consumable = true;
            item.value = Item.sellPrice(0, 0, 20);
        }
        public override bool CanUseItem(Player player)
        {
            return player.ZoneBeach && !NPC.AnyNPCs(NPCType<PollutionElemental>());
        }
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96:Items/PollutElement/污染水源.cs

        public override bool UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer && CanUseItem(player))
            {
<<<<<<< HEAD:Items/PollutElement/ContaminatedLiquor.cs
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Roar, player.Center, 0);
=======
                Main.PlaySound(SoundID.Roar, player.Center, 0);
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96:Items/PollutElement/污染水源.cs
                NPCSpawnOnPlayer(player, NPCType<PollutionElemental>());
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

        public override void AddRecipes()
        {
            BasicIngredient
                .AddIngredient(ItemID.Starfish, 3)
                .AddTile(TileID.AlchemyTable)
                .Register();

            BasicIngredient
                .AddIngredient(ItemID.Seashell, 3)
                .AddTile(TileID.AlchemyTable)
                .Register();

            BasicIngredient
                .AddIngredient(ItemID.Coral, 3)
                .AddTile(TileID.AlchemyTable)
                .Register();
        }
        private Recipe BasicIngredient
        {
            get
            {
                return CreateRecipe()
                    .AddIngredient(ItemID.SharkFin, 3)
                    .AddIngredient(ItemID.BottledWater, 5);
            }
        }
    }
}
