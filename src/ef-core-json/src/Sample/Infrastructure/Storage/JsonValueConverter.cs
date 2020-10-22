// See the LICENSE.TXT file in the project root for full license information.

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace Sample.Infrastructure.Storage
{
    internal class JsonValueConverter<T> : ValueConverter<T, string>
    {
        private static readonly JsonSerializerSettings SerializerOptions = new JsonSerializerSettings
        {
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
        };

        public JsonValueConverter(JsonSerializerSettings serializerOptions = null, ConverterMappingHints mappingHints = null)
            : base(value => JsonConvert.SerializeObject(value, serializerOptions ?? SerializerOptions), value => JsonConvert.DeserializeObject<T>(value, serializerOptions ?? SerializerOptions), mappingHints)
        {
        }
    }
}
