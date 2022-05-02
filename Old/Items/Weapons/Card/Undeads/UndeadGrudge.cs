
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Card.Undeads
{
    public class UndeadGrudge : ModCard
    {
        public override void PreCreated()
        {
            said = "利用亡魂的力量...";
            rare = CardRareID.Gravitation;
            tooltip = "不可使用，过牌时使牌库内随机一张牌费用+1，召唤少量吸血导弹";
            costMana = 0;
            special = true;
            series = CardSeriesID.Undead;
        }
        public override void CardDefaults()
        {
            item.rare = RareID.LV4;
            item.UseSound = SoundID.Item20;
            item.value = Item.sellPrice(silver: 10);
            item.shoot = ProjectileType<Projectiles.Arcane.UndeadGrudge>();
            item.shootSpeed = 10f;
            item.damage = 21;
            item.knockBack = 3.6f;
        }
        public override void SpecialEffects(Player player, Vector2 position, int damage, float knockBack, int number, int packType, bool special, bool drawCard)
        {
            EntrogicPlayer ePlayer = player.GetModPlayer<EntrogicPlayer>();
            for (int a = 0; a < 3; a++)
            {
                // 角度变化
                for (float rad = -MathHelper.TwoPi / 12f; rad < MathHelper.TwoPi; rad += MathHelper.TwoPi / 6)
                {
                    Vector2 finalVec = (0f + rad).ToRotationVector2() * 20f;
                    // 射出去！
                    int p = Projectile.NewProjectile(player.position, finalVec, item.shoot, damage, knockBack, player.whoAmI);
                    Main.projectile[p].timeLeft += a * 20;
                }
            }
            if (ePlayer.LibNum <= 0)
                return;
            while (true)
                for (int i = 0; i < ePlayer.CardReadyType.Length; i++)
                    if (Main.rand.NextBool(5) && ePlayer.CardReadyType[i] != 0)
                    {
                        ePlayer.CardReadyCost[i]++;
                        return;
                    }
        }
        public override bool AbleToGetFromRandom(Player player)
        {
            return false;
        }
    }
    public class UndeadGrudge_Electromagnetism : ModCard
    {
        public override void PreCreated()
        {
            said = "我能感受到它们的热情";
            rare = CardRareID.Electromagnetism;
            tooltip = "不可使用，过牌时使牌库内随机一张牌费用+1，召唤很多吸血导弹";
            costMana = 0;
            special = true;
            series = CardSeriesID.Undead;
        }
        public override void CardDefaults()
        {
            item.rare = RareID.LV4;
            item.UseSound = SoundID.Item20;
            item.value = Item.sellPrice(silver: 10);
            item.shoot = ProjectileType<Projectiles.Arcane.UndeadGrudge>();
            item.shootSpeed = 10f;
            item.damage = 29;
            item.knockBack = 4f;
        }
        public override void SpecialEffects(Player player, Vector2 position, int damage, float knockBack, int number, int packType, bool special, bool drawCard)
        {
            EntrogicPlayer ePlayer = player.GetModPlayer<EntrogicPlayer>();
            // 角度变化
            for (int a = 0; a < 4; a++)
            {
                // 角度变化
                for (float rad = -MathHelper.TwoPi / 16f; rad < MathHelper.TwoPi; rad += MathHelper.TwoPi / 8f)
                {
                    Vector2 finalVec = (0f + rad).ToRotationVector2() * 20f;
                    // 射出去！
                    int p = Projectile.NewProjectile(player.position, finalVec, item.shoot, damage, knockBack, player.whoAmI);
                    Main.projectile[p].timeLeft += a * 15;
                }
            }
            if (ePlayer.LibNum <= 0)
                return;
            while (true)
                for (int i = 0; i < ePlayer.CardReadyType.Length; i++)
                    if (Main.rand.NextBool(5) && ePlayer.CardReadyType[i] != 0)
                    {
                        ePlayer.CardReadyCost[i]++;
                        return;
                    }
        }
        public override bool AbleToGetFromRandom(Player player)
        {
            return NPC.downedBoss3;
        }
    }
    public class UndeadGrudge_Strong : ModCard
    {
        public override void PreCreated()
        {
            said = "它们似乎要从墓地中跳出来了";
            rare = CardRareID.StrongNuclear;
            tooltip = "不可使用，过牌时使牌库内随机一张牌费用+2，召唤巨量吸血穿墙导弹";
            costMana = 0;
            special = true;
            series = CardSeriesID.Undead;
        }
        public override void CardDefaults()
        {
            item.rare = RareID.LV4;
            item.UseSound = SoundID.Item20;
            item.value = Item.sellPrice(silver: 10);
            item.shoot = ProjectileType<Projectiles.Arcane.UndeadGrudge>();
            item.shootSpeed = 12f;
            item.damage = 34;
            item.knockBack = 4.5f;
        }
        public override void SpecialEffects(Player player, Vector2 position, int damage, float knockBack, int number, int packType, bool special, bool drawCard)
        {
            EntrogicPlayer ePlayer = player.GetModPlayer<EntrogicPlayer>();
            // 角度变化
            for (int a = 0; a < 6; a++)
            {
                // 角度变化
                for (float rad = -MathHelper.TwoPi / 24f; rad < MathHelper.TwoPi; rad += MathHelper.TwoPi / 12f)
                {
                    Vector2 finalVec = (0f + rad).ToRotationVector2() * 20f;
                    // 射出去！
                    int p = Projectile.NewProjectile(player.position, finalVec, item.shoot, damage, knockBack, player.whoAmI);
                    Main.projectile[p].timeLeft += a * 10;
                }
            }
            if (ePlayer.LibNum <= 0)
                return;
            while (true)
                for (int i = 0; i < ePlayer.CardReadyType.Length; i++)
                    if (Main.rand.NextBool(5) && ePlayer.CardReadyType[i] != 0)
                    {
                        ePlayer.CardReadyCost[i] += 2;
                        return;
                    }
        }
        public override bool AbleToGetFromRandom(Player player)
        {
            return NPC.downedPlantBoss;
        }
    }
}
