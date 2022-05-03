using Entrogic.Content.DamageClasses;
using System.Linq;

namespace Entrogic.Content.Items.BaseTypes
{
    public abstract class Equippable : ItemBase
    {
        public override void Load() {
            base.Load();
            Translation.RegisterTranslation("Common.KnockbackMultiplier", GameCulture.CultureName.Chinese, "%击退力", "% knockback");
            Translation.RegisterTranslation("Common.WhipKnockbackMultiplier", GameCulture.CultureName.Chinese, "鞭子击退力", "whip knockback");
            Translation.RegisterTranslation("Common.WhipSpeedMultiplier", GameCulture.CultureName.Chinese, "鞭子速度", "whip speed");
            Translation.RegisterTranslation("Common.WhipRangeMultiplier", GameCulture.CultureName.Chinese, "{0}% 鞭子范围", "{0}% whip range");
            Translation.RegisterTranslation("Common.MeleeSpeedMultiplier", GameCulture.CultureName.Chinese, "{0}% 近战速度", "{0}% melee speed");
            Translation.RegisterTranslation("Common.MoveSpeedMultiplier", GameCulture.CultureName.Chinese, "{0}% 移动速度", "{0}% move speed");
            Translation.RegisterTranslation("Common.ManaCostMuiltpiler", GameCulture.CultureName.Chinese, "{0}% 魔力消耗", "{0}% mana usage");
            Translation.RegisterTranslation("Common.MinionCapacityAdditive", GameCulture.CultureName.Chinese, "{0} 仆从容量", "{0} minion capacity");
            Translation.RegisterTranslation("Common.ManaCapacityAdditive", GameCulture.CultureName.Chinese, "{0} 魔力上限", "{0} mana capacity");
            Translation.RegisterTranslation("Common.HealthCapacityAdditive", GameCulture.CultureName.Chinese, "{0} 生命值上限", "{0} life max");

            Translation.RegisterTranslation("Common.AutoSwingGeneric", GameCulture.CultureName.Chinese, "为武器启用自动挥舞", "Enables auto swing for weapons");
            Translation.RegisterTranslation("Common.AutoSwingSummonMeleeSpeed", GameCulture.CultureName.Chinese, "为鞭子启用自动挥舞", "Enables auto swing for whips");
            Translation.RegisterTranslation("Common.AutoJump", GameCulture.CultureName.Chinese, "允许自动跳跃", "Allows auto jump");
        }

        internal class Benefit
        {
            internal Dictionary<DamageClass, float> Damage = new();
            internal Dictionary<DamageClass, int> Crit = new();
            internal Dictionary<DamageClass, float> Speed = new();
            internal Dictionary<DamageClass, float> Knockback = new();
            internal List<DamageClass> AutoSwing = new();
            internal float WhipRangeMultiplier;
            internal float MoveSpeedMuiltpiler;
            internal float ManaCostMuiltpiler;
            internal int MinionCapacityAdditive;
            internal int ManaCapacityAdditive;
            internal int HealthCapacityAdditive;
            internal bool AutoJump;

            public void Clear() {
                Damage.Clear();
                Crit.Clear();
                Speed.Clear();
                Knockback.Clear();
                AutoSwing.Clear();
                WhipRangeMultiplier = 0f;
                MoveSpeedMuiltpiler = 0f;
                ManaCostMuiltpiler = 0f;
                MinionCapacityAdditive = 0;
                ManaCapacityAdditive = 0;
                HealthCapacityAdditive = 0;
                AutoJump = false;
            }
        }

        internal Benefit EquipBenefits = new();
        internal Benefit ArmorSetBenefits = new();
        internal string ArmorSetExtraTip;

        internal void DamageModify(DamageClass damageClass, float damage, bool armorSet = false) {
            if (!armorSet) {
                if (!EquipBenefits.Damage.ContainsKey(damageClass)) {
                    EquipBenefits.Damage.Add(damageClass, damage);
                }
                else {
                    throw new ArgumentException("这个伤害类型已经注册过damage了，armorSet = false");
                }
            }
            else {
                if (!ArmorSetBenefits.Damage.ContainsKey(damageClass)) {
                    ArmorSetBenefits.Damage.Add(damageClass, damage);
                }
                else {
                    throw new ArgumentException("这个伤害类型已经注册过damage了，armorSet = true");
                }
            }
        }

