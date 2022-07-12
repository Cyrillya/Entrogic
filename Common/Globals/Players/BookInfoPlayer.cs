using Entrogic.Common.Hooks.Items;
using Entrogic.Content.Items.BaseTypes;
using Entrogic.Interfaces.GUI;

namespace Entrogic.Common.Globals.Players
{
    public class BookInfoPlayer : ModPlayer
    {
        public static readonly string WarnTexts = "请于物品栏关闭的情况下开启本书籍";
        internal int CurrentPage;
        internal string BookName;
        internal bool IsReading;

        public override bool CanUseItem(Item item) {
            if (item.ModItem != null && item.ModItem is ItemBook && Main.netMode != NetmodeID.Server && Player.whoAmI == Main.myPlayer) {
                // 不会在服务器运行，且只在当尝试开书的玩家是自己时才会运行（别人开书关我什么事）
                if (Main.playerInventory) {
                    // 不需要发给服务器端，其他玩家不会看到这个信息
                    CombatText.NewText(Player.getRect(), Color.Red, WarnTexts);
                    return false;
                }

                if (UIHandler.Instance.BookInterface?.CurrentState == null) UIHandler.Instance.ShowUI(UIHandler.UI.Book);
                else UIHandler.Instance.HideUI(UIHandler.UI.Book);
                IsReading = UIHandler.Instance.BookInterface?.CurrentState != null;
                ReloadBook(Player);
            }
            return base.CanUseItem(item);
        }

        public static void ReloadBook(Player player) {
            var book = player.GetModPlayer<BookInfoPlayer>();
            book.CurrentPage = 1;
            book.BookName = player.HeldItem.Name;

            BookUI.Texture = ResourceManager.BookPanel; // 重新赋值
            IModifyBookContent.Invoke(player.HeldItem, player, ref BookUI.Contents);
            IModifyBookPanel.Invoke(player.HeldItem, player, ref BookUI.Texture);
            BookUI.MaxPages = (player.HeldItem?.ModItem as ItemBook).PageMax;

            ModNetHandler.BookInfo.SendPage(-1, player.whoAmI, (byte)player.whoAmI, book.CurrentPage, book.IsReading);
            ModNetHandler.BookInfo.SendName(-1, player.whoAmI, (byte)player.whoAmI, book.BookName);
        }


        internal bool IsClosingBook;
        internal bool UseBookBubble;
        internal int BookBubbleFrameCounter;
        internal int BookBubbleFrame; // 范围：1-19
    }
}
