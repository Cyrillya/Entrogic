using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.AntaGolem
{
    public class 巨石长枪 : ModItem
    {
        public override void SetDefaults()
        {
            item.useStyle = 5;
            item.useAnimation = 10;
            item.useTime = 10;
            item.shootSpeed = 6.6f;
            item.knockBack = 5.8f;
            item.width = 56;
            item.height = 56;
            item.damage = 55;
            item.UseSound = SoundID.Item1;
            item.shoot = mod.ProjectileType("巨石长枪");
            item.rare = 4;
            item.value = Item.sellPrice(0, 3, 50);
            item.noMelee = true;
            item.noUseGraphic = true;
            item.melee = true;
        }
        public override void HoldItem(Player player)
        {
            item.autoReuse = true;
            base.HoldItem(player);
        }
    }
}