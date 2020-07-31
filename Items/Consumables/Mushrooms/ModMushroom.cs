using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Consumables.Mushrooms
{
    public abstract class ModMushroom : ModItem
    {
        public virtual void MushroomDefaults() { }
        public sealed override void SetDefaults()
        {
            item.width = 10;
            item.height = 10;
            item.useAnimation = 30;
            item.useTime = 30;
            item.useStyle = ItemUseStyleID.EatingUsing;
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
