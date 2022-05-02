using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria.Audio;

namespace Entrogic.Content.Items.Misc.Weapons.Ranged.Gun
{
    public class OldRifle : ModItem
    {
        public override void Load() {
            base.Load();
            IL.Terraria.Player.ItemCheck_Inner += Player_ItemCheck_Inner;
        }

        private void Player_ItemCheck_Inner(ILContext il) {
            ILCursor c = new ILCursor(il);
            c.GotoNext(MoveType.After, i => i.MatchLdfld(typeof(Item), nameof(Item.shoot)));
            c.GotoNext(MoveType.After, i => i.MatchLdcI4(0));
            c.GotoNext(MoveType.After, i => i.MatchBle(out _));
            c.GotoNext(MoveType.After, i => i.MatchLdarg(0));
            c.GotoNext(MoveType.After, i => i.MatchLdfld(typeof(Player), nameof(Player.itemAnimation)));
            c.GotoNext(MoveType.After, i => i.MatchLdcI4(0));
            c.GotoNext(MoveType.After, i => i.MatchBle(out _));
            c.GotoNext(MoveType.After, i => i.MatchLdarg(0));
            c.GotoNext(MoveType.After, i => i.MatchLdcI4(0));
            c.GotoNext(MoveType.After, i => i.MatchAnd()); // 定位到判断，而且和ItemTimeIsZero相关的阶段

            c.Emit(OpCodes.Ldarg_0); // 推入当前Player实例
            c.Emit(OpCodes.Ldloc_2); // 推入ItemCheck_Inner的本地变量Item实例
            c.EmitDelegate<Func<bool, Player, Item, bool>>((returnValue, player, item) => {
                if (item.type == Type) { // 仅在物品是老步枪时进行修改
                    if (player.itemAnimation == 8) { // 老步枪特性
                        if (item.UseSound != null) // 这时候才播放声音
                            SoundEngine.PlaySound(item.UseSound, player.Center);
                        return true;
                    }
                    else if (!Main.dedServ && Main.myPlayer == player.whoAmI && player.itemAnimation > 8) {
                        player.direction = Math.Sign(Main.MouseWorld.X - player.Center.X);
                        // 原版这么写的，不懂
                        Vector2 pointPoisition = player.RotatedRelativePoint(player.MountedCenter, reverseRotation: true);
                        var vec = ModHelper.GetFromToVector(pointPoisition, Main.MouseWorld);
                        player.itemRotation = (float)Math.Atan2(vec.Y * (float)player.direction, vec.X * (float)player.direction) - player.fullRotation;

                        NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, player.whoAmI);
                        NetMessage.SendData(MessageID.ItemAnimation, -1, -1, null, player.whoAmI);
                        return false;
                    }
                }
                return returnValue; // 物品不是老步枪，那随便你了
            });
        }

        public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Old Rifle");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "老旧的步枪");
            Tooltip.SetDefault("“枪身上的磨痕代表着它昔日的荣耀”\n“伪M1-拉栓式”");

            ItemID.Sets.SkipsInitialUseSound[Type] = true; // 在StartActualUse里面不播放声音
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults() {
            Item.CloneDefaults(ItemID.SniperRifle);
            Item.damage = 26;
            Item.shootSpeed = 16f;
            Item.useTime = 8;
            Item.useAnimation = 65;
            Item.value = 10000;
            Item.knockBack = 4f;
            Item.crit = 82;
            Item.width = 76;
            Item.height = 28;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
        }

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame) {
            base.UseStyle(player, heldItemFrame);
        }
    }
}
