// See the LICENSE.TXT file in the project root for full license information.

using Xunit;

namespace Sample.IntegrationTests.Fixtures
{
    [CollectionDefinition(SqliteFixtureCollection.Name)]
    public class SqliteFixtureCollection : ICollectionFixture<SqliteContextFixture>
    {
        public const string Name = nameof(SqliteFixtureCollection);
    }
}
