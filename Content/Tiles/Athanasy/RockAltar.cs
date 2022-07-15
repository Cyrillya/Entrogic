using Entrogic.Content.Items.Athanasy;
using Entrogic.Content.Projectiles.Athanasy;
using Entrogic.Content.Tiles.BaseTypes;
using Terraria.GameContent.ObjectInteractions;

namespace Entrogic.Content.Tiles.Athanasy
{
    public class RockAltar : TileBase
    {
        public override bool AutoSelect(int i, int j, Item item) {
            base.AutoSelect(i, j, item);
            return item.type == ModContent.ItemType<TitansOrder>();
        }

        public override void SetStaticDefaults() => this.QuickSetFurniture(6, 6, 2, DustID.t_LivingWood, SoundID.Dig, false, new Color(151, 107, 75), false, false);

        public override bool CanKillTile(int i, int j, ref bool blockDamaged) {
            return true;
        }

        public override void MouseOver(int i, int j) {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ModContent.ItemType<TitansOrder>();
        }

        public override bool RightClick(int i, int j) {
            Player player = Main.LocalPlayer;

            int titansOrder = -1;
            for (int k = 0; k < player.inventory.Length; k++) {
                if (player.inventory[k].stack > 0 && player.inventory[k].type == ModContent.ItemType<TitansOrder>() && ItemLoader.CanUseItem(player.inventory[k], player)) {
                    titansOrder = k;
                    break;
                }
            }
            if (titansOrder == -1) {
                return false;
            }

            var item = player.inventory[titansOrder];
            if (titansOrder == 58)
                item = Main.mouseItem;
            if (item.consumable && ItemLoader.ConsumeItem(item, player)) {
                item.stack--;
                if (item.stack <= 0) {
                    item.TurnToAir();
                }
            }

            Tile tile = Main.tile[i, j];
            int topX = i - tile.TileFrameX / 18 % 6;
            int topY = j - tile.TileFrameY / 18 % 6;
            Vector2 pos = new Point(topX + 3, topY + 6).ToWorldCoordinates(0, 0); // 中心位置

            //NPC.SpawnBoss((int)pos.X, (int)pos.Y, ModContent.NPCType<NPCs.Enemies.Athanasy.Athanasy>(), Main.myPlayer);

            var spawnPosition = new Vector2(pos.X + 4f, pos.Y - 6 * 16f);
            Projectile.NewProjectile(new EntitySource_TileInteraction(player, i, j), spawnPosition, Vector2.Zero, ModContent.ProjectileType<TitansOrderProj>(), 0, 0f, player.whoAmI);

            return base.RightClick(i, j);
        }

        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY) {
            if (NPC.AnyNPCs(ModContent.NPCType<NPCs.Enemies.Athanasy.Athanasy>())) {
                tileFrameX += 18 * 6;
            }
        }

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) => true;
    }
    public class RockAltarItem : QuickTileItem
    {
        public RockAltarItem() : base("Rock Altar Placer", "DEBUG!!", 0, 0) { }
        public override int TileType() => ModContent.TileType<RockAltar>();
    }
}
