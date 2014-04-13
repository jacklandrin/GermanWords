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
using System.Windows.Media.Animation;
using System.Windows.Media.Animation;

using GermanWordsClassLibrary.Class;
using GermanWordsClassLibrary.Helper;

namespace GermanWordsClassLibrary.UserControl.WordCard
{
    public partial class TwoSideNounCard : System.Windows.Controls.UserControl
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

        private Grid frontGrid;
        private Grid backGrid;

        public TwoSideNounCard()
        {
            InitializeComponent();
            frontGrid = Grid_Front;
            backGrid = Grid_Back;

            Grid_Front.Opacity = 100;
            Grid_Back.Opacity = 0;
        }

        public TwoSideNounCard(Noun noun, bool isWordInFront)
            : this()
        {
            if (isWordInFront)
            {
                TextBlock_FrontLine1.Text = noun.word;
                TextBlock_BackLine2.Text = noun.translation;
            }
            else
            {
                TextBlock_FrontLine1.Text = noun.translation;
                TextBlock_BackLine2.Text = noun.word;
            }
            nowword = noun.word;
            switch (noun.gender)
            {
                case WordGender.Masculine:
                    TextBlock_BackLine1.Text = "der";
                    break;
                case WordGender.Feminine:
                    TextBlock_BackLine1.Text = "die";
                    break;
                case WordGender.Neuter:
                    TextBlock_BackLine1.Text = "das";
                    break;
                case WordGender.MasculineOrNeuter:
                    TextBlock_BackLine1.Text = "der/das";
                    break;
                case WordGender.None:
                    TextBlock_BackLine1.Text = "";
                    break;
            }

            TextBlock_BackLine3.Inlines.Clear();
            if (noun.genitivForm.Equals("") == false)
            {
                Run run = new Run();
                run.Text = noun.genitivForm + ", ";
                TextBlock_BackLine3.Inlines.Add(run);
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
                    TextBlock_BackLine3.Inlines.Add(underline);
                    TextBlock_BackLine3.Inlines.Add(run2);
                }
                else
                {
                    Run run = new Run();
                    run.Text = noun.pluralForm;
                    TextBlock_BackLine3.Inlines.Add(run);
                }
            }

            UIHelper.ReduceFontSizeByWidth(TextBlock_FrontLine1, 420);
            UIHelper.ReduceFontSizeByWidth(TextBlock_BackLine2, 420);
        }

        private void LayoutRoot_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (StoryBoard_TurnBack.GetCurrentState() == ClockState.Active)
                return;

            StoryBoard_TurnBack.Stop();
            Storyboard.SetTargetName(StoryBoard_TurnBack.Children[1], frontGrid.Name);
            Storyboard.SetTargetName(StoryBoard_TurnBack.Children[2], backGrid.Name);
            StoryBoard_TurnBack.Begin();

            Grid tempGrid = frontGrid;
            frontGrid = backGrid;
            backGrid = tempGrid;
        }
    }
}
