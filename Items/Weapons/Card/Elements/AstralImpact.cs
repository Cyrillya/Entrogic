using System;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using System.Text;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;


namespace Entrogic.Items.Weapons.Card.Elements
{
    /// <summary>
    /// 星陨 的摘要说明
    /// 创建人机器名：DESKTOP-QDVG7GB
    /// 创建时间：2019/2/1 19:21:31
    /// </summary>
    public class AstralImpact : ModCard
    {
        public bool IsShootingStar
        {
            get
            {
                if (ModLoader.GetMod("FallenStar49") != null)
                {
                    foreach (Projectile proj in Main.projectile)
                    {
                        if (proj.type == ModLoader.GetMod("FallenStar49").ProjectileType("ShootingStar") && proj.active)
                            return true;
                    }
                }
                return false;
            }
        }
        public override void PreCreated()
        {
            said = "那天边的星陨即是为你送行的烟火！";
            rare = CardRareID.Gravitation;
            tooltip = "召唤49个从天而降的星陨（大嘘）";
            costMana = 1;
            series = CardSeriesID.Element;
        }
        public override void CardDefaults()
        {
            item.value = Item.sellPrice(silver: 1);
            item.rare = 1;
            item.shoot = mod.ProjectileType("陨星");
            item.damage = 18;
            if (Main.hardMode)
            {
                item.damage = 36;
            }
            item.knockBack = 4;
            item.shootSpeed = 4f;
            attackCardRemainingTimes = 15;
        }
        public override string AnotherMessages()
        {
            return "[c/EE4000:获得15次施展魔法“群星坠落”的机会]";
        }
        public override void AttackEffects(Player player, int type, Vector2 position, Vector2 shootTo, float speedX, float speedY, int damage, float knockBack, float speed)
        {
            for (int i = 0; i < (Main.rand.Next(4, 13)); i++)
            {
                Vector2 mouse = shootTo;
                Vector2 r = new Vector2(Main.rand.Next(380, 700), -1000 + -Main.rand.Next(-50, 51));
                r = r - new Vector2(Main.rand.Next(370), 0);
                if (Main.rand.NextBool(2)) r.X = -r.X;
                Vector2 vec = (mouse + r) - mouse;
                Vector2 finalVec = (vec.ToRotation() + MathHelper.Pi).ToRotationVector2() * 24;
                Projectile proj = Main.projectile[Projectile.NewProjectile(mouse + r, finalVec, type, damage, knockBack, Main.myPlayer, Main.MouseWorld.Y - 50, IsShootingStar && ModLoader.GetMod("FallenStar49") != null ? 1f : 0f)];
            }
        }
    }
}
