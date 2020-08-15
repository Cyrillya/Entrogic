using Entrogic.NPCs.CardFightable.CardBullet;
using Entrogic.NPCs.CardFightable.CardBullet.PlayerBullets;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using static Terraria.ModLoader.ModContent;

namespace Entrogic.Items.Weapons.Card.Organisms
{
    public class RainingSpore : ModCard
    {
        public override void PreCreated()
        {
            said = "某乱码蟹提供特效支持";
            rare = CardRareID.Gravitation;
            tooltip = "召唤从天而降的孢子狂潮";
            costMana = 1;
            series = CardSeriesID.Organism;
        }
        public override string AnotherMessages()
        {
            return "[c/EE4000:获得40次召唤孢子雨的机会]";
        }
        public override void CardDefaults()
        {
            item.shootSpeed = 8f;
            item.rare = RareID.LV7;
            item.UseSound = SoundID.Item20;
            item.value = Item.sellPrice(silver: 20);
            item.shoot = ProjectileType<Projectiles.Arcane.Pineapple>();
            item.crit += 30;
            item.damage = 17;
            item.knockBack = 5f;
            attackCardRemainingTimes = 18;
        }
        public override void AttackEffects(Player player, int type, Vector2 position, Vector2 shootTo, float speedX, float speedY, int damage, float knockBack, float speed)
        {
            Vector2 vec = new Vector2(speedX, speedY);
            Projectile.NewProjectile(position, vec, type, damage, knockBack, player.whoAmI);
            Main.NewText("没做完！呵呵！");
        }
        public override void GameAI(float Duration, Vector2 PanelPosition)
        {
            Player clientPlayer = Main.LocalPlayer;
            EntrogicPlayer clientModPlayer = EntrogicPlayer.ModPlayer(clientPlayer);
            if (Duration >= 3.2f && Main.rand.NextBool(3))
            {
                MushroomBullet bullet = new MushroomBullet()
                {
                    Velocity = new Vector2(0f, Main.rand.NextFloat(1f, 3f)),
                    Position = new Vector2(Main.rand.Next((int)PlaygroundSize.X), -50f),
                    UIPosition = PanelPosition
                };
                clientModPlayer._bullets.Add(bullet);
            }
        }
        public override void PreStartGaming(ref float RoundDuration)
        {
            RoundDuration = 7.5f;
        }
        public override bool AbleToGetFromRandom(Player player)
        {
            return false;
        }
    }
}
