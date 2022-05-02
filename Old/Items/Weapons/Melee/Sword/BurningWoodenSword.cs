﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Melee.Sword
{
    public class BurningWoodenSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("“十分的烫手”\n" +
                "点燃你的敌人");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            item.damage = 8;
            item.knockBack = 2f;
            item.crit += 9;
            item.rare = ItemRarityID.White;
            item.useTime = 30;
            item.useAnimation = 30;
<<<<<<< HEAD
            item.useStyle = ItemUseStyleID.Swing;
=======
            item.useStyle = ItemUseStyleID.SwingThrow;
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
            item.autoReuse = true;
            item.DamageType = DamageClass.Melee;
            item.value = Item.sellPrice(0, 0, 60, 0);
            item.UseSound = SoundID.Item1;
            item.scale = 1.4f;
            item.width = 40;
            item.height = 34;
            item.maxStack = 1;
            item.useTurn = true;
        }
        public override void AddRecipes()
        {
<<<<<<< HEAD
            CreateRecipe()
                .AddIngredient(ItemID.WoodenSword, 1)//木剑
                .AddIngredient(ItemID.Torch, 8)//火把
                .AddTile(TileID.WorkBenches)//工作台
                .Register();
=======
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.WoodenSword, 1);//木剑
            recipe.AddIngredient(ItemID.Torch, 8);//火把
            recipe.AddTile(TileID.WorkBenches);//工作台
            recipe.SetResult(this);
            recipe.AddRecipe();
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            target.AddBuff(24, 240);
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            Dust.NewDust(hitbox.TopLeft(), hitbox.Width, hitbox.Height, 6, 0, 0, 90, default(Color), 0.65f);
        }
    }
}