//using Entrogic.Content.Items.BaseTypes;
//using System;
//using Terraria;
//using Terraria.ModLoader;
//using static Entrogic.SpecialHeadPlayer;

//namespace Entrogic.Content.Misc
//{
//    [AutoloadEquip(EquipType.Face)]
//    public class MamonasBlessing : ItemBase
//    {
//        public override void SetStaticDefaults() {
//            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
//        }

//        public override void SetDefaults() {
//            Item.width = 120;
//            Item.height = 100;
//            Item.value = 10000;
//            Item.rare = RarityLevelID.EarlyHM;
//            Item.accessory = true;
//            Item.expert = true;
//            Item.value = Item.sellPrice(0, 8);
//        }

//        public override void UpdateVanity(Player player) {
//            UpdateSpecialHead(player);
//            base.UpdateVanity(player);
//        }


//        public override void UpdateAccessory(Player player, bool hideVisual) {
//            if (!hideVisual) {
//                UpdateSpecialHead(player);
//            }
//        }

//        public float frameCounter;
//        private void UpdateSpecialHead(Player player) {
//            var headPlayer = player.GetModPlayer<SpecialHeadPlayer>();
//            if (Math.Abs(player.velocity.X) < 0.2f && Math.Abs(player.velocity.Y) < 0.2f) {
//                frameCounter = 0;
//            }
//            else {
//                frameCounter += 0.1f;
//                if ((int)frameCounter % 4 == 0) {
//                    frameCounter += 1f;
//                }
//            }
//            HeadDataInfo headDataInfo = new HeadDataInfo {
//                specialHeadTexture = ModContent.Request<Texture2D>($"{Texture}_Effect"),
//                displyFrame = (int)frameCounter % 4
//            };
//            headPlayer.accHeadDataInfos.Add(headDataInfo);
//        }

//        public override bool DrawHead() => false;
//    }
//}
