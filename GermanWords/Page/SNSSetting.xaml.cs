using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using GermanWordsClassLibrary.Helper;

using SinaBase;
using WeiboSdk;
using WeiboSdk.PageViews;

namespace GermanWords.Page
{
    public partial class SNSSetting : PhoneApplicationPage
    {
        bool isToggleSwitchEventIgnored;

        public SNSSetting()
        {
            InitializeComponent();
            isToggleSwitchEventIgnored = false;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (IsolatedStorageSettingsHelper.IsSettingExisted(IsolatedStorageSettingsHelper.SettingStrings.SINA_WEIBO_ACCESS_TOKEN)
                && IsolatedStorageSettingsHelper.IsSettingExisted(IsolatedStorageSettingsHelper.SettingStrings.SINA_WEIBO_REFLESH_TOKEN))
            {
                isToggleSwitchEventIgnored = true;
                ToggleSwitch_SinaWeibo.IsChecked = true;
                Button_Share.IsEnabled = true;
            }
            else
            {
                ToggleSwitch_SinaWeibo.IsChecked = false;
                Button_Share.IsEnabled = false;
            }

            base.OnNavigatedTo(e);
        }

        private void ToggleSwitch_SinaWeibo_Checked(object sender, RoutedEventArgs e)
        {
            if (isToggleSwitchEventIgnored)
            {
                isToggleSwitchEventIgnored = false;
                return;
            }

            AuthenticationView.OAuth2VerifyCompleted = (e1, e2, e3) => VerifyBack(e1, e2, e3);

            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri("/WeiboSdk;component/PageViews/AuthenticationView.xaml", UriKind.Relative));
            });
        }

        private void ToggleSwitch_SinaWeibo_Unchecked(object sender, RoutedEventArgs e)
        {
            IsolatedStorageSettingsHelper.DeleteSetting(IsolatedStorageSettingsHelper.SettingStrings.SINA_WEIBO_ACCESS_TOKEN);
            IsolatedStorageSettingsHelper.DeleteSetting(IsolatedStorageSettingsHelper.SettingStrings.SINA_WEIBO_REFLESH_TOKEN);
        }

        private void Button_Share_Click(object sender, RoutedEventArgs e)
        {
            SdkShare sdkShare = new SdkShare
            {
                AccessToken = IsolatedStorageSettingsHelper.GetSinaWeiboAccessToken(),
                Message = "#德语词卡#我正在使用“德语词卡 for WindowsPhone”，德语学习者的背单词神器！你也来试试吧，轻戳这里：http://www.windowsphone.com/s?appid=9b3ba3fc-c792-48fe-a349-47a12468ab62"
            };
            sdkShare.Completed = new EventHandler<SendCompletedEventArgs>(ShareCompleted);

            sdkShare.Show();
        }

        private void ShareCompleted(object sender, SendCompletedEventArgs e)
        {
            if (e.IsSendSuccess == false)
            {
                if (e.ErrorCode == SdkErrCode.NET_UNUSUAL)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show("当前网络不通畅，请稍后再试。");
                    });
                }
                else if (e.ErrorCode == SdkErrCode.SERVER_ERR)
                {
                    IsolatedStorageSettingsHelper.DeleteSetting(IsolatedStorageSettingsHelper.SettingStrings.SINA_WEIBO_ACCESS_TOKEN);
                    IsolatedStorageSettingsHelper.DeleteSetting(IsolatedStorageSettingsHelper.SettingStrings.SINA_WEIBO_REFLESH_TOKEN);

                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show("授权已失效，请重新链接到新浪微博。");
                    });
                }
                else
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show("当前服务不可用，请稍后再试。");
                    });
                }
            }
        }

        private void VerifyBack(bool isSucess, SdkAuthError errCode, SdkAuth2Res response)
        {
            if (errCode.errCode == SdkErrCode.SUCCESS)
            {
                if (null != response)
                {
                    IsolatedStorageSettingsHelper.SetSinaWeiboAccessToken(response.accesssToken);
                    IsolatedStorageSettingsHelper.SetSinaWeiboRefleshToken(response.refleshToken);
                }

                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.GoBack();
                });
            }
            else if (errCode.errCode == SdkErrCode.NET_UNUSUAL)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show("当前网络不通畅，请稍后再试。");
                });
            }
            else if (errCode.errCode == SdkErrCode.SERVER_ERR)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show("当前服务不可用，请稍后再试。");
                });
            }
        }
    }
}