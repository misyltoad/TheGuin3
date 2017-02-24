using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace TheGuin3.Interfaces.Base
{
    public abstract class User
    {
        public abstract string InterfaceName { get; }

        public abstract string Nickname { get; }
        public abstract string Username { get; }
        public abstract string Tag { get; }
        public abstract string HumanTag { get; }
        public abstract string Id { get; }
        public abstract string DataString { get; }
        public abstract string AvatarUrl { get; }
        public abstract Bitmap Avatar { get; }

        public abstract List<Role> Roles { get; }
        public abstract void GiveRole(Role role);

        public abstract void SendMessage(Message message);
        public bool IsServerOwner => Id == (Server?.Owner.Id ?? "");
        public abstract Server Server { get; }
        public abstract bool IsBotOwner { get; }

        public bool IsAdmin
        {
            get
            {
                if (IsServerOwner || IsBotOwner)
                    return true;

                foreach (var role in Roles)
                {
                    if (role.IsAdmin)
                        return true;
                }

                return false;
            }
        }
    }
}
