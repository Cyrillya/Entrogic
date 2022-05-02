using System.Linq;

namespace Entrogic.Content.NPCs
{
    internal class Tester : ModNPC
    {
        public static RenderTarget2D blackRender;
        //public override void Load() {
        //    base.Load();
        //    Main.QueueMainThreadAction(delegate {
        //        blackRender = new RenderTarget2D(Main.instance.GraphicsDevice, Main.screenWidth, Main.screenHeight);
        //    });
        //    On.Terraria.Main.DrawBlack += Main_DrawBlack;
        //    On.Terraria.Main.RenderBlack += Main_RenderBlack;
        //    On.Terraria.Main.RenderWalls += Main_RenderWalls;
        //    On.Terraria.Main.DoDraw_Tiles_Solid += Main_DoDraw_Tiles_Solid;
        //    Main.OnResolutionChanged += Main_OnResolutionChanged;
        //}

        //private void Main_RenderBlack(On.Terraria.Main.orig_RenderBlack orig, Main self) {
        //    orig.Invoke(self);
        //}

        //private void Main_OnResolutionChanged(Vector2 obj) {
        //    if (!Main.dedServ) {
        //        blackRender = new RenderTarget2D(Main.instance.GraphicsDevice, Main.screenWidth, Main.screenHeight);
        //    }
        //}

        //private void Main_DoDraw_Tiles_Solid(On.Terraria.Main.orig_DoDraw_Tiles_Solid orig, Main self) {

        //    //GraphicsDevice graphicsDevice = Main.graphics.GraphicsDevice;//获取GraphicsDevice
        //    //graphicsDevice.SetRenderTarget(blackRender);//把绘制目标设置为自己创建的弹幕render

        //    //Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.Transform);

        //    //foreach (var npc in from n in Main.npc where n.active && n.type == Type select n) {
        //    //    Vector2 value = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange);
        //    //    Texture2D t = TextureAssets.Npc[npc.type].Value;
        //    //    //在自定义的render上绘制npc
        //    //    SpriteEffects spriteEffects = SpriteEffects.None;
        //    //    if (npc.spriteDirection == 1) {
        //    //        spriteEffects = SpriteEffects.FlipHorizontally;
        //    //    }
        //    //    float extraDrawY = Main.NPCAddHeight(npc);
        //    //    Vector2 origin = new Vector2(t.Width / 2, t.Height / Main.npcFrameCount[npc.type] / 2);
        //    //    var pos = new Vector2(npc.position.X - Main.screenPosition.X + npc.width / 2 - (float)t.Width * npc.scale / 2f + origin.X * npc.scale,
        //    //        npc.position.Y - Main.screenPosition.Y + npc.height - t.Height * npc.scale / Main.npcFrameCount[npc.type] + 4f + extraDrawY + origin.Y * npc.scale + npc.gfxOffY);
        //    //    pos += value;
        //    //    Main.EntitySpriteDraw(t,
        //    //        pos,
        //    //        npc.frame,
        //    //        Color.White, npc.rotation, origin, npc.scale, spriteEffects, 0);
        //    //    if (npc.color != default(Color)) {
        //    //        Main.EntitySpriteDraw(t, pos, npc.frame, Color.White, npc.rotation, origin, npc.scale, spriteEffects, 0);
        //    //    }
        //    //}
        //    //// 重设RenderTarget
        //    //graphicsDevice.SetRenderTarget(Main.screenTarget);
        //    //Main.spriteBatch.End();
        //    //Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.Transform);

        //    //var shader = ResourceManager.CoverRenderer.Value;
        //    //shader.Parameters["uImageSize"].SetValue(new Vector2(Main.screenWidth, Main.screenHeight));
        //    //Main.instance.GraphicsDevice.Textures[1] = Main.instance.blackTarget; // 背景图
        //    //shader.CurrentTechnique.Passes["CoverRenderer"].Apply(); // 然后开启Shader
        //    //Main.spriteBatch.Draw(blackRender, Vector2.Zero, Color.White); // 绘制自定义的弹幕render

        //    //// 重设shader
        //    //Main.spriteBatch.End();

        //    //graphicsDevice.Clear(Color.Transparent);
        //    //graphicsDevice.SetRenderTarget(Main.screenTarget);
        //    orig.Invoke(self);
        //}

        //// 在Wall绘制后，非solid物块绘制前插入魔像
        //private void Main_RenderWalls(On.Terraria.Main.orig_RenderWalls orig, Main self) {
        //    orig.Invoke(self);

        //}

        //private void Main_DrawBlack(On.Terraria.Main.orig_DrawBlack orig, Main self, bool force) {

        //    orig.Invoke(self, force);
        //}

        public override void AI() {
            base.AI();
        }

        public override void SetDefaults() {
            base.SetDefaults();
            NPC.aiStyle = -1;
            NPC.lifeMax = 100;
            NPC.damage = 30;
            NPC.defense = 10;
            NPC.knockBackResist = 0f;
            NPC.width = 140;
            NPC.height = 120;
            NPC.value = Item.buyPrice(0, 40, 0, 0);
            NPC.npcSlots = 100f;
            NPC.dontTakeDamage = true;
            NPC.friendly = false;
            NPC.boss = true;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.dontTakeDamageFromHostiles = true;
            NPC.behindTiles = true;
        }

        public override Color? GetAlpha(Color drawColor) {
            return Color.White;
        }

