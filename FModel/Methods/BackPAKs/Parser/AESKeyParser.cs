﻿// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using FModel.Methods.BackupPAKs.Parser.AESKeyParser;
//
//    var aesKeyParser = AesKeyParser.FromJson(jsonString);

namespace FModel.Methods.BackupPAKs.Parser.AESKeyParser
{
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public class AesKeyParser
    {
        public static string[] FromJson(string json) => JsonConvert.DeserializeObject<string[]>(json, FModel.Methods.BackupPAKs.Parser.AESKeyParser.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this string[] self) => JsonConvert.SerializeObject(self, FModel.Methods.BackupPAKs.Parser.AESKeyParser.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
