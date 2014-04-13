using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

using GermanWordsClassLibrary.Class;
using GermanWordsClassLibrary.Database;
using GermanWordsClassLibrary.Helper;

namespace GermanWords.Page
{
    public partial class WordCardLearning : PhoneApplicationPage
    {
        private bool isNewPageInstance;
        private int currentWordIndex;
        private DateTime startTime;

        // parameters
        private List<Word> wordList;
        private WordSource wordSource;
        private int wordCardType;
        private int startIndex;

        public WordCardLearning()
        {
            InitializeComponent();
            BuildApplicationBar();

            isNewPageInstance = true;
            currentWordIndex = 0;
            startTime = new DateTime();

            // set parameters default value
            wordList = null;
            wordSource = null;
            wordCardType = 0;
            startIndex = 0;
        }

        public void SetParameters(List<Word> wordList, WordSource wordSource, int wordCardType, int startIndex)
        {
            this.wordList = wordList;
            this.wordSource = wordSource;
            this.wordCardType = wordCardType;
            this.startIndex = startIndex;
        }

        private void BuildApplicationBar()
        {
            ApplicationBar = new ApplicationBar();

            ApplicationBarIconButton prevButton = new ApplicationBarIconButton
            {
                IconUri = new Uri("/Resource/ApplicationBarIcon/LeftArrow.png", UriKind.Relative),
                Text = "上一个"
            };
            prevButton.Click += new EventHandler(MoveToPrevWord);

            ApplicationBarIconButton nextButton = new ApplicationBarIconButton
            {
                IconUri = new Uri("/Resource/ApplicationBarIcon/RightArrow.png", UriKind.Relative),
                Text = "下一个"
            };
            nextButton.Click += new EventHandler(MoveToNextWord);

            ApplicationBarMenuItem starOrUnstarWord = new ApplicationBarMenuItem
            {
                Text = "加入生词本"
            };
            starOrUnstarWord.Click += new EventHandler(ApplicationBarMenuItem_StarOrUnstar_Click);

            ApplicationBarMenuItem pinToStart = new ApplicationBarMenuItem
            {
                Text = "固定到开始屏幕"
            };
            pinToStart.Click += new EventHandler(PinOrUnpinToStart);

            ApplicationBarMenuItem share = new ApplicationBarMenuItem
            {
                Text = "分享到新浪微博",
                IsEnabled = false
            };

            ApplicationBarMenuItem saveAndQuit = new ApplicationBarMenuItem
            {
                Text = "保存学习进度并退出"
            };
            saveAndQuit.Click += new EventHandler(SaveAndQuit);

            ApplicationBarMenuItem quit = new ApplicationBarMenuItem
            {
                Text = "退出"
            };
            quit.Click += new EventHandler(Quit);

            ApplicationBar.Buttons.Add(prevButton);
            ApplicationBar.Buttons.Add(nextButton);
            ApplicationBar.MenuItems.Add(starOrUnstarWord);
            ApplicationBar.MenuItems.Add(pinToStart);
            ApplicationBar.MenuItems.Add(share);
            ApplicationBar.MenuItems.Add(saveAndQuit);
            ApplicationBar.MenuItems.Add(quit);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.New) // initialize
            {
                currentWordIndex = startIndex;
                TextBlock_Title.Text = wordSource.title;
                TextBlock_WordCount.Text = wordList.Count.ToString();
                ProgressBar_Progress.Maximum = wordList.Count;

                UserControl_WordCardCanvas.SetParameter(wordList, wordCardType, currentWordIndex);
                UpdateUI();
            }
            else if (isNewPageInstance)                 // restore from saved page states
            {
                RestorePageStates();
                isNewPageInstance = false;
            }

            startTime = DateTime.Now;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            TimeSpan timeSpan = new TimeSpan(DateTime.Now.Ticks - startTime.Ticks);
            DateTime date = new DateTime(startTime.Year, startTime.Month, startTime.Day);
            StudyHistoryOperations.InsertEntry(date, timeSpan.TotalMinutes, wordSource.title);

