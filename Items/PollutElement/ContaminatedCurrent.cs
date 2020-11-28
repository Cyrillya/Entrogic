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
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Tooltip.SetDefault("黑魔法的邪恶产物");
            Item.staff[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.damage = 89;
            item.DamageType = DamageClass.Magic;
            item.mana = 4;
            item.width = 28;
            item.height = 30;
            item.useTime = 20;
            item.useAnimation = 20;
<<<<<<< HEAD:Items/PollutElement/ContaminatedCurrent.cs
            item.useStyle = ItemUseStyleID.Shoot;
=======
            item.useStyle = ItemUseStyleID.HoldingOut;
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96:Items/PollutElement/污秽洋流.cs
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