using System;
using Terraria;
using Terraria.Localization;
using Terraria.ID;

using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Entrogic.NPCs;
using Entrogic.NPCs.Boss.凝胶Java盾;
using static Entrogic.Entrogic;

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
            return !NPC.AnyNPCs(NPCType<Volutio>()) && !NPC.AnyNPCs(NPCType<Embryo>());
        }

        public override bool UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer && CanUseItem(player))
            {
                Main.PlaySound(SoundID.Roar, player.position, 0);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int npc = NPC.NewNPC((int)ModHelper.MousePos.X, (int)ModHelper.MousePos.Y, NPCID.BlueSlime);
                    string typeName = Main.npc[npc].TypeName;
                    Main.NewText(Language.GetTextValue("Announcement.HasAwoken", typeName), 175, 75);
                    return true;
                }
                ModPacket packet = Entrogic.Instance.GetPacket();
                packet.Write((byte)EntrogicModMessageType.NPCSpawn);
                packet.Write((byte)player.whoAmI);
                packet.Write((short)NPCID.BlueSlime);
                packet.Write(true);
                packet.Write(ModHelper.MousePos.X);
                packet.Write(ModHelper.MousePos.Y);
                packet.Send(-1, -1);
                return true;
            }
            return false;
        }
    }
    public class EmbryoCultureFlask : ModItem
    {
        public override string Texture => "Entrogic/Items/VoluGels/GelCultureFlask";
        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 32;
            item.maxStack = 20;
            item.rare = RareID.LV2;
            item.useAnimation = 20;
            item.useTime = 20;
            item.noUseGraphic = true;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.consumable = true;
        }
        public override bool CanUseItem(Player player)
        {
            return EntrogicWorld.Check(player.position.X, player.position.Y);
        }

        public override bool UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer && CanUseItem(player))
            {
                Main.PlaySound(SoundID.Roar, player.position, 0);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int npc = NPC.NewNPC((int)(player.Center.X), (int)player.Center.Y - 16 * 8, NPCType<Embryo>());
                    string typeName = Main.npc[npc].TypeName;
                    Main.NewText(Language.GetTextValue("Announcement.HasAwoken", typeName), 175, 75);
                    return true;
                }
                ModPacket packet = Entrogic.Instance.GetPacket();
                packet.Write((byte)EntrogicModMessageType.NPCSpawn);
                packet.Write((byte)player.whoAmI);
                packet.Write((short)NPCType<Embryo>());
                packet.Write(true);
                packet.Write(player.Center.X);
                packet.Write(player.Center.Y - 16 * 8);
                packet.Send(-1, -1);
                return true;
            }
            return false;
        }
    }
}
