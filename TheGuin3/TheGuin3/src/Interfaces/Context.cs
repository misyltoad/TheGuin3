using System;
using System.Collections.Generic;
using System.Text;
using TheGuin3.Interfaces.Base;

namespace TheGuin3.Interfaces
{
    public class Context
    {
        public Context(User user, TextChannel channel, Server server, Message message, Client client)
        {
            Message = message;
            MessageString = message;
            Client = client;

            string prefix = Config.Schema.BotConfig.Get().CommandPrefix;

            if (MessageString.Length >= prefix.Length && MessageString.Substring(0, prefix.Length) == prefix)
            {
                IsCommand = true;
                int indexOfFirstSpace = MessageString.IndexOf(' ');

                if (indexOfFirstSpace != -1 && indexOfFirstSpace + 1 < MessageString.Length)
                    ArgsString = MessageString.Substring(MessageString.IndexOf(' ') + 1);

                int commandPrefixEnd = indexOfFirstSpace;

                if (commandPrefixEnd == -1)
                    commandPrefixEnd = MessageString.Length;

                CommandName = MessageString.Substring(prefix.Length, commandPrefixEnd - prefix.Length);

                if (ArgsString != null)
                    Args = new List<string>(ArgsString.Split(' '));
                else
                {
                    Args = new List<string>();
                    ArgsString = "";
                }

                for (int i = Args.Count; i >= 0; i--)
                {
                    if (server != null)
                    {
                        List<string> argsList = new List<string>(Args);
                        argsList.RemoveRange(i, argsList.Count - i);
                        ArgsUser = server.FindUser(String.Join(" ", argsList));

                        if (ArgsUser != null)
                        {
                            List<string> remainingArgsList = new List<string>(Args);
                            remainingArgsList.RemoveRange(0, i);
                            ArgsUserArgs = remainingArgsList;
                            ArgsUserArgsString = String.Join(" ", remainingArgsList);
                            break;
                        }
                    }
                }

                if (ArgsUser == null)
                {
                    ArgsUser = user;
                    ArgsUserArgs = new List<string>();
                }
            }
            else
            {
                CommandName = "";
                Args = new List<string>();
                ArgsString = "";
                ArgsUser = user;
                ArgsUserArgs = new List<string>();
                ArgsUserArgsString = "";
                IsCommand = false;
            }

            Channel = channel;
            Server = server;
            User = user;
        }

        public Client Client;
        public User User;
        public TextChannel Channel;
        public Server Server;
        public string MessageString;
        public Message Message;

        public bool IsCommand;
        public string CommandName;

        public List<string> Args;
        public string ArgsString;
        public User ArgsUser;
        public List<string> ArgsUserArgs;
        public string ArgsUserArgsString;
    };
}
