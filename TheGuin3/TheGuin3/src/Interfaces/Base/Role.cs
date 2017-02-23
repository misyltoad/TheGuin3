using System;
using System.Collections.Generic;
using System.Text;

namespace TheGuin3.Interfaces.Base
{
    public abstract class Role
    {
        public abstract string InterfaceName { get; }

        public abstract string Id { get; }
        public abstract string Name { get; }
        public abstract bool IsAdmin { get; }
    }
}
