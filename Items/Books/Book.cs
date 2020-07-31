using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Entrogic.UI.Books;
using Microsoft.Xna.Framework.Graphics;
using static Terraria.ModLoader.ModContent;
using ReLogic.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using static Entrogic.Entrogic;
using Terraria.Localization;

namespace Entrogic.Items.Books
{
    public abstract class ModBook : ModItem
    {
        public Texture2D[] PageTexture = new Texture2D[129];
        public string[] PageText = new string[257];
        public byte MaxPage = 2;
        public bool bold = false;
        public float[] textScale = new float[257];
        public float[] lineDistance = new float[257];
        //public DynamicSpriteFont font = Entrogic.pixelFont;
        public Color textBaseColor = Color.Black;
        public virtual void BookDefaults() { }
        public sealed override void SetDefaults()
        {
            item.value = Item.sellPrice(0, 0, 20);
            item.width = 1;
            item.height = 1;
            item.useAnimation = 20;
            item.useTime = 20;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.value = 1000;
            item.scale = 0.75f;
            item.rare = ItemRarityID.Quest;
            item.GetGlobalItem<EntrogicItem>().book = true;
            for (int i = 0; i < PageTexture.Length; i++)
                PageTexture[i] = GetTexture("Entrogic/UI/Books/书本_01");
            for (int i = 0; i < PageText.Length; i++)
                PageText[i] = " ";
            for (int i = 0; i < textScale.Length; i++)
                textScale[i] = 0.8f;
            base.SetDefaults();
            SetBookInformations();
            BookDefaults();
        }
        public virtual void SetBookInformations() { }
        public override void HoldItem(Player player)
        {
            SetBookInformations();
        }
        public virtual bool UseBook(Player player) { return true; }
        public sealed override bool CanUseItem(Player player)
        {
            EntrogicPlayer plr = player.GetModPlayer<EntrogicPlayer>();
            // 不会在服务器运行，且只在当尝试开书的玩家是自己时才会运行（别人开书关我什么事）
            if (Main.netMode != NetmodeID.Server && player.whoAmI == Main.myPlayer)
            {
                plr.PageNum = 1;
                if (Main.playerInventory)
                {
                    string warnText = "请于物品栏关闭的情况下开启书籍！";
                    // 不需要发给服务器端，其他玩家不会看到这个信息
                    CombatText.NewText(player.getRect(), Color.Red, warnText);
                    return false;
                }
                BookUI.IsActive = !BookUI.IsActive;
                plr.IsBookActive = BookUI.IsActive;
                MessageHelper.SendBookInfo(player.whoAmI, plr.PageNum, plr.IsBookActive);
            }
            return UseBook(player);
        }
        public string WarnTexts => "[c/FF0000:请于物品栏关闭的情况下开启本书籍]";
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine line = new TooltipLine(mod, mod.Name, WarnTexts);
            if (Main.playerInventory)
                tooltips.Add(line);
            if (AEntrogicConfigClient.Instance.ShowUsefulInformations && item.GetGlobalItem<EntrogicItem>().book)
            {
                line = new TooltipLine(mod, mod.Name, "总页数：" + MaxPage * 2)
                {
                    overrideColor = Color.Gray
                };
                tooltips.Add(line);
                for (int i = 1; i <= MaxPage * 2; i++)
                {
                    line = new TooltipLine(mod, mod.Name, "[" + i + "] 行距：" + lineDistance[i] + ", 字体大小：" + textScale[i])
                    {
                        overrideColor = Color.Gray
                    };
                    tooltips.Add(line);
                }
            }
        }
        public override bool CloneNewInstances => true;
    }
}
