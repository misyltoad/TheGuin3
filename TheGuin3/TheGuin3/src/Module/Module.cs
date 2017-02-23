using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace TheGuin3.Module
{
    public class Module
    {
        public Module(string name)
        {
            Meta = new MetaData();
            Meta.Name = name;

            HookSystem = new DynamicSystem(this);
        }

        public Assembly Assembly => HookSystem.Assembly;

        public class MetaData
        {
            public string Name;
        }

        public MetaData Meta;
        private DynamicSystem HookSystem;
    }
}
