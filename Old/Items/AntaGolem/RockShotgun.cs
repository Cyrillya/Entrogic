﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.AntaGolem
{
    public class RockShotgun : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("“它上面刻有许多象形文字...‘谨慎使用’”");
            DisplayName.SetDefault("岩石猎枪");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-12, -4);
        }
        public override void SetDefaults()
        {
            item.scale = 1.2f;
            item.damage = 111;
            item.DamageType = DamageClass.Ranged;
            item.width = 32;
            item.height = 20;
            item.useTime = item.useAnimation = 18;
<<<<<<< HEAD:Items/AntaGolem/RockShotgun.cs
            item.useStyle = ItemUseStyleID.Shoot;
=======
            item.useStyle = ItemUseStyleID.HoldingOut;
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96:Items/AntaGolem/岩石猎枪.cs
            item.noMelee = true;
            item.knockBack = 3;
            item.value = Item.sellPrice(0, 3, 50);
            item.rare = ItemRarityID.Orange;
            item.UseSound = SoundID.Item40;
            item.autoReuse = false;
            item.crit += 29;
            item.shoot = ProjectileID.PurificationPowder;
            item.shootSpeed = 18f;
            item.useAmmo = AmmoID.Bullet;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            type = 242;
            Projectile.NewProjectile(position,new Vector2(speedX,speedY),type,item.damage,3,0,0,0);
            return false;
        }
    }
}
