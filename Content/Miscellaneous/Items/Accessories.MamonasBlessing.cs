using Entrogic.Core.BaseTypes;
using Entrogic.Core.Global.Player;
using Entrogic.Helpers.ID;

namespace Entrogic.Content.Miscellaneous.Items;

[AutoloadEquip(EquipType.Face)]
public class MamonasBlessing : ItemBase
{
    public override void SetStaticDefaults() {
        base.SetStaticDefaults();
        ArmorIDs.Face.Sets.OverrideHelmet[Item.faceSlot] = true;
        ArmorIDs.Face.Sets.PreventHairDraw[Item.faceSlot] = true;
    }

    public override void SetDefaults() {
        Item.width = 120;
        Item.height = 100;
        Item.value = 10000;
        Item.rare = RarityLevelID.EarlyHM;
        Item.accessory = true;
        Item.expert = true;
        Item.value = Item.sellPrice(0, 8);
    }

    public override void UpdateVanity(Player player) {
        UpdateSpecialHead(player);
    }


    public override void UpdateAccessory(Player player, bool hideVisual) {
        if (!hideVisual) {
            UpdateSpecialHead(player);
        }
    }

    public float frameCounter;
    private void UpdateSpecialHead(Player player) {
        var headPlayer = player.GetModPlayer<SpecialHeadPlayer>();
        if (Math.Abs(player.velocity.X) < 0.2f && Math.Abs(player.velocity.Y) < 0.2f) {
            frameCounter = 0;
        }
        else {
            frameCounter += 0.1f;
            if ((int)frameCounter % 4 == 0) {
                frameCounter += 1f;
            }
        }
        SpecialHeadPlayer.HeadDataInfo headDataInfo = new SpecialHeadPlayer.HeadDataInfo {
            specialHeadTexture = ModContent.Request<Texture2D>($"{Texture}_Effect"),
            displyFrame = (int)frameCounter % 4
        };
        headPlayer.accHeadDataInfos.Add(headDataInfo);
    }
}