        public override void DrawBehind(int index) {
            base.DrawBehind(index);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
            base.PostDraw(spriteBatch, screenPos, drawColor);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
            //spriteBatch.End();

            //GraphicsDevice graphicsDevice = Main.graphics.GraphicsDevice;//获取GraphicsDevice
            //graphicsDevice.SetRenderTarget(blackRender);//把绘制目标设置为自己创建的弹幕render
            //graphicsDevice.Clear(Color.Transparent);//用透明清屏

            //spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

            //Texture2D t = TextureAssets.Npc[NPC.type].Value;
            ////在自定义的render上绘制NPC
            //SpriteEffects spriteEffects = SpriteEffects.None;
            //if (NPC.spriteDirection == 1) {
            //    spriteEffects = SpriteEffects.FlipHorizontally;
            //}
            //float extraDrawY = Main.NPCAddHeight(NPC);
            //Vector2 origin = new Vector2(t.Width / 2, t.Height / Main.npcFrameCount[NPC.type] / 2);
            //Main.EntitySpriteDraw(t,
            //    new Vector2(NPC.position.X - screenPos.X + NPC.width / 2 - (float)t.Width * NPC.scale / 2f + origin.X * NPC.scale,
            //    NPC.position.Y - Main.screenPosition.Y + NPC.height - t.Height * NPC.scale / Main.npcFrameCount[NPC.type] + 4f + extraDrawY + origin.Y * NPC.scale + NPC.gfxOffY),
            //    NPC.frame,
            //    NPC.GetAlpha(drawColor), NPC.rotation, origin, NPC.scale, spriteEffects, 0);
            //if (NPC.color != default(Color)) {
            //    Main.EntitySpriteDraw(t, new Vector2(NPC.position.X - screenPos.X + NPC.width / 2 - t.Width * NPC.scale / 2f + origin.X * NPC.scale, NPC.position.Y - Main.screenPosition.Y + NPC.height - t.Height * NPC.scale / Main.npcFrameCount[NPC.type] + 4f + extraDrawY + origin.Y * NPC.scale + NPC.gfxOffY), NPC.frame, NPC.GetColor(drawColor), NPC.rotation, origin, NPC.scale, spriteEffects, 0);
            //}

            //// 重设RenderTarget

            //Main.spriteBatch.End();
            //Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

            //GraphicsDevice graphicsDevice = Main.graphics.GraphicsDevice;//获取GraphicsDevice
            //graphicsDevice.SetRenderTarget(blackRender);//把绘制目标设置为自己创建的弹幕render
            //graphicsDevice.Clear(Color.Transparent);//用透明清屏
            //Main.spriteBatch.Draw(Main.instance.blackTarget, Vector2.Zero, Color.White);

            //Main.spriteBatch.End();
            //graphicsDevice.SetRenderTarget(Main.screenTarget);//把绘制目标设置为自己创建的弹幕render
            //Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);


            Texture2D t = TextureAssets.Npc[NPC.type].Value;
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (NPC.spriteDirection == 1) {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            float extraDrawY = Main.NPCAddHeight(NPC);
            var origin = new Vector2(t.Width / 2, t.Height / Main.npcFrameCount[NPC.type] / 2);
            var pos = new Vector2(NPC.position.X - screenPos.X + NPC.width / 2 - (float)t.Width * NPC.scale / 2f + origin.X * NPC.scale,
                NPC.position.Y - screenPos.Y + NPC.height - t.Height * NPC.scale / Main.npcFrameCount[NPC.type] + 4f + extraDrawY + origin.Y * NPC.scale + NPC.gfxOffY);

            //var shader = ResourceManager.CoverRenderer.Value;
            //Main.instance.GraphicsDevice.Textures[1] = blackRender; // 背景图
            //shader.Parameters["screenSize"].SetValue(new Vector2(Main.screenWidth, Main.screenHeight));
            //shader.Parameters["leftTopInScreen"].SetValue(ToScreenShaderCoord(pos - origin));
            //shader.Parameters["rightBottomInScreen"].SetValue(ToScreenShaderCoord(pos + origin));
            //shader.CurrentTechnique.Passes["CoverRenderer"].Apply(); // 然后开启Shader
            // 绘制NPC
            Main.EntitySpriteDraw(t,
                pos,
                NPC.frame,
                NPC.GetAlpha(drawColor), NPC.rotation, origin, NPC.scale, spriteEffects, 0);
            if (NPC.color != default(Color)) {
                Main.EntitySpriteDraw(t, pos, NPC.frame, NPC.GetColor(drawColor), NPC.rotation, origin, NPC.scale, spriteEffects, 0);
            }
            //Main.spriteBatch.Draw(blackRender, Vector2.Zero, Color.White); // 绘制自定义的弹幕render
            var tileLeftTop = (pos - origin + screenPos).ToTileCoordinates();
            var tileRightBottom = (pos + origin + screenPos).ToTileCoordinates();
            for (int i = tileLeftTop.X; i <= tileRightBottom.X; i++) {
                for (int j = tileLeftTop.Y; j <= tileRightBottom.Y; j++) {
                    if (!WorldGen.InWorld(i, j)) continue;

                    if (Main.tile[i, j] == null)
                        Main.tile[i, j] = new Tile();

                    if (Lighting.Brightness(i, j) < 0.1f)
                        Main.spriteBatch.Draw(TextureAssets.BlackTile.Value, new Vector2(i << 4, j << 4) - screenPos, new Rectangle(0, 0, 16, 16), Color.Black);
                }
            }

            //// 重设shader
            //Main.spriteBatch.End();
            //Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.Transform);
            // 干掉原版绘制
            return false;
        }

        private static Vector2 ToScreenShaderCoord(Vector2 PositionInScreen) {
            return PositionInScreen / Main.ScreenSize.ToVector2();
        }
    }
}
