using Entrogic.Items.Weapons.Card;

using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ModLoader;

namespace Entrogic.NPCs.CardFightable
{
    public abstract class CardFightableNPC : ModNPC
    {
        internal int CardGameHealth;
        internal int CardGameHealthMax;
        internal float RoundDuration;
        internal bool CardGaming;
        internal Vector2 PanelPosition;
        protected Vector2 PlaygroundPos => new Vector2(114f, 86f);
        protected Vector2 PlaygroundSize => new Vector2(358f, 220f);
        public sealed override void SetStaticDefaults()
        {
            SetupDefaults();
            base.SetStaticDefaults();
        }

        public virtual void SetupDefaults() { }
        public virtual void SetupContents(ref string ImgPath, ref Vector2 ImgPosition) { }
        public virtual void StartAttacking()
        {
            Entrogic.Instance.CardGameUI.StartGame();
            if (State == 0)
            {
                Initialize();
                State = 1;
            }
        }
        #region 有限状态机相关 - 攻击AI
        public ModCard currentCard => cardStates[State - 1];
        private List<ModCard> cardStates = new List<ModCard>();
        private Dictionary<string, int> stateDict = new Dictionary<string, int>();
        private int State;
        /// <summary>
        /// 把当前状态变为指定的弹幕状态实例
        /// </summary>
        /// <typeparam name="T">注册过的<see cref="ModCard"/>类名</typeparam>
        public void SetState<T>() where T : ModCard
        {
            var name = typeof(T).FullName;
            if (!stateDict.ContainsKey(name)) throw new ArgumentException("这个状态并不存在");
            State = stateDict[name];
            currentCard.PreStartGaming(ref RoundDuration);
        }
        /// <summary>
        /// 注册状态
        /// </summary>
        /// <typeparam name="T">需要注册的<see cref="ModCard"/>类</typeparam>
        /// <param name="state">需要注册的<see cref="ModCard"/>类的实例</param>
        protected void RegisterState<T>(T state) where T : ModCard
        {
            var name = typeof(T).FullName;
            if (stateDict.ContainsKey(name)) throw new ArgumentException("这个状态已经注册过了");
            cardStates.Add(state);
            stateDict.Add(name, cardStates.Count);
        }

        /// <summary>
        /// 初始化函数，用于注册弹幕状态
        /// </summary>
        public abstract void Initialize();
        /// <summary>
        /// 用before和after函数添加附加信息
        /// </summary>
        public void GameAI()
        {
            GameAIBefore();
            currentCard.GameAI(RoundDuration, PanelPosition);
            CardGameHealth = Math.Min(CardGameHealth, CardGameHealthMax);
            CardGameHealth = Math.Max(CardGameHealth, 0);
            GameAIAfter();
        }
        /// <summary>
        /// 在状态机执行之前要执行的代码，可以重写
        /// </summary>
        public virtual void GameAIAfter() { }
        /// <summary>
        /// 在状态机执行之后要执行的代码，可以重写
        /// </summary>
        public virtual void GameAIBefore() { }
        #endregion
        public virtual void EndAttacking() { }
        public virtual void PreStartRound(bool playerTurn) { }
        public virtual void CardGameWin() { }
        public virtual void CardGameLost() { }
        public sealed override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (firstButton)
                OnFirstButtonClicked(ref shop);
            if (!firstButton)
                OnSecondButtonClicked(ref shop);
            base.OnChatButtonClicked(firstButton, ref shop);
        }
        public virtual void OnFirstButtonClicked(ref bool shop) { }
        public virtual void OnSecondButtonClicked(ref bool shop) { }
    }
}
