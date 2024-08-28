// See the LICENSE.TXT file in the project root for full license information.

using System.Text.Json.Serialization;

#pragma warning disable SA1402
namespace Streaming.Cloudflare.WebApp.Services.Streaming.Models
{
    /// <summary>
    /// Todo.
    /// </summary>
    public partial class Video
    {
        [JsonPropertyName("allowedOrigins")]
        public string[] AllowedOrigins { get; set; } = default!;

        [JsonPropertyName("created")]
        public DateTimeOffset Created { get; set; }

        [JsonPropertyName("duration")]
        public double Duration { get; set; }

        [JsonPropertyName("input")]
        public Input Input { get; set; } = default!;

        [JsonPropertyName("maxSizeBytes")]
        public int? MaxSizeBytes { get; set; }

        [JsonPropertyName("maxDurationSeconds")]
        public int? MaxDurationSeconds { get; set; }

        [JsonPropertyName("meta")]
        public Meta Meta { get; set; } = default!;

        [JsonPropertyName("modified")]
        public DateTimeOffset Modified { get; set; }

        [JsonPropertyName("uploadExpiry")]
        public DateTimeOffset? UploadExpiry { get; set; }

        [JsonPropertyName("playback")]
        public Playback Playback { get; set; } = default!;

        [JsonPropertyName("preview")]
        public Uri Preview { get; set; } = default!;

        [JsonPropertyName("readyToStream")]
        public bool ReadyToStream { get; set; }

        [JsonPropertyName("requireSignedURLs")]
        public bool RequireSignedUrLs { get; set; }

        [JsonPropertyName("size")]
        public long Size { get; set; }

        [JsonPropertyName("status")]
        public Status Status { get; set; } = default!;

        [JsonPropertyName("thumbnail")]
        public Uri Thumbnail { get; set; } = default!;

        [JsonPropertyName("thumbnailTimestampPct")]
        public double ThumbnailTimestampPct { get; set; }

        [JsonPropertyName("uid")]
        public string Uid { get; set; } = default!;

        [JsonPropertyName("creator")]
        public string Creator { get; set; } = default!;

        [JsonPropertyName("liveInput")]
        public string LiveInput { get; set; } = default!;

        [JsonPropertyName("uploaded")]
        public DateTimeOffset Uploaded { get; set; }

        [JsonPropertyName("watermark")]
        public Watermark Watermark { get; set; } = default!;

        [JsonPropertyName("nft")]
        public Nft Nft { get; set; } = default!;
    }

    /// <summary>
    /// Todo.
    /// </summary>
    public partial class Input
    {
        [JsonPropertyName("height")]
        public long Height { get; set; }

        [JsonPropertyName("width")]
        public long Width { get; set; }
    }

    /// <summary>
    /// Todo.
    /// </summary>
    public partial class Meta
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("downloaded-from")]
        public Uri? DownloadedFrom { get; set; }
    }

    /// <summary>
    /// Todo.
    /// </summary>
    public partial class Nft
    {
        [JsonPropertyName("contract")]
        public string Contract { get; set; } = default!;

        [JsonPropertyName("token")]
        public long Token { get; set; }
    }

    /// <summary>
    /// Todo.
    /// </summary>
    public partial class Playback
    {
        [JsonPropertyName("hls")]
        public Uri Hls { get; set; } = default!;

        [JsonPropertyName("dash")]
        public Uri Dash { get; set; } = default!;
    }

    /// <summary>
    /// Todo.
    /// </summary>
    public partial class Status
    {
        [JsonPropertyName("state")]
        public string State { get; set; } = default!;

        [JsonPropertyName("pctComplete")]
        public string PctComplete { get; set; } = default!;

        [JsonPropertyName("errorReasonCode")]
        public string ErrorReasonCode { get; set; } = default!;

        [JsonPropertyName("errorReasonText")]
        public string ErrorReasonText { get; set; } = default!;
    }

    /// <summary>
    /// Todo.
    /// </summary>
    public partial class Watermark
    {
        [JsonPropertyName("uid")]
        public string Uid { get; set; } = default!;

        [JsonPropertyName("size")]
        public long Size { get; set; }

        [JsonPropertyName("height")]
        public long Height { get; set; }

        [JsonPropertyName("width")]
        public long Width { get; set; }

        [JsonPropertyName("created")]
        public DateTimeOffset Created { get; set; }

        [JsonPropertyName("downloadedFrom")]
        public Uri DownloadedFrom { get; set; } = default!;

        [JsonPropertyName("name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("opacity")]
        public double Opacity { get; set; }

        [JsonPropertyName("padding")]
        public double Padding { get; set; }

        [JsonPropertyName("scale")]
        public double Scale { get; set; }

        [JsonPropertyName("position")]
        public string Position { get; set; } = default!;
    }

    /// <summary>
    /// Todo.
    /// </summary>
    public partial class Caption
    {
        [JsonPropertyName("label")]
        public string Label { get; set; } = default!;

        [JsonPropertyName("language")]
        public string Language { get; set; } = default!;
    }
}
#pragma warning restore SA1402
