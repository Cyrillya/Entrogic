using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Summon
{
    public class DeadlySphereStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Deadly Sphere Staff V2");
            Tooltip.SetDefault("Summons deadly spheres to fight for you");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "完美球体召唤仗");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "召唤毫无瑕疵的球体为你作战");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            item.mana = 10;
            item.damage = 140;
            item.useStyle = ItemUseStyleID.Swing;
            item.width = 52;
            item.height = 50;
            item.useTime = 50;
            item.useAnimation = 50;
            item.noMelee = true;
            item.knockBack = 4f;
            item.value = 100000;
            item.rare = 15;
            item.UseSound = SoundID.Item113;
            item.autoReuse = true;
            item.shoot = ProjectileType<Projectiles.Minions.DeadlySphere>();
            item.shootSpeed = 8f;
            item.DamageType = DamageClass.Summon;;
            item.buffType = BuffType<Buffs.Minions.DeadlySphere>();
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
                player.MinionNPCTargetAim(false);
            }
            return base.UseItem(player);
        }
    }
}