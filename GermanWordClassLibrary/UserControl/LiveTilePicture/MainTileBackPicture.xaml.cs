using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using GermanWordsClassLibrary.Helper;

namespace GermanWordsClassLibrary.UserControl.LiveTilePicture
{
    public partial class MainTileBackPicture : System.Windows.Controls.UserControl
    {
        public MainTileBackPicture(string head, double value, string unit)
        {
            InitializeComponent();

            TextBlock_Head.Text = head;
            TextBlock_Value.Text = value.ToString();
            TextBlock_Unit.Text = unit;

            // adjust font size of UI elements
            UIHelper.ReduceFontSizeByWidth(TextBlock_Value, 260);

            // adjust location of TextBlock_Unit
            Canvas.SetLeft(TextBlock_Unit, TextBlock_Value.ActualWidth + 10);
            Canvas.SetTop(TextBlock_Unit, TextBlock_Value.ActualHeight - TextBlock_Unit.ActualHeight);

            // adjust location of Canvas_Content
            double targetValue = 336 - TextBlock_Value.ActualWidth - TextBlock_Unit.ActualWidth - 20;
            Canvas.SetLeft(Canvas_Content, targetValue);
        }
    }
}
