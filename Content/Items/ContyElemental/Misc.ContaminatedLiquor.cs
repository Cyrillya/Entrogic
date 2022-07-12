using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Terraria.ID;

using static Entrogic.Entrogic;
using Terraria.GameContent.Creative;
using Terraria.Audio;
using Entrogic.Content.NPCs.Enemies.ContyElemental;
using Entrogic.Content.Items.BaseTypes;

namespace Entrogic.Content.Items.ContyElemental
{
    public class ContaminatedLiquor : ItemBase
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
            ItemID.Sets.SortingPriorityBossSpawns[Item.type] = 12; // This helps sort inventory know that this is a boss summoning Item.
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 32;
            Item.maxStack = 20;
            Item.rare = RarityLevelID.MiddleHM;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.consumable = true;
            Item.value = Item.sellPrice(0, 0, 20);
        }

        public override bool CanUseItem(Player player) {
            return (player.ZoneBeach || DownedHandler.DownedContyElemental) && !NPC.AnyNPCs(NPCType<ContaminatedElemental>());
        }

        public override bool? UseItem(Player player) {
            if (player.whoAmI == Main.myPlayer) {
                //If the player using the item is the client
                //(explicitely excluded serverside here)
                SoundEngine.PlaySound(SoundID.Roar, player.position);

                int type = NPCType<ContaminatedElemental>();

                if (Main.netMode != NetmodeID.MultiplayerClient) {
                    //If the player is not in multiplayer, spawn directly
                    NPC.SpawnOnPlayer(player.whoAmI, type);
                }
                else {
                    //If the player is in multiplayer, request a spawn
                    //This will only work if NPCID.Sets.MPAllowedEnemies[type] is true, which we set in MinionBossBody
                    NetMessage.SendData(MessageID.SpawnBoss, number: player.whoAmI, number2: type);
                }
            }

            return true;
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
            get => CreateRecipe()
                    .AddIngredient(ItemID.SharkFin, 3)
                    .AddIngredient(ItemID.BottledWater, 5);
        }
    }
}
