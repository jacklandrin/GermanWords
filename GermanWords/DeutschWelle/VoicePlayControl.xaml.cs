using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Data;
using System.IO.IsolatedStorage;
using System.IO;
using System.Windows.Threading;

namespace GermanWords.DeutschWelle
{
    enum SpeedPlay
    {
        NORMAL,
        SLOW,
    }

    public partial class VoicePlayControl : UserControl
    {
        SpeedPlay speedplay = SpeedPlay.SLOW;
        //StatePlay stateplay = StatePlay.NOPLAY;
        string mp3name;
        string fmt = @"mm\:ss";
        private TimeSpan TotalTime;
        DispatcherTimer timer;
        bool isChangeSpeed = false;
       

        public VoicePlayControl()
        {
            InitializeComponent();
            Loaded += VoicePlayControl_Loaded;
        }

        void VoicePlayControl_Loaded(object sender, RoutedEventArgs e)
        {
            player.MediaOpened += player_MediaOpened;
            player.MediaEnded += player_MediaEnded;
        }

        void player_MediaEnded(object sender, RoutedEventArgs e)
        {
            isChangeSpeed = false;
            App.DWModel.DWArtical.StatePlay = StatePlay.NOPLAY;
            App.DWModel.DWArtical.PlayImage = "/images/play.png";
        }

        void player_MediaOpened(object sender, RoutedEventArgs e)
        {
            TotalTime = player.NaturalDuration.TimeSpan;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
            endTimeTBlock.Text = TotalTime.ToString(fmt);
            if (isChangeSpeed || App.DWModel.IsActivated)
            {
                player.Position = TimeSpan.FromSeconds(App.DWModel.DWArtical.ProgressValue * TotalTime.TotalSeconds);
                App.DWModel.IsActivated = false;
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (player.NaturalDuration.TimeSpan.TotalSeconds > 0)
            {
                if (TotalTime.TotalSeconds > 0)
                {
                    App.DWModel.DWArtical.ProgressValue = player.Position.TotalSeconds / TotalTime.TotalSeconds;
                    startTimeTBlock.Text = player.Position.ToString(fmt);

                }
            }
        }

        private void PlayOrdownloadMp3()
        {
            string mp3url = "";
            switch (speedplay)
            {
                case SpeedPlay.NORMAL:
                    if (App.DWModel.DWArtical.NormalPath != "")
                    {
                        mp3url = App.DWModel.DWArtical.NormalPath;
                    }
                    break;
                case SpeedPlay.SLOW:
                    if (App.DWModel.DWArtical.SlowPath != "")
                    {
                        mp3url = App.DWModel.DWArtical.SlowPath;
                    }
                    break;
            }
            if (mp3url != "")
            {
                mp3name = App.DWModel.DWArtical.Title.Replace(".", "").Replace(":", "") + speedplay.ToString() + ".mp3";
                try
                {
                    using (var isofile = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        if (isofile.FileExists(mp3name))
                        {
                            using (var fileStream = isofile.OpenFile(mp3name, FileMode.Open, FileAccess.Read))
                            {
                                player.SetSource(fileStream);
                                player.Play();
                                
                            }
                        }
                        else
                        {
                            var downloadClient = new WebClient();
                            downloadClient.OpenReadCompleted += downloadClient_OpenReadCompleted;
                            downloadClient.OpenReadAsync(new Uri(mp3url));
                            App.DWModel.DWArtical.Visibility = Visibility.Visible;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                App.DWModel.DWArtical.PlayImage = "/images/pause.png";
                App.DWModel.DWArtical.StatePlay = StatePlay.PLAYING;
            }
        }

        private void ControlMp3()
        {
            switch (App.DWModel.DWArtical.StatePlay)
            {
                case StatePlay.NOPLAY:
                    PlayOrdownloadMp3();
                    break;
                case StatePlay.PLAYING:
                    player.Pause();
                    App.DWModel.DWArtical.PlayImage = "/images/play.png";
                    App.DWModel.DWArtical.StatePlay = StatePlay.PAUSE;
                    break;
                case StatePlay.PAUSE:
                    player.Play();
                    App.DWModel.DWArtical.PlayImage = "/images/pause.png";
                    App.DWModel.DWArtical.StatePlay = StatePlay.PLAYING;
                    break;
            }
        }

        private void PlayImage_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            isChangeSpeed = false;
            ControlMp3();
        }

        void downloadClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                using (var strm = e.Result)
                {
                    using (var isofile = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        using (var isoFileStream = new IsolatedStorageFileStream(mp3name,FileMode.Create, isofile))
                        {
                            int read;
                            var buffer = new byte[10000];
                            while ((read = strm.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                isoFileStream.Write(buffer, 0, read);
                            }
                            isoFileStream.Seek(0, SeekOrigin.Begin);
                            player.SetSource(isoFileStream);
                            player.Play();
                            //player.Position = TimeSpan.FromSeconds(App.DWModel.DWArtical.ProgressValue * TotalTime.TotalSeconds);
                            App.DWModel.DWArtical.Visibility = Visibility.Collapsed;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                App.DWModel.DWArtical.Visibility = Visibility.Collapsed;
            }
            
        }

        private void SpeedImage_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            isChangeSpeed = true;
            if (speedplay == SpeedPlay.SLOW)
            {
                speedplay = SpeedPlay.NORMAL;
                App.DWModel.DWArtical.SpeedImage = "/images/normal.png";
            }
            else
            {
                speedplay = SpeedPlay.SLOW;
                App.DWModel.DWArtical.SpeedImage = "/images/slow.png";
            }
            if (App.DWModel.DWArtical.StatePlay == StatePlay.PLAYING)
            {
                player.Pause();
                PlayOrdownloadMp3();
            }
            
        }
    }
}
