using Entrogic.Core.Systems.CardSystem;
using Entrogic.Interfaces.UIElements;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Entrogic.Interfaces.GUI;

public class CardDashboardGUI : UIState
{
    /// <summary>
    /// 选择卡牌的按钮
    /// </summary>
    private class SelectionSlot : UIElement
    {
        private Player Player => Main.LocalPlayer;
        private CardAttackPlayer AttackPlayer => CardAttackPlayer.Get(Player);
        private Item Item => CardModSystem.AlternativeCards[_index];
        private bool HasCard => Item != null;

        private readonly int _index;

        public SelectionSlot(int index) {
            _index = index;
            Width = new StyleDimension(42f, 0f);
            Height = new StyleDimension(52f, 0f);
        }

        // 点击选择卡牌
        public override void Click(UIMouseEvent evt) {
            if (!HasCard || Item.ModItem is not ItemCard itemCard) return;

            itemCard.OnApply();
            // 次数卡选择后设置次数
            if (itemCard is ICardAttack cardAttack) {
                AttackPlayer.SetCurrentAttackCard(Item.Clone());
                AttackPlayer.SetAttackTimes(cardAttack.GetAttackTimes());
            }

            CardModSystem.ClearCard(_index);
        }

        // 绘制卡牌
        protected override void DrawSelf(SpriteBatch spriteBatch) {
            if (!HasCard || Item.ModItem is not ItemCard) return;

            Main.instance.LoadItem(Item.type);
            var position = GetDimensions().Position();
            float opacity = IsMouseHovering ? 1f : 0.5f;
            var color = Color.White * opacity;
            spriteBatch.Draw(TextureAssets.Item[Item.type].Value, position, color);
        }
    }

    /// <summary>
    /// 抽牌图像 UI
    /// </summary>
    private class RollImage : UIImage
    {
        internal RollImage(Asset<Texture2D> texture) : base(texture) {
        }

        // 更新颜色
        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            if (CardModSystem.RerollTimer > 0) {
                Color = Color.Black;
            }
        }

        // 绘制文字
        protected override void DrawSelf(SpriteBatch spriteBatch) {
            base.DrawSelf(spriteBatch);

            if (CardModSystem.RerollTimer <= 0) return;

            var dimensions = GetDimensions();
            var center = dimensions.Center();
            var font = FontAssets.ItemStack.Value;

            // 将 RerollTimer 转换为保留一位小数字符串
            var text = CardModSystem.RerollTimer.ToString("0.0");
            var textSize = font.MeasureString(text);

            // 绘制文字
            ChatManager.DrawColorCodedString(spriteBatch, font, text, center, Color.White, 0f, textSize / 2f,
                Vector2.One);
        }
    }

    internal static bool Visible = true;
    private UIPanel _panel;
    private RollImage _rollImage;
    private ProgressBarVertical _progressBar;

    private Player Player => Main.LocalPlayer;
    private CardAttackPlayer AttackPlayer => CardAttackPlayer.Get(Player);

    public override void OnInitialize() {
        _panel = new UIPanel {
            Top = new StyleDimension(-180f, 1f),
            Width = new StyleDimension(456f, 0f),
            Height = new StyleDimension(120f, 0f),
            BackgroundColor = Color.Transparent,
            BorderColor = Color.Transparent,
            HAlign = 0.5f,
            PaddingBottom = 0f,
            PaddingLeft = 0f,
            PaddingRight = 0f,
            PaddingTop = 0f
        };
        Append(_panel);

        _rollImage = new RollImage(RequestTexture("Pass")) {
            Top = new StyleDimension(0f, 0f),
            Left = new StyleDimension(0f, 0f),
            IgnoresMouseInteraction = true,
            VAlign = 0.5f
        };
        _panel.Append(_rollImage);

        _progressBar = new ProgressBarVertical(RequestTextureValue("ProgressBarEmpty"),
            RequestTextureValue("ProgressBarFilled"), GetAttackTimesPercent) {
            IgnoresMouseInteraction = true,
            HAlign = 0.5f
        };
        _panel.Append(_progressBar);

        var insidePanel = new UIPanel {
            Width = new StyleDimension(420f, 0f),
            Height = new StyleDimension(60f, 0f),
            BackgroundColor = Color.Transparent,
            BorderColor = Color.Transparent,
            HAlign = 0.5f,
            VAlign = 0.5f,
            PaddingBottom = 0f,
            PaddingLeft = 0f,
            PaddingRight = 0f,
            PaddingTop = 0f
        };
        _panel.Append(insidePanel);

        // 单个卡槽的 Align 量
        float dotAlign = 1f / 6f;
        for (int i = 1; i <= 6; i++) {
            float align = dotAlign * i - dotAlign * 0.5f;
            var slot = new SelectionSlot(i - 1) {
                HAlign = align
            };
            insidePanel.Append(slot);
        }
    }

    private float GetAttackTimesPercent() {
        if (AttackPlayer.CurAttackTimesMax <= 0) return 0f;
        return AttackPlayer.CurAttackTimesLeft / (float) AttackPlayer.CurAttackTimesMax;
    }

    private static Texture2D RequestTextureValue(string imageName) {
        return RequestTexture(imageName).Value;
    }

    private static Asset<Texture2D> RequestTexture(string imageName) {
        return ModContent.Request<Texture2D>($"Entrogic/Assets/Images/CardGUI/{imageName}",
            AssetRequestMode.ImmediateLoad);
    }
}