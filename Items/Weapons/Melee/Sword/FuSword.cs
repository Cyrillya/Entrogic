using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Entrogic.Items.Weapons.Melee.Sword
{
    public class FuSword : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 54;
            item.height = 68;
            item.useStyle = ItemUseStyleID.Swing;
            item.useTime = item.useAnimation = 20;
            item.damage = 16;
            item.DamageType = DamageClass.Melee;
            item.autoReuse = true;
            item.crit += 5;
            item.knockBack = 10f;
            item.value = Item.buyPrice(3, 0, 0, 0);
            item.shoot = ProjectileID.PurificationPowder;
        }
        public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
        {
            Vector2 distance = player.Center - target.Center;
            foreach (NPC npc in Main.npc)
                if (npc.boss)
                    return;
            Vector2 knockBackSpeed = Vector2.Zero;
            Terraria.Audio.SoundEngine.PlaySound(target.HitSound, target.position);
            if (target.type == NPCID.TargetDummy)
                return;
            for (int i = 0; i < 20; i++)
            {
                knockBackSpeed -= distance.ToRotation().ToRotationVector2();
                knockBackSpeed = Collision.TileCollision(target.position, knockBackSpeed, target.width, target.height);
                target.position += knockBackSpeed;
                target.netUpdate = true;
            }
        }
    }
}
