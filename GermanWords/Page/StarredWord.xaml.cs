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

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

using GermanWordsClassLibrary.Class;
using GermanWordsClassLibrary.Database;
using GermanWordsClassLibrary.Helper;
using GermanWordsClassLibrary.UserControl.WordCard;
using System.Diagnostics;
using System.Threading;

namespace GermanWords.Page
{
    public partial class StarredWord : PhoneApplicationPage
    {
        private ObservableCollection<ListBoxData> starredWordList;

        public StarredWord()
        {
            InitializeComponent();

            starredWordList = new ObservableCollection<ListBoxData>();
            ReadDataFromDB();
            // read setting from IsolatedStorageSettings
            int cardType = 1;
            if (IsolatedStorageSettingsHelper.IsSettingExisted(
                IsolatedStorageSettingsHelper.SettingStrings.STARRED_WORD_CARD_TYPE))
            {
                object value = IsolatedStorageSettingsHelper.ReadSetting(
                    IsolatedStorageSettingsHelper.SettingStrings.STARRED_WORD_CARD_TYPE);
                if (value is int)
                {
                    cardType = (int)value;
                    if (cardType < 1 || cardType > 3)
                        cardType = 1;
                }
            }
            ListPicker_CardType.SelectedIndex = cardType - 1;

            int orderIndex = 0;
            if (IsolatedStorageSettingsHelper.IsSettingExisted
                (IsolatedStorageSettingsHelper.SettingStrings.STARRED_WORD_ORDER_BY_INDEX))
            {
                object value = IsolatedStorageSettingsHelper.ReadSetting(
                    IsolatedStorageSettingsHelper.SettingStrings.STARRED_WORD_ORDER_BY_INDEX);
                if (value is int
                    && ((int)value == 0 || (int)value == 1))
                    orderIndex = (int)value;
            }
            ListPicker_OrderBy.SelectedIndex = orderIndex;

            int ascOrDescIndex = 1;
            if (IsolatedStorageSettingsHelper.IsSettingExisted
                (IsolatedStorageSettingsHelper.SettingStrings.STARRED_WORD_ASC_OR_DESC_INDEX))
            {
                object value = IsolatedStorageSettingsHelper.ReadSetting(
                    IsolatedStorageSettingsHelper.SettingStrings.STARRED_WORD_ASC_OR_DESC_INDEX);
                if (value is int
                    && ((int)value == 0 || (int)value == 1))
                    ascOrDescIndex = (int)value;
            }
            ListPicker_AscOrDesc.SelectedIndex = ascOrDescIndex;

            UpdateUI();
            SortStarredWordList();
            ListPicker_CardType.SelectionChanged += new SelectionChangedEventHandler(ListPicker_CardType_SelectionChanged);
            ListPicker_OrderBy.SelectionChanged += new SelectionChangedEventHandler(ListPicker_OrderBy_SelectionChanged);
            ListPicker_AscOrDesc.SelectionChanged += new SelectionChangedEventHandler(ListPicker_AscOrDesc_SelectionChanged);
        }

        private void MenuItem_Delete_Click(object sender, RoutedEventArgs e)
        {
            ListBox_StarredWord.UpdateLayout();
            ListBoxData selectedData = (sender as MenuItem).DataContext as ListBoxData;
            StarredWordOperations.DeleteEntry(selectedData.SourceId, selectedData.WordId);
            Dispatcher.BeginInvoke(() =>
            {
                try
                {
                    //starredWordList = new ObservableCollection<ListBoxData>(starredWordList.Except(new[] {(selectedData)}));
                    //ListBox_StarredWord.ItemsSource = starredWordList;
                    starredWordList.Remove(selectedData);
                    SortStarredWordList();
                    //Thread.Sleep(500);
                    ListBox_StarredWord.UpdateLayout();
                }
                catch
                {
 
                }
            });
        }

        private void ListPicker_CardType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int cardType = ListPicker_CardType.SelectedIndex + 1;

            IsolatedStorageSettingsHelper.WriteSetting(
                IsolatedStorageSettingsHelper.SettingStrings.STARRED_WORD_CARD_TYPE, cardType);

            for (int i = 0; i < starredWordList.Count; i++)
            {
                switch (cardType)
                {
                    case 1:
                        starredWordList[i].WordCard = UserControlHelper.GenerateOneSideWordCard(starredWordList[i].Word, 0.9, 0.9);
                        break;
                    case 2:
                        starredWordList[i].WordCard = UserControlHelper.GenerateTwoSideWordCard(starredWordList[i].Word, true, 0.9, 0.9);
                        break;
                    case 3:
                        starredWordList[i].WordCard = UserControlHelper.GenerateTwoSideWordCard(starredWordList[i].Word, false, 0.9, 0.9);
                        break;
                }
            }
        }

        private void ListPicker_OrderBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IsolatedStorageSettingsHelper.WriteSetting(
                IsolatedStorageSettingsHelper.SettingStrings.STARRED_WORD_ORDER_BY_INDEX, ListPicker_OrderBy.SelectedIndex);

