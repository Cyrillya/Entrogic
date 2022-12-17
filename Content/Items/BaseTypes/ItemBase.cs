namespace Entrogic.Content.Items.BaseTypes;

public abstract class ItemBase : ModItem
{
    public bool IsGlove;

    public override void SetStaticDefaults() {
        SacrificeTotal = 1;
    }
}