using Microsoft.Xna.Framework;

using System;

using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Card.Elements
{
    public class Fireball : ModCard
    {
        public override void PreCreated()
        {
            said = "圣洁，纯粹的天火会净化一切！";
            rare = CardRareID.Gravitation;
            tooltip = "射出一发炎爆球";
            costMana = 2;
            series = CardSeriesID.Element;
        }
        public override void CardDefaults()
        {
            attackCardRemainingTimes = 15;
            item.rare = RareID.LV2;
            item.shoot = ProjectileID.InfernoFriendlyBolt;
            item.damage = 8;
            item.knockBack = 7f;
            item.UseSound = SoundID.Item20;
            item.crit += 17;
        }
        public override string AnotherMessages()
        {
            return "[c/EE4000:获得15次施展魔法“火球术”的机会]";
        }
        public override void AttackEffects(Player player, int type, Vector2 position, Vector2 shootTo, float speedX, float speedY, int damage, float knockBack, float speed)
        {
            Projectile.NewProjectile(position.X, position.Y, speedX * 4f, speedY * 4f, type, damage, knockBack, player.whoAmI);
        }
    }
    public class Fireball_Electromagnetism : ModCard
    {
        public override void PreCreated()
        {
            said = "当心它的爆炸！";
            rare = CardRareID.Electromagnetism;
            tooltip = "召唤飞快的魔法火球";
            costMana = 2;
            series = CardSeriesID.Element;
        }
        public override void CardDefaults()
        {
            attackCardRemainingTimes = 15;
            item.rare = RareID.LV2;
            item.shoot = ProjectileID.InfernoFriendlyBolt;
            item.damage = 8;
            item.knockBack = 7f;
            item.UseSound = SoundID.Item20;
            item.crit += 17;
        }
        public override string AnotherMessages()
        {
            return "[c/EE4000:获得15次施展魔法“火球术”的机会]";
        }
        public override void AttackEffects(Player player, int type, Vector2 position, Vector2 shootTo, float speedX, float speedY, int damage, float knockBack, float speed)
        {
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
        }
    }
    public class Fireball_WeakNuclear : ModCard
    {
        public override void PreCreated()
        {
            said = "当心它的爆炸！";
            rare = CardRareID.WeakNuclear;
            tooltip = "召唤飞快的魔法火球";
            costMana = 2;
            series = CardSeriesID.Element;
        }
        public override void CardDefaults()
        {
            attackCardRemainingTimes = 15;
            item.rare = RareID.LV2;
            item.shoot = ProjectileID.InfernoFriendlyBolt;
            item.damage = 8;
            item.knockBack = 7f;
            item.UseSound = SoundID.Item20;
            item.crit += 17;
        }
        public override string AnotherMessages()
        {
            return "[c/EE4000:获得15次施展魔法“火球术”的机会]";
        }
        public override void AttackEffects(Player player, int type, Vector2 position, Vector2 shootTo, float speedX, float speedY, int damage, float knockBack, float speed)
        {
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
        }
    }
    public class Fireball_StrongNuclear : ModCard
    {
        public override void PreCreated()
        {
            said = "当心它的爆炸！";
            rare = CardRareID.StrongNuclear;
            tooltip = "召唤飞快的魔法火球";
            costMana = 2;
            series = CardSeriesID.Element;
        }
        public override void CardDefaults()
        {
            attackCardRemainingTimes = 15;
            item.rare = RareID.LV2;
            item.shoot = ProjectileID.InfernoFriendlyBolt;
            item.damage = 8;
            item.knockBack = 7f;
            item.UseSound = SoundID.Item20;
            item.crit += 17;
        }
        public override string AnotherMessages()
        {
            return "[c/EE4000:获得15次施展魔法“火球术”的机会]";
        }
        public override void AttackEffects(Player player, int type, Vector2 position, Vector2 shootTo, float speedX, float speedY, int damage, float knockBack, float speed)
        {
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
        }
    }
}
