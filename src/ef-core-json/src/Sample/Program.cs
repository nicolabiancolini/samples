// See the LICENSE.TXT file in the project root for full license information.

using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sample.Dtos;
using Sample.Entities;

namespace Sample
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });

            var optionsBuilder = new DbContextOptionsBuilder()
                .UseLoggerFactory(loggerFactory)
                .UseSqlServer(args[0])
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging();

            using var context = new DbContext(optionsBuilder.Options);

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

            await context.Cars.AddAsync(
                new Car("Test", new Metadata(new Retailer("Test"), Color.FromArgb(255, 255, 255), 100)),
                CancellationToken.None).ConfigureAwait(false);

            await context.CommitAsync(CancellationToken.None).ConfigureAwait(false);

            var car = await context.Cars.MaterializeAsync(car => car.NameOfRetailer == "Car Auto Orvieto").ConfigureAwait(false);

            Console.WriteLine($"Car:{Environment.NewLine}{JsonConvert.SerializeObject(car, Formatting.Indented)}");
        }
    }
}
