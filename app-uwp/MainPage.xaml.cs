using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.Core;
using Windows.UI.ViewManagement;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x411 を参照してください

namespace app_uwp
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            Current = this;

            float newWidth = 525;//ウィンドウサイズ変更
            float newHeight = 350;
            ApplicationView.GetForCurrentView().TryResizeView(new Size { Width = newWidth, Height = newHeight });
        }

        public static MainPage Current { get; private set; }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync(); //WPFアプリ起動
        }

        public async Task SetTextAsync(string text)
        {
            void setText()
            {
                textBlock.Text = text;
            }

            if (Dispatcher.HasThreadAccess)
            {
                setText();
            }
            else
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    setText();
                });
            }
        }

        private async void Button_Click2(object sender, RoutedEventArgs e)
        {
            await App.Current.SendNowAsync();
        }
    }
}
