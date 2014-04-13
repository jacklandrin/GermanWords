using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Windows.Media.Imaging;

using GermanWords.Helper;
using GermanWordsClassLibrary.Class;
using GermanWordsClassLibrary.UserControl.LiveTilePicture;
using GermanWordsClassLibrary.Helper;

namespace GermanWords.Page
{
    public partial class LiveTileSetting : PhoneApplicationPage
    {
        public LiveTileSetting()
        {
            InitializeComponent();

            ToggleSwitch_MainTile.IsChecked = IsolatedStorageSettingsHelper.IsMainTileUpdateOn();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string result = BackgroundAgentHelper.RegisterBackgroundAgent();
            if (result.Contains("The action is disabled"))
                TextBlock_NoBackgroundTaskPrompt.Visibility = Visibility.Visible;
            else
                TextBlock_NoBackgroundTaskPrompt.Visibility = Visibility.Collapsed;

            int secondaryTileNumer = 0;
            for (int i = 1; i < ShellTile.ActiveTiles.Count(); i++)
            {
                ShellTile tile = ShellTile.ActiveTiles.ElementAt(i);
                string tileIdString = BasicHelper.GetAttribute(tile.NavigationUri, "tileId");
                if (tileIdString != null)
                    secondaryTileNumer++;
            }
            TextBlock_SecondaryTileNumber.Text = secondaryTileNumer.ToString();

            if (secondaryTileNumer == 0)
            {
                Button_Refresh.IsEnabled = false;
            }

            string lastUpdateTimeString;
            if (secondaryTileNumer == 0)
            {
                lastUpdateTimeString = "N/A";
            }
            else
            {
                if (IsolatedStorageSettingsHelper.IsSettingExisted(IsolatedStorageSettingsHelper.SettingStrings.WORD_TILE_LAST_UPDATE_TIME))
                {
                    lastUpdateTimeString = GenerateLastUpdateTimeString(IsolatedStorageSettingsHelper.GetWordTileLastUpdateTime());
                }
                else
                    lastUpdateTimeString = "N/A";
            }
            TextBlock_LastUpdateTime.Text = lastUpdateTimeString;

            base.OnNavigatedTo(e);
        }

        private void ToggleSwitch_MainTile_Checked(object sender, RoutedEventArgs e)
        {
            IsolatedStorageSettingsHelper.SetMainTileUpdateState(true);
        }

        private void ToggleSwitch_MainTile_Unchecked(object sender, RoutedEventArgs e)
        {
            IsolatedStorageSettingsHelper.SetMainTileUpdateState(false);
        }

        private void Button_Create_Click(object sender, RoutedEventArgs e)
        {
            Random random = new Random();

            int tileId = 0;
            ShellTile tileToFind;
            do
            {
                tileId = random.Next(int.MaxValue);
                tileToFind = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("tileId=" + tileId.ToString()));
            } while (tileToFind != null);

            // select font and back word
            List<WordSource> wordSourceList = (Application.Current as App).wordSourceList;
            Word frontWord = selectWord(wordSourceList);
            Word backWord;
            do
            {
                backWord = selectWord(wordSourceList);
            } while (backWord.word.Equals(frontWord.word));
            
            // generate tile pictures
            System.Windows.Controls.UserControl frontWordTile = UserControlHelper.GenerateWordTile(frontWord);
            System.Windows.Controls.UserControl backWordTile = UserControlHelper.GenerateWordTile(backWord);
            WriteableBitmap frontBmp = new WriteableBitmap(frontWordTile, null);
            WriteableBitmap backBmp = new WriteableBitmap(backWordTile, null);
            IsolatedStorageFileHelper.CreateDirectory("/Shared/ShellContent/WordTiles");
            string frontFilePath = "/Shared/ShellContent/WordTiles/" + tileId.ToString() + "-front";
            string backFilePath = "/Shared/ShellContent/WordTiles/" + tileId.ToString() + "-back";
            IsolatedStorageFileHelper.SaveWriteableBitmap(frontFilePath, frontBmp, 336, 336, 100);
            IsolatedStorageFileHelper.SaveWriteableBitmap(backFilePath, backBmp, 336, 336, 100); 

