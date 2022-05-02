using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Entrogic.Items.Weapons.Summon.Whip
{
	public abstract class ModWhip : ModItem
	{
        public sealed override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
			ItemID.Sets.SummonerWeaponThatScalesWithAttackSpeed[Type] = true;
			WhipStaticDefaults();
		}
		public virtual void WhipStaticDefaults() { }
		public override void Load()
		{
			On.Terraria.Player.ApplyItemAnimation_Item += Player_ApplyItemAnimation_Item;
		}
		private void Player_ApplyItemAnimation_Item(On.Terraria.Player.orig_ApplyItemAnimation_Item orig, Player self, Item sItem)
		{
			orig(self, sItem);
			if (sItem.DamageType == DamageClass.Summon && ItemID.Sets.SummonerWeaponThatScalesWithAttackSpeed[sItem.type])
			{
				int frames = PlayerHooks.TotalMeleeTime((float)sItem.useAnimation * self.meleeSpeed * self.whipUseTimeMultiplier, self, sItem);
				self.itemAnimation = frames;
				self.itemAnimationMax = frames;
			}
		}
	}
	public abstract class ModWhipProj : ModProjectile
	{
		internal List<Vector2> _whipPointsForCollision = new List<Vector2>();
		public float PenetrateDamageMultpiler = 1f;
		public Color LineColor = Color.White;

        public override void Load()
        {
            On.Terraria.Projectile.CutTiles += Projectile_CutTiles;
            On.Terraria.Projectile.Colliding += Projectile_Colliding;
        }

        private bool Projectile_Colliding(On.Terraria.Projectile.orig_Colliding orig, Projectile self, Rectangle myRect, Rectangle targetRect)
		{
			if (self.type == Type)
			{
				_whipPointsForCollision.Clear();
				Projectile.FillWhipControlPoints(self, _whipPointsForCollision);
				for (int n = 0; n < _whipPointsForCollision.Count; n++)
				{
					Point point = _whipPointsForCollision[n].ToPoint();
					myRect.Location = new Point(point.X - myRect.Width / 2, point.Y - myRect.Height / 2);
				}
			}
			return orig(self, myRect, targetRect);
		}

        private void Projectile_CutTiles(On.Terraria.Projectile.orig_CutTiles orig, Projectile self)
        {
            if (self.type == Type)
			{
				_whipPointsForCollision.Clear();
				Projectile.FillWhipControlPoints(self, this._whipPointsForCollision);
				Vector2 value = new Vector2((float)self.width * self.scale / 2f, 0f);
				for (int i = 0; i < this._whipPointsForCollision.Count; i++)
				{
					DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
					Utils.PlotTileLine(this._whipPointsForCollision[i] - value, this._whipPointsForCollision[i] + value, (float)self.height * self.scale, new Utils.TileActionAttempt(DelegateMethods.CutTiles));
				}
            }
			orig(self);
        }

        public sealed override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
			ProjectileID.Sets.IsAWhip[Type] = true;
			WhipStaticDefaults();
        }
		public virtual void WhipStaticDefaults() { }

        public sealed override void SetDefaults()
		{
			projectile.DefaultToWhip();
			projectile.aiStyle = -1;
			WhipDefaults();
		}
		public virtual void WhipDefaults() { }

		public override void AI()
        {
            base.AI();
			Player player = Main.player[projectile.owner];
			projectile.rotation = projectile.velocity.ToRotation() + 1.57079637f;
			projectile.ai[0] += 1f;
            Projectile.GetWhipSettings(projectile, out float timeToFlyOut, out int num11, out float num12);
            projectile.Center = Main.GetPlayerArmPosition(projectile) + projectile.velocity * (projectile.ai[0] - 1f);
			projectile.spriteDirection = ((Vector2.Dot(projectile.velocity, Vector2.UnitX) >= 0f) ? 1 : -1);
			if (projectile.ai[0] >= timeToFlyOut || player.itemAnimation == 0)
			{
				projectile.Kill();
				return;
			}
			player.heldProj = projectile.whoAmI;
			player.itemAnimation = player.itemAnimationMax - (int)(projectile.ai[0] / (float)projectile.MaxUpdates);
			player.itemTime = player.itemAnimation;
			if (projectile.ai[0] == (float)((int)(timeToFlyOut / 2f)))
			{
				_whipPointsForCollision.Clear();
				Projectile.FillWhipControlPoints(projectile, _whipPointsForCollision);
				Vector2 position = _whipPointsForCollision[_whipPointsForCollision.Count - 1];
				SoundEngine.PlaySound(SoundID.Item153, position);
			}
			num11 = projectile.type;
			if (num11 != 952)
			{
				return;
			}
			float t8 = projectile.ai[0] / timeToFlyOut;
			float num10 = Utils.GetLerpValue(0.1f, 0.7f, t8, true) * Utils.GetLerpValue(0.9f, 0.7f, t8, true);
			if (num10 > 0.15f && Main.rand.NextFloat() < num10)
			{
				_whipPointsForCollision.Clear();
				Projectile.FillWhipControlPoints(projectile, _whipPointsForCollision);
				Rectangle r9 = Utils.CenteredRectangle(_whipPointsForCollision[_whipPointsForCollision.Count - 1], new Vector2(30f, 30f));
				Vector2 value6 = _whipPointsForCollision[_whipPointsForCollision.Count - 2].DirectionTo(_whipPointsForCollision[_whipPointsForCollision.Count - 1]).SafeNormalize(Vector2.Zero);
				Dust dust4 = Dust.NewDustDirect(r9.TopLeft(), r9.Width, r9.Height, 26, 0f, 0f, 0, default(Color), 0.7f);
				dust4.noGravity = (Main.rand.Next(3) == 0);
				if (dust4.noGravity)
				{
					dust4.scale += 0.4f;
				}
				dust4.velocity += value6 * 2f;
				return;
			}
		}

		private void DrawWhips(SpriteBatch spriteBatch)
		{
			List<Vector2> list = new List<Vector2>();
			Projectile.FillWhipControlPoints(projectile, list);
			Texture2D value = TextureAssets.FishingLine.Value;
			Rectangle value2 = value.Frame(1, 1, 0, 0, 0, 0);
			Vector2 origin = new Vector2((float)(value2.Width / 2), 2f);
			Color originalColor = LineColor;
			Vector2 value3 = list[0];
			for (int i = 0; i < list.Count - 1; i++)
			{
				Vector2 vector = list[i];
				Vector2 vector2 = list[i + 1] - vector;
				float rotation = vector2.ToRotation() - 1.57079637f;
				Color color = Lighting.GetColor(vector.ToTileCoordinates(), originalColor);
				Vector2 scale = new Vector2(1f, (vector2.Length() + 2f) / (float)value2.Height);
				spriteBatch.Draw(value, value3 - Main.screenPosition, new Rectangle?(value2), color, rotation, origin, scale, SpriteEffects.None, 0f);
				value3 += vector2;
			}
			DrawWhip(spriteBatch, list);
		}
		public virtual void DrawWhip(SpriteBatch spriteBatch, List<Vector2> controlPoints)
		{
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (projectile.spriteDirection == 1)
			{
				spriteEffects ^= SpriteEffects.FlipHorizontally;
			}
			Texture2D value = TextureAssets.Projectile[Type].Value;
			Rectangle rectangle = value.Frame(1, 5, 0, 0, 0, 0);
			int height = rectangle.Height;
			rectangle.Height -= 2;
			Vector2 vector = rectangle.Size() / 2f;
			Vector2 vector2 = controlPoints[0];
			for (int i = 0; i < controlPoints.Count - 1; i++)
			{
				Vector2 origin = vector;
				float scale = 1f;
				bool flag;
				if (i == 0)
				{
					origin.Y -= 4f;
					flag = true;
				}
				else
				{
					flag = true;
					int num = 1;
					if (i > 10)
					{
						num = 2;
					}
					if (i > 20)
					{
						num = 3;
					}
					rectangle.Y = height * num;
				}
				if (i == controlPoints.Count - 2)
				{
					flag = true;
					rectangle.Y = height * 4;
					float timeToFlyOut;
					int num2;
					float num3;
					Projectile.GetWhipSettings(projectile, out timeToFlyOut, out num2, out num3);
					float t = projectile.ai[0] / timeToFlyOut;
					float amount = Utils.GetLerpValue(0.1f, 0.7f, t, true) * Utils.GetLerpValue(0.9f, 0.7f, t, true);
					scale = MathHelper.Lerp(0.5f, 1.5f, amount);
				}
				Vector2 vector3 = controlPoints[i];
				Vector2 vector4 = controlPoints[i + 1] - vector3;
				if (flag)
				{
					float rotation = vector4.ToRotation() - 1.57079637f;
					Color color = Lighting.GetColor(vector3.ToTileCoordinates());
					spriteBatch.Draw(value, vector2 - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(rectangle), color, rotation, origin, scale, spriteEffects, 0f);
				}
				vector2 += vector4;
			}
		}
		public sealed override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			DrawWhips(spriteBatch);
			return false;
		}

		public sealed override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);

			if (target.active)
			{
				Main.player[projectile.owner].MinionAttackTargetNPC = target.whoAmI;
			}
			WhipHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
			projectile.damage = (int)((double)projectile.damage * PenetrateDamageMultpiler);
		}
		public virtual void WhipHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) { }
    }
}
