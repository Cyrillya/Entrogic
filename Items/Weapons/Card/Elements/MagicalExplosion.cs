
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Card.Elements
{
    public class MagicalExplosion : ModCard
    {
        public override void PreCreated()
        {
            said = "炸了！炸了！";
            rare = CardRareID.GrandUnified;
            tooltip = "让你的牌库 瞬 间 爆 炸";
            minion = true;
            costMana = 4;
            series = CardSeriesID.Element;
        }
        public override string AnotherMessages()
        {
            return "[c/EE4000:从牌库中抽取5张不同的攻击牌并释放]";
        }
        public override void CardDefaults()
        {
            item.rare = RareID.LV8;
            item.UseSound = SoundID.Item20;
            item.value = Item.sellPrice(silver: 10);
        }
        public override int DrawCardAmount(Player player, int number, ref bool[] series, ref bool[] style)
        {
            for (int i = 0; i < series.Length; i++)
                series[i] = true;
            style[0] = true;
            return 5;
        }
        public override bool HaveDrawCard(Player player, int number, int a, ref int type, ref int cost)
        {
            NPC target = null;
            float distanceMax = 128000f;
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly)
                {
                    // 计算与玩家的距离
                    float currentDistance = Vector2.Distance(npc.Center, Main.MouseWorld);
                    if (currentDistance < distanceMax)
                    {
                        distanceMax = currentDistance;
                        target = npc;
                    }
                }
            }
            Item i = new Item();
            i.SetDefaults(type);
            ModCard card = (ModCard)i.modItem;
            EntrogicPlayer ePlayer = EntrogicPlayer.ModPlayer(player);
            float power = 1f;
            if (ePlayer.GetHoldItem() != null)
                power = ePlayer.GetHoldItem().shootSpeed * 0.8f;
            if (target != null)
            {
                Vector2 vec = (target.Center - player.Center).ToRotation().ToRotationVector2() * i.shootSpeed;
                card.AttackEffects(player, i.shoot, player.Center, target.Center, vec.X, vec.Y, player.GetWeaponDamage(i), player.GetWeaponKnockback(i, 0.5f), i.shootSpeed + power);
            }
            else
                card.AttackEffects(player, i.shoot, player.Center, player.Center, Main.rand.NextFloat(-10f, 10f), Main.rand.NextFloat(-10f, 10f), player.GetWeaponDamage(i), player.GetWeaponKnockback(i, 0.5f), i.shootSpeed + power);
            return true;
        }
        public override bool AbleToGetFromRandom(Player player)
        {
            return Main.hardMode;
        }
    }
}