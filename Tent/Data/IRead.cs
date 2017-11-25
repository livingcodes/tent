using System.Collections.Generic;
using System.Data;

namespace Tent.Data
{
    public interface IRead
    {
        T Read<T>(IDataReader reader);
        List<T> ReadList<T>(IDataReader reader);
    }

    public class Reader : IRead
    {
        public T Read<T>(IDataReader reader) {
            // todo: new ReaderToItem().Convert(reader); // handles class and struct and value
            return new ReaderToItem<T>().Convert(reader);
        }
        public List<T> ReadList<T>(IDataReader reader) {
            return new ReaderToList<T>().Convert(reader);
        }
    }
}
