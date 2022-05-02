using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Equipables.Accessories
{
    public class EarthElementalAffinityAgent : ModItem
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
            item.value = Item.sellPrice(0, 0, 80, 0);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            EntrogicPlayer ePlayer = player.GetModPlayer<EntrogicPlayer>();
            ePlayer.CanSwimTile = true;
            player.buffImmune[BuffID.Suffocation] = true;
        }
    }
}
