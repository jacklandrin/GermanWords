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

using System.Runtime.Serialization;

namespace GermanWordsClassLibrary.Class
{
    [DataContract]
    public class WordSource
    {
        [DataMember]
        public int sourceId { get; private set; }
        [DataMember]
        public string title { get; private set; }
        [DataMember]
        public Uri fileUri { get; private set; }

        public WordSource(int sourceId, string title, string filePath)
        {
            this.sourceId = sourceId;
            this.title = title;
            fileUri = new Uri(filePath, UriKind.Relative);
        }
    }
}
