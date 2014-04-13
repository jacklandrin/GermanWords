using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GermanWordsClassLibrary.Class;

namespace GermanWordsClassLibrary.Helper
{
    public class WordSourceHelper
    {
        public static WordSource GetWordSourceById(List<WordSource> wordSourceList, int sourceId)
        {
            for (int i = 0; i < wordSourceList.Count; i++)
            {
                if (wordSourceList[i].sourceId == sourceId)
                    return wordSourceList[i];
            }
            return null;
        }
    }
}
