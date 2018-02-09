using System.Data;

namespace Tent.Data
{
    public interface IConnectionFactory
    {
        IDbConnection Create();
    }
}
