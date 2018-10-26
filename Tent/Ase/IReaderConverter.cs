using System.Data;

namespace Ase
{
    public interface IReaderConverter<T>
    {
        T Convert(IDataReader reader);
    }
}
