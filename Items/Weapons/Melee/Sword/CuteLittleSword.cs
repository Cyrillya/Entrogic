using Entrogic.Projectiles.Melee.Swords;

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Melee.Sword
{
    public class CuteLittleSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            item.damage = 51;           //The damage of your weapon 武器的伤害
            item.width = 34;            //Weapon's texture's width 武器材质的宽度
            item.height = 34;           //Weapon's texture's height 武器材质的高度
            item.useTime = 6;          //The time span of using the weapon. Remember in terraria, 60 frames is a second. 使用武器的速度的时间跨度(以帧为单位)。在Terraria中，60帧是一秒。
            item.useAnimation = 6;         //The time span of the using animat3ion of the weapon, suggest set it the same as useTime. 使用武器的动画的时间跨度(以帧为单位),通常和useTime设置成一样
            item.useStyle = ItemUseStyleID.Shoot;
            item.knockBack = 4;         //The force of knockback of the weapon. Maximum is 20 武器的击退,最高是20
            item.value = Item.sellPrice(0, 1, 50);
            item.rare = ItemRarityID.LightPurple;              //The rarity of the weapon, from -1 to 13 武器的稀有度,从-1到13   //The sound when the weapon is using 使用武器时的音效
            item.autoReuse = false;
            item.channel = true;
            item.useTurn = true;
            item.shoot = ProjectileType<CuteBlade>();
            item.shootSpeed = 20f;
            item.noUseGraphic = true;
            item.DamageType = DamageClass.Melee;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SoulofLight, 6)
                .AddIngredient(ItemID.Wire, 12)
                .AddRecipeGroup("IronBar", 4)
                .AddTile(TileID.MythrilAnvil)
                .Register();
            //recipe.AddIngredient(null, "CuteWidget", 1);
        }
        public override bool UseItem(Player player)
        {
            player.statDefense = (int)((float)player.statDefense * 1.3f);
            return true;
        }
    }
}