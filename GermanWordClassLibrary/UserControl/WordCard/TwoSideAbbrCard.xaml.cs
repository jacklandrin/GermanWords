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

using GermanWordsClassLibrary.Class;
using GermanWordsClassLibrary.Helper;

namespace GermanWordsClassLibrary.UserControl.WordCard
{
    public partial class TwoSideAbbrCard : System.Windows.Controls.UserControl
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

        public TwoSideAbbrCard()
        {
            InitializeComponent();
            frontGrid = Grid_Front;
            backGrid = Grid_Back;

            Grid_Front.Opacity = 100;
            Grid_Back.Opacity = 0;
        }

        public TwoSideAbbrCard(Abbreviation abbr, bool isWordInFront)
            : this()
        {
            if (isWordInFront)
            {
                TextBlock_FrontLine1.Text = abbr.word;
                TextBlock_BackLine1.Text = abbr.fullWord;
                TextBlock_BackLine2.Text = abbr.translation;
            }
            else
            {
                TextBlock_FrontLine1.Text = abbr.translation;
                TextBlock_BackLine1.Text = abbr.word;
                TextBlock_BackLine2.Text = abbr.fullWord;
            }
            nowword = abbr.fullWord;
            UIHelper.ReduceFontSizeByWidth(TextBlock_FrontLine1, 420);
            UIHelper.ReduceFontSizeByWidth(TextBlock_BackLine1, 420);
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
