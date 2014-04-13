using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using GermanWordsClassLibrary.Class;
using System.Windows.Threading;
using GermanWordsClassLibrary.Helper;
using GermanWordsClassLibrary;
using GermanWordsClassLibrary.Database;

namespace GermanWords.Page
{
    public partial class DictationPage : PhoneApplicationPage
    {
        //private bool isNewPageInstance;
        private int currentWordIndex;
        private DispatcherTimer timer;
        private DispatcherTimer waittimer;

        private List<Word> wordList;
        private WordSource wordSource;
        private int wordCardType;
        private int startIndex;
        private int waittime;
        private Word currentword;
        private int currentCount;
        const int outtime = 60;

        public DictationPage()
        {
            InitializeComponent();
            //isNewPageInstance = true;
            currentWordIndex = 0;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            waittimer = new DispatcherTimer();
            waittimer.Interval = TimeSpan.FromSeconds(1);


            wordList = null;
            wordSource = null;
            wordCardType = 0;
            startIndex = 0;
            waittime = 0;
            currentword = new Word();
            currentCount = 0;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //if (e.NavigationMode == NavigationMode.New)
            //{
            //if (IsolatedStorageSettingsHelper.IsSpeechExist())
            //{
            //    ApplicationBar.IsVisible = true;
            //}
            if (!IsolatedStorageSettingsHelper.IsSpeechExist())
            {
                ApplicationBar.IsVisible = false;
            }
            currentWordIndex = startIndex;
            TextBlock_Title.Text = wordSource.title;
            TextBlock_WordCount.Text = wordList.Count.ToString();
            timeprogressbar.Maximum = outtime;
            timeprogressbar.Value = 0;
            currentCount = 0;
            UpdateUI();
            //}
            //else if(isNewPageInstance)
            //{
            //    RestorePageStates();
            //    isNewPageInstance = false;
            //}
            waittimer.Tick += waittimer_Tick;
            timer.Tick += timer_Tick;
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            timeprogressbar.Value += 1;
            if (timeprogressbar.Value >= outtime)
            {
                TextBlock_Result.Visibility = Visibility.Visible;
                TextBlock_Result.Text = "Überstunden!";
                if (!StarredWordOperations.IsWordStarred(wordSource.sourceId, wordList[currentWordIndex].wordId))
                    StarredWordOperations.InsertEntry(wordSource.sourceId, wordList[currentWordIndex].wordId);
                timer.Stop();
                waittimer.Start();
                confirm.IsEnabled = false;
            }
        }

        void waittimer_Tick(object sender, EventArgs e)
        {
            waittime++;
            if (waittime >= 2)
            {
                waittimer.Stop();
                waittime = 0;
                nextWord();
            }
        }

        void nextWord()
        {
            TextBlock_Result.Visibility = Visibility.Collapsed;
            TextBlock_correctword.Visibility = Visibility.Collapsed;
            timeprogressbar.Value = 0;
            TextBox_Input.Text = "";

            timer.Start();
            if (currentWordIndex + 1 >= wordList.Count)
            {
                MessageBox.Show("本次默写已完成。您的正确率为" + currentCount / wordList.Count + "%");
                for (int i = 0; i < NavigationService.BackStack.Count() - 1; i++)
                    NavigationService.RemoveBackEntry();
                NavigationService.GoBack();
            }
            else
            {
                currentWordIndex++;
                UpdateUI();
            }
        }

        public void SetParameters(List<Word> wordList, WordSource wordSource, int wordCardType, int startIndex)
        {
            this.wordList = wordList;
            this.wordSource = wordSource;
            this.wordCardType = wordCardType;
            this.startIndex = startIndex;
        }

        private class PageStateStrings
        {
            public static string WORD_LIST = "WORD_LIST";
            public static string WORD_SOURCE_ID = "WORD_SOURCE_ID";
            public static string WORD_CARD_TYPE = "WORD_CARD_TYPE";
            public static string CURRENT_WORD_INDEX = "CURRENT_WORD_INDEX";
        }

