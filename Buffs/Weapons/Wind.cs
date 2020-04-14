using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

//下一行是定义此文件(cs和png)在源码文件夹里面的路径,必备,且要和实际一致
namespace Entrogic.Buffs.Weapons
{
    public class Wind : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("风");
            Description.SetDefault("你感到浑身轻盈无比");
            Main.debuff[Type] = false; //是否为Debuff(只是说Debuff的特征,不是说你增加生命回复速度会自动变成减少生命回复速度之类的),true=是,false=否
            Main.buffNoSave[Type] = true; //退出存档后是否保存Buff,true=否,false=是
            Main.buffNoTimeDisplay[Type] = false; //决定这个buff会不会显示持续时间,true=否,false=是
            canBeCleared = true; //设置这个Buff能不能右键取消,true=能,false=否
        }

        public override void Update(Player player, ref int buffIndex)
        {
            //增加玩家跳跃速度
            player.jumpSpeedBoost += 2.4f;
            player.moveSpeed += 3.5f;
            player.maxRunSpeed += 2.5f;
            // Some other effects:
            //player.lifeRegen++; (玩家生命回复速度)
            //player.meleeCrit += 2; (玩家近战暴击率)
            //player.meleeDamage += 0.051f; (玩家近战伤害)
            //player.meleeSpeed += 0.051f; (玩家近战攻速)
            //player.statDefense += 3; (玩家防御)
            //player.moveSpeed += 0.05f; (玩家移速)
        }
    }
}
