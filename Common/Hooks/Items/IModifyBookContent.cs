using Entrogic.Interfaces.UI.BookUI;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;

namespace Entrogic.Common.Hooks.Items
{
	public interface IModifyBookContent
	{
		void ModifyBookContent(Item item, Player player, ref List<BookContent> contents);
	}

	//TODO: This class will not be needed with C# 8.0 default interface implementations.
	public sealed class HookModifyBookContent : ILoadable
	{
		public delegate void Delegate(Item item, Player player, ref List<BookContent> contents);

		public static HookList<GlobalItem, Delegate> Hook { get; private set; } = new HookList<GlobalItem, Delegate>(
			//Method reference
			typeof(IModifyBookContent).GetMethod(nameof(IModifyBookContent.ModifyBookContent)),
			//Invocation
			e => (Item item, Player player, ref List<BookContent> contents) => {
				foreach (IModifyBookContent g in e.Enumerate(item)) {
					g.ModifyBookContent(item, player, ref contents);
				}
			}
		);

		public void Load(Mod mod) => ItemLoader.AddModHook(Hook);
		public void Unload() { }
	}
}
