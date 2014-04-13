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
using GermanWordsClassLibrary;

namespace GermanWords.Page
{
    public partial class ListenPage : PhoneApplicationPage
    {
        public ListenPage()
        {
            InitializeComponent();
        }

        private async void ListenBtn_Click(object sender, RoutedEventArgs e)
        {
            if (IsolatedStorageSettingsHelper.IsSpeechExist())
            {
                try
                {
                    await Speech.synthesizer.SpeakTextAsync(TextBox_Sentence.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("您未开启语音设置!");
            }
        }
    }
}