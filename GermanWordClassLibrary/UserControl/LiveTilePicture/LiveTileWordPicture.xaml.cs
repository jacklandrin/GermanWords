using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using GermanWordsClassLibrary.Class;
using GermanWordsClassLibrary.Helper;

namespace GermanWordsClassLibrary.UserControl.LiveTilePicture
{
    public partial class LiveTileWordPicture : System.Windows.Controls.UserControl
    {
        public LiveTileWordPicture()
        {
            InitializeComponent();
        }

        public LiveTileWordPicture(Word word)
            : this()
        {
            TextBlock_Line0.Text = Word.WordTypeToString(word.wordType);
            TextBlock_Line1.Text = word.word;
            TextBlock_Line2.Text = word.translation;

            UIHelper.AdjustWrappedTextBlockFontSize(TextBlock_Line1, 300, 160);
            UIHelper.ReduceFontSizeByWidth(TextBlock_Line2, 300);
            UIHelper.SetTextBlockVerticalCenterOfCanvas(TextBlock_Line1);
            UIHelper.SetTextBlockVerticalCenterOfCanvas(TextBlock_Line2);
        }
    }
}
