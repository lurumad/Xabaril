using System.Collections.Generic;

namespace Xabaril.Core
{
    public interface IDiscoverableActivatorParameters
    {
        IEnumerable<ActivatorParameterDescriptor> Descriptors { get; }
    }
}
