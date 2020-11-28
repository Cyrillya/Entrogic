using System;

using Entrogic.Projectiles.衰落魔像;

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.AntaGolem
{
    public class RockSpear : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
<<<<<<< HEAD:Items/AntaGolem/RockSpear.cs
            item.useStyle = ItemUseStyleID.Shoot;
=======
            item.useStyle = ItemUseStyleID.HoldingOut;
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96:Items/AntaGolem/巨石长枪.cs
            item.useAnimation = 22;
            item.useTime = 22;
            item.shootSpeed = 4.6f;
            item.knockBack = 5.8f;
            item.width = 56;
            item.height = 56;
            item.damage = 55;
            item.UseSound = SoundID.Item1;
            item.shoot = ProjectileType<RockSpearProj>();
            item.rare = ItemRarityID.LightRed;
            item.value = Item.sellPrice(0, 3, 50);
            item.noMelee = true;
            item.noUseGraphic = true;
            item.DamageType = DamageClass.Melee;
        }
        public override void HoldItem(Player player)
        {
            item.autoReuse = true;
            base.HoldItem(player);
        }
    }
}