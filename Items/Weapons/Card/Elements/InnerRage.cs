
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Card.Elements
{
    public class InnerRage : ModCard
    {
        public override void PreCreated()
        {
            said = "纯粹的天火会烧毁一切！";
            rare = CardRareID.Gravitation;
            tooltip = "销毁你使用的下一张手牌，抽一张元素类牌";
            costMana = 1;
            minion = true;
            series = CardSeriesID.Element;
        }
        public override string AnotherMessages()
        {
            return "烧到自己就搞笑了";
        }
        public override void CardDefaults()
        {
            item.rare = 1;
            item.UseSound = SoundID.Item20;
            item.value = Item.sellPrice(silver: 1);
        }
        public override void MinionEffects(Player player, Vector2 position, int damage, float knockBack, int number)
        {
            EntrogicPlayer ePlayer = player.GetModPlayer<EntrogicPlayer>();
            ePlayer.IsDestroyNextCard_InnerRage = true;
        }
    }
}

