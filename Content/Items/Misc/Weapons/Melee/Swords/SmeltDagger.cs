using Entrogic.Content.Items.Symbiosis;
using Entrogic.Content.Projectiles.Weapons.Melee.Swords;

namespace Entrogic.Content.Items.Misc.Weapons.Melee.Swords
{
    public class SmeltDagger : ModItem
    {
        public override void SetStaticDefaults() {
            ItemID.Sets.SkipsInitialUseSound[Type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults() {
            Item.damage = 33;
            Item.width = 30;
            Item.height = 30;
            Item.useTime = 26;
            Item.useAnimation = 26;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 1f;
            Item.scale = 2.4f;
            Item.value = Item.sellPrice(gold: 3);
            Item.rare = RarityLevelID.LatePHM;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = false;

            Item.shoot = ModContent.ProjectileType<SmeltDaggerProjectile>();
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.shootSpeed = 4f;
            Item.DamageType = DamageClass.Melee;
        }

        public override void Load() {
            On.Terraria.Item.Prefix += Item_Prefix;
        }

        private bool Item_Prefix(On.Terraria.Item.orig_Prefix orig, Item self, int pre) {
            if (self.type == Type) {
                self.noUseGraphic = false; // 让武器可以获得诸如“传奇”的词缀
                bool flag = orig.Invoke(self, pre);
                self.noUseGraphic = true;
                return flag;
            }
            return orig.Invoke(self, pre);
        }

        // 根据攻速提升的伤害
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage) {
            damage *= (float)Math.Sqrt(player.GetAttackSpeed(DamageClass.Melee));
            base.ModifyWeaponDamage(player, ref damage);
        }

        public override void AddRecipes() => CreateRecipe()
                .AddIngredient(ModContent.ItemType<GelOfLife>(), 7)
                .AddIngredient(ItemID.MeteoriteBar, 10)
                .AddIngredient(ItemID.HellstoneBar, 8)
                .AddTile(TileID.Anvils)
                .Register();
    }
}