using System;

<<<<<<< HEAD
using Entrogic.Items.Materials;
=======
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
using Entrogic.Projectiles.Ammos;

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Ranged.Bow
{
    public class ChaosAccelerator : ModItem
    {
        public override string Texture => "Entrogic/Items/Weapons/Ranged/Bow/ChaosAccelerator_1";
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            item.damage = 62;
            item.width = 42;
            item.height = 30;
            item.useTime = 12;
            item.useAnimation = 12;
<<<<<<< HEAD
            item.useStyle = ItemUseStyleID.Shoot;
=======
            item.useStyle = ItemUseStyleID.HoldingOut;
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
            item.noMelee = true;
            item.value = Item.sellPrice(0, 5);
            item.rare = ItemRarityID.Pink;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.shoot = ProjectileType<ProGodArrow>();
            item.shootSpeed = 12f;
            item.useAmmo = AmmoID.Arrow;
            item.DamageType = DamageClass.Ranged;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            type = ProjectileType<ProGodArrow>();
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemType<CBar>(), 6)
                .AddIngredient(ItemID.HallowedRepeater, 1)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-6, 0);
        }
        public override bool ConsumeAmmo(Player player)
        {
            return Main.rand.NextFloat() >= .45f;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool CanUseItem(Player player)
        {
            item.useTime = 20;
            item.useAnimation = 20;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            return base.CanUseItem(player);
        }

        public int timeCounter = 0;
        public int timer = 1;
        public override void ModifyWeaponDamage(Player player, ref Modifier damage, ref float flat)
        {
            timeCounter++;
<<<<<<< HEAD
            TextureAssets.Item[ItemType<ChaosAccelerator>()] = Entrogic.Instance.GetTexture("Items/Weapons/Ranged/Bow/ChaosAccelerator_" + timer);
=======
            Main.itemTexture[ItemType<ChaosAccelerator>()] = mod.GetTexture("Items/Weapons/Ranged/Bow/ChaosAccelerator_" + timer);
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
            if (timeCounter >= 6)
            {
                if (timer >= 4) timer = 0;
                timer++;
                timeCounter = 0;
            }
            base.ModifyWeaponDamage(player, ref damage, ref flat);
        }
    }
}