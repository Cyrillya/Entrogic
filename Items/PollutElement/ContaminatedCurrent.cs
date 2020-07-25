using Entrogic.Projectiles.Magic.Books;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.PollutElement
{
    public class ContaminatedCurrent : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("黑魔法的邪恶产物");
            Item.staff[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.damage = 89;
            item.magic = true;
            item.mana = 4;
            item.width = 28;
            item.height = 30;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true; //当然是让它不能近战攻击啦
            item.knockBack = 5;
            item.value = Item.sellPrice(0, 5);
            item.rare = ItemRarityID.LightPurple;
            item.UseSound = SoundID.Item20;
            item.autoReuse = true;
            item.shoot = ProjectileType<污染水流>();
            item.shootSpeed = 20f;        
        }
    }
}