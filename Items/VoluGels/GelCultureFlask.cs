using System;
using Terraria;
using Terraria.Localization;
using Terraria.ID;

using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Entrogic.NPCs;

namespace Entrogic.Items.VoluGels
{
    public class GelCultureFlask : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 32;
            item.maxStack = 20;
            item.rare = RareID.LV2;
            item.useAnimation = 20;
            item.useTime = 20;
            item.noUseGraphic = true;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.consumable = true;
            item.makeNPC = NPCID.BlueSlime;
            item.tileBoost += 10000000;
        }
        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(ItemID.Gel, 20);
            modRecipe.AddIngredient(ItemID.Bottle);
            modRecipe.AddIngredient(ItemType<Materials.SoulOfPure>(), 5);
            modRecipe.AddRecipeGroup("IronBar");
            modRecipe.AddTile(TileID.WorkBenches);
            modRecipe.SetResult(this);
            modRecipe.AddRecipe();
        }
        public override bool CanUseItem(Player player)
        {
            string typeName = Language.GetTextValue("NPCName.BlueSlime");
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                Main.NewText(Language.GetTextValue("Announcement.HasAwoken", typeName), 175, 75, byte.MaxValue, false);
            }
            else
            {
                NetMessage.BroadcastChatMessage(NetworkText.FromKey("Announcement.HasAwoken", new object[]
                {
                                Language.GetTextValue("NPCName.BlueSlime")
                }), new Color(175, 75, 255), -1);
            }
            Main.PlaySound(SoundID.Roar, player.position, 0);
            return true;
        }
    }
}
