using Entrogic.Projectiles.Melee.Swords;

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Melee.Sword
{
    public class GoldenHarvest : ModItem
    {
        public override void SetDefaults()
        {
            item.UseSound = SoundID.Item1;
            item.Size = new Vector2(168, 84);
            item.useTime = 21;
            item.useAnimation = 21;
            item.useStyle = ItemUseStyleID.Thrust;
            item.knockBack = 5f;
            item.value = Item.sellPrice(0, 1, 24, 0);
            item.rare = RareID.LV4;
            item.damage = 36;
            item.crit += 9;
            item.knockBack = 7f;
            item.shoot = ProjectileType<GoldenHarvestProjectile>();
            item.shootSpeed = 0.01f;
            item.noUseGraphic = true;
            item.DamageType = DamageClass.Melee;
            item.autoReuse = true;
            return;
        }
        public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
        {
            if (crit && target != null)
            { 
                target.GetGlobalNPC<MoreMoney>().moneyPoint50++; 
            }
        }
    }
    public class MoreMoney : GlobalNPC
    {
        public float moneyPoint50;
        public int ExtraMoney;
        public override void NPCLoot(NPC npc)
        {
            if (moneyPoint50 < 3)
                return;
            float n = 5 * (moneyPoint50 / 2 + 1);
            float n2 = (float)Main.rand.Next(500, 1501);
            float n3 = n * (n2 / 1000);

            npc.value += ExtraMoney;
            float normalMoney = npc.value;
            if (n3 > 2.5f) npc.value *= n3;
            else npc.value *= 2.5f;

            float add = npc.value - normalMoney;
        }
        public override bool InstancePerEntity => true;
    }
}
