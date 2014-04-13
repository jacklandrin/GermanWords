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

using GermanWordsClassLibrary.Class;
using GermanWordsClassLibrary.Helper;

namespace GermanWordsClassLibrary.UserControl.WordCard
{
    public partial class OneSideWordCard : System.Windows.Controls.UserControl
    {
        public string nowword = "";
        public double ScaleX
        {
            get
            {
                return ScaleTransform_Layout.ScaleX;
            }
            set
            {
                ScaleTransform_Layout.ScaleX = value;
            }
        }
        public double ScaleY
        {
            get
            {
                return ScaleTransform_Layout.ScaleY;
            }
            set
            {
                ScaleTransform_Layout.ScaleY = value;
            }
        }
        public Brush BackgroundBrush
        {
            get
            {
                return LayoutRoot.Background;
            }
            set
            {
                LayoutRoot.Background = value;
            }
        }

        public OneSideWordCard()
        {
            InitializeComponent();
        }

        public OneSideWordCard(Word word)
            : this()
        {
            TextBlock_Line0.Text = Word.WordTypeToString(word.wordType);
            TextBlock_Line1.Text = word.word;
            nowword = word.word;
            TextBlock_Line2.Text = word.translation;

            UIHelper.AdjustWrappedTextBlockFontSize(TextBlock_Line1, 420, 150);
            UIHelper.ReduceFontSizeByWidth(TextBlock_Line2, 420);
        }

        private async void LayoutRoot_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (IsolatedStorageSettingsHelper.IsSpeechExist())
            {
                try
                {
                    await Speech.synthesizer.SpeakTextAsync(nowword);
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                }
            }
            
        }
    }
}
