using Entrogic.Projectiles.Arcane;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Entrogic.Items.Weapons.Card;

namespace Entrogic
{
    public class CardBuffPlayer : ModPlayer
    {
        public int minionBuffType = 0;
        public float attackCardRemainingTimes = 0;
        public float attackCardRemainingTimesMax = 0;

        public LegacySoundStyle itemSound = null;
        public ModCard mCard = null;
        public Item card = null;
        public override bool Shoot(Item item, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            EntrogicItem eItem = item.GetGlobalItem<EntrogicItem>();
            EntrogicPlayer ePlayer = EntrogicPlayer.ModPlayer(player);
            if (eItem.glove && attackCardRemainingTimes > 0 && mCard != null && card != null)
            {
                Vector2 vec = (Main.MouseWorld - player.Center).ToRotation().ToRotationVector2() * (card.shootSpeed + item.shootSpeed);
                int Tdamage = player.GetWeaponDamage(card) + damage;
                float TknockBack = player.GetWeaponKnockback(card, card.knockBack) + knockBack;
                int Tshoot = card.shoot;
                if (((ModCard)item.modItem).ModifyGloveShootCard(player, ref position, ref vec.X, ref vec.Y, ref Tshoot, ref Tdamage, ref TknockBack))
                {
                    for (int i = 0; i < ((ModCard)item.modItem).ShootCardTimes(player); i++)
                    {
                        mCard.AttackEffects(player, Tshoot, position, Main.MouseWorld, vec.X, vec.Y, Tdamage, TknockBack, card.shootSpeed + item.shootSpeed);
                        attackCardRemainingTimes -= ((ModCard)item.modItem).AttackCardRemainingTimesReduce(player);
                        if (attackCardRemainingTimes <= 0 && CEntrogicCardConfig.Instance.AutoUseCard)
                        {
                            for (int index = 0; index < ePlayer.CardHandType.Length; index++)
                            {
                                int handCard = ePlayer.CardHandType[index];
                                if (handCard == card.type)
                                {
                                    Entrogic.Instance.CardUI.Grid[index].UseThisCard();
                                    return false;
                                }
                            }
                        }
                        if (itemSound != null)
                            SoundEngine.PlaySound(itemSound, position);
                    }
                }
                return false;
            }
            return true;
        }
    }
}