namespace Entrogic.Content.Items.BaseTypes
{
    public abstract class Equippable : ItemBase
	{
		public override void Load() {
			base.Load();
			Translation.RegisterTranslation("Common.Critical", GameCulture.CultureName.Chinese, "暴击率", "critical strike chance");
			Translation.RegisterTranslation("Common.Damage", GameCulture.CultureName.Chinese, "伤害", "damage");
			Translation.RegisterTranslation("Common.MeleeSpeedMultiplier", GameCulture.CultureName.Chinese, "{0}% 近战速度", "{0}% melee speed");
			Translation.RegisterTranslation("Common.WhipSpeedMultiplier", GameCulture.CultureName.Chinese, "{0}% 鞭子速度", "{0}% whip speed");
			Translation.RegisterTranslation("Common.WhipRangeMultiplier", GameCulture.CultureName.Chinese, "{0}% 鞭子范围", "{0}% whip range");
			Translation.RegisterTranslation("Common.MoveSpeedMultiplier", GameCulture.CultureName.Chinese, "{0}% 移动速度", "{0}% move speed");
			Translation.RegisterTranslation("Common.ManaCostMuiltpiler", GameCulture.CultureName.Chinese, "{0}% 魔力消耗", "{0}% mana usage");
			Translation.RegisterTranslation("Common.MinionCapacityAdditive", GameCulture.CultureName.Chinese, "{0} 仆从容量", "{0} minion capacity");
			Translation.RegisterTranslation("Common.ManaCapacityAdditive", GameCulture.CultureName.Chinese, "{0} 魔力上限", "{0} mana capacity");
			Translation.RegisterTranslation("Common.HealthCapacityAdditive", GameCulture.CultureName.Chinese, "{0} 生命值上限", "{0} life max");

			Translation.RegisterTranslation("Common.AutoSwing", GameCulture.CultureName.Chinese, "为武器启用自动挥舞", "Enables auto swing for weapons");
			Translation.RegisterTranslation("Common.AutoSwingWhip", GameCulture.CultureName.Chinese, "为鞭子启用自动挥舞", "Enables auto swing for whips");
			Translation.RegisterTranslation("Common.AutoSwingMelee", GameCulture.CultureName.Chinese, "为近战武器启用自动挥舞", "Enables auto swing for melee weapons");
			Translation.RegisterTranslation("Common.AutoSwingRanged", GameCulture.CultureName.Chinese, "为远程武器启用自动挥舞", "Enables auto swing for ranged weapons");
			Translation.RegisterTranslation("Common.AutoJump", GameCulture.CultureName.Chinese, "允许自动跳跃", "Allows auto jump");
		}

		internal class Damages
        {
			internal Dictionary<DamageClass, float> damage = new Dictionary<DamageClass, float>();
			internal Dictionary<DamageClass, int> crit = new Dictionary<DamageClass, int>();
			internal float meleeSpeedMultiplier;
			internal float whipSpeedMultiplier;
			internal float whipRangeMultiplier;
			internal float moveSpeedMuiltpiler;
			internal float manaCostMuiltpiler;
			internal int minionCapacityAdditive;
			internal int manaCapacityAdditive;
			internal int healthCapacityAdditive;
			internal bool autoSwing;
			internal bool autoSwingWhip;
			internal bool autoSwingMelee;
			internal bool autoSwingRanged;
			internal bool autoJump;

			public void Clear() {
				damage.Clear();
				crit.Clear();
				meleeSpeedMultiplier = 0f;
				whipSpeedMultiplier = 0f;
				whipRangeMultiplier = 0f;
				moveSpeedMuiltpiler = 0f;
				manaCostMuiltpiler = 0f;
				minionCapacityAdditive = 0;
				manaCapacityAdditive = 0;
				healthCapacityAdditive = 0;
				autoSwing = false;
				autoSwingWhip = false;
				autoSwingMelee = false;
				autoSwingRanged = false;
				autoJump = false;
			}
		}

		internal Damages equipDamages = new Damages();
		internal Damages armorSetDamages = new Damages();
		internal string armorSetExtra;

