using Entrogic.Content.NPCs.BaseTypes;

namespace Entrogic.Content.NPCs.Enemies.Athanasy
{
    public class StoneHand : NPCBase
    {
        public override void SetStaticDefaults() {
            Athanasy.StoneHandType = Type;
            Main.npcFrameCount[Type] = 6;
        }

        public override void SetDefaults() {
            NPC.aiStyle = -1;
            NPC.width = 54;
            NPC.height = 68;
            NPC.damage = 70;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.lifeMax = 200;
            NPC.defense = 20;
            NPC.hide = true;
            NPC.behindTiles = true;
            NPC.lavaImmune = true;
            NPC.knockBackResist = 0f;
            NPC.dontTakeDamage = true;
            // 花岗岩巨人的音效
            NPC.DeathSound = SoundID.NPCDeath43;
            NPC.HitSound = SoundID.NPCHit41;
        }

        private Vector2 AthanasyCenter => new(NPC.ai[2], NPC.ai[3]);

        public override void OnSpawn(IEntitySource source) {
            if (source.Context is not null && (source.Context == "Stage1" || source.Context == "Stage2") && Main.netMode != NetmodeID.Server) {
                NPC.position += NPC.netOffset;

                Vector2 vec = Utils.DirectionTo(AthanasyCenter, NPC.Center) * 120f;
                Vector2 startPosition = AthanasyCenter + vec;
                if (source.Context == "Stage2")
                    goto SpawnCircleDust;
                float distance = Vector2.Distance(startPosition, NPC.Center);

                Dust.QuickDustLine(startPosition, NPC.Bottom, distance / 12f, Color.SkyBlue);

                SpawnCircleDust:;
                int dusts = source.Context == "Stage2" ? 12 : 8;
                for (int i = 0; i < dusts; i++) {
                    float radians = 6.28f / 8f * i;
                    var velocity = Vector2.One.RotatedBy(radians) * 1.4f;
                    var d = Dust.NewDustPerfect(NPC.Bottom, MyDustID.BlueWhiteBubble, velocity, 180, Scale: source.Context == "Stage2" ? 2f : 1f);
                    d.fadeIn = 0.4f;
                    d.noGravity = true;
                }

                NPC.position -= NPC.netOffset;
            }
        }

        public override void AI() {
            Timer++;
            if (Timer == 100 && Main.netMode != NetmodeID.Server) {
                SoundEngine.PlaySound(SoundAssets.StoneHand, NPC.Bottom);
            }
            if (Timer > 120) {
                NPC.dontTakeDamage = false;
            }
            if (Timer >= 90) {
                NPC.hide = false;
            }
            else {
                Dust.NewDust(NPC.BottomLeft - new Vector2(0f, 16f), NPC.width, 16, DustID.Stone);
            }
            //else {
            //    for (int i = 0; i < 20; i++) {
            //        var d = Dust.NewDustDirect(NPC.BottomLeft - new Vector2(0f, 16f), NPC.width, 16, DustID.Smoke, Alpha: 50); d.noGravity = true;
            //        d.velocity.Y = -5f + Main.rand.NextFloat() * -3f;
            //        d.velocity.X *= 7f;
            //    }
            //}
            if (NPC.ai[2] == -1 && Timer >= 220) {
                NPC.active = false;
                NPC.life = 0;
                NPC.netUpdate = true;
            }
        }

        public override void FindFrame(int frameHeight) {
            // 95 100 105 110 115 120
            if (Timer >= 100 && Timer <= 120 && NPC.frameCounter <= 5 && Timer % 5 == 0) {
                NPC.frameCounter++;
            }
            if (NPC.ai[2] == -1 && Timer >= 200 && Timer <= 220 && NPC.frameCounter >= 0 && Timer % 5 == 0) {
                NPC.frameCounter--;
            }
            NPC.frame.Y = (int)NPC.frameCounter * frameHeight;
        }

        public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit) {
            if (item.pick > 0) {
                damage *= 100;
                if (damage < NPC.lifeMax + NPC.defense) {
                    damage = Main.rand.Next(2000, 8000);
                }
            }
            else if (damage <= NPC.lifeMax / 2f + NPC.defense) {
                damage = Main.rand.Next(NPC.lifeMax / 2 + NPC.defense, NPC.lifeMax / 2 + NPC.defense + 100);
            }
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) {
            if (Main.player.IndexInRange(projectile.owner) &&
                Main.player[projectile.owner].heldProj == projectile.whoAmI &&
                Main.player[projectile.owner].HeldItem is not null &&
                Main.player[projectile.owner].HeldItem.channel &&
                Main.player[projectile.owner].HeldItem.pick > 0) {
                damage *= 100;
                if (damage < NPC.lifeMax + NPC.defense) {
                    damage = Main.rand.Next(2000, 8000);
                }
            }
            if (damage <= NPC.lifeMax / 2f + NPC.defense) {
                damage = Main.rand.Next(NPC.lifeMax / 2 + NPC.defense, NPC.lifeMax / 2 + NPC.defense + 100);
            }
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot) {
            return base.CanHitPlayer(target, ref cooldownSlot) && Timer >= 120;
        }

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit) {
            target.ApplyDamageToNPC(NPC, Main.rand.Next(2000, 8000), 0f, target.direction, false);
            if (Main.expertMode && !target.HasBuff(BuffID.Stoned)) {
                target.AddBuff(BuffID.Stoned, 30, true);
            }
        }

        public override void HitEffect(int hitDirection, double damage) {
            if (NPC.life > 0 || Main.dedServ)
                return;

            for (int i = 0; i < 30; i++) {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Stone, Scale: Main.rand.NextFloat(0.8f, 1.4f));
            }

            var entitySource = NPC.GetSource_Death();
            for (int i = 1; i <= 4; i++) {
                int goreType = Mod.Find<ModGore>($"StoneHandGore_{i}").Type;
                Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), goreType);
            }
        }
    }
}
