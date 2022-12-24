namespace Entrogic.Core.Systems.CardSystem;

/// <summary>
/// 攻击牌<br/>
/// 攻击次数 - <see cref="GetAttackTimes"/><br/>
/// 发射射弹 - <see cref="Item.shoot"/><br/>
/// </summary>
public interface ICardAttack
{
    /// <summary>
    /// 卡牌基础攻击次数
    /// </summary>
    /// <returns>基础攻击次数</returns>
    public int GetAttackTimes();

    /// <summary>
    /// 持有手套，且卡牌为该卡牌时调用
    /// </summary>
    public void OnHold() {}
}