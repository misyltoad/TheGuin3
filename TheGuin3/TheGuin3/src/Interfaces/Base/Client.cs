using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TheGuin3.Interfaces.Base
{
    public abstract class Client
    {
        public Client()
        {
            ModuleRegistry = new Module.ModuleRegistry();
        }

        public abstract string InterfaceName { get; }

        public void Start()
        {
            Init();
        }

        public abstract void Stop();

        public abstract void Init();

        public void OnPublicMessageRecieved(User user, TextChannel channel, Server server, string content)
        {
            Command.Context context = new Command.Context(user, channel, server, content);
            
            if (context.IsCommand)
            {
                return;
            }
        }

        Module.ModuleRegistry ModuleRegistry;

        public void OnPrivateMessageRecieved(User user, string content)
        {
        }
    }
}
