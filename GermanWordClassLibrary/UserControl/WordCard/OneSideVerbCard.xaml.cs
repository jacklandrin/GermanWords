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
    public partial class OneSideVerbCard : System.Windows.Controls.UserControl
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

        public OneSideVerbCard()
        {
            InitializeComponent();
        }

        public OneSideVerbCard(Verb verb)
            : this()
        {
            TextBlock_Line0.Text = Word.WordTypeToString(verb.wordType);
            TextBlock_Line1.Text = verb.word;
            nowword = verb.word;
            TextBlock_Line2.Text = verb.presentForm;
            if (verb.presentForm.Equals(string.Empty) == false && verb.pastForm.Equals(string.Empty) == false)
                TextBlock_Line2.Text += ",";
            TextBlock_Line2.Text += verb.pastForm;
            TextBlock_Line3.Text = Verb.PerfectAuxiliaryVerbToString(verb.perfectAuxiliaryVerb);
            TextBlock_Line3.Text += verb.perfectForm;
            TextBlock_Line4.Text = verb.translation;

            UIHelper.ReduceFontSizeByWidth(TextBlock_Line1, 420);
            UIHelper.ReduceFontSizeByWidth(TextBlock_Line2, 420);
            UIHelper.ReduceFontSizeByWidth(TextBlock_Line3, 420);
            UIHelper.ReduceFontSizeByWidth(TextBlock_Line4, 420);
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
