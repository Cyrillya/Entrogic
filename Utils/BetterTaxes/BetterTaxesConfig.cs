using BetterTaxes.UI;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader.Config;

namespace BetterTaxes
{
    public class InvalidConfigException : Exception
    {
        public static readonly string messageFormat = "Malformed config: {0}. See https://github.com/atenfyr/bettertaxes/wiki/Config-File-Format for more information.";
        public InvalidConfigException()
        {
        }

        public InvalidConfigException(string message) : base(string.Format(messageFormat, message))
        {
        }

        public InvalidConfigException(string message, Exception inner) : base(string.Format(messageFormat, message), inner)
        {
        }
    }

    public class SpecialIntConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (!(value is SpecialInt val)) val = new SpecialInt(100);
            writer.WriteValue(val.Value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            SpecialInt val = (SpecialInt)(long)reader.Value;
            if (val == null) val = new SpecialInt(100);
            return val;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(SpecialInt).IsAssignableFrom(objectType);
        }
    }

    /*
        SpecialInt only exists because I wanted to implement a slider which displays the correct units within ModConfig's DictionaryElement, but there's no way to do that without making a custom slider class, and you can't apply the attribute for the custom slider class onto int because int is a primitive
        The only way to circumvent this was to make a class which replicated the behavior of int so that I could apply attributes to it
    */
    [SliderColor(204, 181, 72)]
    [JsonConverter(typeof(SpecialIntConverter))]
    [CustomModConfigItem(typeof(SpecialIntRangeElement))]
    public class SpecialInt : IComparable<SpecialInt>
    {
        [JsonIgnore]
        public int Value { get; set; }

        public SpecialInt(int value)
        {
            Value = value;
            if (Value < 0) Value = 0;
        }

        public SpecialInt()
        {
            Value = 0;
        }

        public static implicit operator SpecialInt(int value)
        {
            return new SpecialInt(value);
        }

        public static implicit operator int(SpecialInt value)
        {
            return value.Value;
        }

        public static explicit operator SpecialInt(long value)
        {
            return new SpecialInt((int)value);
        }

        public static explicit operator long(SpecialInt value)
        {
            return value.Value;
        }

        public int CompareTo(SpecialInt that)
        {
            return Value.CompareTo(that.Value);
        }

        public bool Equals(SpecialInt other)
        {
            return Value == other.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}