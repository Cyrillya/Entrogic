using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.NPCs.Enemies
{
    public class Ooze : FSM_NPC
    {
        private int state;
        private int timer;
        public override void AI()
        {
            npc.TargetClosest(true);
            switch ((NPCState)state)
            {
                case NPCState.Move:
                    {
                        Player player = Main.player[npc.target];
                        npc.spriteDirection = -npc.direction;
                        npc.aiStyle = 3;
                        npc.TargetClosest(true);
                        if (npc.Distance(player.Center) <= npc.width)
                        {
                            SwitchState((int)NPCState.Attack);
                        }
                        break;
                    }
                case NPCState.Attack:
                    {
                        npc.aiStyle = -1;
                        timer++;
                        if (timer >= 18 && timer <= 28)
                        {
                            Entrogic.Explode(npc.Bottom + new Vector2(npc.width * npc.spriteDirection, 0f), npc.Size, npc.damage, false, 0, false);
                        }
                        if (timer >= 28)
                        {
                            SwitchState((int)NPCState.Move);
                        }
                        break;
                    }
                default:
                    break;
            }
        }
        private void FindFrame(int frameHeight, int frameWidth)
        {
            switch ((NPCState)state)
            {
                // 正常状态下
                case NPCState.Move:
                    {
                        npc.frame.X = 0;
                        npc.frameCounter++;
                        if (npc.frameCounter >= 4)
                        {
                            npc.frame.Y += frameHeight;
                            npc.frameCounter = 0;
                        }
                        if (npc.frame.Y >= frameHeight * 4)
                        {
                            npc.frame.Y = 0;
                        }
                        break;
                    }
                // 攻击状态下
                case NPCState.Attack:
                    {
                        npc.frame.X = frameWidth;
                        npc.frame.Y = timer / 4 * frameHeight;
                        break;
                    }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            //Main.npcTexture[npc.type] = Entrogic.Instance.GetTexture("NPCs/Enemies/EarthElemental_" + tex);
            Texture2D tex = Entrogic.Instance.GetTexture("NPCs/Enemies/Ooze");
            int npcFrameCountX = 2;
            FindFrame(tex.Height / Main.npcFrameCount[npc.type], tex.Width / npcFrameCountX);
            npc.frame.Width = tex.Width / npcFrameCountX;
            spriteBatch.Draw(tex, npc.position - Main.screenPosition + new Vector2(-npc.frame.Width / npcFrameCountX, -npc.height - 13), new Rectangle?(npc.frame), drawColor, npc.rotation, Vector2.Zero, 1f, npc.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0.0f);
            //SpriteEffects effects = SpriteEffects.None;
            //if (npc.spriteDirection < 0) effects = SpriteEffects.FlipHorizontally;
            //spriteBatch.Draw(tex, npc.position, npc.frame, npc.GetAlpha(drawColor), npc.rotation, Vector2.Zero, new Vector2(npc.scale), effects, 0f);
            return false;
        }
        protected override void SwitchState(int state)
        {
            Main.NewText($"{state}");
            npc.frameCounter = 0;
            timer = 0;
            this.state = state;
        }
        enum NPCState
        {
            Attack,
            Move
        }
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 7;
        }
        public override void SetDefaults()
        {
            npc.width = 86;
            npc.height = 46;
            npc.damage = 5;
            npc.defense = 6;
            npc.lifeMax = 62;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = Item.buyPrice(0, 0, 4, 0);
            npc.knockBackResist = 0.5f;
            npc.aiStyle = -1;
            npc.noTileCollide = false;
            //npc.scale = 2;
        }
    }
}
