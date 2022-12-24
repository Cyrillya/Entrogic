using static Entrogic.Core.Global.Player.SpecialHeadPlayer;

namespace Entrogic.Core.Global.Player;

public class SpecialHeadPlayer : ModPlayer
{
    public class HeadDataInfo
    {
        public Asset<Texture2D> specialHeadTexture;
        public Vector2 displyOffset;
        public int displyFrame;
    }
    public List<HeadDataInfo> armorHeadDataInfos = new();
    public List<HeadDataInfo> accHeadDataInfos = new();

    public override void ResetEffects() {
        armorHeadDataInfos.Clear();
        accHeadDataInfos.Clear();
    }

    public override void UpdateDead() {
        armorHeadDataInfos.Clear();
        accHeadDataInfos.Clear();
    }

    public override void PostUpdate() {

    }
}
public class SpecialHeadDrawLayer : PlayerDrawLayer
{
    internal class DrawDataInfo
    {
        public Vector2 Position;
        public Rectangle? Frame;
        public float Rotation;
        public Texture2D Texture;
        public Vector2 Origin;
    }

    //Returning true in this property makes this layer appear on the minimap player head icon.
    public override bool IsHeadLayer => true;

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) {
        SpecialHeadPlayer specialHeadPlayer = drawInfo.drawPlayer.GetModPlayer<SpecialHeadPlayer>();
        return specialHeadPlayer.armorHeadDataInfos.Count + specialHeadPlayer.accHeadDataInfos.Count > 0;

        // If you'd like to reference another PlayerDrawLayer's visibility,
        // you can do so by getting its instance via ModContent.GetInstance<OtherDrawLayer>(), and calling GetDefaultVisibility on it
    }

    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Head);

    internal static DrawDataInfo GetHeadDrawData(PlayerDrawSet drawInfo, HeadDataInfo headDataInfo) {
        Terraria.Player drawPlayer = drawInfo.drawPlayer;

        var displyOffset = headDataInfo.displyOffset;
        Vector2 pos = drawPlayer.headPosition + drawInfo.headVect + new Vector2(
            (int)(drawInfo.Position.X + drawPlayer.width / 2f - drawPlayer.bodyFrame.Width / 2f - Main.screenPosition.X + drawPlayer.direction * displyOffset.X),
            (int)(drawInfo.Position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height + 4f - Main.screenPosition.Y + displyOffset.Y)
        );
        // 特定帧下，position要向上2个像素格（常规的直接在帧图上表现了，像这样单图就要特殊处理）
        if ((drawPlayer.bodyFrame.Y >= 7 * 56 && drawPlayer.bodyFrame.Y <= 9 * 56) || drawPlayer.bodyFrame.Y >= 14 * 56 && drawPlayer.bodyFrame.Y <= 16 * 56) {
            pos.Y -= 2;
        }

        return new DrawDataInfo {
            Position = pos,
            Frame = drawPlayer.bodyFrame,
            Origin = drawInfo.headVect,
            Rotation = drawPlayer.headRotation,
            Texture = headDataInfo.specialHeadTexture.Value
        };
    }

    protected override void Draw(ref PlayerDrawSet drawInfo) {
        var drawPlayer = drawInfo.drawPlayer;
        var specialHeadPlayer = drawPlayer.GetModPlayer<SpecialHeadPlayer>();
        specialHeadPlayer.armorHeadDataInfos.AddRange(specialHeadPlayer.accHeadDataInfos);
        var headDataInfo = specialHeadPlayer.armorHeadDataInfos[specialHeadPlayer.armorHeadDataInfos.Count - 1];
        if (headDataInfo.specialHeadTexture == null) {
            return;
        }
        var drawDataInfo = GetHeadDrawData(drawInfo, headDataInfo);
        var effects = SpriteEffects.None;

        if (drawPlayer.direction == -1) {
            effects |= SpriteEffects.FlipHorizontally;
        }

        if (drawPlayer.gravDir == -1) {
            effects |= SpriteEffects.FlipVertically;
        }

        var data = new DrawData(
            drawDataInfo.Texture,
            drawDataInfo.Position,
            new Rectangle(0, 56 * headDataInfo.displyFrame, 40, 56),
            Lighting.GetColor(((drawDataInfo.Position + Main.screenPosition) / 16f).ToPoint()) * (1 - drawInfo.shadow),
            drawDataInfo.Rotation,
            drawDataInfo.Origin,
            1f,
            effects,
            0
        );
        // 存在饰品集合里的
        if (specialHeadPlayer.accHeadDataInfos.Contains(headDataInfo)) {
            data.shader = drawInfo.cFaceFlower;
        }
        else {
            data.shader = drawInfo.cHead;
        }
        drawInfo.DrawDataCache.Add(data);

        if (drawInfo.drawPlayer.babyBird) {
            Rectangle bodyFrame5 = drawInfo.drawPlayer.bodyFrame;
            bodyFrame5.Y = 0;
            data = new DrawData(TextureAssets.Extra[100].Value, new Vector2((int)(drawInfo.Position.X - Main.screenPosition.X - (float)(drawInfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawInfo.drawPlayer.width / 2)), (int)(drawInfo.Position.Y - Main.screenPosition.Y + (float)drawInfo.drawPlayer.height - (float)drawInfo.drawPlayer.bodyFrame.Height + 4f)) + drawInfo.drawPlayer.headPosition + drawInfo.headVect + Main.OffsetsPlayerHeadgear[drawInfo.drawPlayer.bodyFrame.Y / drawInfo.drawPlayer.bodyFrame.Height] * drawInfo.drawPlayer.gravDir, bodyFrame5, drawInfo.colorArmorHead, drawInfo.drawPlayer.headRotation, drawInfo.headVect, 1f, drawInfo.playerEffect, 0);
            drawInfo.DrawDataCache.Add(data);
        }
    }
}