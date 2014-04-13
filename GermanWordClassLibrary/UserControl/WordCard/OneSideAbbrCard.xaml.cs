using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using System.Windows.Documents;
using System.Windows.Media;
using GermanWordsClassLibrary.Class;
using GermanWordsClassLibrary.Helper;

namespace GermanWordsClassLibrary.UserControl.WordCard
{
    public partial class OneSideAbbrCard : System.Windows.Controls.UserControl
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

        public OneSideAbbrCard()
        {
            InitializeComponent();
        }

        public OneSideAbbrCard(Abbreviation abbr)
            : this()
        {
            TextBlock_Line0.Text = Word.WordTypeToString(abbr.wordType);
            TextBlock_Line1.Text = abbr.word;
            nowword = abbr.word;
            TextBlock_Line2.Text = abbr.fullWord;
            TextBlock_Line3.Text = abbr.translation;
            
            UIHelper.ReduceFontSizeByWidth(TextBlock_Line1, 420);
            UIHelper.ReduceFontSizeByWidth(TextBlock_Line2, 420);
            UIHelper.ReduceFontSizeByWidth(TextBlock_Line3, 420);
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

                }
            }
            
        }

    }
}
