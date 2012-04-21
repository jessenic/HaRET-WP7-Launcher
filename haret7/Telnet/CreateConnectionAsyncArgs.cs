using System;

namespace haret7.Telnet
{
    public class CreateConnectionAsyncArgs : EventArgs
    {
        public bool ConnectionOk { get; private set; }

        public CreateConnectionAsyncArgs(bool connectionOk)
        {
            ConnectionOk = connectionOk;
        }
    }
}