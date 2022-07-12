using Entrogic.Interfaces.UIElements;
using Terraria.ModLoader.Core;
using Hook = Entrogic.Common.Hooks.Items.IModifyBookContent;

namespace Entrogic.Common.Hooks.Items
{
    public interface IModifyBookContent
	{
		void ModifyBookContent(Item item, Player player, ref List<BookContent> contents);

		public static readonly HookList<GlobalItem> Hook = ItemLoader.AddModHook(new HookList<GlobalItem>(typeof(Hook).GetMethod(nameof(ModifyBookContent))));

		public static void Invoke(Item item, Player player, ref List<BookContent> contents) {
			if (item.ModItem is Hook) {
				(item.ModItem as Hook).ModifyBookContent(item, player, ref contents);
			}
			foreach (Hook g in Hook.Enumerate(item)) {
				g.ModifyBookContent(item, player, ref contents);
			}
		}
	}
}
