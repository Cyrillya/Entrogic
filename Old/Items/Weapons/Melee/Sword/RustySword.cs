using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Melee.Sword
{
	public class RustySword : ModItem
<<<<<<< HEAD
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
=======
	{
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
        public override void SetDefaults()
		{
            item.damage = 14;
            item.knockBack = 5f;
            item.crit += 14;
            item.rare = ItemRarityID.Blue;
            item.useTime = 40;
            item.useAnimation = 40;
<<<<<<< HEAD
            item.useStyle = ItemUseStyleID.Swing;
            item.autoReuse = true;
            item.DamageType = DamageClass.Melee;
=======
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.autoReuse = true;
            item.melee = true;
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
            item.value = Item.sellPrice(0, 0, 90, 0);
            item.UseSound = SoundID.Item1;
            item.scale = 0.9f;
            item.width = 52;
            item.height = 52;
            item.maxStack = 1;
            item.useTurn = true;
		}
		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			target.AddBuff(32, 240, false);
			target.AddBuff(36, 240, false);
		}
	}
}
