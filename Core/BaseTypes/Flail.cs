namespace Entrogic.Core.BaseTypes
{
    public abstract class Flail : ProjectileBase
	{
		public string ChainTexturePath => GetType().FullName.Replace('.', '/') + "_Chain";

		protected enum AIState
		{
			Spinning,
			LaunchingForward,
			Retracting,
			ForcedRetracting,
			Ricochet,
			Dropping
		}

		protected AIState CurrentAIState {
			get => (AIState)Projectile.ai[0];
			set => Projectile.ai[0] = (float)value;
		}
		public ref float StateTimer => ref Projectile.ai[1];
		public ref float CollisionCounter => ref Projectile.localAI[0];
		public ref float SpinningStateTimer => ref Projectile.localAI[1];

		public override void SetStaticDefaults() {
			// These lines facilitate the trail drawing
			ProjectileID.Sets.TrailCacheLength[Type] = 6;
			ProjectileID.Sets.TrailingMode[Type] = 2;
		}

		public virtual void SetFlailDefaults() { }

		/// <summary>
		/// How much time the projectile can go before retracting (speed and shootTimer will set the flail's range)
		/// </summary>
		public int LaunchTimeLimit = 15;
		/// <summary>
		/// How fast the projectile can move
		/// </summary>
		public float LaunchSpeed = 14f;
		/// <summary>
		/// How far the projectile's chain can stretch before being forced to retract when in launched state
		/// </summary>
		public float MaxLaunchLength = 800f;
		/// <summary>
		/// How quickly the projectile will accelerate back towards the player while retracting
		/// </summary>
		public float RetractAcceleration = 3f;
		/// <summary>
		/// The max speed the projectile will have while retracting
		/// </summary>
		public float MaxRetractSpeed = 10f;
		/// <summary>
		/// How quickly the projectile will accelerate back towards the player while being forced to retract
		/// </summary>
		public float ForcedRetractAcceleration = 6f;
		/// <summary>
		/// The max speed the projectile will have while being forced to retract
		/// </summary>
		public float MaxForcedRetractSpeed = 15f;
		/// <summary>
		/// How often your flail hits when resting on the ground, or retracting
		/// </summary>
		public int DefaultHitCooldown = 10;
		/// <summary>
		/// How often your flail hits when spinning
		/// </summary>
		public int SpinHitCooldown = 20;
		/// <summary>
		/// How often your flail hits when moving
		/// </summary>
		public int MovingHitCooldown = 10;

		public override sealed void SetDefaults() {
			Projectile.netImportant = true; // This ensures that the projectile is synced when other players join the world.
			Projectile.width = 24; // The width of your projectile
			Projectile.height = 24; // The height of your projectile
			Projectile.friendly = true; // Deals damage to enemies
			Projectile.penetrate = -1; // Infinite pierce
			Projectile.DamageType = DamageClass.Melee; // Deals melee damage
			Projectile.usesLocalNPCImmunity = true; // Used for hit cooldown changes in the ai hook
			Projectile.localNPCHitCooldown = 10; // This facilitates custom hit cooldown logic
			SetFlailDefaults();
			// Vanilla flails all use aiStyle 15, but the code isn't customizable so an adaption of that aiStyle is used in the AI method
		}

		// This AI code was adapted from vanilla code: Terraria.Projectile.AI_015_Flails() 
		public override void AI() {
			Player player = Main.player[Projectile.owner];
			// Kill the projectile if the player dies or gets crowd controlled
			if (!player.active || player.dead || player.noItems || player.CCed || Vector2.Distance(Projectile.Center, player.Center) > 900f) {
				Projectile.Kill();
				return;
			}
			if (Main.myPlayer == Projectile.owner && Main.mapFullscreen) {
				Projectile.Kill();
				return;
			}

			Vector2 mountedCenter = player.MountedCenter;
			bool doFastThrowDust = false;
			bool shouldOwnerHitCheck = false;
			int ricochetTimeLimit = LaunchTimeLimit + 5;

			// Scaling these speeds and accelerations by the players meleeSpeed make the weapon more responsive if the player boosts their meleeSpeed
			float meleeSpeed = player.GetAttackSpeed(DamageClass.Melee);
			float meleeSpeedMultiplier = 1f / meleeSpeed;
			float launchSpeed = LaunchSpeed * meleeSpeedMultiplier;
			float retractAcceleration = RetractAcceleration * meleeSpeedMultiplier;
			float maxRetractSpeed = MaxRetractSpeed * meleeSpeedMultiplier;
			float forcedRetractAcceleration = ForcedRetractAcceleration * meleeSpeedMultiplier;
			float maxForcedRetractSpeed = MaxForcedRetractSpeed * meleeSpeedMultiplier;
			float launchRange = LaunchSpeed * LaunchTimeLimit;
			float maxDroppedRange = launchRange + 160f;
			Projectile.localNPCHitCooldown = DefaultHitCooldown;

			switch (CurrentAIState) {
				case AIState.Spinning: {
						shouldOwnerHitCheck = true;
						if (Projectile.owner == Main.myPlayer) {
							Vector2 unitVectorTowardsMouse = mountedCenter.DirectionTo(Main.MouseWorld).SafeNormalize(Vector2.UnitX * player.direction);
							player.ChangeDir((unitVectorTowardsMouse.X > 0f) ? 1 : (-1));
							if (!player.channel) // If the player releases then change to moving forward mode
							{
								CurrentAIState = AIState.LaunchingForward;
								StateTimer = 0f;
								Projectile.velocity = unitVectorTowardsMouse * launchSpeed + player.velocity;
								Projectile.Center = mountedCenter;
								Projectile.netUpdate = true;
								Projectile.ResetLocalNPCHitImmunity();
								Projectile.localNPCHitCooldown = MovingHitCooldown;
								break;
							}
						}
						SpinningStateTimer += 1f;
						// This line creates a unit vector that is constantly rotated around the player. 10f controls how fast the projectile visually spins around the player
						Vector2 offsetFromPlayer = new Vector2(player.direction).RotatedBy((float)Math.PI * 10f * (SpinningStateTimer / 60f) * player.direction);

						offsetFromPlayer.Y *= 0.8f;
						if (offsetFromPlayer.Y * player.gravDir > 0f) {
							offsetFromPlayer.Y *= 0.5f;
						}
						Projectile.Center = mountedCenter + offsetFromPlayer * 30f;
						Projectile.velocity = Vector2.Zero;
						Projectile.localNPCHitCooldown = SpinHitCooldown; // set the hit speed to the spinning hit speed
						break;
					}
				case AIState.LaunchingForward: {
						doFastThrowDust = true;
						bool shouldSwitchToRetracting = StateTimer++ >= LaunchTimeLimit;
						shouldSwitchToRetracting |= Projectile.Distance(mountedCenter) >= MaxLaunchLength;
						if (player.controlUseItem) // If the player clicks, transition to the Dropping state
						{
							CurrentAIState = AIState.Dropping;
							StateTimer = 0f;
							Projectile.netUpdate = true;
							Projectile.velocity *= 0.2f;
							// This is where Drippler Crippler spawns its projectile
							/*
							if (Main.myPlayer == Projectile.owner)
								Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), Projectile.Center, Projectile.velocity, 928, Projectile.damage, Projectile.knockBack, Main.myPlayer);
							*/
							if (Main.myPlayer == Projectile.owner) {
								ShootProjectile();
							}
							break;
						}
						if (shouldSwitchToRetracting) {
							CurrentAIState = AIState.Retracting;
							StateTimer = 0f;
							Projectile.netUpdate = true;
							Projectile.velocity *= 0.3f;
							// This is also where Drippler Crippler spawns its projectile, see above code.
							if (Main.myPlayer == Projectile.owner) {
								ShootProjectile();
							}
						}
						player.ChangeDir((player.Center.X < Projectile.Center.X) ? 1 : (-1));
						Projectile.localNPCHitCooldown = MovingHitCooldown;
						break;
					}
				case AIState.Retracting: {
						Vector2 unitVectorTowardsPlayer = Projectile.DirectionTo(mountedCenter).SafeNormalize(Vector2.Zero);
						if (Projectile.Distance(mountedCenter) <= maxRetractSpeed) {
							Projectile.Kill(); // Kill the projectile once it is close enough to the player
							return;
						}
						if (player.controlUseItem) // If the player clicks, transition to the Dropping state
						{
							CurrentAIState = AIState.Dropping;
							StateTimer = 0f;
							Projectile.netUpdate = true;
							Projectile.velocity *= 0.2f;
						}
						else {
							Projectile.velocity *= 0.98f;
							Projectile.velocity = Projectile.velocity.MoveTowards(unitVectorTowardsPlayer * maxRetractSpeed, retractAcceleration);
							player.ChangeDir((player.Center.X < Projectile.Center.X) ? 1 : (-1));
						}
						break;
					}
				case AIState.ForcedRetracting: {
						Projectile.tileCollide = false;
						Vector2 unitVectorTowardsPlayer = Projectile.DirectionTo(mountedCenter).SafeNormalize(Vector2.Zero);
						if (Projectile.Distance(mountedCenter) <= maxForcedRetractSpeed) {
							Projectile.Kill(); // Kill the projectile once it is close enough to the player
							return;
						}
						Projectile.velocity *= 0.98f;
						Projectile.velocity = Projectile.velocity.MoveTowards(unitVectorTowardsPlayer * maxForcedRetractSpeed, forcedRetractAcceleration);
						Vector2 target = Projectile.Center + Projectile.velocity;
						Vector2 value = mountedCenter.DirectionFrom(target).SafeNormalize(Vector2.Zero);
						if (Vector2.Dot(unitVectorTowardsPlayer, value) < 0f) {
							Projectile.Kill(); // Kill projectile if it will pass the player
							return;
						}
						player.ChangeDir((player.Center.X < Projectile.Center.X) ? 1 : (-1));
						break;
					}
				case AIState.Ricochet:
					if (StateTimer++ >= ricochetTimeLimit) {
						CurrentAIState = AIState.Dropping;
						StateTimer = 0f;
						Projectile.netUpdate = true;
					}
					else {
						Projectile.localNPCHitCooldown = MovingHitCooldown;
						Projectile.velocity.Y += 0.6f;
						Projectile.velocity.X *= 0.95f;
						player.ChangeDir((player.Center.X < Projectile.Center.X) ? 1 : (-1));
					}
					break;
				case AIState.Dropping:
					if (!player.controlUseItem || Projectile.Distance(mountedCenter) > maxDroppedRange) {
						CurrentAIState = AIState.ForcedRetracting;
						StateTimer = 0f;
						Projectile.netUpdate = true;
					}
					else {
						Projectile.velocity.Y += 0.8f;
						Projectile.velocity.X *= 0.95f;
						player.ChangeDir((player.Center.X < Projectile.Center.X) ? 1 : (-1));
					}
					break;
			}

			// This is where Flower Pow launches projectiles. Decompile Terraria to view that code.
			if (Projectile.owner == Main.myPlayer) {
				ThrowProjectile();
			}

			Projectile.direction = (player.direction > 0f) ? 1 : -1;
			Projectile.spriteDirection = Projectile.direction;
			Projectile.ownerHitCheck = shouldOwnerHitCheck; // This prevents attempting to damage enemies without line of sight to the player. The custom Colliding code for spinning makes this necessary.

			// This rotation code is unique to this flail, since the sprite isn't rotationally symmetric and has tip.
			bool freeRotation = CurrentAIState == AIState.Ricochet || CurrentAIState == AIState.Dropping;
			if (ModifyRotation(ref freeRotation)) {
				if (freeRotation) {
					if (Projectile.velocity.Length() > 1f)
						Projectile.rotation = Projectile.velocity.ToRotation() + Projectile.velocity.X * 0.1f; // skid
					else
						Projectile.rotation += Projectile.velocity.X * 0.1f; // roll
				}
				else {
					Vector2 vectorTowardsPlayer = Projectile.DirectionTo(mountedCenter).SafeNormalize(Vector2.Zero);
					Projectile.rotation = vectorTowardsPlayer.ToRotation() + MathHelper.PiOver2;
				}
			}

			Projectile.timeLeft = 2; // Makes sure the flail doesn't die (good when the flail is resting on the ground)
			player.heldProj = Projectile.whoAmI;
			player.SetDummyItemTime(2); //Add a delay so the player can't button mash the flail
			player.itemRotation = Projectile.DirectionFrom(mountedCenter).ToRotation();
			if (Projectile.Center.X < mountedCenter.X) {
				player.itemRotation += (float)Math.PI;
			}
			player.itemRotation = MathHelper.WrapAngle(player.itemRotation);

			PostAI(doFastThrowDust);
		}

		/// <summary>
		/// 滴滴怪跛子那样的，丢出去生成射弹
		/// </summary>
		public virtual void ShootProjectile() { }

		/// <summary>
		/// 花冠那样的只要AI在运行就每隔一段时间生成射弹
		/// </summary>
		public virtual void ThrowProjectile() { }

		public virtual bool ModifyRotation(ref bool freeRotation) { return true; }

		public virtual void PostAI(bool doFastThrowDust) { }

		public override bool OnTileCollide(Vector2 oldVelocity) {
			int defaultLocalNPCHitCooldown = 10;
			int impactIntensity = 0;
			Vector2 velocity = Projectile.velocity;
			float bounceFactor = 0.2f;
			if (CurrentAIState == AIState.LaunchingForward || CurrentAIState == AIState.Ricochet)
				bounceFactor = 0.4f;

			if (CurrentAIState == AIState.Dropping)
				bounceFactor = 0f;

			if (oldVelocity.X != Projectile.velocity.X) {
				if (Math.Abs(oldVelocity.X) > 4f)
					impactIntensity = 1;

				Projectile.velocity.X = (0f - oldVelocity.X) * bounceFactor;
				CollisionCounter += 1f;
			}

			if (oldVelocity.Y != Projectile.velocity.Y) {
				if (Math.Abs(oldVelocity.Y) > 4f)
					impactIntensity = 1;

				Projectile.velocity.Y = (0f - oldVelocity.Y) * bounceFactor;
				CollisionCounter += 1f;
			}

			// If in the Launched state, spawn sparks
			if (CurrentAIState == AIState.LaunchingForward) {
				CurrentAIState = AIState.Ricochet;
				Projectile.localNPCHitCooldown = defaultLocalNPCHitCooldown;
				Projectile.netUpdate = true;
				Point scanAreaStart = Projectile.TopLeft.ToTileCoordinates();
				Point scanAreaEnd = Projectile.BottomRight.ToTileCoordinates();
				impactIntensity = 2;
				Projectile.CreateImpactExplosion(2, Projectile.Center, ref scanAreaStart, ref scanAreaEnd, Projectile.width, out bool causedShockwaves);
				Projectile.CreateImpactExplosion2_FlailTileCollision(Projectile.Center, causedShockwaves, velocity);
				Projectile.position -= velocity;
			}

			// Here the tiles spawn dust indicating they've been hit
			if (impactIntensity > 0) {
				Projectile.netUpdate = true;
				for (int i = 0; i < impactIntensity; i++) {
					Collision.HitTiles(Projectile.position, velocity, Projectile.width, Projectile.height);
				}

				SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
			}

			// Force retraction if stuck on tiles while retracting
			if (CurrentAIState != AIState.Spinning && CurrentAIState != AIState.Ricochet && CurrentAIState != AIState.Dropping && CollisionCounter >= 10f) {
				CurrentAIState = AIState.ForcedRetracting;
				Projectile.netUpdate = true;
			}

			// tModLoader currently does not provide the wetVelocity parameter, this code should make the flail bounce back faster when colliding with tiles underwater.
			//if (Projectile.wet)
			//	wetVelocity = Projectile.velocity;

			return false;
		}

		public override bool? CanDamage() {
			// Flails in spin mode won't damage enemies within the first 12 ticks. Visually this delays the first hit until the player swings the flail around for a full spin before damaging anything.
			if (CurrentAIState == AIState.Spinning && SpinningStateTimer <= 12f)
				return false;
			return base.CanDamage();
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			// Flails do special collision logic that serves to hit anything within an ellipse centered on the player when the flail is spinning around the player. For example, the projectile rotating around the player won't actually hit a bee if it is directly on the player usually, but this code ensures that the bee is hit. This code makes hitting enemies while spinning more consistant and not reliant of the actual position of the flail projectile.
			if (CurrentAIState == AIState.Spinning) {
				Vector2 mountedCenter = Main.player[Projectile.owner].MountedCenter;
				Vector2 shortestVectorFromPlayerToTarget = targetHitbox.ClosestPointInRect(mountedCenter) - mountedCenter;
				shortestVectorFromPlayerToTarget.Y /= 0.8f; // Makes the hit area an ellipse. Vertical hit distance is smaller due to this math.
				float hitRadius = 55f; // The length of the semi-major radius of the ellipse (the long end)
				return shortestVectorFromPlayerToTarget.Length() <= hitRadius;
			}
			// Regular collision logic happens otherwise.
			return base.Colliding(projHitbox, targetHitbox);
		}

		public override void ModifyDamageScaling(ref float damageScale) {
			// Flails do 20% more damage while spinning
			if (CurrentAIState == AIState.Spinning)
				damageScale *= 1.2f;

			// Flails do 100% more damage while launched or retracting. This is the damage the item tooltip for flails aim to match, as this is the most common mode of attack. This is why the item has ItemID.Sets.ToolTipDamageMultiplier[Type] = 2f;
			if (CurrentAIState == AIState.LaunchingForward || CurrentAIState == AIState.Retracting)
				damageScale *= 2f;
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) {
			// Flails do a few custom things, you'll want to keep these to have the same feel as vanilla flails.

			// The hitDirection is always set to hit away from the player, even if the flail damages the npc while returning
			hitDirection = (Main.player[Projectile.owner].Center.X < target.Center.X) ? 1 : (-1);

			// Knockback is only 25% as powerful when in spin mode
			if (CurrentAIState == AIState.Spinning)
				knockback *= 0.25f;
			// Knockback is only 50% as powerful when in drop down mode
			if (CurrentAIState == AIState.Dropping)
				knockback *= 0.5f;

			base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
		}

		// PreDraw is used to draw a chain and trail before the projectile is drawn normally.
		public override bool PreDraw(ref Color lightColor) {
			Vector2 playerArmPosition = Main.GetPlayerArmPosition(Projectile);

			// This fixes a vanilla GetPlayerArmPosition bug causing the chain to draw incorrectly when stepping up slopes. The flail itself still draws incorrectly due to another similar bug. This should be removed once the vanilla bug is fixed.
			playerArmPosition.Y -= Main.player[Projectile.owner].gfxOffY;

			Asset<Texture2D> chainTexture = ModContent.Request<Texture2D>(ChainTexturePath);
			Texture2D projectileTexture = TextureAssets.Projectile[Type].Value;

			Rectangle? chainSourceRectangle = null;
			// Drippler Crippler customizes sourceRectangle to cycle through sprite frames: sourceRectangle = asset.Frame(1, 6);
			float chainHeightAdjustment = 0f; // Use this to adjust the chain overlap. 

			int drawOriginOffsetY = 0;
			int drawOffsetX = 0;
			float drawOriginOffsetX = (float)(projectileTexture.Width - Projectile.width) * 0.5f + (float)Projectile.width * 0.5f;
			ProjectileLoader.DrawOffset(Projectile, ref drawOffsetX, ref drawOriginOffsetY, ref drawOriginOffsetX);
			var projectileCenter = Projectile.position + new Vector2(drawOriginOffsetX + drawOffsetX, Projectile.height / 2 + Projectile.gfxOffY);

			Vector2 chainOrigin = chainSourceRectangle.HasValue ? (chainSourceRectangle.Value.Size() / 2f) : (chainTexture.Size() / 2f);
			Vector2 chainDrawPosition = projectileCenter;
			Vector2 vectorFromProjectileToPlayerArms = playerArmPosition.MoveTowards(chainDrawPosition, 8f) - chainDrawPosition;
			Vector2 unitVectorFromProjectileToPlayerArms = vectorFromProjectileToPlayerArms.SafeNormalize(Vector2.Zero);
			float chainSegmentLength = (chainSourceRectangle.HasValue ? chainSourceRectangle.Value.Height : chainTexture.Height()) + chainHeightAdjustment;
			if (chainSegmentLength == 0)
				chainSegmentLength = 10; // When the chain texture is being loaded, the height is 0 which would cause infinite loops.
			float chainRotation = unitVectorFromProjectileToPlayerArms.ToRotation() + MathHelper.PiOver2;
			int chainCount = 0;
			float chainLengthRemainingToDraw = vectorFromProjectileToPlayerArms.Length() + chainSegmentLength / 2f;

			// This while loop draws the chain texture from the projectile to the player, looping to draw the chain texture along the path
			while (chainLengthRemainingToDraw > 0f) {
				// This code gets the lighting at the current tile coordinates
				Color chainDrawColor = Lighting.GetColor((int)chainDrawPosition.X / 16, (int)(chainDrawPosition.Y / 16f));

				// Flaming Mace and Drippler Crippler use code here to draw custom sprite frames with custom lighting.
				// Cycling through frames: sourceRectangle = asset.Frame(1, 6, 0, chainCount % 6);
				// This example shows how Flaming Mace works. It checks chainCount and changes chainTexture and draw color at different values

				// Here, we draw the chain texture at the coordinates
				Main.spriteBatch.Draw(chainTexture.Value, chainDrawPosition - Main.screenPosition, chainSourceRectangle, chainDrawColor, chainRotation, chainOrigin, 1f, SpriteEffects.None, 0f);

				// chainDrawPosition is advanced along the vector back to the player by the chainSegmentLength
				chainDrawPosition += unitVectorFromProjectileToPlayerArms * chainSegmentLength;
				chainCount++;
				chainLengthRemainingToDraw -= chainSegmentLength;
			}

			// Add a motion trail when moving forward, like most flails do (don't add trail if already hit a tile)
			if (CurrentAIState == AIState.LaunchingForward) {
				Vector2 drawOrigin = new(projectileTexture.Width * 0.5f, Projectile.height * 0.5f);
				SpriteEffects spriteEffects = SpriteEffects.None;
				if (Projectile.spriteDirection == -1)
					spriteEffects = SpriteEffects.FlipHorizontally;
				for (int k = 0; k < Projectile.oldPos.Length && k < StateTimer; k++) {
					Vector2 drawPos = Projectile.oldPos[k] + new Vector2(drawOriginOffsetX + drawOffsetX, Projectile.height / 2 + Projectile.gfxOffY) - Main.screenPosition;
					Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
					Main.spriteBatch.Draw(projectileTexture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale - k / (float)Projectile.oldPos.Length / 3, spriteEffects, 0f);
				}
			}
			return true;
		}
	}
}
