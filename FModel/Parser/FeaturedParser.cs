﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModel.Parser.Featured
{
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class FeaturedParser
    {
        [JsonProperty("export_type")]
        public string ExportType { get; set; }

        [JsonProperty("TileImage")]
        public ImageLol TileImage { get; set; }

        [JsonProperty("DetailsImage")]
        public ImageLol DetailsImage { get; set; }

        [JsonProperty("Gradient")]
        public Gradient Gradient { get; set; }

        [JsonProperty("Background")]
        public Background Background { get; set; }
    }

    public partial class Background
    {
        [JsonProperty("r")]
        public double R { get; set; }

        [JsonProperty("g")]
        public double G { get; set; }

        [JsonProperty("b")]
        public double B { get; set; }

        [JsonProperty("a")]
        public long A { get; set; }
    }

    public partial class ImageLol
    {
        [JsonProperty("ImageSize")]
        public ImageSize ImageSize { get; set; }

        [JsonProperty("ResourceObject")]
        public string ResourceObject { get; set; }
    }

    public partial class ImageSize
    {
        [JsonProperty("x")]
        public long X { get; set; }

        [JsonProperty("y")]
        public long Y { get; set; }
    }

    public partial class Gradient
    {
        [JsonProperty("Start")]
        public Background Start { get; set; }

        [JsonProperty("Stop")]
        public Background Stop { get; set; }
    }

    public partial class FeaturedParser
    {
        public static FeaturedParser[] FromJson(string json) => JsonConvert.DeserializeObject<FeaturedParser[]>(json, FModel.Parser.Featured.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this FeaturedParser[] self) => JsonConvert.SerializeObject(self, FModel.Parser.Featured.Converter.Settings);
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
