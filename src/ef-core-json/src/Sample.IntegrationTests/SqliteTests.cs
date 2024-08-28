// See the LICENSE.TXT file in the project root for full license information.

using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using Sample.Dtos;
using Sample.Entities;
using Sample.IntegrationTests.Fixtures;
using Xunit;

namespace Sample.IntegrationTests
{
    [Collection(SqliteFixtureCollection.Name)]
    public class SqliteTests : TestBase
    {
        private readonly SqliteContextFixture fixture;

        public SqliteTests(SqliteContextFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task Test()
        {
            // Arrange
#pragma warning disable CA2000 // Dispose objects before losing scope
            var context = this.fixture.CreateContext();
#pragma warning restore CA2000 // Dispose objects before losing scope

            await this.SeedAsync(context).ConfigureAwait(false);

            await context.Cars.AddAsync(new Car("Fiat Panda 4x4", new Metadata(new Retailer("Car Auto Orvieto"), Color.FromArgb(255, 255, 255), 1240))).ConfigureAwait(false);

            await context.CommitAsync(CancellationToken.None).ConfigureAwait(false);

            // Act
            var car = await context.Cars.MaterializeAsync(car => car.NameOfRetailer == "Car Auto Orvieto").ConfigureAwait(false);

            // Assert
            Assert.NotNull(car);
        }
    }
}