            SortStarredWordList();
        }

        private void ListPicker_AscOrDesc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListPicker_AscOrDesc == null) return;

            IsolatedStorageSettingsHelper.WriteSetting(
                IsolatedStorageSettingsHelper.SettingStrings.STARRED_WORD_ASC_OR_DESC_INDEX, ListPicker_AscOrDesc.SelectedIndex);

            SortStarredWordList();
        }

        private void IgnoreSelectionChange(object sender, SelectionChangedEventArgs e)
        {
            //(sender as LongListSelector).SelectedItem = null;
        }

        private void ReadDataFromDB()
        {
            List<StarredWordItem> starredWordItemList = StarredWordOperations.GetListDescByAddTime();
            for (int i = 0; i < starredWordItemList.Count; i++)
            {
                List<WordSource> wordSourceList = (Application.Current as App).wordSourceList;
                WordSource wordSource = WordSourceHelper.GetWordSourceById(wordSourceList, starredWordItemList[i].WordSourceId);
                if (wordSource != null)
                {
                    Word word = WordFileHelper.GetWordById(wordSourceList, wordSource.sourceId, starredWordItemList[i].WordIdOfWordSource);
                    if (word != null)
                    {
                        ListBoxData data = new ListBoxData
                        {
                            SourceId = wordSource.sourceId,
                            SourceTitle = wordSource.title,
                            WordId = word.wordId,
                            Word = word,
                            WordCard = null,
                            AddTime = starredWordItemList[i].AddTime,
                            AddTimeString = starredWordItemList[i].AddTime.ToString()
                        };
                        starredWordList.Add(data);
                    }
                }
            }
        }

        private void UpdateUI()
        {
            if (starredWordList.Count == 0)
            {
                TextBlock_NoDataPrompt.Visibility = Visibility.Visible;
            }
            else
            {
                TextBlock_NoDataPrompt.Visibility = Visibility.Collapsed;
            }
        }

        private void SortStarredWordList()
        {
            int orderByIndex = ListPicker_OrderBy.SelectedIndex;
            int ascOrDescIndex = ListPicker_AscOrDesc.SelectedIndex;
            int cardType = ListPicker_CardType.SelectedIndex + 1;

            List<ListBoxData> orderedList = null;
            if (orderByIndex == 0) // order by add time
            {
                if (ascOrDescIndex == 0)        // ascending
                {
                    orderedList = new List<ListBoxData>(starredWordList.OrderBy(data => data.AddTime));
                }
                else if (ascOrDescIndex == 1)   // descending
                {
                    orderedList = new List<ListBoxData>(starredWordList.OrderByDescending(data => data.AddTime));
                }
                else
                {
                    return;
                }
            }
            else if (orderByIndex == 1) // order by alphabet
            {
                if (ascOrDescIndex == 0)        // ascending
                {
                    orderedList = new List<ListBoxData>(starredWordList.OrderBy(data => data.Word.word));
                }
                else if (ascOrDescIndex == 1)   // descending
                {
                    orderedList = new List<ListBoxData>(starredWordList.OrderByDescending(data => data.Word.word));
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }


            starredWordList = new ObservableCollection<ListBoxData>();
            for (int i = 0; i < orderedList.Count; i++)
            {
                ListBoxData data = new ListBoxData
                {
                    SourceId = orderedList[i].SourceId,
                    SourceTitle = orderedList[i].SourceTitle,
                    WordId = orderedList[i].WordId,
                    Word = orderedList[i].Word,
                    WordCard = null,
                    AddTime = orderedList[i].AddTime,
                    AddTimeString = orderedList[i].AddTimeString
                };
                switch (cardType)
                {
                    case 1:
                        data.WordCard = UserControlHelper.GenerateOneSideWordCard(data.Word, 0.9, 0.9);
                        break;
                    case 2:
                        data.WordCard = UserControlHelper.GenerateTwoSideWordCard(data.Word, true, 0.9, 0.9);
                        break;
                    case 3:
                        data.WordCard = UserControlHelper.GenerateTwoSideWordCard(data.Word, false, 0.9, 0.9);
                        break;
                }
                starredWordList.Add(data);
            }
            ListBox_StarredWord.ItemsSource = starredWordList;
            
        }

        public class ListBoxData : INotifyPropertyChanged
        {
            public int SourceId { set; get; }
            public string SourceTitle { set; get; }
            public int WordId { set; get; }
            public Word Word { set; get; }
            private UserControl _wordCard;
            public UserControl WordCard
            {
                get
                {
                     return _wordCard; 
                }
                set
                {
                    _wordCard = value;
                    NotifyPropertyChanged("WordCard");
                }
            }
            public DateTime AddTime { set; get; }
            public string AddTimeString { set; get; }

            #region INotifyPropertyChanged Members

            public event PropertyChangedEventHandler PropertyChanged;

            // Used to notify the app that a property has changed.
            private void NotifyPropertyChanged(string propertyName)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }
            #endregion
        }

        private void ContextMenu_Unloaded(object sender, RoutedEventArgs e)
        {
            //if (starredWordList.Count < 7)
            //{
            //    ContextMenu conmen = (sender as ContextMenu);
            //    conmen.ClearValue(FrameworkElement.DataContextProperty);
            //}

        }
    }
}