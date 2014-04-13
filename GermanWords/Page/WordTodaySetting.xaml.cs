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

using GermanWordsClassLibrary.Class;
using GermanWordsClassLibrary.Helper;

namespace GermanWords.Page
{
    public partial class WordTodaySetting : PhoneApplicationPage
    {
        public WordTodaySetting()
        {
            InitializeComponent();

            ListPicker_WordSource.ItemsSource = (Application.Current as App).wordSourceList;
            ListPicker_WordSource.SelectionChanged += ListPicker_WordSource_SelectionChanged;

            // read settings
            int cardType = 1;
            if (IsolatedStorageSettingsHelper.IsSettingExisted(IsolatedStorageSettingsHelper.SettingStrings.WORD_TODAY_CARD_TYPE))
            {
                object value = IsolatedStorageSettingsHelper.ReadSetting(IsolatedStorageSettingsHelper.SettingStrings.WORD_TODAY_CARD_TYPE);
                if (value is int)
                {
                    cardType = (int)value;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("ERROR OCCURED IN 'WordTodaySetting.xaml.cs'.");
                }
            }
            if (cardType == 1)
                ListPicker_CardType.SelectedIndex = 0;
            else if (cardType == 2)
                ListPicker_CardType.SelectedIndex = 1;
            else if (cardType == 3)
                ListPicker_CardType.SelectedIndex = 2;
            else
            {
                ListPicker_CardType.SelectedIndex = 0;
                System.Diagnostics.Debug.WriteLine("ERROR OCCURED IN 'WordTodaySetting.xaml.cs'.");
            }

            int index = 0;
            if (IsolatedStorageSettingsHelper.IsSettingExisted(IsolatedStorageSettingsHelper.SettingStrings.WORD_TODAY_SELECT_MODE_INDEX))
            {
                object value = IsolatedStorageSettingsHelper.ReadSetting(IsolatedStorageSettingsHelper.SettingStrings.WORD_TODAY_SELECT_MODE_INDEX);
                if (value is int)
                {
                    index = (int)value;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("ERROR OCCURED IN 'WordTodaySetting.xaml.cs'.");
                }
            }
            if (index == 0)
                RadioButton_Random.IsChecked = true;
            else if (index == 1)
                RadioButton_StarredWords.IsChecked = true;
            else if (index == 2)
                RadioButton_SelectedSources.IsChecked = true;
            //else if (index == 3)
            //    RadioButton_Smart.IsChecked = true;
            else
            {
                RadioButton_Random.IsChecked = true;
                System.Diagnostics.Debug.WriteLine("ERROR OCCURED IN 'WordTodaySetting.xaml.cs'.");
            }

            List<WordSource> selectedSourceList = null;
            if (IsolatedStorageSettingsHelper.IsSettingExisted(IsolatedStorageSettingsHelper.SettingStrings.WORD_TODAY_SELECTED_SOURCE_LIST))
            {
                object value = IsolatedStorageSettingsHelper.ReadSetting(IsolatedStorageSettingsHelper.SettingStrings.WORD_TODAY_SELECTED_SOURCE_LIST);
                if (value is List<WordSource>)
                {
                    selectedSourceList = value as List<WordSource>;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("ERROR OCCURED IN 'WordTodaySetting.xaml.cs'.");
                }
            }
            if (selectedSourceList != null && selectedSourceList.Count >= 1)
            {
                ListPicker_WordSource.SelectedItem = selectedSourceList[0];
            }
        }

        private void ListPicker_CardType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListPicker_CardType != null)
            {
                int cardType = ListPicker_CardType.SelectedIndex + 1;
                IsolatedStorageSettingsHelper.WriteSetting(IsolatedStorageSettingsHelper.SettingStrings.
                    WORD_TODAY_CARD_TYPE, cardType);
            }
        }

        private void RadioButton_Random_Checked(object sender, RoutedEventArgs e)
        {
            if (ListPicker_WordSource != null)
            {
                ListPicker_WordSource.Visibility = Visibility.Collapsed;
                IsolatedStorageSettingsHelper.WriteSetting(
                    IsolatedStorageSettingsHelper.SettingStrings.WORD_TODAY_SELECT_MODE_INDEX, 0);
            }
        }

        private void RadioButton_StarredWords_Checked(object sender, RoutedEventArgs e)
        {
            if (ListPicker_WordSource != null)
            {
                ListPicker_WordSource.Visibility = Visibility.Collapsed;
                IsolatedStorageSettingsHelper.WriteSetting(
                    IsolatedStorageSettingsHelper.SettingStrings.WORD_TODAY_SELECT_MODE_INDEX, 1);
            }
        }

        private void RadioButton_SelectedSources_Checked(object sender, RoutedEventArgs e)
        {
            if (ListPicker_WordSource != null)
            {
                ListPicker_WordSource.Visibility = Visibility.Visible;
                IsolatedStorageSettingsHelper.WriteSetting(
                    IsolatedStorageSettingsHelper.SettingStrings.WORD_TODAY_SELECT_MODE_INDEX, 2);
            }
        }

        private void RadioButton_Smart_Checked(object sender, RoutedEventArgs e)
        {
            if (ListPicker_WordSource != null)
            {
                ListPicker_WordSource.Visibility = Visibility.Collapsed;
                IsolatedStorageSettingsHelper.WriteSetting(
                    IsolatedStorageSettingsHelper.SettingStrings.WORD_TODAY_SELECT_MODE_INDEX, 3);
            }
        }

        private void ListPicker_WordSource_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListPicker_WordSource != null)
            {
                List<WordSource> selectedSourceList = new List<WordSource>();
                selectedSourceList.Add(ListPicker_WordSource.SelectedItem as WordSource);
                IsolatedStorageSettingsHelper.WriteSetting(
                    IsolatedStorageSettingsHelper.SettingStrings.WORD_TODAY_SELECTED_SOURCE_LIST, selectedSourceList);
            }
        }
    }
}