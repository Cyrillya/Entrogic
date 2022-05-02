using Entrogic.Items.Weapons.Card;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

<<<<<<< HEAD
using ReLogic.Content;
=======
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
using ReLogic.Graphics;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.Utilities;

using static Entrogic.Entrogic;

using static Entrogic.Entrogic;

namespace Entrogic
{

    public static class ModHelper
    {
        #region Properties

        /// <summary>
        /// 鼠标碰撞箱
        /// </summary>
        public static Rectangle MouseHitBox = new Rectangle(Main.mouseX, Main.mouseY, 0, 0);

        public static bool ControlShift => Main.keyState.IsKeyDown(Keys.LeftShift) || Main.keyState.IsKeyDown(Keys.RightShift);

        public static Vector2 MousePos => Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);

        public static Vector2 MouseScreenPos => new Vector2(Main.mouseX, Main.mouseY);

        public static Player LocalPlayer
        {
            get
            {
                return Main.player[Main.myPlayer];
            }
        }
        /// <summary>
        /// 返回游戏窗口中心坐标。
        /// </summary>
        /// <returns></returns>
        public static Vector2 ScreenCenter => new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);

        /// <summary>
        /// 返回游戏窗口的中轴线的横坐标。
        /// </summary>
        /// <returns></returns>
        public static float ScreenVerticalAxis => Main.screenWidth / 2;
        #endregion

<<<<<<< HEAD
        public static void Return(Player player)
        {
            player.FindSpawn();
            if (!Player.CheckSpawn(player.SpawnX, player.SpawnY))
            {
                player.SpawnX = -1;
                player.SpawnY = -1;
            }
            if (player.SpawnX >= 0 && player.SpawnY >= 0)
            {
                Vector2 telePos = new Vector2(player.SpawnX * 16 + 8 - player.width / 2, player.SpawnY * 16 - player.height);
                player.HandleTeleport(telePos);
            }
            else
            {
                player.HandleTeleport(new Vector2(Main.spawnTileX * 16 + 8 - player.width / 2, Main.spawnTileY * 16 - player.height));
                for (int i = Main.spawnTileX - 1; i < Main.spawnTileX + 2; i++)
                {
                    for (int j = Main.spawnTileY - 3; j < Main.spawnTileY; j++)
                    {
                        if (Main.tile[i, j] != null)
                        {
                            if (Main.tileSolid[(int)Main.tile[i, j].type] && !Main.tileSolidTop[(int)Main.tile[i, j].type])
                            {
                                WorldGen.KillTile(i, j, false, false, false);
                            }
                            if (Main.tile[i, j].liquid > 0)
                            {
                                Main.tile[i, j].lava(false);
                                Main.tile[i, j].liquid = 0;
                                WorldGen.SquareTileFrame(i, j, true);
                            }
                        }
                    }
                }
            }
        }

        public static void HandleTeleport(this Player player, Vector2 telePos)
        {
            player.Teleport(telePos);
            if (Main.netMode != NetmodeID.SinglePlayer)
                NetMessage.SendData(MessageID.Teleport, -1, -1, null, 2, (float)player.whoAmI, telePos.X, telePos.Y, 0, 0, 0);
        }

        public static Vector2 MeasureString(this Asset<DynamicSpriteFont> fontAsset, string text)
        {
            return ((DynamicSpriteFont)fontAsset).MeasureString(text);
        }

=======
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
        /// <summary>
        /// 判断鼠标是否在某个矩形上。
        /// </summary>
        /// <param name="rectangle1">矩形</param>
        /// <returns></returns>
        public static bool MouseInRectangle(Rectangle rectangle1)
        {
            return rectangle1.Intersects(new Rectangle(Main.mouseX, Main.mouseY, 1, 1));
        }
        /// <summary>
        /// 判断鼠标是否在某个矩形上。
        /// </summary>
        /// <param name="X">矩形横坐标</param>
        /// <param name="Y">矩形纵坐标</param>
        /// <param name="width">矩形宽度</param>
        /// <param name="height">矩形高度</param>
        /// <returns></returns>
        public static bool MouseInRectangle(int X, int Y, int width, int height)
        {
            return new Rectangle(X, Y, width, height).Intersects(new Rectangle(Main.mouseX, Main.mouseY, 1, 1));
        }
        /// <summary>
        /// 判断鼠标是否在某个矩形上。
        /// </summary>
        /// <param name="X">矩形横坐标</param>
        /// <param name="Y">矩形纵坐标</param>
        /// <param name="width">矩形宽度</param>
        /// <param name="height">矩形高度</param>
        /// <param name="OFFXLeft">向左偏移长度。</param>
        /// <param name="OFFYTop">向上偏移长度。</param>
        /// <returns></returns>
        public static bool MouseInRectangle(int X, int Y, int width, int height, int OFFXLeft = 0, int OFFYTop = 0)
        {
            Vector2 mountedCenter = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
            return new Rectangle((int)mountedCenter.X, (int)mountedCenter.Y, 0, 0).Intersects(new Rectangle((int)(X + Main.screenPosition.X - OFFXLeft), (int)(Y + Main.screenPosition.Y - OFFYTop), width, height));
        }

        /// <summary>
        /// 鼠标与某矩形重合后绘制鼠标旁的悬浮字
        /// </summary>
        /// <param name="font">字体</param>
        /// <param name="text">文本</param>
        /// <param name="X">矩形横坐标</param>
        /// <param name="Y">矩形纵坐标</param>
        /// <param name="Width">矩形宽</param>
        /// <param name="Hegith">矩形高</param>
        public static void DrawMouseTextOnRectangle(DynamicSpriteFont font, string text, int X, int Y, int Width, int Hegith)
        {
            Vector2 mountedCenter = Main.MouseScreen;
            if (new Rectangle((int)mountedCenter.X, (int)mountedCenter.Y, 0, 0).Intersects(new Rectangle((int)X, (int)Y, Width, Hegith)))
            {
                string name = text;
                Vector2 worldPos = new Vector2(mountedCenter.X + 15, mountedCenter.Y + 15);
                Vector2 size = font.MeasureString(name);
                Vector2 texPos = worldPos + new Vector2(-size.X * 0.5f, name.Length);
                Main.spriteBatch.DrawString(font, name, new Vector2(texPos.X, texPos.Y), Color.White);
            }
        }
