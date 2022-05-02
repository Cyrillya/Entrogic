using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ObjectData;
using Entrogic.NPCs.Enemies;
using Terraria.ID;

namespace Entrogic.NPCs.Banners
{
    public class MonsterBanner : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.StyleWrapLimit = 111;
            TileObjectData.addTile(Type);
            dustType = -1;
            TileID.Sets.DisableSmartCursor[Type] = true;
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("{$MapObject.Banner}");
            AddMapEntry(new Color(13, 88, 130), name);
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            int style = frameX / 18;
            switch (style)
            {
                case 0:
                    Item.NewItem(i * 16, j * 16, 16, 48, ItemType<CrimsonSpiritBanner>());
                    break;
                case 1:
                    Item.NewItem(i * 16, j * 16, 16, 48, ItemType<AngryofCorruptionBanner>());
                    break;
                case 2:
                    Item.NewItem(i * 16, j * 16, 16, 48, ItemType<StoneSlimeBanner>());
                    break;
                default:
                    return;
            }
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)
            {
                Player player = Main.LocalPlayer;
                int style = Main.tile[i, j].frameX / 18;
                switch (style)
                {
                    case 0:
                        Main.SceneMetrics.NPCBannerBuff[NPCType<CrimsonSpirit>()] = true;
                        break;
                    case 1:
                        Main.SceneMetrics.NPCBannerBuff[NPCType<AngryofCorruption>()] = true;
                        break;
                    case 2:
                        Main.SceneMetrics.NPCBannerBuff[NPCType<StoneSlime>()] = true;
                        break;
                    default:
                        return;
                }
                Main.SceneMetrics.hasBanner = true;
            }
        }

        public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
        {
            if (i % 2 == 1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
        }
    }
}