using Entrogic.Items.Materials;

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Magic
{
    /// <summary>
    /// 哈雷 的摘要说明
    /// 创建人机器名：DESKTOP-QDVG7GB
    /// 创建时间：2019/8/19 16:04:01
    /// </summary>
    public class Halle :ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            item.knockBack = 4f;
            item.damage = 67;
            item.DamageType = DamageClass.Magic;
            item.width = 86;
            item.height = 32;
            item.useTime = 40;
            item.useAnimation = 40;
<<<<<<< HEAD:Items/Weapons/Magic/Halle.cs
            item.useStyle = ItemUseStyleID.Shoot;
=======
            item.useStyle = ItemUseStyleID.HoldingOut;
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96:Items/Weapons/Magic/哈雷.cs
            item.noMelee = true;
            item.crit += 37;
            item.mana = 10;
            item.value = Item.sellPrice(0, 1, 20, 0);
            item.rare = RareID.LV4;
            item.UseSound = SoundID.Item40;
            item.autoReuse = false;
            item.shoot = ProjectileType<Projectiles.Magic.Halle>();
            item.shootSpeed = 12f;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-4.0f, 0.0f);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Wire, 6)
                .AddIngredient(ItemID.FallenStar, 6)
                .AddIngredient(ItemType<碳钢枪械部件>())
                .AddIngredient(ItemType<CastIronBar>(), 6)
                .AddIngredient(ItemType<SoulOfPure>(), 3)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
