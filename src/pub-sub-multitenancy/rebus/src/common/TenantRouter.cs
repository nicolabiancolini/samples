// See the LICENSE.TXT file in the project root for full license information.

using Rebus.Messages;
using Rebus.Routing;
using Rebus.Topic;

public class TenantRouter : IRouter
{
    private readonly MultiTenantTopicNameConvention topicNameConvention;

    public TenantRouter(string tenant)
    {
        this.topicNameConvention = new MultiTenantTopicNameConvention(tenant);
    }

    public Task<string> GetDestinationAddress(Message message)
    {
        return Task.FromResult(this.topicNameConvention.GetTopic(typeof(Message)));
    }

    public Task<string> GetOwnerAddress(string topic)
    {
        throw new NotImplementedException();
    }
}
