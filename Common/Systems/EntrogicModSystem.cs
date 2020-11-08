using Entrogic.Items.Materials;
using Entrogic.UI.Books;
using Entrogic.UI.CardGame;
using Entrogic.UI.Cards;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Entrogic.Common
{
    public class EntrogicModSystem : ModSystem
    {
        public static EntrogicModSystem Instance;
        public EntrogicModSystem()
        {
            Instance = this;
        }
        private int MimicryFrameCounter
        {
            get => Entrogic.MimicryFrameCounter;
            set => Entrogic.MimicryFrameCounter = value;
        }
        internal BookUI BookUI { get; private set; }
        private UserInterface BookUIE;
        internal CardUI CardUI { get; private set; }
        private UserInterface CardUIE;
        internal CardInventoryUI CardInventoryUI { get; private set; }
        private UserInterface CardInventoryUIE;
        internal CardGameUI CardGameUI { get; private set; }
        private UserInterface CardGameUIE;
        public override void Load()
        {

            BookUI = new BookUI();
            BookUI.Activate();
            BookUIE = new UserInterface();
            BookUIE.SetState(BookUI);

            CardUI = new CardUI();
            CardUI.Activate();
            CardUIE = new UserInterface();
            CardUIE.SetState(CardUI);

            CardInventoryUI = new CardInventoryUI();
            CardInventoryUI.Activate();
            CardInventoryUIE = new UserInterface();
            CardInventoryUIE.SetState(CardInventoryUI);

            CardGameUI = new CardGameUI();
            CardGameUI.Activate();
            CardGameUIE = new UserInterface();
            CardGameUIE.SetState(CardGameUI);
        }
        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            //if (Main.gamePaused && Filters.Scene["OrangeScreen"].IsActive()) Filters.Scene["OrangeScreen"].Deactivate();
            //if (!Filters.Scene["OrangeScreen"].IsActive()/* && !Main.gamePaused*/) Filters.Scene.Activate("OrangeScreen", Vector2.Zero);
            //Filters.Scene.Activate("ReallyDark", Vector2.Zero);
            if (!Main.dedServ)
            {
                MimicryFrameCounter++;
                if (MimicryFrameCounter >= 13 * 5)
                {
                    MimicryFrameCounter = 5;
                }
                if (AEntrogicConfigClient.Instance.HookMouse)
                {
                    if (Entrogic.HookCursorHotKey.JustPressed)
                    {
                        Entrogic.Instance.HookCursor = ModHelper.MouseScreenPos;
                    }
                    const float scaleMax = 1.3f;
                    float scale = 1f + ((float)Entrogic.ModTimer % 100f / 99f * (scaleMax - 1f) * 2f);
                    if (scale >= scaleMax) // scale范围[1.0, scaleMax]
                    {
                        scale = scaleMax - (scale - scaleMax);
                    }
                    spriteBatch.Draw((Texture2D)Entrogic.ModTexturesTable["HookCursor"], Entrogic.Instance.HookCursor + new Vector2(-scale * 8f), null, Color.White, 0f, Vector2.Zero, new Vector2(scale), SpriteEffects.None, 0f);
                }
                if (Main.gameMenu)
                {
                    if (DateTime.Now.Month == 3 && DateTime.Now.Day == 20)
                    {
                        string text = "[c/ff0000:H][c/ff2b00:a][c/ff5600:p][c/ff8100:p][c/ffa800:y] [c/ffd700:B][c/ffef00:i][c/e8f300:r][c/a6d200:t][c/63b100:h][c/219000:d][c/009021:a][c/00b163:y][c/00d2a6:,] [c/00d2ff:-][c/0090ff:C][c/004dff:y][c/000bff:r][c/1b00e3:i][c/3d00c2:l][c/5e00a1:-][c/7f0080:!]";
                        string seeing = "Happy Birthday, -Cyril-!";
                        Vector2 pos = new Vector2(Main.screenWidth, 0f);
                        ChatManager.DrawColorCodedStringWithShadow(spriteBatch, (DynamicSpriteFont)FontAssets.MouseText, text, pos += new Vector2(-FontAssets.MouseText.MeasureString(seeing).X, 0f), Color.White, 0f, Vector2.Zero, new Vector2(1f));
                    }
                }
            }
            base.PostDrawInterface(spriteBatch);
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            EntrogicPlayer ePlayer = Main.LocalPlayer.GetModPlayer<EntrogicPlayer>();
            int MouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (MouseTextIndex != -1)
            {
                layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
                    "Entrogic: Book UI",
                    delegate
                    {
                        if (BookUI.IsActive)
                        {
                            BookUIE.Draw(Main.spriteBatch, new GameTime());
                        }
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
                //layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
                //    "Entrogic: Book Page UI",
                //    delegate
                //    {
                //        BookPageUIE.Draw(Main.spriteBatch, new GameTime());
                //        return true;
                //    },
                //    InterfaceScaleType.UI)
                //);
                layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
                    "Entrogic: Card UI",
                    delegate
                    {
                        if (CardUI.IsActive)
                        {
                            CardUIE.Draw(Main.spriteBatch, new GameTime());
                        }
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
                layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
                    "Entrogic: Card Game UI",
                    delegate
                    {
                        if (ePlayer.CardGameActive)
                        {
                            CardGameUIE.Draw(Main.spriteBatch, new GameTime());
                        }
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
            int InventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
            if (InventoryIndex != -1)
            {
                layers.Insert(InventoryIndex, new LegacyGameInterfaceLayer(
                    "Entrogic: Card Inventory UI",
                    delegate
                    {
                        if (CardInventoryUI.IsActive)
                        {
                            CardInventoryUIE.Draw(Main.spriteBatch, new GameTime());
                        }
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
        public override void UpdateUI(GameTime gameTime)
        {
            Player player = Main.LocalPlayer;
            EntrogicPlayer ePlayer = player.GetModPlayer<EntrogicPlayer>();
            if (BookUIE != null && BookUI.IsActive)
                BookUIE.Update(gameTime);
            //if (BookPageUIE != null && Main.LocalPlayer.GetModPlayer<EntrogicPlayer>().ActiveBook)
            //    BookPageUIE.Update(gameTime);
            if (CardUIE != null && CardUI.IsActive)
                CardUIE.Update(gameTime);
            if (CardInventoryUIE != null && CardInventoryUI.IsActive)
                CardInventoryUIE.Update(gameTime);
            if (CardGameUIE != null && ePlayer.CardGameActive)
                CardGameUIE.Update(gameTime);
            //if (Main.netMode != NetmodeID.MultiplayerClient)
            //{
            int texNum = MimicryFrameCounter / 5;
            TextureAssets.Item[ModContent.ItemType<拟态魔能>()] = Entrogic.ModTexturesTable[$"拟态魔能_{texNum}"];
            //}
        }
        public override void UpdateMusic(ref int music, ref MusicPriority priority)
        {
            Player player = Main.LocalPlayer;
            if (Main.myPlayer == -1 || Main.gameMenu || !player.active)
            {
                return;
            }
            //EntrogicPlayer modPlayer = EntrogicPlayer.ModPlayer(player);
            //bool magicStorm = EntrogicWorld.magicStorm;
            //if (magicStorm && player.ZoneOverworldHeight)
            //{
            //    music = Entrogic.Instance.GetMusic("Sounds/Music/MagicStorm");
            //    priority = MusicPriority.Environment;
            //}
            //if (modPlayer.CardGaming)
            //{
            //    music = GetSoundSlot(SoundType.Music, "Sounds/Music/Toby Fox - Rude Buster");
            //    priority = MusicPriority.BossHigh;
            //}
            base.UpdateMusic(ref music, ref priority);
        }
        public override void PreUpdateEntities()
        {
            base.PreUpdateEntities();
        }
        public override void MidUpdateTimeWorld()
        {
            Entrogic.ModTimer++;
            CardInventoryUI.IsActive = Main.playerInventory;
            base.MidUpdateTimeWorld();
        }
    }
}
