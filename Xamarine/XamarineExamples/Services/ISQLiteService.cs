using SQLite;      
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectInsightMobile.Services
{
    public interface ISQLiteService
    {
        SQLiteConnection GetConnection();

    }
}
