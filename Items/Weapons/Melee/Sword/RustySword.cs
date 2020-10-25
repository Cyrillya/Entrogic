using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Melee.Sword
{
	public class RustySword : ModItem
	{
        public override void SetDefaults()
		{
            item.damage = 14;
            item.knockBack = 5f;
            item.crit += 14;
            item.rare = ItemRarityID.Blue;
            item.useTime = 40;
            item.useAnimation = 40;
            item.useStyle = ItemUseStyleID.Swing;
            item.autoReuse = true;
            item.DamageType = DamageClass.Melee;
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