            // save page states
            if (e.NavigationMode != NavigationMode.Back)
            {
                SavePageStates();
                IsolatedStorageFileHelper.SaveRunState(wordList, wordSource.sourceId, wordCardType, currentWordIndex);
            }
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
                ProgressBar_Progress.Maximum = wordList.Count;
            }
            if (State.ContainsKey(PageStateStrings.WORD_SOURCE_ID))
            {
                int sourceId = int.Parse(State[PageStateStrings.WORD_SOURCE_ID] as string);
                List<WordSource> wordSourceList = (Application.Current as App).wordSourceList;
                wordSource = WordSourceHelper.GetWordSourceById(wordSourceList , sourceId);
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
                ProgressBar_Progress.Value = currentWordIndex + 1;
            }

            if (State.ContainsKey(PageStateStrings.WORD_LIST) && State.ContainsKey(PageStateStrings.WORD_CARD_TYPE) && State.ContainsKey(PageStateStrings.CURRENT_WORD_INDEX))
            {
                UserControl_WordCardCanvas.SetParameter(wordList, wordCardType, currentWordIndex);
                UpdateUI();
            }
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Quit(sender, null);
        }

        private void ContentPanel_GestureListener_Flick(object sender, FlickGestureEventArgs e)
        {
            if (e.Angle > 135 && e.Angle < 225)         // to right 
            {
                MoveToNextWord(sender, null);
            }
            else if (e.Angle > 315 || e.Angle < 45)     // to left 
            {
                MoveToPrevWord(sender, null);
            } 
        }

        void MoveToPrevWord(object sender, EventArgs e)
        {
            currentWordIndex = UserControl_WordCardCanvas.MovePrev();
            UpdateUI();
        }

        void MoveToNextWord(object sender, EventArgs e)
        {
            if (currentWordIndex + 1 >= wordList.Count)
            {
                IsolatedStorageFileHelper.DeleteRunState();

                MessageBox.Show("本次学习已完成。");
                for (int i = 0; i < NavigationService.BackStack.Count() - 1; i++)
                    NavigationService.RemoveBackEntry();
                NavigationService.GoBack();
            }
            else
            {
                currentWordIndex = UserControl_WordCardCanvas.MoveNext();
                UpdateUI();
            }
        }



        void PinOrUnpinToStart(object sender, EventArgs e)
        {
            Word currentWord = wordList[currentWordIndex];
            int sourceId = wordSource.sourceId;
            int wordId = currentWord.wordId;
            string wordUriParameter = "sourceId=" + sourceId.ToString() + "&wordId=" + wordId.ToString();
            string fileName = sourceId.ToString() + "-" + wordId.ToString() + ".jpg";

            ShellTile tileToFind = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains(wordUriParameter));
            if (tileToFind != null)
            {
                tileToFind.Delete();
                IsolatedStorageFileHelper.DeleteFile("/Shared/ShellContent/StaticWordTiles/" + fileName);
            }
            else
            {
                UserControl wordPicUS = UserControlHelper.GenerateWordTile(currentWord);
                WriteableBitmap wordPic = new WriteableBitmap(wordPicUS, null);

                IsolatedStorageFileHelper.CreateDirectory("/Shared/ShellContent/StaticWordTiles");
                IsolatedStorageFileHelper.SaveWriteableBitmap("/Shared/ShellContent/StaticWordTiles/" + fileName, wordPic, 336, 336, 100);

                StandardTileData tileData = new StandardTileData
                {
                    BackgroundImage = new Uri("isostore:/Shared/ShellContent/StaticWordTiles/" + fileName, UriKind.Absolute)
                };
                ShellTile.Create(new Uri("/MainPage.xaml?" + wordUriParameter, UriKind.Relative), tileData);
            }

            UpdateUI();
            // delete useless picture
            DeleteUselessStaticTilePictures();
        }

        void Quit(object sender, EventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("学习正在进行中，你确定真的要退出吗？如果退出，将会丢失学习进度。", "你确定吗？", MessageBoxButton.OKCancel);
            if (messageBoxResult == MessageBoxResult.OK)
            {
                IsolatedStorageFileHelper.DeleteRunState();
                NavigationService.GoBack();
            }
        }

        void SaveAndQuit(object sender, EventArgs e)
        {
            IsolatedStorageFileHelper.SaveRunState(wordList, wordSource.sourceId, wordCardType, currentWordIndex);
            for (int i = 0; i < NavigationService.BackStack.Count() - 1; i++)
                NavigationService.RemoveBackEntry();
            NavigationService.GoBack();
        }

        void ApplicationBarMenuItem_StarOrUnstar_Click(object sender, EventArgs e)
        {
            if (StarredWordOperations.IsWordStarred(wordSource.sourceId, wordList[currentWordIndex].wordId))
                StarredWordOperations.DeleteEntry(wordSource.sourceId, wordList[currentWordIndex].wordId);
            else
                StarredWordOperations.InsertEntry(wordSource.sourceId, wordList[currentWordIndex].wordId);

            UpdateUI();
        }

        private void UpdateUI()
        {
            ApplicationBar.Opacity = 0.7;
            ApplicationBarIconButton prevButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
            ApplicationBarIconButton nextButton = ApplicationBar.Buttons[1] as ApplicationBarIconButton;
            ApplicationBarMenuItem starItem = ApplicationBar.MenuItems[0] as ApplicationBarMenuItem;
            ApplicationBarMenuItem pinToStart = ApplicationBar.MenuItems[1] as ApplicationBarMenuItem;

            TextBlock_CurrentWordNumber.Text = (currentWordIndex + 1).ToString();
            ProgressBar_Progress.Value = currentWordIndex + 1;

            if (currentWordIndex <= 0)
                prevButton.IsEnabled = false;
            else
                prevButton.IsEnabled = true;

            if (currentWordIndex + 1 >= wordList.Count)
            {
                nextButton.Text = "完成";
                nextButton.IconUri = new Uri("/Resource/ApplicationBarIcon/CheckMark.png", UriKind.Relative);
            }
            else
            {
                nextButton.Text = "下一个";
                nextButton.IconUri = new Uri("/Resource/ApplicationBarIcon/RightArrow.png", UriKind.Relative);
            }

            if (StarredWordOperations.IsWordStarred(wordSource.sourceId, wordList[currentWordIndex].wordId))
            {
                starItem.Text = "从生词本中移除";
                Image_Star.Source = new BitmapImage(new Uri("/Resource/Picture/starred.png", UriKind.Relative));
            }
            else
            {
                starItem.Text = "加入生词本";
                Image_Star.Source = new BitmapImage(new Uri("/Resource/Picture/unstarred.png", UriKind.Relative));
            }

            if (!IsWordPinnedToStart(wordSource.sourceId, wordList[currentWordIndex].wordId))
            {
                pinToStart.Text = "固定到开始屏幕";
            }
            else
            {
                pinToStart.Text = "从开始屏幕取消固定";
            }
        }

        private bool IsWordPinnedToStart(int sourceId, int wordId)
        {
            string wordUriParameter = "sourceId=" + sourceId.ToString() + "&wordId=" + wordId.ToString();
            ShellTile tileToFind = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains(wordUriParameter));
            if (tileToFind != null)
                return true;
            else
                return false;
        }

        private void DeleteUselessStaticTilePictures()
        {
            string[] fileNames = IsolatedStorageFileHelper.GetFileNames("/Shared/ShellContent/StaticWordTiles/*");
            for (int i = 0; i < fileNames.Count(); i++)
            {  
                string fileName = fileNames[i];
                System.Diagnostics.Debug.WriteLine(fileName);
                string[] sourceIdAndWordIdString = (fileName.Split('.'))[0].Split('-');
                if (sourceIdAndWordIdString.Count() == 2)
                {
                    string sourceIdString = sourceIdAndWordIdString[0];
                    string wordIdString = sourceIdAndWordIdString[1];
                    int sourceId = 0;
                    int wordId = 0;
                    try
                    {
                        sourceId = int.Parse(sourceIdString);
                        wordId = int.Parse(wordIdString);
                    }
                    catch
                    {
                        IsolatedStorageFileHelper.DeleteFile("/Shared/ShellContent/StaticWordTiles/" + fileName);
                        continue;
                    }

                    if (!IsWordPinnedToStart(sourceId, wordId))
                    {
                        IsolatedStorageFileHelper.DeleteFile("/Shared/ShellContent/StaticWordTiles/" + fileName);
                    }
                }
                else
                {
                    IsolatedStorageFileHelper.DeleteFile("/Shared/ShellContent/StaticWordTiles/" + fileName);
                    continue;
                }
            }
        }
    }
}