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
    public partial class LiveTileVerbPicture : System.Windows.Controls.UserControl
    {
        public LiveTileVerbPicture()
        {
            InitializeComponent();
        }

        public LiveTileVerbPicture(Verb verb)
            : this()
        {
            TextBlock_Line1.Text = verb.word;
            TextBlock_Line2.Text = verb.presentForm;
            if (verb.presentForm.Equals(string.Empty) == false && verb.pastForm.Equals(string.Empty) == false)
                TextBlock_Line2.Text += ",";
            TextBlock_Line2.Text += verb.pastForm;
            TextBlock_Line3.Text = Verb.PerfectAuxiliaryVerbToString(verb.perfectAuxiliaryVerb);
            TextBlock_Line3.Text += verb.perfectForm;
            TextBlock_Line4.Text = verb.translation;

            UIHelper.ReduceFontSizeByWidth(TextBlock_Line1, 300);
            UIHelper.ReduceFontSizeByWidth(TextBlock_Line2, 300);
            UIHelper.ReduceFontSizeByWidth(TextBlock_Line3, 300);
            UIHelper.ReduceFontSizeByWidth(TextBlock_Line4, 300);
            UIHelper.SetTextBlockVerticalCenterOfCanvas(TextBlock_Line1);
            UIHelper.SetTextBlockVerticalCenterOfCanvas(TextBlock_Line2);
            UIHelper.SetTextBlockVerticalCenterOfCanvas(TextBlock_Line3);
            UIHelper.SetTextBlockVerticalCenterOfCanvas(TextBlock_Line4);
        }
    }
}
