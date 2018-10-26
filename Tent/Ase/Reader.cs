﻿using System.Collections.Generic;
using System.Data;

namespace Ase
{
    public class Reader : IRead
    {
        public T ReadOne<T>(IDataReader reader) =>
            new ReaderToItem<T>().Convert(reader);
        
        public List<T> ReadList<T>(IDataReader reader) =>
            new ReaderToList<T>().Convert(reader);
    }
}
