using System;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using System.Text;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using static Entrogic.Entrogic;
using Microsoft.Xna.Framework.Graphics;

namespace Entrogic.Items.Materials
{
    public class 拟态魔能 : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("“变换不定的魔法”");
            ItemID.Sets.ItemIconPulse[item.type] = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 24;
            item.maxStack = 999;
            item.value = Item.sellPrice(silver: 20);
            item.rare = ItemRarityID.Orange;
        }

        public override void GrabRange(Player player, ref int grabRange)
        {
            grabRange *= 3;
        }

        public override bool GrabStyle(Player player)
        {
            Vector2 vectorItemToPlayer = player.Center - item.Center;
            Vector2 movement = -vectorItemToPlayer.SafeNormalize(default(Vector2)) * 0.02f;
            item.velocity = item.velocity + movement;
            item.velocity = Collision.TileCollision(item.position, item.velocity, item.width, item.height);
            return true;
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(item.Center, Color.Blue.ToVector3() * 0.75f * Main.essScale);
        }
        public override bool CanPickup(Player player)
        {
            return base.CanPickup(player);
        }
    }
}
