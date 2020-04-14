using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Melee.Sword
{
	// Token: 0x020000AC RID: 172
	public class 生锈的长剑 : ModItem
	{
		// Token: 0x060003E5 RID: 997 RVA: 0x000031BD File Offset: 0x000013BD
		public override void SetStaticDefaults()
		{
			base.Tooltip.SetDefault("“一把十分古老的剑，它锈迹斑斑”\n" +
                "让你的敌人获得破伤风\n" +
                "“小心割到手”");
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0002E504 File Offset: 0x0002C704
		public override void SetDefaults()
		{
			base.item.damage = 14;
			base.item.knockBack = 5f;
			base.item.crit += 14;
			base.item.rare = 1;
			base.item.useTime = 40;
			base.item.useAnimation = 40;
			base.item.useStyle = 1;
			base.item.autoReuse = true;
			base.item.melee = true;
			base.item.value = Item.sellPrice(0, 0, 90, 0);
			base.item.UseSound = SoundID.Item1;
			base.item.scale = 0.9f;
			base.item.width = 52;
			base.item.height = 52;
			base.item.maxStack = 1;
			base.item.useTurn = true;
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x000031D1 File Offset: 0x000013D1
		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			target.AddBuff(32, 240, false);
			target.AddBuff(36, 240, false);
		}
	}
}
