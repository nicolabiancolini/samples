// See the LICENSE.TXT file in the project root for full license information.

using System.Reflection;
using Messages;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Logging;
using Rebus.Routing.TransportMessages;
using Rebus.Topic;

using var activator = new BuiltinHandlerActivator();
var configure = Configure.With(activator);
var assembly = Assembly.GetExecutingAssembly();
var multiTenantQueueName = assembly.GetCustomAttributes<AssemblyMetadataAttribute>().Single(m => m.Key == "MultiTenantQueueName").Value;
var rebusTransportConnectionString = assembly.GetCustomAttributes<AssemblyMetadataAttribute>().Single(m => m.Key == "RebusTransportConnectionString").Value;
configure
    .Logging(l => l.ColoredConsole(minLevel: LogLevel.Warn))
    .Transport(s => s.UseAzureServiceBus(rebusTransportConnectionString, multiTenantQueueName))
    .Routing(r => r.AddTransportMessageForwarder(async transportMessage =>
    {
        if (transportMessage.Headers.ContainsKey("tenant"))
        {
            return ForwardAction.ForwardTo(
                new MultiTenantTopicNameConvention(transportMessage.Headers["tenant"])
                .GetTopic(Type.GetType(transportMessage.Headers["rbs2-msg-type"])));
        }

        return ForwardAction.Ignore();
    }))
    .Start();
await activator.Bus.Subscribe<Message>().ConfigureAwait(false);

do
{
    Console.WriteLine("Do you want publish a message? [y/1/2/n]");
    var keyChar = char.ToLower(Console.ReadKey(true).KeyChar);
    var message = new Message();
    switch (keyChar)
    {
        case 'y':
            {
                await activator.Bus.Publish(message).ConfigureAwait(false);
            }

            break;
        case '1':
            {
                await activator.Bus.Publish(message, new Dictionary<string, string>
                {
                    { "tenant", "sub1" }
                }).ConfigureAwait(false);
            }

            break;
        case '2':
            {
                await activator.Bus.Publish(message, new Dictionary<string, string>
                {
                    { "tenant", "sub2" }
                }).ConfigureAwait(false);
            }

            break;
        case 'n':
            goto consideredHarmful;
    }

    Console.WriteLine($"Message {message.Uid} published");
}
while (true);
consideredHarmful:
Console.WriteLine("Quitting!");
