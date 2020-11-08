using Terraria;
using Terraria.Localization;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;


namespace Entrogic.Items.Equipables.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class 得摩斯头盔 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("得摩斯头盔");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 30;
            item.value = Item.sellPrice(0, 7, 50);
            item.rare = ItemRarityID.Green;
            item.defense = 15;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemType<得摩斯胸甲>() && legs.type == ItemType<得摩斯护胫>();
        }
        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Ranged) += 0.1f;
            player.GetCrit(DamageClass.Ranged) += 5;
            player.GetCrit(DamageClass.Magic) += 8;
            player.statManaMax2 += 40;
        }
        public override void UpdateArmorSet(Player player)
        { 
            player.setBonus = "\n" + Language.GetTextValue("Mods.Entrogic.得摩斯奖励") + 
                "\n" + Language.GetTextValue("Mods.Entrogic.得摩斯奖励2");
           德摩斯Item.twiceChance = true;
        }
        /*public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }*/
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position = Main.MouseWorld;
            Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI);
            return false;
        }
    }
    public class 德摩斯Item : GlobalItem
    {
        public static bool twiceChance = false;
        public static bool twice = false;
        public bool twice2 = false;
        public Item item;
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }
        public override void HoldItem(Item item, Player player)
        {
            if (twiceChance)
            {
                int timer = Main.rand.Next(0, 3);
                if (timer == 0 || timer == 1) twice = false;
                else twice = true;
            }
            else
            {
                twice = false;
            }
            if (item.DamageType == DamageClass.Ranged || item.DamageType == DamageClass.Magic)
            {
                if (twice && !twice2)
                {
                    item.useTime /= 2;
                    item.reuseDelay += 6;
                    twice2 = true;
                }
                if (!twice && twice2)
                {
                    item.useTime *= 2;
                    item.reuseDelay -= 6;
                    twice2 = false;
                }
            }
            this.item = item;
        }
        public override void SetDefaults(Item item)
        {

        }
        public override bool ConsumeAmmo(Item item, Player player)
        {
            if (twiceChance)
            {
                return !(player.itemAnimation < item.useAnimation - 2) && twice && item.DamageType == DamageClass.Ranged;
            }
            else
                return true;
        }
    }
}
