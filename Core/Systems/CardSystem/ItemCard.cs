using Entrogic.Core.BaseTypes;

namespace Entrogic.Core.Systems.CardSystem;

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
    public RarityID Rarity;

    /// <summary>
    /// 点击选择该卡牌时调用
    /// </summary>
    public virtual void OnApply() {}

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