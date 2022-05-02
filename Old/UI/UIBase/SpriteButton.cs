using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Entrogic.UI
{
    /// <summary>
    /// 一个图片按钮，可以被继承，可以被实例化，启用需要手动调用Start方法。
    /// </summary>
    public class SpriteButton : ButtonBase
    {
        /// <summary>
        /// 图片按钮的纹理，实例化时赋值，可以被赋值。
        /// </summary>
        public Texture2D buttonImage;
        /// <summary>
        /// 实例化一个图片按钮。
        /// </summary>
        /// <param name="buttonImage">图片按钮的纹理。</param>
        public SpriteButton ( Texture2D buttonImage )
        {
            this.buttonImage = buttonImage;
            uiColor = Color.White;
            uiWidth = buttonImage.Width;
            uiHeight = buttonImage.Height;
            uiSize = new Vector2( uiWidth , uiHeight );
            uiCenter = new Vector2( uiWidth / 2 , uiHeight / 2 );
        }
        public override void UIEvent ( )
        {
        }
        public override void Draw ( SpriteBatch spriteBatch )
        {
            spriteBatch.Draw( buttonImage , uiPosition , uiColor );
        }
        public override void ClickEvent ( )
        {
<<<<<<< HEAD
            Terraria.Audio.SoundEngine.PlaySound( SoundID.MenuTick );
=======
            Main.PlaySound( SoundID.MenuTick );
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
        }
    }
}
