namespace Entrogic.Content.Items.BaseTypes;

public abstract class ItemCard : ItemBase
{
    /// <summary>
    /// 卡牌稀有度ID
    /// </summary>
    public enum RarityID : short
    {
        Beginner,
        Traveler,
        Sage,
        Return,
        Satellite
    }

    /// <summary>
    /// 卡牌稀有度
    /// </summary>
    public short Rare;

    /// <summary>
    /// 点击选择该卡牌时调用
    /// </summary>
    public virtual void OnApply() {}

    /// <summary>
    /// 持有的手套正在使用该卡牌时调用
    /// </summary>
    public virtual void OnHold() {}

    /// <summary>
    /// 设置基础属性
    /// </summary>
    public virtual void BasicProperties() {}
    
    public sealed override void SetDefaults() {
        Item.width = 42;
        Item.height = 52;
        BasicProperties();
    }
}