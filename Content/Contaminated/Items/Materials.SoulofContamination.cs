using Entrogic.Core.BaseTypes;

namespace Entrogic.Content.Contaminated.Items
{
    public class SoulofContamination : ItemBase
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            ItemID.Sets.ItemIconPulse[Item.type] = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;

            DisplayName.SetDefault("Soul of Contamination");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "污染之魂");
            Tooltip.SetDefault("It looks repulsive to you.");
            Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "它看起来对你很排斥。");
        }
        
        public override void SetDefaults()
        {
            Item refItem = new();
            refItem.SetDefaults(ItemID.SoulofSight);
            Item.width = refItem.width;
            Item.height = refItem.height;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(silver: 20);
            Item.rare = ItemRarityID.Orange;
        }
        
        public override void GrabRange(Player player, ref int grabRange)
        {
            grabRange *= 3;
        }

        public override bool GrabStyle(Player player)
        {
            Vector2 vectorItemToPlayer = player.Center - Item.Center;
            Vector2 movement = -vectorItemToPlayer.SafeNormalize(default(Vector2)) * 0.1f;
            Item.velocity = Item.velocity + movement;
            Item.velocity = Collision.TileCollision(Item.position, Item.velocity, Item.width, Item.height);
            return true;
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(Item.Center, Color.Gray.ToVector3() * 0.35f * Main.essScale);
        }
    }
}