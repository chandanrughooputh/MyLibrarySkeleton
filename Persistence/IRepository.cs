
using System.Collections.Generic;

namespace Persistence
{
    public interface IRepository
    {
        IList<T> RunQuery<T>(string query);
    }
}
