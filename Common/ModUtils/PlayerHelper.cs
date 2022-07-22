namespace Entrogic
{
    public static partial class ModHelper
    {
        public static bool Exists(this Player player) => player.active && !player.dead;
    }
}
