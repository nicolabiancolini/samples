// See the LICENSE.TXT file in the project root for full license information.

using Messages;
using Rebus.Handlers;

namespace Subscriber1;

internal class Handler : IHandleMessages<Message>
{
    public Handler()
    {
    }

    public Task Handle(Message message)
    {
        Console.WriteLine($"Handle message {message.Uid}");
        return Task.CompletedTask;
    }
}
