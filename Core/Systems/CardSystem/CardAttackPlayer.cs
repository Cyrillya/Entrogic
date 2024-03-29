﻿namespace Entrogic.Core.Systems.CardSystem;

public class CardAttackPlayer : ModPlayer
{
    public bool IsAttackAvailable => HasAttackCard && CurAttackTimesLeft > 0;
    public bool HasAttackCard => AttackCard is {ModItem: ICardAttack, IsAir: false};
    public Item AttackCard { get; private set; }
    public int CurAttackTimesMax;
    public int CurAttackTimesLeft;

    public static CardAttackPlayer Get(Player player) {
        return player.GetModPlayer<CardAttackPlayer>();
    }

    public void ReduceAttackTimes() {
        CurAttackTimesLeft--;
    }
    
    public void SetAttackTimes(int times) {
        CurAttackTimesMax = times;
        CurAttackTimesLeft = times;
    }

    public void SetCurrentAttackCard(Item item) {
        AttackCard = item;
    }
}