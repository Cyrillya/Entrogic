using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace Entrogic.Projectiles.Arcane
{
    public abstract class ArcaneProjectile : ModProjectile
    {
        public sealed override void SetDefaults()
        {
            ArcaneDefaults();
            projectile.DamageType = ModContent.GetInstance<ArcaneDamageClass>();
        }
        public virtual void ArcaneDefaults() { }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Color DamagedArcaneHostile = new Color(224, 141, 255);
            Color DamagedArcaneHostileCrit = new Color(193, 49, 255);
            int recent = -1;
            for (int i = 99; i >= 0; i--)
            {
                CombatText ctToCheck = Main.combatText[i];
                if (ctToCheck.lifeTime == 60 || ctToCheck.lifeTime == 120)
                {
                    if (ctToCheck.alpha == 1f)
                    {
                        if (ctToCheck.color == CombatText.DamagedHostile || ctToCheck.color == CombatText.DamagedHostileCrit)
                        {
                            recent = i;
                            break;
                        }
                    }
                }
            }
            if (recent != -1)
            {
                CombatText text = Main.combatText[recent];
                text.color = DamagedArcaneHostile;
                if (crit)
                    text.color = DamagedArcaneHostileCrit;
            }
            OnSafeHitNPC(target, damage, knockback, crit);
        }
        public virtual void OnSafeHitNPC(NPC target, int damage, float knockback, bool crit) { }
    }
}
