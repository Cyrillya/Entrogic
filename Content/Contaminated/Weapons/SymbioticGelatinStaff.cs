using Entrogic.Content.Contaminated.Buffs;
using Entrogic.Content.Contaminated.Friendly;
using Entrogic.Core.BaseTypes;
using Entrogic.Helpers.ID;
using Terraria.Utilities;

namespace Entrogic.Content.Contaminated.Weapons
{
    public class SymbioticGelatinStaff : ItemBase
    {
        public override void SetStaticDefaults() {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.GamepadWholeScreenUseRange[Type] = true; // This lets the player target anywhere on the whole screen while using a controller
            ItemID.Sets.LockOnIgnoresCollision[Type] = true;

            DisplayName.SetDefault("Symbiotic Gelatin Staff");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "共生明胶法杖");
            Tooltip.SetDefault("Summon a few flexible symbiotic gelatins to fight for you.");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "召唤几只灵活的共生明胶为你而战。");
        }

        public override void SetDefaults() {
            Item.mana = 10;
            Item.damage = 31;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.width = 48;
            Item.height = 46;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.knockBack = 3f;
            Item.value = Item.sellPrice(0, 5);
            Item.rare = RarityLevelID.MiddleHM;
            Item.UseSound = SoundID.Item113;

            Item.noMelee = true;
            Item.DamageType = DamageClass.Summon;
            Item.buffType = ModContent.BuffType<SymbioticGelatinBuff>();
            Item.shoot = ModContent.ProjectileType<SymbioticGelatin>();
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
            // Here you can change where the minion is spawned. Most vanilla minions spawn at the cursor position
            position = Main.MouseWorld;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            // This is needed so the buff that keeps your minion alive and allows you to despawn it properly applies
            player.AddBuff(Item.buffType, 2);

            // Minions have to be spawned manually, then have originalDamage assigned to the damage of the summon item
            for (int i = 1; i <= 3; i++) { // 一次召唤三个
                var projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
                projectile.originalDamage = Item.damage;
                if (projectile.ModProjectile is Minion) {
                    var minion = projectile.ModProjectile as Minion;
                    minion.BossFirst = Main.rand.NextFloat() < .66f; // 66%的概率Boss优先
                    WeightedRandom<Minion.SearchMode> mode = new();
                    mode.Add(Minion.SearchMode.MinionClosest);
                    mode.Add(Minion.SearchMode.PlayerClosest);
                    minion.SelectedSearchMode = mode;
                }
            }

            // Since we spawned the projectile manually already, we do not need the game to spawn it for ourselves anymore, so return false
            return false;
        }
    }
}
