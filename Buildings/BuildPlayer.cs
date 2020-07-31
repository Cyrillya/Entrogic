using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Entrogic
{
    public class BuildPlayer : ModPlayer
    {
        public static Vector2 MousePos
        {
            get { return Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY); }
        }
        Vector2? start = null;
        public override void PreUpdate()
        {
            if (DEntrogicDebugClient.Instance.AuthorMode && Main.netMode == NetmodeID.SinglePlayer)
            {
                // 这里是buildings文件生成的代码
                if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.U)
                && !Main.oldKeyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.U))
                {
                    if (start == null)
                    {
                        start = MousePos;
                        Main.NewText("Start: " + MousePos + " T " + MousePos.ToTileCoordinates());
                    }
                    else
                    {
                        Main.NewText("End: " + MousePos + " T " + MousePos.ToTileCoordinates());
                        Buildings.Export(start.Value, MousePos);
                        start = null;
                    }
                }
                if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.B)
                    && !Main.oldKeyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.B))
                {
                    using (FileStream fs = File.OpenRead(Buildings.BuildingSavePath + "1.buildings"))
                        Buildings.Build(fs, MousePos);
                }
            }
        }
    }
}
