using Microsoft.Xna.Framework;
using ReLogic.Content;
using ReLogic.Graphics;
using System;
using Terraria;
using Terraria.GameContent;

namespace Entrogic.Interfaces.UIElements
{
    public class BookContent
    {
        internal string Text;
        internal float Scale;
        internal int TextWarpWidth;
        internal short ExtraLineDistance;
        internal Vector2 Position;
        internal Color Color;
        internal Asset<DynamicSpriteFont> Font;

        internal static readonly Vector2 LeftPagePos = new(38, 46);
        internal static readonly Vector2 RightPagePos = new(338, 46);
        internal BookContent(string text, Vector2 pos, float scale = 1f, Color color = default, short extraLineDistance = 0, int textWarpWidth = 224, Asset<DynamicSpriteFont> font = null) {
            Text = text;
            Position = pos;
            Scale = scale;
            Color = color;
            ExtraLineDistance = extraLineDistance;
            TextWarpWidth = textWarpWidth;
            Font = font;
        }
    }
}
