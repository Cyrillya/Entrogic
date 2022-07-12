using Terraria.ModLoader.Core;
using Hook = Entrogic.Common.Hooks.Items.IModifyBookPanel;

namespace Entrogic.Common.Hooks.Items
{
    public interface IModifyBookPanel
	{
		void ModifyBookPanel(Item item, Player player, ref Asset<Texture2D> panelImage);

		public static readonly HookList<GlobalItem> Hook = ItemLoader.AddModHook(new HookList<GlobalItem>(typeof(Hook).GetMethod(nameof(ModifyBookPanel))));

		public static void Invoke(Item item, Player player, ref Asset<Texture2D> panelImage) {
			if (item.ModItem is Hook) {
				(item.ModItem as Hook).ModifyBookPanel(item, player, ref panelImage);
			}
			foreach (Hook g in Hook.Enumerate(item)) {
				g.ModifyBookPanel(item, player, ref panelImage);
			}
		}
	}
}
