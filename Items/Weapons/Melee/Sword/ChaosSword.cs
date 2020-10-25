using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

using Microsoft.Xna.Framework.Graphics;
using Entrogic.Projectiles.Ranged.Bows;
using Entrogic.Projectiles.Melee.Swords;
using Entrogic.Items.Materials;

namespace Entrogic.Items.Weapons.Melee.Sword
{
    public class ChaosSword : ModItem
    {
        public bool enpowered = false;
        public float charge = 0;
        public override void SetStaticDefaults()
        {
            Item.staff[item.type] = true;
        }
        public override void SetDefaults()
        {
            item.UseSound = SoundID.Item1;
            item.value = Item.sellPrice(0, 5);
            item.crit += 4;
            item.damage = 68;           //The damage of your weapon 武器的伤害
            item.width = 88;            //Weapon's texture's width 武器材质的宽度
            item.height = 88;           //Weapon's texture's height 武器材质的高度
            item.useTime = 12;          //The time span of using the weapon. Remember in terraria, 60 frames is a second. 使用武器的速度的时间跨度(以帧为单位)。在Terraria中，60帧是一秒。
            item.useAnimation = 12;         //The time span of the using animat3ion of the weapon, suggest set it the same as useTime. 使用武器的动画的时间跨度(以帧为单位),通常和useTime设置成一样
            item.useStyle = ItemUseStyleID.Shoot;
            item.knockBack = 8;         //The force of knockback of the weapon. Maximum is 20 武器的击退,最高是20
            item.rare = RareID.LV6;              //The rarity of the weapon, from -1 to 13 武器的稀有度,从-1到13   //The sound when the weapon is using 使用武器时的音效
            item.autoReuse = false;
            item.channel = true;
            item.useTurn = true;
            item.shoot = ProjectileType<CompoundWill>();
            item.shootSpeed = 60f;
            item.noUseGraphic = false;
            item.DamageType = DamageClass.Melee;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                if (!enpowered)
                {
                    item.UseSound = SoundID.Item4;
                    enpowered = true;
                }
                else
                {
                    item.UseSound = SoundID.Item4;
                    enpowered = false;
                }
                item.useTime = 60;
                item.useAnimation = 60;
                item.noMelee = true;
            }
            else
            {
                if (enpowered)
                {
                    item.noUseGraphic = false;
                    item.useAnimation = item.useTime = 60;
                    item.UseSound = SoundID.Item45;
                    item.shoot = ProjectileType<GodBeamFri>();
                    item.noMelee = false;
                }
                else
                {
                    item.noUseGraphic = true;
                    item.useTime = 22;
                    item.useAnimation = 22;
                    item.UseSound = SoundID.Item1;
                    item.shoot = ProjectileType<CompoundWill>();
                    item.noMelee = true;
                }
            }
            return base.CanUseItem(player);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.altFunctionUse != 2)
            {
                if (enpowered)
                {
                    if (charge > 0)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            float speed = i * 10 - 5;
                            Projectile.NewProjectile(position, new Vector2(speed * speedY, 0f), type, damage, knockBack, Main.myPlayer);
                            if (charge >= 45)
                            {
                                Projectile.NewProjectile(position, new Vector2(0f, speed * speedX), type, damage, knockBack, Main.myPlayer);
                            }
                        }
                        if (charge >= 90)
                        {
                            float s = 5;
                            Projectile.NewProjectile(position, new Vector2(s, -s), type, damage, knockBack, Main.myPlayer);
                            Projectile.NewProjectile(position, new Vector2(-s, s), type, damage, knockBack, Main.myPlayer);
                            Projectile.NewProjectile(position, new Vector2(s, s), type, damage, knockBack, Main.myPlayer);
                            Projectile.NewProjectile(position, new Vector2(-s, -s), type, damage, knockBack, Main.myPlayer);
                        }
                        if (charge >= 135)
                        {
                            int i = 0;
                            if (Main.MouseWorld.X - player.Center.X > 0) i = 1;
                            else i = -1;
                            Projectile.NewProjectile(position.X, position.Y, 24f * i, -0.1f, ProjectileType<Projectiles.Miscellaneous.Dash>(), damage, knockBack, Main.myPlayer, 0f, 1f);
                        }
                        charge = 0;
                    }
                }
            }
            return false;
        }
        public int charge2 = 0;
        public int dire = 1;
        public int UnenpoweredHitCooldownMax = 5;
        public int[] UnenpoweredHitCooldown = new int[201];
        public override void UseStyle(Player player)
        {
            if (enpowered)
            {
                int i = 0;
                if (Main.MouseWorld.X - player.Center.X > 0) i = 1;
                else i = -1;
                player.direction = i;
                //if (charge2 % 2 == 0)
                //{
                //    int j = Projectile.NewProjectile(player.position, player.velocity, ModContent.ProjectileType<特效BowGod>(), 0, 0, player.whoAmI);
                //    Main.projectile[j].timeLeft = (int)(charge * 0.148f) + 5;
                //}
                if (player.itemAnimation >= player.itemAnimationMax - 1 && player.controlUseItem)
                {
                    if (charge < 135)
                    {
                        charge2 += 1;
                    }
                    if (charge2 >= 2)
                    {
                        charge += 1;
                        charge2 = 0;
                    }
                    player.itemAnimation = (int)player.itemAnimationMax;
                    player.itemTime = (int)item.useTime;
                }
                else if (charge > 0) player.itemTime = 0;
            }
            else
            {
                for (int i = 0; i < UnenpoweredHitCooldown.Length; i++)
                    UnenpoweredHitCooldown[i]--;
                if (player.itemAnimation <= item.useAnimation - 2)
                {
                    item.noUseGraphic = false;
                    player.direction = dire;
                }
                else
                {
                    if (Main.MouseWorld.X - player.Center.X > 0) dire = 1;
                    else dire = -1;
                }
                if (player.itemAnimation >= item.useAnimation - 14)
                {
                    player.itemRotation = MathHelper.PiOver2 + MathHelper.PiOver4;
                    if (player.direction == 1)
                        player.itemRotation = -player.itemRotation;
                }
                else
                {
                    player.itemRotation += (player.direction == -1) ? -MathHelper.PiOver4 / 2.0f : MathHelper.PiOver4 / 2.0f;
                    Rectangle hitbox = new Rectangle((int)player.Center.X - ((player.direction == -1) ? item.width : 0), (int)player.Center.Y - item.height / 2, item.width, item.height);
                    foreach (NPC npc in Main.npc)
                    {
                        if (hitbox.Intersects(npc.getRect()) && UnenpoweredHitCooldown[npc.whoAmI] <= 0 && !npc.friendly && !npc.dontTakeDamage && npc.active)
                        {
                            npc.StrikeNPC(Main.DamageVar(item.damage), item.knockBack, player.direction, Main.rand.Next(1, 101) <= item.crit ? true : false);
                            UnenpoweredHitCooldown[npc.whoAmI] = UnenpoweredHitCooldownMax;
                        }
                    }
                }
                if (player.itemAnimation == item.useAnimation - 14)
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item1, player.Center);
                if (player.itemRotation == 1)
                    item.noUseGraphic = true;
            }
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Player player = Main.LocalPlayer;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemType<CBar>(), 6)
                .AddIngredient(ItemID.Excalibur, 1)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}