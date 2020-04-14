using Entrogic.Items.Weapons.Card.Elements;

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Card.Gloves
{
    public class MagicGatling : ModCard
    {
        public override void PreCreated()
        {
            glove = true;
        }
        public override void CardDefaults()
        {
            base.CardDefaults();
            item.value = Item.buyPrice(0, 5);
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.shootSpeed = 14f;
            item.useTime = item.useAnimation = 6;
            item.autoReuse = true;
            item.damage = 4;
            item.crit -= 20;
            item.scale = 1f;
            item.noUseGraphic = false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-12f, -6f);
        }
        public override void HoldItem(Player player)
        {
            EntrogicPlayer ePlayer = player.GetModPlayer<EntrogicPlayer>();
            CardBuffPlayer cPlayer = player.GetModPlayer<CardBuffPlayer>();
            ePlayer.ManaMax--;
            ePlayer.CardHandMax--;
        }
        public override void OnGloveUseCard(Player player)
        {
            EntrogicPlayer ePlayer = player.GetModPlayer<EntrogicPlayer>();
            for (int i = 0; i < ePlayer.CardHandType.Length; i++)
            {
                if (ePlayer.CardHandType[i] == 0)
                {
                    Item item = new Item();
                    item.SetDefaults(ItemType<ArcaneMissle>());
                    ModCard card = (ModCard)item.modItem;
                    ePlayer.CardHandType[i] = ItemType<ArcaneMissle>();
                    ePlayer.CardHandCost[i] = card.costMana;
                    if (Main.hardMode)
                    {
                        item.SetDefaults(ItemType<ArcaneMissle_Electromagnetism>());
                        card = (ModCard)item.modItem;
                        ePlayer.CardHandType[i] = ItemType<ArcaneMissle_Electromagnetism>();
                        ePlayer.CardHandCost[i] = card.costMana;
                    }
                    break;
                }
            }
        }
        public override bool ModifyGloveShootCard(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            damage = (int)(damage * 0.80f);
            position += (Main.MouseWorld - player.Center).ToRotation().ToRotationVector2() * 3.6f * 16f;
            return true;
        }
        public override int ShootCardTimes(Player player)
        {
            return Main.rand.Next(1, 2 + 1);
        }
        public override float AttackCardRemainingTimesReduce(Player player)
        {
            return Main.rand.NextBool(2) ? 1 : 0;
        }
    }
}
