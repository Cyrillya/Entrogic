using Entrogic.Content.Items.BaseTypes;
using Entrogic.Content.Projectiles.Weapons.Melee.Swords.TwoHandSword;
using System.Linq;
using Terraria.Audio;

namespace Entrogic.Content.Items.Misc.Weapons.Melee.Swords
{
    public class TwoHandSword : ItemBase
    {
        public override void SetStaticDefaults() {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults() {
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 18;
            Item.useTime = 18;
            Item.shootSpeed = 0f;
            Item.knockBack = 5.8f;
            Item.width = 56;
            Item.height = 56;
            Item.damage = 55;
            Item.shoot = ModContent.ProjectileType<TwoHandSword_Proj>();
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(0, 3, 50);
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Melee;
            Item.channel = true;
            Item.autoReuse = false;
        }

        private List<int> swordProjectiles = new();
        public override bool CanUseItem(Player player) {
            if (swordProjectiles.Count < 3) {
                swordProjectiles = new List<int>() { ModContent.ProjectileType<TwoHandSword_Proj>(), ModContent.ProjectileType<TwoHandSword_Proj2>(), ModContent.ProjectileType<TwoHandSword_Proj3>() };
            }
            Item.autoReuse = false;
            bool use = true;
            foreach (var proj in from p in Main.projectile where p.active && swordProjectiles.Contains(p.type) && p.owner == player.whoAmI select p) { use = false; break; }
            return use;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
            TwoHandSwordPlayer swordPlayer = player.GetModPlayer<TwoHandSwordPlayer>();

            player.direction = Main.MouseWorld.X < player.Center.X ? -1 : 1;
            if (swordPlayer.ComboMode == 1) {
                type = ModContent.ProjectileType<TwoHandSword_Proj2>();
            }
            else if (swordPlayer.ComboMode == 2) {
                type = ModContent.ProjectileType<TwoHandSword_Proj3>();
                player.direction = -player.direction;
            }
            else {
                type = ModContent.ProjectileType<TwoHandSword_Proj>();
            }
            position = player.Center;
            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            SoundEngine.PlaySound(SoundID.DD2_DarkMageCastHeal, player.Center);
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
    }
    public class TwoHandSwordPlayer : ModPlayer
    {
        internal int ComboTimer;
        internal short ComboMode;
        public override void PostUpdate() {
            base.PostUpdate();
            ComboTimer--;
            if (ComboTimer <= 0) ComboMode = 0;
        }
    }
}
