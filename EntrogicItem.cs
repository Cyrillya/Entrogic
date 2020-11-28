using Entrogic.Items.AntaGolem;
using Entrogic.Items.PollutElement;
using Entrogic.Items.VoluGels;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ModLoader.IO;
using Entrogic.Items.Weapons.Card;
using Entrogic.UI.Cards;
using Entrogic.Items.Weapons.Card.Undeads;
using Entrogic.Items.Weapons.Card.Nones;
using Entrogic.Items.Weapons.Card.Organisms;

namespace Entrogic
{
    public class EntrogicItem : GlobalItem
    {
        public bool book = false;
        public bool card = false;
        public bool glove = false;
        public bool GloveAttackWhileNoCard = false;
        public int cardID = 0;
        public float cardProb = 0;
        public bool ammoCost85 { get { return EntrogicPlayer.CanAmmoCost85; } }
        public bool ammoCost90 { get { return EntrogicPlayer.CanAmmoCost90; } }
        public override void SetDefaults(Item item)
        {
            if (item.type == ItemID.BoneSword) item.damage = 26;
            if (item.type == ItemID.Bone) item.damage += 5;
            if (item.type == ItemID.WandofSparking) { item.damage += 4; item.reuseDelay = 44; item.useTime = 5; item.mana = 5; }
            if (item.type == ItemID.IceBlock) item.ammo = item.type;
            if (item.type == ItemID.IceRod) { item.mana /= 2; item.useTime /= 2; item.damage += 12; }
            if (item.type == ItemID.SpaceGun) { item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/SpaceGun1.4"); }
        }
        public override void OpenVanillaBag(string context, Player player, int arg)
        {
            if (context == "bossBag")
            {
                if (arg == ItemID.SkeletronBossBag)
                    player.QuickSpawnItem(ItemType<TheWalkingDead_Strong>());
                if (IsMechanicalBossBag(arg) && Main.rand.Next(1, 11) <= 3) // 30%
                    player.QuickSpawnItem(ItemType<PerpetuumMobileoftheFifthKind>());
                if (arg == ItemID.WallOfFleshBossBag && Main.rand.Next(1, 11) <= 3) // 30%
                    player.QuickSpawnItem(ItemType<Worldflipper>());
                if (arg == ItemID.KingSlimeBossBag && Main.rand.Next(1, 101) <= 45) // 45%
                    player.QuickSpawnItem(ItemType<GelofMimicry>());
                if (arg == ItemID.EyeOfCthulhuBossBag && Main.rand.Next(1, 11) <= 4) // 40%
                    player.QuickSpawnItem(ItemType<Foresight>());
            }
        }
        public bool IsMechanicalBossBag(int arg)
        {
            return arg == ItemID.SkeletronPrimeBossBag || arg == ItemID.TwinsBossBag || arg == ItemID.DestroyerBossBag;
        }
        public override bool ConsumeAmmo(Item item, Player player)
        {
            bool flag = false;
            if (ammoCost85 && Main.rand.NextFloat() < .15f)
                flag = true;
            else if (ammoCost90 && Main.rand.NextFloat() < .10f)
                flag = true;
            return !flag;
        }
        public override void HoldItem(Item item, Player player)
        {
            EntrogicPlayer ePlayer = player.GetModPlayer<EntrogicPlayer>();
            ePlayer.PickPowerHand = item.pick;
            if (glove && !Main.dedServ)
                CardUI.IsActive = true;
        }
        public override bool Shoot(Item item, Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (item.type == ItemID.Torch)
            {
                Vector2 vec = ModHelper.GetFromToVector(position, Main.MouseWorld);
                position -= vec * 48;
            }
            return base.Shoot(item, player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }
        public override void PostUpdate(Item item)
        {
            
        }
        public override bool CanUseItem(Item item, Player player)
        {
            if (!glove || GloveAttackWhileNoCard || player.GetModPlayer<CardBuffPlayer>().attackCardRemainingTimes > 0)
                return true;
            return false;
        }
        public override bool UseItem(Item item, Player player)
        {
            EntrogicPlayer ePlayer = player.GetModPlayer<EntrogicPlayer>();
            return base.UseItem(item, player);
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (AEntrogicConfigClient.Instance.ShowUsefulInformations)
            {
                TooltipLine line = new TooltipLine(mod, mod.Name, $"物品ID：{item.type}\n最大堆叠数：{item.maxStack}")
                {
                    overrideColor = Color.Gray
                };
                tooltips.Add(line);
                if (item.type >= 3930 && item.modItem != null)
                {
                    line = new TooltipLine(mod, mod.Name, $"物品内部名：{item.modItem.mod.Name}:{item.modItem.Name}")
                    {
                        overrideColor = Color.Gray
                    };
                    tooltips.Add(line);
                }
            }
            List<int> presents = new List<int>
            {
                ItemType<VolutioTreasureBag>(),
                ItemType<ContaminatedElementalTreasureBag>(),
                ItemType<AthanasyTreasureBag>(),
                ItemType<NoviceCardPack>()
            };
            List<string> textPres = new List<string>
            {
                "<Volutio 瓦卢提奥>",
                "<Pollution Elemental 污染之灵>",
                "<Athanasy 安达尼希>",
                "<Novice Card Pack 新手卡包>"
            };
            foreach (int t in presents)
                if (item.type == t)
                {
                    EntrogicPlayer mPlayer = Main.LocalPlayer.GetModPlayer<EntrogicPlayer>();
                    string sr = mPlayer.colorDecR.ToString("X2");
                    string sg = mPlayer.colorDecG.ToString("X2");
                    string sb = mPlayer.colorDecB.ToString("X2");
                    TooltipLine line = new TooltipLine(mod, mod.Name, "[C/" + sr + sg + sb + ":" + textPres[presents.IndexOf(t)] + "]");
                    tooltips.Add(line);
                }

            // 跟收藏Tooltip说再见
            tooltips.RemoveAll((TooltipLine line) => line.mod == "Terraria" && line.Name.StartsWith("Favorite"));
        }
        public override bool InstancePerEntity => true;
        public override bool CloneNewInstances => true;
    }
}
