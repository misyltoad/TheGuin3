using System;
using System.Collections.Generic;
using System.Text;

namespace TheGuin3.Interfaces.Base
{
    public abstract class Server
    {
        public abstract List<TextChannel> TextChannels { get; }
        public abstract List<User> Users { get; }

        public abstract void KickUser(User user);
        public abstract void BanUser(User user, int days);

        public abstract User Owner { get; }

        public Role FindRole(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
                return null;

            foreach (var role in Roles)
            {
                if (name == role.Id)
                    return role;

                if (name == role.Name)
                    return role;
            }

            foreach (var role in Roles)
            {
                if (name.ToLower() == role.Name.ToLower())
                    return role;
            }

            foreach (var role in Roles)
            {
                if (role.Name.IndexOf(name) != -1)
                    return role;
            }

            foreach (var role in Roles)
            {
                if (name.ToLower().IndexOf(role.Name.ToLower()) != -1)
                    return role;
            }

            return null;
        }

        public Role GetRoleById(string id)
        {
            if (String.IsNullOrWhiteSpace(id))
                return null;

            foreach (var role in Roles)
            {
                if (role.Id == id)
                    return role;
            }

            return null;
        }

        public TextChannel FindTextChannel(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
                return null;

            foreach (var role in TextChannels)
            {
                if (name == role.Id)
                    return role;

                if (name == role.Name)
                    return role;
            }

            foreach (var role in TextChannels)
            {
                if (name.ToLower() == role.Name.ToLower())
                    return role;
            }

            foreach (var role in TextChannels)
            {
                if (name.IndexOf(role.Name) != -1)
                    return role;
            }

            foreach (var role in TextChannels)
            {
                if (name.ToLower().IndexOf(role.Name.ToLower()) != -1)
                    return role;
            }

            return null;
        }

        public TextChannel GetTextChannelById(string id)
        {
            if (String.IsNullOrWhiteSpace(id))
                return null;

            foreach (var channel in TextChannels)
            {
                if (channel.Id == id)
                    return channel;
            }

            return null;
        }

        public User FindUser(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
                return null;

            // First pass of literal search tests.
            foreach (var user in Users)
            {
                if (name == user.Tag)
                    return user;

                if (name == user.HumanTag)
                    return user;

                if (name == user.Nickname)
                    return user;

                if (name == user.Username)
                    return user;
            }

            // First pass of lower search tests, removing mentions as this is not possible.
            foreach (var user in Users)
            {
                if (name.ToLower() == user.Nickname.ToLower())
                    return user;

                if (name.ToLower() == user.Username.ToLower())
                    return user;
            }


            // Second pass for literal search test, removing mentions for search
            foreach (var user in Users)
            {
                if (user.Nickname.IndexOf(name) != -1)
                    return user;

                if (user.Username.IndexOf(name) != -1)
                    return user;
            }

            // Second pass for lower search test, removing mentions for search
            foreach (var user in Users)
            {
                if (user.Nickname.ToLower().IndexOf(name.ToLower()) != -1)
                    return user;

                if (user.Username.ToLower().IndexOf(name.ToLower()) != -1)
                    return user;
            }

            return null;
        }

        public User GetUserById(string id)
        {
            if (String.IsNullOrWhiteSpace(id))
                return null;

            foreach (var user in Users)
            {
                if (user.Id == id)
                    return user;
            }

            return null;
        }
    }
}
