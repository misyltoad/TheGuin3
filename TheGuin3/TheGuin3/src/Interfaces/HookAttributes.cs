using System;
using System.Collections.Generic;
using System.Text;

namespace TheGuin3.Interfaces
{
    [AttributeUsage(AttributeTargets.All)]
    public class OnCommand : System.Attribute
    {
        public OnCommand(string name, string description, bool adminOnly)
        {
            Name = name;
            Description = description;
            AdminOnly = adminOnly;
        }
        public string Name;
        public string Description;
        public bool AdminOnly;
    }

    [AttributeUsage(AttributeTargets.All)]
    public class OnMessage : System.Attribute
    { }

    [AttributeUsage(AttributeTargets.All)]
    public class OnUserUnbanned : System.Attribute
    { }

    [AttributeUsage(AttributeTargets.All)]
    public class OnUserBanned : System.Attribute
    { }

    [AttributeUsage(AttributeTargets.All)]
    public class OnUserLeft : System.Attribute
    { }

    [AttributeUsage(AttributeTargets.All)]
    public class OnUserJoined : System.Attribute
    { }

    [AttributeUsage(AttributeTargets.All)]
    public class OnUserChange : System.Attribute
    { }
}
