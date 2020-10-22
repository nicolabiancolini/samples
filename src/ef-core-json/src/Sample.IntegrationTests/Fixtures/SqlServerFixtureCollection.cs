// See the LICENSE.TXT file in the project root for full license information.

using Xunit;

namespace Sample.IntegrationTests.Fixtures
{
    [CollectionDefinition(SqlServerFixtureCollection.Name)]
    public class SqlServerFixtureCollection : ICollectionFixture<SqlServerContextFixture>
    {
        public const string Name = nameof(SqlServerFixtureCollection);
    }
}
