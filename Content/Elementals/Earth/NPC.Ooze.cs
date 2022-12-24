using Entrogic.Core.BaseTypes;
using Entrogic.Helpers;

namespace Entrogic.Content.Elementals.Earth
{
    public class Ooze : NPCBase
    {
        // TODO: 翻译
        public override void SetStaticDefaults() {
            Main.npcFrameCount[Type] = 11;
        }

        public override void SetDefaults() {
            NPC.aiStyle = -1;
            NPC.lifeMax = 120;
            NPC.defense = 10;
            NPC.damage = 30;
            NPC.width = 80;
            NPC.height = 40;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.7f;
            NPC.Opacity = 0.9f;
            //DrawOffsetY = -10;
        }

        private Player Player => Main.player[NPC.target];

        public ref float JumpCount => ref NPC.ai[2];

        public override void AI() {
            if (JumpCount <= 5) {
                NPC.TargetClosest(true);
                if (Player.Exists() && (NPC.collideY || NPC.collideX)) {
                    NPC.direction = NPC.spriteDirection = Math.Sign(Player.Center.X - NPC.Center.X);
                }
            }
            else {
                JumpCount++;
                if (JumpCount >= 300 || NPC.justHit) {
                    JumpCount = 0;
                }
            }

            var targetCenter = Player.Center;
            var targetPosition = Player.position;

            if (Player.Bottom.Y == NPC.Bottom.Y)
                NPC.directionY = -1;

            float distance = NPC.Distance(Player.Center);
            bool hasHitablePlayer = Player.Exists() && Collision.CanHitLine(NPC.position, NPC.width, NPC.height,
                targetPosition, Player.width, Player.height);
            if (hasHitablePlayer && distance <= 160 && JumpCount <= 5) {
                // 硬核保持距离
                NPC.direction = NPC.spriteDirection = Math.Sign(targetCenter.X - NPC.Center.X);
                if (State == 0) {
                    if (distance > 80) {
                        State = 1;
                        Timer = 0;
                    }

                    if (NPC.collideY) {
                        Jump(NPC.Distance(Player.Center) > 80 ? 3 : 1);
                        NPC.velocity.X *= 1.6f;
                        if (Math.Abs(NPC.velocity.X) < 2f) {
                            NPC.velocity.X = NPC.direction * 2f;
                        }
                    }
                }
            }

            if (State == 1) {
                Timer++;
                if (Timer == 18 && NPC.velocity.Y < 0) {
                    Timer = 17;
                }

                if (Timer > 15) {
                    NPC.velocity.X *= 0.92f;
                }

                if (!hasHitablePlayer || Timer % 30 == 0) {
                    State = 0;
                }

                return;
            }

            float MaxXSpeed = 4f; // 最大速度
            const float AccelerateSpeed = 0.086f; // 加速度
            const float AccelerateSpeedRotating = 0.8f; // 转向加速度

            // 向左
            void AcceLeft() {
                NPC.velocity.X -= AccelerateSpeed;
                if (NPC.velocity.X <= -MaxXSpeed) {
                    NPC.velocity.X = -MaxXSpeed;
                }

                if (NPC.velocity.X >= 1f) {
                    NPC.velocity.X -= AccelerateSpeedRotating;
                }
            }

            // 向右
            void AcceRight() {
                NPC.velocity.X += AccelerateSpeed;
                if (NPC.velocity.X >= MaxXSpeed) {
                    NPC.velocity.X = MaxXSpeed;
                }

                if (NPC.velocity.X <= -1f) {
                    NPC.velocity.X += AccelerateSpeedRotating;
                }
            }

            // 寻路移动
            if (NPC.direction > 0) {
                AcceRight();
            }
            // 玩家在左边
            else {
                AcceLeft();
            }

            // 走上方块代码
            int LegY = (int) (NPC.Bottom.Y - 8) >> 4;
            if (NPC.collideY && NPC.velocity.Y == 0) {
                // 一旦在地面就试图StepUp
                NPC.stepSpeed = 2f;
                Collision.StepUp(ref NPC.position, ref NPC.velocity, NPC.width, NPC.height, ref NPC.stepSpeed,
                    ref NPC.gfxOffY, 1, true, 1);
            }

            void Jump(int height) {
                switch (height) {
                    case 1:
                        NPC.velocity.Y = -4.2f;
                        break;
                    case 2:
                        NPC.velocity.Y = -5.1f;
                        break;
                    case 3:
                        NPC.velocity.Y = -5.8f;
                        break;
                    case 4:
                        NPC.velocity.Y = -6.6f;
                        break;
                    case 5:
                        NPC.velocity.Y = -7.2f;
                        break;
                }
            }

            int WallHeightCount(int amount) {
                amount -= 1; // 不记录为0
                int tileWallHigh = 0;
                for (int j = -amount; j <= 1; j++) {
                    Tile t = Framing.GetTileSafely((int) (NPC.Center.X + NPC.direction * 50) >> 4, LegY + j);
                    if (Main.tileSolid[t.TileType] && t.HasUnactuatedTile && !t.IsActuated) {
                        tileWallHigh++;
                    }
                }

                return tileWallHigh;
            }

            // 跳跃
            if (NPC.velocity.Y == 0f) {
                if (NPC.collideX && NPC.velocity.X == 0f) {
                    Timer++;
                    if (Timer >= 15f) {
                        Jump(5);
                        Timer = 0;
                    }
                }

                // 如果能直线看到玩家，就向玩家去
                if (JumpCount <= 5 && Collision.CanHitLine(NPC.position, NPC.width, NPC.height, targetPosition,
                        Player.width, Player.height)) {
                    int toPlayerYTile = (int) (targetCenter.Y - NPC.Center.Y) >> 4;
                    int distanceXTile = Math.Abs((int) (targetCenter.X - NPC.Center.X) >> 4);
                    if (distanceXTile <= 6 && toPlayerYTile < -2) {
                        // 玩家离得近，高度比史莱姆高
                        Jump(Math.Min(5, -toPlayerYTile)); // 跳最高5格高度
                        JumpCount++;
                    }
                }
                // 否则爬墙
                else {
                    // 看看面前的墙有多高，往前向上寻找5格高看有多少墙
                    int tileWallHigh = WallHeightCount(5);
                    Jump(tileWallHigh - 1);
                }
            }
        }