<<<<<<< HEAD

        public static void ProjectileExplode(this Projectile projectile, float statRangeX = 22f, float statRangeY = 22f)
        {
            SoundEngine.PlaySound(SoundID.Item14, projectile.position);
            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = (int)(22f * projectile.scale);
            projectile.height = (int)(22f * projectile.scale);
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
            int num3;
            for (int num765 = 0; num765 < 20; num765 = num3 + 1)
            {
                int num766 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, default(Color), 1.5f);
                Dust dust = Main.dust[num766];
                dust.velocity *= 1.4f;
                num3 = num765;
            }
            for (int num767 = 0; num767 < 10; num767 = num3 + 1)
            {
                int num768 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 2.5f);
                Main.dust[num768].noGravity = true;
                Dust dust = Main.dust[num768];
                dust.velocity *= 5f;
                num768 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 1.5f);
                dust = Main.dust[num768];
                dust.velocity *= 3f;
                num3 = num767;
            }
            int num769 = Gore.NewGore(new Vector2(projectile.position.X, projectile.position.Y), default(Vector2), Main.rand.Next(61, 64), 1f);
            Gore gore = Main.gore[num769];
            gore.velocity *= 0.4f;
            Gore gore138 = Main.gore[num769];
            gore138.velocity.X = gore138.velocity.X + 1f;
            Gore gore139 = Main.gore[num769];
            gore139.velocity.Y = gore139.velocity.Y + 1f;
            num769 = Gore.NewGore(new Vector2(projectile.position.X, projectile.position.Y), default(Vector2), Main.rand.Next(61, 64), 1f);
            gore = Main.gore[num769];
            gore.velocity *= 0.4f;
            Gore gore140 = Main.gore[num769];
            gore140.velocity.X = gore140.velocity.X - 1f;
            Gore gore141 = Main.gore[num769];
            gore141.velocity.Y = gore141.velocity.Y + 1f;
            num769 = Gore.NewGore(new Vector2(projectile.position.X, projectile.position.Y), default(Vector2), Main.rand.Next(61, 64), 1f);
            gore = Main.gore[num769];
            gore.velocity *= 0.4f;
            Gore gore142 = Main.gore[num769];
            gore142.velocity.X = gore142.velocity.X + 1f;
            Gore gore143 = Main.gore[num769];
            gore143.velocity.Y = gore143.velocity.Y - 1f;
            num769 = Gore.NewGore(new Vector2(projectile.position.X, projectile.position.Y), default(Vector2), Main.rand.Next(61, 64), 1f);
            gore = Main.gore[num769];
            gore.velocity *= 0.4f;
            Gore gore144 = Main.gore[num769];
            gore144.velocity.X = gore144.velocity.X - 1f;
            Gore gore145 = Main.gore[num769];
            gore145.velocity.Y = gore145.velocity.Y - 1f;
        }

        public static double GetLength(this Vector2 v)
        {
            return Math.Sqrt(Math.Pow(v.X, 2) + Math.Pow(v.Y, 2));
=======
        public static double GetLength(this Vector2 v)
        {
            return Math.Sqrt(Math.Pow(v.X,2) + Math.Pow(v.Y, 2));
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
        }
        public static Rectangle CreateFromVector2(Vector2 vec, float width, float height)
        {
            return new Rectangle((int)vec.X, (int)vec.Y, (int)width, (int)height);
        }
        public static Rectangle CreateFromVector2(Vector2 vec, Vector2 size)
        {
            return CreateFromVector2(vec, size.X, size.Y);
        }
        public static Vector2 GetFromToVector(Vector2 v1, Vector2 v2)
        {
            Vector2 diff = v2 - v1;
            diff.Normalize();
            return diff;
        }
        public static float GetFromToRadians(Vector2 v1, Vector2 v2)
        {
            Vector2 diff = v2 - v1;
            return diff.ToRotation();
        }
        /// <summary>
        /// 给可视化减震
        /// </summary>
        /// <param name="vec">被减震的向量</param>
        /// <returns></returns>
        public static Vector2 NoShake(this Vector2 vec)
        {
            return vec.ToPoint().ToVector2();
        }
        public static Vector2 ToTilePos(this Vector2 vec)
        {
            return new Vector2((int)(vec.X / 16), (int)(vec.Y / 16));
        }
        public static Vector2 ToScreenPos(this Vector2 vec)
        {
            return vec - Main.screenPosition;
        }
        public static Vector2 GetDrawPosition(Vector2 position, Vector2 origin, int width, int height, int texWidth, int texHeight, int framecount, float scale)
        {
            Vector2 screenPos = new Vector2(Main.screenPosition.X, Main.screenPosition.Y);
            return position - screenPos + new Vector2(width * 0.5f, height) - new Vector2(texWidth * scale / 2f, texHeight * scale / (float)framecount) + (origin * scale) + new Vector2(0f, 5f);
        }
        public static Vector2 GetDrawPosition(Vector2 position, Vector2 size)
        {
            return position - Main.screenPosition + size / 2;
        }
        public static Color GetLightColor(Vector2 position)
        {
            return Lighting.GetColor((int)(position.X / 16f), (int)(position.Y / 16f));
        }
        public static Vector2 GetShootVel(Player p)
        {
            Vector2 diff = p.Center - MousePos;
            diff.Normalize();
            return -diff;
        }
        public static Vector2 GetUnitVector(Vector2 e1, Vector2 e2)
        {
            Vector2 diff = e1 - e2;
            diff.Normalize();
            return -diff;
        }
        public static bool BuffExist(this Player p, int type)
        {
            return p.FindBuffIndex(type) > -1;
        }
        public static bool CanHit(Entity source, Entity target)
        {
            return Collision.CanHit(source.position, source.width, source.height, target.position, target.width, target.height);
        }
        public static void ShowHitBox(Entity ent, SpriteBatch sb)
        {
            sb.Draw((Texture2D)ModTexturesTable["Block"], new Rectangle((int)(ent.position.X - Main.screenPosition.X),
                (int)(ent.position.Y - Main.screenPosition.Y),
                ent.width,
                ent.height), Color.White);
        }
        public static ushort TileType(string str)
        {
            return Entrogic.ModTiles.Find(s => s.Name == str).Type;
        }
        public static int ProjectileType(string str)
        {
            return Entrogic.ModProjectiles.Find(s => s.Name == str).Type;
        }
        public static int ItemType(string str)
        {
            return Entrogic.ModItems.Find(s => s.Name == str).Type;
        }
        public static Player FindCloestPlayer(Vector2 position, float maxDistance, Func<Player, bool> predicate)
        {
            float maxDis = maxDistance;
            Player res = null;
            foreach (var player in Main.player.Where(pla => pla.active && !pla.dead && predicate(pla)))
            {
                float dis = Vector2.Distance(position, player.Center);
                if (dis < maxDis)
                {
                    maxDis = dis;
                    res = player;
                }
            }
            return res;
        }
        public static NPC FindCloestEnemy(Vector2 position, float maxDistance, Func<NPC, bool> predicate)
        {
            float maxDis = maxDistance;
            NPC res = null;
            foreach (var npc in Main.npc.Where(n => n.active && !n.friendly && predicate(n)))
            {
                float dis = Vector2.Distance(position, npc.Center);
                if (dis < maxDis)
                {
                    maxDis = dis;
                    res = npc;
                }
            }
            return res;
        }
        public static NPC FindCloestNoBossEnemy(Vector2 position, float maxDistance)
        {
            float maxDis = maxDistance;
            NPC res = null;
            foreach (var npc in Main.npc.Where(n => n.active && !n.friendly && !n.boss
            && !n.dontTakeDamage && n.type != NPCID.TargetDummy))
            {
                float dis = Vector2.Distance(position, npc.Center);
                if (dis < maxDis)
                {
                    maxDis = dis;
                    res = npc;
                }
            }
            return res;
        }
        public static void DefaultToWhip(this Projectile proj)
        {
            proj.width = 18;
            proj.height = 18;
            proj.aiStyle = 165;
            proj.friendly = true;
            proj.penetrate = -1;
            proj.tileCollide = false;
            proj.scale = 1f;
            proj.ownerHitCheck = true;
            proj.extraUpdates = 1;
            proj.usesLocalNPCImmunity = true;
            proj.localNPCHitCooldown = -1;
        }
        public static void SetShopValues(this Item item, ItemRarityColor rarity, int coinValue)
        {
            item.rare = (int)rarity;
            item.value = coinValue;
        }
        public static void DefaultToWhip(this Item item, int projectileId, int dmg, float kb, float shootspeed, int animationTotalTime = 30)
        {
            item.DamageType = DamageClass.Summon;
            item.autoReuse = false;
            item.useStyle = ItemUseStyleID.Swing;
            item.useAnimation = animationTotalTime;
            item.useTime = animationTotalTime;
            item.width = 18;
            item.height = 18;
            item.shoot = projectileId;
            item.UseSound = SoundID.Item152;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.damage = dmg;
            item.knockBack = kb;
            item.shootSpeed = shootspeed;
        }
        public static string GetItemName(int i)
        {
            Item item = new Item();
            item.SetDefaults(i);
            return item.Name;
        }
        public static void SpawnCoins(Vector2 pos, float value)
        {
            while (value >= 1000000)
            {
<<<<<<< HEAD
                value -= 1000000;
=======
                value -= 1000000; 
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
                int number = Item.NewItem(pos, ItemID.PlatinumCoin);
                if (Main.netMode == NetmodeID.MultiplayerClient && number >= 0)
                {
                    NetMessage.SendData(MessageID.SyncItem, -1, -1, null, number);
                }
            }
            while (value >= 10000)
            {
                value -= 10000;
                int number = Item.NewItem(pos, ItemID.GoldCoin);
                if (Main.netMode == NetmodeID.MultiplayerClient && number >= 0)
                {
                    NetMessage.SendData(MessageID.SyncItem, -1, -1, null, number);
                }
            }
            while (value >= 100)
            {
                value -= 100;
                int number = Item.NewItem(pos, ItemID.SilverCoin);
                if (Main.netMode == NetmodeID.MultiplayerClient && number >= 0)
                {
                    NetMessage.SendData(MessageID.SyncItem, -1, -1, null, number);
                }
            }
            while (value > 0)
            {
                value -= 1;
                int number = Item.NewItem(pos, ItemID.CopperCoin);
                if (Main.netMode == NetmodeID.MultiplayerClient && number >= 0)
                {
                    NetMessage.SendData(MessageID.SyncItem, -1, -1, null, number);
                }
            }
        }
        internal static int FindFirst(int[] array, int find, int startIndex = 0)
        {
            for (int i = startIndex; i < array.Length; i++)
            {
                if (array[i] == find)
                {
                    return i;
                }
            }
            return -1;
        }
        public static Item AddItem(this Chest chest, int type, int count, ref int index)
        {
            chest.item[index] = new Item();
            chest.item[index].SetDefaults(type);
            chest.item[index].stack = count;
            return chest.item[index++];
        }
        #region Vanilla Chest
        public static void AddNormalChestItem(this Chest chest, int Y, int index)
        {
            int num3 = Y;
            int m = index;
            while (m == index)
            {
                if ((double)num3 < Main.worldSurface + 25.0)
                {
                    if (WorldGen.genRand.Next(6) == 0)
                    {
                        chest.item[m].SetDefaults(3093, false);
                        chest.item[m].stack = 1;
                        if (WorldGen.genRand.Next(5) == 0)
                        {
                            chest.item[m].stack += WorldGen.genRand.Next(2);
                        }
                        if (WorldGen.genRand.Next(10) == 0)
                        {
                            chest.item[m].stack += WorldGen.genRand.Next(3);
                        }
                        m++;
                    }
                    if (WorldGen.genRand.Next(3) == 0)
                    {
                        chest.item[m].SetDefaults(168, false);
                        chest.item[m].stack = WorldGen.genRand.Next(3, 6);
                        m++;
                    }
                    if (WorldGen.genRand.Next(2) == 0)
                    {
                        int num6 = WorldGen.genRand.Next(2);
                        int type2 = WorldGen.genRand.Next(8) + 3;
                        if (num6 == 0)
                        {
                            chest.item[m].SetDefaults(WorldGen.copperBar, false);
                        }
                        if (num6 == 1)
                        {
                            chest.item[m].SetDefaults(WorldGen.ironBar, false);
                        }
                        chest.item[m].stack = type2;
                        m++;
                    }
                    if (WorldGen.genRand.Next(2) == 0)
                    {
                        int type3 = WorldGen.genRand.Next(50, 101);
                        chest.item[m].SetDefaults(965, false);
                        chest.item[m].stack = type3;
                        m++;
                    }
                    if (WorldGen.genRand.Next(3) != 0)
                    {
                        int num7 = WorldGen.genRand.Next(2);
                        int stack = WorldGen.genRand.Next(26) + 25;
                        if (num7 == 0)
                        {
                            chest.item[m].SetDefaults(40, false);
                        }
                        if (num7 == 1)
                        {
                            chest.item[m].SetDefaults(42, false);
                        }
                        chest.item[m].stack = stack;
                        m++;
                    }
                    if (WorldGen.genRand.Next(2) == 0)
                    {
                        bool flag2 = WorldGen.genRand.Next(1) != 0;
                        int stack2 = WorldGen.genRand.Next(3) + 3;
                        if (!flag2)
                        {
                            chest.item[m].SetDefaults(28, false);
                        }
                        chest.item[m].stack = stack2;
                        m++;
                    }
                    if (WorldGen.genRand.Next(3) != 0)
                    {
                        chest.item[m].SetDefaults(2350, false);
                        chest.item[m].stack = WorldGen.genRand.Next(2, 5);
                        m++;
                    }
                    if (WorldGen.genRand.Next(3) > 0)
                    {
                        int num8 = WorldGen.genRand.Next(6);
                        int stack3 = WorldGen.genRand.Next(1, 3);
                        if (num8 == 0)
                        {
                            chest.item[m].SetDefaults(292, false);
                        }
                        if (num8 == 1)
                        {
                            chest.item[m].SetDefaults(298, false);
                        }
                        if (num8 == 2)
                        {
                            chest.item[m].SetDefaults(299, false);
                        }
                        if (num8 == 3)
                        {
                            chest.item[m].SetDefaults(290, false);
                        }
                        if (num8 == 4)
                        {
                            chest.item[m].SetDefaults(2322, false);
                        }
                        if (num8 == 5)
                        {
                            chest.item[m].SetDefaults(2325, false);
                        }
                        chest.item[m].stack = stack3;
                        m++;
                    }
                    if (WorldGen.genRand.Next(2) == 0)
                    {
                        int num9 = WorldGen.genRand.Next(2);
                        int stack4 = WorldGen.genRand.Next(11) + 10;
                        if (num9 == 0)
                        {
                            chest.item[m].SetDefaults(8, false);
                        }
                        if (num9 == 1)
                        {
                            chest.item[m].SetDefaults(31, false);
                        }
                        chest.item[m].stack = stack4;
                        m++;
                    }
                    if (WorldGen.genRand.Next(2) == 0)
                    {
                        chest.item[m].SetDefaults(72, false);
                        chest.item[m].stack = WorldGen.genRand.Next(10, 30);
                        m++;
                    }
                }
                else if ((double)num3 < Main.rockLayer)
                {
                    if (WorldGen.genRand.Next(3) == 0)
                    {
                        chest.item[m].SetDefaults(166, false);
                        chest.item[m].stack = WorldGen.genRand.Next(10, 20);
                        m++;
                    }
                    if (WorldGen.genRand.Next(5) == 0)
                    {
                        chest.item[m].SetDefaults(52, false);
                        m++;
                    }
                    if (WorldGen.genRand.Next(3) == 0)
                    {
                        int stack5 = WorldGen.genRand.Next(50, 101);
                        chest.item[m].SetDefaults(965, false);
                        chest.item[m].stack = stack5;
                        m++;
                    }
                    if (WorldGen.genRand.Next(2) == 0)
                    {
                        int num11 = WorldGen.genRand.Next(2);
                        int stack6 = WorldGen.genRand.Next(10) + 5;
                        if (num11 == 0)
                        {
                            chest.item[m].SetDefaults(WorldGen.ironBar, false);
                        }
                        if (num11 == 1)
                        {
                            chest.item[m].SetDefaults(WorldGen.silverBar, false);
                        }
                        chest.item[m].stack = stack6;
                        m++;
                    }
                    if (WorldGen.genRand.Next(2) == 0)
                    {
                        int num12 = WorldGen.genRand.Next(2);
                        int stack7 = WorldGen.genRand.Next(25) + 25;
                        if (num12 == 0)
                        {
                            chest.item[m].SetDefaults(40, false);
                        }
                        if (num12 == 1)
                        {
                            chest.item[m].SetDefaults(42, false);
                        }
                        chest.item[m].stack = stack7;
                        m++;
                    }
                    if (WorldGen.genRand.Next(2) == 0)
                    {
                        bool flag3 = WorldGen.genRand.Next(1) != 0;
                        int stack8 = WorldGen.genRand.Next(3) + 3;
                        if (!flag3)
                        {
                            chest.item[m].SetDefaults(28, false);
                        }
                        chest.item[m].stack = stack8;
                        m++;
                    }
                    if (WorldGen.genRand.Next(3) > 0)
                    {
                        int num13 = WorldGen.genRand.Next(9);
                        int stack9 = WorldGen.genRand.Next(1, 3);
                        if (num13 == 0)
                        {
                            chest.item[m].SetDefaults(289, false);
                        }
                        if (num13 == 1)
                        {
                            chest.item[m].SetDefaults(298, false);
                        }
                        if (num13 == 2)
                        {
                            chest.item[m].SetDefaults(299, false);
                        }
                        if (num13 == 3)
                        {
                            chest.item[m].SetDefaults(290, false);
                        }
                        if (num13 == 4)
                        {
                            chest.item[m].SetDefaults(303, false);
                        }
                        if (num13 == 5)
                        {
                            chest.item[m].SetDefaults(291, false);
                        }
                        if (num13 == 6)
                        {
                            chest.item[m].SetDefaults(304, false);
                        }
                        if (num13 == 7)
                        {
                            chest.item[m].SetDefaults(2322, false);
                        }
                        if (num13 == 8)
                        {
                            chest.item[m].SetDefaults(2329, false);
                        }
                        chest.item[m].stack = stack9;
                        m++;
                    }
                    if (WorldGen.genRand.Next(3) != 0)
                    {
                        int stack10 = WorldGen.genRand.Next(1, 3);
                        chest.item[m].SetDefaults(2350, false);
                        chest.item[m].stack = stack10;
                        m++;
                    }
                    if (WorldGen.genRand.Next(2) == 0)
                    {
                        int stack11 = WorldGen.genRand.Next(11) + 10;
                        chest.item[m].SetDefaults(8, false);
                        chest.item[m].stack = stack11;
                        m++;
                    }
                    if (WorldGen.genRand.Next(2) == 0)
                    {
                        chest.item[m].SetDefaults(72, false);
                        chest.item[m].stack = WorldGen.genRand.Next(50, 90);
                        m++;
                    }
                }
                else if (num3 < Main.maxTilesY - 250)
                {
                    if (WorldGen.genRand.Next(3) == 0)
                    {
                        chest.item[m].SetDefaults(167, false);
                        m++;
                    }
                    if (WorldGen.genRand.Next(4) == 0)
                    {
                        chest.item[m].SetDefaults(51, false);
                        chest.item[m].stack = WorldGen.genRand.Next(26) + 25;
                        m++;
                    }
                    if (WorldGen.genRand.Next(2) == 0)
                    {
                        int num15 = WorldGen.genRand.Next(2);
                        int stack12 = WorldGen.genRand.Next(8) + 3;
                        if (num15 == 0)
                        {
                            chest.item[m].SetDefaults(WorldGen.goldBar, false);
                        }
                        if (num15 == 1)
                        {
                            chest.item[m].SetDefaults(WorldGen.silverBar, false);
                        }
                        chest.item[m].stack = stack12;
                        m++;
                    }
                    if (WorldGen.genRand.Next(2) == 0)
                    {
                        int num16 = WorldGen.genRand.Next(2);
                        int stack13 = WorldGen.genRand.Next(26) + 25;
                        if (num16 == 0)
                        {
                            chest.item[m].SetDefaults(41, false);
                        }
                        if (num16 == 1)
                        {
                            chest.item[m].SetDefaults(279, false);
                        }
                        chest.item[m].stack = stack13;
                        m++;
                    }
                    if (WorldGen.genRand.Next(2) == 0)
                    {
                        bool flag4 = WorldGen.genRand.Next(1) != 0;
                        int stack14 = WorldGen.genRand.Next(3) + 3;
                        if (!flag4)
                        {
                            chest.item[m].SetDefaults(188, false);
                        }
                        chest.item[m].stack = stack14;
                        m++;
                    }
                    if (WorldGen.genRand.Next(3) > 0)
                    {
                        int num17 = WorldGen.genRand.Next(6);
                        int stack15 = WorldGen.genRand.Next(1, 3);
                        if (num17 == 0)
                        {
                            chest.item[m].SetDefaults(296, false);
                        }
                        if (num17 == 1)
                        {
                            chest.item[m].SetDefaults(295, false);
                        }
                        if (num17 == 2)
                        {
                            chest.item[m].SetDefaults(299, false);
                        }
                        if (num17 == 3)
                        {
                            chest.item[m].SetDefaults(302, false);
                        }
                        if (num17 == 4)
                        {
                            chest.item[m].SetDefaults(303, false);
                        }
                        if (num17 == 5)
                        {
                            chest.item[m].SetDefaults(305, false);
                        }
                        chest.item[m].stack = stack15;
                        m++;
                    }
                    if (WorldGen.genRand.Next(3) > 1)
                    {
                        int num18 = WorldGen.genRand.Next(7);
                        int stack16 = WorldGen.genRand.Next(1, 3);
                        if (num18 == 0)
                        {
                            chest.item[m].SetDefaults(301, false);
                        }
                        if (num18 == 1)
                        {
                            chest.item[m].SetDefaults(302, false);
                        }
                        if (num18 == 2)
                        {
                            chest.item[m].SetDefaults(297, false);
                        }
                        if (num18 == 3)
                        {
                            chest.item[m].SetDefaults(304, false);
                        }
                        if (num18 == 4)
                        {
                            chest.item[m].SetDefaults(2329, false);
                        }
                        if (num18 == 5)
                        {
                            chest.item[m].SetDefaults(2351, false);
                        }
                        if (num18 == 6)
                        {
                            chest.item[m].SetDefaults(2329, false);
                        }
                        chest.item[m].stack = stack16;
                        m++;
                    }
                    if (WorldGen.genRand.Next(2) == 0)
                    {
                        int stack17 = WorldGen.genRand.Next(1, 3);
                        chest.item[m].SetDefaults(2350, false);
                        chest.item[m].stack = stack17;
                        m++;
                    }
                    if (WorldGen.genRand.Next(2) == 0)
                    {
                        int num19 = WorldGen.genRand.Next(2);
                        int stack18 = WorldGen.genRand.Next(15) + 15;
                        if (num19 == 0)
                        {
                            chest.item[m].SetDefaults(8, false);
                        }
                        if (num19 == 1)
                        {
                            chest.item[m].SetDefaults(282, false);
                        }
                        chest.item[m].stack = stack18;
                        m++;
                    }
                    if (WorldGen.genRand.Next(2) == 0)
                    {
                        chest.item[m].SetDefaults(73, false);
                        chest.item[m].stack = WorldGen.genRand.Next(1, 3);
                        m++;
                    }
                }
                else
                {
                    if (WorldGen.genRand.Next(2) == 0)
                    {
                        int num21 = WorldGen.genRand.Next(2);
                        int stack19 = WorldGen.genRand.Next(15) + 15;
                        if (num21 == 0)
                        {
                            chest.item[m].SetDefaults(117, false);
                        }
                        if (num21 == 1)
                        {
                            chest.item[m].SetDefaults(WorldGen.goldBar, false);
                        }
                        chest.item[m].stack = stack19;
                        m++;
                    }
                    if (WorldGen.genRand.Next(2) == 0)
                    {
                        int num22 = WorldGen.genRand.Next(2);
                        int stack20 = WorldGen.genRand.Next(25) + 50;
                        if (num22 == 0)
                        {
                            chest.item[m].SetDefaults(265, false);
                        }
                        if (num22 == 1)
                        {
                            chest.item[m].SetDefaults(278, false);
                        }
                        chest.item[m].stack = stack20;
                        m++;
                    }
                    if (WorldGen.genRand.Next(2) == 0)
                    {
                        int num23 = WorldGen.genRand.Next(2);
                        int stack21 = WorldGen.genRand.Next(6) + 15;
                        if (num23 == 0)
                        {
                            chest.item[m].SetDefaults(226, false);
                        }
                        if (num23 == 1)
                        {
                            chest.item[m].SetDefaults(227, false);
                        }
                        chest.item[m].stack = stack21;
                        m++;
                    }
                    if (WorldGen.genRand.Next(4) > 0)
                    {
                        int num24 = WorldGen.genRand.Next(8);
                        int stack22 = WorldGen.genRand.Next(1, 3);
                        if (num24 == 0)
                        {
                            chest.item[m].SetDefaults(296, false);
                        }
                        if (num24 == 1)
                        {
                            chest.item[m].SetDefaults(295, false);
                        }
                        if (num24 == 2)
                        {
                            chest.item[m].SetDefaults(293, false);
                        }
                        if (num24 == 3)
                        {
                            chest.item[m].SetDefaults(288, false);
                        }
                        if (num24 == 4)
                        {
                            chest.item[m].SetDefaults(294, false);
                        }
                        if (num24 == 5)
                        {
                            chest.item[m].SetDefaults(297, false);
                        }
                        if (num24 == 6)
                        {
                            chest.item[m].SetDefaults(304, false);
                        }
                        if (num24 == 7)
                        {
                            chest.item[m].SetDefaults(2323, false);
                        }
                        chest.item[m].stack = stack22;
                        m++;
                    }
                    if (WorldGen.genRand.Next(3) > 0)
                    {
                        int num25 = WorldGen.genRand.Next(8);
                        int stack23 = WorldGen.genRand.Next(1, 3);
                        if (num25 == 0)
                        {
                            chest.item[m].SetDefaults(305, false);
                        }
                        if (num25 == 1)
                        {
                            chest.item[m].SetDefaults(301, false);
                        }
                        if (num25 == 2)
                        {
                            chest.item[m].SetDefaults(302, false);
                        }
                        if (num25 == 3)
                        {
                            chest.item[m].SetDefaults(288, false);
                        }
                        if (num25 == 4)
                        {
                            chest.item[m].SetDefaults(300, false);
                        }
                        if (num25 == 5)
                        {
                            chest.item[m].SetDefaults(2351, false);
                        }
                        if (num25 == 6)
                        {
                            chest.item[m].SetDefaults(2348, false);
                        }
                        if (num25 == 7)
                        {
                            chest.item[m].SetDefaults(2345, false);
                        }
                        chest.item[m].stack = stack23;
                        m++;
                    }
                    if (WorldGen.genRand.Next(3) == 0)
                    {
                        int stack24 = WorldGen.genRand.Next(1, 3);
                        chest.item[m].SetDefaults(2350, false);
                        chest.item[m].stack = stack24;
                        m++;
                    }
                    if (WorldGen.genRand.Next(2) == 0)
                    {
                        int num26 = WorldGen.genRand.Next(2);
                        int stack25 = WorldGen.genRand.Next(15) + 15;
                        if (num26 == 0)
                        {
                            chest.item[m].SetDefaults(8, false);
                        }
                        if (num26 == 1)
                        {
                            chest.item[m].SetDefaults(282, false);
                        }
                        chest.item[m].stack = stack25;
                        m++;
                    }
                    if (WorldGen.genRand.Next(2) == 0)
                    {
                        chest.item[m].SetDefaults(73, false);
                        chest.item[m].stack = WorldGen.genRand.Next(2, 5);
                        m++;
                    }
                }
            }
        }
        #endregion
        public static Item GetRandomCard(Player player, Terraria.Utilities.UnifiedRandom rand, int rare = -1, int series = -1, bool useCheckGet = true)
        {
            List<int> ableToGet = new List<int>();
            foreach (ModItem findItem in ModItems)
            {
                Item i = new Item();
                i.SetDefaults(findItem.item.type);
                if (i.GetGlobalItem<EntrogicItem>().card)
                {
                    bool canGetRare = rare == -1 ? true : ((ModCard)i.modItem).rare == rare;
                    bool canGetSeries = series == -1 ? true : ((ModCard)i.modItem).series == series;
                    if ((((ModCard)i.modItem).AbleToGetFromRandom(player) || !useCheckGet) && canGetRare && canGetSeries)
                    {
                        ableToGet.Add(findItem.item.type);
                    }
                }
            }
            if (ableToGet.Count > 0)
            {
                while (true)
                {
                    int getItem = ableToGet[rand.Next(ableToGet.Count)];
                    Item i = new Item();
                    i.SetDefaults(getItem);
                    if (rand.NextFloat() < ((ModCard)i.modItem).cardProb)
                    {
                        return i;
                    }
                }
            }
            return null;
        }
        public static string StringBool(this string str, string succ, string fail, bool reas)
        {
            if (!reas)
            {
                str += fail;
            }
            else
            {
                str += succ;
            }
            return str;
        }
        public static void FindAndReplaceWall(Rectangle searchRange, ushort wallType)
        {
            var r = searchRange;
            for (int i = 0; i < r.Width; i++)
                for (int j = 0; j < r.Height; j++)
                    if (Main.tile[r.X + i, r.Y + j].wall != 0)
                        Main.tile[r.X + i, r.Y + j].wall = wallType;
        }
        public static Chest FindAndCreateChest(Rectangle searchRange, int tileType)
        {
            var r = searchRange;
            for (int i = 0; i < r.Width; i++)
                for (int j = 0; j < r.Height; j++)
                    if (Main.tile[r.X + i, r.Y + j].type == tileType)
                        return Main.chest[Chest.CreateChest(r.X + i, r.Y + j)];
            return null;
        }
        public static Sign FindAndCreateSign(Rectangle searchRange, int tileType)
        {
            var r = searchRange;
            for (int i = 0; i < r.Width; i++)
                for (int j = 0; j < r.Height; j++)
                    if (Main.tile[r.X + i, r.Y + j].type == tileType)
                        return Main.sign[Sign.ReadSign(r.X + i, r.Y + j)];
            return null;
        }
<<<<<<< HEAD
        public static float GetLerpValue(float from, float to, float t, bool clamped = false)
        {
            if (clamped)
            {
                if (from < to)
                {
                    if (t < from)
                    {
                        return 0f;
                    }
                    if (t > to)
                    {
                        return 1f;
                    }
                }
                else
                {
                    if (t < to)
                    {
                        return 1f;
                    }
                    if (t > from)
                    {
                        return 0f;
                    }
                }
            }
            return (t - from) / (to - from);
        }

        public static Vector2 DirectionTo(this Vector2 Origin, Vector2 Target)
        {
            return Vector2.Normalize(Target - Origin);
        }

        // Token: 0x06000101 RID: 257 RVA: 0x000068ED File Offset: 0x00004AED
        public static Rectangle Rectangle(this Texture2D texture)
        {
            return new Rectangle(0, 0, texture.Width, texture.Height);
        }

        // Token: 0x06000102 RID: 258 RVA: 0x00006902 File Offset: 0x00004B02
        public static float Distance(this Vector2 Origin, Vector2 Target)
        {
            return Vector2.Distance(Origin, Target);
        }

=======
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
        public static void SafeBegin(this SpriteBatch sb)
        {
            try
            {
                sb.Begin();
            }
            catch (InvalidOperationException)
            {
            }
        }
        public static void SafeBegin(this SpriteBatch sb, SamplerState sampler, RasterizerState rasterizer)
        {
            try
            {
                sb.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, sampler, null, rasterizer);
            }
            catch (InvalidOperationException)
            {
            }
        }

        public static void SafeEnd(this SpriteBatch sb)
        {
            try
            {
                sb.End();
            }
            catch (InvalidOperationException)
            {
            }
        }

        public static string GetCardSlotInfo(Player player)
        {
            EntrogicPlayer entrogicPlayer = player.GetModPlayer<EntrogicPlayer>();
            Item item = new Item();
            string text = "";
            for (int i = 0; i < entrogicPlayer.CardType.Length; i++)
            {
                if (entrogicPlayer.CardType[i] == 0)
                {
                    text += "0\n";
                }
                else
                {
                    item.SetDefaults(entrogicPlayer.CardType[i]);
                    text += $"{item.modItem.Mod.Name}:{item.modItem.Name}\n";
                }
            }
            return text;
        }

        public static string GetPlayerPathFromName(string playerName, bool cloudSave, out string trueName)
        {
            char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
            string text = "";
            if (playerName == ".")
            {
                playerName = "_";
            }
            else if (playerName == "..")
            {
                playerName = "__";
            }
            foreach (char c in playerName)
            {
                char c2;
                if (invalidFileNameChars.Contains(c))
                {
                    c2 = '-';
                }
                else if (c == ' ')
                {
                    c2 = '_';
                }
                else
                {
                    c2 = c;
                }
                text += c2.ToString();
            }
            string text2 = cloudSave ? Main.CloudPlayerPath : Main.PlayerPath;
            if (FileUtilities.GetFullPath($"{text2}{Path.DirectorySeparatorChar}{text}.plr", cloudSave).StartsWith("\\\\.\\", StringComparison.Ordinal))
            {
                text += "_";
            }
            if (FileUtilities.Exists($"{text2}{Path.DirectorySeparatorChar}{text}.plr", cloudSave))
            {
                int num = 2;
                while (FileUtilities.Exists(string.Concat(new object[]
                {
                    text2,
                    Path.DirectorySeparatorChar.ToString(),
                    text,
                    num,
                    ".plr"
                }), cloudSave))
                {
                    num++;
                }
                text += num;
            }
            trueName = text;
            return $"{text2}{Path.DirectorySeparatorChar}{text}";
        }

        public static string GetWorldPathFromName(string worldName, bool cloudSave, out string trueName)
        {
            char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
            string text = "";
            foreach (char c in worldName)
            {
                char c2;
                if (invalidFileNameChars.Contains(c))
                {
                    c2 = '-';
                }
                else if (c == ' ')
                {
                    c2 = '_';
                }
                else
                {
                    c2 = c;
                }
                text += c2.ToString();
            }
            string text2 = cloudSave ? Main.CloudWorldPath : Main.WorldPath;
            if (FileUtilities.GetFullPath($"{text2}{Path.DirectorySeparatorChar}{text}.wld", cloudSave).StartsWith("\\\\.\\", StringComparison.Ordinal))
            {
                text += "_";
            }
            if (FileUtilities.Exists($"{text2}{Path.DirectorySeparatorChar}{text}.wld", cloudSave))
            {
                int num = 2;
                while (FileUtilities.Exists(string.Concat(new object[]
                {
                    text2,
                    Path.DirectorySeparatorChar.ToString(),
                    text,
                    num,
                    ".wld"
                }), cloudSave))
                {
                    num++;
                }
                text += num;
            }
            trueName = text;
            return $"{text2}{Path.DirectorySeparatorChar}{text}";
        }
        /// <summary>
        /// 无入侵
        /// </summary>
        public static bool NoInvasion(NPCSpawnInfo spawnInfo)
        {
            return !spawnInfo.invasion && ((!Main.pumpkinMoon && !Main.snowMoon) || spawnInfo.spawnTileY > Main.worldSurface || Main.dayTime) && (!Main.eclipse || spawnInfo.spawnTileY > Main.worldSurface || !Main.dayTime);
        }
        /// <summary>
        /// 草地环境/无环境
        /// </summary>
        public static bool NoBiome(NPCSpawnInfo spawnInfo)
        {
            Player player = spawnInfo.player;
            return !player.ZoneJungle && !player.ZoneDungeon && !player.ZoneCorrupt && !player.ZoneCrimson && !player.ZoneHallow && !player.ZoneSnow && !player.ZoneUndergroundDesert;
        }
        /// <summary>
        /// 不在太空, 陨石, 蜘蛛洞地形
        /// </summary>
        public static bool NoZoneAllowWater(NPCSpawnInfo spawnInfo)
        {
            return !spawnInfo.sky && !spawnInfo.player.ZoneMeteor && !spawnInfo.spiderCave;
        }
        /// <summary>
        /// 不在太空, 陨石, 蜘蛛洞地形, 且NPC将判定产生的物块不包含水
        /// </summary>
        public static bool NoZone(NPCSpawnInfo spawnInfo)
        {
            return NoZoneAllowWater(spawnInfo) && !spawnInfo.water;
        }
        /// <summary>
        /// 普通敌怪生成区域(无入侵+周围无城镇)
        /// </summary>
        public static bool NormalSpawn(NPCSpawnInfo spawnInfo)
        {
            return !spawnInfo.playerInTown && NoInvasion(spawnInfo);
        }
        /// <summary>
        /// 普通敌怪生成区域(草地环境+周围无城镇) +
        /// 不在太空, 陨石, 蜘蛛洞地形 +
        /// NPC将判定产生的物块不包含水
        /// </summary>
        public static bool NoZoneNormalSpawn(NPCSpawnInfo spawnInfo)
        {
            return NormalSpawn(spawnInfo) && NoZone(spawnInfo);
        }
        /// <summary>
        /// 普通敌怪生成区域(草地环境+周围无城镇) + 不在太空, 陨石, 蜘蛛洞地形
        /// </summary>
        public static bool NoZoneNormalSpawnAllowWater(NPCSpawnInfo spawnInfo)
        {
            return NormalSpawn(spawnInfo) && NoZoneAllowWater(spawnInfo);
        }
        /// <summary>
        /// 普通敌怪生成区域(草地环境+周围无城镇) + 不在太空, 陨石, 蜘蛛洞地形 + NPC将判定产生的物块不包含水 + 草地环境
        /// </summary>
        public static bool NoBiomeNormalSpawn(NPCSpawnInfo spawnInfo)
        {
            return NormalSpawn(spawnInfo) && NoBiome(spawnInfo) && NoZone(spawnInfo);
        }
    }
}
