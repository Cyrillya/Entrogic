using Terraria.ID;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using System;
<<<<<<< HEAD
using Terraria.GameContent;
=======
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96

namespace Entrogic
{
    public static class NPCHelper
    {
        // Simple，因为旧位置是通过计算估计，而且其他属性也没有同步
        // 经过3个小时终于写出来了
        /// <summary>
        /// 显示一个简单残影，计算性高，空间占用小
        /// </summary>
        /// <param name="npc">需要应用残影的NPC</param>
        /// <param name="spriteBatch">Draw的前提</param>
        /// <param name="lightColor">环境光因素</param>
        /// <param name="length">残影数量，一般NPC建议别超过15</param>
        public static void SimpleDrawShadow(this NPC npc, SpriteBatch spriteBatch, Color lightColor, int length)
        {
            // 残影思路（透明度）：
            // 设残影总量为n，每个个体为a[]，本体为b
            // 遍历NPC所有的oldPos
            // 透明度：a[x]=b/2*0.95/n*(n-x)
            // 果然还是这种靠谱n=8,b=255则 a[1]:105
            //							   a[2]:90
            //							   a[3]:75
            //							   a[4]:60
            //							   a[5]:45
            //							   a[6]:30
            //							   a[7]:45
            // 渐进，差不多符合想要的效果，来吧试一试你
            // PS：255是不透明，0是完全透明
            // PSS：不要用TR自己的东西，自己自定义长度吧
            // PSSS：考虑环境光，我死了几亿次了
<<<<<<< HEAD
            Texture2D texture = (Texture2D)TextureAssets.Npc[npc.type];
=======
            Texture2D texture = Main.npcTexture[npc.type];
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
            SpriteEffects effects = (npc.spriteDirection == -1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            int b = npc.alpha;
            int n = length;
            int frameHeight = texture.Height / Main.npcFrameCount[npc.type];
            Vector2 origin = new Vector2(texture.Width / 2, frameHeight / 2);
            for (int x = 1; x < n; x++)
            {
                int a = (int)(b / 2 * 0.95 / n * (n - x));
                float amu = (float)a / 255f; // 转换成百分比形式，直接乘就行
                Vector2 drawPos = npc.Center - Main.screenPosition - npc.velocity * x * 0.5f;
                spriteBatch.Draw(texture,
                    drawPos.ToPoint().ToVector2(), // 小数点数字无意义还会拖慢进程 
                    (Rectangle?)npc.frame,
                    lightColor * amu, 
                    npc.rotation, 
                    origin,
                    npc.scale * (1f + 0.02f * x), // 因为好看
                    effects, 
                    0f);
            }
            float alphamu = (float)npc.alpha / 255f; // 转换成百分比形式，直接乘就行
            // 再Draw一遍原来的NPC（因为一般都是在PreDraw里return false）
            spriteBatch.Draw(texture,
                (npc.Center - Main.screenPosition).NoShake(),
                (Rectangle?)npc.frame,
                lightColor * alphamu,
                npc.rotation,
                origin,
                npc.scale,
                effects,
                0f);
        }

        /// <summary>
        /// NPC下平台函数
        /// </summary>
        /// <param name="npc">需要进行下平台操作的NPC</param>
        public static void TryDownstairs(this NPC npc)
        {
            if (npc.velocity.Y == 0f && Main.player[npc.target].Bottom.Y > npc.position.Y + npc.height)
            {
                int tileY = (int)((npc.Bottom.Y + npc.height + 8) / 16f) + 1; // 寻找脚底下的方块
                                                                              // 寻找一排方块
                for (int tileX = (int)((npc.position.X - 8) / 16f); tileX < (int)((npc.position.X + npc.width + 8) / 16f); tileX++)
                {
                    if (WorldGen.InWorld(tileX, tileY))
                    {
                        if (Main.tile[tileX, tileY] == null)
                        {
                            Main.tile[tileX, tileY] = new Tile();
                        }
                        if ((Main.tileSolid[Main.tile[tileX, tileY].type] == true || Main.tileSolidTop[Main.tile[tileX, tileY].type] == false) && Main.tile[tileX, tileY].active() == true)
                        {
                            return; // 如果脚下方块里有任意一个方块存在且不可穿过/上方不是实体，则下平台失败
                        }
                    }
                }
                npc.noTileCollide = true; // 开启物块穿透达到伪下平台
            }
        }
    }
}
