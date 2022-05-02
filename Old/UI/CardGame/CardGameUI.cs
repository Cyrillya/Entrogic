﻿using Entrogic.NPCs.CardFightable;
using Entrogic.NPCs.CardFightable.CardBullet;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

<<<<<<< HEAD
using ReLogic.Graphics;

=======
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
using System;
using System.Collections.Generic;

using Terraria;
<<<<<<< HEAD
using Terraria.Audio;
using Terraria.GameContent;
=======
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
using Terraria.GameContent.UI.Elements;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

using static Terraria.ModLoader.ModContent;

namespace Entrogic.UI.CardGame
{
    public class CardGameUI : UIState
    {
        public UIPanel CardGame;//新建UI
        internal Vector2 ScreenCenter => new Vector2(Main.screenWidth, Main.screenHeight) / 2f;

        internal UIImage GamePanel = new UIImage(GetTexture("Entrogic/UI/CardGame/CardGamePanel"));
        Vector2 PanelSize = new Vector2(574f, 436f);
        Vector2 PanelPos => new Vector2(ScreenCenter.X - PanelSize.X / 2, ScreenCenter.Y - PanelSize.Y / 2);
        Vector2 PanelCenter => PanelPos + PanelSize / 2f + new Vector2(10f / 2f, 0f); // 左边多出10px
<<<<<<< HEAD
        internal Confirm ConfirmButton = new Confirm();
        internal List<HandCardSlot> HandCardSlots = new List<HandCardSlot>();
        private NPCCardSlot NPCCardSlot = new NPCCardSlot()
        {
            uiPosition = new Vector2(274f, 8f)
        }; // 它只是为了一个动画
        protected static Vector2 PlaygroundPos => new Vector2(114f, 86f);
        protected Vector2 PlaygroundSize => new Vector2(358f, 220f);
        public bool PlayerTurnOver = false;
=======
        internal List<HandCardSlot> HandCardSlots = new List<HandCardSlot>();
        protected Vector2 PlaygroundPos => new Vector2(114f, 86f);
        protected Vector2 PlaygroundSize => new Vector2(358f, 220f);
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
        private int AnimationTimer;
        private int TimerCountdown = 60;
        private string TurnText = " ";
        private Color TurnTextColor = Color.White;
        private int Deathing; // 0:非死亡状态、1:玩家死、2:NPC死
        private int DeathTimer;
        public bool IsUseBiggerTexture;
        private float DelayHealthPercentPlayer;
        private float DelayHealthPercentNPC;
        private int LastRecordHealthPlayer;
        private int LastRecordHealthNPC;
        public override void OnInitialize()
        {
            GamePanel.Left.Set(PanelPos.X, 0f);//UI距离左边
            GamePanel.Top.Set(PanelPos.Y, 0f);//UI距离上面
            GamePanel.Width.Set(PanelSize.X, 0f);//UI的宽
            GamePanel.Height.Set(PanelSize.Y, 0f);//UI的高
            Append(GamePanel);

            Vector2 GridSize = new Vector2(38f, 50f); // 卡牌边框区域大小，目前似乎无用
            // 一般卡牌规格贴图，右边总有四个像素是费用贴图突出
            // 然而，这并不影响我们直接用uiPosition定位
            HandCardSlot slot1 = new HandCardSlot(0);
            slot1.fatherPosition = PanelPos;
            slot1.uiPosition.X = 150f;
            slot1.uiPosition.Y = 370f;
            HandCardSlot slot2 = new HandCardSlot(1);
            slot2.fatherPosition = PanelPos;
            slot2.uiPosition.X = 274f;
            slot2.uiPosition.Y = 370f;
            HandCardSlot slot3 = new HandCardSlot(2);
            slot3.fatherPosition = PanelPos;
            slot3.uiPosition.X = 398f;
            slot3.uiPosition.Y = 370f;
            HandCardSlots.Add(slot1);
            HandCardSlots.Add(slot2);
            HandCardSlots.Add(slot3);
<<<<<<< HEAD

            ConfirmButton.uiPosition.X = 500f;
            ConfirmButton.uiPosition.Y = 182f;
=======
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
        }
        public void StartGame()
        {
            Player clientPlayer = Main.LocalPlayer;
            EntrogicPlayer clientModPlayer = EntrogicPlayer.ModPlayer(clientPlayer);
            NPC npc = Main.npc[clientModPlayer.CardGameNPCIndex];
            CardFightableNPC fightNPC = (CardFightableNPC)npc.modNPC;

            clientModPlayer.CardGamePlayerHealth = clientModPlayer.CardGamePlayerMaxHealth;
            clientModPlayer.CardGameActive = true;
            clientModPlayer.CardGaming = true;
            fightNPC.CardGameHealth = fightNPC.CardGameHealthMax;
            fightNPC.CardGaming = true;
            Main.npcChatText = "";

<<<<<<< HEAD
            PlayerTurnOver = false;
            clientModPlayer.CardGamePlayerTurn = true;
            IsUseBiggerTexture = true;
            clientModPlayer._bullets.Clear();
            clientModPlayer._particles.Clear();
            clientModPlayer.CardGameLeftCard = 1;
            //List<int> buffer = new List<int>();
            //foreach (int card in clientModPlayer.CardType)
            //    buffer.Add(card);
            //for (int i = 0; i < clientModPlayer.CardGameType.Length; i++)
            //{
            //    int chosed = Main.rand.Next(0, buffer.Count);
            //    clientModPlayer.CardGameType[i] = buffer[chosed];
            //    buffer.RemoveAt(chosed);
            //}

            SoundEngine.PlaySound(ResourceLoader.CGChangeTurn);
=======
            Main.PlaySound(Entrogic.Instance.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/CGChangeTurn"));
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
            TimerCountdown = 0;
            AnimationTimer = 0;
            TurnText = Language.GetTextValue("Mods.Entrogic.Common.PlayerTurn");
        }
        public override void Update(GameTime gameTime)
        {
            if (Main.dedServ) return;
            Player clientPlayer = Main.LocalPlayer;
            EntrogicPlayer clientModPlayer = EntrogicPlayer.ModPlayer(clientPlayer);
            for (int i = 0; i < HandCardSlots.Count; i++)
            {
                Item slotitem = new Item();
                slotitem.SetDefaults(clientModPlayer.CardGameType[i]);
                HandCardSlots[i].inventoryItem = slotitem;
<<<<<<< HEAD
                HandCardSlots[i].Update();
            }
            NPCCardSlot.Update();
=======
            }
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96

            bool pause = (!Main.hasFocus || Main.gamePaused) && Main.netMode == NetmodeID.SinglePlayer;
            if (pause)
                return;
            if (clientModPlayer.CardGameNPCIndex == -1)
                return;
            //clientModPlayer._bullets = new List<CardFightBullet>() // 意思意思
            //{
            //    new MushroomBullet()
            //    {
            //        Position = new Vector2(100f, 100f)
            //    }
            //};
            NPC npc = Main.npc[clientModPlayer.CardGameNPCIndex];
            if (npc.type != NPCID.None && npc.modNPC is CardFightableNPC) // 表示此ModNPC属于CardFightableNPC
            {
                CardFightableNPC fightNPC = (CardFightableNPC)npc.modNPC;
                fightNPC.PanelPosition = PanelPos;
                if (clientModPlayer.CardGaming != fightNPC.CardGaming) QuitGame(); // 必须要同为一个状态的
                if (fightNPC.CardGaming && clientModPlayer.CardGaming)
                {
                    if (!clientPlayer.active || clientPlayer.dead || !npc.active)
                    {
                        QuitGame();
                        return;
                    }
                    clientPlayer.stoned = true;
                    clientPlayer.noKnockback = true;
                    npc.dontTakeDamageFromHostiles = true;
                    npc.dontTakeDamage = true;
                    #region NPC Stand
                    if (npc.ai[0] != 0f)
                    {
                        npc.netUpdate = true;
                    }
                    npc.ai[0] = 0f;
                    npc.ai[1] = 300f;
                    npc.localAI[3] = 100f;
                    if (Main.LocalPlayer.Center.X < npc.Center.X)
                    {
                        npc.direction = -1;
                    }
                    else
                    {
                        npc.direction = 1;
                    }
                    npc.spriteDirection = npc.direction;
                    #endregion
                    clientModPlayer.CardGameNPCHealth = fightNPC.CardGameHealth;
                    clientModPlayer.CardGameNPCMaxHealth = fightNPC.CardGameHealthMax;
                    if (Deathing != 0) return;
                    if (clientModPlayer.CardGamePlayerTurn) // 玩家局
                    {
<<<<<<< HEAD
                        foreach (HandCardSlot slot in HandCardSlots)
                        {
                            slot.PlayerTurn = true;
                        }
                        if (PlayerTurnOver) // 转NPC局
                        {
                            IsUseBiggerTexture = false;
                            clientModPlayer.CardGamePlayerTurn = false;
                            fightNPC.PreStartRound(false);
                            SoundEngine.PlaySound(ResourceLoader.CGChangeTurn);

                            TimerCountdown = 0;
                            AnimationTimer = 0;
                            TurnText = Language.GetTextValue("Mods.Entrogic.Common.NPCTurn");

                            CardGamePlayer player = new CardGamePlayer()
                            {
                                Center = PlaygroundSize / 2f,
                                UIPosition = PanelPos
                            };
                            clientModPlayer._bullets.Add(player);
=======
                        bool Clicked = false;
                        foreach (HandCardSlot slot in HandCardSlots)
                        {
                            slot.PlayerTurn = true;
                            if (slot.Clicked) // 转NPC局
                            {
                                IsUseBiggerTexture = false;
                                clientModPlayer.CardGamePlayerTurn = false;
                                fightNPC.PreStartRound(false);
                                slot.Clicked = false;
                                Clicked = true;
                                Main.PlaySound(Entrogic.Instance.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/CGChangeTurn"));

                                TimerCountdown = 0;
                                AnimationTimer = 0;
                                TurnText = Language.GetTextValue("Mods.Entrogic.Common.NPCTurn");

                                CardFightBullet player = new CardGamePlayer()
                                {
                                    Position = PlaygroundSize / 2f,
                                    UIPosition = PanelPos
                                };
                                clientModPlayer._bullets.Add(player);
                            }
                        }
                        if (Clicked)
                        {
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
                            for (int i = 0; i < HandCardSlots.Count; i++)
                            {
                                clientModPlayer.CardGameType[i] = ItemID.None;
                            }
<<<<<<< HEAD
                            NPCCardSlot.ActiveAnimation();
=======
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
                        }
                    }
                    if (!clientModPlayer.CardGamePlayerTurn) // NPC局
                    {
                        foreach (HandCardSlot slot in HandCardSlots)
                        {
                            slot.PlayerTurn = false;
                            slot.Clicked = false;
                        }
<<<<<<< HEAD
                        fightNPC.GameAI();
                        fightNPC.RoundDuration -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                        if (fightNPC.RoundDuration <= 0) // 转玩家局
                        {
                            PlayerTurnOver = false;
                            clientModPlayer.CardGamePlayerTurn = true;
                            fightNPC.PreStartRound(true);
                            IsUseBiggerTexture = true;
                            SoundEngine.PlaySound(ResourceLoader.CGChangeTurn);
                            for (int i = 0; i < clientModPlayer._bullets.Count; i++)
                            {
                                if (clientModPlayer._bullets[i].IsFriendly == false)
                                {
                                    clientModPlayer._bullets[i].Kill(clientPlayer);
                                    clientModPlayer._bullets.RemoveAt(i);
                                    i--;
                                }
                            }

                            clientModPlayer.CardGameLeftCard = 1; // 设置出牌机会
                            //List<int> buffer = new List<int>();
                            //foreach (int card in clientModPlayer.CardType)
                            //    buffer.Add(card);
                            //for (int i = 0; i < clientModPlayer.CardGameType.Length; i++)
                            //{
                            //    int chosed = Main.rand.Next(0, buffer.Count);
                            //    clientModPlayer.CardGameType[i] = buffer[chosed];
                            //    buffer.RemoveAt(chosed);
                            //}
=======
                        fightNPC.OnAttacking();
                        fightNPC.RoundDuration -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                        if (fightNPC.RoundDuration <= 0)
                        {
                            IsUseBiggerTexture = false;
                            clientModPlayer.CardGamePlayerTurn = true;
                            fightNPC.PreStartRound(true);
                            Main.PlaySound(Entrogic.Instance.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/CGChangeTurn"));
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96

                            TimerCountdown = 0;
                            AnimationTimer = 0;
                            TurnText = Language.GetTextValue("Mods.Entrogic.Common.PlayerTurn");
                        }
                    }
                    foreach (var bullet in clientModPlayer._bullets.ToArray())
                    {
                        bullet.Update(gameTime, clientPlayer, npc);
                    }
                    for (int i = 0; i < clientModPlayer._bullets.Count; i++)
                    {
                        if (clientModPlayer._bullets[i].IsRemoved)
                        {
<<<<<<< HEAD
                            clientModPlayer._bullets[i].Kill(clientPlayer);
=======
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
                            clientModPlayer._bullets.RemoveAt(i);
                            i--;
                        }
                    }
<<<<<<< HEAD
                    foreach (var particle in clientModPlayer._particles.ToArray())
                    {
                        particle.Update(gameTime);
                    }
                    for (int i = 0; i < clientModPlayer._particles.Count; i++)
                    {
                        if (clientModPlayer._particles[i].IsRemoved)
                        {
                            clientModPlayer._particles.RemoveAt(i);
                            i--;
                        }
                    }
=======
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
                    clientModPlayer.CardGamePlayerHealth = Math.Min(clientModPlayer.CardGamePlayerHealth, clientModPlayer.CardGamePlayerMaxHealth);
                    clientModPlayer.CardGameHealthAlpha = Math.Min(clientModPlayer.CardGameHealthAlpha + 0.02f, 1f);
                    clientModPlayer.CardGameNPCHealthAlpha = Math.Min(clientModPlayer.CardGameNPCHealthAlpha + 0.02f, 1f);
                    if (clientModPlayer.CardGamePlayerLastHealth != clientModPlayer.CardGamePlayerHealth)
                    {
                        clientModPlayer.CardGameHealthAlpha = -2; // 这样就不用再搞Delay变量了
                        DelayHealthPercentPlayer = 0f;
                        LastRecordHealthPlayer = clientModPlayer.CardGamePlayerLastHealth;
                    }
                    if (clientModPlayer.CardGameNPCLastHealth != clientModPlayer.CardGameNPCHealth)
                    {
                        clientModPlayer.CardGameNPCHealthAlpha = -2; // 这样就不用再搞Delay变量了
                        DelayHealthPercentNPC = 0f;
                        LastRecordHealthNPC = clientModPlayer.CardGameNPCLastHealth;
                    }
                    clientModPlayer.CardGamePlayerLastHealth = clientModPlayer.CardGamePlayerHealth;
                    clientModPlayer.CardGameNPCLastHealth = clientModPlayer.CardGameNPCHealth;
                    // NPC没血，NPC输玩家赢
                    if (clientModPlayer.CardGameNPCHealth <= 0)
                    {
                        fightNPC.CardGameLost();
                        PlayerWin();
                        clientModPlayer._bullets.Clear();
                        Deathing = 2; // NPC死

                        TimerCountdown = 0;
                        AnimationTimer = 0;
                        TurnTextColor = new Color(228, 196, 74);
                        TurnText = Language.GetTextValue("Mods.Entrogic.Common.YouWin");
                    }
                    // 玩家没血，NPC赢玩家输
                    if (clientModPlayer.CardGamePlayerHealth <= 0)
                    {
                        fightNPC.CardGameWin();
                        PlayerLost();
                        clientModPlayer._bullets.Clear();
                        Deathing = 1; // 玩家死

                        TimerCountdown = 0;
                        AnimationTimer = 0;
                        TurnTextColor = new Color(242, 12, 12);
                        TurnText = Language.GetTextValue("Mods.Entrogic.Common.YouLost");
                    }
                }
            }
            else
            {
                clientModPlayer.CardGameNPCIndex = -1;
            }

            base.Update(gameTime);
        }
        private void QuitGame()
        {
            if (Main.dedServ) return;
            Player clientPlayer = Main.LocalPlayer;
            EntrogicPlayer clientModPlayer = EntrogicPlayer.ModPlayer(clientPlayer);
            if (clientModPlayer.CardGameNPCIndex == -1)
                goto IL_BackToDefault;
            NPC npc = Main.npc[clientModPlayer.CardGameNPCIndex];
            if (npc.type != NPCID.None && npc.modNPC is CardFightableNPC) // 表示此ModNPC属于CardFightableNPC
            {
                CardFightableNPC fightNPC = (CardFightableNPC)npc.modNPC;

                fightNPC.CardGaming = false;
            }
            IL_BackToDefault:
            // 一切都要回到初始状态
            clientModPlayer.CardGaming = false;
            clientModPlayer.CardGameActive = false;
            clientModPlayer.CardGameNPCIndex = -1;
            clientModPlayer.CardGamePlayerTurn = true;
            Array.Clear(clientModPlayer.CardGameType, 0, clientModPlayer.CardGameType.Length);
            AnimationTimer = 0;
            TimerCountdown = 60;
            TurnTextColor = Color.White;
            TurnText = " ";
            Deathing = 0;
            DeathTimer = 0;
            IsUseBiggerTexture = false;
        }
        private void PlayerWin()
        {

        }
        private void PlayerLost()
        {

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Player clientPlayer = Main.LocalPlayer;
            EntrogicPlayer clientModPlayer = EntrogicPlayer.ModPlayer(clientPlayer);
            GamePanel.Left.Pixels = PanelPos.X;
            GamePanel.Top.Pixels = PanelPos.Y;
            // 原版很香
            base.Draw(spriteBatch);
<<<<<<< HEAD
            if (!IsUseBiggerTexture)
            {
                DrawBulletsParticles(spriteBatch);
            }

=======
            // Draw子弹
            foreach (var bullet in clientModPlayer._bullets.ToArray())
            {
                if (bullet.IsRemoved) continue;
                if (bullet.Position.X + bullet.Size.X < -108f || bullet.Position.Y + bullet.Size.Y < -62f ||
                    bullet.Position.X > 432f || bullet.Position.Y > 316f) continue;
                bullet.Draw(spriteBatch);
            }
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
            // 再Draw一个透明板子
            switch (IsUseBiggerTexture)
            {
                case false:
<<<<<<< HEAD
                    spriteBatch.Draw((Texture2D)GetTexture("Entrogic/UI/CardGame/CardGamePanel_Front"), PanelPos, Color.White);
                    break;

            }
            // Draw确认按钮
            ConfirmButton.fatherPosition = PanelPos;
            ConfirmButton.Start();
            // 对局魔力值显示
            if (clientModPlayer.CardGameLeftCard >= 1)
                spriteBatch.Draw((Texture2D)GetTexture("Entrogic/UI/CardGame/ManaCrystal"), PanelPos + new Vector2(424, 342), Color.White);
            if (clientModPlayer.CardGameLeftCard >= 2)
                spriteBatch.Draw((Texture2D)GetTexture("Entrogic/UI/CardGame/ManaCrystal"), PanelPos + new Vector2(456, 342), Color.White);
            if (clientModPlayer.CardGameLeftCard >= 3)
                spriteBatch.Draw((Texture2D)GetTexture("Entrogic/UI/CardGame/ManaCrystal"), PanelPos + new Vector2(488, 342), Color.White);

            if (clientModPlayer.CardGameNPCIndex == -1)
                goto IL_DRAWBIGTEXTUREPARTICLES;
=======
                    spriteBatch.Draw(GetTexture("Entrogic/UI/CardGame/CardGamePanel_Front"), PanelPos, Color.White);
                    break;
                case true:
                    spriteBatch.Draw(GetTexture("Entrogic/UI/CardGame/CardGamePanel_FrontBig"), PanelPos, Color.White);
                    break;

            }
            // 板子Draw完了再Draw槽，不然会被盖住
            foreach (var slot in HandCardSlots)
            {
                slot.fatherPosition = PanelPos;
                slot.Start();
            }

            if (clientModPlayer.CardGameNPCIndex == -1)
                goto IL_DRAWTIP;
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
            NPC npc = Main.npc[clientModPlayer.CardGameNPCIndex];
            CardFightableNPC fightNPC = (CardFightableNPC)npc.modNPC;
            string ImgPath = "Entrogic/Images/CardFightPlayer";
            Vector2 ImgPosition = new Vector2(274f, 12f);
            fightNPC.SetupContents(ref ImgPath, ref ImgPosition);
            // Deathing: 0.没失败者, 1.玩家失败, 2.NPC失败
            if (Deathing == 0)
            {
                // Draw玩家&NPC血条
                // 构成：第一层：FightPanel
                //       第二层：原贴图
                //       第三层：Surface
                //       第四层：Bar
                // 玩家：
<<<<<<< HEAD
                Texture2D FightPanel_Player = (Texture2D)Entrogic.ModTexturesTable["CardFightPlayer_FightPanel"];
                Texture2D Surface_Player = (Texture2D)Entrogic.ModTexturesTable["CardFightPlayer_Surface"];
                Texture2D Bar_Player = (Texture2D)Entrogic.ModTexturesTable["CardFightPlayer_Bar"];
                Texture2D Player = (Texture2D)Entrogic.ModTexturesTable["CardFightPlayer"];
=======
                Texture2D FightPanel_Player = Entrogic.ModTexturesTable["CardFightPlayer_FightPanel"];
                Texture2D Surface_Player = Entrogic.ModTexturesTable["CardFightPlayer_Surface"];
                Texture2D Bar_Player = Entrogic.ModTexturesTable["CardFightPlayer_Bar"];
                Texture2D Player = Entrogic.ModTexturesTable["CardFightPlayer"];
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
                Vector2 drawPosition = new Vector2(274f, 314f);
                spriteBatch.Draw(FightPanel_Player, PanelPos + drawPosition, Color.White * (1f - clientModPlayer.CardGameHealthAlpha));
                // 只有在血量变动的时候显示血量
                spriteBatch.Draw(Player, PanelPos + drawPosition, Color.White * clientModPlayer.CardGameHealthAlpha);
                // 计算出1所占的像素
                int maxhealth = Math.Max(1000, clientModPlayer.CardGamePlayerMaxHealth); // 防止除以0
                float tpercentone = (float)(Surface_Player.Height - 4) / (float)maxhealth; // -4去掉上下的光边面积
                // 这个值跑满了是(Surface_Player.Height - 4)
                int varhealth = LastRecordHealthPlayer - clientModPlayer.CardGamePlayerHealth;
                DelayHealthPercentPlayer = Math.Min(DelayHealthPercentPlayer + 0.05f, 1f);
                int tdrawHeight = (int)(tpercentone * (LastRecordHealthPlayer - (varhealth * DelayHealthPercentPlayer))); 
                // 所以这里-2，X值纠正到跑满了是2，刚好是丢掉光边的顶端X值。Height+1把光边补上来，1px玩家不会在意的
                Rectangle tPerRectangle = new Rectangle(0, Surface_Player.Height - 2 - tdrawHeight, Surface_Player.Width, tdrawHeight + 1);
                Vector2 drawPos = PanelPos + drawPosition;
                drawPos.Y += Surface_Player.Height - 2 - tdrawHeight;
                spriteBatch.Draw(Surface_Player, drawPos.NoShake(), (Rectangle?)tPerRectangle, Color.White * (1f - clientModPlayer.CardGameHealthAlpha));
                // 水面比水下多出2像素，所以水下的要-2
                tPerRectangle.Y += 2;
                tPerRectangle.Height -= 2;
                drawPos.Y += 2f;
                spriteBatch.Draw(Bar_Player, drawPos.NoShake(), (Rectangle?)tPerRectangle, Color.White * (1f - clientModPlayer.CardGameHealthAlpha));
<<<<<<< HEAD
                // 玩家鼠标移上去显示血量
                if (ModHelper.MouseInRectangle(new Rectangle((int)(PanelPos + drawPosition).X, (int)(PanelPos + drawPosition).Y, Player.Width, Player.Height)))
                    Main.hoverItemName = $"{Language.GetTextValue("Mods.Entrogic.Common.You")}: {clientModPlayer.CardGamePlayerHealth} / {clientModPlayer.CardGamePlayerMaxHealth}";
                // NPC：
                Texture2D FightPanel_NPC = (Texture2D)GetTexture($"{ImgPath}_FightPanel");
                Texture2D Surface_NPC = (Texture2D)GetTexture($"{ImgPath}_Surface");
                Texture2D Bar_NPC = (Texture2D)GetTexture($"{ImgPath}_Bar");
                Texture2D NPC = (Texture2D)GetTexture($"{ImgPath}_Fight");
=======
                // NPC：
                Texture2D FightPanel_NPC = GetTexture($"{ImgPath}_FightPanel");
                Texture2D Surface_NPC = GetTexture($"{ImgPath}_Surface");
                Texture2D Bar_NPC = GetTexture($"{ImgPath}_Bar");
                Texture2D NPC = GetTexture($"{ImgPath}_Fight");
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
                //Texture2D FightPanel_NPC = Entrogic.ModTexturesTable["CardFightPlayer_FightPanel"];
                //Texture2D Surface_NPC = Entrogic.ModTexturesTable["CardFightPlayer_Surface"];
                //Texture2D Bar_NPC = Entrogic.ModTexturesTable["CardFightPlayer_Bar"];
                //Texture2D NPC = Entrogic.ModTexturesTable["CardFightPlayer"];
                spriteBatch.Draw(FightPanel_NPC, PanelPos + ImgPosition, Color.White * (1f - clientModPlayer.CardGameNPCHealthAlpha));
                // 只有在血量变动的时候显示血量
                spriteBatch.Draw(NPC, PanelPos + ImgPosition, Color.White * clientModPlayer.CardGameNPCHealthAlpha);
                // 计算出1所占的像素
                maxhealth = Math.Max(1000, clientModPlayer.CardGameNPCMaxHealth); // 防止除以0
                tpercentone = (float)(Surface_NPC.Height - 4) / (float)maxhealth; // -4去掉上下的光边面积
                // 这个值跑满了是(Surface_NPC.Height - 4)
                varhealth = LastRecordHealthNPC - clientModPlayer.CardGameNPCHealth;
                DelayHealthPercentNPC = Math.Min(DelayHealthPercentNPC + 0.05f, 1f);
                tdrawHeight = (int)(tpercentone * (LastRecordHealthNPC - (varhealth * DelayHealthPercentNPC)));
                // 所以这里-2，X值纠正到跑满了是2，刚好是丢掉光边的顶端X值。Height+1把光边补上来，1px玩家不会在意的
                tPerRectangle = new Rectangle(0, Surface_NPC.Height - 2 - tdrawHeight, Surface_NPC.Width, tdrawHeight + 1);
                drawPos = PanelPos + ImgPosition;
                drawPos.Y += Surface_NPC.Height - 2 - tdrawHeight;
                spriteBatch.Draw(Surface_NPC, drawPos.NoShake(), (Rectangle?)tPerRectangle, Color.White * (1f - clientModPlayer.CardGameNPCHealthAlpha));
                // 水面比水下多出2像素，所以水下的要-2
                tPerRectangle.Y += 2;
                tPerRectangle.Height -= 2;
                drawPos.Y += 2f;
                spriteBatch.Draw(Bar_NPC, drawPos.NoShake(), (Rectangle?)tPerRectangle, Color.White * (1f - clientModPlayer.CardGameNPCHealthAlpha));
<<<<<<< HEAD
                // 玩家鼠标移上去显示血量
                if (ModHelper.MouseInRectangle(new Rectangle((int)(PanelPos + ImgPosition).X, (int)(PanelPos + ImgPosition).Y, Player.Width, Player.Height)))
                    Main.hoverItemName = $"{npc.GivenName}: {clientModPlayer.CardGameNPCHealth} / {clientModPlayer.CardGameNPCMaxHealth}";
                    //Main.hoverItemName = $"{Language.GetTextValue($"Mods.{fightNPC.mod.ToString().Split('.')[0]}.NPCName.{fightNPC.Name}")}: {clientModPlayer.CardGameNPCHealth} / {clientModPlayer.CardGameNPCMaxHealth}";
=======
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
            }
            else
            {
                DeathTimer++;
<<<<<<< HEAD
                Texture2D Tex = Deathing == 1 ? (Texture2D)Entrogic.ModTexturesTable["CardFightPlayer"] : (Texture2D)GetTexture($"{ImgPath}_Fight");
                Vector2 drawPosition = new Vector2(274f, 314f);
                if (DeathTimer <= 120f + 20f)
                {
                    Main.spriteBatch.SafeEnd();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);
=======
                Texture2D Tex = Deathing == 1 ? Entrogic.ModTexturesTable["CardFightPlayer"] : GetTexture($"{ImgPath}_Fight");
                Vector2 drawPosition = new Vector2(274f, 314f);
                if (DeathTimer <= 120f + 20f)
                {
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
                    // Retrieve reference to shader
                    var deathShader = GameShaders.Misc["ExampleMod:DeathAnimation"];
                    // Reset back to default value.
                    deathShader.UseOpacity(1f);
                    // We use AnimationTimer as a counter since the real death.
                    if (DeathTimer > 20f)
                    {
                        // Our shader uses the Opacity register to drive the effect. See ExampleEffectDeath.fx to see how the Opacity parameter factors into the shader math. 
                        deathShader.UseOpacity(1f - (DeathTimer - 20f) / 120f);
                        if (DeathTimer % 60f == 21f)
                        {
<<<<<<< HEAD
                            SoundEngine.PlaySound(SoundID.NPCDeath22, npc.Center); // every second while dying, play a sound
=======
                            Main.PlaySound(SoundID.NPCDeath22, npc.Center); // every second while dying, play a sound
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
                        }
                    }
                    // Call Apply to apply the shader to the SpriteBatch. Only 1 shader can be active at a time.
                    deathShader.Apply(null);

                    spriteBatch.Draw(Tex, PanelPos + (Deathing == 1 ? drawPosition : ImgPosition), Color.White);

                    // As mentioned above, be sure not to forget this step.
<<<<<<< HEAD
                    Main.spriteBatch.SafeEnd();
                    Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, null, null, null, Main.UIScaleMatrix);
                }
                Tex = Deathing == 2 ? (Texture2D)Entrogic.ModTexturesTable["CardFightPlayer"] : (Texture2D)GetTexture($"{ImgPath}_Fight");
=======
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
                }
                Tex = Deathing == 2 ? Entrogic.ModTexturesTable["CardFightPlayer"] : GetTexture($"{ImgPath}_Fight");
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
                spriteBatch.Draw(Tex, PanelPos + (Deathing == 2 ? drawPosition : ImgPosition), Color.White);
                if (DeathTimer >= 150f + 20f)
                {
                    QuitGame();
                }
            }
<<<<<<< HEAD
            IL_DRAWBIGTEXTUREPARTICLES:
            // Draw槽
            NPCCardSlot.fatherPosition = PanelPos;
            NPCCardSlot.Start();
            foreach (var slot in HandCardSlots)
            {
                slot.fatherPosition = PanelPos;
                slot.Start();
            }
            if (IsUseBiggerTexture)
            {
                DrawBulletsParticles(spriteBatch, true);
            }

            IL_DRAWTIP:
            Texture2D TipTexture = (Texture2D)GetTexture("Entrogic/UI/CardGame/CardGameTurnTip");
=======

            IL_DRAWTIP:
            Texture2D TipTexture = GetTexture("Entrogic/UI/CardGame/CardGameTurnTip");
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
            int max = 15;
            if (TimerCountdown < 60)
            {
                AnimationTimer = Math.Min(AnimationTimer + 1, max);
                if (AnimationTimer >= max) TimerCountdown++;
            }
            else
            {
                AnimationTimer--;
            }
            if (AnimationTimer <= 0) return;
            // 计算出1所占的像素
            float percentone = (float)TipTexture.Height / (float)max;
            int drawHeight = (int)(percentone * AnimationTimer);
            Rectangle PerRectangle = new Rectangle(0, TipTexture.Height - drawHeight, TipTexture.Width, drawHeight);
            Vector2 AniPos = new Vector2(PanelCenter.X - TipTexture.Width / 2f, PanelPos.Y); // Panel左边多出10px
            spriteBatch.Draw(TipTexture, AniPos.NoShake(), (Rectangle?)PerRectangle, Color.White);
            // 提示信息
            if (AnimationTimer != max) goto IL_DRAWTOPBAR;
<<<<<<< HEAD
            float hsize = ((DynamicSpriteFont)FontAssets.MouseText).MeasureString(TurnText).X / 2f;
            Vector2 textPos = PanelCenter;
            textPos.X -= hsize;
            textPos.Y = PanelPos.Y + 12f;
            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, (DynamicSpriteFont)FontAssets.MouseText, TurnText, textPos, TurnTextColor, 0f, Vector2.Zero, Vector2.One);
            // 再Draw顶端，看起来好看一点
            IL_DRAWTOPBAR:
            spriteBatch.Draw((Texture2D)GetTexture("Entrogic/UI/CardGame/CardGameTop"), PanelPos, Color.White);
        }
        private void DrawBulletsParticles(SpriteBatch spriteBatch, bool allowPanelEntity = false)
        {
            EntrogicPlayer clientModPlayer = EntrogicPlayer.ModPlayer(Main.LocalPlayer);
            // Draw子弹
            foreach (var bullet in clientModPlayer._bullets.ToArray())
            {
                if (bullet.IsRemoved) continue;
                if ((bullet.Position.X < -108f || bullet.Position.Y < -62f ||
                    bullet.Position.X + bullet.Size.X > 432f || bullet.Position.Y + bullet.Size.Y > 316f)
                    && (!allowPanelEntity || !bullet.IsPanelBullet)) continue;
                bullet.Draw(spriteBatch);
            }
            // Draw粒子
            foreach (var particle in clientModPlayer._particles.ToArray())
            {
                if (particle.IsRemoved) continue;
                if ((particle.Position.X < 0f || particle.Position.Y < 0f ||
                    particle.Position.X + particle.Size.X > PanelSize.X || particle.Position.Y + particle.Size.Y > PanelSize.Y)
                    && (!allowPanelEntity || !particle.IsPanelParticle)) continue;
                particle.Draw(spriteBatch);
            }
        }
        public static Vector2 ToPlaygroundPos(Vector2 PosUI)
        {
            PosUI = PosUI - PlaygroundPos;
            return PosUI;
        }
        public static Vector2 ToUIPos(Vector2 PosPlayground)
        {
            PosPlayground = PosPlayground + PlaygroundPos;
            return PosPlayground;
=======
            float hsize = Main.fontMouseText.MeasureString(TurnText).X / 2f;
            Vector2 textPos = PanelCenter;
            textPos.X -= hsize;
            textPos.Y = PanelPos.Y + 12f;
            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontMouseText, TurnText, textPos, TurnTextColor, 0f, Vector2.Zero, Vector2.One);
            // 再Draw顶端，看起来好看一点
            IL_DRAWTOPBAR:
            spriteBatch.Draw(GetTexture("Entrogic/UI/CardGame/CardGameTop"), PanelPos, Color.White);
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
        }
    }
}
