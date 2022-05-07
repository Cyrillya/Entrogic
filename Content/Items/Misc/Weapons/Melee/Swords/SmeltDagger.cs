using Entrogic.Content.Items.Symbiosis;
using Entrogic.Content.Projectiles.Misc.Weapons.Melee.Swords;

namespace Entrogic.Content.Items.Misc.Weapons.Melee.Swords
{
    public class SmeltDagger : ModItem
    {
        public override void SetStaticDefaults() {
            ItemID.Sets.SkipsInitialUseSound[Type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Tooltip.SetDefault("“新鲜出炉”\n" +
                "有可能会有点软化");
        }

        public override void SetDefaults() {
            Item.damage = 143;
            Item.width = 30;
            Item.height = 30;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 3;
            Item.scale = 2.2f;
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

        public override void AddRecipes() => CreateRecipe()
                .AddIngredient(ModContent.ItemType<GelOfLife>(), 7)
                .AddIngredient(ItemID.MeteoriteBar, 10)
                .AddIngredient(ItemID.HellstoneBar, 8)
                .AddTile(TileID.Anvils)
                .Register();
    }
}