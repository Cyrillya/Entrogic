using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Magic.Staff
{
    public class CorruCloudStaff : ModItem
    {
        public override void SetDefaults()
        {
            item.mana = 10;
            item.damage = 52;
            item.useStyle = ItemUseStyleID.Swing;
            item.shootSpeed = 16f;
            item.shoot = ProjectileType<Projectiles.Magic.Staff.CorruCloud>();
            item.width = 26;
            item.height = 28;
            item.UseSound = SoundID.Item66;
            item.useAnimation = 22;
            item.useTime = 22;
            item.rare = ItemRarityID.LightPurple;
            item.noMelee = true;
            item.knockBack = 0f;
            item.value = Item.sellPrice(0, 4, 50, 0);
            item.DamageType = DamageClass.Magic;
        }
    }
}