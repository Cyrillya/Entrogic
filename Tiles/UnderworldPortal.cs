using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ObjectData;
using Entrogic.Items.Miscellaneous.Placeable.Furnitrue;
using Microsoft.Xna.Framework.Graphics;

namespace Entrogic.Tiles
{
    public class UnderworldPortal : ModTile
    {
        public override void SetDefaults()
        {
			Main.tileSolid[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileBlockLight[Type] = false;
            Main.tileNoSunLight[Type] = false;
            Main.tileHammer[Type] = false;

            ModTranslation name = CreateMapEntryName();
            name.SetDefault("地狱传送门");
            dustType = -1;
            soundType = -1;
            //dustType = MyDustId.PurpleLight;
            AddMapEntry(new Color(105, 5, 196), name);
        }
        public override bool NewRightClick(int i, int j)
        {
            // 多人模式由服务器处理一切
            // 遇事不决给服务器做就行了，别想着客户端上能怎么优化，给服务器做速度快又没有大大小小的问题
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                MessageHelper.FindUWTeleporter(i, j, Main.myPlayer);
                return base.NewRightClick(i, j);
            }
            HandleTransportion(i, j, Main.myPlayer);
            return base.NewRightClick(i, j);
        }
        public static void HandleTransportion(int i, int j, int playernum, bool inServer = false)
        {
            int statX = i;
            int addX = 0;
            int PortalType = ModContent.TileType<UnderworldPortal>();
            Vector2 PortalUnderworld = Vector2.Zero;
            if (j >= Main.maxTilesY - 360) // 如果在地狱附近（传送目标地表）
            {
                Player player = Main.player[playernum];
                // 搜索可用的在地表附近的地狱门
                while (PortalUnderworld == Vector2.Zero)
                {
                    addX++;
                    for (int y = 10; y < Main.maxTilesY - 360; y++)
                    {
                        if (Main.tile[statX + addX, y] == null)
                            Main.tile[statX + addX, y] = new Tile();
                        if (Main.tile[statX - addX, y] == null)
                            Main.tile[statX - addX, y] = new Tile();
                        if (WorldGen.InWorld(statX + addX, y) && Main.tile[statX + addX, y].type == PortalType)
                        {
                            PortalUnderworld = new Vector2(statX + addX, y);
                        }
                        if (WorldGen.InWorld(statX - addX, y) && Main.tile[statX - addX, y].type == PortalType)
                        {
                            PortalUnderworld = new Vector2(statX - addX, y);
                        }
                    }
                    // 如果左右300格范围内没有，直接传送至出生点
                    if (addX >= 300)
                    {
                        ModHelper.Return(player);
                        return;
                    }
                }
                // 如果搜索到，传送
                player.HandleTeleport(PortalUnderworld.ToWorldCoordinates(0, -16));
            }
            else // 在地表
            {
                // 搜索可用的在地狱高度的地狱门
                while (PortalUnderworld == Vector2.Zero)
                {
                    for (int y = Main.maxTilesY - 180; y < Main.maxTilesY - 30; y++)
                    {
                        if (Main.tile[statX + addX, y] == null)
                            Main.tile[statX + addX, y] = new Tile();
                        if (Main.tile[statX - addX, y] == null)
                            Main.tile[statX - addX, y] = new Tile();
                        if (WorldGen.InWorld(statX + addX, y) && Main.tile[statX + addX, y].type == PortalType)
                        {
                            PortalUnderworld = new Vector2(statX + addX, y);
                        }
                        if (WorldGen.InWorld(statX - addX, y) && Main.tile[statX - addX, y].type == PortalType)
                        {
                            PortalUnderworld = new Vector2(statX - addX, y);
                        }
                    }
                    // 如果左右150格范围内没有，停止搜索
                    if (addX >= 150) break;
                    addX++;
                }
                // 如果没有找到，转而生成地狱门
                if (PortalUnderworld == Vector2.Zero)
                {
                    PortalUnderworld.X = statX;
                    PortalUnderworld.Y = Main.maxTilesY - Main.rand.Next(160, 210);
                    if (PortalUnderworld.X < 30)
                    {
                        PortalUnderworld.X = 30;
                    }
                    if (PortalUnderworld.X > Main.maxTilesX - 30 - 6)
                    {
                        PortalUnderworld.X = Main.maxTilesX - 30 - 6;
                    }
                    // 创建物块
                    if (inServer) MessageHelper.BuildBuilding("Buildings/UnderworldPortal.ebuilding", PortalUnderworld.ToWorldCoordinates());
                    else Buildings.Build("Buildings/UnderworldPortal.ebuilding", PortalUnderworld.ToWorldCoordinates());
                    // 定位到左上角的传送门方块
                    PortalUnderworld.X += 1;
                    PortalUnderworld.Y += 2;
                }
                // 2：进行传送
                Player player = Main.player[playernum];
                player.HandleTeleport(PortalUnderworld.ToWorldCoordinates(8, -16));
            }
        }
        public override void MouseOver(int i, int j)
        {
            Entrogic.Instance.showIconTexture = true;
            Entrogic.Instance.showIconTexture2 = Entrogic.ModTexturesTable["TeleportIcon"];
            base.MouseOver(i, j);
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            base.ModifyLight(i, j, ref r, ref g, ref b);
            r = 130f / 255f;
            g = 30f / 255f;
            b = 193f / 255f;
        }
        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            frameCounter++;
            if (frameCounter > 4)
            {
                frameCounter = 0;
                frame++;
                if (frame >= 32)
                {
                    frame = 0;
                }
            }
            //Main.tileFrame[Type] = frame;
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            // Flips the sprite
            SpriteEffects effects = SpriteEffects.None;

            Tile tile = Main.tile[i, j];
            Texture2D texture;
            if (Main.canDrawColorTile(i, j))
            {
                texture = Main.tileAltTexture[Type, (int)tile.color()];
            }
            else
            {
                texture = Main.tileTexture[Type];
            }
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            int animate = Main.tileFrame[Type] * 32;

            Color color = Lighting.GetColor(i, j);
            color.A = 200;
            if (tile.inActive())
            {
                color = tile.actColor(color);
            }

            Main.spriteBatch.Draw(
                texture,
                new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero,
                new Rectangle(tile.frameX, tile.frameY + animate, 16, 16),
                color, 0f, default(Vector2), 1f, effects, 0f);

            return false; // return false to stop vanilla draw.
        }
        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return false;
        }
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
    }
}
