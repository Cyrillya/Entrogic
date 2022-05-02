
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Card.Gloves
{
    public class Glove : ModCard
    {
        public override void PreCreated()
        {
            glove = true;
        }
        public override void CardDefaults()
        {
            base.CardDefaults();
            item.shootSpeed = 5f;
            item.value = Item.buyPrice(0, 1);
            item.damage = 6;
        }
    }
}
