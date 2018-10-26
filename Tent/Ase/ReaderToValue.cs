﻿using System.Data;

namespace Ase
{
    public class ReaderToValue<T> : IReaderConverter<T>
    {
        public T Convert(IDataReader reader) {
            var value = default(T);
            while (reader.Read()) {
                value = (T)reader[0];
                break;
            }
            return value;
        }
    }
}
