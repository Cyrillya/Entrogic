using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Entrogic.Common.Globals.Players
{
    public class TileSwimPlayer : ModPlayer
    {
        public List<int> SwimTiles = new();

        public override void PreUpdateMovement() {
            Point posPoint = new((int)(Player.position.X / 16f), (int)(Player.position.Y / 16f));
            if (SwimTiles.Count > 0 && WorldGen.InWorld(posPoint.X, posPoint.Y)) {
                int leftSafeAmount = 0;
                int rightSafeAmount = 0;
                int downSafeAmount = 0;
                int upSafeAmount = 0;
                #region Count Amount
                for (int i = 0; i < 3; i++) {
                    if (WorldGen.InWorld(posPoint.X - 1, posPoint.Y + i)) {
                        Tile t = Main.tile[posPoint.X - 1, posPoint.Y + i];
                        if (t.HasUnactuatedTile && SwimTiles.Contains(t.TileType)) {
                            leftSafeAmount++;
                        }
                        if (WorldGen.SolidTile(t) && !SwimTiles.Contains(t.TileType)) {
                            leftSafeAmount = 0;
                            break;
                        }
                    }
                }
                for (int i = 0; i < 3; i++) {
                    if (WorldGen.InWorld(posPoint.X + 2, posPoint.Y + i)) {
                        Tile t = Main.tile[posPoint.X + 2, posPoint.Y + i];
                        if (t.HasUnactuatedTile && SwimTiles.Contains(t.TileType)) {
                            rightSafeAmount++;
                        }
                        if (WorldGen.SolidTile(t) && !SwimTiles.Contains(t.TileType)) {
                            rightSafeAmount = 0;
                            break;
                        }
                    }
                }
                for (int i = 0; i < 2; i++) {
                    if (WorldGen.InWorld(posPoint.X + i, posPoint.Y + 3)) {
                        Tile t = Main.tile[posPoint.X + i, posPoint.Y + 3];
                        if (t.HasUnactuatedTile && SwimTiles.Contains(t.TileType)) {
                            downSafeAmount++;
                        }
                        if (WorldGen.SolidTile(t) && !SwimTiles.Contains(t.TileType)) {
                            downSafeAmount = 0;
                            break;
                        }
                    }
                }
                for (int i = 0; i < 2; i++) {
                    for (int j = -1; j < 2; j++) {
                        if (WorldGen.InWorld(posPoint.X + i, posPoint.Y - j)) {
                            Tile t = Main.tile[posPoint.X + i, posPoint.Y - j];
                            if (t.HasUnactuatedTile && SwimTiles.Contains(t.TileType)) {
                                upSafeAmount++;
                            }
                            if (WorldGen.SolidTile(t) && !SwimTiles.Contains(t.TileType)) {
                                upSafeAmount = 0;
                                break;
                            }
                        }
                    }
                }
                #endregion
                if (leftSafeAmount >= 1 && (Player.controlLeft || Player.velocity.X < 0)) {
                    Player.position.X -= 0.6f;
                    Player.slowFall = true;
                }
                if (rightSafeAmount >= 1 && (Player.controlRight || Player.velocity.X > 0)) {
                    Player.position.X += 0.6f;
                    Player.slowFall = true;
                }
                if (downSafeAmount >= 1 && Player.controlDown) {
                    Player.position.Y += 0.6f;
                    Player.slowFall = true;
                }
                if (upSafeAmount >= 1 && (Player.controlJump || Player.controlUp)) {
                    Player.position.Y -= 0.6f;
                    Player.velocity.Y = -4f; // 我推荐用"="
                    Player.slowFall = true;
                }
            }
        }

        public override void ResetEffects() {
            base.ResetEffects();
            SwimTiles.Clear();
        }

        public override void UpdateDead() {
            base.UpdateDead();
            SwimTiles.Clear();
        }
    }
}
