//using Entrogic.Common.ModSystems;
//using Entrogic.Content.Items.BaseTypes;
//using System;
//using Terraria;
//using Terraria.DataStructures;
//using Terraria.Localization;
//using Terraria.ModLoader;
//using static Entrogic.SpecialHeadPlayer;

//namespace Entrogic.Content.Items.ContyElemental.ContyMask
//{
//    [AutoloadEquip(EquipType.Head)]
//    public class ContyMask : Equippable
//    {
//        public override void SetStaticDefaults()
//        {
//            base.SetStaticDefaults();

//            DisplayName.SetDefault("Contaminated Elemental Mask");
//            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "污染之灵面具");
//            Tooltip.SetDefault("It may be confusing to the gel creatures\nNo matter what happened, please refer to the actual product");
//            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "也许对凝胶生物有一定的混淆作用\n无论如何，请以实物为准。");

//            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));
//        }

//        public override void SetDefaults()
//        {
//            Item.width = 18;
//            Item.height = 18;
//            Item.rare = RarityLevelID.MiddleHM;
//            Item.vanity = true;
//            Item.value = Item.sellPrice(0, 0, 20, 0);
//        }

//        public override bool DrawHead() => false;

//        public override bool IsVanitySet(int head, int body, int legs)
//            => head == Mod.GetEquipSlot(nameof(ContyMask), EquipType.Head);

//        public float frameCounter;
//        public override void UpdateVanitySet(Player player) {
//            var headPlayer = player.GetModPlayer<SpecialHeadPlayer>();
//            frameCounter += Main.gameMenu ? 0.12f : 0.09f + Math.Abs(Main.windSpeedCurrent) * 0.3f;
//            HeadDataInfo headDataInfo = new HeadDataInfo {
//                specialHeadTexture = ModContent.Request<Texture2D>($"{ItemAssets.ContyElemental}ContyMask_Head_Effect"),
//                displyFrame = (int)frameCounter % 4
//            };
//            headPlayer.armorHeadDataInfos.Add(headDataInfo);
//        }
//    }
//}
