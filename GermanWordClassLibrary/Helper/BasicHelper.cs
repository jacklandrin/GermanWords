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
    public class BasicHelper
    {
        public static string GetAttribute(Uri uri, string attributeName)
        {
            string[] uriParts = uri.ToString().Split('?');
            if (uriParts.Length != 2) return null;
            string[] attributes = uriParts[1].Split('&');
            for (int i = 0; i < attributes.Length; i++)
            {
                string[] attributeParts = attributes[i].Split('=');
                if (attributeParts.Length != 2) continue;
                if (attributeParts[0].Trim().Equals(attributeName))
                    return attributeParts[1].Trim();
            }
            return null;
        }
    }
}
