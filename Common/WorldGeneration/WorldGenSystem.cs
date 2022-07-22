using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;

namespace Entrogic.Common.WorldGeneration
{
    internal class WorldGenSystem : ModSystem
    {
        public static List<List<Color>> GenList { get; private set; }
        public static Color[,] AthanasyPlatform = new Color[110, 80];

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight) {
            // 衰落魔像房间，以及清除战斗房间里面的宝箱和机关，为了规避莫名其妙的把岩浆变成水就只能放后面了
            int LihzahrdAltarsIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Lihzahrd Altars"));
            if (LihzahrdAltarsIndex != -1) {
                tasks.Insert(LihzahrdAltarsIndex + 1, new ImmortalGolemRoom());
                tasks.Insert(LihzahrdAltarsIndex + 2, new ImmortalGolemTrapRoom());
            }
        }

        public override void PostWorldGen() {
        }

        public override void SaveWorldData(TagCompound tag) {
            base.SaveWorldData(tag);
            ImmortalGolemRoom.SaveWorldData(tag);
        }

        public override void LoadWorldData(TagCompound tag) {
            base.LoadWorldData(tag);
            ImmortalGolemRoom.LoadWorldData(tag);
        }

        public override void OnWorldLoad() {
            if (Main.netMode is NetmodeID.MultiplayerClient) {
                ModNetHandler.WorldData.SendGolemRoom(-1, -1);
            }
        }

        public override void Load() {
            Main.RunOnMainThread(delegate {
                ResourceManager_SetupContentEvent();
            });
        }

        public override void Unload() {
            Array.Clear(AthanasyPlatform, 0, AthanasyPlatform.Length);
            GenList?.Clear();
            GenList = null;
        }

        private static void ResourceManager_SetupContentEvent() {
            if (!Main.dedServ) {
                Texture2D Platform = ModContent.Request<Texture2D>("Entrogic/Assets/Images/WorldGeneration/AthanasyPlatform", AssetRequestMode.ImmediateLoad).Value;
                Color[] dataColors = new Color[Platform.Width * Platform.Height];
                Platform.GetData(dataColors);
                for (int c = 0; c < dataColors.Length; c++) {
                    int x = c % Platform.Width;
                    int y = c / Platform.Width;
                    AthanasyPlatform[x, y] = dataColors[c];
                }
            }
        }
    }
}
