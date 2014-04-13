using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.Linq;

using GermanWordsClassLibrary.Helper;

namespace GermanWordsClassLibrary.Database
{
    public class StudyHistoryOperations
    {
        public static void CreateDatabase()
        {
            using (StudyHistoryDataContext db = new StudyHistoryDataContext(StudyHistoryDataContext.DBConnectionString))
            {
                if (db.DatabaseExists() == false)
                    db.CreateDatabase();
            }
        }

        public static void DeleteDatabase()
        {
            using (StudyHistoryDataContext db = new StudyHistoryDataContext(StudyHistoryDataContext.DBConnectionString))
            {
                db.DeleteDatabase();
            }
        }

        public static void InsertEntry(DateTime date, double durationMinutes, string contentTitle)
        {
            StudyHistoryDataContext studyHistoryDB = new StudyHistoryDataContext(StudyHistoryDataContext.DBConnectionString);

            StudyHistoryItem itemToFind = null;
            var itemsInDB = from StudyHistoryItem studyHistory in studyHistoryDB.StudyHistoryItems
                            select studyHistory;
            IEnumerator<StudyHistoryItem> enumerator = studyHistoryDB.StudyHistoryItems.GetEnumerator();
            while (enumerator.MoveNext())
            {
                StudyHistoryItem item = enumerator.Current;
                if (item.ContentTitle.Equals(contentTitle)
                    && date.CompareTo(item.Date) == 0)
                {
                    itemToFind = item;
                    break;
                }
            }

            if (itemToFind == null)
            {
                itemToFind = new StudyHistoryItem
                {
                    Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                    DurationMinute = durationMinutes,
                    ContentTitle = contentTitle
                };
                studyHistoryDB.StudyHistoryItems.InsertOnSubmit(itemToFind);
            }
            else
            {
                itemToFind.DurationMinute += durationMinutes;
            }
            studyHistoryDB.SubmitChanges();
        }

        public static List<StudyHistoryItem> GetTodayList()
        {
            List<StudyHistoryItem> todayList;

            using (StudyHistoryDataContext db = new StudyHistoryDataContext(StudyHistoryDataContext.DBConnectionString))
            {
                var itemsInDB = from StudyHistoryItem studyHistory in db.StudyHistoryItems
                                where studyHistory.Date.Date.CompareTo(DateTime.Now.Date) == 0
                                orderby studyHistory.DurationMinute descending
                                select studyHistory;

                todayList = new List<StudyHistoryItem>(itemsInDB);

                for (int i = 0; i < todayList.Count; i++)
                {
                    double value = todayList[i].DurationMinute;
                    value = Math.Ceiling(value * 10) / 10;
                    todayList[i].DurationMinute = value;
                }
            }
            return todayList;
        }
        
        public static List<StudyHistoryItem> GetDayList(int maxEntries)
        {
            List<StudyHistoryItem> dayList = new List<StudyHistoryItem>();

            using (StudyHistoryDataContext db = new StudyHistoryDataContext(StudyHistoryDataContext.DBConnectionString))
            {
                var itemsInDB = from StudyHistoryItem studyHistory in db.StudyHistoryItems
                                where studyHistory.Date.Date.CompareTo(DateTime.Now.Date) < 0
                                orderby studyHistory.Date descending
                                select studyHistory;

                IEnumerator<StudyHistoryItem> enumerator = itemsInDB.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    StudyHistoryItem item = enumerator.Current;
                    
                    double value = item.DurationMinute;
                    value = Math.Ceiling(value * 10) / 10;
                    if (dayList.Count > 0
                        && dayList.Last().Date.Date.CompareTo(item.Date.Date) == 0)
                    {
                        dayList.Last().DurationMinute += value;
                    }
                    else
                    {
                        if (dayList.Count >= maxEntries)
                            break;
                        else
                        {
                            dayList.Add(item);
                            dayList.Last().DurationMinute = value;
                        }
                    }
                }
            }
            return dayList;
        }

