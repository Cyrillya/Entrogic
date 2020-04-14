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
    public class 哈雷 :ModItem
    {
        public override void SetDefaults()
        {
            item.knockBack = 4f;
            item.damage = 67;
            item.magic = true;
            item.width = 86;
            item.height = 32;
            item.useTime = 40;
            item.useAnimation = 40;
            item.useStyle = 5;
            item.noMelee = true;
            item.crit += 37;
            item.mana = 10;
            item.value = Item.sellPrice(0, 1, 20, 0);
            item.rare = RareID.LV4;
            item.UseSound = SoundID.Item40;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("哈雷");
            item.shootSpeed = 12f;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-4.0f, 0.0f);
        }
        public override void AddRecipes()
        {
            ModRecipe r = new ModRecipe(mod);
            r.AddIngredient(ItemID.Wire, 6);
            r.AddIngredient(ItemID.FallenStar, 6);
            r.AddIngredient(ItemType<碳钢枪械部件>());
            r.AddIngredient(null, "CastIronBar", 6);
            r.AddIngredient(null, "SoulOfPure", 3);
            r.AddTile(TileID.Anvils);
            r.SetResult(this);
            r.AddRecipe();
        }
    }
}
