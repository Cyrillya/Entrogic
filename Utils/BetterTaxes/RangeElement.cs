using System;
using System.Collections.Generic;
using Terraria.Localization;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config.UI;

namespace BetterTaxes.UI
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Class)]
    public class UnitsAttribute : Attribute
    {
        public Unit units;
        public UnitsAttribute(Unit unit)
        {
            units = unit;
        }
    }

    public enum Unit : byte
    {
        Coins,
        Time
    }

    public abstract class ChangedTextRangeElement<T> : RangeElement where T : IComparable<T>
    {
        public T min;
        public T max;
        public T increment;
        public IList<T> tList;

        public override void OnBind()
        {
            base.OnBind();
            tList = (IList<T>)list;
            TextDisplayFunction = () => TransformValue(GetValue(), memberInfo.Name);

            if (tList != null) TextDisplayFunction = () => TransformValue(tList[index], (index + 1).ToString());
            if (labelAttribute != null) TextDisplayFunction = () => TransformValue(GetValue(), labelAttribute.Label);
            if (rangeAttribute != null && rangeAttribute.min is T && rangeAttribute.max is T)
            {
                min = (T)rangeAttribute.min;
                max = (T)rangeAttribute.max;
            }
            if (incrementAttribute != null && incrementAttribute.increment is T) increment = (T)incrementAttribute.increment;
        }

        public virtual string TransformValue(T val, string label)
        {
            return label + ": " + val.ToString();
        }

        protected virtual T GetValue() => (T)GetObject();

        protected virtual void SetValue(object value)
        {
            if (value is T t) SetObject(value);
        }
    }

    public class SpecialIntRangeElement : RangeElement
    {
        public Unit units;
        public int min;
        public int max;
        public int increment;
        public IList<int> tList;
        public override int NumberTicks => ((max - min) / increment) + 1;
        public override float TickIncrement => (float)increment / (max - min);

        protected override float Proportion
        {
            get => (GetValue() - min) / (float)(max - min);
            set => SetValue((int)Math.Round((value * (max - min) + min) * (1f / increment)) * increment);
        }

        public override void OnBind()
        {
            base.OnBind();
            units = ConfigManager.GetCustomAttribute<UnitsAttribute>(memberInfo, item, list)?.units ?? Unit.Coins;
            tList = (IList<int>)list;
            TextDisplayFunction = () => TransformValue(GetValue(), memberInfo.Name);

            if (tList != null) TextDisplayFunction = () => TransformValue(tList[index], (index + 1).ToString());
            if (labelAttribute != null) TextDisplayFunction = () => TransformValue(GetValue(), labelAttribute.Label);
            if (rangeAttribute != null && rangeAttribute.min is int && rangeAttribute.max is int)
            {
                min = (int)rangeAttribute.min;
                max = (int)rangeAttribute.max;
            }
            if (incrementAttribute != null && incrementAttribute.increment is int) increment = (int)incrementAttribute.increment;
        }

        public string TransformValue(int val, string label)
        {
            string newLabel = label == "value" ? Language.GetTextValue("Mods.BetterTaxes.Config.Rent") : label;
            if (units == Unit.Time) return newLabel + ": " + UsefulThings.SecondsToHMS(val, "1 " + Language.GetTextValue("Mods.BetterTaxes.Config.Tick"));
            return newLabel + ": " + UsefulThings.ValueToCoins(val, (label == "value") ? ("0 " + Language.GetTextValue("LegacyInterface.18")) : Language.GetTextValue("Mods.BetterTaxes.Config.Unlimited"));
        }

        protected SpecialInt GetValue()
        {
            return (SpecialInt)GetObject();
        }

        protected void SetValue(object value)
        {
            if (value is int t) SetObject(new SpecialInt((int)value));
        }

        public SpecialIntRangeElement()
        {
            min = 1;
            max = 5000;
            increment = 50;
        }
    }

    public class BoostRangeElement : ChangedTextRangeElement<float>
    {
        public override int NumberTicks => (int)((max - min) / increment) + 1;
        public override float TickIncrement => (increment) / (max - min);

        protected override float Proportion
        {
            get => (GetValue() - min) / (max - min);
            set => SetValue((float)Math.Round((value * (max - min) + min) * (1 / increment)) * increment);
        }

        public BoostRangeElement()
        {
            min = 1f;
            max = 4f;
            increment = 0.05f;
        }

        public override string TransformValue(float val, string label)
        {
            if (val == 1) return label + ": " + Language.GetTextValue("Mods.BetterTaxes.Config.Disabled");
            return label + ": " + val + "×";
        }
    }
}
