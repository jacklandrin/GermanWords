using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Media;

using GermanWordsClassLibrary.Class;
using GermanWordsClassLibrary.UserControl.LiveTilePicture;
using GermanWordsClassLibrary.UserControl.WordCard;

namespace GermanWordsClassLibrary.Helper
{
    public class UserControlHelper
    {
        public static System.Windows.Controls.UserControl GenerateOneSideWordCard(Word word, double scaleX = 1, double scaleY = 1, Brush background = null)
        {
            System.Windows.Controls.UserControl uc;
            if (word is Noun)
            {
                uc = new OneSideNounCard(word as Noun);
                (uc as OneSideNounCard).ScaleX = scaleX;
                (uc as OneSideNounCard).ScaleY = scaleY;
                if (background != null)
                    (uc as OneSideNounCard).BackgroundBrush = background;
            }
            else if (word is Verb)
            {
                uc = new OneSideVerbCard(word as Verb);
                (uc as OneSideVerbCard).ScaleX = scaleX;
                (uc as OneSideVerbCard).ScaleY = scaleY;
                if (background != null)
                    (uc as OneSideVerbCard).BackgroundBrush = background;
            }
            else if (word is Abbreviation)
            {
                uc = new OneSideAbbrCard(word as Abbreviation);
                (uc as OneSideAbbrCard).ScaleX = scaleX;
                (uc as OneSideAbbrCard).ScaleY = scaleY;
                if (background != null) 
                    (uc as OneSideAbbrCard).BackgroundBrush = background;
            }
            else
            {
                uc = new OneSideWordCard(word);
                (uc as OneSideWordCard).ScaleX = scaleX;
                (uc as OneSideWordCard).ScaleY = scaleY;
                if (background != null) 
                    (uc as OneSideWordCard).BackgroundBrush = background;
            }
            uc.Width = 450;
            uc.Height = 300;

            return uc;
        }

        public static System.Windows.Controls.UserControl GenerateTwoSideWordCard(Word word, bool isWordInFront, double scaleX = 1, double scaleY = 1, Brush background = null)
        {
            System.Windows.Controls.UserControl uc;
            if (word is Noun)
            {
                uc = new TwoSideNounCard(word as Noun, isWordInFront);
                (uc as TwoSideNounCard).ScaleX = scaleX;
                (uc as TwoSideNounCard).ScaleY = scaleY;
                if (background != null)
                    (uc as TwoSideNounCard).BackgroundBrush = background;
            }
            else if (word is Verb)
            {
                uc = new TwoSideVerbCard(word as Verb, isWordInFront);
                (uc as TwoSideVerbCard).ScaleX = scaleX;
                (uc as TwoSideVerbCard).ScaleY = scaleY;
                if (background != null)
                    (uc as TwoSideVerbCard).BackgroundBrush = background;
            }
            else if (word is Abbreviation)
            {
                uc = new TwoSideAbbrCard(word as Abbreviation, isWordInFront);
                (uc as TwoSideAbbrCard).ScaleX = scaleX;
                (uc as TwoSideAbbrCard).ScaleY = scaleY;
                if (background != null)
                    (uc as TwoSideAbbrCard).BackgroundBrush = background;
            }
            else
            {
                uc = new TwoSideWordCard(word, isWordInFront);
                (uc as TwoSideWordCard).ScaleX = scaleX;
                (uc as TwoSideWordCard).ScaleY = scaleY;
                if (background != null)
                    (uc as TwoSideWordCard).BackgroundBrush = background;
            }
            uc.Width = 450;
            uc.Height = 300;

            return uc;
        }

        public static System.Windows.Controls.UserControl GenerateWordTile(Word word)
        {
            if (word is Noun)
                return new LiveTileNounPicture(word as Noun);
            else if (word is Verb)
                return new LiveTileVerbPicture(word as Verb);
            else if (word is Abbreviation)
                return new LiveTileAbbrPicture(word as Abbreviation);
            else
                return new LiveTileWordPicture(word);
        }
    }
}
