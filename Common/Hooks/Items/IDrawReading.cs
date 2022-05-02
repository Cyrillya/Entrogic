using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;

namespace Entrogic.Common.Hooks.Items
{
	public interface IDrawReading
	{
		bool DrawReading(SpriteBatch spriteBatch, Item item, Player player);
	}

	//TODO: This class will not be needed with C# 8.0 default interface implementations.
	public sealed class HookDrawReading : ILoadable
	{
		public delegate void Delegate(SpriteBatch spriteBatch, Item item, Player player);

		public static HookList<GlobalItem, Func<SpriteBatch, Item, Player, bool>> Hook { get; private set; } = new HookList<GlobalItem, Func<SpriteBatch, Item, Player, bool>>(
			//Method reference
			typeof(IDrawReading).GetMethod(nameof(IDrawReading.DrawReading)),
			//Invocation
			e => (SpriteBatch spriteBatch, Item item, Player player) => {
				bool result = false;

				foreach (IDrawReading g in e.Enumerate(item)) {
					if (g.DrawReading(spriteBatch, item, player))
						result = true;
					else return false;
				}

				return result;
			}
		);

		public void Load(Mod mod) => ItemLoader.AddModHook(Hook);
		public void Unload() { }
	}
}
