// See the LICENSE.TXT file in the project root for full license information.

using System.Reflection;
using Messages;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Logging;
using Rebus.Routing.TypeBased;
using Rebus.Topic;
using Subscriber2;

using var activator = new BuiltinHandlerActivator();
activator.Register(() => new Handler());
var configure = Configure.With(activator);
var assembly = Assembly.GetExecutingAssembly();
var tenantQueueName = assembly.GetCustomAttributes<AssemblyMetadataAttribute>().Single(m => m.Key == "TenantQueueName").Value;
var tenant = assembly.GetCustomAttributes<AssemblyMetadataAttribute>().Single(m => m.Key == "Tenant").Value;
var rebusTransportConnectionString = assembly.GetCustomAttributes<AssemblyMetadataAttribute>().Single(m => m.Key == "RebusTransportConnectionString").Value;
configure
    .Logging(l => l.ColoredConsole(minLevel: LogLevel.Warn))
    .Transport(s => s.UseAzureServiceBus(rebusTransportConnectionString, tenantQueueName))
    .Routing(r => r.TypeBased().MapAssemblyOf<Message>(tenantQueueName))
    .Options(o =>
    {
        o.Decorate<ITopicNameConvention>(_ => new MultiTenantTopicNameConvention(tenant));
    })
    .Start();
await activator.Bus.Subscribe<Message>().ConfigureAwait(false);
Console.WriteLine($"This is {assembly.GetName().Name.Replace('.', ' ')}");
Console.WriteLine("Press ENTER to quit");
Console.ReadLine();
Console.WriteLine("Quitting...");
