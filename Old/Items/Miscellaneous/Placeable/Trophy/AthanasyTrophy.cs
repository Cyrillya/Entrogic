﻿using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using Terraria.Localization;
using Entrogic.Tiles;

namespace Entrogic.Items.Miscellaneous.Placeable.Trophy
{
    public class AthanasyTrophy : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("{$Mods.Entrogic.NPCName.衰落魔像} " + "{$MapObject.Trophy}");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "衰落魔像纪念章");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.SkeletronPrimeTrophy);
            item.createTile = TileType<BossTrophy>();
            item.placeStyle = 1;
        }
    }
}