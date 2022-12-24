using Entrogic.Core.BaseTypes;
using Entrogic.Helpers.ID;

namespace Entrogic.Content.Symbiosis
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
