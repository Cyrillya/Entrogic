using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace Entrogic.Items.Equipables.Accessories
{
    public class LuckyCoin : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            item.Size = new Vector2(32, 32);
            item.rare = ItemRarityID.Lime;
            item.accessory = true;
            item.value = Item.buyPrice(0, 3, 0, 0);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            EntrogicPlayer ePlayer = player.GetModPlayer<EntrogicPlayer>();
            ePlayer.IsMoreManaOrCard_LuckyCoin = true;
        }
    }
}
