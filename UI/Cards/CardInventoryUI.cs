using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;
using ReLogic.Graphics;
using Entrogic.UI;
using Entrogic.Items.Weapons.Card;
using System.Collections.Generic;

namespace Entrogic.UI.Cards
{
    public class CardInventoryUI : UIState
    {
        internal bool slotActive = false;
        internal static bool IsActive = false;//UI开启的bool
        internal Vector2 statPoint = new Vector2(40f, 280f);
        UIHoverImageButton InventoryHighlight = new UIHoverImageButton(GetTexture("Entrogic/UI/Cards/Inventory_Highlight"), "卡牌背包");
        UIHoverImageButton Inventory = new UIHoverImageButton(GetTexture("Entrogic/UI/Cards/Inventory"), "卡牌背包");
        public List<CardInventoryGridSlot> Grid = new List<CardInventoryGridSlot>();
        public override void OnInitialize()
        {
            Vector2 inventory = new Vector2(30f, 32f);
            Inventory.Left.Set(statPoint.X - inventory.X * 0.5f, 0f);
            Inventory.Top.Set(statPoint.Y - inventory.Y * 0.5f, 0f);
            Inventory.Width.Set(inventory.X, 0f);
            Inventory.Height.Set(inventory.Y, 0f);
            Inventory.SetVisibility(1f, 0.5f);
            Append(Inventory);
            InventoryHighlight.Left.Set(0f, 0f);
            InventoryHighlight.Top.Set(0f, 0f);
            InventoryHighlight.Width.Set(inventory.X, 0f);
            InventoryHighlight.Height.Set(inventory.Y, 0f);
            InventoryHighlight.SetVisibility(1f, 0f);
            InventoryHighlight.OnClick += new MouseEvent(InventoryClicked);
            //InventoryHighlight.OnRightClick += new MouseEvent(InventoryRightClicked);
            Inventory.Append(InventoryHighlight);

            Vector2 grid = new Vector2(64f, 64f);
            for (int i = 0; i < 9; i++)
            {
                float posX = statPoint.X + 32f + inventory.X + grid.X * (-0.5f + (int)(i % 3)) + (int)(i % 3) * 10f;
                float posY = statPoint.Y + 18f + grid.Y * (-0.5f + (int)(i / 3)) + (int)(i / 3) * 10f;
                CardInventoryGridSlot Gri = new CardInventoryGridSlot((Texture2D)GetTexture("Entrogic/UI/Cards/InventoryGrid"), i);
                Gri.uiPosition = new Vector2(posX, posY);
                Gri.uiWidth = Gri.uiHeight = 64f;
                Grid.Add(Gri);
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (slotActive && (Main.npcShop != 0 || Main.HoverItem.favorited || Main.LocalPlayer.chest != -1))
            {
                slotActive = false;
            }
            if (!slotActive)
                return;
            for (int i = 0; i < 9; i++)
            {
                if (Grid[i] == null)
                    continue;
                Grid[i].uiColor = new Color(215, 215, 215, 215);
                if (ModHelper.MouseInRectangle(Grid[i].uiRectangle))
                {
                    Grid[i].uiColor = Color.White;
                }
                Grid[i].Start();
            }
        }
        public void UpdateSlots(int index = -1)
        {
            if (index != -1)
            {
                Grid[index].inventoryItem.SetDefaults(EntrogicPlayer.ModPlayer(Main.LocalPlayer).CardType[index]);
            }
            for (int i = 0; i < Grid.Count; i++)
            {
                Grid[i].inventoryItem.SetDefaults(EntrogicPlayer.ModPlayer(Main.LocalPlayer).CardType[i]);
            }
        }
        public int HoverOnSlot()
        {
            foreach (CardInventoryGridSlot Gri in Grid)
            {
                if (ModHelper.MouseInRectangle(Gri.uiRectangle))
                {
                    return Grid.IndexOf(Gri);
                }
            }
            return -1;
        }
        private void InventoryClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            Main.LocalPlayer.chest = -1;
            Main.SetNPCShopIndex(0);
            Terraria.Audio.SoundEngine.PlaySound(SoundID.MenuOpen);
            slotActive = !slotActive;
        }
    }
    public class CardInventoryGridSlot : InventoryBase
    {
        internal int Index;
        internal Texture2D Texture;
        public CardInventoryGridSlot(Texture2D texture, int index) : base(texture)
        {
            Texture = texture;
            Index = index;
            itemOffset = new Vector2(2f, 0f);
        }
        public override void ClickEvent()
        {
            if (ModHelper.ControlShift && inventoryItem.type != ItemID.None)
            {
                if (!Main.LocalPlayer.ItemSpaceForCofveve(inventoryItem))
                    return;
                Main.LocalPlayer.PutItemInInventoryFromItemUsage(inventoryItem.type);
                inventoryItem = new Item();
                EntrogicPlayer.ModPlayer(Main.LocalPlayer).CardType[Index] = inventoryItem.type;
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Grab, -1, -1, 1, 1f, 0.0f);
                return;
            }
            RightClickEvent();
        }
        public override void RightClickEvent()
        {
            if (!AllowPutin(inventoryItem, Main.mouseItem, Index))
                return;
            base.RightClickEvent();
            if (Main.mouseItem.type != ItemID.None && inventoryItem.type != ItemID.None && Main.mouseItem.stack == 1)
            {
                Item mouItem = Main.mouseItem;
                Main.mouseItem = inventoryItem;
                inventoryItem = mouItem;
            }
            if (Main.mouseLeft && Main.mouseLeftRelease && EntrogicPlayer.ModPlayer(Main.LocalPlayer).CardType[Index] != inventoryItem.type)
            {
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Grab, -1, -1, 1, 1f, 0.0f);
            }
            EntrogicPlayer.ModPlayer(Main.LocalPlayer).CardType[Index] = inventoryItem.type;
        }
        internal static bool AllowPutin(Item inventoryItem, Item itemToPut, int Index)
        {
            EntrogicPlayer player = Main.LocalPlayer.GetModPlayer<EntrogicPlayer>();
            if (itemToPut.type != ItemID.None)
            {
                if (!itemToPut.GetGlobalItem<EntrogicItem>().card)
                    return false;
            } // 特殊判断：如果手上物品不是卡牌就return
            if (itemToPut.type == ItemID.None && player.CardType[Index] == 0)
                return false;
            if (itemToPut.type != ItemID.None)
            {
                if (!itemToPut.GetGlobalItem<EntrogicItem>().card || itemToPut.GetGlobalItem<EntrogicItem>().glove)
                    return false;
                ModCard card = (ModCard)itemToPut.modItem;
                if (card.rare == CardRareID.GrandUnified)
                {
                    // 如果本框牌也是大统一，让你过
                    if (inventoryItem.type != ItemID.None && inventoryItem.GetGlobalItem<EntrogicItem>().card && ((ModCard)inventoryItem.modItem).rare == CardRareID.GrandUnified)
                    {
                        return true;
                    }
                    for (int i = 0; i < player.CardType.Length; i++)
                    {
                        if (player.CardType[i] != 0)
                        {
                            Item item = new Item();
                            item.SetDefaults(player.CardType[i]);
                            ModCard mCard = (ModCard)item.modItem;
                            if (mCard.rare == CardRareID.GrandUnified)
                                return false;
                        }
                    }
                }
                else
                {
                    List<int> maxStacks = new List<int> { 6, 3, 2, 1 };
                    int theAmountOfCardsLikeMe = 0;
                    for (int i = 0; i < player.CardType.Length; i++)
                    {
                        if (player.CardType[i] != 0)
                        {
                            Item item = new Item();
                            item.SetDefaults(player.CardType[i]);
                            ModCard mCard = (ModCard)item.modItem;
                            if (player.CardType[i] == itemToPut.type)
                            {
                                theAmountOfCardsLikeMe++;
                                if (theAmountOfCardsLikeMe >= maxStacks[mCard.rare])
                                    return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
    }
}
