<<<<<<< HEAD:Items/Weapons/Ranged/Thrown/LeafbladeItem.cs
=======
using CalamityMod.Projectiles.Ranged;

>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96:Items/Weapons/Thrown/飞叶.cs
using Entrogic.Projectiles.Thrown;

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Ranged.Thrown
{
    public class LeafbladeItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("“飞花摘叶皆可伤人”\n" +
                "早已失传的武功");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            item.damage = 5;       
            item.width = 28;         
            item.height = 22;          
            item.useTime = 12;          
            item.useAnimation = 12;     
<<<<<<< HEAD:Items/Weapons/Ranged/Thrown/LeafbladeItem.cs
            item.useStyle = ItemUseStyleID.Swing;
=======
            item.useStyle = ItemUseStyleID.SwingThrow;
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96:Items/Weapons/Thrown/飞叶.cs
            item.knockBack = 2;         
            item.value = Item.sellPrice(copper: 5);
            item.rare = ItemRarityID.Green;              
            item.UseSound = SoundID.Item1;      
            item.autoReuse = true;
            item.shoot = ProjectileType<Leafblade>();
            item.shootSpeed = 15f;
<<<<<<< HEAD:Items/Weapons/Ranged/Thrown/LeafbladeItem.cs
            item.DamageType = DamageClass.Ranged;
=======
            item.ranged = true;
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96:Items/Weapons/Thrown/飞叶.cs
            item.noUseGraphic = true;
            item.crit += 19;
            item.consumable = true;
            item.maxStack = 999;
        }
        public override void AddRecipes()
        {
            CreateRecipe(15)
                .AddRecipeGroup("Wood", 1)
                .AddTile(TileID.LivingLoom)
                .Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.Next(10) == 0)
            {
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 2);
            }
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(5));
            speedX = perturbedSpeed.X;
            speedY = perturbedSpeed.Y;
            return true;
        }
    }
}
