using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using GermanWords.DeutschWelle.ViewModels;
using Microsoft.Phone.Net.NetworkInformation;
using GermanWords.DeutschWelle.WP7Http;

namespace GermanWords.DeutschWelle
{
    public class HttpGet
    {
        public delegate void htmlCallbackForString(string response);
        public htmlCallbackForString htmlcallbackforstring { get; set; }
        public void GetHtml(string url)
        {
            try
            {
                WP7HttpRequest request = new WP7HttpRequest();
                request.httpCompleted += (s, e) =>
                    {
                        htmlcallbackforstring(e.result);
                    };
                request.requestUrl = url;
                request.requestMethod = requestType.GET;
                request.request();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
