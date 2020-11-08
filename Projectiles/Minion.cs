using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Projectiles
{
    public abstract class Minion : ModProjectile
    {
        public int weaponType;

        public override void AI() {
            CheckActive();
            Behavior();
            MinionMouse.weaponType[projectile.whoAmI] = weaponType;
        }

        public virtual void CheckActive() { }

        public virtual void Behavior() { }
	}
    public class MinionMouse : GlobalItem
    {
        public static int[] weaponType = new int[1001]; 
        public override bool CanUseItem(Item item, Player player)
        {
            if (player.altFunctionUse == 0)
                foreach (Projectile proj in Main.projectile)
                    if (item.type == weaponType[proj.whoAmI] && proj.active) {
                        proj.Center = Main.MouseWorld;
                        weaponType[proj.whoAmI] = 0;
                        return base.CanUseItem(item, player);
                    }
            return base.CanUseItem(item, player);
        }
        public override bool InstancePerEntity => true;
        public override GlobalItem Clone(Item item, Item itemClone)
        {
            return base.Clone(item, itemClone);
        }
    }
}