using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace haret7.Telnet
{
    public class TelnetMessageReceivedFromServer : EventArgs
    {
        public string Message { get; private set; }

        public TelnetMessageReceivedFromServer(string message)
        {
            Message = message;
        }

    }
}