        public override void FindFrame(int frameHeight) {
            switch (State) {
                case 0:
                    NPC.frameCounter += Math.Abs(NPC.velocity.X) * 0.05f;
                    NPC.frame.Y = (int) (NPC.frameCounter % 4) * frameHeight;
                    if (Math.Abs(NPC.velocity.X) <= 0.28f) {
                        NPC.frame.Y = 0;
                    }

                    break;
                case 1:
                    int frame = (int) (Timer / 5f) % 6;
                    NPC.frame.Y = (int) (frame + 4) * frameHeight;
                    break;
            }
        }

        public override bool ModifyCollisionData(Rectangle victimHitbox, ref int immunityCooldownSlot,
            ref float damageMultiplier, ref Rectangle npcHitbox) {
            if (State != 1 || !((Timer / 5 % 6) >= 4)) return true;

            damageMultiplier *= 1.75f;
            Rectangle mouth = new(0, 0, 20, 80) {
                Location = NPC.BottomRight.ToPoint()
            };
            if (NPC.direction < 0) {
                mouth.Location = NPC.BottomLeft.ToPoint();
                mouth.X -= mouth.Width;
            }

            mouth.Y -= mouth.Height;

            if (victimHitbox.Intersects(mouth)) {
                npcHitbox = mouth;
            }

            return true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
            //Rectangle mouth = new(0, 0, 30, 80) {
            //    Location = NPC.BottomRight.ToPoint()
            //};
            //if (NPC.direction < 0) {
            //    mouth.Location = NPC.BottomLeft.ToPoint();
            //    mouth.X -= mouth.Width;
            //}
            //mouth.Y -= mouth.Height;
            //ModHelper.DrawBorderedRect(Main.spriteBatch, Color.Transparent, Color.Red, mouth.Location.ToVector2() - Main.screenPosition, mouth.Size(), 2);
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }
    }
}