using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using System;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;
using Terraria.World.Generation;
using Terraria.GameContent.Generation;

namespace Entrogic
{
    internal class ModGenHelper
    {
        public static void SmoothTile(int StartX, int StartY, int X, int Y, GenerationProgress progress, bool useProgress = false)
        {
            for (int k = StartX; k < X; k++)
            {
                if (useProgress)
                {
                    int truex = k - StartX;
                    float value = (float)truex / 160;
                    progress.Set(value);
                }
                for (int l = StartY; l < Y; l++)
                {
                    if (!WorldGen.InWorld(k, l)) continue;
                    if (!Main.tile[k, l - 1].active())
                    {
                        if (WorldGen.SolidTile(k, l) && TileID.Sets.CanBeClearedDuringGeneration[(int)Main.tile[k, l].type])
                        {
                            if (!Main.tile[k - 1, l].halfBrick() && !Main.tile[k + 1, l].halfBrick() && Main.tile[k - 1, l].slope() == 0 && Main.tile[k + 1, l].slope() == 0)
                            {
                                if (WorldGen.SolidTile(k, l + 1))
                                {
                                    if (!WorldGen.SolidTile(k - 1, l) && !Main.tile[k - 1, l + 1].halfBrick() && WorldGen.SolidTile(k - 1, l + 1) && WorldGen.SolidTile(k + 1, l) && !Main.tile[k + 1, l - 1].active())
                                    {
                                        if (WorldGen.genRand.Next(2) == 0)
                                        {
                                            WorldGen.SlopeTile(k, l, 2);
                                        }
                                        else
                                        {
                                            WorldGen.PoundTile(k, l);
                                        }
                                    }
                                    else if (!WorldGen.SolidTile(k + 1, l) && !Main.tile[k + 1, l + 1].halfBrick() && WorldGen.SolidTile(k + 1, l + 1) && WorldGen.SolidTile(k - 1, l) && !Main.tile[k - 1, l - 1].active())
                                    {
                                        if (WorldGen.genRand.Next(2) == 0)
                                        {
                                            WorldGen.SlopeTile(k, l, 1);
                                        }
                                        else
                                        {
                                            WorldGen.PoundTile(k, l);
                                        }
                                    }
                                    else if (WorldGen.SolidTile(k + 1, l + 1) && WorldGen.SolidTile(k - 1, l + 1) && !Main.tile[k + 1, l].active() && !Main.tile[k - 1, l].active())
                                    {
                                        WorldGen.PoundTile(k, l);
                                    }
                                    if (WorldGen.SolidTile(k, l))
                                    {
                                        if (WorldGen.SolidTile(k - 1, l) && WorldGen.SolidTile(k + 1, l + 2) && !Main.tile[k + 1, l].active() && !Main.tile[k + 1, l + 1].active() && !Main.tile[k - 1, l - 1].active())
                                        {
                                            WorldGen.KillTile(k, l, false, false, false);
                                        }
                                        else if (WorldGen.SolidTile(k + 1, l) && WorldGen.SolidTile(k - 1, l + 2) && !Main.tile[k - 1, l].active() && !Main.tile[k - 1, l + 1].active() && !Main.tile[k + 1, l - 1].active())
                                        {
                                            WorldGen.KillTile(k, l, false, false, false);
                                        }
                                        else if (!Main.tile[k - 1, l + 1].active() && !Main.tile[k - 1, l].active() && WorldGen.SolidTile(k + 1, l) && WorldGen.SolidTile(k, l + 2))
                                        {
                                            if (WorldGen.genRand.Next(5) == 0)
                                            {
                                                WorldGen.KillTile(k, l, false, false, false);
                                            }
                                            else if (WorldGen.genRand.Next(5) == 0)
                                            {
                                                WorldGen.PoundTile(k, l);
                                            }
                                            else
                                            {
                                                WorldGen.SlopeTile(k, l, 2);
                                            }
                                        }
                                        else if (!Main.tile[k + 1, l + 1].active() && !Main.tile[k + 1, l].active() && WorldGen.SolidTile(k - 1, l) && WorldGen.SolidTile(k, l + 2))
                                        {
                                            if (WorldGen.genRand.Next(5) == 0)
                                            {
                                                WorldGen.KillTile(k, l, false, false, false);
                                            }
                                            else if (WorldGen.genRand.Next(5) == 0)
                                            {
                                                WorldGen.PoundTile(k, l);
                                            }
                                            else
                                            {
                                                WorldGen.SlopeTile(k, l, 1);
                                            }
                                        }
                                    }
                                }
                                if (WorldGen.SolidTile(k, l) && !Main.tile[k - 1, l].active() && !Main.tile[k + 1, l].active())
                                {
                                    WorldGen.KillTile(k, l, false, false, false);
                                }
                            }
                        }
                        else if (!Main.tile[k, l].active())
                        {
                            if (WorldGen.SolidTile(k - 1, l + 1) && WorldGen.SolidTile(k + 1, l) && !Main.tile[k - 1, l].active() && !Main.tile[k + 1, l - 1].active())
                            {
                                WorldGen.PlaceTile(k, l, (int)Main.tile[k, l + 1].type, false, false, -1, 0);
                                if (WorldGen.genRand.Next(2) == 0)
                                {
                                    WorldGen.SlopeTile(k, l, 2);
                                }
                                else
                                {
                                    WorldGen.PoundTile(k, l);
                                }
                            }
                            if (WorldGen.SolidTile(k + 1, l + 1) && WorldGen.SolidTile(k - 1, l) && !Main.tile[k + 1, l].active() && !Main.tile[k - 1, l - 1].active())
                            {
                                WorldGen.PlaceTile(k, l, (int)Main.tile[k, l + 1].type, false, false, -1, 0);
                                if (WorldGen.genRand.Next(2) == 0)
                                {
                                    WorldGen.SlopeTile(k, l, 1);
                                }
                                else
                                {
                                    WorldGen.PoundTile(k, l);
                                }
                            }
                        }
                    }
                    else if (!Main.tile[k, l + 1].active() && WorldGen.genRand.Next(2) == 0 && WorldGen.SolidTile(k, l) && !Main.tile[k - 1, l].halfBrick() && !Main.tile[k + 1, l].halfBrick() && Main.tile[k - 1, l].slope() == 0 && Main.tile[k + 1, l].slope() == 0 && WorldGen.SolidTile(k, l - 1))
                    {
                        if (WorldGen.SolidTile(k - 1, l) && !WorldGen.SolidTile(k + 1, l) && WorldGen.SolidTile(k - 1, l - 1))
                        {
                            WorldGen.SlopeTile(k, l, 3);
                        }
                        else if (WorldGen.SolidTile(k + 1, l) && !WorldGen.SolidTile(k - 1, l) && WorldGen.SolidTile(k + 1, l - 1))
                        {
                            WorldGen.SlopeTile(k, l, 4);
                        }
                    }
                }
            }
            for (int m = StartX; m < X; m++)
            {
                for (int n = StartY; n < Y; n++)
                {
                    if (WorldGen.genRand.Next(2) == 0 && !Main.tile[m, n - 1].active() && Main.tile[m, n].type != 137 && Main.tile[m, n].type != 48 && Main.tile[m, n].type != 232 && Main.tile[m, n].type != 191 && Main.tile[m, n].type != 151 && Main.tile[m, n].type != 274 && Main.tile[m, n].type != 75 && Main.tile[m, n].type != 76 && WorldGen.SolidTile(m, n) && Main.tile[m - 1, n].type != 137 && Main.tile[m + 1, n].type != 137)
                    {
                        if (WorldGen.SolidTile(m, n + 1) && WorldGen.SolidTile(m + 1, n) && !Main.tile[m - 1, n].active())
                        {
                            WorldGen.SlopeTile(m, n, 2);
                        }
                        if (WorldGen.SolidTile(m, n + 1) && WorldGen.SolidTile(m - 1, n) && !Main.tile[m + 1, n].active())
                        {
                            WorldGen.SlopeTile(m, n, 1);
                        }
                    }
                    if (Main.tile[m, n].slope() == 1 && !WorldGen.SolidTile(m - 1, n))
                    {
                        WorldGen.SlopeTile(m, n, 0);
                        WorldGen.PoundTile(m, n);
                    }
                    if (Main.tile[m, n].slope() == 2 && !WorldGen.SolidTile(m + 1, n))
                    {
                        WorldGen.SlopeTile(m, n, 0);
                        WorldGen.PoundTile(m, n);
                    }
                }
            }
        }
        public static void RoundTile(int X, int Y, int Xmult, int Ymult, double minStrength, double maxStrength, bool initialplace, int type, int handleTile = 0, double length = 0.5, int liquidType = 0, byte liquid = 0)
        {
            if (initialplace)
            {
                Runner(true, X, Y, minStrength * 5.0, maxStrength * 5.0, 1, type, liquidType, liquid, handleTile, length);
            }
            for (int truesloperight = 0; truesloperight < 350; truesloperight++)
            {
                int Xstray = (int)(0.0 - Math.Sin((double)truesloperight) * (double)Xmult);
                int level = (int)(0.0 - Math.Cos((double)truesloperight) * (double)Ymult);
                Runner(true, X + Xstray, Y + level, minStrength, maxStrength, 1, type, liquidType, liquid, handleTile, length);
            }
        }
        public static void RoundWall(int X, int Y, int Xmult, int Ymult, double minStrength, double maxStrength, bool initialplace, int type, int handleWall = 0, double length = 0.5)
        {
            if (initialplace)
            {
                Runner(false, X, Y, minStrength * 5.0, maxStrength * 5.0, 1, type, 0, 0, handleWall, length);
            }
            for (int truesloperight = 0; truesloperight < 350; truesloperight++)
            {
                int Xstray = (int)(0.0 - Math.Sin((double)truesloperight) * (double)Xmult);
                int level = (int)(0.0 - Math.Cos((double)truesloperight) * (double)Ymult);
                Runner(false, X + Xstray, Y + level, minStrength, maxStrength, 1, type, 0, 0, handleWall, length);
            }
        }
        public static void Runner(bool TileMode, int i, int j, double minStrength, double maxStrength, int steps, int type, int liquidType, byte liquid, int handleTile, double length)
        {
            double min = minStrength;
            double num = maxStrength;
            float num2 = (float)steps;
            Vector2 vector;
            vector.X = (float)i;
            vector.Y = (float)j;
            Vector2 vector2;
            vector2.X = (float)WorldGen.genRand.Next(-10, 11) * 0.1f;
            vector2.Y = (float)WorldGen.genRand.Next(-10, 11) * 0.1f;
            while (num > 0.0 && num2 > 0f)
            {
                min = minStrength * (double)(num2 / (float)steps);
                num = maxStrength * (double)(num2 / (float)steps);
                num2 -= 1f;
                int num3 = (int)((double)vector.X - num * 0.5);
                int num4 = (int)((double)vector.X + num * 0.5);
                int num5 = (int)((double)vector.Y - num * 0.5);
                int num6 = (int)((double)vector.Y + num * 0.5);
                bool flag3 = num3 < 1;
                if (flag3)
                {
                    num3 = 1;
                }
                bool flag4 = num5 < 1;
                if (flag4)
                {
                    num5 = 1;
                }
                bool flag5 = num6 > Main.maxTilesY - 1;
                if (flag5)
                {
                    num6 = Main.maxTilesY - 1;
                }
                for (int k = num3; k < num4; k++)
                {
                    for (int l = num5; l < num6; l++)
                    {
                        double rand = (double)WorldGen.genRand.Next(-7, 8);
                        bool flag6 = (double)(Math.Abs((float)k - vector.X) + Math.Abs((float)l - vector.Y)) < maxStrength * length * (1.0 + rand * 0.015);
                        bool nohollow = (double)(Math.Abs((float)k - vector.X) + Math.Abs((float)l - vector.Y)) < minStrength * length * (1.0 + rand * 0.015);
                        if (flag6 && !nohollow)
                        {
                            bool flag10 = TileMode;
                            Tile tile = Main.tile[k, l];
                            if (flag10)
                            {
                                tile.type = (ushort)type;
                                goto IL_483;
                            }
                            else
                            {
                                tile.wall = (ushort)type;
                            }
                            IL_483:
                            if (handleTile == 1)
                            {
                                Main.tile[k, l].active(true);
                                Main.tile[k, l].liquid = 0;
                                Main.tile[k, l].lava(false);
                                Main.tile[k, l].honey(false);
                            }
                            else if (handleTile == 2)
                            {
                                Main.tile[k, l].active(false);
                                Main.tile[k, l].liquid = 0;
                                Main.tile[k, l].lava(false);
                                Main.tile[k, l].honey(false);
                            }
                            WorldGen.SlopeTile(k, l);
                            if (liquidType == 1)
                            {
                                Main.tile[k, l].liquid = 255;
                            }
                            if (liquid != 0)
                            {
                                Main.tile[k, l].liquid = liquid;
                            }
                            if (liquidType == 2)
                            {
                                Main.tile[k, l].lava(true);
                            }
                            if (liquidType == 3)
                            {
                                Main.tile[k, l].honey(true);
                            }
                        }
                    }
                }
            }
        }
    }
}