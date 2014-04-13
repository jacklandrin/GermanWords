using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.Linq;

namespace GermanWordsClassLibrary.Database
{
    class StudyHistoryDataContext : DataContext
    {
        public static string DBConnectionString = "Data Source=isostore:/StudyHistory.sdf";

        public StudyHistoryDataContext(string connectionString)
            : base(connectionString)
        { }

        public Table<StudyHistoryItem> StudyHistoryItems;
    }
}