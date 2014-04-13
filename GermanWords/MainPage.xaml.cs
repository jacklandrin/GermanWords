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

using Microsoft.Phone.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Windows.Navigation;

using GermanWordsClassLibrary.Class;
using GermanWordsClassLibrary.Database;
using GermanWordsClassLibrary.Helper;
using GermanWordsClassLibrary.UserControl.WordCard;
using GermanWordsClassLibrary;
using Microsoft.Phone.Shell;

namespace GermanWords
{
    public partial class MainPage : PhoneApplicationPage
    {
        private bool isNewPageInstance = false;
        private BackgroundWorker searchWordWorker;
        private string nowword = "";

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            isNewPageInstance = true;
            try
            {
                Speech.Initialize();
            }
            catch (Exception ex)
            {
                MessageBox.Show("未检测到德语语音包，请在设置-语音-语音语音中下载Deutsch语音包。", "温馨提示", MessageBoxButton.OK);
            }
            // initial BackgroundWorker
            searchWordWorker = new BackgroundWorker();
            searchWordWorker.WorkerReportsProgress = false;
            searchWordWorker.WorkerSupportsCancellation = true;
            searchWordWorker.DoWork += new DoWorkEventHandler(delegate(object sender, DoWorkEventArgs e)
                {
                    BackgroundWorker worker = sender as BackgroundWorker;
                    string keyword = e.Argument as string;
                    List<WordSource> wordSourceList = (Application.Current as App).wordSourceList;
                    for (int i = 0; i < wordSourceList.Count; i++)
                    {
                        if (worker.CancellationPending == true)
                        {
                            e.Cancel = true;
                            return;
                        }

                        List<Word> wordList = WordFileHelper.ReadWordFileToList(wordSourceList[i].fileUri);
                        for (int j = 0; j < wordList.Count; j++)
                        {
                            if (worker.CancellationPending == true)
                            {
                                e.Cancel = true;
                                return;
                            }

                            Word word = wordList[j];
                            if (word.word.ToLower().StartsWith(keyword.Trim().ToLower()))
                            {
                                Dispatcher.BeginInvoke(delegate()
                                {
                                    System.Windows.Controls.UserControl uc = UserControlHelper.GenerateOneSideWordCard(word, 0.7, 0.7, null);
                                    uc.Margin = new Thickness(0, -60, 0, -20);
                                    StackPanel_SearchResult.Children.Add(uc);
                                });
                            }
                        }
                    }
                });
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // calculate if "Word Today" should be updated
            if (IsolatedStorageSettingsHelper.IsSettingExisted(IsolatedStorageSettingsHelper.SettingStrings.WORD_TODAY_WORD_ID) == false
                || IsolatedStorageSettingsHelper.IsSettingExisted(IsolatedStorageSettingsHelper.SettingStrings.WORD_TODAY_SOURCE_ID) == false
                || IsolatedStorageSettingsHelper.IsSettingExisted(IsolatedStorageSettingsHelper.SettingStrings.WORD_TODAY_LAST_UPDATE_DATETIME) == false)
            {
                UpdateWordToday();
            }
            else
            {
                DateTime lastUpdateDate = new DateTime(0);
                object value = IsolatedStorageSettingsHelper.ReadSetting(IsolatedStorageSettingsHelper.SettingStrings.WORD_TODAY_LAST_UPDATE_DATETIME);
                if (value is DateTime)
                    lastUpdateDate = (DateTime)value;

                if (DateTime.Now.Date.CompareTo(lastUpdateDate.Date) > 0)
                {
                    UpdateWordToday();
                }
            }

            // read "Word Today", which has been generated/updated just now
            UpdateWordTodayUI();

