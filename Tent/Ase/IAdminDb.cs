﻿using System.Collections.Generic;

namespace Ase
{
    public interface IAdminDb
    {
        void Truncate(string table);
        void DropTable(string table);
        int ExecuteRaw(string sql); // won't add parameters
    }
}
