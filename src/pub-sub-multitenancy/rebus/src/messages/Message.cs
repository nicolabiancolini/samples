// See the LICENSE.TXT file in the project root for full license information.

using System;

namespace Messages
{
    public class Message
    {
        public Message()
        {
            this.Uid = Guid.NewGuid().ToString("N");
        }

        public string Uid { get; }
    }
}
