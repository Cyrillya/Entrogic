
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Card.Nones
{
    public class PerpetuumMobileoftheFifthKind : ModCard
    {
        public override void PreCreated()
        {
            said = "永动机是存在的！";
            rare = CardRareID.GrandUnified;
            tooltip = "获得1点费用，抽1张牌，将该牌的两张复制洗入牌库";
            minion = true;
            costMana = 0;
        }
        public override void CardDefaults()
        {
            item.rare = RareID.LV3;
            item.UseSound = SoundID.Item20;
            item.value = Item.sellPrice(silver: 10);
        }
        public override void MinionEffects(Player player, Vector2 position, int damage, float knockBack, int number)
        {
            EntrogicPlayer ePlayer = player.GetModPlayer<EntrogicPlayer>();
            if (ePlayer.ManaLeft < EntrogicPlayer.ManaTrueMax)
                ePlayer.ManaLeft++;
            int Added = 0;
            for (int i = 0; i < ePlayer.CardReadyType.Length; i++)
                if (ePlayer.CardReadyType[i] == 0)
                {
                    ePlayer.CardReadyType[i] = item.type;
                    ePlayer.CardReadyCost[i] = costMana;
                    Added++;
                    if (Added >= 2)
                        break;
                }
            if (ePlayer.LibNum <= 0)
            {
                return;
            }
            for (int i = 0; i < ePlayer.CardHandType.Length; i++)
                while (ePlayer.CardHandType[i] == 0)
                    for (int j = 0; j < ePlayer.CardReadyType.Length; j++)
                        if (ePlayer.CardReadyType[j] != 0 && Main.rand.NextBool(4))
                        {
                            ePlayer.CardHandType[i] = ePlayer.CardReadyType[j];
                            ePlayer.CardHandCost[i] = ePlayer.CardReadyCost[j];
                            ePlayer.CardReadyType[j] = 0;
                            ePlayer.CardReadyCost[j] = 0;
                            return;
                        }
        }
    }
}
