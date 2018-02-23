using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Diagnostics;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;

namespace app_wpf
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private AppServiceConnection _appServiceConnection;

        private async Task<bool> ConnectAsync()
        {
            if (_appServiceConnection != null)
            {
                return true;
            }

            var appServiceConnection = new AppServiceConnection
            {
                AppServiceName = "CommunicationService",
                PackageFamilyName = Package.Current.Id.FamilyName
            };
            appServiceConnection.RequestReceived += AppServiceConnection_RequestReceived;
            var r = await appServiceConnection.OpenAsync() == AppServiceConnectionStatus.Success;
            if (r)
            {
                _appServiceConnection = appServiceConnection;
            }

            return r;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!(await ConnectAsync()))
            {
                MessageBox.Show($"Failed");
                return;
            }

            var res = await _appServiceConnection.SendMessageAsync(new ValueSet
            {
                ["Input"] = inputTextBox.Text,
            });

            logTextBlock.Text = res.Message["Result"] as string;
        }

        private void AppServiceConnection_RequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            void setText()
            {
                textBlock.Text = (string)args.Request.Message["Input"];
                logTextBlock.Text = (string)args.Request.Message["Now"];
            }

            if (Dispatcher.CheckAccess())
            {
                setText();
            }
            else
            {
                Dispatcher.Invoke(() => setText());
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await ConnectAsync();
        }
    }
}
