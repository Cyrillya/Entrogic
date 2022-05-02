namespace Entrogic.Content.Items.Misc.Materials
{

    public class SoulOfPure : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("“纯净的魔法能源”");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
            Main.RegisterItemAnimation(Type, new DrawAnimationVertical(5, 6));
            ItemID.Sets.AnimatesAsSoul[Type] = true; // Makes the item have 4 animation frames by default.
            ItemID.Sets.ItemIconPulse[Type] = true;
            ItemID.Sets.ItemNoGravity[Type] = true;
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
            Vector2 movement = vectorItemToPlayer.SafeNormalize(default(Vector2)) * 0.19f;
            Item.velocity = Item.velocity + movement;
            Item.velocity = Collision.TileCollision(Item.position, Item.velocity, Item.width, Item.height);
            return true;
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(Item.Center, Color.Gray.ToVector3() * 0.75f * Main.essScale);
        }
    }
}
