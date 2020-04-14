using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Equipables.Accessories
{
    public class StaticWatch : ModItem
    {
        public override void SetDefaults()
        {
            item.Size = new Vector2(32, 32);
            item.rare = 7;
            item.accessory = true;
            item.value = Item.sellPrice(0, 1, 20, 0);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            EntrogicPlayer ePlayer = player.GetModPlayer<EntrogicPlayer>();
            ePlayer.CardHandMax += 2;
            ePlayer.CardPassStatDelay -= 4 * 60;
            ePlayer.IsDelayCycle_StaticWatch = true;
        }
    }
}
