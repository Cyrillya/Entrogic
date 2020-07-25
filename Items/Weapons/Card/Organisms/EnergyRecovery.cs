
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Card.Organisms
{
    public class EnergyRecovery : ModCard
    {
        public override void PreCreated()
        {
            said = "精神焕发";
            rare = CardRareID.Gravitation;
            tooltip = "恢复50点生命，玩家剩余费用+2，下次过牌多抽一张牌";
            costMana = 1;
            minion = true;
            series = CardSeriesID.Organism;
            healMana = 2;
        }
        public override void CardDefaults()
        {
            item.rare = ItemRarityID.Blue;
            item.UseSound = SoundID.Item20;
            item.value = Item.sellPrice(silver: 1);
        }
        public override void MinionEffects(Player player, Vector2 position, int damage, float knockBack, int number)
        {
            int healAmount = (int)MathHelper.Min(50f, player.statLifeMax2 - player.statLife);
            player.statLife += healAmount;
            player.HealEffect(healAmount);
            EntrogicPlayer ePlayer = player.GetModPlayer<EntrogicPlayer>();
            ePlayer.MoreCard_EnergyRecovery++;
        }
    }
}
