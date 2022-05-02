using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;

namespace Entrogic.Common.Hooks.Items
{
	public interface IModifyBookPanel
	{
		void ModifyBookPanel(Item item, Player player, ref Asset<Texture2D> panelImage);
	}

	//TODO: This class will not be needed with C# 8.0 default interface implementations.
	public sealed class HookModifyBookPanel : ILoadable
	{
		public delegate void Delegate(Item item, Player player, ref Asset<Texture2D> panelImage);

		public static HookList<GlobalItem, Delegate> Hook { get; private set; } = new HookList<GlobalItem, Delegate>(
			//Method reference
			typeof(IModifyBookPanel).GetMethod(nameof(IModifyBookPanel.ModifyBookPanel)),
			//Invocation
			e => (Item item, Player player, ref Asset<Texture2D> panelImage) => {
				foreach (IModifyBookPanel g in e.Enumerate(item)) {
					g.ModifyBookPanel(item, player, ref panelImage);
				}
			}
		);

		public void Load(Mod mod) => ItemLoader.AddModHook(Hook);
		public void Unload() { }
	}
}
