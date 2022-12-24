using Entrogic.Core.BaseTypes;
using Entrogic.Core.Global.Resource;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ObjectData;

namespace Entrogic.Content.Miscellaneous.Items;

public class HangingRope : QuickTileItem
{
    public HangingRope() : base("Hanging Rope", "A good rope to hang yourself", (int) ItemRarityColor.Green2, 1,
        "Entrogic/Content/Miscellaneous/Items/") {
    }

    public override int TileType() => ModContent.TileType<HangingRope_Tile>();

    public override void SafeSetDefaults() {
        Item.maxStack = 99;
        Item.value = Item.buyPrice(0, 20, 0, 0);

        base.SafeSetDefaults();
    }

    public override void AddRecipes() {
        CreateRecipe()
            .AddIngredient(ItemID.Rope, 12)
            .Register();
    }
}

internal class HangingRope_Tile : TileBase
{
    public override string HighlightTexture => $"{Texture}_Highlight";

    public override void SetStaticDefaults() {
        Main.tileFrameImportant[Type] = true;
        Main.tileNoAttach[Type] = true;
        Main.tileLavaDeath[Type] = true;
        TileID.Sets.HasOutlines[Type] = true;
        TileID.Sets.DisableSmartCursor[Type] = true;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
        TileObjectData.newTile.Width = 2;
        TileObjectData.newTile.Height = 4;
        TileObjectData.newTile.CoordinateHeights = new[] {16, 16, 16, 16};
        TileObjectData.newTile.StyleHorizontal = true;
        TileObjectData.newTile.AnchorTop =
            new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom,
                TileObjectData.newTile.Width, 0);
        TileObjectData.newTile.StyleWrapLimit = 111;
        TileObjectData.addTile(Type);

        AddMapEntry(new Color(13, 88, 130), Language.GetText("MapObject.Rope"));
    }

    public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) => true;

    public override void KillMultiTile(int i, int j, int frameX, int frameY) => Item.NewItem(
        new EntitySource_TileBreak(i, j), i * 16, j * 16, 64, 32, ModContent.ItemType<HangingRope>());

    public override bool RightClick(int i, int j) {
        Player player = Main.LocalPlayer;

        Tile tile = Main.tile[i, j];
        int topX = i - tile.TileFrameX / 18 % 2;
        int topY = j - tile.TileFrameY / 18 % 4;
        player.GetModPlayer<HangingPlayer>().StartHanging(topX, topY);

        return true;
    }

    public override void MouseOver(int i, int j) {
        Player player = Main.LocalPlayer;
        player.noThrow = 2;
        player.cursorItemIconEnabled = true;
        player.cursorItemIconID = ItemID.Rope;
    }
}

internal class HangingDrawLayer : PlayerDrawLayer
{
    private readonly Asset<Texture2D> tex = ModContent.Request<Texture2D>(TextureManager.Images + "Hanging");

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) {
        HangingPlayer hangingPlayer = drawInfo.drawPlayer.GetModPlayer<HangingPlayer>();
        return hangingPlayer.Hanging;

        // If you'd like to reference another PlayerDrawLayer's visibility,
        // you can do so by getting its instance via ModContent.GetInstance<OtherDrawLayer>(), and calling GetDefaultVisibility on it
    }

    public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.Head);

    protected override void Draw(ref PlayerDrawSet drawInfo) {
        var drawPlayer = drawInfo.drawPlayer;
        var hangingPlayer = drawPlayer.GetModPlayer<HangingPlayer>();

        var data = new DrawData(
            tex.Value,
            hangingPlayer.HangingRingPos * 16f - Main.screenPosition,
            null,
            Lighting.GetColor(hangingPlayer.HangingRingPos.ToPoint()),
            0f,
            Vector2.Zero,
            1f,
            SpriteEffects.None,
            0
        );
        drawInfo.DrawDataCache.Add(data);
    }
}

internal class HangingPlayer : ModPlayer
{
    internal bool Hanging;
    internal int HangingTimer;
    internal Vector2 HangingPos;
    internal Vector2 HangingRingPos;

    public void StartHanging(int x, int y) {
        Player.StopVanityActions();
        Player.RemoveAllGrapplingHooks();
        Player.RemoveAllFishingBobbers();
        if (Player.mount.Active)
            Player.mount.Dismount(Player);

        Player.position = new Vector2(x + 0.4f, y + 2.5f) * 16f;
        Player.velocity = Vector2.Zero;
        Player.gravDir = 1f;
        Hanging = true;
        HangingPos = new Vector2(x, y);
        HangingRingPos = new Vector2(x, y + 2f);
        if (Main.myPlayer == Player.whoAmI) ; // 联机同步以后补上
        //NetMessage.SendData(13, -1, -1, null, player.whoAmI);
    }

    public override void UpdateDead() {
        base.UpdateDead();
        Hanging = false;
        HangingTimer = 0;
    }

    public override bool CanUseItem(Item item) {
        return !Hanging;
    }

    public override bool PreItemCheck() {
        if (Hanging) {
            var t = Framing.GetTileSafely(HangingPos.ToPoint());
            if (t == null || !t.HasUnactuatedTile || t.TileType != ModContent.TileType<HangingRope_Tile>()) {
                UpdateDead();
                return base.PreItemCheck();
            }

            Player.controlJump = false;
            Player.controlDown = false;
            Player.controlLeft = false;
            Player.controlRight = false;
            Player.controlUp = false;
            Player.controlUseItem = false;
            Player.controlUseTile = false;
            Player.controlThrow = false;
            Player.gravDir = 1f;
            Player.velocity = Vector2.Zero;
            Player.StopVanityActions();
            Player.RemoveAllGrapplingHooks();
            Player.RemoveAllFishingBobbers();
            if (Player.mount.Active)
                Player.mount.Dismount(Player);

            HangingTimer++;
            if (HangingTimer > 1)
                Player.position = Player.oldPosition;

            Player.lifeRegen = 0;
            Player.lifeRegenCount = 0;
            if (HangingTimer % 5 == 0) {
                Player.statLife -= 5;
                CombatText.NewText(
                    new Rectangle((int) Player.position.X, (int) Player.position.Y, Player.width, Player.height),
                    CombatText.LifeRegen, 5, dramatic: false, dot: true);
                if (Player.statLife <= 0 && Player.whoAmI == Main.myPlayer) {
                    Player.KillMe(PlayerDeathReason.ByCustomReason($"{Player.name}将自己绞死了。"), 10.0, 0);
                }

                if (Player.creativeGodMode) {
                    Main.NewText("[c/FF0000:你试图在无敌模式绞死自己，可惜这并不可能]");
                    UpdateDead();
                }
            }

            Player.bodyFrame = new Rectangle(0, 0, 40, 56);
            Player.legFrame = new Rectangle(0, 0, 40, 56);
            Player.eyeHelper.BlinkBecausePlayerGotHurt();
        }

        return base.PreItemCheck();
    }
}