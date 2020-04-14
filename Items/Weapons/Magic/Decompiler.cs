using MonoMod.Cil;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using static Mono.Cecil.Cil.OpCodes;
using System;
using Entrogic.Projectiles.Magic;

namespace Entrogic.Items.Weapons.Magic
{
    public class Decompiler : ModItem
    {
        public override void SetDefaults()
        {
            item.damage = 62;
            item.magic = true;
            item.width = 26;
            item.height = 26;
            item.useTime = 90;
            item.useAnimation = 90;
            item.useStyle = 5;
            item.noMelee = true;
            item.crit = 0;
            item.mana = 15;
            item.value = Item.sellPrice(0, 5, 10, 0);
            item.rare = 6;
            item.UseSound = SoundID.Item75;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("Binary");
            item.shootSpeed = 8f;
            item.scale = 0.8f;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 vec = new Vector2(speedX, speedY);//Main.MouseWorld - position;
            position += vec.ToRotation().ToRotationVector2() * 3f * 16f;
            float revisionScale = Math.Abs(vec.ToRotation()); // 第一次折叠（正负/上下）
            if (revisionScale > MathHelper.PiOver2) // 第二次折叠（左右）
                revisionScale = MathHelper.Pi - revisionScale; // 将数值控制在0-PiOver2
            revisionScale = revisionScale / MathHelper.PiOver2; // 将数值控制在0-1
            if (vec.ToRotation() <= -MathHelper.PiOver2) // 特殊处理因为我不知道那种情况该咋办
            {
                revisionScale *= revisionScale * 2f;
            }
            int phrase = 0;
            for (int k = 1; k <= 1000; k++)
            {
                phrase++;
                switch (phrase % 4)
                {
                    case 0:
                        {
                            int i = Projectile.NewProjectile(position.X, position.Y, 0.0f, 0.0f, ProjectileType<Binary>(), damage, knockBack, player.whoAmI, 0.0f, 0.0f);
                            Projectile proj = Main.projectile[i];
                            proj.position += proj.Size / 2f * revisionScale;
                            proj.rotation = vec.ToRotation();
                            if (Math.Abs(vec.ToRotation()) > MathHelper.PiOver2)
                            {
                                proj.rotation = vec.ToRotation() - MathHelper.Pi;
                            }
                            if (Main.netMode == 2)
                                NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, i);
                        }
                        break;
                    case 2:
                        {
                            int i = Projectile.NewProjectile(position.X, position.Y, 0.0f, 0.0f, ProjectileType<Binary>(), damage, knockBack, player.whoAmI, 0.0f, 0.0f);
                            Projectile proj = Main.projectile[i];
                            proj.position += proj.Size / 2f * revisionScale;
                            proj.rotation = vec.ToRotation();
                            if (Math.Abs(vec.ToRotation()) > MathHelper.PiOver2)
                            {
                                proj.rotation = vec.ToRotation() - MathHelper.Pi;
                            }
                            proj.frame = 1;
                            if (Main.netMode == 2)
                                NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, i);
                        }
                        break;
                }
                position += new Vector2(speedX, speedY);
                if (Collision.SolidCollision(position, 16, 16))
                {
                    return false;
                }
            }
            return false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-6.0f, 0.0f);
        }
        public override void GetWeaponCrit(Player player, ref int crit)
        {
            crit = 0;
        }
        public override void AddRecipes()
        {
            ModRecipe r = new ModRecipe(mod);
            r.AddIngredient(ItemID.SoulofSight, 5);
            r.AddIngredient(ItemID.SoulofFright, 5);
            r.AddIngredient(ItemID.SoulofMight, 5);
            r.AddIngredient(ItemID.HallowedBar, 10);
            r.AddIngredient(ItemID.LaserRifle);
            r.AddTile(TileID.MythrilAnvil); 
            r.SetResult(this);
            r.AddRecipe();
        }
    }
}
