using System.Collections.Generic;

namespace signalr_best_practice_core.Interfaces.Managers
{
    public interface IDataMapper
    {
        To Parse<From, To>(From model);
        IEnumerable<To> ParseCollection<From, To>(IEnumerable<From> models);
    }
}
