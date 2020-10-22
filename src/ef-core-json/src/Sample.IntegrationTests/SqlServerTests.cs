// See the LICENSE.TXT file in the project root for full license information.

using System.Drawing;
using System.Threading.Tasks;
using Bogus;
using Sample.Dtos;
using Sample.Entities;
using Sample.IntegrationTests.Fixtures;
using Xunit;

namespace Sample.IntegrationTests
{
    [Collection(SqlServerFixtureCollection.Name)]
    public class SqlServerTests
    {
        private readonly SqlServerContextFixture fixture;

        public SqlServerTests(SqlServerContextFixture fixture)
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

            var retailer = new Faker<Retailer>().CustomInstantiator(faker => new Retailer(faker.Person.FullName));

            var metadata = new Faker<Metadata>()
                .CustomInstantiator(faker => new Metadata(
                    retailer,
                    Color.FromArgb(faker.Random.Number(255), faker.Random.Number(255), faker.Random.Number(255)),
                    faker.Random.Double(800, 3000)));

            foreach (var fakeCar in new Faker<Car>()
                .CustomInstantiator(faker => new Car(faker.Vehicle.Model(), metadata))
                .Generate(1000))
            {
                await context.Cars.AddAsync(fakeCar).ConfigureAwait(false);
            }

            await context.Cars.AddAsync(new Car("Fiat Panda 4x4", new Metadata(new Retailer("Car Auto Orvieto"), Color.FromArgb(255, 255, 255), 1240))).ConfigureAwait(false);

            // Act
            var car = await context.Cars.MaterializeAsync(car => car.NameOfRetailer == "Car Auto Orvieto").ConfigureAwait(false);

            // Assert
            Assert.NotNull(car);
        }
    }
}
