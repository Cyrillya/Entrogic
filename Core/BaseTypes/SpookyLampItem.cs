namespace Entrogic.Core.BaseTypes
{
    public abstract class SpookyLampItem : ItemBase
    {
        public override void SetStaticDefaults() {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults() {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.maxStack = 99;
            Item.consumable = true;
            Item.createTile = CreateTile();
            Item.width = 10;
            Item.height = 24;
            Item.value = 500;
        }

        public abstract int CreateTile();
    }
}
