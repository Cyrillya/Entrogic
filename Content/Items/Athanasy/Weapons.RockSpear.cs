using Entrogic.Content.Items.BaseTypes;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Entrogic.Content.Items.Athanasy
{
    public class RockSpear : ItemBase
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 22;
            Item.useTime = 22;
            Item.shootSpeed = 4.6f;
            Item.knockBack = 5.8f;
            Item.width = 56;
            Item.height = 56;
            Item.damage = 55;
            Item.UseSound = SoundID.Item1;
            //Item.shoot = ProjectileType<RockSpearProj>();
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(0, 3, 50);
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Melee;
        }
        public override void HoldItem(Player player)
        {
            Item.autoReuse = true;
            base.HoldItem(player);
        }
    }
}