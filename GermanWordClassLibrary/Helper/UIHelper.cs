using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace GermanWordsClassLibrary.Helper
{
    public class UIHelper
    {
        public static void ReduceFontSizeByWidth(TextBlock target, int maxWidth)
        {
            while (target.ActualWidth > maxWidth)
                target.FontSize--;
        }

        public static void AdjustWrappedTextBlockFontSize(TextBlock target, int maxWidth, int maxHeight)
        {
            TextBlock tb = new TextBlock();
            tb.Width = maxWidth;
            tb.Text = target.Text;
            tb.TextWrapping = System.Windows.TextWrapping.Wrap;
            tb.FontSize = target.FontSize;

            while (tb.ActualHeight > maxHeight)
            {
                tb.FontSize--;
            }

            TextBlock tb2 = new TextBlock();
            tb2.FontSize = tb.FontSize;
            string[] singleWords = target.Text.Split(' ');
            for (int i = 0; i < singleWords.Length; i++)
            {
                tb2.Text = singleWords[i];
                while (tb2.ActualWidth > maxWidth)
                    tb2.FontSize--;
            }

            target.FontSize = tb2.FontSize;
        }

        public static void SetTextBlockVerticalCenterOfCanvas(TextBlock target)
        {
            double canvasTopValue = ((target.Parent as Canvas).Height - target.ActualHeight) / 2;
            Canvas.SetTop(target, canvasTopValue);
        }
    }
}
