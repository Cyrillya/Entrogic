namespace Entrogic.Content.Items.Misc.Weapons.Melee.Swords
{
    public class FuSword : ModItem
    {
        public override void SetStaticDefaults() {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 114514;
        }

        public override void SetDefaults() {
            Item.width = 54;
            Item.height = 68;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = Item.useAnimation = 20;
            Item.damage = 16;
            Item.DamageType = DamageClass.Melee;
            Item.autoReuse = true;
            Item.crit += 5;
            Item.knockBack = 10f;
            Item.value = Item.buyPrice(3, 0, 0, 0);
            Item.shoot = ProjectileID.PurificationPowder;
        }

        public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit) {
            Vector2 distance = player.Center - target.Center;
            foreach (NPC npc in Main.npc)
                if (npc.boss)
                    return;
            Vector2 knockBackSpeed = Vector2.Zero;
            Terraria.Audio.SoundEngine.PlaySound(target.HitSound, target.position);
            if (target.type == NPCID.TargetDummy)
                return;
            for (int i = 0; i < 20; i++) {
                knockBackSpeed -= distance.ToRotation().ToRotationVector2();
                knockBackSpeed = Collision.TileCollision(target.position, knockBackSpeed, target.width, target.height);
                target.position += knockBackSpeed;
                target.netUpdate = true;
            }
        }
    }
}
