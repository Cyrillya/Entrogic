using Terraria.ID;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using System;
using Terraria.GameContent;

namespace Entrogic
{
    public static partial class ModHelper
    {
        /// <summary>
        /// 把对应模式需要的伤害写进去，自动生成，这个直接用就行不需要考虑普通大师专家的伤害倍数了
        /// </summary>
        /// <param name="NPC">发射射弹的NPC</param>
        /// <param name="normalDamage">普通模式伤害</param>
        /// <param name="expertDamage">专家模式伤害</param>
        /// <param name="masterDamage">大师模式伤害</param>
        /// <returns></returns>
        public static int GetAttackDamage_ForProjectiles_MultiLerp_Exactly(this NPC NPC, float normalDamage, float expertDamage, float masterDamage) {
            int damage = NPC.GetAttackDamage_ForProjectiles_MultiLerp(normalDamage, expertDamage, masterDamage);
            // 普通模式2倍伤害，专家模式4倍伤害，大师模式6倍伤害
            float divisor = Main.masterMode ? 6f : (Main.expertMode ? 4f : 2f);
            return (int)(damage / divisor);
        }

        // Simple，因为旧位置是通过计算估计，而且其他属性也没有同步
        // 经过3个小时终于写出来了
        /// <summary>
        /// 显示一个简单残影，计算性高，空间占用小
        /// </summary>
        /// <param name="NPC">需要应用残影的NPC</param>
        /// <param name="spriteBatch">Draw的前提</param>
        /// <param name="lightColor">环境光因素</param>
        /// <param name="length">残影数量，一般NPC建议别超过15</param>
        public static void SimpleDrawShadow(this NPC NPC, SpriteBatch spriteBatch, Vector2 screenPos, Color lightColor, int length) {
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
            Texture2D texture = (Texture2D)TextureAssets.Npc[NPC.type];
            SpriteEffects effects = (NPC.spriteDirection == -1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            int b = NPC.alpha;
            int n = length;
            int frameHeight = texture.Height / Main.npcFrameCount[NPC.type];
            Vector2 origin = new(texture.Width / 2, frameHeight / 2);
            if (NPC.ichor)
                lightColor = new Color(255, 255, 0, 255);
            for (int x = 1; x < n; x++) {
                int a = (int)(b / 2 * 0.95 / n * (n - x));
                float amu = (float)a / 255f; // 转换成百分比形式，直接乘就行
                Vector2 drawPos = (NPC.Center - NPC.velocity * x * 0.5f) - screenPos;
                spriteBatch.Draw(texture,
                    drawPos.Floor(), // 小数点数字无意义还会拖慢进程 
                    (Rectangle?)NPC.frame,
                    lightColor * amu,
                    NPC.rotation,
                    origin,
                    NPC.scale * (1f + 0.02f * x), // 因为好看
                    effects,
                    0f);
            }
            float alphamu = (float)NPC.alpha / 255f; // 转换成百分比形式，直接乘就行
            // 再Draw一遍原来的NPC（因为一般都是在PreDraw里return false）
            spriteBatch.Draw(texture,
                NPC.Center.Floor() - screenPos,
                (Rectangle?)NPC.frame,
                lightColor * alphamu,
                NPC.rotation,
                origin,
                NPC.scale,
                effects,
                0f);
        }
    }
}
