using GermanWords.DeutschWelle.ViewModels;
using HtmlAgilityPack;
using Microsoft.Phone.Net.NetworkInformation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GermanWords.DeutschWelle
{
    class AgilityData
    {
        HttpGet httpget = new HttpGet();
        SpeedMode speedmode = SpeedMode.TWOMODE;
        public void GetTitle()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                App.DWModel.Visibility = System.Windows.Visibility.Visible;
                httpget.GetHtml("http://de.hujiang.com/new/tingli/page" + App.PageCount + "/");
                //GetHtml("http://de.hujiang.com/new/tingli");
                httpget.htmlcallbackforstring = GetTitleDataCallback;
                //return agilityTitle(html);
            }
            else
            {
                throw (new Exception("no network!"));
            }
        }

        private void GetTitleDataCallback(string response)
        {
            App.DWModel.DWtileList = agilityTitle(response);
            App.DWModel.Visibility = System.Windows.Visibility.Collapsed;
        }

        public void GetArtical(string url,SpeedMode speedmode)
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                this.speedmode = speedmode;
                httpget.GetHtml(url);
                httpget.htmlcallbackforstring = GetArticalDataCallback;
            }
            else
            {
                throw (new Exception("no network!"));
            }
        }

        private void GetArticalDataCallback(string response)
        {
            DWArticalModel dwartical = agilityArtical(response);
            App.DWModel.DWArtical.Artical = dwartical.Artical;
            App.DWModel.DWArtical.NormalPath = dwartical.NormalPath;
            App.DWModel.DWArtical.SlowPath = dwartical.SlowPath;
            App.DWModel.DWArtical.Visibility = System.Windows.Visibility.Collapsed;
            App.DWModel.DWArtical.IsCanPlay = true;
            App.DWModel.DWArtical.PlayImage = "/images/play.png";
            App.DWModel.DWArtical.SpeedImage = "/images/slow.png";
        }

        private string getmap3Path(string docContext, string name)
        {
            string mp3mark = name + "：HJPlayer.init(\"hjptype=song&player=1&son=http://f1.w.hjfile.cn/doc/";
            string httpmark = "http://f1.w.hjfile.cn/doc/";
            int mp3marklen = mp3mark.Length;
            int httpmarklen = httpmark.Length;
            //HtmlNode mp3Node = docContext.DocumentNode.SelectSingleNode(node);
            string mp3Str = docContext;
            int mp3StrStart = mp3Str.IndexOf(mp3mark) + mp3marklen - httpmarklen;
            int mp3StrEnd = mp3Str.IndexOf("mp3",mp3StrStart) + 2;
            mp3Str = mp3Str.Substring(mp3StrStart, mp3StrEnd - mp3StrStart + 1);
            return mp3Str;
        }

        private DWArticalModel agilityArtical(string html)
        {
            if (speedmode == SpeedMode.JUSTSLOW)
            {
                string mackstr = "<span id=\"hjcang_container\" ></span>";
                html.Insert(html.IndexOf(mackstr) + mackstr.Length, "</span>");
            }
            HtmlDocument doc = new HtmlDocument();
            DWArticalModel dwartical = new DWArticalModel();
            doc.LoadHtml(html);
            try
            {
                int index = 6;
                string currentArtical = "";
                string ArticalStr = "";
                string XPath = "./html[1]/body[1]/div[3]/div[5]/div[1]/div[1]/div[1]/div[2]";
                HtmlDocument docArticalContext = ReadNodes(XPath, doc);
                do
                {
                    HtmlNode articalNode = docArticalContext.DocumentNode.SelectSingleNode("./p[" + index + "]");
                    currentArtical = articalNode.InnerText;
                    ArticalStr += currentArtical;
                    index++;
                } while (!currentArtical.Contains("欢迎收听更多德国之声慢速新闻"));
                //ArticalStr = ArticalStr.Replace("\r\n", "");
                ArticalStr = ArticalStr.Replace("欢迎收听更多德国之声慢速新闻&gt;&gt;&gt;", "");
                ArticalStr = ArticalStr.Replace("\t", "");
                ArticalStr = ArticalStr.Replace("&nbsp;", "");
                ArticalStr = ArticalStr.Replace("&uuml;", "ü");
                ArticalStr = ArticalStr.Replace("&ouml;", "ö");
                ArticalStr = ArticalStr.Replace("&auml;", "ä");
                ArticalStr = ArticalStr.Replace("&Auml;", "Ä");
                ArticalStr = ArticalStr.Replace("&Uuml;", "Ü");
                ArticalStr = ArticalStr.Replace("&Ouml;", "Ö");
                ArticalStr = ArticalStr.Replace("&szlig;", "ß");
                ArticalStr = ArticalStr.Replace("&quot;", "\"");
                ArticalStr = ArticalStr.Replace("&oacute;", "ó");
                ArticalStr = ArticalStr.Replace("&ntilde;", "ñ");
                dwartical.Artical = ArticalStr;
                //saveArtical(dwartical);
                string contextStr = docArticalContext.DocumentNode.InnerText;
                if (contextStr.Contains("慢速版"))
                {
                    dwartical.SlowPath = getmap3Path(contextStr, "慢速版");
                }
                if (contextStr.Contains("常速版"))
                {
                    dwartical.NormalPath = getmap3Path(contextStr, "常速版");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return dwartical;
        }

        private void saveArtical(DWArticalModel svdwartical)
        {
            string aticalname = svdwartical.Title.Replace(".", "").Replace(":", "") + ".atc";

        }

        ObservableCollection<DWListItemModel> dwlist = new ObservableCollection<DWListItemModel>();
        private ObservableCollection<DWListItemModel> agilityTitle(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            try
            {
                for (int i = 1; i < 22; i++)
                {
                    DWListItemModel listItem = new DWListItemModel();
                    string XPath = @"./html[1]/body[1]/div[3]/div[3]/div[2]/div[3]/div[1]/ul[1]/li[" + i + "]/div[1]";
                    HtmlDocument docTitleContext = ReadNodes(XPath, doc);
                    HtmlNode titleNode = docTitleContext.DocumentNode.SelectSingleNode("./h2[1]/a[2]");
                    if (titleNode.InnerText.Contains("德国之声新闻："))
                    {
                        listItem.SpeedMode = SpeedMode.TWOMODE;
                        listItem.UrlPath = "http://de.hujiang.com" + titleNode.Attributes["href"].Value;
                        listItem.Title = titleNode.InnerText.Replace("德国之声新闻：","");
                        HtmlNode abstructNode = docTitleContext.DocumentNode.SelectSingleNode("./div[1]/div[1]");
                        listItem.Abstruct = abstructNode.InnerText.Replace("\r\n", "");
                        dwlist.Add(listItem);
                    }
                    else if (titleNode.InnerText.Contains("德国之声慢速新闻："))
                    {
                        listItem.SpeedMode = SpeedMode.JUSTSLOW;
                        listItem.UrlPath = "http://de.hujiang.com" + titleNode.Attributes["href"].Value;
                        listItem.Title = titleNode.InnerText.Replace("德国之声慢速新闻：", "");
                        HtmlNode abstructNode = docTitleContext.DocumentNode.SelectSingleNode("./div[1]/div[1]");
                        listItem.Abstruct = abstructNode.InnerText.Replace("\r\n", "");
                        dwlist.Add(listItem);
                    }
                }
                if (dwlist.Count < 20)
                {
                    App.PageCount++;
                    GetTitle();
                }
                //GetArtical();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return dwlist;
        }

        HtmlDocument ReadNodes(string path, HtmlDocument doc)
        {
            HtmlDocument docStockNode = new HtmlDocument();
            string Node = doc.DocumentNode.SelectSingleNode(path).InnerHtml;
            docStockNode.LoadHtml(Node);
            return docStockNode;
        }
    }
}
