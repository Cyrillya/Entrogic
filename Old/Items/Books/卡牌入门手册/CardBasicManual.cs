using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Entrogic.UI.Books;
using static Terraria.ModLoader.ModContent;
using Terraria.Localization;

namespace Entrogic.Items.Books.卡牌入门手册
{
    public class CardBasicManual : ModBook
    {
        public override void SetBookInformations()
        {
            item.value = 0;

            MaxPage = 4;
            for (int i = 1; i <= MaxPage * 2f; i++)
                textScale[i] = 0.64f;
            for (int i = 1; i <= MaxPage * 2f; i++)
                lineDistance[i] = -10f;
            lineDistance[1] = -6f;
            PageText[1] = Language.GetTextValue("Mods.Entrogic.Common.PageText1.CardBasicManual"); 
            PageText[2] = Language.GetTextValue("Mods.Entrogic.Common.PageText2.CardBasicManual");
            PageTexture[1] = GetTexture("Entrogic/Items/Books/卡牌入门手册/卡牌入门手册_1");
            PageText[3] = Language.GetTextValue("Mods.Entrogic.Common.PageText3.CardBasicManual");
            PageText[4] = Language.GetTextValue("Mods.Entrogic.Common.PageText4.CardBasicManual");
            PageTexture[2] = GetTexture("Entrogic/Items/Books/卡牌入门手册/卡牌入门手册_2");
            PageText[5] = Language.GetTextValue("Mods.Entrogic.Common.PageText5.CardBasicManual");
            PageText[6] = Language.GetTextValue("Mods.Entrogic.Common.PageText6.CardBasicManual");
            PageTexture[3] = GetTexture("Entrogic/Items/Books/卡牌入门手册/卡牌入门手册_3");
            PageText[7] = Language.GetTextValue("Mods.Entrogic.Common.PageText7.CardBasicManual");
            PageText[8] = Language.GetTextValue("Mods.Entrogic.Common.PageText8.CardBasicManual");
        }
    }
}
