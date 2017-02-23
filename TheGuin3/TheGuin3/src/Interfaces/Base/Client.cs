using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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
                ExecuteHook(typeof(OnCommand), server, context);
                return;
            }
        }

        private void ExecuteHook(Type hook, Server server, params object[] arguments)
        {
            var availableModules = Config.Schema.ModuleListConfig.Get(server).Modules;

            foreach (var module in ModuleRegistry.Modules)
            {
                bool isValidModule = false;
                foreach (var availableModuleName in availableModules)
                {
                    if (availableModuleName == module.Meta.Name)
                        isValidModule = true;
                }

                if (isValidModule)
                {
                    if (module.Assembly != null)
                    {
                        foreach (var type in module.Assembly.GetTypes())
                        {
                            object[] attributes = (object[])type.GetTypeInfo().GetCustomAttributes(hook, true);
                            if (attributes.Length > 0)
                                Activator.CreateInstance(type, arguments);
                        }
                    }
                }
            }
        }

        Module.ModuleRegistry ModuleRegistry;

        public void OnPrivateMessageRecieved(User user, string content)
        {
        }
    }
}