        public static List<StudyHistoryItem> GetThisMonthList()
        {
            List<StudyHistoryItem> thisMonthList = new List<StudyHistoryItem>();

            using (StudyHistoryDataContext db = new StudyHistoryDataContext(StudyHistoryDataContext.DBConnectionString))
            {
                DateTime today = DateTime.Now.Date;

                var itemsInDB = from StudyHistoryItem studyHistory in db.StudyHistoryItems
                                where studyHistory.Date.Year == today.Year && studyHistory.Date.Month == today.Month
                                select studyHistory;

                IEnumerator<StudyHistoryItem> enumerator = itemsInDB.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    StudyHistoryItem item = enumerator.Current;

                    double value = item.DurationMinute;
                    value = Math.Ceiling(value * 10) / 10;

                    bool isElementFound = false;
                    for (int i = 0; i < thisMonthList.Count; i++)
                    {
                        if (thisMonthList[i].ContentTitle.Equals(item.ContentTitle))
                        {
                            isElementFound = true;
                            thisMonthList[i].DurationMinute += value;
                            break;
                        }
                    }
                    if (isElementFound == false)
                    {
                        thisMonthList.Add(item);
                        thisMonthList.Last().DurationMinute = value;
                    }
                }
            }
            return thisMonthList;
        }

        public static List<StudyHistoryItem> GetMonthList(int maxEntries)
        {
            List<StudyHistoryItem> monthList = new List<StudyHistoryItem>();

            using (StudyHistoryDataContext db = new StudyHistoryDataContext(StudyHistoryDataContext.DBConnectionString))
            {
                DateTime firstDayOfThisMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

                var itemsInDB = from StudyHistoryItem studyHistory in db.StudyHistoryItems
                                where studyHistory.Date.Date.CompareTo(firstDayOfThisMonth.Date) < 0
                                orderby studyHistory.Date descending
                                select studyHistory;

                IEnumerator<StudyHistoryItem> enumerator = itemsInDB.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    StudyHistoryItem item = enumerator.Current;

                    double value = item.DurationMinute;
                    value = Math.Ceiling(value * 10) / 10;
                    if (monthList.Count > 0
                        && monthList.Last().Date.Year == item.Date.Year && monthList.Last().Date.Month == item.Date.Month)
                    {
                        monthList.Last().DurationMinute += value;
                    }
                    else
                    {
                        if (monthList.Count >= maxEntries)
                            break;
                        else
                        {
                            monthList.Add(item);
                            monthList.Last().DurationMinute = value;
                        }
                    }
                }
            }
            return monthList;
        }

        public static List<StudyHistoryItem> GetHistroyListByContentTitle()
        {
            List<StudyHistoryItem> historyList = new List<StudyHistoryItem>();

            using (StudyHistoryDataContext db = new StudyHistoryDataContext(StudyHistoryDataContext.DBConnectionString))
            {
                var itemsInDB = from StudyHistoryItem studyHistory in db.StudyHistoryItems
                                where studyHistory.Date.Date.CompareTo(DateTime.Now.Date) <= 0
                                select studyHistory;

                IEnumerator<StudyHistoryItem> enumerator = itemsInDB.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    StudyHistoryItem item = enumerator.Current;

                    double value = item.DurationMinute;
                    value = Math.Ceiling(value * 10) / 10;

                    bool isElementFound = false;
                    for (int i = 0; i < historyList.Count; i++)
                    {
                        if (historyList[i].ContentTitle.Equals(item.ContentTitle))
                        {
                            isElementFound = true;
                            historyList[i].DurationMinute += value;
                            break;
                        }
                    }
                    if (isElementFound == false)
                    {
                        historyList.Add(item);
                        historyList.Last().DurationMinute = value;
                    }
                }
            }
            return historyList;
        }

        public static StudyHistoryItem GetEarliestItem()
        {
            using (StudyHistoryDataContext db = new StudyHistoryDataContext(StudyHistoryDataContext.DBConnectionString))
            {
                var itemsInDB = from StudyHistoryItem studyHistory in db.StudyHistoryItems
                                orderby studyHistory.Date ascending
                                select studyHistory;
                return itemsInDB.FirstOrDefault();
            }
        }

        public static void DeleteAllEntries()
        {
            StudyHistoryDataContext studyHistoryDB = new StudyHistoryDataContext(StudyHistoryDataContext.DBConnectionString);
            
            studyHistoryDB.StudyHistoryItems.DeleteAllOnSubmit(studyHistoryDB.StudyHistoryItems);
            studyHistoryDB.SubmitChanges();
        }
    }
}
