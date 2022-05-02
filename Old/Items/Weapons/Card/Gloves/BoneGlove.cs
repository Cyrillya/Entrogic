
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Card.Gloves
{
    public class BoneGlove : ModCard
    {
        public override void PreCreated()
        {
            glove = true;
            GloveAttackWhileNoCard = true;
        }
        public override void CardDefaults()
        {
            item.shootSpeed = 8f;
            item.useTime = item.useAnimation = 15;
            item.autoReuse = true;
            item.shoot = ProjectileID.BoneGloveProj;
            item.damage = 4;
        }
        public override void HoldItem(Player player)
        {
            EntrogicPlayer ePlayer = player.GetModPlayer<EntrogicPlayer>();
            for (int i = 0; i < ePlayer.CardType.Length; i++)
            {
                try
                {
                    if (ePlayer.CardType[i] != 0)
                    {
                        Item Item = new Item();
                        Item.SetDefaults(ePlayer.CardType[i]);
                        if (((ModCard)Item.modItem).series != CardSeriesID.Undead)
                            return;
                    }
                }
                catch
                {
                    return;
                }
            }
            ePlayer.CardHandMax++;
            ePlayer.ManaMax++;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            damage += 10;
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }
    }
}
