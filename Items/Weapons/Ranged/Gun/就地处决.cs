using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Ranged.Gun
{
    public class 就地处决 : ModItem
    {
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }
        public override void SetDefaults()
        {
            item.scale = 1.2f;
            item.damage = 167;
            item.ranged = true;
            item.width = 32;
            item.height = 20;
            item.useTime = 50;
            item.useAnimation = 50;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 18;
            item.value = Item.sellPrice(gold:1, silver:80);
            item.rare = 6;
            item.UseSound = SoundID.Item40;
            item.autoReuse = true;
            item.crit += 21;
            item.shoot = 10;
            item.shootSpeed = 18f;
            item.useAmmo = AmmoID.Bullet;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (type == ProjectileID.Bullet) type = ProjectileID.BulletHighVelocity;
            Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI);
            return false;
        }
        public override void HoldItem(Player player)
        {
            player.GetModPlayer<EntrogicPlayer>().ProjectieHasArmorPenetration = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "污染之魂", 7);
            recipe.AddIngredient(ItemID.HallowedBar, 18);
            recipe.AddIngredient(ItemID.Shotgun);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
        {
            damage += target.defense / 2;
        }
    }
}
