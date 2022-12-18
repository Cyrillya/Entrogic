namespace Entrogic.Core.CardSystem;

public abstract class ItemGlove : ItemBase
{
    internal void SetGloveDefaults(int singleShotTime, float shotVelocity, bool hasAutoReuse = true) {
        Item.DefaultToMagicWeapon(10, singleShotTime, shotVelocity, hasAutoReuse);
        Item.DamageType = ModContent.GetInstance<ArcaneDamageClass>();
    }
    
    public override bool CanUseItem(Player player) {
        return CardModPlayer.Get(player).IsAttackAvailable;
    }

    public override void HoldItem(Player player) {
        var modPlayer = CardModPlayer.Get(player);

        if (!modPlayer.HasAttackCard || modPlayer.AttackCard.ModItem is not ICardAttack cardAttack) return;

        cardAttack.OnHold();

        // 也许用 HoldItem?
        // ItemLoader.HoldItem(card, player);
    }

    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type,
        ref int damage, ref float knockback) {
        var card = CardModPlayer.Get(player).AttackCard;

        // 速度
        float speed = velocity.Length();
        velocity = velocity.SafeNormalize(Vector2.One);
        velocity *= speed * card.shootSpeed;

        // 伤害
        var damageModifier = player.GetTotalDamage(Item.DamageType);
        damage = (int) (damageModifier.ApplyTo(Item.damage + card.damage) + 5E-06f);
        
        // 击退
        knockback = player.GetWeaponKnockback(Item, Item.knockBack + card.knockBack);

        // 射弹类型
        type = card.shoot;

        CombinedHooks.ModifyShootStats(player, card, ref position, ref velocity, ref type, ref damage, ref knockback);

        // base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity,
        int type, int damage, float knockback) {
        var modPlayer = CardModPlayer.Get(player);
        var card = modPlayer.AttackCard;
        modPlayer.ReduceAttackTimes();
        return CombinedHooks.Shoot(player, card, source, position, velocity, type, damage, knockback);

        // return base.Shoot(player, source, position, velocity, type, damage, knockback);
    }
}