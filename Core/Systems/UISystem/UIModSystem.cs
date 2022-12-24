using Entrogic.Interfaces.GUI;
using Terraria.UI;

namespace Entrogic.Core.Systems.UISystem;

/// <summary>
/// 用户界面
/// </summary>
public class UIModSystem : ModSystem
{
    internal static UIModSystem Instance;

    #region 定义

    internal BookGUI BookUI;
    internal static UserInterface BookInterface;
    internal CardDashboardGUI CardDashboardUI;
    internal static UserInterface CardDashboardInterface;

    #endregion

    #region 卸载 & 加载

    public override void Load() {
        Instance = this;

        if (Main.dedServ) return;

        BookUI = new BookGUI();
        CardDashboardUI = new CardDashboardGUI();

        LoadGUI(ref BookUI, out BookInterface);
        LoadGUI(ref CardDashboardUI, out CardDashboardInterface);
    }

    public override void Unload() {
        Instance = null;

        if (Main.dedServ) return;

        BookUI?.Unload();
        BookUI = null;
        BookInterface = null;
            
        CardDashboardUI = null;
        CardDashboardInterface = null;
    }

    public static void LoadGUI<T>(ref T uiState, out UserInterface uiInterface, Action preActive = null)
        where T : UIState {
        uiInterface = new UserInterface();
        preActive?.Invoke();
        uiState.Activate();
        uiInterface.SetState(uiState);
    }

    #endregion

    #region 更新

    public override void UpdateUI(GameTime gameTime) {
        if (BookGUI.Visible) {
            BookInterface?.Update(gameTime);
        }
        if (CardDashboardGUI.Visible) {
            CardDashboardInterface?.Update(gameTime);
        }
    }

    #endregion

    #region 绘制

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
        int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));

        if (mouseTextIndex != -1) {
            layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                "Entrogic: Book UI",
                delegate {
                    if (BookGUI.Visible) {
                        BookInterface.Draw(Main.spriteBatch, new GameTime());
                    }

                    return true;
                },
                InterfaceScaleType.UI)
            );
            layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                "Entrogic: Card Dashboard UI",
                delegate {
                    if (CardDashboardGUI.Visible) {
                        CardDashboardInterface.Draw(Main.spriteBatch, new GameTime());
                    }

                    return true;
                },
                InterfaceScaleType.UI)
            );
        }
    }

    #endregion
}