using Entrogic.Content.Items.BaseTypes;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Entrogic.Content.Items.ContyElemental
{
    [AutoloadEquip(EquipType.Wings)]
    public class BottleofStorm : ItemBase
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Bottle of Storm");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "风暴之瓶");
            Tooltip.SetDefault("Flight attributes are affected by different factors.\n\"A bottle of storm, much more powerful than those with a bottle of sand!\"");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "飞行属性会被不同因素影响\n“瓶中风暴，可比那些装着一瓶沙子的人厉害多了！”");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            // These wings use the same values as the solar wings
            // Fly time: 180 ticks = 3 seconds
            // Fly speed: 9
            // Acceleration multiplier: 2.5
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(180, 9f, 2.5f);
        }

        public override void SetDefaults() {
            Item.width = 120;
            Item.height = 100;
            Item.value = 10000;
            Item.rare = RarityLevelID.MiddleHM;
            Item.accessory = true;
            Item.expert = true;
            Item.value = Item.sellPrice(0, 8);
        }

        public override void UpdateVanity(Player player) {
            player.GetModPlayer<BottleofStormPlayer>().HasWings = true;
            base.UpdateVanity(player);
        }


        public override void UpdateAccessory(Player player, bool hideVisual) {
            if (!hideVisual) {
                player.GetModPlayer<BottleofStormPlayer>().HasWings = true;
            }
        }

        public override bool WingUpdate(Player player, bool inUse) {
            if (inUse)
                for (int i = 0; i < 3; i++) {
                    Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.FishronWings, -player.velocity.X, -player.velocity.Y * 0.5f, 100, default(Color), 1.6f);
                    dust.fadeIn = 0.4f;
                    dust.noGravity = true;
                }
            return false;
        }

        /// <param name="ascentWhenFalling">下落途中(speedY>0)突然爬升的速度值（与基础速度值[constantAscend]相加）</param>
        /// <param name="ascentWhenRising">持续上升(speedY<0)的速度值（与基础速度值[constantAscend]相加）</param>
        /// <param name="maxCanAscendMultiplier">爬升速度小于[jumpSpeed*该值]时，基础速度值[constantAscend]与持续上升速度值[ascentWhenRising]相加</param>
        /// <param name="maxAscentMultiplier">最大爬升速度（具体限制：爬升速度小于[jumpSpeed*该值]）</param>
        /// <param name="constantAscend">基础速度值</param>
        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend) {
            // 时间值，最大值为54000+32400=86400
            double time = Main.time + (Main.dayTime ? 0 : 54000); 
            // 利用圆形轨迹决定速度（为了一个非线性速度变化，尽管意义不大）（Y坐标决定速度，区间:[-1, 1]）
            Vector2 TimeRound = ((float)time / 86400 * MathHelper.TwoPi).ToRotationVector2(); 

            // 这些为了方便就不作更改了，最重要的是爬升速度极限
            ascentWhenFalling = 0.85f;
            ascentWhenRising = 0.15f;
            maxCanAscendMultiplier = 1f;
            maxAscentMultiplier = 3f;
            constantAscend = 0.235f;

            // 按照时间改变速度（0.24：白天更快，-0.24：晚上更快）
            maxAscentMultiplier += (TimeRound.Y - 1f) * -0.24f;
            // 按照周围城镇NPC多少改变速度（越多越快，上限5个）
            maxAscentMultiplier += Math.Min(player.townNPCs, 5) * 0.09f;

            // 滑翔速度tModLoader好像没给字段
        }

        public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration) {
            acceleration = 1.8f;
            // 顺风飞行给予加成，反之逆风会减缓速度
            speed += Main.windSpeedCurrent * player.direction * 1.5f;
            // 向着太阳/月亮的方向飞行会更快，反之变慢
            float astronPosition = (float)(Main.time / (Main.dayTime ? 54000 : 32400)) - 0.5f;
            int astronDirection = astronPosition < 0 ? -1 : 1;
            speed += player.direction == astronDirection ? 0.4f : -0.4f;
            // 设计是以日耀翅膀为基础，根据各种游戏机制不同降低翅膀属性来对上阶段
            speed *= 0.7f;
        }
    }
}
