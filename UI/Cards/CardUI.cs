using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;
using ReLogic.Graphics;

using Entrogic.Items.Weapons.Card;
using System;
using System.Collections.Generic;
using Entrogic.Buffs.Miscellaneous;
using Terraria.UI.Chat;
using Terraria.Localization;
using Entrogic.Items.Weapons.Card.Elements;

namespace Entrogic.UI.Cards
{
    public class CardUI : UIState
    {
        internal static bool IsActive = false;
        public UIPanel Card;//新建UI
        public Vector2 statPoint = new Vector2(Main.screenWidth * 0.5f, Main.screenHeight * 0.8f);

        UIHoverImageButton GraveyardHighlight = new UIHoverImageButton(GetTexture("Entrogic/UI/Cards/Graveyard_Highlight"), "墓地");
        UIHoverImageButton PassHighlight = new UIHoverImageButton(GetTexture("Entrogic/UI/Cards/Pass_Highlight"), "过牌");
        UIHoverImageButton Graveyard = new UIHoverImageButton(GetTexture("Entrogic/UI/Cards/Graveyard"), "墓地");
        UIHoverImageButton Pass = new UIHoverImageButton(GetTexture("Entrogic/UI/Cards/Pass"), "过牌");
        UIDelayImageY GraveyardDelay = new UIDelayImageY(GetTexture("Entrogic/UI/Cards/Graveyard_Delay"), 220);
        UIDelayImageY PassDelay = new UIDelayImageY(GetTexture("Entrogic/UI/Cards/Pass_Delay"), 220);
        UITextImage Libraries = new UITextImage(GetTexture("Entrogic/UI/Cards/Libraries"), "0", new Color(20, 20, 20));
        UIImage Bar = new UIImage(GetTexture("Entrogic/UI/Cards/CardProgressBar_Empty"));
        UIDelayImageX BarFull = new UIDelayImageX(GetTexture("Entrogic/UI/Cards/CardProgressBar"), 255);
        UIPanel EventBar = new UIPanel();
        internal List<UICardHandGridButton> Grid = new List<UICardHandGridButton>();
        CardManaUI[] ManaCrystal = new CardManaUI[10];
        public override void OnInitialize()
        {
            Vector2 grave = new Vector2(48f, 30f);
            Graveyard.Left.Set(statPoint.X + 220f - grave.X * 0.5f, 0f);
            Graveyard.Top.Set(statPoint.Y - grave.Y * 0.5f, 0f);
            Graveyard.Width.Set(grave.X, 0f);
            Graveyard.Height.Set(grave.Y, 0f);
            Append(Graveyard);
            GraveyardHighlight.Left.Set(0f, 0f);
            GraveyardHighlight.Top.Set(0f, 0f);
            GraveyardHighlight.Width.Set(grave.X, 0f);
            GraveyardHighlight.Height.Set(grave.Y, 0f);
            GraveyardHighlight.SetVisibility(1f, 0f);
            GraveyardHighlight.OnClick += new MouseEvent(GraveyardClicked);
            Graveyard.Append(GraveyardHighlight);
            GraveyardDelay.Left.Set(statPoint.X + 220f - grave.X * 0.5f, 0f);
            GraveyardDelay.Top.Set(statPoint.Y - grave.Y * 0.5f, 0f);
            GraveyardDelay.Width.Set(grave.X, 0f);
            GraveyardDelay.Height.Set(grave.Y, 0f);
            Append(GraveyardDelay);

            Vector2 libraries = new Vector2(38f, 32f);
            Libraries.Left.Set(statPoint.X - 220f - libraries.X * 0.5f, 0f);
            Libraries.Top.Set(statPoint.Y - libraries.Y * 1.2f, 0f);
            Libraries.Width.Set(libraries.X, 0f);
            Libraries.Height.Set(libraries.Y, 0f);
            Append(Libraries);

            Vector2 pass = new Vector2(32f, 30f);
            Pass.Left.Set(statPoint.X - 220f - pass.X * 0.5f, 0f);
            Pass.Top.Set(statPoint.Y + pass.Y * 0.2f, 0f);
            Pass.Width.Set(pass.X, 0f);
            Pass.Height.Set(pass.Y, 0f);
            Append(Pass);
            PassHighlight.Left.Set(0f, 0f);
            PassHighlight.Top.Set(0f, 0f);
            PassHighlight.Width.Set(pass.X, 0f);
            PassHighlight.Height.Set(pass.Y, 0f);
            PassHighlight.SetVisibility(1f, 0f);
            PassHighlight.OnClick += new MouseEvent(PassClicked);
            Pass.Append(PassHighlight);
            PassDelay.Left.Set(statPoint.X - 220f - pass.X * 0.5f, 0f);
            PassDelay.Top.Set(statPoint.Y + pass.Y * 0.2f, 0f);
            PassDelay.Width.Set(pass.X, 0f);
            PassDelay.Height.Set(pass.Y, 0f);
            Append(PassDelay);

            Vector2 bar = new Vector2(346f, 22f);
            Bar.SetPadding(0);
            Bar.Left.Set(statPoint.X - bar.X / 2f, 0f);
            Bar.Top.Set(statPoint.Y - bar.Y / 2f - 50f, 0f);
            Bar.Width.Set(bar.X, 0f);
            Bar.Height.Set(bar.Y, 0f);
            Append(Bar);
            BarFull.SetPadding(0);
            BarFull.Left.Set(statPoint.X - bar.X / 2f, 0f);
            BarFull.Top.Set(statPoint.Y - bar.Y / 2f - 50f, 0f);
            BarFull.Width.Set(bar.X, 0f);
            BarFull.Height.Set(bar.Y, 0f);
            Append(BarFull);

            for (int i = 0; i >= -(ManaCrystal.Length - 1); i--)
            {
                Vector2 crystal = new Vector2(20f, 20f);
                float posX = statPoint.X + 150f + (crystal.X * 0.5f + 15) * i;
                float posY = statPoint.Y + 52f - crystal.Y * 0.5f;
                int num = Math.Abs(i);
                ManaCrystal[num] = new CardManaUI(GetTexture("Entrogic/UI/Cards/ManaCrystal"), num, new Vector2(posX, posY));
                ManaCrystal[num].SetPadding(0);
                ManaCrystal[num].Left.Set(posX, 0f);
                ManaCrystal[num].Top.Set(posY, 0f);
                ManaCrystal[num].Width.Set(crystal.X, 0f);
                ManaCrystal[num].Height.Set(crystal.Y, 0f);
                Append(ManaCrystal[num]);
            }
            Vector2 grid = new Vector2(66f, 66f);
            for (int i = 0; i < 6; i++)
            {
                float posX = statPoint.X - 200f + 2f + grid.X * i;
                float posY = statPoint.Y - grid.Y * 0.5f;
                UICardHandGridButton Gri = new UICardHandGridButton(GetTexture("Entrogic/UI/Cards/HelpGrid"), i, new Vector2(posX, posY));
                Gri.Left.Set(posX, 0f);
                Gri.Top.Set(posY, 0f);
                Gri.Width.Set(grid.X, 0f);
                Gri.Height.Set(grid.Y, 0f);
                Grid.Add(Gri);
                try
                {
                    Append(Grid[i]);
                }
                catch { }
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Player player = Main.LocalPlayer;
            EntrogicPlayer entrogicPlayer = EntrogicPlayer.ModPlayer(player);
            bool Active = false;
            if (player.inventory[player.selectedItem] != null && player.inventory[player.selectedItem].type != ItemID.None)
                if (player.inventory[player.selectedItem].GetGlobalItem<EntrogicItem>().glove)
                    Active = true;
            if (Main.mouseItem != null && Main.mouseItem.type != ItemID.None)
                if (Main.mouseItem.GetGlobalItem<EntrogicItem>().glove)
                    Active = true;
            if (!Active)
                return;

            if (Entrogic.PassHotkey.JustPressed) CardPass();
            if (Entrogic.WashHotkey.JustPressed) CardWash();
            #region 实时调整位置
            statPoint = new Vector2(Main.screenWidth * 0.5f, Main.screenHeight * 0.8f);
            Vector2 grave = new Vector2(48f, 30f);
            Graveyard.Left.Pixels = statPoint.X + 220f - grave.X * 0.5f;
            Graveyard.Top.Pixels = statPoint.Y - grave.Y * 0.5f;
            GraveyardDelay.Left.Pixels = statPoint.X + 220f - grave.X * 0.5f;
            GraveyardDelay.Top.Pixels = statPoint.Y - grave.Y * 0.5f;
            Vector2 libraries = new Vector2(38f, 32f);
            Libraries.Left.Pixels = statPoint.X - 220f - libraries.X * 0.5f;
            Libraries.Top.Pixels = statPoint.Y - libraries.Y * 1.2f;
            Vector2 pass = new Vector2(32f, 30f);
            Pass.Left.Pixels = statPoint.X - 220f - pass.X * 0.5f;
            Pass.Top.Pixels = statPoint.Y + pass.Y * 0.2f;
            PassDelay.Left.Pixels = statPoint.X - 220f - pass.X * 0.5f;
            PassDelay.Top.Pixels = statPoint.Y + pass.Y * 0.2f;
            Vector2 bar = new Vector2(346f, 22f);
            Bar.Left.Pixels = statPoint.X - bar.X / 2f;
            Bar.Top.Pixels = statPoint.Y - bar.Y / 2f - (CEntrogicCardConfig.Instance.HandSlotSpecial ? 0f : 50f);
            BarFull.Left.Pixels = Bar.Left.Pixels;
            BarFull.Top.Pixels = Bar.Top.Pixels;
            for (int i = 0; i >= -(ManaCrystal.Length - 1); i--)
            {
                Vector2 crystal = new Vector2(20f, 20f);
                float posX = statPoint.X + 150f + (crystal.X * 0.5f + 15) * i;
                float posY = statPoint.Y + 52f - crystal.Y * 0.5f;
                int num = Math.Abs(i);
                ManaCrystal[num].position = new Vector2(posX, posY);
                ManaCrystal[num].Left.Pixels = posX;
                ManaCrystal[num].Top.Pixels = posY;
            }
            for (int i = 0; i < Grid.Count; i++)
            {
                Vector2 grid = new Vector2(66f, 66f);
                float posX = statPoint.X - 200f + 2f + grid.X * i;
                float posY = statPoint.Y - grid.Y * 0.5f;
                if (CEntrogicCardConfig.Instance.HandSlotSpecial)
                {
                    Vector2 screenCenter = new Vector2(Main.screenWidth, Main.screenHeight) * 0.5f;
                    
                    switch (i)
                    {
                        case 0:
                        case 2:
                        case 3:
                        case 5:
                            {
                                posX = screenCenter.X + ((i + 1) % 3 != 0 ? -grid.X - 10 : grid.X + 10) - grid.X / 2f;
                                posY = screenCenter.Y + (i <= 2 ? -50 : 50) - grid.Y / 2f;
                            }
                            break;
                        case 1:
                        case 4:
                            {
                                posX = screenCenter.X - grid.X / 2f;
                                posY = screenCenter.Y + (i <= 2 ? -72 : 72) - grid.Y / 2f;
                            }
                            break;
                    }
                }
                Grid[i].position = new Vector2(posX, posY);
                Grid[i].Left.Pixels = posX;
                Grid[i].Top.Pixels = posY;
                Append(Grid[i]);
            }
            #endregion

            base.Draw(spriteBatch);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Player player = Main.LocalPlayer;
            EntrogicPlayer ePlayer = player.GetModPlayer<EntrogicPlayer>();
            Libraries.Text = ePlayer.LibNum.ToString();
            GraveyardDelay.maxValue = Main.LocalPlayer.GetModPlayer<EntrogicPlayer>().CardWashStatDelay;
            GraveyardDelay.delayValue = ePlayer.CardWashDelay;
            PassDelay.maxValue = Main.LocalPlayer.GetModPlayer<EntrogicPlayer>().CardPassStatDelay;
            PassDelay.delayValue = ePlayer.CardPassDelay;
            CardBuffPlayer cPlayer = player.GetModPlayer<CardBuffPlayer>();
            BarFull.maxValue = (int)cPlayer.attackCardRemainingTimesMax;
            BarFull.delayValue = (int)cPlayer.attackCardRemainingTimes;

            if (Main.LocalPlayer.GetModPlayer<EntrogicPlayer>().CardWashDelay > 0)
            {
                Graveyard.SetVisibility(0.3f, 0.3f);
                GraveyardHighlight.SetVisibility(0f, 0f);
            }
            else
            {
                Graveyard.SetVisibility(1f, 0.3f);
                GraveyardHighlight.SetVisibility(1f, 0f);
            }
            if (Main.LocalPlayer.GetModPlayer<EntrogicPlayer>().CardPassDelay > 0)
            {
                Pass.SetVisibility(0.3f, 0.3f);
                PassHighlight.SetVisibility(0f, 0f);
            }
            else
            {
                Pass.SetVisibility(1f, 0.3f);
                PassHighlight.SetVisibility(1f, 0f);
            }
        }
        private void GraveyardClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            CardWash();
        }
        private void PassClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            CardPass();
        }
        private void CardWash()
        {
            EntrogicPlayer player = Main.LocalPlayer.GetModPlayer<EntrogicPlayer>();
            if (player.CardWashDelay > 0)
                return;
            int cardGot = 0;
            Main.PlaySound(SoundID.MenuOpen);
            bool ButWeStillHaveCards = false;
            for (int i = 0; i < player.CardGraveType.Length; i++)
            {
                player.CardGraveType[i] = 0;
            }
            for (int i = 0; i < player.CardType.Length; i++)
            {
                cardGot++;
                if (player.CardReadyType[i] != 0)
                    ButWeStillHaveCards = true;
                player.CardReadyType[i] = player.CardType[i];
                player.CardReadyCost[i] = 0;
                if (player.CardType[i] != 0)
                {
                    Item item = new Item();
                    item.SetDefaults(player.CardType[i]);
                    player.CardReadyCost[i] = ((ModCard)item.modItem).costMana;
                }
            }
            for (int i = player.CardType.Length; i < player.CardReadyType.Length; i++)
            {
                if (player.CardReadyType[i] != 0)
                    ButWeStillHaveCards = true;
                player.CardReadyType[i] = 0;
                player.CardReadyCost[i] = 0;
            }
            player.NewRecentCardMessage(Language.GetTextValue("Mods.Entrogic.NormalDrawCard", cardGot));
            if (player.IsDelayCycle_StaticWatch)
            {
                int manaGot = 0;
                if (player.IsMoreMana_CryptTreasure && player.ManaLeft > 0 && ButWeStillHaveCards)
                {
                    Main.LocalPlayer.AddBuff(BuffType<MagicCursed>(), 5);
                    player.TwoCardsCount_CryptTreasure = 2;
                    player.IsTwoCards_CryptTreasure = true;
                    player.ManaLeft = player.ManaMax;
                    manaGot += player.ManaLeft;
                    for (int i = 0; i < 2; i++)
                    {
                        player.ManaLeft++;
                        manaGot++;
                        if (player.ManaLeft >= EntrogicPlayer.ManaTrueMax)
                            break;
                    }
                }
                else
                {
                    manaGot += player.ManaMax - player.ManaLeft;
                    player.ManaLeft = player.ManaMax;
                }
                player.NewRecentCardMessage(Language.GetTextValue("Mods.Entrogic.StaticWatchDrawCard", cardGot, manaGot));
            }
            player.CardWashDelay = player.CardWashStatDelay;
        }
        private void CardPass()
        {
            EntrogicPlayer ePlayer = Main.LocalPlayer.GetModPlayer<EntrogicPlayer>();
            if (ePlayer.CardPassDelay > 0)
                return;
            int cardDrawed = 0;
            int manaRegrew = 0;
            Main.PlaySound(SoundID.MenuOpen);
            bool ButWeStillHaveCards = false;
            for (int a = 0; a < ePlayer.CardHandType.Length; a++)
            {
                if (ePlayer.CardHandType[a] != 0)
                {
                    int type = ePlayer.CardHandType[a];
                    ePlayer.CardHandType[a] = 0;
                    ePlayer.CardSetToGrave(type, a);
                    ButWeStillHaveCards = true;
                }
            }
            ePlayer.CardHandMax += ePlayer.MoreCard_EnergyRecovery;
            for (int a = 0; a < ePlayer.CardHandMax; a++)
            {
                List<int> canChooseCards = new List<int>();
                for (int i = 0; i < ePlayer.CardReadyType.Length; i++)
                {
                    if (ePlayer.CardReadyType[i] == 0)
                        continue;
                    canChooseCards.Add(i);
                }
                if (canChooseCards.Count > 0)
                {
                    for (int i = 0; i < ePlayer.CardHandType.Length; i++)
                        if (ePlayer.CardHandType[i] == 0)
                        {
                            int chooseCard = Main.rand.Next(0, canChooseCards.Count);
                            ePlayer.CardHandType[i] = ePlayer.CardReadyType[canChooseCards[chooseCard]];
                            ePlayer.CardHandCost[i] = ePlayer.CardReadyCost[canChooseCards[chooseCard]];
                            ePlayer.CardReadyType[canChooseCards[chooseCard]] = 0;
                            ePlayer.CardReadyCost[canChooseCards[chooseCard]] = 0;
                            cardDrawed++;
                            break;
                        }
                }
            }
            // 精力恢复效果
            ePlayer.MoreCard_EnergyRecovery = 0;
            // 宝藏效果
            if (ePlayer.IsTwoCards_CryptTreasure)
            {
                ePlayer.IsTwoCards_CryptTreasure = false;
                ePlayer.TwoCardsCount_CryptTreasure = 0;
            }
            if (!ePlayer.IsDelayCycle_StaticWatch)
            {
                // 宝藏效果
                if (ePlayer.IsMoreMana_CryptTreasure && ePlayer.ManaLeft > 0 && ButWeStillHaveCards) // 宝藏效果
                {
                    Main.LocalPlayer.AddBuff(BuffType<MagicCursed>(), 5);
                    ePlayer.TwoCardsCount_CryptTreasure = 2;
                    ePlayer.IsTwoCards_CryptTreasure = true;
                    ePlayer.ManaLeft = ePlayer.ManaMax;
                    manaRegrew += ePlayer.ManaLeft;
                    for (int i = 0; i < 2; i++)
                    {
                        ePlayer.ManaLeft++;
                        manaRegrew++;
                        if (ePlayer.ManaLeft >= EntrogicPlayer.ManaTrueMax)
                            break;
                    }
                } // 宝藏效果
                else
                {
                    ePlayer.ManaLeft = ePlayer.ManaMax;
                    manaRegrew += ePlayer.ManaLeft;
                }
                // 幸运币效果
                if (ePlayer.IsMoreManaOrCard_LuckyCoin) // 幸运币效果
                {
                    if (Main.rand.NextBool(2)) // 魔力+1
                    {
                        if (ePlayer.ManaLeft < EntrogicPlayer.ManaTrueMax)
                        {
                            ePlayer.ManaLeft++;
                            manaRegrew++;
                        }
                    } // 魔力+1
                    else // 抽牌
                    {
                        List<int> canChooseCards = new List<int>();
                        for (int i = 0; i < ePlayer.CardReadyType.Length; i++)
                        {
                            if (ePlayer.CardReadyType[i] == 0)
                                continue;
                            canChooseCards.Add(i);
                        }
                        if (canChooseCards.Count > 0)
                        {
                            for (int i = 0; i < ePlayer.CardHandType.Length; i++)
                                if (ePlayer.CardHandType[i] == 0)
                                {
                                    int chooseCard = Main.rand.Next(0, canChooseCards.Count);
                                    ePlayer.CardHandType[i] = ePlayer.CardReadyType[canChooseCards[chooseCard]];
                                    ePlayer.CardHandCost[i] = ePlayer.CardReadyCost[canChooseCards[chooseCard]];
                                    ePlayer.CardReadyType[canChooseCards[chooseCard]] = 0;
                                    ePlayer.CardReadyCost[canChooseCards[chooseCard]] = 0;
                                    cardDrawed++;
                                    break;
                                }
                        }
                    } // 抽牌
                } // 幸运币效果
            }
            ePlayer.NewRecentCardMessage(Language.GetTextValue("Mods.Entrogic.NormalPassCard", cardDrawed, manaRegrew));
            ePlayer.CardPassDelay = ePlayer.CardPassStatDelay;
        }
    }
    internal class UICardHandGridButton : UIImage
    {
        internal Vector2 Size = new Vector2(64f, 64f);
        internal int number;
        internal Vector2 position;
        internal bool hasTicked = false;
        public UICardHandGridButton(Texture2D texture, int num, Vector2 pos) : base(texture)
        {
            number = num;
            position = pos;
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            //base.DrawSelf(spriteBatch); // HelpGrid是隐形的
            Player player = Main.LocalPlayer;
            EntrogicPlayer ePlayer = player.GetModPlayer<EntrogicPlayer>();
            int type = ePlayer.CardHandType[number];
            Item ContainedItem = new Item();
            ContainedItem.SetDefaults(type);
            if (ContainedItem.type != ItemID.None)
            {
                Texture2D tex = Main.itemTexture[type];
                int alpha = 80;
                if (IsMouseHovering)
                    alpha = 255;
                var frame = Main.itemAnimations[ContainedItem.type] != null ? Main.itemAnimations[ContainedItem.type].GetFrame(Main.itemTexture[ContainedItem.type]) : Main.itemTexture[ContainedItem.type].Frame(1, 1, 0, 0);
                spriteBatch.Draw(tex, position + Size * 0.5f - tex.Size() * 0.5f, new Rectangle?(frame), new Color(alpha, alpha, alpha, alpha), 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            }
            if (IsMouseHovering && type != 0 && ContainedItem != null)
            {
                Main.hoverItemName = ContainedItem.Name + "\r\n费用：" + ePlayer.CardHandCost[number];
                if (!hasTicked)
                {
                    Main.PlaySound(SoundID.MenuTick);
                    hasTicked = true;
                }
            }
            else hasTicked = false;
        }
        public override void Click(UIMouseEvent evt)
        {
            UseThisCard();
        }
        internal void UseThisCard()
        {
            Player player = Main.LocalPlayer;
            EntrogicPlayer ePlayer = player.GetModPlayer<EntrogicPlayer>();
            CardBuffPlayer cPlayer = player.GetModPlayer<CardBuffPlayer>();
            if (ePlayer.CardHandType[number] != 0)
            {
                Item item = new Item();
                item.SetDefaults(ePlayer.CardHandType[number]);
                ModItem mItem = item.modItem;
                ModCard card = (ModCard)mItem;
                if (card.special || !card.CanUseCard() || !TheCardEffectThatICantDoInACSharpFile(player, ePlayer, card))
                    return;
                if (ePlayer.ManaLeft < ePlayer.CardHandCost[number])
                    return;
                if (ePlayer.IsTwoCards_CryptTreasure)
                {
                    if (ePlayer.TwoCardsCount_CryptTreasure <= 0)
                        return;
                    ePlayer.TwoCardsCount_CryptTreasure--;
                }
                if (Main.mouseItem != null && Main.mouseItem.type != ItemID.None) // 手套使用物品
                {
                    if (Main.mouseItem.GetGlobalItem<EntrogicItem>().glove)
                    {
                        ((ModCard)Main.mouseItem.modItem).OnGloveUseCard(player);
                    }
                } // 手套使用物品
                if (player.inventory[player.selectedItem] != null && player.inventory[player.selectedItem].type != ItemID.None) // 手套使用物品
                {
                    if (player.inventory[player.selectedItem].GetGlobalItem<EntrogicItem>().glove)
                    {
                        ((ModCard)player.inventory[player.selectedItem].modItem).OnGloveUseCard(player);
                    }
                } // 手套使用物品
                if (card.minion)
                {
                    card.MinionEffects(player, player.Center, item.damage, item.knockBack, number);
                    ePlayer.ManaLeft -= ePlayer.CardHandCost[number];
                    goto End;
                }
                ePlayer.ManaLeft -= ePlayer.CardHandCost[number];
                cPlayer.attackCardRemainingTimes = card.attackCardRemainingTimes;
                cPlayer.attackCardRemainingTimesMax = card.attackCardRemainingTimes;
                cPlayer.card = item;
                cPlayer.mCard = card;
                ePlayer.NewRecentCardMessage($"你使用了{card.DisplayName}作为攻击手段！", true);
            End:
                ePlayer.CardUseCount++;
                if (item.buffType != -1)
                    player.AddBuff(item.buffType, item.buffTime);
                int manaHealAmount = (int)MathHelper.Min(card.healMana, EntrogicPlayer.ManaTrueMax - ePlayer.ManaLeft);
                ePlayer.ManaLeft += manaHealAmount;
                #region Draw Cards
                bool[] series = new bool[4];
                bool[] style = new bool[3]; // 0-攻击, 1-仆从, 2-特殊
                string eventMessageSeries = "";
                string eventMessageStyle = "";
                int amount = card.DrawCardAmount(player, number, ref series, ref style);
                #region Find String
                bool firstSeries = false;
                for (int i = 0; i < series.Length; i++)
                {
                    if (!series[i])
                    {
                        goto FindSeries;
                    }
                }
                eventMessageSeries += Language.GetTextValue("Mods.Entrogic.All");
                goto FindStyle;
            FindSeries:
                if (series[0])
                {
                    eventMessageSeries += Language.GetTextValue("Mods.Entrogic.SeriesNone");
                    firstSeries = true;
                }
                if (series[1])
                {
                    if (firstSeries)
                        eventMessageSeries += "、";
                    eventMessageSeries += Language.GetTextValue("Mods.Entrogic.SeriesElement");
                    firstSeries = true;
                }
                if (series[2])
                {
                    if (firstSeries)
                        eventMessageSeries += "、";
                    eventMessageSeries += Language.GetTextValue("Mods.Entrogic.SeriesOrganism");
                    firstSeries = true;
                }
                if (series[3])
                {
                    if (firstSeries)
                        eventMessageSeries += "、";
                    eventMessageSeries += Language.GetTextValue("Mods.Entrogic.SeriesUndead");
                }
            FindStyle:
                for (int i = 0; i < style.Length; i++)
                {
                    if (!style[i])
                    {
                        goto StillFindingStyle;
                    }
                }
                goto EndFinding;
            StillFindingStyle:
                if (style[0])
                {
                    eventMessageStyle += Language.GetTextValue("Mods.Entrogic.CardAttack");
                }
                if (style[1] || style[2])
                {
                    eventMessageStyle += Language.GetTextValue("Mods.Entrogic.CardMinion");
                }
            EndFinding:
                #endregion
                if (amount != 0 && ePlayer.LibNum > 0)
                {
                    int cardGot = 0;
                    for (int a = 0; a < amount; a++)
                    {
                        List<int> canChooseCards = new List<int>();
                        for (int i = 0; i < ePlayer.CardReadyType.Length; i++)
                        {
                            if (ePlayer.CardReadyType[i] == 0)
                                continue;
                            Item iI = new Item();
                            iI.SetDefaults(ePlayer.CardReadyType[i]);
                            ModCard cI = (ModCard)iI.modItem;
                            bool meetsRequirements = false;
                            if (!cI.minion && !cI.special && style[0] || cI.minion && !cI.special && style[1] || !cI.minion && cI.special && style[2])
                                for (int j = 0; j < series.Length; j++)
                                {
                                    if (cI.series == j && series[j])
                                    {
                                        meetsRequirements = true;
                                        break;
                                    }
                                }
                            if (meetsRequirements)
                                canChooseCards.Add(i);
                        }
                        if (canChooseCards.Count > 0)
                        {
                            if (a != 0)
                            {
                                for (int i = 0; i < ePlayer.CardHandType.Length; i++)
                                    if (ePlayer.CardHandType[i] == 0)
                                    {
                                        int chooseCard = Main.rand.Next(0, canChooseCards.Count);
                                        int type = ePlayer.CardReadyType[canChooseCards[chooseCard]], cost = ePlayer.CardReadyCost[canChooseCards[chooseCard]];
                                        if (card.HaveDrawCard(player, number, a, ref type, ref cost))
                                        {
                                            cardGot++;
                                            ePlayer.CardHandType[i] = type;
                                            ePlayer.CardHandCost[i] = cost;
                                            ePlayer.CardReadyType[canChooseCards[chooseCard]] = 0;
                                            ePlayer.CardReadyCost[canChooseCards[chooseCard]] = 0;
                                        }
                                        break;
                                    }
                            }
                            else
                            {
                                int chooseCard = Main.rand.Next(0, canChooseCards.Count);
                                int type = ePlayer.CardReadyType[canChooseCards[chooseCard]], cost = ePlayer.CardReadyCost[canChooseCards[chooseCard]];
                                if (!card.HaveDrawCard(player, number, a, ref type, ref cost))
                                    continue;
                                ePlayer.CardSetToGrave(ePlayer.CardHandType[number], number, false, 1, true);
                                cardGot++;
                                ePlayer.CardHandType[number] = type;
                                ePlayer.CardHandCost[number] = cost;
                                ePlayer.CardReadyType[canChooseCards[chooseCard]] = 0;
                                ePlayer.CardReadyCost[canChooseCards[chooseCard]] = 0;
                            }
                        }
                    }
                    string text = Language.GetTextValue("Mods.Entrogic.CardDrawCard", item.Name, cardGot, eventMessageSeries, eventMessageStyle);
                    if (!card.HaveDrawCardMessage(player, ref text, cardGot, eventMessageSeries, eventMessageStyle, number))
                        return;
                    ePlayer.NewRecentCardMessage(text, true);
                    return;
                }
                #endregion
                if (!card.NoUseNormalDelete)
                {
                    int type = ePlayer.CardHandType[number];
                    ePlayer.CardHandType[number] = 0;
                    ePlayer.CardSetToGrave(type, number);
                    ePlayer.CardHandCost[number] = 0;
                }
            }
        }
        public bool TheCardEffectThatICantDoInACSharpFile(Player player, EntrogicPlayer ePlayer, ModCard card)
        {
            if (ePlayer.IsDestroyNextCard_InnerRage)
            {
                ePlayer.IsDestroyNextCard_InnerRage = false;
                List<int> elementCardNumber = new List<int>();
                for (int i = 0; i < ePlayer.CardReadyType.Length; i++)
                {
                    if (ePlayer.CardReadyType[i] == 0)
                        continue;
                    Item item = new Item();
                    item.SetDefaults(ePlayer.CardReadyType[i]);
                    ModItem modItem = item.modItem;
                    ModCard modCard = (ModCard)modItem;
                    if (modCard.series == CardSeriesID.Element)
                    {
                        elementCardNumber.Add(i);
                    }
                }
                if (elementCardNumber.Count > 0)
                {
                    int chooseCard = Main.rand.Next(0, elementCardNumber.Count);
                    ePlayer.CardSetToGrave(ePlayer.CardHandType[number], number, true, 1, true);
                    ePlayer.CardHandType[number] = ePlayer.CardReadyType[elementCardNumber[chooseCard]];
                    ePlayer.CardHandCost[number] = ePlayer.CardReadyCost[elementCardNumber[chooseCard]];
                    ePlayer.CardReadyType[elementCardNumber[chooseCard]] = 0;
                    ePlayer.CardReadyCost[elementCardNumber[chooseCard]] = 0;
                    return false;
                }
                Item item1 = new Item();
                item1.SetDefaults(ItemType<InnerRage>());
                ePlayer.NewRecentCardMessage(Language.GetTextValue("Mods.Entrogic.CardBurnAndDrawCard", item1.Name, 1, Language.GetTextValue("Mods.Entrogic.All"), ""), true);
                return true;
            }
            return true;
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime); // don't remove.
            Player player = Main.LocalPlayer;
            EntrogicPlayer ePlayer = player.GetModPlayer<EntrogicPlayer>();
            if (ContainsPoint(Main.MouseScreen) && ePlayer.CardHandType[number] != 0)
            {
                Main.LocalPlayer.mouseInterface = true;
            }
        }
    }
    public class CardManaUI : UIImage
    {
        internal Vector2 Size = new Vector2(64f, 64f);
        internal int number;
        internal Vector2 position;
        public CardManaUI(Texture2D texture, int num, Vector2 pos) : base(texture)
        {
            number = num;
            position = pos;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Player player = Main.LocalPlayer;
            EntrogicPlayer ePlayer = player.GetModPlayer<EntrogicPlayer>();
            if (ePlayer.ManaLeft <= number)
                return;
            base.Draw(spriteBatch);
        }
    }
}