using Entrogic.Common.ModSystems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Entrogic.Content.Tiles.BaseTypes
{
    public abstract class QuickTileItem : ModItem
    {
        public string Itemname;
        public string Itemtooltip;
        private readonly int Rare;
        private readonly int SacrificeNeeded;
        private readonly string TexturePath;
        private readonly bool PathHasName;
        public abstract int TileType();

        /// <summary>
        /// 快速设置放置物块物品
        /// </summary>
        /// <param name="name">物品显示名</param>
        /// <param name="tooltip">物品提示</param>
        /// <param name="rare">物品稀有值</param>
        /// <param name="sacrificeNeeded">旅途模式研究所需物品数量</param>
        /// <param name="texturePath">图片资源文件路径</param>
        /// <param name="pathHasName">资源文件路径是否包含名称</param>
        public QuickTileItem(string name, string tooltip, int rare, int sacrificeNeeded = 1, string texturePath = null, bool pathHasName = false) {
            Itemname = name;
            Itemtooltip = tooltip;
            Rare = rare;
            TexturePath = texturePath;
            SacrificeNeeded = sacrificeNeeded;
            PathHasName = pathHasName;
        }

        public override string Texture => string.IsNullOrEmpty(TexturePath) ? "Entrogic/Content/Tiles/QuickTile" : TexturePath + (PathHasName ? string.Empty : Name);

        public override ModItem Clone(Item item) {
            return base.Clone(item);
        }

        public virtual void SafeSetDefaults() { }

        public override void SetStaticDefaults() {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = SacrificeNeeded;
            DisplayName.SetDefault(Itemname ?? "ERROR");
            Tooltip.SetDefault(Itemtooltip ?? "Report me please!");
        }

        public override void SetDefaults() {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = TileType();
            Item.rare = Rare;

            SafeSetDefaults();
        }
    }
}
