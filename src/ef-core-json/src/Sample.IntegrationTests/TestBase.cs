// See the LICENSE.TXT file in the project root for full license information.

using System.Drawing;
using System.Threading.Tasks;
using Bogus;
using Sample.Dtos;
using Sample.Entities;

namespace Sample.IntegrationTests
{
    public class TestBase
    {
        protected virtual async Task SeedAsync(IContext context)
        {
            if (context is null)
            {
                throw new System.ArgumentNullException(nameof(context));
            }

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
        }
    }
}
