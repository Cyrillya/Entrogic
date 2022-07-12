using Terraria.Audio;

namespace Entrogic.Content.Projectiles.Misc.Weapons.Melee.Swords.TwoHandSword
{
    public abstract class TwoHandSwordBase : ModProjectile
    {
        // 一个贴图，无实际碰撞箱，伤害等
        public override string Texture => $"{GetType().Namespace.Replace('.', '/')}/SwordProj";
        public override void SetDefaults() {
            Projectile.width = 0;
            Projectile.height = 0;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.scale = 2f;
        }

        public override bool? CanDamage() {
            return false;
        }

        internal short LocalAI_RotateFactor_MAX = 60;
        public float LocalAI_RotateFactor {
            get => Projectile.localAI[0];
            set => Projectile.localAI[0] = value;
        }
        private bool _rotateMode;
        public float LocalAI_RotateShake {
            get => Projectile.localAI[1];
            set => Projectile.localAI[1] = value;
        }

        internal short AI_CHARGE_MAX = 30;
        public float AI_Charge {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }
        internal Vector2 Offset = new(0, -85);

        public override void AI() {
            base.AI();
            Player player = Main.player[Projectile.owner];
            player.itemTime = 2;
            player.itemAnimation = 2;
            //player.heldProj = Projectile.whoAmI;
            // 如果玩家仍然在控制弹幕
            if (player.channel) {
                // 调整玩家转向以及手持物品的转动方向
                player.direction = Main.MouseWorld.X < player.Center.X ? -1 : 1;
                Projectile.spriteDirection = player.direction;
                Projectile.timeLeft = 20;
                Projectile.Center = player.Center + Offset;
                if (Main.netMode != NetmodeID.Server) {
                    if (LocalAI_RotateFactor == 0) {
                        LocalAI_RotateFactor = 1;
                    }
                    if (LocalAI_RotateFactor >= 1 && LocalAI_RotateFactor < LocalAI_RotateFactor_MAX) {
                        LocalAI_RotateFactor += 5.8f + LocalAI_RotateFactor * 0.4f;
                    }
                }
                if (AI_Charge < AI_CHARGE_MAX)
                    AI_Charge += 1f * player.GetAttackSpeed(Projectile.DamageType); // 根据速度减少使用时间
                if (AI_Charge > AI_CHARGE_MAX) AI_Charge = AI_CHARGE_MAX;
                Channeling();
            }
            else {
                Unchanneling();
                if (AI_Charge < AI_CHARGE_MAX)
                    AI_Charge -= 2f; // 蓄力回退视觉效果
                // 视觉效果的微调
                if (Projectile.timeLeft == 14) {
                    int direction = Projectile.spriteDirection;
                    SummonProjectile(out int type, out Vector2 offset, ref direction, out float damageMult);
                    Projectile p = Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Projectile.Center + offset, Vector2.Zero, type, (int)(Projectile.damage * damageMult), Projectile.knockBack, Projectile.owner);
                    p.spriteDirection = direction;
                    SoundEngine.PlaySound(SoundID.Item71, Projectile.Center);

                    if (!Main.dedServ) {
                        if (AI_Charge < AI_CHARGE_MAX) {
                            CombatText.NewText(player.getRect(), Color.OrangeRed, "Fail!", true);
                        }
                        else {
                            CombatText.NewText(player.getRect(), Color.CornflowerBlue, "Combo!", true);
                        }
                    }
                }
                if (Projectile.timeLeft > 10) {
                    Projectile.Center = player.Center + Offset;
                }

                ComboController(player);
            }

            LocalAI_RotateShake += 0.7f * _rotateMode.ToDirectionInt();
            if (LocalAI_RotateShake >= 15 || LocalAI_RotateShake <= 0) _rotateMode = !_rotateMode;
            LocalAI_RotateShake %= 6;
            //Projectile.rotation += MathHelper.ToRadians(LocalAI_RotateShake);
        }

        public virtual void Unchanneling() { }

        public virtual void Channeling() { }

        public virtual void SummonProjectile(out int type, out Vector2 offset, ref int direction, out float damageMult) { type = ModContent.ProjectileType<SwordCombo1>(); offset = Vector2.Zero; damageMult = 1f; }

        public virtual void ComboController(Player player) { }

        public override bool ShouldUpdatePosition() {
            return false;
        }

        public override bool PreDraw(ref Color lightColor) {
            Asset<Texture2D> tex = TextureAssets.Projectile[Type];
            //Main.NewText(FindFrame(tex.Height() / frameYCount, tex.Width() / frameXCount));
            SpriteEffects spriteEffects = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Vector2 origin = new(5, tex.Height() - 5);
            Vector2 drawPos = (Projectile.position + origin) - Main.screenPosition;
            float rotation = Projectile.rotation; // 镜像翻转的问题，修正
            if (spriteEffects == SpriteEffects.FlipHorizontally) { // 修正镜像翻转问题
                origin.X = tex.Width() - 5;
                drawPos = (Projectile.position + origin) - Main.screenPosition;
                drawPos.X -= tex.Width(); // 确保剑端位置唯一
                rotation = -rotation;
            }

            if (Projectile.timeLeft > 14) {
                Main.EntitySpriteDraw(tex.Value, drawPos.Floor(), null, Color.White, rotation, origin, Projectile.scale, spriteEffects, 0);
            }

            if (AI_Charge != AI_CHARGE_MAX) {
                Player player = Main.player[Projectile.owner];
                tex = ResourceManager.Miscellaneous["ComboRing"];
                if (!player.channel) {
                    Main.EntitySpriteDraw(tex.Value, player.Center - Main.screenPosition, null, Color.Red * (AI_Charge / AI_CHARGE_MAX), 0f, tex.Size() / 2f, MathHelper.Lerp(1f, 0f, AI_Charge / AI_CHARGE_MAX) + 0.1f, SpriteEffects.None, 0);
                }
                else {
                    Main.EntitySpriteDraw(tex.Value, player.Center - Main.screenPosition, null, Color.White * (AI_Charge / AI_CHARGE_MAX), 0f, tex.Size() / 2f, MathHelper.Lerp(1f, 0f, AI_Charge / AI_CHARGE_MAX) + 0.1f, SpriteEffects.None, 0);
                }
            }

            return false;
        }
    }
}
