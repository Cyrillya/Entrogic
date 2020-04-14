using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

using Entrogic.Projectiles.衰落魔像;

namespace Entrogic.Items.AntaGolem
{
    public class StoneSlimeStaff: ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stoneslime Staff");
            Tooltip.SetDefault("Summons a stone slime to fight for you");
            DisplayName.AddTranslation(GameCulture.Chinese, "岩石史莱姆法杖");
            Tooltip.AddTranslation(GameCulture.Chinese, "召唤岩石史莱姆为你而战");
        }
        public override void SetDefaults()
        {
            item.mana = 10;
            item.damage = 20;
            item.useStyle = 1;
            item.width = 48;
            item.height = 46;
            item.useTime = 30;
            item.useAnimation = 30;
            item.noMelee = true;
            item.knockBack = 3f;
            item.value = Item.sellPrice(0, 3, 50);
            item.rare = RareID.LV5;
            item.UseSound = SoundID.Item113;
            item.shoot = ProjectileType<Stoneslime>();
            item.shootSpeed = 10f;
            item.summon = true;
            item.buffType = BuffType<Buffs.Minions.Stoneslime>();
            item.buffTime = 3600;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position = Main.MouseWorld;
            return player.altFunctionUse != 2;
        }

        public override bool UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                player.MinionNPCTargetAim();
            }
            return base.UseItem(player);
        }
    }
}
