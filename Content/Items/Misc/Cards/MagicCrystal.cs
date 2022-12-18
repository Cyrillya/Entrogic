using Entrogic.Content.Projectiles.Weapons.Arcane;
using Entrogic.Core.CardSystem;

namespace Entrogic.Content.Items.Misc.Cards;

public class MagicCrystal : ItemCard, ICardAttack
{
    public int GetAttackTimes() => 30;

    public override void BasicProperties() {
        Item.damage = 10;
        Item.shootSpeed = 8f;
        Item.shoot = ModContent.ProjectileType<CrystalProj>();
        Rarity = RarityID.Beginner;
    }
}