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

using GermanWordsClassLibrary.Class;
using GermanWordsClassLibrary.Helper;

namespace GermanWordsClassLibrary.UserControl.LiveTilePicture
{
    public partial class LiveTileAbbrPicture : System.Windows.Controls.UserControl
    {
        public LiveTileAbbrPicture()
        {
            InitializeComponent();
        }

        public LiveTileAbbrPicture(Abbreviation abbr)
            : this()
        {
            TextBlock_Line1.Text = abbr.word;
            TextBlock_Line2.Text = abbr.fullWord;
            TextBlock_Line3.Text = abbr.translation;

            UIHelper.ReduceFontSizeByWidth(TextBlock_Line1, 300);
            UIHelper.ReduceFontSizeByWidth(TextBlock_Line2, 300);
            UIHelper.ReduceFontSizeByWidth(TextBlock_Line3, 300);
        }
    }
}
