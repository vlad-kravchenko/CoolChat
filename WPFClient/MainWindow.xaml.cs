using Client;
using System;
using System.Windows;
using System.Windows.Input;

namespace WPFClient
{
    public partial class MainWindow : Window
    {
        ChatClient client;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(UserName.Text))
            {
                MessageBox.Show("Please, enter username!");
                return;
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
            Message.Focus();
        }

        private void UserName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Connect_Click(null, null);
        }

        private void Message_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Send_Click(null, null);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (client != null)
                client.Disconnect();
        }

        public void WriteMessage(string user, string message)
        {
            Dispatcher.BeginInvoke((Action)(() =>
            {
                string date = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ":" + DateTime.Now.Second;
                string result = date + " " + user + ": " + message + Environment.NewLine;
                Chat.AppendText(result);
            }));
        }
    }
}