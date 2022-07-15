using Entrogic.Content.NPCs.BaseTypes;
using Terraria.Audio;
using Terraria.ID;

namespace Entrogic.Content.NPCs.Enemies.Athanasy
{
    public class StoneFairyCritter : NPCBase
    {
        enum AIState
        {
            FlyToTarget,
            CircleOnTarget
        }

        public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = 4;
		}

        public override void SetDefaults() {
            AnimationType = NPCID.FairyCritterBlue;
			NPC.width = 18;
			NPC.height = 20;
			NPC.damage = 0;
			NPC.defense = 0;
			NPC.lifeMax = 5;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.dontTakeDamage = true;
			NPC.dontTakeDamageFromHostiles = true;
			NPC.aiStyle = -1;
			NPC.scale = 0f;
		}

        public Vector2 TargetPosition => new(NPC.ai[2], NPC.ai[3]);

        public override void AI() {
			switch (State) {
				case (int)AIState.FlyToTarget: {
						Timer++;
						// 一出来先停顿一会
						if (Timer < 50) {
							NPC.scale = Math.Min(Timer, 30f) / 30f;
							NPC.width = 1;
							NPC.height = 1;
							return;
						}

						if (Timer == 50) {
							var position = NPC.position;
							NPC.scale = 1f;
							NPC.width = 18;
							NPC.height = 20;
							NPC.Center = position;
							NPC.position.Y -= 8f;
						}

						Rectangle targetRectangle = Utils.CenteredRectangle(TargetPosition, Vector2.One * 5f);
						if (NPC.Hitbox.Intersects(targetRectangle)) {
							SwitchState((int)AIState.CircleOnTarget);
							Timer = 0;
							NPC.netUpdate = true;
							NPC.TargetClosest(false);
							break;
						}

						Vector2 targetPosition = targetRectangle.ClosestPointInRect(NPC.Center);
						Vector2 speedLerp = NPC.DirectionTo(targetPosition);
						float distance = NPC.Distance(targetPosition);
						if (distance > 150f)
							speedLerp *= 3f;
						else if (distance > 80f)
							speedLerp *= 2f;

						Point point2 = NPC.Center.ToTileCoordinates();
						NPC.velocity = Vector2.Lerp(NPC.velocity, speedLerp, 0.2f);
						GetBirdFlightRecommendation(4, 2, point2, out bool goDownwards2, out bool goUpwards2);
						if (goDownwards2)
							NPC.velocity.Y += 0.14f;

						if (goUpwards2)
							NPC.velocity.Y -= 0.14f;

						if (NPC.velocity.Y > 4f)
							NPC.velocity.Y = 4f;

						if (NPC.velocity.Y < -4f)
							NPC.velocity.Y = -4f;
						break;
					}
				case (int)AIState.CircleOnTarget: {
						if (Timer == 15f)
							SoundEngine.PlaySound(SoundID.Pixie, NPC.position);

						if (Timer <= 15f) {
							NPC.velocity *= 0.9f;
						}
						else {
							float num3 = Timer - 15f;
							int num5 = (int)(num3 / 50f);
							float num2 = (float)Math.Cos(num5 * 1f) * ((float)Math.PI * 2f) / 16f;
							float num4 = (float)Math.Cos(num5 * 2f) * 10f + 8f;
							num2 *= NPC.direction;
							Vector2 fairyCircleOffset = GetFairyCircleOffset(num3 / 50f, num2, num4);
							Vector2 fairyCircleOffset2 = GetFairyCircleOffset(num3 / 50f + 0.02f, num2, num4);
							NPC.velocity = fairyCircleOffset2 - fairyCircleOffset;
							if (Main.player[NPC.target].Center.X > NPC.Center.X)
								NPC.spriteDirection = -1;
							else
								NPC.spriteDirection = 1;
						}

						Timer++;
						if (Main.netMode != NetmodeID.MultiplayerClient && Timer > 200f) {
							NPC.active = false;

							var tileCoord = TargetPosition.ToTileCoordinates16();
                            Tile tile = Framing.GetTileSafely(tileCoord);
                            int topX = tileCoord.X - tile.TileFrameX / 18 % 3;
                            int topY = tileCoord.Y - tile.TileFrameY / 18 % 2;

                            for (int k = 0; k < 3; k++) {
                                for (int l = 0; l < 2; l++) {
                                    if (Main.tile[topX + k, topY + l].TileFrameY < 36) {
                                        Main.tile[topX + k, topY + l].TileFrameY += 36;
                                    }
                                }
                            }

                            if (Main.netMode == NetmodeID.SinglePlayer) {
								NPC.FairyEffects(NPC.Center, 2); // 2是蓝色的
							}
							else if (Main.netMode == NetmodeID.Server) {
								NPC.netSkip = -1;
								NPC.life = 0;
								NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, NPC.whoAmI);
								NetMessage.SendData(MessageID.SpecialFX, -1, -1, null, 2, (int)NPC.Center.X, (int)NPC.Center.Y, 0f, 2);
                                NetMessage.SendTileSquare(-1, topX, topY, 3, 2);
							}
						}
						break;
					}
			}

			Color colorLight = Color.RoyalBlue;
			Color colorDust = Color.LightBlue;
			int offset = 4;

			if ((int)Main.timeForVisualEffects % 2 == 0) {
				NPC.position += NPC.netOffset;
				Dust dust = Dust.NewDustDirect(NPC.Center - new Vector2(offset) * 0.5f, offset + 4, offset + 4, DustID.FireworksRGB, 0f, 0f, 200, Color.Lerp(colorLight, colorDust, Main.rand.NextFloat()), 0.65f);
				dust.velocity *= 0f;
				dust.velocity += NPC.velocity * 0.3f;
				dust.noGravity = true;
				dust.noLight = true;
				NPC.position -= NPC.netOffset;
			}

			Lighting.AddLight(NPC.Center, colorLight.ToVector3() * 0.7f);
		}

		private Vector2 GetFairyCircleOffset(float elapsedTime, float circleRotation, float circleHeight) =>
			((((float)Math.PI * 2f * elapsedTime + (float)Math.PI / 2f).ToRotationVector2() + new Vector2(0f, -1f)) * new Vector2(6 * -NPC.direction, circleHeight)).RotatedBy(circleRotation);

		private void GetBirdFlightRecommendation(int downScanRange, int upRange, Point tCoords, out bool goDownwards, out bool goUpwards) {
			tCoords.X += NPC.direction;
			goDownwards = true;
			goUpwards = false;
			int x = tCoords.X;
			int num = tCoords.Y;
			while (true) {
				if (num < tCoords.Y + downScanRange && WorldGen.InWorld(x, num)) {
					Tile tile = Main.tile[x, num];
					if (tile != null) {
						if ((!tile.HasUnactuatedTile && Main.tileSolid[tile.TileType]) || tile.LiquidType > 0)
							break;

						num++;
						continue;
					}

					return;
				}

				return;
			}

			if (num < tCoords.Y + upRange)
				goUpwards = true;

			goDownwards = false;
		}
	}
}
