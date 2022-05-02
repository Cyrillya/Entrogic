using Entrogic.Content.Items.Athanasy;
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

        public override void SetStaticDefaults() => this.QuickSetFurniture(6, 6, DustID.t_LivingWood, SoundID.Dig, false, new Color(151, 107, 75), false, false, "Rock Altar");

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
            if (player.HeldItem?.type != ModContent.ItemType<TitansOrder>() || !player.HeldItem.ModItem.CanUseItem(player)) {
                return false;
            }

            player.HeldItem.stack--;
            if (player.HeldItem.stack <= 0) {
                player.HeldItem.TurnToAir();
            }

            Tile tile = Main.tile[i, j];
            int topX = i - tile.TileFrameX / 18 % 6;
            int topY = j - tile.TileFrameY / 18 % 6;
            Vector2 pos = new Point(topX + 3, topY + 6).ToWorldCoordinates(0, 0); // 中心位置

            NPC.SpawnBoss((int)pos.X, (int)pos.Y, ModContent.NPCType<NPCs.Enemies.Athanasy.Athanasy>(), Main.myPlayer);
            return base.RightClick(i, j);
        }

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) => true;
    }
    public class RockAltarItem : QuickTileItem
    {
        public RockAltarItem() : base("Rock Altar Placer", "DEBUG!!", 0, 0) { }
        public override int TileType() => ModContent.TileType<RockAltar>();
    }
}
