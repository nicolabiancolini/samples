// See the LICENSE.TXT file in the project root for full license information.

using System;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;
using Streaming.Cloudflare.WebApp.Services.Streaming.Models;

namespace Streaming.Cloudflare.WebApp.Services.Streaming
{
    public class CloudflareStream
    {
        private readonly HttpClient client;

        public CloudflareStream(HttpClient client, IOptions<CloudflareStreamOptions> options)
        {
            this.client = client;
            this.client.BaseAddress = new Uri($"https://api.cloudflare.com/client/v4/accounts/{options.Value.AccountId}/stream/");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", options.Value.AccessToken);
        }

        public async Task<IEnumerable<Video>> ListVideosAsync(string? search = default, CancellationToken cancellationToken = default)
        {
            var builder = new QueryBuilder();
            if (!string.IsNullOrEmpty(search))
            {
                builder.Add("search", search);
            }

            return await this.ValidateResponseAsync<IEnumerable<Video>>(await this.client.GetAsync(builder.ToQueryString().Value), cancellationToken);
        }

        public async Task<Video> UploadFileAsync(string name, BinaryData data, CancellationToken cancellationToken = default)
        {
            using var multipartContent = new MultipartFormDataContent();
            multipartContent.Add(new ByteArrayContent(data.ToArray()), "file", name);

            return await this.ValidateResponseAsync<Video>(await this.client.PostAsync(
                string.Empty,
                multipartContent,
                cancellationToken));
        }

        public async Task<Video> CopyVideoAsync(string name, Uri url, bool requireSignedURLs = false, IReadOnlyCollection<Uri>? allowedOrigins = null, CancellationToken cancellationToken = default)
        {
            return await this.ValidateResponseAsync<Video>(await this.client.PostAsJsonAsync(
                "copy",
                new
                {
                    url = url,
                    meta = new
                    {
                        name = name,
                    },
                    requireSignedURLs = requireSignedURLs,
                    allowedOrigins = this.NormalizeAllowedOrigins(allowedOrigins ?? Array.Empty<Uri>())
                },
                cancellationToken));
        }

        public async Task<Video> UpdateVideoMetadataAsync(string uid, string? name, bool? requireSignedURLs = false, IReadOnlyCollection<Uri>? allowedOrigins = null, CancellationToken cancellationToken = default)
        {
            return await this.ValidateResponseAsync<Video>(await this.client.PostAsJsonAsync(
                uid,
                new
                {
                    meta = new
                    {
                        name = name,
                    },
                    requireSignedURLs = requireSignedURLs,
                    allowedOrigins = this.NormalizeAllowedOrigins(allowedOrigins ?? Array.Empty<Uri>())
                },
                cancellationToken));
        }

        public async Task<Video> RetrieveVideoDetailsAsync(string uid, CancellationToken cancellationToken = default)
        {
            return await this.ValidateResponseAsync<Video>(await this.client.GetAsync(uid), cancellationToken);
        }

        private async Task<T> ValidateResponseAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken = default)
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Cloudflare replied with:{Environment.NewLine}{(await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync())).RootElement}",
                    null,
                    response.StatusCode);
            }

            if (this.TryDeserializeResponseContent(await response.Content.ReadAsByteArrayAsync(cancellationToken), out T model))
            {
                return model;
            }

            throw new SerializationException();
        }

        private bool TryDeserializeResponseContent<T>(ReadOnlySpan<byte> bytes, out T model)
        {
            model = default!;
            var reader = new Utf8JsonReader(bytes);
            if (!JsonDocument.TryParseValue(ref reader, out var document))
            {
                return false;
            }

            if (!document!.RootElement.TryGetProperty("result", out var element))
            {
                return false;
            }

            model = element!.Deserialize<T>()!;

            return true;
        }

        private IEnumerable<string> NormalizeAllowedOrigins(IReadOnlyCollection<Uri> allowedOrigins)
        {
            return allowedOrigins.Select(uri => uri.IsAbsoluteUri ? uri.DnsSafeHost : uri.OriginalString);
        }
    }
}
