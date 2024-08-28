// See the LICENSE.TXT file in the project root for full license information.

using Xunit;

namespace Sample.IntegrationTests.Fixtures
{
    [CollectionDefinition(PostgreSqlFixtureCollection.Name)]
    public class PostgreSqlFixtureCollection : ICollectionFixture<PostgreSqlContextFixture>
    {
        public const string Name = nameof(PostgreSqlFixtureCollection);
    }
}
