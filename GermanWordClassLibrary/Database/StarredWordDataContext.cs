using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using System.Data.Linq;

namespace GermanWordsClassLibrary.Database
{
    public class StarredWordDataContext : DataContext
    {
        public static string DBConnectionString = "Data Source=isostore:/StarredWordList.sdf";

        public StarredWordDataContext(string connectionString)
            : base(connectionString)
        { }

        public Table<StarredWordItem> StarredWordItems;
    }
}
