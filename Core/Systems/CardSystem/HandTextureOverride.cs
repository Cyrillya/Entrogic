namespace Entrogic.Core.Systems.CardSystem;

public class HandTextureOverride : ModPlayer
{
    public override void UpdateVisibleVanityAccessories() {
        var item = Player.HeldItem;
        if (item.ModItem is IOverrideHand) {
            Player.UpdateVisibleAccessory(0, item);
        }
    }
}