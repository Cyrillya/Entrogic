//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Terraria;
//using Terraria.ModLoader;
//using Terraria.GameContent.UI.Elements;
//using Terraria.UI;
//using Terraria.ID;
//using static Terraria.ModLoader.ModContent;
//using ReLogic.Graphics;
//
//using Entrogic.Items.Weapons.Card;
//using System;
//using System.Collections.Generic;
//using Entrogic.Buffs.Miscellaneous;
//using Entrogic.Items.Books;

//namespace Entrogic.UI.Books
//{
//    public class BookPageUI : UIState
//    {
//        public UIPanel BookPage;//新建UI
//        UIBookPageButton NextPage = new UIBookPageButton(GetTexture("Entrogic/UI/Books/书本_02"));
//        UIBookPageButton PreviousPage = new UIBookPageButton(GetTexture("Entrogic/UI/Books/书本_03"));
//        Vector2 bookSize = new Vector2(600f, 480f);
//        public override void OnInitialize()
//        {
//            Vector2 bookPos = new Vector2((Main.screenWidth - bookSize.X) / 2, (Main.screenHeight - bookSize.Y) / 2);

//            NextPage.Width.Set(72, 0f);
//            NextPage.Height.Set(117, 0f);
//            NextPage.Left.Set(bookPos.X + bookSize.X - 72, 0f);//UI距离左边
//            NextPage.Top.Set(bookPos.Y + bookSize.Y - 117, 0f);//UI距离上面
//            NextPage.OnClick += new MouseEvent(NextPageClicked);
//            Append(NextPage);

//            PreviousPage.Width.Set(72, 0f);
//            PreviousPage.Height.Set(117, 0f);
//            PreviousPage.Left.Set(bookPos.X, 0f);//UI距离左边
//            PreviousPage.Top.Set(bookPos.Y + bookSize.Y - 117, 0f);//UI距离上面
//            PreviousPage.OnClick += new MouseEvent(PreviousPageClicked);
//            Append(PreviousPage);
//        }
//        private void NextPageClicked(UIMouseEvent evt, UIElement listeningElement)
//        {
//            EntrogicPlayer plr = Main.LocalPlayer.GetModPlayer<EntrogicPlayer>();
//            if (plr.PageNum >= plr.MaxPage)
//                return;
//            Terraria.Audio.SoundEngine.PlaySound(SoundID.MenuOpen);
//            plr.PageNum += 1;
//        }
//        private void PreviousPageClicked(UIMouseEvent evt, UIElement listeningElement)
//        {
//            EntrogicPlayer plr = Main.LocalPlayer.GetModPlayer<EntrogicPlayer>();
//            if (plr.PageNum <= 1)
//                return;
//            Terraria.Audio.SoundEngine.PlaySound(SoundID.MenuOpen);
//            plr.PageNum -= 1;
//        }
//        protected override void DrawSelf(SpriteBatch spriteBatch)
//        {
//            Player player = Main.LocalPlayer;
//            EntrogicPlayer ePlayer = player.GetModPlayer<EntrogicPlayer>();
//            if (ePlayer.PageNum == 1)
//                PreviousPage.drawButton = false;
//            else
//                PreviousPage.drawButton = true;

//            int MaxPage = 1;
//            if (ePlayer.GetHoldItem() != null)
//                if (ePlayer.GetHoldItem().GetGlobalItem<EntrogicItem>().book)
//                    MaxPage = ((ModBook)ePlayer.GetHoldItem().modItem).MaxPage;
//            if (ePlayer.PageNum == MaxPage)
//                NextPage.drawButton = false;
//            else
//                NextPage.drawButton = true;

//            base.DrawSelf(spriteBatch);
//        }
//    }
//    internal class UIBookPageButton : UIImage
//    {
//        internal bool hasTicked = false;
//        internal bool drawButton = true;
//        public UIBookPageButton(Texture2D texture) : base(texture)
//        {
//        }
//        protected override void DrawSelf(SpriteBatch spriteBatch)
//        {
//            Player player = Main.LocalPlayer;
//            EntrogicPlayer ePlayer = player.GetModPlayer<EntrogicPlayer>();

//            if (IsMouseHovering && drawButton)
//            {
//                Main.hoverItemName = "点击以翻页";
//                base.DrawSelf(spriteBatch);
//                if (!hasTicked)
//                {
//                    Terraria.Audio.SoundEngine.PlaySound(SoundID.MenuTick);
//                    hasTicked = true;
//                }
//            }
//            else hasTicked = false;
//        }
//        public override void Update(GameTime gameTime)
//        {
//            base.Update(gameTime); // don't remove.
//            Player player = Main.LocalPlayer;
//            EntrogicPlayer ePlayer = player.GetModPlayer<EntrogicPlayer>();
//            if (ContainsPoint(Main.MouseScreen) && ePlayer.ActiveBook)
//            {
//                Main.LocalPlayer.mouseInterface = true;
//            }
//        }
//        public override void Click(UIMouseEvent evt)
//        {
//            EntrogicPlayer plr = Main.LocalPlayer.GetModPlayer<EntrogicPlayer>();
//            if (plr.PageNum <= 1)
//                return;
//            Terraria.Audio.SoundEngine.PlaySound(SoundID.MenuOpen);
//            plr.PageNum -= 1;
//        }
//        public override void MiddleClick(UIMouseEvent evt)
//        {
//            EntrogicPlayer plr = Main.LocalPlayer.GetModPlayer<EntrogicPlayer>();
//            if (plr.PageNum >= plr.MaxPage)
//                return;
//            Terraria.Audio.SoundEngine.PlaySound(SoundID.MenuOpen);
//            plr.PageNum += 1;
//        }
//    }
//}
