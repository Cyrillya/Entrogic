using System;

using Terraria;
using Terraria.ModLoader;

namespace Entrogic
{
    public static partial class ModHelper
    {
        /// <summary>
        /// 无入侵
        /// </summary>
        public static bool NoInvasion(NPCSpawnInfo spawnInfo) {
            return !spawnInfo.invasion && ((!Main.pumpkinMoon && !Main.snowMoon) || spawnInfo.spawnTileY > Main.worldSurface || Main.dayTime) && (!Main.eclipse || spawnInfo.spawnTileY > Main.worldSurface || !Main.dayTime);
        }
        /// <summary>
        /// 草地环境/无环境
        /// </summary>
        public static bool NoBiome(NPCSpawnInfo spawnInfo) {
            Player player = spawnInfo.player;
            return !player.ZoneJungle && !player.ZoneDungeon && !player.ZoneCorrupt && !player.ZoneCrimson && !player.ZoneHallow && !player.ZoneSnow && !player.ZoneUndergroundDesert;
        }
        /// <summary>
        /// 不在太空, 陨石, 蜘蛛洞地形
        /// </summary>
        public static bool NoZoneAllowWater(NPCSpawnInfo spawnInfo) {
            return !spawnInfo.sky && !spawnInfo.player.ZoneMeteor && !spawnInfo.spiderCave;
        }
        /// <summary>
        /// 不在太空, 陨石, 蜘蛛洞地形, 且NPC将判定产生的物块不包含水
        /// </summary>
        public static bool NoZone(NPCSpawnInfo spawnInfo) {
            return NoZoneAllowWater(spawnInfo) && !spawnInfo.water;
        }
        /// <summary>
        /// 普通敌怪生成区域(无入侵+周围无城镇)
        /// </summary>
        public static bool NormalSpawn(NPCSpawnInfo spawnInfo) {
            return !spawnInfo.playerInTown && NoInvasion(spawnInfo);
        }
        /// <summary>
        /// 普通敌怪生成区域(草地环境+周围无城镇) +
        /// 不在太空, 陨石, 蜘蛛洞地形 +
        /// NPC将判定产生的物块不包含水
        /// </summary>
        public static bool NoZoneNormalSpawn(NPCSpawnInfo spawnInfo) {
            return NormalSpawn(spawnInfo) && NoZone(spawnInfo);
        }
        /// <summary>
        /// 普通敌怪生成区域(草地环境+周围无城镇) + 不在太空, 陨石, 蜘蛛洞地形
        /// </summary>
        public static bool NoZoneNormalSpawnAllowWater(NPCSpawnInfo spawnInfo) {
            return NormalSpawn(spawnInfo) && NoZoneAllowWater(spawnInfo);
        }
        /// <summary>
        /// 普通敌怪生成区域(草地环境+周围无城镇) + 不在太空, 陨石, 蜘蛛洞地形 + NPC将判定产生的物块不包含水 + 草地环境
        /// </summary>
        public static bool NoBiomeNormalSpawn(NPCSpawnInfo spawnInfo) {
            return NormalSpawn(spawnInfo) && NoBiome(spawnInfo) && NoZone(spawnInfo);
        }
    }
}
