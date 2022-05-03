namespace Entrogic.Common.Globals.Players
{
    internal class PlayerStats : ModPlayer
    {
        /// <summary>
        /// 记录上几帧旋转角度，全端记录
        /// </summary>
        internal float[] RotationsForSmeltDagger = new float[10];
    }
}