        internal void CritChanceModify(DamageClass damageClass, int crit, bool armorSet = false) {
            if (!damageClass.UseStandardCritCalcs) {
                throw new Exception("该伤害类型不支持一般暴击形式");
            }
            ref var benefits = ref EquipBenefits;
            if (armorSet) {
                benefits = ref ArmorSetBenefits;
            }
            if (!benefits.Crit.ContainsKey(damageClass)) {
                benefits.Crit.Add(damageClass, crit);
            }
            else {
                throw new ArgumentException($"这个伤害类型已经注册过crit了，armorSet = {armorSet}");
            }
        }

        internal void SpeedModify(DamageClass damageClass, float speed, bool armorSet = false) {
            if (!armorSet) {
                if (!EquipBenefits.Speed.ContainsKey(damageClass)) {
                    EquipBenefits.Speed.Add(damageClass, speed);
                }
                else {
                    throw new ArgumentException("这个伤害类型已经注册过speed了，armorSet = false");
                }
            }
            else {
                if (!ArmorSetBenefits.Speed.ContainsKey(damageClass)) {
                    ArmorSetBenefits.Speed.Add(damageClass, speed);
                }
                else {
                    throw new ArgumentException("这个伤害类型已经注册过speed了，armorSet = true");
                }
            }
        }

        internal void KnockbackModify(DamageClass damageClass, float knockback, bool armorSet = false) {
            if (!armorSet) {
                if (!EquipBenefits.Knockback.ContainsKey(damageClass)) {
                    EquipBenefits.Knockback.Add(damageClass, knockback);
                }
                else {
                    throw new ArgumentException("这个伤害类型已经注册过knockback了，armorSet = false");
                }
            }
            else {
                if (!ArmorSetBenefits.Knockback.ContainsKey(damageClass)) {
                    ArmorSetBenefits.Knockback.Add(damageClass, knockback);
                }
                else {
                    throw new ArgumentException("这个伤害类型已经注册过knockback了，armorSet = false");
                }
            }
        }

        internal void AutoSwingModify(DamageClass damageClass, bool armorSet = false) {
            if (!armorSet) {
                if (!EquipBenefits.AutoSwing.Contains(damageClass)) {
                    EquipBenefits.AutoSwing.Add(damageClass);
                }
                else {
                    throw new ArgumentException("这个伤害类型已经注册过autoswing了，armorSet = false");
                }
            }
            else {
                if (!ArmorSetBenefits.AutoSwing.Contains(damageClass)) {
                    ArmorSetBenefits.AutoSwing.Add(damageClass);
                }
                else {
                    throw new ArgumentException("这个伤害类型已经注册过autoswing了，armorSet = true");
                }
            }
        }

