using Microsoft.Xna.Framework.Graphics;
using Terraria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using Terraria.GameContent;

namespace Entrogic.UI
{
    /// <summary>
    /// 一个物品框，可以被继承，可以被实例化，启用需要手动调用Start方法。
    /// </summary>
    public class InventoryBase : SpriteButton
    {
        /// <summary>
        /// 是否使用自动调整大小
        /// </summary>
        public bool useScaleChange;
        /// <summary>
        /// 物品框内物品。
        /// </summary>
        public Item inventoryItem;
        /// <summary>
        /// 物品框内物品位置的偏移
        /// </summary>
        public Vector2 itemOffset;
        /// <summary>
        /// 实例化一个物品框。
        /// </summary>
        /// <param name="inventoryImage">物品框的贴图。</param>
        public InventoryBase ( Texture2D inventoryImage ) : base ( inventoryImage )
        {
            this.buttonImage = inventoryImage;
            inventoryItem = new Item();
        }
        public override void Draw ( SpriteBatch spriteBatch )
        {
            if (ModHelper.MouseInRectangle(uiRectangle) && inventoryItem.type != ItemID.None && inventoryItem != null)
            {
                Main.hoverItemName = inventoryItem.Name;
                Main.HoverItem = inventoryItem.Clone();
            }
            spriteBatch.Draw( buttonImage , uiPosition , uiColor );
            if (inventoryItem.type != ItemID.None && inventoryItem != null)
            {
                var frame = Main.itemAnimations[inventoryItem.type] != null ? Main.itemAnimations[inventoryItem.type].GetFrame((Texture2D)TextureAssets.Item[inventoryItem.type]) : ((Texture2D)TextureAssets.Item[inventoryItem.type]).Frame(1, 1, 0, 0);
                var size = frame.Size();
                float texScale = 1f;
                if ((size.X > uiRectangle.Width || size.Y > uiRectangle.Height) && useScaleChange)
                {
                    texScale = size.X > size.Y ? size.X / uiRectangle.Width : size.Y / uiRectangle.Height;
                    texScale = 0.8f / texScale;
                    size *= texScale;
                }
                spriteBatch.Draw((Texture2D)TextureAssets.Item[inventoryItem.type], uiPosition + uiCenter - size / 2 + itemOffset, new Rectangle?(frame), uiColor, 0, Vector2.Zero, texScale, 0, 0);
                if (inventoryItem.stack > 1)
                {
                    spriteBatch.DrawString((DynamicSpriteFont)FontAssets.MouseText, inventoryItem.stack.ToString(), new Vector2(uiRectangle.X + 10, uiRectangle.Y + uiRectangle.Height - 20), Color.White);
                }
            }
        }
        public override void ClickEvent ( )
        {
            if (inventoryItem.type == Main.mouseItem.type && inventoryItem.type != ItemID.None)
            {
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Grab);
                inventoryItem.stack += Main.mouseItem.stack;
                Main.mouseItem = new Item();
            }
            else if (Main.mouseItem.type == ItemID.None && inventoryItem.type != ItemID.None)
            {
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Grab);
                Main.mouseItem = inventoryItem;
                inventoryItem = new Item();
            }
            else if (Main.mouseItem.type != ItemID.None && inventoryItem.type == ItemID.None)
            {
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Grab);
                inventoryItem = Main.mouseItem;
                Main.mouseItem = new Item();
            }
            else if (Main.mouseItem.type != ItemID.None && inventoryItem.type != ItemID.None)
            {
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Grab);
                Item mouseItem = Main.mouseItem;
                Main.mouseItem = inventoryItem;
                inventoryItem = mouseItem;
            }
        }
        public override void RightClickEvent()
        {
            if ((Main.mouseItem.type == ItemID.None || Main.mouseItem.type == inventoryItem.type && Main.mouseItem.stack < Main.mouseItem.maxStack) && inventoryItem.type != ItemID.None)
            {
                Terraria.Audio.SoundEngine.PlaySound(SoundID.MenuTick);
                int stack = Main.mouseItem.stack;
                Main.mouseItem = inventoryItem.Clone();
                Main.mouseItem.stack = stack + 1;
                inventoryItem.stack--;
                if (inventoryItem.stack <= 0)
                {
                    inventoryItem = new Item();
                }
            }
            else if ((inventoryItem.type == ItemID.None || inventoryItem.type == Main.mouseItem.type && inventoryItem.stack < inventoryItem.maxStack) && Main.mouseItem.type != ItemID.None)
            {
                Terraria.Audio.SoundEngine.PlaySound(SoundID.MenuTick);
                int stack = inventoryItem.stack;
                inventoryItem = Main.mouseItem.Clone();
                inventoryItem.stack = stack + 1;
                Main.mouseItem.stack--;
                if (Main.mouseItem.stack <= 0)
                {
                    Main.mouseItem = new Item();
                }
            }
        }
        public override void ReleasedEvent ( )
        {

        }
    }
}
