using System.Data;

namespace Ase
{
    public interface IConnectionFactory
    {
        IDbConnection Create();
    }
}
