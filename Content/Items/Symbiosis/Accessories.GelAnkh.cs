using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Entrogic.Content.Items.BaseTypes;

namespace Entrogic.Content.Items.Symbiosis
{
    public class GelAnkh : ItemBase
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.Size = new Vector2(48);
            Item.rare = RarityLevelID.Expert;
            Item.expert = true;
            Item.accessory = true;
            Item.value = Item.sellPrice(0, 3, 20, 0);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            //player.GetModPlayer<EntrogicPlayer>().CanReborn = true;
        }
    }
}
