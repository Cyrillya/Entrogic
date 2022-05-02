
using Entrogic.Content.Items.BaseTypes;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Entrogic.Content.Items.Athanasy
{
    public class RockslimeStaff : ItemBase
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Rockslime Staff");
            Tooltip.SetDefault("Summons a rock slime to fight for you.\n\"It's time to retaliate\"");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "岩石史莱姆法杖");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "召唤岩石史莱姆为你而战。\n“是时候以牙还牙了”");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults() {
            Item.mana = 10;
            Item.damage = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.width = 48;
            Item.height = 46;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.noMelee = true;
            Item.knockBack = 3f;
            Item.value = Item.sellPrice(0, 3, 50);
            Item.rare = RarityLevelID.EarlyHM;
            Item.UseSound = SoundID.Item113;
            //Item.shoot = ProjectileType<Rockslime>();
            Item.shootSpeed = 10f;
            Item.DamageType = DamageClass.Summon;
            //Item.buffType = BuffType<Buffs.Minions.Stoneslime>();
            Item.buffTime = 3600;
        }

        public override bool AltFunctionUse(Player player) {
            return true;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
            position = Main.MouseWorld;
            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
        }

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            return player.altFunctionUse != 2;
        }

        public override bool CanUseItem(Player player) {
            if (player.altFunctionUse == 2) {
                player.MinionNPCTargetAim(false);
            }
            return base.CanUseItem(player);
        }
    }
}
