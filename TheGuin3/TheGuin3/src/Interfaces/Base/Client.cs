using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TheGuin3.Interfaces.Base
{
    public abstract class Client
    {
        public abstract string InterfaceName { get; }

        public void Start()
        {
            Init();
        }

        public abstract void Stop();

        public abstract void Init();

        public void OnPublicMessageRecieved(User user, TextChannel channel, Server server, string content)
        {

        }

        public void OnPrivateMessageRecieved(User user, string content)
        {

        }
    }
}