        internal void DamageModify(DamageClass damageClass, float damage, bool armorSet = false) {
			if (!armorSet) {
				if (!equipDamages.damage.ContainsKey(damageClass)) {
					equipDamages.damage.Add(damageClass, damage);
				}
				else {
					throw new ArgumentException("这个伤害类型已经注册过damage了，armorSet = false");
				}
            }
            else {
				if (!armorSetDamages.damage.ContainsKey(damageClass)) {
					armorSetDamages.damage.Add(damageClass, damage);
				}
				else {
					throw new ArgumentException("这个伤害类型已经注册过damage了，armorSet = true");
				}
			}
		}

		internal void CritChanceModify(DamageClass damageClass, int crit, bool armorSet = false) {
			if (!armorSet) {
				if (!equipDamages.crit.ContainsKey(damageClass)) {
					equipDamages.crit.Add(damageClass, crit);
				}
				else {
					throw new ArgumentException("这个伤害类型已经注册过crit了，armorSet = false");
				}
			}
			else {
				if (!armorSetDamages.crit.ContainsKey(damageClass)) {
					armorSetDamages.crit.Add(damageClass, crit);
				}
				else {
					throw new ArgumentException("这个伤害类型已经注册过crit了，armorSet = true");
				}
			}
		}

		public override void SetStaticDefaults() {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		private void ApplyBonus(Player player, bool armorSet = false, bool edittingTooltip = false) {
			equipDamages.Clear();
			armorSetDamages.Clear();
			SetBonus(player, armorSet);
			if (edittingTooltip) return;

			Damages damages = armorSet ? armorSetDamages : equipDamages;
			foreach (var item in damages.damage) {
				player.GetDamage(item.Key) += item.Value;
			}
			foreach (var item in damages.crit) {
				player.GetCritChance(item.Key) += item.Value;
			}
			player.meleeSpeed += damages.meleeSpeedMultiplier;
			player.whipRangeMultiplier += damages.whipRangeMultiplier;
			player.whipUseTimeMultiplier *= 1f / ( 1f + damages.whipSpeedMultiplier);
			player.moveSpeed += damages.moveSpeedMuiltpiler;
			player.manaCost += damages.manaCostMuiltpiler;
			player.maxMinions += damages.minionCapacityAdditive;
			player.statManaMax2 += damages.manaCapacityAdditive;
			player.statLifeMax2 += damages.healthCapacityAdditive;
			player.autoJump = damages.autoJump ? true : player.autoJump;
			player.GetModPlayer<AutoswingHandler>().allowAutoReuse = damages.autoSwing;
			player.GetModPlayer<AutoswingHandler>().allowAutoReuseWhip = damages.autoSwingWhip;
			player.GetModPlayer<AutoswingHandler>().allowAutoReuseMelee = damages.autoSwingMelee;
			player.GetModPlayer<AutoswingHandler>().allowAutoReuseRanged = damages.autoSwingRanged;
		}

		public virtual void SetBonus(Player player, bool inArmorSet) { }

        public override void UpdateEquip(Player player) {
            base.UpdateEquip(player);

			ApplyBonus(player, false);
		}

        public override void UpdateArmorSet(Player player) {
            base.UpdateArmorSet(player);

			ApplyBonus(player, true);
			player.setBonus = $"\n{GenerateBonusTooltip(armorSetDamages)}{armorSetExtra}";
		}

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            base.ModifyTooltips(tooltips);
			ApplyBonus(Main.LocalPlayer, false, true);
			bool inVanity = false;
			// 如果在社交栏，不显示属性
			foreach (TooltipLine line in tooltips) {
				if (line.mod == "Terraria" && (line.Name == "Social" || line.Name == "SocialDesc")) {
					inVanity = true;
				}
				//Main.NewText(tooltips.IndexOf(line) + " " + line.Name);
			}
			if (inVanity) return;

			// 如果找到“套装奖励”，就把我们的放在这前面
			foreach (TooltipLine line in tooltips) {
				if (line.mod == "Terraria" && (line.Name == "SetBonus")) {
					tooltips.Insert(tooltips.IndexOf(line), new TooltipLine(Mod, "ArmorTooltip", GenerateBonusTooltip(equipDamages)));
					return;
				}
			}
			TooltipLine tooltipLine = new TooltipLine(Mod, "ArmorTooltip", GenerateBonusTooltip(equipDamages));
			tooltips.Add(tooltipLine);
		}

