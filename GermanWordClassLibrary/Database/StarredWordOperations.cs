using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.Linq;

using GermanWordsClassLibrary.Class;
using System.Diagnostics;

namespace GermanWordsClassLibrary.Database
{
    public class StarredWordOperations
    {
        public static void CreateDatabase()
        {
            using (StarredWordDataContext db = new StarredWordDataContext(StarredWordDataContext.DBConnectionString))
            {
                if (db.DatabaseExists() == false)
                    db.CreateDatabase();
            }
        }

        public static void DeleteDatabase()
        {
            using (StarredWordDataContext db = new StarredWordDataContext(StarredWordDataContext.DBConnectionString))
            {
                db.DeleteDatabase();
            }
        }

        public static bool IsWordStarred(int wordSourceId, int wordId)
        {
            using (StarredWordDataContext db = new StarredWordDataContext(StarredWordDataContext.DBConnectionString))
            {
                var itemsInDB = from StarredWordItem starredWordItem in db.StarredWordItems
                                where starredWordItem.WordSourceId == wordSourceId && starredWordItem.WordIdOfWordSource == wordId
                                select starredWordItem;

                if (itemsInDB.Count() != 0)
                    return true;
                else
                    return false;
            }
        }

        /* insert only when entry not existed */
        public static void InsertEntry(int wordSourceId, int wordId)
        {
            if (IsWordStarred(wordSourceId, wordId)) return;

            using (StarredWordDataContext db = new StarredWordDataContext(StarredWordDataContext.DBConnectionString))
            {
                StarredWordItem newItem = new StarredWordItem
                {
                    WordSourceId = wordSourceId,
                    WordIdOfWordSource = wordId,
                    AddTime = DateTime.Now
                };
                db.StarredWordItems.InsertOnSubmit(newItem);
                db.SubmitChanges();
            }
        }

        public static void DeleteEntry(int wordSourceId, int wordId)
        {
            try
            {
                using (StarredWordDataContext db = new StarredWordDataContext(StarredWordDataContext.DBConnectionString))
                {
                    var itemsInDB = from StarredWordItem starredWordItem in db.StarredWordItems
                                    where starredWordItem.WordSourceId == wordSourceId && starredWordItem.WordIdOfWordSource == wordId
                                    select starredWordItem;

                    db.StarredWordItems.DeleteAllOnSubmit(itemsInDB);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            
        }

        public static List<StarredWordItem> GetListDescByAddTime()
        {
            using (StarredWordDataContext db = new StarredWordDataContext(StarredWordDataContext.DBConnectionString))
            {
                var itemsInDB = from StarredWordItem starredWordItem in db.StarredWordItems
                                orderby starredWordItem.AddTime descending
                                select starredWordItem;

                return new List<StarredWordItem>(itemsInDB);
            }
        }
    }
}
