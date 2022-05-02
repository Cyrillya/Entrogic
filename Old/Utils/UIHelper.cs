using Terraria.GameContent.UI.Elements;

using System;

namespace Entrogic
{
    public static class UIHelper
    {
        public static void FastUIImage(this UIImage uiImage, float x, float y, float width, float height)
        {
            uiImage.Left.Set(x, 0f);//UI距离左边
            uiImage.Top.Set(y, 0f);//UI距离上面
            uiImage.Width.Set(width, 0f);//UI的宽
            uiImage.Height.Set(height, 0f);//UI的高
        }
    }
}
