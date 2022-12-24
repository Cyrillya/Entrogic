namespace Entrogic.Core.BaseTypes;

public abstract class ItemBase : ModItem
{
    public override void SetStaticDefaults() {
        SacrificeTotal = 1;
    }
}