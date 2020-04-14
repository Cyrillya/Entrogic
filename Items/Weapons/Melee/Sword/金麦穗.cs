using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Melee.Sword
{
    public class 金麦穗 : ModItem
    {
        public override void SetDefaults()
        {
            item.UseSound = SoundID.Item1;
            item.Size = new Vector2(168, 84);
            item.useTime = 21;
            item.useAnimation = 21;
            item.useStyle = 3;
            item.knockBack = 5f;
            item.value = Item.sellPrice(0, 1, 24, 0);
            item.rare = RareID.LV4;
            item.damage = 26;
            item.crit += 4;
            item.knockBack = 7f;
            item.shoot = mod.ProjectileType("金麦穗");
            item.shootSpeed = 20f;
            item.noUseGraphic = true;
            item.melee = true;
            return;
        }
        public override void HoldItem(Player player)
        {
            MoreMoney.money = true;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 vector;
            vector.X = ((player.direction == -1) ? player.position.X - item.width : player.position.X + item.width);
            vector.Y = player.Center.Y;
            Projectile.NewProjectile(position.X = vector.X, position.Y = vector.Y, speedX * 0f, speedY * 0f, type, damage, knockBack, Main.myPlayer, 0f, 0f);
            return false;
        }
        public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
        {
            if (crit && target != null)
            { target.GetGlobalNPC<MoreMoney>().moneyPoint50++; }
        }
    }
    public class MoreMoney : GlobalNPC
    {
        public float moneyPoint50 = 0;
        public static bool money = false;
        public override void NPCLoot(NPC npc)
        {
            if (money)
            {
                float n = 5 * (moneyPoint50 / 2 + 1);
                float n2 = (float)(Main.rand.Next(500, 1501));
                float n3 = n * (n2 / 1000);

                float normalMoney = npc.value;
                if (n3 > 2.5f) npc.value *= n3;
                else npc.value *= 2.5f;

                float add = npc.value - normalMoney;
            }
            moneyPoint50 = 0;
        }
        public override bool InstancePerEntity => true;
    }
}
