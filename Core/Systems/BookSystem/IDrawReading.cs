using Terraria.ModLoader.Core;
using Hook = Entrogic.Core.Systems.BookSystem.IDrawReading;

namespace Entrogic.Core.Systems.BookSystem
{
    public interface IDrawReading
	{
		bool DrawReading(SpriteBatch spriteBatch, Item item, Player player);

		public static readonly HookList<GlobalItem> Hook = ItemLoader.AddModHook(new HookList<GlobalItem>(typeof(Hook).GetMethod(nameof(DrawReading))));

		public static bool Invoke(SpriteBatch spriteBatch, Item item, Player player) {
			bool result = true;

			if (item.ModItem is Hook) {
				result &= (item.ModItem as Hook).DrawReading(spriteBatch, item, player);
			}

			foreach (Hook g in Hook.Enumerate(item)) {
				result &= g.DrawReading(spriteBatch, item, player);
			}

			return result;
		}
	}
}
