using Client;
using System;
using System.Windows.Forms;

namespace WFClient
{
    public partial class MainForm : Form
    {
        ChatClient client;

        public MainForm()
        {
            InitializeComponent();
        }

        private void username_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                connect_Click(null, null);
        }

        private void message_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                send_Click(null, null);
        }

        private void connect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(username.Text))
            {
                MessageBox.Show("Please, enter username!");
                return;
            }

            client = new ChatClient(username.Text);
            client.ReceiveMessage = WriteMessage;
            client.StartClient();
            username.Enabled = false;
        }

        private void send_Click(object sender, EventArgs e)
        {
            client.Say(message.Text);
            message.Text = "";
            message.Focus();
        }

        public void WriteMessage(string user, string message)
        {
            MethodInvoker invokerDelegate = delegate ()
            {
                string date = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ":" + DateTime.Now.Second;
                string result = date + " " + user + ": " + message + Environment.NewLine;
                chat.Text += result;
            };

            if (InvokeRequired)
                Invoke(invokerDelegate);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (client != null)
                client.Disconnect();
        }
    }
}