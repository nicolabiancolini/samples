
using System;
using System.Collections.Generic;
using System.Text;

namespace Sample
{
    public interface IEntity<TKey>
    {
        TKey Id { get; }
    }
}
