using System.Windows;
using Microsoft.Phone.Scheduler;

using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows.Media.Imaging;

using GermanWordsClassLibrary.Class;
using GermanWordsClassLibrary.Helper;
using GermanWordsClassLibrary.UserControl.LiveTilePicture;

namespace ScheduledTaskAgent1
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
        private static volatile bool _classInitialized;

        /// <remarks>
        /// ScheduledAgent constructor, initializes the UnhandledException handler
        /// </remarks>
        public ScheduledAgent()
        {
            if (!_classInitialized)
            {
                _classInitialized = true;
                // Subscribe to the managed exception handler
                Deployment.Current.Dispatcher.BeginInvoke(delegate
                {
                    Application.Current.UnhandledException += ScheduledAgent_UnhandledException;
                });
            }
        }

        /// Code to execute on Unhandled Exceptions
        private void ScheduledAgent_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        /// <summary>
        /// Agent that runs a scheduled task
        /// </summary>
        /// <param name="task">
        /// The invoked task
        /// </param>
        /// <remarks>
        /// This method is called when a periodic or resource intensive task is invoked
        /// </remarks>
        protected override void OnInvoke(ScheduledTask task)
        {
            bool isUIOperationFinished = false;
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    // update main tile
                    if (IsolatedStorageSettingsHelper.IsMainTileUpdateOn())
                        if (IsolatedStorageSettingsHelper.GetMainTileLastUpdateDate().CompareTo(DateTime.Now) != 0)
                        {
                            MainTileBackPicture picControl = new MainTileBackPicture("今日", 0, "分钟");
                            WriteableBitmap bmp = new WriteableBitmap(picControl, null);
                            IsolatedStorageFileHelper.CreateDirectory("/Shared/ShellContent/");
                            IsolatedStorageFileHelper.SaveWriteableBitmap("/Shared/ShellContent/MainBack.jpg", bmp, 173, 173, 100);

                            StandardTileData newTileData = new StandardTileData
                            {
                                BackBackgroundImage = new Uri("isostore:/Shared/ShellContent/MainBack.jpg", UriKind.Absolute)
                            };

                            ShellTile.ActiveTiles.First().Update(newTileData);
                            IsolatedStorageSettingsHelper.SetMainTileLastUpdateDate(DateTime.Now.Date);
                        }

                    // build word source list
                    List<WordSource> wordSourceList = new List<WordSource>();
                    wordSourceList.Add(new WordSource(1, "新求精德语初级I 1课", "./WordSource/StichwortDeutschGrundstufe/1.txt"));
                    wordSourceList.Add(new WordSource(2, "新求精德语初级I 2课", "./WordSource/StichwortDeutschGrundstufe/2.txt"));
                    wordSourceList.Add(new WordSource(3, "新求精德语初级I 3课", "./WordSource/StichwortDeutschGrundstufe/3.txt"));
                    wordSourceList.Add(new WordSource(4, "新求精德语初级I 4课", "./WordSource/StichwortDeutschGrundstufe/4.txt"));
                    wordSourceList.Add(new WordSource(5, "新求精德语初级I 5课", "./WordSource/StichwortDeutschGrundstufe/5.txt"));
                    wordSourceList.Add(new WordSource(6, "新求精德语初级I 6课", "./WordSource/StichwortDeutschGrundstufe/6.txt"));
                    wordSourceList.Add(new WordSource(7, "新求精德语初级I 7课", "./WordSource/StichwortDeutschGrundstufe/7.txt"));
                    wordSourceList.Add(new WordSource(8, "新求精德语初级I 8课", "./WordSource/StichwortDeutschGrundstufe/8.txt"));
                    wordSourceList.Add(new WordSource(9, "新求精德语初级I 9课", "./WordSource/StichwortDeutschGrundstufe/9.txt"));
                    wordSourceList.Add(new WordSource(10, "新求精德语初级I 10课", "./WordSource/StichwortDeutschGrundstufe/10.txt"));
                    wordSourceList.Add(new WordSource(11, "新求精德语初级I 11课", "./WordSource/StichwortDeutschGrundstufe/11.txt"));
                    wordSourceList.Add(new WordSource(12, "新求精德语初级I 12课", "./WordSource/StichwortDeutschGrundstufe/12.txt"));
                    wordSourceList.Add(new WordSource(13, "新求精德语初级I 13课", "./WordSource/StichwortDeutschGrundstufe/13.txt"));
                    wordSourceList.Add(new WordSource(14, "新求精德语初级I 14课", "./WordSource/StichwortDeutschGrundstufe/14.txt"));
                    wordSourceList.Add(new WordSource(15, "新求精德语初级II 15课", "./WordSource/StichwortDeutschGrundstufe/15.txt"));
                    wordSourceList.Add(new WordSource(16, "新求精德语初级II 16课", "./WordSource/StichwortDeutschGrundstufe/16.txt"));
                    wordSourceList.Add(new WordSource(17, "新求精德语初级II 17课", "./WordSource/StichwortDeutschGrundstufe/17.txt"));
                    wordSourceList.Add(new WordSource(18, "新求精德语初级II 18课", "./WordSource/StichwortDeutschGrundstufe/18.txt"));
                    wordSourceList.Add(new WordSource(19, "新求精德语初级II 19课", "./WordSource/StichwortDeutschGrundstufe/19.txt"));
                    wordSourceList.Add(new WordSource(20, "新求精德语初级II 20课", "./WordSource/StichwortDeutschGrundstufe/20.txt"));
                    wordSourceList.Add(new WordSource(21, "新求精德语初级II 21课", "./WordSource/StichwortDeutschGrundstufe/21.txt"));
                    wordSourceList.Add(new WordSource(22, "新求精德语初级II 22课", "./WordSource/StichwortDeutschGrundstufe/22.txt"));
                    wordSourceList.Add(new WordSource(23, "新求精德语初级II 23课", "./WordSource/StichwortDeutschGrundstufe/23.txt"));
                    wordSourceList.Add(new WordSource(24, "新求精德语初级II 24课", "./WordSource/StichwortDeutschGrundstufe/24.txt"));
                    wordSourceList.Add(new WordSource(25, "新求精德语初级II 25课", "./WordSource/StichwortDeutschGrundstufe/25.txt"));
                    wordSourceList.Add(new WordSource(26, "新求精德语初级II 26课", "./WordSource/StichwortDeutschGrundstufe/26.txt"));

                    // clear folder
                    IsolatedStorageFileHelper.DeleteAllFilesOfDirectory("/Shared/ShellContent/WordTiles");

                    // update secondary tiles
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
                        IsolatedStorageFileHelper.SaveWriteableBitmap(frontFilePath, frontBmp, 173, 173, 100);
                        IsolatedStorageFileHelper.SaveWriteableBitmap(backFilePath, backBmp, 173, 173, 100);

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
                    //.ApplicationSettings.Save();

                    isUIOperationFinished = true;
                });

            while (!isUIOperationFinished) ;
            NotifyComplete();
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