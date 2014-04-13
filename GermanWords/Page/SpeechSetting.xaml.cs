using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using GermanWordsClassLibrary.Helper;

namespace GermanWords.Page
{
    public partial class SpeechSetting : PhoneApplicationPage
    {
        public SpeechSetting()
        {
            InitializeComponent();
            Loaded += SpeechSetting_Loaded;
        }

        void SpeechSetting_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsolatedStorageSettingsHelper.IsSpeechPackageExist())
            {
                ToggleSwitch_MainTile.IsChecked = false;
                ToggleSwitch_MainTile.IsEnabled = false;
                IsolatedStorageSettingsHelper.SetSPeechState(false);
            }
            else
            {
                ToggleSwitch_MainTile.IsEnabled = true;
                ToggleSwitch_MainTile.IsChecked = IsolatedStorageSettingsHelper.IsSpeechExist() ? true : false;

            }
        }

        private void ToggleSwitch_MainTile_Checked(object sender, RoutedEventArgs e)
        {
            IsolatedStorageSettingsHelper.SetSPeechState(true);
        }

        private void ToggleSwitch_MainTile_Unchecked(object sender, RoutedEventArgs e)
        {
            IsolatedStorageSettingsHelper.SetSPeechState(false);
        }
    }
}