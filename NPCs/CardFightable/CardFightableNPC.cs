using Microsoft.Xna.Framework;

using System;

using Terraria;
using Terraria.ModLoader;

namespace Entrogic.NPCs.CardFightable
{
    public abstract class CardFightableNPC : ModNPC
    {
        internal bool CloseChatNextFrame;
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
            Player clientPlayer = Main.LocalPlayer;
            EntrogicPlayer clientModPlayer = EntrogicPlayer.ModPlayer(clientPlayer);
            clientModPlayer.CardGamePlayerHealth = clientModPlayer.CardGamePlayerMaxHealth;
            clientModPlayer.CardGamePlayerLastHealth = clientModPlayer.CardGamePlayerHealth;
            CardGameHealth = CardGameHealthMax;
        }
        public virtual void OnAttacking() 
        {
            CardGameHealth = Math.Min(CardGameHealth, CardGameHealthMax);
            CardGameHealth = Math.Max(CardGameHealth, 0);
        }
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
