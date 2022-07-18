namespace Entrogic.Content.Items.Athanasy.Weapons
{
    public class RockFlail : ItemBase
	{
		public override void SetStaticDefaults() {
			SacrificeTotal = 1;
			ItemID.Sets.ToolTipDamageMultiplier[Type] = 2f; // 原版链球都这样
		}

		public override void SetDefaults() {
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 45;
			Item.useTime = 45;
			Item.knockBack = 5.5f;
			Item.width = 32;
			Item.height = 32;
			Item.damage = 85;
			Item.noUseGraphic = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Athanasy.Weapons.RockFlail>();
			Item.shootSpeed = 12f;
			Item.UseSound = SoundID.Item1;
			Item.rare = RarityLevelID.LatePHM;
			Item.value = Item.sellPrice(gold: 1, silver: 50);
			Item.DamageType = DamageClass.Melee;
			Item.channel = true;
			Item.noMelee = true;
		}
	}
}
