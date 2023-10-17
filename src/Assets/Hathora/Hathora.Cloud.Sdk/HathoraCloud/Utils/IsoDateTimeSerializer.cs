
//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by Speakeasy (https://speakeasyapi.dev). DO NOT EDIT.
//
// Changes to this file may cause incorrect behavior and will be lost when
// the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
namespace HathoraCloud.Utils
{
    using System;
    using System.Globalization;
    using Newtonsoft.Json;

    internal class IsoDateTimeSerializer: JsonConverter
    {
        public override bool CanConvert(Type objectType) =>
            objectType == typeof(DateTime);

        public override bool CanRead => false;

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer) =>
            throw new NotImplementedException();

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteValue("null");
                return;
            }

            DateTime time = (DateTime)value;
            // The built-in Iso converter coerces to local time;
            // This standardizes to UTC.
            writer.WriteValue(time.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture));
        }
    }
}