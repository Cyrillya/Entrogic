﻿using Entrogic.Content.SpookyLamps;
using Terraria.Enums;
using Terraria.ObjectData;

namespace Entrogic.Core.BaseTypes
{
    public abstract class SpookyLamp : TileBase
    {
        internal Asset<Texture2D> flameTexture;

        public virtual Color LightColor { get; }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b) {
            Tile tile = Main.tile[i, j];
            if (tile.TileFrameX == 0) {
                // We can support different light colors for different styles here: switch (tile.frameY / 54)
                r = LightColor.R / 255f;
                g = LightColor.G / 255f;
                b = LightColor.B / 255f;
            }
        }

        public virtual Asset<Texture2D> SelectFlameTexture() => ModContent.Request<Texture2D>($"{Texture}_Flame");

        public override void SetStaticDefaults() {
            // Properties
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileWaterDeath[Type] = true;
            Main.tileLavaDeath[Type] = true;
            // Main.tileFlame[Type] = true; // This breaks it.
            SpookyLampHandler.Lamps.Add(Type);

            // Placement
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1xX);
            TileObjectData.newTile.DrawYOffset = 2;// 原版的FloorLamp地面有两个像素是陷进去的
            TileObjectData.newTile.WaterDeath = true;
            TileObjectData.newTile.WaterPlacement = LiquidPlacement.NotAllowed;
            TileObjectData.newTile.LavaPlacement = LiquidPlacement.NotAllowed;
            TileObjectData.addTile(Type);

            // Etc
            AddMapEntry(new Color(253, 221, 3), Language.GetText("MapObject.FloorLamp"));

            // Assets
            if (!Main.dedServ) {
                flameTexture = SelectFlameTexture();
            }
        }

        public override void HitWire(int i, int j) {
            Tile tile = Main.tile[i, j];
            int topY = j - tile.TileFrameY / 18 % 3;
            short frameAdjustment = (short)(tile.TileFrameX > 0 ? -18 : 18);

            Main.tile[i, topY].TileFrameX += frameAdjustment;
            Main.tile[i, topY + 1].TileFrameX += frameAdjustment;
            Main.tile[i, topY + 2].TileFrameX += frameAdjustment;

            Wiring.SkipWire(i, topY);
            Wiring.SkipWire(i, topY + 1);
            Wiring.SkipWire(i, topY + 2);

            // Avoid trying to send packets in singleplayer.
            if (Main.netMode != NetmodeID.SinglePlayer) {
                NetMessage.SendTileSquare(-1, i, topY + 1, 3, TileChangeType.None);
            }
        }

        public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects) {
            if (i % 2 == 1) {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch) {
            SpriteEffects effects = SpriteEffects.None;

            if (i % 2 == 1) {
                effects = SpriteEffects.FlipHorizontally;
            }

            Vector2 zero = new(Main.offScreenRange, Main.offScreenRange);

            if (Main.drawToScreen) {
                zero = Vector2.Zero;
            }

            Tile tile = Main.tile[i, j];
            int width = 16;
            int offsetY = 0;
            int height = 16;
            short TileFrameX = tile.TileFrameX;
            short frameY = tile.TileFrameY;

            TileLoader.SetDrawPositions(i, j, ref width, ref offsetY, ref height, ref TileFrameX, ref frameY);

            ulong randSeed = Main.TileFrameSeed ^ (ulong)((long)j << 32 | (long)(uint)i); // Don't remove any casts.

            // We can support different flames for different styles here: int style = Main.tile[j, i].frameY / 54;
            for (int c = 0; c < 7; c++) {
                float shakeX = Utils.RandomInt(ref randSeed, -10, 11) * 0.15f;
                float shakeY = Utils.RandomInt(ref randSeed, -10, 1) * 0.35f;

                spriteBatch.Draw(flameTexture.Value, new Vector2(i * 16 - (int)Main.screenPosition.X - (width - 16f) / 2f + shakeX, j * 16 - (int)Main.screenPosition.Y + offsetY + shakeY) + zero, new Rectangle(TileFrameX, frameY, width + 2, height), new Color(100, 100, 100, 0), 0f, default, 1f, effects, 0f);
            }
        }
    }
}
