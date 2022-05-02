using System;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.Chat;
using Entrogic.Content.Items.BaseTypes;

namespace Entrogic.Content.Items.Symbiosis
{
    public class GelCultureFlask : ItemBase
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 32;
            Item.maxStack = 20;
            Item.rare = RarityLevelID.EarlyPHM;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
        }
        public override void AddRecipes()
        {
            //CreateRecipe()
            //    .AddIngredient(ItemID.Gel, 20)
            //    .AddIngredient(ItemID.Bottle)
            //    .AddIngredient(ItemType<Materials.SoulOfPure>(), 5)
            //    .AddRecipeGroup("IronBar")
            //    .AddTile(TileID.WorkBenches)
            //    .Register();

        }
        public override bool CanUseItem(Player player) {
            int npc = NPC.NewNPC((int)Main.MouseWorld.X, (int)Main.MouseWorld.Y, 113);
            SoundEngine.PlaySound(SoundID.Roar, player.position, 0);
            if (Main.netMode == NetmodeID.SinglePlayer)
                Main.NewText(Language.GetTextValue("Announcement.HasAwoken", Main.npc[npc].TypeName), 175, 75);
            else if (Main.netMode == NetmodeID.Server)
                ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Announcement.HasAwoken", Main.npc[npc].GetTypeNetName()), new Color(175, 75, 255));
            return true;
        }
    }
}
