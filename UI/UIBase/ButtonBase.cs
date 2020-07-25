using Entrogic;

using Microsoft.Xna.Framework;
using Terraria;

namespace Entrogic.UI
{
    /// <summary>
    /// 一个按钮基类，不可以被实例化，只能被继承。
    /// </summary>
    public abstract class ButtonBase : UIBase
    {
        /// <summary>
        /// 按钮的大小，实例化时赋值，可以被赋值。
        /// </summary>
        public Vector2 uiSize;
        /// <summary>
        /// 按钮 本身 的中心坐标，实例化时赋默认值，可以被赋值。
        /// </summary>
        public Vector2 uiCenter;
        /// <summary>
        ///鼠标与按钮交互时是否可以使用物品，实例化时赋默认值，可以被赋值。
        /// </summary>
        public bool canUseItem;
        /// <summary>
        /// ButtonBase的构造函数。
        /// </summary>
        public ButtonBase()
        {
            uiSize = new Vector2( uiWidth , uiHeight );
            uiCenter = new Vector2( uiWidth / 2 , uiHeight / 2 );
            canUseItem = false;
        }
        /// <summary>
        /// 启用UI。
        /// </summary>
        public void Start ( )
        {
            PreEvent();
            UIEvent();
            Click += ClickEvent;
            RightClick += RightClickEvent;
            Pressed += PressedEvent;
            Released += ReleasedEvent;
            if(ModHelper.MouseInRectangle( new Rectangle( (int)uiPosition.X , (int)uiPosition.Y , (int)uiWidth , (int)uiHeight ) ))
            {
                if (!canUseItem)
                {
                    Main.LocalPlayer.mouseInterface = true;
                }
                if(Main.mouseLeft && Main.mouseLeftRelease)
                {
                    Click.Invoke();
                }
                if (Main.mouseRight && Main.mouseRightRelease)
                {
                    RightClick.Invoke();
                }
                else if(Main.mouseLeft)
                {
                    Pressed.Invoke();
                }
                else if(Main.mouseLeftRelease)
                {
                    Released.Invoke();
                }
            }
            Click -= ClickEvent;
            RightClick -= RightClickEvent;
            Pressed -= PressedEvent;
            Released -= ReleasedEvent;
            Draw( Main.spriteBatch );
            PostEvent();
        }
        /// <summary>
        /// 鼠标单击UI时触发的函数，顺序在UIEvent之后。
        /// </summary>
        public virtual void ClickEvent ( )
        {
        }
        /// <summary>
        /// 鼠标右键单击UI时触发的函数，顺序在ClickEvent之后。
        /// </summary>
        public virtual void RightClickEvent()
        {
        }
        /// <summary>
        /// 鼠标长按UI时触发的函数，顺序在RightClickEvent之后。
        /// </summary>
        public virtual void PressedEvent ( )
        {
        }
        /// <summary>
        /// 鼠标悬浮在UI上方时触发的函数，顺序在PressedEvent之后。
        /// </summary>
        public virtual void ReleasedEvent ( )
        {
        }
        /// <summary>
        /// 鼠标单击。
        /// </summary>
        public event MouseEvent Click;
        /// <summary>
        /// 鼠标右键。
        /// </summary>
        public event MouseEvent RightClick;
        /// <summary>
        /// 鼠标长按。
        /// </summary>
        public event MouseEvent Pressed;
        /// <summary>
        /// 鼠标悬浮。
        /// </summary>
        public event MouseEvent Released;
        /// <summary>
        /// [警告]不建议单独使用。
        /// </summary>
        public delegate void MouseEvent ( );
    }
}
