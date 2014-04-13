using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

using System.ComponentModel;
using System.Windows.Navigation;
using System.Threading;

using GermanWordsClassLibrary.Database;

namespace GermanWords.Page
{
    public partial class StudyHistory : PhoneApplicationPage
    {
        private bool isNewPageInstance = false;

        public StudyHistory()
        {
            InitializeComponent();
            isNewPageInstance = true;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            #region initialize Pivot 1

            List<StudyHistoryItem> todayList = StudyHistoryOperations.GetTodayList();
            List<StudyHistoryItem> dayList = StudyHistoryOperations.GetDayList(20);

            double todayTotal = 0;
            for (int i = 0; i < todayList.Count; i++)
                todayTotal += todayList[i].DurationMinute;

            for (int i = 0; i < dayList.Count; i++)
            {
                DateTime today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                DateTime yesterday = today.AddDays(-1);
                DateTime dayBeforeYesterday = yesterday.AddDays(-1);

                if (dayList[i].Date.Date.CompareTo(yesterday) == 0)
                    dayList[i].ContentTitle = "昨日";
                else if (dayList[i].Date.Date.CompareTo(dayBeforeYesterday) == 0)
                    dayList[i].ContentTitle = "前日";
                else
                    dayList[i].ContentTitle = dayList[i].Date.Month.ToString() + "月" + dayList[i].Date.Day.ToString() + "日";
            }

            TextBlock_TodayTotalMinutes.Text = todayTotal.ToString();
            ListBox_TodayDetailed.ItemsSource = todayList;
            ListBox_DayMinutes.ItemsSource = dayList;

            #endregion

            #region initialize Pivot 2

            List<StudyHistoryItem> thisMonthList = StudyHistoryOperations.GetThisMonthList();
            List<StudyHistoryItem> monthList = StudyHistoryOperations.GetMonthList(10);

            double thisMonthTotal = 0;
            for (int i = 0; i < thisMonthList.Count; i++)
            {
                thisMonthTotal += thisMonthList[i].DurationMinute;
            }

            for (int i = 0; i < monthList.Count; i++)
            {
                DateTime lastMonth = DateTime.Now.AddMonths(-1);

                if (monthList[i].Date.Year == lastMonth.Year && monthList[i].Date.Month == lastMonth.Month)
                {
                    monthList[i].ContentTitle = "上个月";
                }
                else
                {
                    monthList[i].ContentTitle = monthList[i].Date.Year.ToString() + "月" + monthList[i].Date.Month.ToString() + "月";
                }
            }

            thisMonthList.Sort(CompareByDuration);

            TextBlock_ThisMonthTotalMinutes.Text = thisMonthTotal.ToString();
            ListBox_ThisMonthDetailed.ItemsSource = thisMonthList;
            ListBox_MonthMinutes.ItemsSource = monthList;

            #endregion

            #region initialize Pivot 3

            List<StudyHistoryItem> historyList = StudyHistoryOperations.GetHistroyListByContentTitle();
            historyList.Sort(CompareByDuration);

            double totalMinute = 0;
            for (int i = 0; i < historyList.Count; i++)
            {
                historyList[i].EntryId = (int)(historyList[i].DurationMinute * 100 / historyList[0].DurationMinute);
                totalMinute += historyList[i].DurationMinute;
            }

            TextBlock_TotalMinutes.Text = totalMinute.ToString();
            ListBox_TotalDetailed.ItemsSource = historyList;

            StudyHistoryItem earliestEntry = StudyHistoryOperations.GetEarliestItem();
            if (earliestEntry == null || earliestEntry.Date.Date.CompareTo(DateTime.Now.Date) > 0)
            {
                TextBlock_FromDate.Text = "自" + DateTime.Now.Year + "年" + DateTime.Now.Month + "月" + DateTime.Now.Day + "日";
            }
            else
            {
                TextBlock_FromDate.Text = "自" + earliestEntry.Date.Year + "年" + earliestEntry.Date.Month + "月" + earliestEntry.Date.Day + "日";
            }

            #endregion

            // restore from saved page states
            if (isNewPageInstance)
            {
                RestorePageStates();
                isNewPageInstance = false;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // save page states
            if (e.NavigationMode != NavigationMode.Back)
            {
                SavePageStates();
            }
        }

        private class PageStateStrings
        {
            public static string PIVOT_ITEM_INDEX = "PIVOT_ITEM_INDEX";
        }

        private void SavePageStates()
        {
            State[PageStateStrings.PIVOT_ITEM_INDEX] = Pivot_Main.SelectedIndex.ToString();
        }

        private void RestorePageStates()
        {
            // direct to pivot item
            if (State.ContainsKey(PageStateStrings.PIVOT_ITEM_INDEX))
            {
                Pivot_Main.SelectedIndex = int.Parse(State[PageStateStrings.PIVOT_ITEM_INDEX] as string);
            }
        }

        private void Button_ClearHistory_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mbresult = MessageBox.Show("确定要删除学习记录吗？一旦删除记录将无法恢复。", "你确定吗？", MessageBoxButton.OKCancel);
            if (mbresult == MessageBoxResult.OK)
            {
                StudyHistoryOperations.DeleteAllEntries();

                // update UI
                TextBlock_FromDate.Text = "自" + DateTime.Now.Year + "年" + DateTime.Now.Month + "月" + DateTime.Now.Day + "日";
                TextBlock_TodayTotalMinutes.Text = "0";
                TextBlock_ThisMonthTotalMinutes.Text = "0";
                TextBlock_TotalMinutes.Text = "0";
                ListBox_TodayDetailed.ItemsSource = null;
                ListBox_DayMinutes.ItemsSource = null;
                ListBox_ThisMonthDetailed.ItemsSource = null;
                ListBox_MonthMinutes.ItemsSource = null;
                ListBox_TotalDetailed.ItemsSource = null;
            }
        }

        private void IgnoreSelectionChange(object sender, SelectionChangedEventArgs e)
        {
            (sender as ListBox).SelectedIndex = -1;
        }

        private static int CompareByDuration(StudyHistoryItem item1, StudyHistoryItem item2)
        {
            return (int)((item2.DurationMinute - item1.DurationMinute) * 10);
        }

        private List<T> SearchElements<T>(DependencyObject parentElement) where T : DependencyObject
        {
            List<T> result = new List<T>();

            List<DependencyObject> elementList = new List<DependencyObject>();
            elementList.Add(parentElement);

            while (elementList.Count > 0)
            {
                DependencyObject element = elementList.ElementAt(0);
                elementList.RemoveAt(0);

                if (element != null && element is T)
                {
                    result.Add(element as T);
                }
                else
                {
                    int childrenCount = VisualTreeHelper.GetChildrenCount(element);
                    for (int i = 0; i < childrenCount; i++)
                    {
                        elementList.Add(VisualTreeHelper.GetChild(element, i));
                    }
                }
            }

            return result;
        }
    }
}