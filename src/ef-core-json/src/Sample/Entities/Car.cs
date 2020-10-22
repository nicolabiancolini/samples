// See the LICENSE.TXT file in the project root for full license information.

using System;
using Sample.Dtos;

namespace Sample.Entities
{
    public class Car : IEntity<Guid>
    {
        public Car(string name, Metadata metadata)
            : this()
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace", nameof(name));
            }

            this.Name = name;
            this.Metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));

            this.Id = Guid.NewGuid();
        }

        private Car()
        {
        }

        public string NameOfRetailer { get; }

        public Guid Id { get; }

        public string Name { get; }

        public Metadata Metadata { get; private set; }

        public void AddMetadata(Metadata metadata)
        {
            this.Metadata = metadata;
        }
    }
}
