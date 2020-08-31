using Entrogic.Items.Weapons.Card;
using Entrogic.NPCs.CardFightable;
using Entrogic.NPCs.CardFightable.Particles;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ReLogic.Graphics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace Entrogic.UI.CardGame
{
    public class HandCardSlot : InventoryBase
    {
        // 当前slot指向的ModPlayer中CardGameType的索引
        internal int Index;
        internal bool PlayerTurn;
        internal bool Clicked;
        private Vector2 ScreenCenter => new Vector2(Main.screenWidth, Main.screenHeight) / 2f;
        private Vector2 PanelSize = new Vector2(574f, 436f);
        private Vector2 PanelPos => new Vector2(ScreenCenter.X - PanelSize.X / 2, ScreenCenter.Y - PanelSize.Y / 2);

        private bool StartAnimation;
        private int AnimationTimer;
        private Vector2 AnimationPosition;
        public HandCardSlot(int index) : base(Main.magicPixel) // 无贴图
        {
            Index = index;
        }
        public void Update()
        {
            if (Main.dedServ) return;
            Player clientPlayer = Main.LocalPlayer;
            EntrogicPlayer clientModPlayer = EntrogicPlayer.ModPlayer(clientPlayer);
            if (clientModPlayer.CardGameNPCIndex == -1)
                return;
            NPC npc = Main.npc[clientModPlayer.CardGameNPCIndex];
            CardFightableNPC fightNPC = (CardFightableNPC)npc.modNPC;

            if (StartAnimation)
            {
                Vector2 TargetPosition = new Vector2(274f, 252f);
                AnimationTimer++;
                if (AnimationTimer <= 15f)
                {
                    AnimationPosition = (TargetPosition - uiPosition) / 15f * AnimationTimer + uiPosition;
                }
                else if (AnimationTimer >= 25f)
                {
                    // 出击
                    if (inventoryItem.modItem is ModCard)
                    {
                        ModCard card = (ModCard)inventoryItem.modItem;
                        Vector2 drawPosition = new Vector2(274f, 314f) + new Vector2(17f, 28f); // 元件在UI中的位置
                        drawPosition = CardGameUI.ToPlaygroundPos(drawPosition);
                        card.CardGameAttack(clientPlayer, npc, drawPosition, new Vector2(292f, 64f), PanelPos);

                        clientModPlayer.CardGameType[Index] = 0;
                    }
                    StartAnimation = false;
                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Player clientPlayer = Main.LocalPlayer;
            EntrogicPlayer clientModPlayer = EntrogicPlayer.ModPlayer(clientPlayer);
            bool Animationing = StartAnimation && AnimationTimer >= 1 ? true : false;
            uiWidth = 38;
            uiHeight = 50;
            float alpha = 0.36f;
            if (!Animationing && ModHelper.MouseInRectangle(uiRectangle) && inventoryItem.type != ItemID.None && inventoryItem != null)
            {
                if (PlayerTurn && clientModPlayer.CardGameLeftCard > 0)
                {
                    Main.hoverItemName = $"{inventoryItem.Name}\n点击以使用";
                    alpha = 1f;
                }
            }
            // 没有UI贴图
            //spriteBatch.Draw(ModContent.GetTexture("Entrogic/UI/Cards/HelpGrid"), finalUIPosition, uiColor);
            if (!Animationing && inventoryItem.type != ItemID.None && inventoryItem != null)
            {
                Texture2D t = Main.itemTexture[inventoryItem.type];
                var frame = Main.itemAnimations[inventoryItem.type] != null ? Main.itemAnimations[inventoryItem.type].GetFrame(Main.itemTexture[inventoryItem.type]) : Main.itemTexture[inventoryItem.type].Frame(1, 1, 0, 0);
                spriteBatch.Draw(t, finalUIPosition, (Rectangle?)frame, Color.White * alpha);
            }

            if (Animationing)
            {
                Texture2D t = Main.itemTexture[inventoryItem.type];
                var frame = Main.itemAnimations[inventoryItem.type] != null ? Main.itemAnimations[inventoryItem.type].GetFrame(Main.itemTexture[inventoryItem.type]) : Main.itemTexture[inventoryItem.type].Frame(1, 1, 0, 0);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);
                // Retrieve reference to shader
                var whiteBlur = GameShaders.Misc["Entrogic:WhiteBlur"];
                // Reset back to default value.
                whiteBlur.UseOpacity(0f);
                if (AnimationTimer > 15f)
                {
                    whiteBlur.UseOpacity(MathHelper.Min((AnimationTimer - 15f) / 8f, 1f));
                }
                whiteBlur.Apply(null);

                spriteBatch.Draw(t, AnimationPosition + fatherPosition, (Rectangle?)frame, Color.White);

                // As mentioned above, be sure not to forget this step.
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, null, null, null, Main.UIScaleMatrix);
            }
        }
        public override void ClickEvent()
        {
            if (Main.dedServ || StartAnimation) return;
            Player clientPlayer = Main.LocalPlayer;
            EntrogicPlayer clientModPlayer = EntrogicPlayer.ModPlayer(clientPlayer);
            if (clientModPlayer.CardGameNPCIndex == -1)
                return;
            NPC npc = Main.npc[clientModPlayer.CardGameNPCIndex];
            CardFightableNPC fightNPC = (CardFightableNPC)npc.modNPC;
            if (!PlayerTurn || clientModPlayer.CardGameLeftCard <= 0) return;
            clientModPlayer.CardGameLeftCard--;
            //Clicked = true;
            StartAnimation = true;
            AnimationTimer = 0;
        }
        public override void RightClickEvent()
        {
        }
    }
    public class NPCCardSlot : InventoryBase
    {
        private Vector2 ScreenCenter => new Vector2(Main.screenWidth, Main.screenHeight) / 2f;
        private Vector2 PanelSize = new Vector2(574f, 436f);
        private Vector2 PanelPos => new Vector2(ScreenCenter.X - PanelSize.X / 2, ScreenCenter.Y - PanelSize.Y / 2);

        private bool StartAnimation;
        private int AnimationTimer;
        private Vector2 AnimationPosition;
        public NPCCardSlot() : base(Main.magicPixel) // 无贴图
        {
        }
        public void Update()
        {
            if (Main.dedServ) return;
            Player clientPlayer = Main.LocalPlayer;
            EntrogicPlayer clientModPlayer = EntrogicPlayer.ModPlayer(clientPlayer);
            if (clientModPlayer.CardGameNPCIndex == -1)
                return;
            NPC npc = Main.npc[clientModPlayer.CardGameNPCIndex];

            if (StartAnimation)
            {
                Vector2 TargetPosition = new Vector2(274f, 90f);
                AnimationTimer++;
                if (AnimationTimer <= 15f)
                {
                    AnimationPosition = (TargetPosition - uiPosition) / 15f * AnimationTimer + uiPosition;
                }
                else if (AnimationTimer >= 30f)
                {
                    // 出击
                    // 我不想再改代码了，所以这里直接消失掉吧
                    StartAnimation = false;
                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Player clientPlayer = Main.LocalPlayer;
            EntrogicPlayer clientModPlayer = EntrogicPlayer.ModPlayer(clientPlayer);
            bool Animationing = StartAnimation && AnimationTimer >= 1 ? true : false;
            uiWidth = 38;
            uiHeight = 50;
            NPC npc = Main.npc[clientModPlayer.CardGameNPCIndex];
            CardFightableNPC fightNPC = (CardFightableNPC)npc.modNPC;
            if (Animationing)
            {
                Texture2D t = Main.itemTexture[inventoryItem.type];
                var frame = Main.itemAnimations[inventoryItem.type] != null ? Main.itemAnimations[inventoryItem.type].GetFrame(Main.itemTexture[inventoryItem.type]) : Main.itemTexture[inventoryItem.type].Frame(1, 1, 0, 0);

                Main.spriteBatch.SafeEnd();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);
                // Retrieve reference to shader
                var whiteBlur = GameShaders.Misc["Entrogic:WhiteBlur"];
                // Reset back to default value.
                whiteBlur.UseOpacity(0f);
                if (AnimationTimer > 15f)
                {
                    whiteBlur.UseOpacity(MathHelper.Min((AnimationTimer - 15f) / 14f, 1f));
                }
                whiteBlur.Apply(null);

                spriteBatch.Draw(t, AnimationPosition + fatherPosition, (Rectangle?)frame, Color.White);

                // As mentioned above, be sure not to forget this step.
                Main.spriteBatch.SafeEnd();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, null, null, null, Main.UIScaleMatrix);
            }
        }
        public void ActiveAnimation()
        {
            StartAnimation = true;
            AnimationTimer = 0;

            Player clientPlayer = Main.LocalPlayer;
            EntrogicPlayer clientModPlayer = EntrogicPlayer.ModPlayer(clientPlayer);
            NPC npc = Main.npc[clientModPlayer.CardGameNPCIndex];
            CardFightableNPC fightNPC = (CardFightableNPC)npc.modNPC;
            Item item = new Item();
            // 命名空间的第一个字符串即为Mod内部名（当然乱命名也没法了）
            Mod currentCardMod = ModLoader.GetMod(fightNPC.currentCard.GetType().FullName.Split('.')[0]);
            // 通过类名寻找相应物品
            item.SetDefaults(currentCardMod.GetItem(fightNPC.currentCard.GetType().Name).item.type);
            inventoryItem = item;
            //foreach (var card in Entrogic.ModItems)
            //{
            //    // 通过命名空间寻找相应卡牌
            //    if (card.GetType().FullName == fightNPC.currentCard.GetType().FullName)
            //    {
            //        Item item = new Item();
            //        item.SetDefaults(card.item.type);
            //        inventoryItem = item;
            //    }
            //}
        }
    }
}
