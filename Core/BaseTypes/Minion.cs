namespace Entrogic.Core.BaseTypes
{
    public abstract class Minion : ProjectileBase
    {
        public enum SearchMode
        {
            PlayerClosest,
            MinionClosest
        }

        public int BuffType = 0;
        public bool BossFirst = true;
        public bool SearchThroughWall = false;
        public SearchMode SelectedSearchMode = SearchMode.MinionClosest;
        internal float SearchDistance = 700;
        internal float LockDistance = 2000;

        public override sealed void SetDefaults() {
            base.SetDefaults();
            MinionDefaults(out int buff);
            BuffType = buff;
        }

        public virtual void MinionDefaults(out int buffType) { buffType = -1; }

        public override void AI() {
            Player owner = Main.player[Projectile.owner];

            if (!CheckActive(owner)) {
                return;
            }

            GeneralBehavior(owner, out Vector2 vectorToIdlePosition, out float distanceToIdlePosition);
            SearchForTargets(owner, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter);
            Movement(foundTarget, distanceFromTarget, targetCenter, distanceToIdlePosition, vectorToIdlePosition);
            Visuals();
        }

        // This is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
        private bool CheckActive(Player owner) {
            if (owner.dead || !owner.active) {
                owner.ClearBuff(BuffType);

                return false;
            }

            if (owner.HasBuff(BuffType)) {
                Projectile.timeLeft = 2;
            }

            return true;
        }

        public virtual void GeneralBehavior(Player owner, out Vector2 vectorToIdlePosition, out float distanceToIdlePosition) { vectorToIdlePosition = Vector2.Zero; distanceToIdlePosition = 0f; }

        public virtual void SearchForTargets(Player owner, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter) {
            // Starting search distance
            distanceFromTarget = SearchDistance;
            targetCenter = Projectile.position;
            foundTarget = false;

            // This code is required if your minion weapon has the targeting feature
            if (owner.HasMinionAttackTargetNPC) {
                NPC npc = Main.npc[owner.MinionAttackTargetNPC];
                float between = Vector2.Distance(npc.Center, Projectile.Center);

                // Reasonable distance away so it doesn't target across multiple screens
                if (between < LockDistance) {
                    distanceFromTarget = between;
                    targetCenter = npc.Center;
                    foundTarget = true;
                }
            }

            if (!foundTarget) {
                var availableBosses = new List<int>();
                var searcherPosition = (SelectedSearchMode == SearchMode.MinionClosest ? Projectile.Center : owner.Center);
                // This code is required either way, used for finding a target
                for (int i = 0; i < Main.maxNPCs; i++) {
                    NPC npc = Main.npc[i];

                    if (npc.CanBeChasedBy()) {
                        float between = Vector2.Distance(npc.Center, searcherPosition);
                        bool closest = Vector2.Distance(searcherPosition, targetCenter) > between;
                        bool inRange = between < distanceFromTarget;
                        bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height) || SearchThroughWall;
                        // Additional check for this specific minion behavior, otherwise it will stop attacking once it dashed through an enemy while flying though tiles afterwards
                        // The number depends on various parameters seen in the movement code below. Test different ones out until it works alright
                        bool closeThroughWall = between < 100f;

                        if (npc.boss && BossFirst) {
                            availableBosses.Add(i);
                        }
                        // 如果模式为优先选取Boss，那么如果已经找到至少一个Boss就不进行锁定了，只搜索
                        if (availableBosses.Count > 0) {
                            // 重置
                            distanceFromTarget = SearchDistance;
                            targetCenter = Projectile.position;
                            foundTarget = false;
                            continue;
                        }
                        if (((closest && inRange) || !foundTarget) && (lineOfSight || closeThroughWall)) {
                            distanceFromTarget = between;
                            targetCenter = npc.Center;
                            foundTarget = true;
                        }
                    }
                }

                foreach (var i in availableBosses) {
                    NPC npc = Main.npc[i];

                    float between = Vector2.Distance(npc.Center, searcherPosition);
                    bool closest = Vector2.Distance(searcherPosition, targetCenter) > between;
                    bool inRange = between < distanceFromTarget;
                    bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height) || SearchThroughWall;
                    // Additional check for this specific minion behavior, otherwise it will stop attacking once it dashed through an enemy while flying though tiles afterwards
                    // The number depends on various parameters seen in the movement code below. Test different ones out until it works alright
                    bool closeThroughWall = between < 100f;

                    if (((closest && inRange) || !foundTarget) && (lineOfSight || closeThroughWall)) {
                        distanceFromTarget = between;
                        targetCenter = npc.Center;
                        foundTarget = true;
                    }
                }
            }

            // friendly needs to be set to true so the minion can deal contact damage
            // friendly needs to be set to false so it doesn't damage things like target dummies while idling
            // Both things depend on if it has a target or not, so it's just one assignment here
            // You don't need this assignment if your minion is shooting things instead of dealing contact damage
            Projectile.friendly = foundTarget;
        }

        public virtual void Movement(bool foundTarget, float distanceFromTarget, Vector2 targetCenter, float distanceToIdlePosition, Vector2 vectorToIdlePosition) { }

        public virtual void Visuals() { }

        // Here you can decide if your minion breaks things like grass or pots
        public override bool? CanCutTiles() => false;

        // This is mandatory if your minion deals contact damage (further related stuff in AI() in the Movement region)
        public override bool MinionContactDamage() => true;
    }
}