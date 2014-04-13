using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Windows.UI.Input;

namespace GermanWords.DeutschWelle
{
    public partial class ArticalPage : PhoneApplicationPage
    {
        public ArticalPage()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            AgilityData agilitydata = new AgilityData();
            
            if (DataContext == null)
            {
                string selectedIndex = "";
                if (NavigationContext.QueryString.TryGetValue("selectedItem", out selectedIndex))
                {
                    //int index = int.Parse(selectedIndex);
                    foreach (var temp in App.DWModel.DWtileList)
                    {
                        if (temp.Title == selectedIndex)
                        {
                            DataContext = App.DWModel.DWArtical;
                            App.DWModel.DWArtical.Title = temp.Title;
                            App.DWModel.DWArtical.Abstruct = temp.Abstruct;
                            voicepalyer.DataContext = App.DWModel.DWArtical;
                            try
                            {
                                App.DWModel.DWArtical.Artical = "";
                                App.DWModel.DWArtical.NormalPath = "";
                                App.DWModel.DWArtical.SlowPath = "";
                                App.DWModel.DWArtical.StatePlay = StatePlay.NOPLAY;
                                App.DWModel.DWArtical.IsCanPlay = false;
                                App.DWModel.DWArtical.ProgressValue = 0;
                                App.DWModel.DWArtical.Visibility = System.Windows.Visibility.Visible;
                                App.DWModel.DWArtical.PlayImage = "/images/play.png";
                                App.DWModel.DWArtical.SpeedImage = "/images/slow.png";
                                agilitydata.GetArtical(temp.UrlPath,temp.SpeedMode);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "提示", MessageBoxButton.OK);
                            }
                            break;
                        }
                    }
                }

            }
        }
    }
}