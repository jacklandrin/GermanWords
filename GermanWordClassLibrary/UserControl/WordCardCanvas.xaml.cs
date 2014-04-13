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
using System.Windows.Media.Animation;

using GermanWordsClassLibrary.Class;
using GermanWordsClassLibrary.Helper;
using GermanWordsClassLibrary.UserControl.WordCard;

namespace GermanWordsClassLibrary.UserControl
{
    public partial class WordCardCanvas : System.Windows.Controls.UserControl
    {
        List<StackPanel> targetStackPanelOrder;
        int currentIndex;

        // parameters
        List<Word> wordList;
        int cardType;
        int startIndex;

        public WordCardCanvas()
        {
            InitializeComponent();
            targetStackPanelOrder = new List<StackPanel>();
            targetStackPanelOrder.Add(StackPanel_Card1);
            targetStackPanelOrder.Add(StackPanel_Card2);
            targetStackPanelOrder.Add(StackPanel_Card3);
            currentIndex = 0;

            StackPanel_Card1.Children.Clear();
            StackPanel_Card2.Children.Clear();
            StackPanel_Card3.Children.Clear(); 

            // initialize parameters
            wordList = new List<Word>();
            cardType = 0;
            startIndex = 0;
        }

        public void SetParameter(List<Word> wordList, int cardType, int startIndex)
        {
            this.wordList = wordList;
            this.cardType = cardType;
            this.startIndex = startIndex;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            currentIndex = startIndex;
            Word word = new Word();
            if (startIndex - 1 > 0)
            {
                word = wordList[startIndex - 1];
                System.Windows.Controls.UserControl wordCardControl = GenerateWordCardControl(word);
                if (wordCardControl != null)
                    StackPanel_Card1.Children.Add(wordCardControl);
            }

            if (startIndex >= 0 && startIndex < wordList.Count)
            {
                word = wordList[startIndex];
                System.Windows.Controls.UserControl wordCardControl = GenerateWordCardControl(word);
                if (wordCardControl != null)
                    StackPanel_Card2.Children.Add(wordCardControl);
            }

            if (startIndex + 1 < wordList.Count)
            {
                word = wordList[startIndex + 1];
                System.Windows.Controls.UserControl wordCardControl = GenerateWordCardControl(word);
                if (wordCardControl != null)
                    StackPanel_Card3.Children.Add(wordCardControl);
            }
            //if (word != null)
            //{
            //    BaseData.word = word.word;
            //}
        }

        public int MoveNext()
        {
            if (currentIndex + 1 >= wordList.Count)
                return currentIndex;

            // prepare new data
            targetStackPanelOrder[2].Children.Clear();
            Word word = wordList[currentIndex + 1];
            System.Windows.Controls.UserControl wordCardControl = GenerateWordCardControl(word);
            if (wordCardControl != null)
                targetStackPanelOrder[2].Children.Add(wordCardControl);
            if (currentIndex - 1 < 0)
                targetStackPanelOrder[0].Children.Clear();
            
            // animate
            Storyboard_MoveNext.Stop();
            Storyboard.SetTargetName(Storyboard_MoveNext.Children[2], targetStackPanelOrder[0].Name);
            Storyboard.SetTargetName(Storyboard_MoveNext.Children[3], targetStackPanelOrder[1].Name);
            Storyboard.SetTargetName(Storyboard_MoveNext.Children[4], targetStackPanelOrder[2].Name);
            Storyboard_MoveNext.Begin();

            targetStackPanelOrder.Add(targetStackPanelOrder[0]);
            targetStackPanelOrder.RemoveAt(0);
            
            currentIndex++;
            return currentIndex;
        }

        public int MovePrev()
        {
            if (currentIndex - 1 < 0)
                return currentIndex;

            // prepare new data
            targetStackPanelOrder[0].Children.Clear();
            Word word = wordList[currentIndex - 1];
            System.Windows.Controls.UserControl wordCardControl = GenerateWordCardControl(word);
            if (wordCardControl != null)
                targetStackPanelOrder[0].Children.Add(wordCardControl);
            if (currentIndex + 1 >= wordList.Count)
                targetStackPanelOrder[2].Children.Clear();

            // animate
            Storyboard_MovePrev.Stop();
            Storyboard.SetTargetName(Storyboard_MovePrev.Children[2], targetStackPanelOrder[0].Name);
            Storyboard.SetTargetName(Storyboard_MovePrev.Children[3], targetStackPanelOrder[1].Name);
            Storyboard.SetTargetName(Storyboard_MovePrev.Children[4], targetStackPanelOrder[2].Name);
            Storyboard_MovePrev.Begin();

            targetStackPanelOrder.Insert(0, targetStackPanelOrder.Last());
            targetStackPanelOrder.RemoveAt(3);

            currentIndex--;
            return currentIndex;
        }

        private System.Windows.Controls.UserControl GenerateWordCardControl(Word word)
        {
            System.Windows.Controls.UserControl uc;

            switch (cardType)
            {
                case 1:
                    uc = UserControlHelper.GenerateOneSideWordCard(word, 1, 1, (Brush)Application.Current.Resources["PhoneAccentBrush"]);
                    break;
                case 2:
                    uc = UserControlHelper.GenerateTwoSideWordCard(word, true, 1, 1, (Brush)Application.Current.Resources["PhoneAccentBrush"]);
                    break;
                case 3:
                    uc = UserControlHelper.GenerateTwoSideWordCard(word, false, 1, 1, (Brush)Application.Current.Resources["PhoneAccentBrush"]);
                    break;
                default:
                    uc = null;
                    break;
            }

            return uc;
        }
    }
}
