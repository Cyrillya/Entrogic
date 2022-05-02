/*using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace Entrogic.Items.Weapons.Melee.Sword
{
    public class ChaosSword : ModItem
    {
        public bool enpowered = false;
        public float charge = 0;
        public override void SetDefaults()
        {
            item.UseSound = SoundID.Item1;
            item.value = Item.sellPrice(0, 7, 24, 0);
            item.crit += 4;
            item.damage = 68;           //The damage of your weapon �������˺�
            item.width = 88;            //Weapon's texture's width �������ʵĿ��
            item.height = 88;           //Weapon's texture's height �������ʵĸ߶�
            item.useTime = 6;          //The time span of using the weapon. Remember in terraria, 60 frames is a second. ʹ���������ٶȵ�ʱ����(��֡Ϊ��λ)����Terraria�У�60֡��һ�롣
            item.useAnimation = 6;         //The time span of the using animat3ion of the weapon, suggest set it the same as useTime. ʹ�������Ķ�����ʱ����(��֡Ϊ��λ),ͨ����useTime���ó�һ��
            item.useStyle = 5;
            item.knockBack = 8;         //The force of knockback of the weapon. Maximum is 20 �����Ļ���,�����20
            item.rare = RareID.LV6;              //The rarity of the weapon, from -1 to 13 ������ϡ�ж�,��-1��13   //The sound when the weapon is using ʹ������ʱ����Ч
            item.autoReuse = false;
            item.channel = true;
            item.useTurn = true;
            item.shoot = ModContent.ProjectileType<������־>();
            item.shootSpeed = 20f;
            item.noUseGraphic = true;
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
                item.noUseGraphic = item.autoReuse = false;
                item.useTime = 60;
                item.useAnimation = 60;
                item.noMelee = true;
            }
            else
            {
                if (enpowered)
                {
                    item.useAnimation = item.useTime = 60;
                    item.UseSound = SoundID.Item45;
                    item.shoot = ModContent.ProjectileType<GodBeamFri>();
                }
                else
                {
                    item.useTime = 6;
                    item.useAnimation = 6;
                    item.UseSound = SoundID.Item1;
                    item.shoot = ModContent.ProjectileType<������־>();
                }
                item.noUseGraphic = true;
            }
            return base.CanUseItem(player);
        }
        public override void ModifyHitNPC(Player player, NPC npc, ref int damage, ref float knockBack, ref bool crit)
        {
            if (Main.rand.Next(3) == 0)
            {
                npc.AddBuff(BuffType<Buffs.Weapons.Unconsciousness>(), 150);
            }
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
                            Projectile.NewProjectile(position.X, position.Y, 24f * i, -0.1f, ModContent.ProjectileType<Dash>(), damage, knockBack, Main.myPlayer, 0f, 1f);
                        }
                        charge = 0;
                    }
                }
                else
                    return true;
            }
            item.damage = 38;
            item.shootSpeed = 20f;
            return false;
        }
        public int charge2 = 0;
        public override void UseStyle(Player player)
        {
            if (enpowered)
            {
                int i = 0;
                if (Main.MouseWorld.X - player.Center.X > 0) i = 1;
                else i = -1;
                player.direction = i;
                if (charge2 % 2 == 0)
                {
                    int j = Projectile.NewProjectile(player.position, player.velocity, ModContent.ProjectileType<��ЧBowGod>(), 0, 0, player.whoAmI);
                    Main.projectile[j].timeLeft = (int)(charge * 0.148f) + 5;
                }
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
                    if (item.damage >= 350) item.damage = 100;
                    item.damage += (int)(1 + (charge / 115f));
                    item.shootSpeed += (1 + (charge / 45f));
                }
                else if (charge > 0) player.itemTime = 0;
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "CBar", 6);
            recipe.AddIngredient(ItemID.Excalibur, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}*/