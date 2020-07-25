using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace Entrogic.Items.Equipables.Accessories
{
    public class CryptTreasure : ModItem
    {
        public override void SetDefaults()
        {
            item.Size = new Vector2(32, 32);
            item.rare = ItemRarityID.Lime;
            item.accessory = true;
            item.value = Item.sellPrice(0, 1, 20, 0);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            EntrogicPlayer ePlayer = player.GetModPlayer<EntrogicPlayer>();
            ePlayer.IsMoreMana_CryptTreasure = true;
        }
    }
}
