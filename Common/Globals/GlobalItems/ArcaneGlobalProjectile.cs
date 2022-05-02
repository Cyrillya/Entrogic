using Entrogic.Content.DamageClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entrogic.Common.Globals.GlobalItems
{
    internal class ArcaneGlobalProjectile : GlobalProjectile
    {
        public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit) {
            // 如果开启了ColoredDamageTypes Mod，我们自己的武器伤害也得有颜色
            if (projectile.DamageType != ModContent.GetInstance<ArcaneDamageClass>() || !ModLoader.TryGetMod("ColoredDamageTypes", out _)) {
                return;
            }

            Color DamagedArcaneHostile = new(224, 141, 255);
            Color DamagedArcaneHostileCrit = new(193, 49, 255);
            int recent = -1;
            for (int i = 99; i >= 0; i--) {
                CombatText ctToCheck = Main.combatText[i];
                if (ctToCheck.lifeTime == 60 || ctToCheck.lifeTime == 120) {
                    if (ctToCheck.alpha == 1f) {
                        if (ctToCheck.color == CombatText.DamagedHostile || ctToCheck.color == CombatText.DamagedHostileCrit) {
                            recent = i;
                            break;
                        }
                    }
                }
            }
            if (recent != -1) {
                CombatText text = Main.combatText[recent];
                text.color = DamagedArcaneHostile;
                if (crit)
                    text.color = DamagedArcaneHostileCrit;
            }
        }
    }
}
