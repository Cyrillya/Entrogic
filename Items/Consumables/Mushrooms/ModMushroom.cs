using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Consumables.Mushrooms
{
    public abstract class ModMushroom : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 10;
        }
        public virtual void MushroomDefaults() { }
        public sealed override void SetDefaults()
        {
            item.width = 10;
            item.height = 10;
            item.useAnimation = 30;
            item.useTime = 30;
<<<<<<< HEAD
            item.useStyle = ItemUseStyleID.EatFood;
=======
            item.useStyle = ItemUseStyleID.EatingUsing;
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
            item.maxStack = 50;
            item.value = Item.sellPrice(0, 0, 50);
            item.consumable = true;
            item.rare = ItemRarityID.Quest;
            item.UseSound = SoundID.Item2;
            base.SetDefaults();
            MushroomDefaults();
        }
    }
}