            // detect last run state
            List<Word> wordList;
            WordSource wordSource;
            int cardType;
            int startIndex;
            List<WordSource> wordSourceList = (Application.Current as App).wordSourceList;
            if (IsolatedStorageFileHelper.ReadRunState(wordSourceList, out wordList, out wordSource, out cardType, out startIndex))
            {
                StackPanel_ContinueLearning.Visibility = Visibility.Visible;
                StackPanel_NewLearning.Visibility = Visibility.Collapsed;
                if (cardType == 1)
                    TextBlock_CardType.Text = "1号卡片";
                else if (cardType == 2)
                    TextBlock_CardType.Text = "2号卡片";
                else if (cardType == 3)
                    TextBlock_CardType.Text = "3号卡片";
                TextBlock_WordListTitle.Text = wordSource.title;
                double percent = ((startIndex + 1) * 1000 / (wordList.Count)) / 10.0;
                TextBlock_Percent.Text = percent.ToString() + "%";
            }
            else
            {
                StackPanel_ContinueLearning.Visibility = Visibility.Collapsed;
                StackPanel_NewLearning.Visibility = Visibility.Visible;
            }

            // restore from saved page states
            if (isNewPageInstance)
            {
                RestorePageStates();
                isNewPageInstance = false;
            }

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (e.Content is Page.WordCardLearning)
            {
                List<Word> wordList;
                WordSource wordSource;
                int cardType;
                int startIndex;
                List<WordSource> wordSourceList = (Application.Current as App).wordSourceList;
                if (IsolatedStorageFileHelper.ReadRunState(wordSourceList, out wordList, out wordSource, out cardType, out startIndex))
                {
                    Page.WordCardLearning targetPage = e.Content as Page.WordCardLearning;
                    targetPage.SetParameters(wordList, wordSource, cardType, startIndex);
                }
            }

            // save page states
            if (e.NavigationMode != NavigationMode.Back)
                SavePageStates();
        }

        private class PageStateStrings
        {
            public static string PANORAMA_ITEM_INDEX = "PANORAMA_ITEM_INDEX";
            public static string SEARCH_WORD = "SEARCH_WORD";
        }

        private void SavePageStates()
        {
            State[PageStateStrings.PANORAMA_ITEM_INDEX] = Panorama_Main.SelectedIndex.ToString();
            State[PageStateStrings.SEARCH_WORD] = TextBox_SearchWord.Text;
        }

        private void RestorePageStates()
        {
            // direct to panorama item
            if (State.ContainsKey(PageStateStrings.PANORAMA_ITEM_INDEX))
            {
                int panoramaIndex = int.Parse(State[PageStateStrings.PANORAMA_ITEM_INDEX] as string);
                Panorama_Main.DefaultItem = Panorama_Main.Items[panoramaIndex];
            }

            if (State.ContainsKey(PageStateStrings.SEARCH_WORD))
            {
                TextBox_SearchWord.Text = State[PageStateStrings.SEARCH_WORD] as string;
            }
        }

        private void Button_UpdateWordToday_Click(object sender, RoutedEventArgs e)
        {
            UpdateWordToday();
            UpdateWordTodayUI();
        }

