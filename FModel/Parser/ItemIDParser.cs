﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModel.Parser.Items
{
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class ItemsIDParser
    {
        [JsonProperty("export_type")]
        public string ExportType { get; set; }

        [JsonProperty("cosmetic_item")]
        public string cosmetic_item { get; set; }

        [JsonProperty("CharacterParts")]
        public string[] CharacterParts { get; set; }

        [JsonProperty("HeroDefinition")]
        public string HeroDefinition { get; set; }

        [JsonProperty("WeaponDefinition")]
        public string WeaponDefinition { get; set; }

        [JsonProperty("Rarity")]
        public string Rarity { get; set; }

        [JsonProperty("DisplayName")]
        public string DisplayName { get; set; }

        [JsonProperty("ShortDescription")]
        public string ShortDescription { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("GameplayTags")]
        public GameplayTags GameplayTags { get; set; }

        [JsonProperty("SmallPreviewImage")]
        public PreviewImage SmallPreviewImage { get; set; }

        [JsonProperty("LargePreviewImage")]
        public PreviewImage LargePreviewImage { get; set; }

        [JsonProperty("DisplayAssetPath")]
        public DisplayAssetPath DisplayAssetPath { get; set; }
    }

    public partial class GameplayTags
    {
        [JsonProperty("gameplay_tags")]
        public string[] GameplayTagsGameplayTags { get; set; }
    }

    public partial class PreviewImage
    {
        [JsonProperty("asset_path_name")]
        public string AssetPathName { get; set; }

        [JsonProperty("sub_path_string")]
        public string SubPathString { get; set; }
    }

    public partial class DisplayAssetPath
    {
        [JsonProperty("asset_path_name")]
        public string AssetPathName { get; set; }

        [JsonProperty("sub_path_string")]
        public string SubPathString { get; set; }
    }

    public partial class ItemsIDParser
    {
        public static ItemsIDParser[] FromJson(string json) => JsonConvert.DeserializeObject<ItemsIDParser[]>(json, FModel.Parser.Items.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this ItemsIDParser[] self) => JsonConvert.SerializeObject(self, FModel.Parser.Items.Converter.Settings);
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
