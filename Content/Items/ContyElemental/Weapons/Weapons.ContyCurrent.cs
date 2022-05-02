using Entrogic.Content.Items.BaseTypes;
using Entrogic.Content.Projectiles.ContyElemental.Friendly;

namespace Entrogic.Content.Items.ContyElemental.Weapons
{
    public class ContyCurrent : ItemBase
    {
        public override void Load() {
            base.Load();
            On.Terraria.Player.ItemCheck_Shoot += Player_ItemCheck_Shoot;
        }

        private void Player_ItemCheck_Shoot(On.Terraria.Player.orig_ItemCheck_Shoot orig, Player self, int i, Item sItem, int weaponDamage) {
            if (sItem.type == Type) {
                int projToShoot = sItem.shoot;
                int damage = sItem.damage;
                float KnockBack = sItem.knockBack;

                for (int p = 0; p < Main.maxProjectiles; p++) {
                    if (Main.projectile[p].active && Main.projectile[p].type == ModContent.ProjectileType<ContyCurrent_Proj2>() && Main.projectile[p].owner == self.whoAmI) {
                        orig(self, i, sItem, weaponDamage);
                        return;
                    }
                }
                SpawnMinionOnCursor(self, sItem, i, projToShoot, damage, KnockBack);
                return;
            }
            orig(self, i, sItem, weaponDamage);
        }

        private int SpawnMinionOnCursor(Player player, Item sItem, int ownerIndex, int minionProjectileId, int originalDamageNotScaledByMinionDamage, float KnockBack, Vector2 offsetFromCursor = default(Vector2), Vector2 velocityOnSpawn = default(Vector2)) {
            Vector2 pointPoisition = Main.MouseWorld;
            pointPoisition += offsetFromCursor;
            player.LimitPointToPlayerReachableArea(ref pointPoisition);
            int num = Projectile.NewProjectile(player.GetSource_ItemUse(sItem), pointPoisition, velocityOnSpawn, minionProjectileId, originalDamageNotScaledByMinionDamage, KnockBack, ownerIndex);
            Main.projectile[num].originalDamage = originalDamageNotScaledByMinionDamage;
            return num;
        }

        public override void SetStaticDefaults() {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            DisplayName.SetDefault("Electric Current");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "电流");
            Tooltip.SetDefault("Uncontrollable force of nature.");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "不可控的自然之力。");
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults() {
            Item.damage = 153;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 15;
            Item.width = 28;
            Item.height = 30;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true; //当然是让它不能近战攻击啦
            Item.knockBack = 5;
            Item.value = Item.sellPrice(0, 5);
            Item.rare = RarityLevelID.MiddleHM;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<ContyCurrent_Proj2>();
            Item.shootSpeed = 24f;
        }


        // 这个是仅当射弹幕那位存在时才执行的，一般不用担心找不到
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
            type = ModContent.ProjectileType<ContyCurrent_Proj>();
            for (int p = 0; p < Main.maxProjectiles; p++) {
                if (Main.projectile[p].active && Main.projectile[p].type == ModContent.ProjectileType<ContyCurrent_Proj2>() && Main.projectile[p].owner == player.whoAmI) {
                    position = Main.projectile[p].Center;
                    velocity = Vector2.Normalize(Main.MouseWorld - position) * Item.shootSpeed;
                    break;
                }
            }
            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);

            // 一些预判相关，现在不再需要了
            //Vector2 projToMus = Vector2.Normalize(Main.MouseWorld - position) * Item.shootSpeed;
            //float maxDis = 1600f;
            //NPC target = null;
            //// 选取最近NPC，如果target是null说明没有临近的敌人
            //foreach (var NPC in Main.npc)
            //{
            //    if (NPC.active && !NPC.friendly && NPC.value > 0 && !NPC.dontTakeDamage)
            //    {
            //        float dis = Vector2.Distance(NPC.Center, Main.MouseWorld);
            //        if (dis < maxDis)
            //        {
            //            maxDis = dis;
            //            target = NPC;
            //        }
            //    }
            //}
            //if (target != null)
            //{
            //    // 获取从玩家到NPC的向量
            //    Projectile.NewProjectile(position, GetShootVector(target, position, Item.shootSpeed, projToMus), type, damage, knockBack, player.whoAmI);
            //    return false;
            //}
            //else
            //{
            //return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
            //}
        }

        // 来自fs49.org
        //private Vector2 GetShootVector(NPC NPC, Vector2 pos, float speed, Vector2 defaultSpeed)
        //{
        //    // 一开始我们假设往NPC现在所处位置发射弹幕
        //    Vector2 target = NPC.Center - pos;
        //    for (int i = 0; i < 12; i++)
        //    {
        //        // 获取弹幕的飞行时间
        //        float t = (target - pos).Length() / speed;
        //        // 过了这么多时间以后NPC到哪里了？
        //        Vector2 NPCPos = NPC.Center + t * NPC.velocity;
        //        // 我们射到的位置是否距离NPC最终位置足够近？
        //        if (Vector2.Distance(target, NPCPos) < 0.1f)
        //        {
        //            // 足够近，我们直接返回这个发射向量
        //            return (target - pos).ToRotation().ToRotationVector2() * speed;
        //        }
        //        // 不够近，我们把目标位置改动一下，进行下一次尝试
        //        target = NPCPos;
        //    }
        //    Vector2 projToNPC = NPC.Center - pos;
        //    // 怎么样都不够近，乱给一个解方程
        //    float G = CrossProduct(projToNPC, NPC.velocity) * speed / projToNPC.Length();
        //    // 再不行，直接向着鼠标发射吧
        //    if (G > 1 || G < -1)
        //    {
        //        defaultSpeed.Normalize();
        //        return projToNPC.SafeNormalize(defaultSpeed) * speed;
        //    }
        //    float offset = target.ToRotation();
        //    float realr = (float)(offset + Math.Asin(G));
        //    return realr.ToRotationVector2();
        //}

        //private float CrossProduct(Vector2 v1, Vector2 v2)
        //{
        //    return v1.X * v2.Y - v1.Y * v2.X;
        //}
    }
}
