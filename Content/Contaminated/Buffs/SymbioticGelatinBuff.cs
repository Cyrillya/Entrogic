using Entrogic.Content.Contaminated.Friendly;
using Entrogic.Core.BaseTypes;

namespace Entrogic.Content.Contaminated.Buffs
{
    public class SymbioticGelatinBuff : BuffBase
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Symbiotic Gelatin");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "共生明胶");
            Description.SetDefault("Flexible symbiotic gelatins will fight for you.");
            Description.AddTranslation((int)GameCulture.CultureName.Chinese, "灵活的共生明胶将为你而战。");

            Main.buffNoSave[Type] = true; // This buff won't save when you exit the world
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex) {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<SymbioticGelatin>()] > 0) {
                player.buffTime[buffIndex] = 18000;
            }
            else {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}
