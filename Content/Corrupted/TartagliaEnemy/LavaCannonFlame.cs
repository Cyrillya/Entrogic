using Entrogic.Core.BaseTypes;

namespace Entrogic.Content.Corrupted.TartagliaEnemy
{
    public class LavaCannonFlame : ProjectileBase
	{
		public override void SetDefaults() {
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.timeLeft = 120;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
		}

		public override bool PreAI() {
			cataloguePos();
			Vector2 rotational = Utils.RotatedBy(new Vector2(0f, -1.8f), (double)MathHelper.ToRadians(Utils.NextFloat(Main.rand, -30f, 30f)), default(Vector2));
			rotational.X *= 0.25f;
			rotational.Y *= 0.75f;
			rotational += Projectile.velocity;
			rotational = Utils.SafeNormalize(rotational, Vector2.Zero) * 3f;
			_particleList.Add(new FireParticle(Projectile.Center - rotational * 2f, rotational, Utils.NextFloat(Main.rand, -3f, 3f), Utils.NextFloat(Main.rand, -2f, 2f), Utils.NextFloat(Main.rand, 0.9f, 1.1f)));
			return base.PreAI();
		}

		public void cataloguePos() {
			for (int i = 0; i < _particleList.Count; i++) {
				FireParticle fireParticle = _particleList[i];
				fireParticle.Update();
				if (!fireParticle.active) {
					_particleList.RemoveAt(i);
					i--;
				}
			}
		}

		public override bool PreDraw(ref Color lightColor) {
			Asset<Texture2D> texture = TextureAssets.Projectile[Type];
			Vector2 drawOrigin = new(texture.Width() * 0.5f, texture.Height() * 0.5f);
			for (int i = 0; i < _particleList.Count; i++) {
				Color color = new(255, 69, 0, 0);
				Vector2 drawPos = _particleList[i].position - Main.screenPosition;
				color = Projectile.GetAlpha(color) * (0.35f + 0.65f * _particleList[i].scale);
				for (int j = 0; j < 2; j++) {
					float x = Utils.NextFloat(Main.rand, -2f, 2f);
					float y = Utils.NextFloat(Main.rand, -2f, 2f);
					Main.EntitySpriteDraw(texture.Value, drawPos + new Vector2(x, y), null, color, _particleList[i].rotation, drawOrigin, _particleList[i].scale, 0, 0);
				}
			}
			return false;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit) {
			target.AddBuff(24, 360, false);
		}

		public override bool ShouldUpdatePosition() {
			return true;
		}

		public override void AI() {
			Projectile.velocity *= 0.98f;
			Projectile projectile = Projectile;
			projectile.velocity.Y = projectile.velocity.Y + 0.14f;
			if (Projectile.timeLeft <= 51) {
				Projectile.alpha += 5;
			}
			if (Projectile.timeLeft <= 15) {
				Projectile.hostile = false;
			}
			Lighting.AddLight(Projectile.Center, 0.75f, 0.25f, 0f);
			Projectile.ai[0] += 1f;
		}

		private List<FireParticle> _particleList = new();
	}
	public class FireParticle
	{
		public FireParticle() {
			position = Vector2.Zero;
			velocity = Vector2.Zero;
			rotation = 0f;
			nextRotation = 0f;
			scale = 1f;
			mult = Utils.NextFloat(Main.rand, 0.9f, 1.1f);
		}

		public FireParticle(Vector2 position, Vector2 velocity, float rotation, float nextRotation, float scale) {
			this.position = position;
			this.velocity = velocity;
			this.rotation = rotation;
			this.nextRotation = nextRotation;
			this.scale = scale;
			mult = Utils.NextFloat(Main.rand, 0.9f, 1.1f);
		}

		public void Update() {
			counter += 1f;
			float veloMult = 0.6f + 0.4f * counter / 15f;
			if (veloMult > 1f) {
				veloMult = 1f;
			}
			position += velocity * veloMult;
			for (int i = 0; i < 1 + (int)(Utils.NextFloat(Main.rand, 1f) * mult); i++) {
				velocity.Y = velocity.Y * 0.95f;
				velocity.X = velocity.X * 0.98f;
				scale *= 0.95f;
			}
			if (counter < 31f) {
				rotation += nextRotation / 30f;
			}
			if (scale <= 0.05f) {
				active = false;
			}
		}

		internal Vector2 position;
		internal Vector2 velocity;
		internal float rotation;
		internal float nextRotation;
		internal float mult;
		internal float counter;
		internal float scale;
		internal bool active = true;
	}
}
