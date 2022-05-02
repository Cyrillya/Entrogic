using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;

namespace Entrogic.Common.WorldGeneration
{
    internal class WorldGenSystem : ModSystem
    {
        public static List<List<Color>> GenList { get; private set; }
        public static Color[,] AthanasyPlatform = new Color[110, 80];

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight) {
            // 衰落魔像房间，选择在Life Crystals这一个步骤之后是为了避免Smooth World的影响，同时还有罐子和雕像生成
            int LifeCrystalsIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Life Crystals"));
            if (LifeCrystalsIndex != -1) {
                tasks.Insert(LifeCrystalsIndex + 1, new ImmortalGolemRoom());
            }
            // 衰落魔像陷阱房间，以及清除战斗房间里面的宝箱和机关，为了规避莫名其妙的把岩浆变成水就只能放后面了
            int FinalCleanupIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Final Cleanup"));
            if (FinalCleanupIndex != -1) {
                tasks.Insert(FinalCleanupIndex + 1, new ImmortalGolemTrapRoom());
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

        public override void Load() {
            base.Load();
            Main.QueueMainThreadAction(delegate {
                ResourceManager_SetupContentEvent();
            });
        }

        private void ResourceManager_SetupContentEvent() {
            if (!Main.dedServ) {
                Texture2D Platform = ModContent.Request<Texture2D>("Entrogic/Assets/Images/WorldGeneration/AthanasyPlatform", AssetRequestMode.ImmediateLoad).Value;
                Color[] dataColors = new Color[Platform.Width * Platform.Height];
                Platform.GetData<Color>(dataColors);
                for (int c = 0; c < dataColors.Length; c++) {
                    int x = c % Platform.Width;
                    int y = c / Platform.Width;
                    AthanasyPlatform[x, y] = dataColors[c];
                }
            }
        }
    }
}
