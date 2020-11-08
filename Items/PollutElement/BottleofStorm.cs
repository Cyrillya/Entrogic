
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace Entrogic.Items.PollutElement
{
	[AutoloadEquip(EquipType.Wings)]
	public class BottleofStorm : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
		{
			item.width = 120;
			item.height = 100;
			item.value = 10000;
			item.rare = ItemRarityID.LightRed;
			item.accessory = true;
            item.expert = true;
            item.value = Item.sellPrice(0, 8);
        }
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.wingTimeMax = 175;
		}
        public float speedMoon
        {
            get
            {
                if (Main.moonPhase == MoonID.FullMoon || Main.moonPhase == MoonID.NewMoon)
                    return 1f;
                else if (Main.moonPhase == MoonID.ThirdQuarter || Main.moonPhase == MoonID.FirstQuarter)
                    return -1f;
                else return 0f;
            }
        }
        public float speedTime
        {
            get
            {
                const double maxDayTime = 54000;
                const double maxNightTime = 32400;
                if (Main.dayTime) // 昼
                {
                    return (float)(Main.time / maxDayTime * 2) - 1;
                }
                else // 夜
                {
                    return 1 - (float)(Main.time / maxNightTime * 2);
                }
            }
        }
        public float speedWeather
        {
            get
            {
                float value = -1;
                value += Main.numClouds / 200;
                value += Main.maxRaining;
                return value;
            }
        }
        public override bool WingUpdate(Player player, bool inUse)
        {
            if (inUse)
                for (int i = 0; i < 2; i++)
                {
                    Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, 217, -player.velocity.X, -player.velocity.Y, 100, default(Color), 0.8f);
                    dust.fadeIn = 0.5f;
                    dust.noGravity = true;
                }
            return false;
        }
        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
		{
            float addSpeed = (speedMoon + speedTime + speedWeather) / 2f;
            //addSpeed = MathHelper.Max(-2.25f, addSpeed);

            ascentWhenFalling = (float)(0.8 - addSpeed * 0.17);
			ascentWhenRising = (float)(0.53 + addSpeed * 0.11);
			maxCanAscendMultiplier = 1.29f + addSpeed * 0.32f;
			maxAscentMultiplier = 2.49f + addSpeed * 0.46f;
			constantAscend = 0.175f + addSpeed * 0.038f;
		}

		public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
        {
            float addSpeed = (speedMoon + speedTime + speedWeather) / 2f;
            //addSpeed = MathHelper.Max(-2.25f, addSpeed);

            speed = 9.2f + addSpeed * 1.81f;
			acceleration *= 5.1f + addSpeed * 1.06f;
		}
    }
    public class PolluWings1 : EquipTexture
    {
    }
    public class PolluWings2 : EquipTexture
    {
    }
    public class PolluWings3 : EquipTexture
    {
    }
    public class PolluWings4 : EquipTexture
    {
    }
    public class PolluWings5 : EquipTexture
    {
    }
    public class PolluWings6 : EquipTexture
    {
    }
    public class PolluWings7 : EquipTexture
    {
    }
    public class PolluWings8 : EquipTexture
    {
    }
}