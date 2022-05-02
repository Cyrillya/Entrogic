using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Entrogic
{
    public class AutoswingHandler : ModPlayer
    {
        public bool allowAutoReuse = false;
        public bool allowAutoReuseWhip = false;
        public bool allowAutoReuseMelee = false;
        public bool allowAutoReuseRanged = false;

        public override void Load() {
            base.Load();
            On.Terraria.Player.TryAllowingItemReuse += Player_TryAllowingItemReuse;
        }

		private void Player_TryAllowingItemReuse(On.Terraria.Player.orig_TryAllowingItemReuse orig, Player self, Item sItem) {
            orig(self, sItem);

            if (self.GetModPlayer<AutoswingHandler>().allowAutoReuse) {
                self.GetModPlayer<AutoswingHandler>().allowAutoReuse = false;
                self.releaseUseItem = true;
            }
            if (self.GetModPlayer<AutoswingHandler>().allowAutoReuseWhip && sItem.CountsAsClass(DamageClass.Summon) && ItemID.Sets.SummonerWeaponThatScalesWithAttackSpeed[sItem.type]) {
                self.GetModPlayer<AutoswingHandler>().allowAutoReuseWhip = false;
                self.releaseUseItem = true;
            }
            if (self.GetModPlayer<AutoswingHandler>().allowAutoReuseMelee && sItem.CountsAsClass(DamageClass.Melee)) {
                self.GetModPlayer<AutoswingHandler>().allowAutoReuseMelee = false;
                self.releaseUseItem = true;
            }
            if (self.GetModPlayer<AutoswingHandler>().allowAutoReuseRanged && sItem.CountsAsClass(DamageClass.Ranged)) {
                self.GetModPlayer<AutoswingHandler>().allowAutoReuseRanged = false;
                self.releaseUseItem = true;
            }
        }
    }
}
