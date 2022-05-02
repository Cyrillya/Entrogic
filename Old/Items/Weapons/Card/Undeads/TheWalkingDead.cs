
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Card.Undeads
{
    public class TheWalkingDead : ModCard
    {
        public override void PreCreated()
        {
            said = "至死不渝！";
            rare = CardRareID.WeakNuclear;
            tooltip = "复活墓地中的所有牌，并些许增加牌库中所有牌的费用";
            costMana = 2;
            minion = true;
            series = CardSeriesID.Undead;
        }
        public override void CardDefaults()
        {
            item.rare = RareID.LV4;
            item.UseSound = SoundID.Item20;
            item.value = Item.sellPrice(silver: 10);
        }
        public override void MinionEffects(Player player, Vector2 position, int damage, float knockBack, int number)
        {
            EntrogicPlayer ePlayer = player.GetModPlayer<EntrogicPlayer>();
            for (int i = 0; i < ePlayer.CardReadyType.Length; i++)
            {
                bool hasCard = false;
                if (ePlayer.CardReadyType[i] == 0)
                {
                    for (int j = 0; j < ePlayer.CardGraveType.Length; j++)
                        if (ePlayer.CardGraveType[j] != 0)
                        {
                            ePlayer.CardReadyType[i] = ePlayer.CardGraveType[j];
                            Item item = new Item();
                            item.SetDefaults(ePlayer.CardGraveType[j]);
                            ModCard card = (ModCard)item.modItem;
                            ePlayer.CardReadyCost[i] = card.costMana;
                            ePlayer.CardGraveType[j] = 0;
                            hasCard = true;
                            break;
                        }
                    if (!hasCard)
                        break;
                }
                ePlayer.CardReadyCost[i]++;
            }
        }
    }
    public class TheWalkingDead_Strong : ModCard
    {
        public override void PreCreated()
        {
            said = "工作怎么会停止呢";
            rare = CardRareID.StrongNuclear;
            tooltip = "复活墓地中的所有牌并将此牌的一张复制置入牌库，并些许增加牌库中所有牌的费用";
            costMana = 2;
            minion = true;
            series = CardSeriesID.Undead;
        }
        public override void CardDefaults()
        {
            item.rare = RareID.LV4;
            item.UseSound = SoundID.Item20;
            item.value = Item.sellPrice(silver: 10);
        }
        public override void MinionEffects(Player player, Vector2 position, int damage, float knockBac, int numberk)
        {
            EntrogicPlayer ePlayer = player.GetModPlayer<EntrogicPlayer>();
            for (int i = 0; i < ePlayer.CardReadyType.Length; i++)
            {
                bool hasCard = false;
                if (ePlayer.CardReadyType[i] == 0)
                {
                    for (int j = 0; j < ePlayer.CardGraveType.Length; j++)
                        if (ePlayer.CardGraveType[j] != 0)
                        {
                            ePlayer.CardReadyType[i] = ePlayer.CardGraveType[j];
                            Item item = new Item();
                            item.SetDefaults(ePlayer.CardGraveType[j]);
                            ModCard card = (ModCard)item.modItem;
                            ePlayer.CardReadyCost[i] = card.costMana;
                            ePlayer.CardGraveType[j] = 0;
                            hasCard = true;
                            break;
                        }
                    if (!hasCard)
                    {
                        ePlayer.CardReadyType[i] = item.type;
                        ePlayer.CardReadyCost[i] = costMana;
                        break;
                    }
                }
                if (ePlayer.CardReadyType[i] != 0)
                    ePlayer.CardReadyCost[i]++;
            }
        }
        public override bool AbleToGetFromRandom(Player player)
        {
            return false;
        }
    }
}