        private void SavePageStates()
        {
            State[PageStateStrings.WORD_LIST] = wordList;
            State[PageStateStrings.WORD_SOURCE_ID] = wordSource.sourceId.ToString();
            State[PageStateStrings.WORD_CARD_TYPE] = wordCardType.ToString();
            State[PageStateStrings.CURRENT_WORD_INDEX] = currentWordIndex.ToString();
        }

        private void RestorePageStates()
        {
            if (State.ContainsKey(PageStateStrings.WORD_LIST))
            {
                wordList = State[PageStateStrings.WORD_LIST] as List<Word>;
                TextBlock_WordCount.Text = wordList.Count.ToString();
            }
            if (State.ContainsKey(PageStateStrings.WORD_SOURCE_ID))
            {
                int sourceId = int.Parse(State[PageStateStrings.WORD_SOURCE_ID] as string);
                List<WordSource> wordSourceList = (Application.Current as App).wordSourceList;
                wordSource = WordSourceHelper.GetWordSourceById(wordSourceList, sourceId);
                TextBlock_Title.Text = wordSource.title;
            }
            if (State.ContainsKey(PageStateStrings.WORD_CARD_TYPE))
            {
                wordCardType = int.Parse(State[PageStateStrings.WORD_CARD_TYPE] as string);
            }
            if (State.ContainsKey(PageStateStrings.CURRENT_WORD_INDEX))
            {
                currentWordIndex = int.Parse(State[PageStateStrings.CURRENT_WORD_INDEX] as string);
                TextBlock_CurrentWordNumber.Text = (currentWordIndex + 1).ToString();
            }

            if (State.ContainsKey(PageStateStrings.WORD_LIST) && State.ContainsKey(PageStateStrings.WORD_CARD_TYPE) && State.ContainsKey(PageStateStrings.CURRENT_WORD_INDEX))
            {
                UpdateUI();
            }
        }

        private void UpdateUI()
        {
            TextBlock_CurrentWordNumber.Text = (currentWordIndex + 1).ToString();
            currentword = wordList[currentWordIndex];
            TextBlock_Chinese.Text = currentword.wordType.ToString() + "  " + currentword.translation;
            speech();
            confirm.IsEnabled = true;
        }

        private async void speech()
        {
            if (IsolatedStorageSettingsHelper.IsSpeechExist())
            {
                try
                {
                    await Speech.synthesizer.SpeakTextAsync(currentword.word);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void voice_Click(object sender, EventArgs e)
        {
            speech();
        }

        private void confirm_Click(object sender, RoutedEventArgs e)
        {
            if (confirm.Content.ToString() == "确    定")
            {
                if (TextBox_Input.Text == currentword.word)
                {
                    currentCount++;
                    TextBlock_Result.Text = "Richtig!";
                    waittime = 0;
                    waittimer.Start();
                    confirm.IsEnabled = false;
                    if (StarredWordOperations.IsWordStarred(wordSource.sourceId, wordList[currentWordIndex].wordId))
                    {
                        StarredWordOperations.DeleteEntry(wordSource.sourceId, wordList[currentWordIndex].wordId);
                    }
                }
                else
                {
                    TextBlock_Result.Text = "Falsch!";
                    TextBlock_correctword.Visibility = Visibility.Visible;
                    TextBlock_correctword.Text = currentword.word;
                    if (!StarredWordOperations.IsWordStarred(wordSource.sourceId, wordList[currentWordIndex].wordId))
                        StarredWordOperations.InsertEntry(wordSource.sourceId, wordList[currentWordIndex].wordId);
                    confirm.Content = "下 一 个";
                }
                timer.Stop();
                TextBlock_Result.Visibility = Visibility.Visible;
            }
            else if (confirm.Content.ToString() == "下 一 个")
            {
                nextWord();
                confirm.Content = "确    定";
            }
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            timer.Stop();
            e.Cancel = true;
            Quit(sender, null);
        }

        void Quit(object sender, EventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("正在默写中，你确定真的要退出吗？", "你确定吗？", MessageBoxButton.OKCancel);
            if (messageBoxResult == MessageBoxResult.OK)
            {
                NavigationService.GoBack();
            }
            else
            {
                timer.Start();
            }
        }
    }
}