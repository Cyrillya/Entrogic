using Entrogic.NPCs.CardFightable;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ReLogic.Graphics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Entrogic.UI.CardGame
{
    public class HandCardSlot : InventoryBase
    {
        // 当前slot指向的ModPlayer中CardGameType的索引
        internal int Index;
        internal bool PlayerTurn;
        internal bool Clicked;
        public HandCardSlot(int index) : base(Main.magicPixel) // 无贴图
        {
            Index = index;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            uiWidth = 38;
            uiHeight = 50;
            float alpha = 0.36f;
            if (ModHelper.MouseInRectangle(uiRectangle) && inventoryItem.type != ItemID.None && inventoryItem != null)
            {
                Main.hoverItemName = $"{inventoryItem.Name}\n点击以使用";
                if (!PlayerTurn)
                    Main.hoverItemName = $"{inventoryItem.Name}(暂不可用)";
                alpha = 1f;
            }
            // 没有UI贴图
            //spriteBatch.Draw(ModContent.GetTexture("Entrogic/UI/Cards/HelpGrid"), finalUIPosition, uiColor);
            if (inventoryItem.type != ItemID.None && inventoryItem != null)
            {
                Texture2D t = Main.itemTexture[inventoryItem.type];
                var frame = Main.itemAnimations[inventoryItem.type] != null ? Main.itemAnimations[inventoryItem.type].GetFrame(Main.itemTexture[inventoryItem.type]) : Main.itemTexture[inventoryItem.type].Frame(1, 1, 0, 0);
                spriteBatch.Draw(t, finalUIPosition, (Rectangle?)frame, Color.White * alpha);
            }
        }
        public override void ClickEvent()
        {
            if (Main.dedServ) return;
            Player clientPlayer = Main.LocalPlayer;
            EntrogicPlayer clientModPlayer = EntrogicPlayer.ModPlayer(clientPlayer);
            if (clientModPlayer.CardGameNPCIndex == -1)
                return;
            NPC npc = Main.npc[clientModPlayer.CardGameNPCIndex];
            CardFightableNPC fightNPC = (CardFightableNPC)npc.modNPC;
            if (!PlayerTurn) return;
            clientModPlayer.CardGameType[Index] = 0;
            Clicked = true;

            // 伤害计算
            fightNPC.CardGameHealth -= 100;
        }
        public override void RightClickEvent()
        {
        }
    }
}
