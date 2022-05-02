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
using ReLogic.Content;
using Terraria.GameContent;

namespace Entrogic.UI.Cards
{
    public class CardInventoryGridUI : UIState
    {
        public UIPanel CardInventoryGrid;//新建UI
        public static bool visible = false;//UI开启的bool
        public Vector2 statPoint = new Vector2(40f, 280f);
        UICardGridButton[] Grid = new UICardGridButton[9];
        public override void OnInitialize()
        {
            Vector2 inventory = new Vector2(30f, 32f);
            Vector2 grid = new Vector2(64f, 64f);
            for (int i = 0; i < Grid.Length; i++)
            {
                float posX = statPoint.X + 32f + inventory.X + grid.X * (-0.5f + (int)(i % 3)) + (int)(i % 3) * 10f;
                float posY = statPoint.Y + 18f + grid.Y * (-0.5f + (int)(i / 3)) + (int)(i / 3) * 10f;
                Grid[i] = new UICardGridButton(GetTexture("Entrogic/UI/Cards/InventoryGrid"), i, new Vector2(posX, posY));
                Grid[i].Left.Set(posX, 0f);
                Grid[i].Top.Set(posY, 0f);
                Grid[i].Width.Set(grid.X, 0f);
                Grid[i].Height.Set(grid.Y, 0f);
                Grid[i].SetVisibility(1f, 0.86f);
                Append(Grid[i]);
            }
        }
    }
    internal class UICardGridButton : UIImageButton
    {
        internal Vector2 Size = new Vector2(64f, 64f);
        internal int number;
        internal Vector2 position;
        public UICardGridButton(Asset<Texture2D> texture, int num, Vector2 pos) : base(texture)
        {
            number = num;
            position = pos;
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            EntrogicPlayer player = Main.LocalPlayer.GetModPlayer<EntrogicPlayer>();
            Item ContainedItem = new Item();
            ContainedItem.SetDefaults(player.CardType[number]);
            if (ContainedItem.type != ItemID.None)
            {
                if (ContainsPoint(Main.MouseScreen))
                {
                    Main.hoverItemName = ContainedItem.Name;
                    Main.HoverItem = ContainedItem.Clone();
                }
                var frame = Main.itemAnimations[ContainedItem.type] != null ? Main.itemAnimations[ContainedItem.type].GetFrame((Texture2D)TextureAssets.Item[ContainedItem.type]) : ((Texture2D)TextureAssets.Item[ContainedItem.type]).Frame(1, 1, 0, 0);
                Texture2D tex = (Texture2D)TextureAssets.Item[ContainedItem.type];
                spriteBatch.Draw(tex, position + Size * 0.5f - tex.Size() * 0.5f + new Vector2(2f, -1f), new Rectangle?(frame), Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            }
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime); // don't remove.
            if (ContainsPoint(Main.MouseScreen))
            {
                Main.LocalPlayer.mouseInterface = true;
            }
        }
        public override void Click(UIMouseEvent evt)
        {
            EntrogicPlayer player = Main.LocalPlayer.GetModPlayer<EntrogicPlayer>();
            Item mouseItem = Main.mouseItem;
            if (mouseItem.type != ItemID.None) // 特殊判断：如果手上物品不是卡牌就return
            {
                if (!mouseItem.GetGlobalItem<EntrogicItem>().card)
                    return;
            } // 特殊判断：如果手上物品不是卡牌就return
            if (mouseItem.type == ItemID.None && player.CardType[number] == 0)
                return;
            if (mouseItem.type != ItemID.None)
            {
                ModCard card = (ModCard)mouseItem.modItem;
                if (card.rare == CardRareID.GrandUnified)
                {
                    for (int i = 0; i < player.CardType.Length; i++)
                    {
                        if (player.CardType[i] != 0)
                        {
                            Item item = new Item();
                            item.SetDefaults(player.CardType[i]);
                            ModCard mCard = (ModCard)item.modItem;
                            if (mCard.rare == CardRareID.GrandUnified)
                                return;
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
                            if (player.CardType[i] == mouseItem.type)
                            {
                                theAmountOfCardsLikeMe++;
                                if (theAmountOfCardsLikeMe >= maxStacks[mCard.rare])
                                    return;
                            }
                        }
                    }
                }
            }
<<<<<<< HEAD
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Grab, -1, -1, 1, 1f, 0.0f);
=======
            Main.PlaySound(SoundID.Grab, -1, -1, 1, 1f, 0.0f);
>>>>>>> cce2d304a6401d54e5264babee0ed98d0c73ee96
            if (player.CardType[number] != 0 && mouseItem.type == ItemID.None)
            {
                Item item = new Item();
                item.SetDefaults(player.CardType[number]);
                Main.mouseItem = item;
                player.CardType[number] = 0;
                return;
            }
            if (!mouseItem.GetGlobalItem<EntrogicItem>().card || mouseItem.GetGlobalItem<EntrogicItem>().glove)
                return;
            else if (player.CardType[number] == mouseItem.type && mouseItem.stack < mouseItem.maxStack)
            {
                player.CardType[number] = 0;
                Main.mouseItem.stack++;
                return;
            }
            if (player.CardType[number] != 0)
            {
                if (mouseItem.stack <= 1)
                {
                    Item item = new Item();
                    item.SetDefaults(player.CardType[number]);
                    player.CardType[number] = mouseItem.type;
                    Main.mouseItem = item;
                }
            }
            else
            {
                Main.mouseItem.stack--;
                player.CardType[number] = mouseItem.type;
            }
        }
    }
}
