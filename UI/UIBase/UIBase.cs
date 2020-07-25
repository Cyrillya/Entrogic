using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Entrogic.UI
{
    /// <summary>
    /// 所有UI的基类，不可被实例化，只能被继承。
    /// </summary>
    public abstract class UIBase
    {
        /// <summary>
        /// UI的宽度。
        /// </summary>
        public float uiWidth;
        /// <summary>
        /// UI的高度。
        /// </summary>
        public float uiHeight;
        /// <summary>
        /// UI的背景颜色。
        /// </summary>
        public Color uiColor;
        /// <summary>
        /// UI的位置。
        /// </summary>
        public Vector2 uiPosition;
        /// <summary>
        /// UI相应的矩形。
        /// </summary>
        public Rectangle uiRectangle => new Rectangle((int)uiPosition.X, (int)uiPosition.Y, (int)uiWidth, (int)uiHeight);
        /// <summary>
        /// 在Start方法被调用后，最先被调用的函数。
        /// </summary>
        public virtual void PreEvent ( )
        {

        }
        /// <summary>
        /// 在Start方法被调用后，第二个被调用的函数。
        /// </summary>
        public virtual void UIEvent ( )
        {
        }
        /// <summary>
        /// 在Start方法被调用后，第三个被调用的函数，建议在此编写绘制相关。
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw ( SpriteBatch spriteBatch )
        {
        }
        /// <summary>
        /// 在Start方法被调用后，最后被调用的函数。
        /// </summary>
        public virtual void PostEvent ( )
        {
        }
    }
}