            // create tile
            StandardTileData newTileData = new StandardTileData
            {
                BackgroundImage = new Uri("isostore:" + frontFilePath, UriKind.Absolute),
                BackBackgroundImage = new Uri("isostore:" + backFilePath, UriKind.Absolute)
            };
            ShellTile.Create(new Uri("/Page/LiveTileSetting.xaml?tileId=" + tileId.ToString(), UriKind.Relative), newTileData);
            IsolatedStorageSettingsHelper.WriteSetting(
                IsolatedStorageSettingsHelper.SettingStrings.WORD_TILE_LAST_UPDATE_TIME, DateTime.Now);
        }

        private void Button_Refresh_Click(object sender, RoutedEventArgs e)
        {
            // clear folder
            IsolatedStorageFileHelper.DeleteAllFilesOfDirectory("/Shared/ShellContent/WordTiles");
            
            for (int i = 1; i < ShellTile.ActiveTiles.Count(); i++)
            {
                ShellTile tile = ShellTile.ActiveTiles.ElementAt(i);

                // read tileId from NavigationUri
                string tileIdString = BasicHelper.GetAttribute(tile.NavigationUri, "tileId");
                if (tileIdString == null)
                    continue;
                int tileId = 0;
                if (!int.TryParse(tileIdString, out tileId))    // if tileId unavailable
                {
                    tile.Delete();
                    continue;
                }

                // select words
                List<WordSource> wordSourceList = (Application.Current as App).wordSourceList;
                Word frontWord = selectWord(wordSourceList);
                Word backWord;
                do
                {
                    backWord = selectWord(wordSourceList);
                } while (backWord.word.Equals(frontWord.word));

                // generate tile pictures
                System.Windows.Controls.UserControl frontWordTile = UserControlHelper.GenerateWordTile(frontWord);
                System.Windows.Controls.UserControl backWordTile = UserControlHelper.GenerateWordTile(backWord);
                WriteableBitmap frontBmp = new WriteableBitmap(frontWordTile, null);
                WriteableBitmap backBmp = new WriteableBitmap(backWordTile, null);
                IsolatedStorageFileHelper.CreateDirectory("/Shared/ShellContent/WordTiles");
                string frontFilePath = "/Shared/ShellContent/WordTiles/" + tileId.ToString() + "-front";
                string backFilePath = "/Shared/ShellContent/WordTiles/" + tileId.ToString() + "-back";
                IsolatedStorageFileHelper.SaveWriteableBitmap(frontFilePath, frontBmp, 336, 336, 100);
                IsolatedStorageFileHelper.SaveWriteableBitmap(backFilePath, backBmp, 336, 336, 100); 

                // update
                StandardTileData newTileData = new StandardTileData
                {
                    BackgroundImage = new Uri("isostore:" + frontFilePath, UriKind.Absolute),
                    BackBackgroundImage = new Uri("isostore:" + backFilePath, UriKind.Absolute)
                };
                tile.Update(newTileData);
            }

            // save last update time
            IsolatedStorageSettingsHelper.SetWordTileLastUpdateTime(DateTime.Now);
            TextBlock_LastUpdateTime.Text = GenerateLastUpdateTimeString(IsolatedStorageSettingsHelper.GetWordTileLastUpdateTime());
        }

        private string GenerateLastUpdateTimeString(DateTime time)
        {
            string lastUpdateTimeString = time.ToShortTimeString() ;
            if (time.Date.CompareTo(DateTime.Now.Date) != 0)
            {
                if (time.Date.AddDays(1).CompareTo(DateTime.Now.Date) == 0)
                {
                    lastUpdateTimeString = "昨天" + lastUpdateTimeString;
                }
                else if (time.Date.AddDays(2).CompareTo(DateTime.Now.Date) == 0)
                {
                    lastUpdateTimeString = "前天" + lastUpdateTimeString;
                }
                else
                {
                    lastUpdateTimeString = "很久以前";
                }
            }
            return lastUpdateTimeString;
        }

        private Word selectWord(List<WordSource> wordSourceList)
        {
            Random random = new Random();

            int sourceIndex = random.Next(wordSourceList.Count);
            WordSource wordSource = wordSourceList[sourceIndex];
            List<Word> wordList = WordFileHelper.ReadWordFileToList(wordSource.fileUri);
            int wordIndex = random.Next(wordList.Count);
            return wordList[wordIndex];
        }
    }
}