        void Button_Continue_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Page/WordCardLearning.xaml", UriKind.Relative));
        }

        private void Button_NewLearning_Click(object sender, RoutedEventArgs e)
        {
            IsolatedStorageFileHelper.DeleteRunState();
            StackPanel_ContinueLearning.Visibility = Visibility.Collapsed;
            StackPanel_NewLearning.Visibility = Visibility.Visible;
        }

        private void Button_CardTypeOne_Click(object sender, RoutedEventArgs e)
        {
            App.dictationOrLearn = true;
            NavigationService.Navigate(new Uri("/Page/LearningSetting.xaml?cardtype=1", UriKind.Relative));
        }

        private void Button_CardTypeTwo_Click(object sender, RoutedEventArgs e)
        {
            App.dictationOrLearn = true;
            NavigationService.Navigate(new Uri("/Page/LearningSetting.xaml?cardtype=2", UriKind.Relative));
        }

        private void Button_CardTypeThree_Click(object sender, RoutedEventArgs e)
        {
            App.dictationOrLearn = true;
            NavigationService.Navigate(new Uri("/Page/LearningSetting.xaml?cardtype=3", UriKind.Relative));
        }

        private void ListBox_Tools_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (sender as ListBox).SelectedIndex = -1;
        }

        private void ListBoxItem_DictationWord_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.dictationOrLearn = false;
            NavigationService.Navigate(new Uri("/Page/LearningSetting.xaml", UriKind.Relative));
        }

        private void ListBoxItem_StarredWord_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Page/StarredWord.xaml", UriKind.Relative));
        }

        private void ListBoxItem_StudyHistory_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Page/StudyHistory.xaml", UriKind.Relative));
        }

        private void ListBoxItem_ListenMe_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Page/ListenPage.xaml", UriKind.Relative));
        }
        private void ListBoxItem_DW_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            App.PageCount = 1;
            NavigationService.Navigate(new Uri("/DeutschWelle/DWTileListView.xaml", UriKind.Relative));
        }

        private void ListBoxItem_DefiniteArticle_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Page/GermanRelated/DefiniteArticleTable.xaml", UriKind.Relative));
        }

        private void ListBoxItem_PersonalPronoun_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Page/GermanRelated/PersonalPronounTable.xaml", UriKind.Relative));
        }

        private void ListBoxItem_Adjektive_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Page/GermanRelated/AdjektiveTable.xaml", UriKind.Relative));
        }

        private void TextBox_SearchWord_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBox_SearchWord.Text.Trim().Equals(""))
            {
                StackPanel_SearchResult.Children.Clear();
                return;
            }

            while (searchWordWorker.IsBusy)
                searchWordWorker.CancelAsync();

            StackPanel_SearchResult.Children.Clear();
            string keyword = TextBox_SearchWord.Text;
            searchWordWorker.RunWorkerAsync(keyword as object);
        }

        private void TextBox_SearchWord_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                this.Focus();
        }

        private void ListBox_Settings_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (sender as ListBox).SelectedIndex = -1;
        }


        private void ListBoxItem_WordTodaySetting_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Page/WordTodaySetting.xaml", UriKind.Relative));
        }

        private void ListBoxItem_LiveTileSetting_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Page/LiveTileSetting.xaml", UriKind.Relative));
        }

        private void ListBoxItem_SpeechSetting_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Page/SpeechSetting.xaml", UriKind.Relative));
        }

        private void ListBoxItem_Share_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Page/SNSSetting.xaml", UriKind.Relative));
        }

        private void ListBoxItem_Rate_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();
            marketplaceReviewTask.Show();
        }

        private void ListBoxItem_About_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Page/About.xaml", UriKind.Relative));
        }

        private void UpdateWordToday()
        {
            List<WordSource> wordSourceList = (Application.Current as App).wordSourceList;
            
            int selectMode = 0;
            /*  0 - random
             *  1 - from starred words
             *  2 - selected list
             *  3 - smart mode
             */
            if (IsolatedStorageSettingsHelper.IsSettingExisted(IsolatedStorageSettingsHelper.SettingStrings.WORD_TODAY_SELECT_MODE_INDEX))
            {
                object value = IsolatedStorageSettingsHelper.GetWordTodaySelectModeIndex();
                if (value is int)
                    selectMode = (int)value;
            }
            if (selectMode == 0 || selectMode == 3)
            {
                // select a wordsource
                Random random = new Random();
                WordSource selectedWordSource = wordSourceList[random.Next(wordSourceList.Count)];
                IsolatedStorageSettingsHelper.WriteSetting(
                    IsolatedStorageSettingsHelper.SettingStrings.WORD_TODAY_SOURCE_ID, selectedWordSource.sourceId);

                // select a word
                List<Word> wordList = WordFileHelper.ReadWordFileToList(selectedWordSource.fileUri);
                Word selectedWord = wordList[random.Next(wordList.Count)];
                IsolatedStorageSettingsHelper.WriteSetting(
                    IsolatedStorageSettingsHelper.SettingStrings.WORD_TODAY_WORD_ID, selectedWord.wordId);
            }
            else if (selectMode == 1)
            {
                List<StarredWordItem> starredWordItemList = StarredWordOperations.GetListDescByAddTime();
                List<StarredWordItem> validItemList = new List<StarredWordItem>();
                for (int i = 0; i < starredWordItemList.Count; i++)
                {
                    StarredWordItem item = starredWordItemList[i];
                    Word word = WordFileHelper.GetWordById(wordSourceList, item.WordSourceId, item.WordIdOfWordSource);
                    if (word != null)
                        validItemList.Add(item);
                }

                if (validItemList.Count > 0)
                {
                    Random random = new Random();
                    StarredWordItem selectedItem = validItemList[random.Next(validItemList.Count)];
                    IsolatedStorageSettingsHelper.WriteSetting(
                        IsolatedStorageSettingsHelper.SettingStrings.WORD_TODAY_SOURCE_ID, selectedItem.WordSourceId);
                    IsolatedStorageSettingsHelper.WriteSetting(
                        IsolatedStorageSettingsHelper.SettingStrings.WORD_TODAY_WORD_ID, selectedItem.WordIdOfWordSource);
                }
                else
                {
                    // select a wordsource
                    Random random = new Random();
                    WordSource selectedWordSource = wordSourceList[random.Next(wordSourceList.Count)];
                    IsolatedStorageSettingsHelper.WriteSetting(
                        IsolatedStorageSettingsHelper.SettingStrings.WORD_TODAY_SOURCE_ID, selectedWordSource.sourceId);

                    // select a word
                    List<Word> wordList = WordFileHelper.ReadWordFileToList(selectedWordSource.fileUri);
                    Word selectedWord = wordList[random.Next(wordList.Count)];
                    IsolatedStorageSettingsHelper.WriteSetting(
                        IsolatedStorageSettingsHelper.SettingStrings.WORD_TODAY_WORD_ID, selectedWord.wordId);
                }
            }
            else if (selectMode == 2)
            {
                WordSource selectedWordSource = null;
                if (IsolatedStorageSettingsHelper.IsSettingExisted(IsolatedStorageSettingsHelper.SettingStrings.WORD_TODAY_SELECTED_SOURCE_LIST))
                {
                    List<WordSource> selectedSourceList = null;
                    object value = IsolatedStorageSettingsHelper.ReadSetting(IsolatedStorageSettingsHelper.SettingStrings.WORD_TODAY_SELECTED_SOURCE_LIST);
                    if (value is List<WordSource>)
                        selectedSourceList = value as List<WordSource>;
                    if (selectedSourceList != null && selectedSourceList.Count >= 1)
                        selectedWordSource = selectedSourceList[0];
                }
                if (selectedWordSource == null)
                {
                    selectedWordSource = wordSourceList[0];
                }
                IsolatedStorageSettingsHelper.WriteSetting(
                    IsolatedStorageSettingsHelper.SettingStrings.WORD_TODAY_SOURCE_ID, selectedWordSource.sourceId);

                // select a word
                Random random = new Random();
                List<Word> wordList = WordFileHelper.ReadWordFileToList(selectedWordSource.fileUri);
                Word selectedWord = wordList[random.Next(wordList.Count)];
                IsolatedStorageSettingsHelper.WriteSetting(
                    IsolatedStorageSettingsHelper.SettingStrings.WORD_TODAY_WORD_ID, selectedWord.wordId);
                nowword = selectedWord.word;
            }

            // update date/time
            IsolatedStorageSettingsHelper.WriteSetting(
                IsolatedStorageSettingsHelper.SettingStrings.WORD_TODAY_LAST_UPDATE_DATETIME, DateTime.Now);
        }

        private void UpdateWordTodayUI()
        {
            int sourceId = IsolatedStorageSettingsHelper.GetWordTodaySourceId();
            int wordId = IsolatedStorageSettingsHelper.GetWordTodayWordId();

            List<WordSource> wordSourceList = (Application.Current as App).wordSourceList;
            Word word = WordFileHelper.GetWordById(wordSourceList, sourceId, wordId);
            if (word == null)
            {
                UpdateWordToday();
                sourceId = IsolatedStorageSettingsHelper.GetWordTodaySourceId();
                wordId = IsolatedStorageSettingsHelper.GetWordTodayWordId();
                word = WordFileHelper.GetWordById(wordSourceList, sourceId, wordId);
            }
            
            WordSource wordSource = WordSourceHelper.GetWordSourceById(wordSourceList, sourceId);
            TextBlock_WordSource.Text = wordSource.title;

            // generate a user control
            int cardType = 1;
            if (IsolatedStorageSettingsHelper.IsSettingExisted(IsolatedStorageSettingsHelper.SettingStrings.WORD_TODAY_CARD_TYPE))
            {
                cardType = IsolatedStorageSettingsHelper.GetWordTodayCardType();
            }

            System.Windows.Controls.UserControl uc = null;
            if (cardType == 1)
            {
                uc = UserControlHelper.GenerateOneSideWordCard(word, 0.9, 0.9, new SolidColorBrush());
            }
            else if (cardType == 2)
            {
                uc = UserControlHelper.GenerateTwoSideWordCard(word, true, 0.9, 0.9, (Brush)Application.Current.Resources["PhoneAccentBrush"]);
            }
            else if (cardType == 3)
            {
                uc = UserControlHelper.GenerateTwoSideWordCard(word, false, 0.9, 0.9, (Brush)Application.Current.Resources["PhoneAccentBrush"]);
            }

            StackPanel_WordToday.Children.Clear();
            StackPanel_WordToday.Children.Add(uc);
        }

        private async void voice_Click(object sender, EventArgs e)
        {
            try
            {
                string readword = "";
                if (StackPanel_WordToday.Children[0] is OneSideNounCard)
                {
                    OneSideNounCard onesidenouncard = StackPanel_WordToday.Children[0] as OneSideNounCard;
                    readword = onesidenouncard.nowword;
                }
                else if (StackPanel_WordToday.Children[0] is OneSideAbbrCard)
                {
                    OneSideAbbrCard onesideabbrcard = StackPanel_WordToday.Children[0] as OneSideAbbrCard;
                    readword = onesideabbrcard.nowword;
                }
                else if (StackPanel_WordToday.Children[0] is OneSideVerbCard)
                {
                    OneSideVerbCard onesideverbcard = StackPanel_WordToday.Children[0] as OneSideVerbCard;
                    readword = onesideverbcard.nowword;
                }
                else if (StackPanel_WordToday.Children[0] is OneSideWordCard)
                {
                    OneSideWordCard onesidewordcard = StackPanel_WordToday.Children[0] as OneSideWordCard;
                    readword = onesidewordcard.nowword;
                }
                else if (StackPanel_WordToday.Children[0] is TwoSideAbbrCard)
                {
                    TwoSideAbbrCard twosideabbrcard = StackPanel_WordToday.Children[0] as TwoSideAbbrCard;
                    readword = twosideabbrcard.nowword;
                }
                else if (StackPanel_WordToday.Children[0] is TwoSideNounCard)
                {
                    TwoSideNounCard twosidenouncard = StackPanel_WordToday.Children[0] as TwoSideNounCard;
                    readword = twosidenouncard.nowword;
                }
                else if (StackPanel_WordToday.Children[0] is TwoSideVerbCard)
                {
                    TwoSideVerbCard twosidevercard = StackPanel_WordToday.Children[0] as TwoSideVerbCard;
                    readword = twosidevercard.nowword;
                }
                else if (StackPanel_WordToday.Children[0] is TwoSideWordCard)
                {
                    TwoSideWordCard twosidewordcard = StackPanel_WordToday.Children[0] as TwoSideWordCard;
                    readword = twosidewordcard.nowword;
                }
                await Speech.synthesizer.SpeakTextAsync(readword);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TodayWordPrm_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsolatedStorageSettingsHelper.IsSpeechExist())
            {
                ApplicationBar = ((ApplicationBar)this.Resources["voiceAppBar"]);
                ApplicationBarIconButton iconbtn = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
                if (Panorama_Main.SelectedIndex == 0)
                {
                    ApplicationBar.IsVisible = true;
                }
                else
                {
                    ApplicationBar.IsVisible = false;
                }
            }

        }

        private void TodayWordPrm_Unloaded(object sender, RoutedEventArgs e)
        {
            if (IsolatedStorageSettingsHelper.IsSpeechExist())
                ApplicationBar.IsVisible = false;
        }

        private void Panorama_Main_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsolatedStorageSettingsHelper.IsSpeechExist())
            {
                Panorama pan = sender as Panorama;
                switch (pan.SelectedIndex)
                {
                    case 0:
                        ApplicationBar.IsVisible = true;
                        break;
                    case 1:
                        ApplicationBar.IsVisible = false;
                        break;
                    case 2:
                        ApplicationBar.IsVisible = false;
                        break;
                    case 3:
                        ApplicationBar.IsVisible = false;
                        break;
                    default:
                        ApplicationBar.IsVisible = false;
                        break;
                }
            }
            
        }


    }
}