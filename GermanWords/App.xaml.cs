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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using System.Windows.Media.Imaging;

using GermanWords.Helper;
using GermanWordsClassLibrary.Class;
using GermanWordsClassLibrary.Database;
using GermanWordsClassLibrary.Helper;
using GermanWordsClassLibrary.UserControl.LiveTilePicture;

using WeiboSdk;
using GermanWords.DeutschWelle.ViewModels;

namespace GermanWords
{
    public partial class App : Application
    {
        public List<WordSource> wordSourceList = new List<WordSource>();
        public static bool dictationOrLearn = true;

        private static int _pagecount = 1;

        public static int PageCount
        {
            get { return _pagecount; }
            set { _pagecount = value; }
        }

        private static DWModel _dwmodel;

        public static DWModel DWModel
        {
            get
            {
                if (_dwmodel == null)
                    _dwmodel = new DWModel();
                return _dwmodel;
            }
            set { _dwmodel = value; }
        }
        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions. 
            UnhandledException += Application_UnhandledException;

            // Standard Silverlight initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

            // Show graphics profiling information while debugging.
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Display the current frame rate counters.
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode, 
                // which shows areas of a page that are handed off to GPU with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Disable the application idle detection by setting the UserIdleDetectionMode property of the
                // application's PhoneApplicationService object to Disabled.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }

            // initialize word source list
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

            // set sina weibo
            SdkData.AppKey = "380284961";
            SdkData.AppSecret = "42d24822d2f2b6b5a48109363d11e2b3";
            SdkData.RedirectUri = "https://api.weibo.com/oauth2/default.html";

            StudyHistoryOperations.CreateDatabase();
            StarredWordOperations.CreateDatabase();
        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            UpgradeAppData();

            // re-register background agent
            BackgroundAgentHelper.RegisterBackgroundAgent();
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            App.DWModel.DWArtical.PlayImage = "/images/play.png";
            App.DWModel.DWArtical.StatePlay = StatePlay.NOPLAY;
            App.DWModel.IsActivated = true;
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
            UpdateMainTile();
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
            UpdateMainTile();
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {

                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        private void UpgradeAppData()
        {
            if (IsolatedStorageSettingsHelper.IsSettingExisted(IsolatedStorageSettingsHelper.SettingStrings.APP_VERSION))
            {
                Version currentVersion = null;
                object value = IsolatedStorageSettingsHelper.ReadSetting(IsolatedStorageSettingsHelper.SettingStrings.APP_VERSION);
                if (value is string)
                {
                    try
                    {
                        currentVersion = Version.Parse((string)value);
                    } catch { }
                }

                if (currentVersion != null)
                {
                    if (currentVersion.CompareTo(new Version(1, 3)) == 0)
                        return;

                    // upgrade app according to version
                }
            }
            else
            {
                // for version 1.0
                IsolatedStorageFileHelper.DeleteRunState();
            }

            IsolatedStorageSettingsHelper.DeleteUselessSettings();

            IsolatedStorageSettingsHelper.WriteSetting(
                IsolatedStorageSettingsHelper.SettingStrings.APP_VERSION, new Version(1,3).ToString());
        }   

        private void UpdateMainTile()
        {
            StandardTileData newTileData = new StandardTileData
                {
                    BackBackgroundImage = new Uri(String.Empty, UriKind.Relative)
                };

            bool isUpdateOn = true;
            object value = IsolatedStorageSettingsHelper.ReadSetting(IsolatedStorageSettingsHelper.SettingStrings.IS_MAIN_TILE_UPDATE_ON);
            if (value is bool)
            {
                isUpdateOn = (bool)value;
            }
            if (isUpdateOn)
            {
                List<StudyHistoryItem> todayList = StudyHistoryOperations.GetTodayList();
                double todayTotal = 0;
                for (int i = 0; i < todayList.Count; i++)
                {
                    todayTotal += todayList[i].DurationMinute;
                }

                MainTileBackPicture picControl = new MainTileBackPicture("今日", todayTotal, "分钟");
                WriteableBitmap bmp = new WriteableBitmap(picControl, null);
                IsolatedStorageFileHelper.CreateDirectory("/Shared/ShellContent/");
                IsolatedStorageFileHelper.SaveWriteableBitmap("/Shared/ShellContent/MainBack.jpg", bmp, 336, 336, 100);

                newTileData = new StandardTileData
                {
                    BackBackgroundImage = new Uri("isostore:/Shared/ShellContent/MainBack.jpg", UriKind.Absolute)
                };
            }

            ShellTile.ActiveTiles.First().Update(newTileData);
            IsolatedStorageSettingsHelper.WriteSetting(
                IsolatedStorageSettingsHelper.SettingStrings.MAIN_TILE_LAST_UPDATE_DATE, DateTime.Now.Date);
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new TransitionFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;
            RootFrame.Navigated += CompleteInitializePhoneApplication;
            RootFrame.Navigated += CheckForResetNavigation;

            RootFrame.Navigating += RootFrame_Navigating;
            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }
        void CheckForResetNavigation(object sender, NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Reset)
                RootFrame.Navigated += ClearBackStackAfterReset;
        }
        bool isReset = false;
        void RootFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            //Resest验证判断 (此处会执行两次)
            if (e.NavigationMode == NavigationMode.Reset)
            {
                isReset = true;
            }
            else if (e.NavigationMode == NavigationMode.New && isReset)
            {
                isReset = false;
                e.Cancel = true; // 取消导航到新页面s
                RootFrame.Navigated -= ClearBackStackAfterReset; // 取消导航完成事件(否则会直接退出程序)
            }
        }
        private void ClearBackStackAfterReset(object sender, NavigationEventArgs e)
        {
            RootFrame.Navigated -= ClearBackStackAfterReset;

            if (e.NavigationMode != NavigationMode.New)
                return;

            while (RootFrame.RemoveBackEntry() != null)
            {
                ;
            }
        }
        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion
    }
}