using ClientCore;
using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace UWPClient
{
    public sealed partial class MainPage : Page
    {
        ChatClient client;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void ConnectBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(UserName.Text))
            {
                var dialog = new ContentDialog()
                {
                    Title = "Warning",
                    Content = "Please, enter username!",
                    CloseButtonText = "Ok"
                };
                await dialog.ShowAsync();
            }

            client = new ChatClient(UserName.Text);
            client.ReceiveMessage = WriteMessage;
            client.StartClient();
            UserName.IsEnabled = false;
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            client.Say(Message.Text);
            Message.Text = "";
            Message.Focus(FocusState.Keyboard);
        }

        private void UserName_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
                ConnectBtn_Click(null, null);
        }

        private void Message_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
                Send_Click(null, null);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (client != null)
                client.Disconnect();
            base.OnNavigatedFrom(e);
        }

        public async void WriteMessage(string user, string message)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                string date = DateTime.Now.ToString();
                string result = date + " " + user + ": " + message + Environment.NewLine;
                
                Paragraph paragraph = new Paragraph();
                paragraph.FontSize = 20;
                Run run = new Run();                
                run.Text = result;
                paragraph.Inlines.Add(run);
                Chat.Blocks.Add(paragraph);
            });
        }
    }
}