// See the LICENSE.TXT file in the project root for full license information.

using Rebus.AzureServiceBus;
using Rebus.Topic;

internal class MultiTenantTopicNameConvention : ITopicNameConvention
{
    private ITopicNameConvention defaultTopicNameConvention;
    private readonly string tenant;

    public MultiTenantTopicNameConvention(string tenant)
    {
        this.defaultTopicNameConvention = new DefaultAzureServiceBusTopicNameConvention();
        this.tenant = tenant;
    }

    public string GetTopic(Type eventType)
    {
        return $"{this.defaultTopicNameConvention.GetTopic(eventType)}.{this.tenant}";
    }
}
