namespace Entrogic.Content.Items.Misc.Cards;

public class MagicCrystal : ItemCard, ICardAttack
{
    public int GetAttackTimes() => 30;

    public override void BasicProperties() {
        Item.damage = 10;
        Item.shootSpeed = 8f;
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type,
        int damage, float knockback) {
        return base.Shoot(player, source, position, velocity, type, damage, knockback);
    }
}