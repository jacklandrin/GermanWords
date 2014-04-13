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
    public partial class LiveTileNounPicture : System.Windows.Controls.UserControl
    {
        public LiveTileNounPicture()
        {
            InitializeComponent();
        }

        public LiveTileNounPicture(Noun noun)
            : this()
        {
            TextBlock_Line1.Text =  Noun.GenderToString(noun.gender);

            TextBlock_Line3.Inlines.Clear();
            if (noun.genitivForm.Equals("") == false)
            {
                Run run = new Run();
                run.Text = noun.genitivForm + ", ";
                TextBlock_Line3.Inlines.Add(run);
            }
            if (noun.pluralForm.Equals("") == false)
            {
                if (noun.pluralForm.StartsWith("-.."))
                {
                    Underline underline = new Underline();
                    Run run = new Run();
                    run.Text = "..";
                    underline.Inlines.Add(run);

                    Run run2 = new Run();
                    run2.Text = noun.pluralForm.Substring(3);
                    TextBlock_Line3.Inlines.Add(underline);
                    TextBlock_Line3.Inlines.Add(run2);
                }
                else
                {
                    Run run = new Run();
                    run.Text = noun.pluralForm;
                    TextBlock_Line3.Inlines.Add(run);
                }
            }

            TextBlock_Line2.Text = noun.word;
            TextBlock_Line4.Text = noun.translation;

            UIHelper.ReduceFontSizeByWidth(TextBlock_Line2, 300);
            UIHelper.ReduceFontSizeByWidth(TextBlock_Line4, 300);
            UIHelper.SetTextBlockVerticalCenterOfCanvas(TextBlock_Line2);
            UIHelper.SetTextBlockVerticalCenterOfCanvas(TextBlock_Line4);
        }
    }
}
