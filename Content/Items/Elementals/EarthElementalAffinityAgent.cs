using Entrogic.Common.Globals.Players;
using Entrogic.Content.Items.BaseTypes;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace Entrogic.Content.Items.Elementals
{
    public class EarthElementalAffinityAgent : ItemBase
    {
        public override void SetStaticDefaults() {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            DisplayName.SetDefault("Earth Elemental Affinity Agent");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "土元素亲和剂");

            base.SetStaticDefaults();
        }
        public override void SetDefaults() {
            Item.Size = new Vector2(32, 32);
            Item.rare = ItemRarityID.Lime;
            Item.accessory = true;
            Item.value = Item.sellPrice(0, 0, 80, 0);
        }
        public override void UpdateAccessory(Player player, bool hideVisual) {
            player.buffImmune[BuffID.Suffocation] = true;

            // 调用原版可被铲子挖掘的物块作为可以游泳的“松软物块”
            TileSwimPlayer swimPlayer = player.GetModPlayer<TileSwimPlayer>();
            for (int i = 0; i < TileID.Sets.CanBeDugByShovel.Length; i++) {
                if (!TileID.Sets.CanBeDugByShovel[i]) continue;
                swimPlayer.SwimTiles.Add(i);
            }
            // 云类也算
            for (int i = 0; i < TileID.Sets.Clouds.Length; i++) {
                if (!TileID.Sets.Clouds[i]) continue;
                swimPlayer.SwimTiles.Add(i);
            }
        }
    }
}
