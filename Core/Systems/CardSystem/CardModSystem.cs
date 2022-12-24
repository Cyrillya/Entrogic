using Entrogic.Content.Miscellaneous.Cards;
using Microsoft.Xna.Framework.Input;

namespace Entrogic.Core.Systems.CardSystem;

public class CardModSystem : ModSystem
{
    private const float RerollCooldown = 5f;
    internal static float RerollTimer;

    /// <summary> 备选卡牌 </summary>
    public static Item[] AlternativeCards = new Item[6];

    public override void PostUpdateInput() {
        if (Main.keyState.IsKeyDown(Keys.X)) {
            var item = new Item(ModContent.ItemType<MagicCrystal>());
            for (int i = 0; i < 6; i++) {
                AlternativeCards[i] = item.Clone();
            }
        }
    }

    public override void PostUpdatePlayers() {
        if (Main.netMode is NetmodeID.Server) return;

        if (RerollTimer <= 0f) return;

        RerollTimer -= 1f / 60f;
        if (RerollTimer <= 0f) {
            RerollTimer = 0f;
        }
    }

    public static void ResetRerollTimer() {
        RerollTimer = RerollCooldown;
    }

    public static void ClearCard(int index) {
        AlternativeCards[index] = new Item();
    }
}