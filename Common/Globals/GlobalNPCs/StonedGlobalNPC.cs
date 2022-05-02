namespace Entrogic.Common.Globals.GlobalNPCs
{
    internal class StonedGlobalNPC : GlobalNPC
    {
        //public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
        //    spriteBatch.End();
        //    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        //}
        //public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
        //    spriteBatch.End();
        //    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        //    Main.instance.GraphicsDevice.Textures[1] = ResourceManager.StonedImage.Value;
        //    ResourceManager.Stoned.Value.Parameters["uImageSize0"].SetValue(TextureAssets.Npc[npc.type].Size()); // 直接用这个的尺寸，对于一些额外的Draw贴图就会出错了...
        //    ResourceManager.Stoned.Value.Parameters["uImageSize1"].SetValue(ResourceManager.StonedImage.Size());
        //    ResourceManager.Stoned.Value.CurrentTechnique.Passes["Stoned"].Apply();
        //    return true;
        //}
    }
}
