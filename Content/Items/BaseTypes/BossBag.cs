namespace Entrogic.Content.Items.BaseTypes
{
    public abstract class BossBag : ModItem
    {
        /// <summary>
        /// 是否是肉前宝藏袋，原版的话肉前宝藏袋只有在特殊种子下才能开出开发者套，这就是拿来干这个的。注意1.4不应该单独写开发者套装代码了
        /// </summary>
        public virtual bool PreHardmode { get; }

        /// <summary>
        /// 原版的宝藏袋实际上稀有度并不是-12，而是有自己的依时期而定的稀有度
        /// </summary>
        public virtual int Rarity { get; }

        public sealed override void SetStaticDefaults() {
            // 自动翻译
            DisplayName.SetDefault(($"{{$Mods.I.CommonItemName.TreasureBag}} ({Lang.GetNPCName(BossBagNPC)})"));
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");

            ItemID.Sets.PreHardmodeLikeBossBag[Type] = PreHardmode;
            ItemID.Sets.BossBag[Type] = true;

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3; // 旅途模式
        }

        public sealed override void SetDefaults() {
            Item.CloneDefaults(ItemID.EyeOfCthulhuBossBag); // 直接复制原版Default方便快捷
            Item.rare = Rarity == 0 ? RarityLevelID.Expert : Rarity; // 自己设置稀有度
        }

        public sealed override void Update(ref float gravity, ref float maxFallSpeed) {
            Lighting.AddLight((int)((Item.position.X + Item.width) / 16f), (int)((Item.position.Y + Item.height / 2) / 16f), 0.4f, 0.4f, 0.4f);
            bool flag82 = Item.timeSinceItemSpawned % 12 == 0;
            if (flag82) {
                Dust dust2 = Dust.NewDustPerfect(Item.Center + new Vector2(0f, Item.height * -0.1f) + Main.rand.NextVector2CircularEdge(Item.width * 0.6f, Item.height * 0.6f) * (0.3f + Main.rand.NextFloat() * 0.5f), 279, new Vector2?(new Vector2(0f, (0f - Main.rand.NextFloat()) * 0.3f - 1.5f)), 127, default(Color), 1f);
                dust2.scale = 0.5f;
                dust2.fadeIn = 1.1f;
                dust2.noGravity = true;
                dust2.noLight = true;
                dust2.alpha = 0;
            }
        }

        public sealed override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI) {
            Texture2D texture = TextureAssets.Item[Item.type].Value;
            Rectangle rectangle = new(0, 0, texture.Width, texture.Height);
            Vector2 vector = rectangle.Size() / 2f;
            Vector2 value = new(Item.width / 2 - vector.X, Item.height - rectangle.Height);
            Vector2 vector2 = Item.position - Main.screenPosition + vector + value;
            float num = Item.velocity.X * 0.2f;
            float num3 = Item.timeSinceItemSpawned / 240f + Main.GlobalTimeWrappedHourly * 0.04f;
            float num4 = Main.GlobalTimeWrappedHourly;
            num4 %= 4f;
            num4 /= 2f;
            if (num4 >= 1f) {
                num4 = 2f - num4;
            }
            num4 = num4 * 0.5f + 0.5f;
            for (float num5 = 0f; num5 < 1f; num5 += 0.25f) {
                Main.spriteBatch.Draw(texture, vector2 + new Vector2(0f, 8f).RotatedBy((num5 + num3) * 6.28318548f, default(Vector2)) * num4, new Microsoft.Xna.Framework.Rectangle?(rectangle), new Microsoft.Xna.Framework.Color(90, 70, 255, 50), num, vector, scale, SpriteEffects.None, 0f);
            }
            for (float num6 = 0f; num6 < 1f; num6 += 0.34f) {
                Main.spriteBatch.Draw(texture, vector2 + new Vector2(0f, 4f).RotatedBy((num6 + num3) * 6.28318548f, default(Vector2)) * num4, new Microsoft.Xna.Framework.Rectangle?(rectangle), new Microsoft.Xna.Framework.Color(140, 120, 255, 77), num, vector, scale, SpriteEffects.None, 0f);
            }
            Main.spriteBatch.Draw(texture, vector2, new Microsoft.Xna.Framework.Rectangle?(rectangle), alphaColor, num, vector, scale, SpriteEffects.None, 0f);
            return false;
        }

        public sealed override bool CanRightClick() {
            return true;
        }

        public sealed override void OpenBossBag(Player player) {
            BossBagLoot(player.GetSource_OpenItem(Type), player);
        }

        /// <summary>
        /// 自定义宝藏袋开出的物品
        /// </summary>
        /// <param name="source">生成物品所用的Source</param>
        /// <param name="player">开启宝藏袋的玩家</param>
        public abstract void BossBagLoot(IEntitySource source, Player player);
    }
}