using Terraria.DataStructures;
namespace Entrogic.Common.Globals.Players.Accessories
{
    public class DemonAccPlayer : ModPlayer
    {
        public static DemonAccPlayer Get(Player player) => player.GetModPlayer<DemonAccPlayer>();

        public bool BrokenTail;
        public bool GoatHorn;

        public override void UpdateEquips() {
            if (BrokenTail) {
                Player.statLifeMax2 -= 80;
                // 同时穿戴时全伤害、移速增加且免疫岩浆
                if (GoatHorn) {
                    Player.GetDamage(DamageClass.Generic) += 0.1f;
                    Player.moveSpeed *= 1.05f;
                    Player.lavaImmune = true;
                }
            }
        }

        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit) {
            if (GoatHorn && crit) {
                damage = (int)(damage * 1.25f); // 2*1.25=2.5
            }
        }

        //public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter) {
        //    if (GoatHorn) {
        //        damage = (int)(damage * 1.4f); // 增加40%的受伤
        //    }
        //    return base.PreHurt(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource, ref cooldownCounter);
        //}

        // 这里写断尾，才可以应用到immune
        //public override void PostHurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit, int cooldownCounter) {
        //    if (BrokenTail && damage > Player.statLifeMax2 * 0.4 && !pvp) {
        //        Player.AddImmuneTime(cooldownCounter, 60);
        //    }
        //}

        private void Reset() {
            BrokenTail = false;
            GoatHorn = false;
        }

        public override void ResetEffects() => Reset();

        public override void UpdateDead() => Reset();
    }
}
