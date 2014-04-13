using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using GermanWords.Resource;
using GermanWords.DeutschWelle.ViewModels;
using System.Windows.Media;
using System.IO.IsolatedStorage;
using System.IO;

namespace GermanWords.DeutschWelle
{
    public partial class DWTileListView : PhoneApplicationPage
    {
        ApplicationBarIconButton morepageBtn;
        AgilityData agilitydata = new AgilityData();
        // 构造函数
        public DWTileListView()
        {
            InitializeComponent();
            // 用于本地化 ApplicationBar 的示例代码
            BuildLocalizedApplicationBar();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                if (DataContext == null)
                {
                    agilitydata.GetTitle();
                    DataContext = App.DWModel;
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButton.OK);
            }

        }

        private void MainLongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 如果所选项为空(没有选定内容)，则不执行任何操作
            if (MainLongListSelector.SelectedItem == null)
                return;

            // 导航到新页面
            NavigationService.Navigate(new Uri("/DeutschWelle/ArticalPage.xaml?selectedItem=" + (MainLongListSelector.SelectedItem as DWListItemModel).Title, UriKind.Relative));

            // 将所选项重置为 null (没有选定内容)
            MainLongListSelector.SelectedItem = null;
        }

        private void nextpageBtn_Click(object sender, EventArgs e)
        {
            if (App.PageCount < 70)
            {
                App.PageCount++;
                agilitydata.GetTitle();
            }
            if (App.PageCount > 69)
            {
                morepageBtn.IsEnabled = false;
            }
        }
        // 用于生成本地化 ApplicationBar 的示例代码
        private void BuildLocalizedApplicationBar()
        {
            // 将页面的 ApplicationBar 设置为 ApplicationBar 的新实例。
            ApplicationBar = new ApplicationBar();
            //ApplicationBar.BackgroundColor = Color.FromArgb(255,30,167,245);
            ApplicationBar.Opacity = 0.7;

            morepageBtn = new ApplicationBarIconButton(new Uri("/Assets/AppBar/sync.png", UriKind.Relative));
            morepageBtn.Text = AppResources.MorePage;
            ApplicationBar.Buttons.Add(morepageBtn);
            morepageBtn.Click += nextpageBtn_Click;

            ApplicationBarMenuItem removeBtn = new ApplicationBarMenuItem(AppResources.RemoveFiles);
            ApplicationBar.MenuItems.Add(removeBtn);
            removeBtn.Click += removeBtn_Click;
        }

        void removeBtn_Click(object sender, EventArgs e)
        {
            try
            {
                using (var isofile = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    string pattern = "*.mp3";
                    string[] filenames = isofile.GetFileNames(pattern);
                    foreach (var filename in filenames)
                    {
                        isofile.DeleteFile(filename);
                    }
                    MessageBox.Show("清除缓存成功！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}