        public override void SetStaticDefaults() {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        private void ApplyBonus(Player player, bool armorSet = false, bool edittingTooltip = false) {
            var modPlayer = player.GetModPlayer<AutoswingHandler>();

            EquipBenefits.Clear();
            ArmorSetBenefits.Clear();
            SetBonus(player, armorSet);
            if (edittingTooltip) return;

            Benefit benefits = armorSet ? ArmorSetBenefits : EquipBenefits;
            foreach (var item in benefits.Damage) {
                player.GetDamage(item.Key) += item.Value;
            }
            foreach (var item in benefits.Crit) {
                player.GetCritChance(item.Key) += item.Value;
            }
            foreach (var item in benefits.Speed) {
                player.GetAttackSpeed(item.Key) += item.Value;
            }
            foreach (var item in benefits.Knockback) {
                player.GetKnockback(item.Key) += item.Value;
            }
            foreach (var item in from i in benefits.AutoSwing where !modPlayer.AllowAutoSwing.Contains(i) select i) {
                modPlayer.AllowAutoSwing.Add(item);
            }
            player.whipRangeMultiplier += benefits.WhipRangeMultiplier;
            player.moveSpeed += benefits.MoveSpeedMuiltpiler;
            player.manaCost += benefits.ManaCostMuiltpiler;
            player.maxMinions += benefits.MinionCapacityAdditive;
            player.statManaMax2 += benefits.ManaCapacityAdditive;
            player.statLifeMax2 += benefits.HealthCapacityAdditive;
            player.autoJump = benefits.AutoJump || player.autoJump;
        }

        public virtual void SetBonus(Player player, bool inArmorSet) { }

        public override void UpdateEquip(Player player) {
            base.UpdateEquip(player);

            ApplyBonus(player, false);
        }

        public override void UpdateArmorSet(Player player) {
            base.UpdateArmorSet(player);

            ApplyBonus(player, true);
            var bonus = GenerateBonusTooltip(ArmorSetBenefits);
            if (bonus != string.Empty) {
                bonus += "\n"; // 如果有加成文本才换行
            }
            player.setBonus = $"\n{bonus}{ArmorSetExtraTip}";
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            base.ModifyTooltips(tooltips);
            ApplyBonus(Main.LocalPlayer, false, true);
            bool inVanity = false;
            // 如果在社交栏，不显示属性
            foreach (TooltipLine line in tooltips) {
                if (line.Mod == "Terraria" && (line.Name == "Social" || line.Name == "SocialDesc")) {
                    inVanity = true;
                }
                //Main.NewText(tooltips.IndexOf(line) + " " + line.Name);
            }
            if (inVanity) return;

            // 如果找到“套装奖励”，就把我们的放在这前面
            foreach (TooltipLine line in tooltips) {
                if (line.Mod == "Terraria" && (line.Name == "SetBonus")) {
                    tooltips.Insert(tooltips.IndexOf(line), new TooltipLine(Mod, "ArmorTooltip", GenerateBonusTooltip(EquipBenefits)));
                    return;
                }
            }
            TooltipLine tooltipLine = new(Mod, "ArmorTooltip", GenerateBonusTooltip(EquipBenefits));
            tooltips.Add(tooltipLine);
        }

        /// <summary>
        /// 用于根据伤害类型DisplayName获得其他文本（比如远程暴击率，近战范围之类）
        /// </summary>
        /// <param name="text">伤害类型DisplayName</param>
        /// <param name="translateKey">目标文本的翻译key（值中应包括%）</param>
        /// <returns></returns>
        private string GetTooltipText(string text, string translateKey) {
            var targetText = Language.GetTextValue(translateKey);
            var damageText = Language.GetTextValue("LegacyTooltip.39"); // %伤害 | % damage
            // 删除开头的 %，用剩下的文本（伤害）替换XX伤害中的“伤害”，获得纯职业文本（例：近战伤害->近战）
            // 无职业武器获得一个空格
            var classOnlyText = text.Replace(damageText.Remove(0, 1).Trim(), ""); // 注意英文等语言开头还有个空格
            // targetText去掉开头 % 后，和classOnlyText合起来
            // 还有删除多余空格（两个或以上空格连一起、首位空格）网上抄的，不太懂哦
            return System.Text.RegularExpressions.Regex.Replace(($"{classOnlyText}{targetText.Remove(0, 1)}").Trim(), @"\b\s+\b", " ");
        }


        internal string GenerateBonusTooltip(Benefit benefits) {
            List<string> tooltips = new();
            foreach (var item in benefits.Damage) {
                tooltips.Add($"{item.Value * 100f:+#;-#;0}% {item.Key.DisplayName}");
            }
            foreach (var item in benefits.Crit) {
                // LegacyTooltip.41 | %暴击率 | % critical strike chance
                string name = item.Key is ArcaneDamageClass ? ArcaneDamageClass.DisplayClassName : item.Key.DisplayName;
                tooltips.Add($"{item.Value:+#;-#;0}% {GetTooltipText(name, "LegacyTooltip.41")}");
            }
            foreach (var item in benefits.Speed) {
                if (item.Key == DamageClass.SummonMeleeSpeed) {
                    // 对于召唤鞭子类加成，文本改成“鞭子速度”
                    tooltips.Add($"{item.Value * 100f:+#;-#;0}% {Language.GetTextValue("Mods.Entrogic.Common.WhipSpeedMultiplier")}");
                    continue;
                }
                // LegacyTooltip.40 | %速度 | % speed
                string name = item.Key is ArcaneDamageClass ? ArcaneDamageClass.DisplayClassName : item.Key.DisplayName;
                tooltips.Add($"{item.Value * 100f:+#;-#;0}% {GetTooltipText(name, "LegacyTooltip.40")}");
            }
            foreach (var item in benefits.Knockback) {
                if (item.Key == DamageClass.SummonMeleeSpeed) {
                    // 对于召唤鞭子类加成，文本改成“鞭子击退力”
                    tooltips.Add($"{item.Value * 100f:+#;-#;0}% {Language.GetTextValue("Mods.Entrogic.Common.WhipKnockbackMultiplier")}");
                    continue;
                }
                // 原版没有，得用我们自己的文本
                string name = item.Key is ArcaneDamageClass ? ArcaneDamageClass.DisplayClassName : item.Key.DisplayName;
                tooltips.Add($"{item.Value * 100f:+#;-#;0}% {GetTooltipText(name, Language.GetTextValue("Mods.Entrogic.Common.KnockbackMultiplier"))}");
            }
            if (benefits.WhipRangeMultiplier != 0f) {
                tooltips.Add(string.Format(Language.GetTextValue("Mods.Entrogic.Common.WhipRangeMultiplier"), (benefits.WhipRangeMultiplier * 100).ToString("+#;-#;0")));
            }
            if (benefits.MoveSpeedMuiltpiler != 0f) {
                tooltips.Add(string.Format(Language.GetTextValue("Mods.Entrogic.Common.MoveSpeedMultiplier"), (benefits.MoveSpeedMuiltpiler * 100).ToString("+#;-#;0")));
            }
            if (benefits.ManaCostMuiltpiler != 0f) {
                tooltips.Add(string.Format(Language.GetTextValue("Mods.Entrogic.Common.ManaCostMuiltpiler"), (benefits.ManaCostMuiltpiler * 100).ToString("+#;-#;0")));
            }
            if (benefits.MinionCapacityAdditive != 0) {
                tooltips.Add(string.Format(Language.GetTextValue("Mods.Entrogic.Common.MinionCapacityAdditive"), benefits.MinionCapacityAdditive.ToString("+#;-#;0")));
            }
            if (benefits.ManaCapacityAdditive != 0) {
                tooltips.Add(string.Format(Language.GetTextValue("Mods.Entrogic.Common.ManaCapacityAdditive"), benefits.ManaCapacityAdditive.ToString("+#;-#;0")));
            }
            if (benefits.HealthCapacityAdditive != 0) {
                tooltips.Add(string.Format(Language.GetTextValue("Mods.Entrogic.Common.HealthCapacityAdditive"), benefits.HealthCapacityAdditive.ToString("+#;-#;0")));
            }
            foreach (var item in benefits.AutoSwing) {
                if (item == DamageClass.SummonMeleeSpeed || item == DamageClass.Generic) {
                    // 对于召唤鞭子类加成和全职业加成，用我们自己的文本，Name后面会包括DamageClass也要去掉
                    tooltips.Add(Language.GetTextValue($"Mods.Entrogic.Common.AutoSwing{item.Name.Replace("DamageClass", "")}"));
                    continue;
                }

                // 用猛爪手套的Tooltip获取自动挥舞文本，这时候还带有“近战”词条
                var originalMeleedText = Language.GetTextValue("ItemTooltip.FeralClaws").Split('\n')[1];
                var damageText = Language.GetTextValue("LegacyTooltip.39"); // %伤害 | % damage

                // 拿原版的“ 近战伤害”把伤害去掉获得“ 近战”，再去掉首尾空格获得“近战”
                var meleeText = DamageClass.Melee.DisplayName
                    .Replace(damageText.Remove(0, 1), "")
                    .Trim();

                var damageClassText = item.DisplayName
                    .Replace(damageText.Remove(0, 1), "")
                    .Trim(); // 获得我们自己职业的伤害文本

                var originalText = originalMeleedText.Replace(meleeText, damageClassText); // “近战”文本全部替换为我们自己职业的文本

                tooltips.Add(System.Text.RegularExpressions.Regex.Replace(originalText, @"\b\s+\b", " ")); // 去空格并加到tooltips
            }
            if (benefits.AutoJump) {
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
