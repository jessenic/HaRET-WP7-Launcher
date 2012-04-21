using System;
using System.Windows;
using Microsoft.Phone.Controls;
using haret7.Telnet;

namespace haret7
{
    public partial class TelnetPage : PhoneApplicationPage
    {
        private readonly TelnetClient client;
        public TelnetPage()
        {
            InitializeComponent();
            AddLine("Connecting...");
            this.client = new TelnetClient();
            this.client.CreateConnectionCompleted += OnCreateConnectionCompleted;

            this.client.TelnetMessageReceivedFromServer += OnTelnetMessageReceivedFromServer;
            try
            {
                var serverAddress = "localhost";
                var port = 9999;

                this.client.CreateConnection(serverAddress, port);
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid server or port.");
            }
        }

        private void OnCreateConnectionCompleted(object sender, CreateConnectionAsyncArgs e)
        {
            if (!e.ConnectionOk)
            {
                AddLine("Connection failed");
                return;
            }

            AddLine("Connection OK");
        }

        private void AddLine(string newLine)
        {
            Dispatcher.BeginInvoke(() => this.telnetBox.Text += newLine + "\n");
        }
        private void OnTelnetMessageReceivedFromServer(object sender, TelnetMessageReceivedFromServer e)
        {
            AddLine(e.Message);
        }

        private void sendBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                client.SendToServer(sendBox.Text);
                sendBox.Text = "";
            }
        }
    }
}