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
            Context context = new Context(user, channel, server, content, this);
            
            if (context.IsCommand)
            {
                ExecuteHook<OnCommand>(server, context);
            }
        }

        public List<Type> GetAllTypesWithAttributeInAvailableModules<T>(Server server) where T : System.Attribute
        {
            List<Type> types = new List<Type>();
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
                            T[] attributes = (T[])type.GetTypeInfo().GetCustomAttributes(typeof(T), true);
                            if (attributes.Length > 0)
                                types.Add(type);
                        }
                    }
                }
            }

            return types;
        }

        private void ExecuteHook<T>(Server server, params object[] arguments) where T : System.Attribute
        {
            foreach (var type in GetAllTypesWithAttributeInAvailableModules<T>(server))
            {
                Activator.CreateInstance(type, arguments);
            }
        }

        public Module.ModuleRegistry ModuleRegistry;

        public void OnPrivateMessageRecieved(User user, string content)
        {
            /*Context context = new Context(user, , content);

            if (context.IsCommand)
            {
                ExecuteHook(typeof(OnCommand), server, context);
            }*/
        }
    }
}