		private string ReplaceDamageDisplyName(string text) {
			return text.Replace(Language.GetTextValue("Mods.Entrogic.Common.Damage"), Language.GetTextValue("Mods.Entrogic.Common.Critical"));
        }

		internal string GenerateBonusTooltip(Damages damages) {
			List<string> tooltips = new List<string>();
			foreach (var item in damages.damage) {
				tooltips.Add($"{item.Value * 100f:+#;-#;0}% {item.Key.DisplayName}");
			}
			foreach (var item in damages.crit) {
				tooltips.Add($"{item.Value:+#;-#;0}% {ReplaceDamageDisplyName(item.Key.DisplayName)}");
			}
			if (damages.meleeSpeedMultiplier != 0f) {
				tooltips.Add(string.Format(Language.GetTextValue("Mods.Entrogic.Common.MeleeSpeedMultiplier"), (damages.meleeSpeedMultiplier * 100).ToString("+#;-#;0")));
			}
			if (damages.whipRangeMultiplier != 0f) {
				tooltips.Add(string.Format(Language.GetTextValue("Mods.Entrogic.Common.WhipRangeMultiplier"), (damages.whipRangeMultiplier * 100).ToString("+#;-#;0")));
			}
			if (damages.whipSpeedMultiplier != 0f) {
				tooltips.Add(string.Format(Language.GetTextValue("Mods.Entrogic.Common.WhipSpeedMultiplier"), (damages.whipSpeedMultiplier * 100).ToString("+#;-#;0")));
			}
			if (damages.moveSpeedMuiltpiler != 0f) {
				tooltips.Add(string.Format(Language.GetTextValue("Mods.Entrogic.Common.MoveSpeedMultiplier"), (damages.moveSpeedMuiltpiler * 100).ToString("+#;-#;0")));
			}
			if (damages.manaCostMuiltpiler != 0f) {
				tooltips.Add(string.Format(Language.GetTextValue("Mods.Entrogic.Common.ManaCostMuiltpiler"), (damages.manaCostMuiltpiler * 100).ToString("+#;-#;0")));
			}
			if (damages.minionCapacityAdditive != 0) {
				tooltips.Add(string.Format(Language.GetTextValue("Mods.Entrogic.Common.MinionCapacityAdditive"), damages.minionCapacityAdditive.ToString("+#;-#;0")));
			}
			if (damages.manaCapacityAdditive != 0) {
				tooltips.Add(string.Format(Language.GetTextValue("Mods.Entrogic.Common.ManaCapacityAdditive"), damages.manaCapacityAdditive.ToString("+#;-#;0")));
			}
			if (damages.healthCapacityAdditive != 0) {
				tooltips.Add(string.Format(Language.GetTextValue("Mods.Entrogic.Common.HealthCapacityAdditive"), damages.healthCapacityAdditive.ToString("+#;-#;0")));
			}
			if (damages.autoSwing) {
				tooltips.Add(string.Format(Language.GetTextValue("Mods.Entrogic.Common.AutoSwing")));
			}
			if (damages.autoSwingWhip) {
				tooltips.Add(string.Format(Language.GetTextValue("Mods.Entrogic.Common.AutoSwingWhip")));
			}
			if (damages.autoSwingMelee) {
				tooltips.Add(string.Format(Language.GetTextValue("Mods.Entrogic.Common.AutoSwingMelee")));
			}
			if (damages.autoSwingRanged) {
				tooltips.Add(string.Format(Language.GetTextValue("Mods.Entrogic.Common.AutoSwingRanged")));
			}
			if (damages.autoJump) {
				tooltips.Add(string.Format(Language.GetTextValue("Mods.Entrogic.Common.AutoJump")));
			}
			string tooltip = "";
			if (tooltips.Count > 0) {
				tooltip = tooltips[0];
				tooltips.RemoveAt(0);
				foreach (var tip in tooltips) {
					tooltip += $"\n{tip}";
                }
			}
			return tooltip;
		}
    }
}
