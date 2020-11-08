using Entrogic.Projectiles.Melee.Swords;

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Melee.Sword
{
    public class SmeltDagger : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Tooltip.SetDefault("“新鲜出炉”\n" +
                "有可能会有点软化");
        }

        public override void SetDefaults()
        {
            item.damage = 24;           //The damage of your weapon 武器的伤害
            item.width = 30;            //Weapon's texture's width 武器材质的宽度
            item.height = 30;           //Weapon's texture's height 武器材质的高度
            item.useTime = 6;          //The time span of using the weapon. Remember in terraria, 60 frames is a second. 使用武器的速度的时间跨度(以帧为单位)。在Terraria中，60帧是一秒。
            item.useAnimation = 6;         //The time span of the using animat3ion of the weapon, suggest set it the same as useTime. 使用武器的动画的时间跨度(以帧为单位),通常和useTime设置成一样
            item.useStyle = ItemUseStyleID.Shoot;
            item.knockBack = 3;         //The force of knockback of the weapon. Maximum is 20 武器的击退,最高是20
            item.value = Item.sellPrice(gold: 3);
            item.rare = ItemRarityID.Lime;              //The rarity of the weapon, from -1 to 13 武器的稀有度,从-1到13
            item.UseSound = SoundID.Item1;      //The sound when the weapon is using 使用武器时的音效
            item.autoReuse = true;
            item.useTurn = true;
            item.channel = true;
            item.shoot = ProjectileType<SmeltDaggerProjectile>();
            item.shootSpeed = 20f;
            item.noUseGraphic = true;
        }
        
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(null,"GelOfLife", 7)
                .AddIngredient(ItemID.MeteoriteBar, 10)
                .AddIngredient(ItemID.HellstoneBar, 8)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 180);
        }
    }
}