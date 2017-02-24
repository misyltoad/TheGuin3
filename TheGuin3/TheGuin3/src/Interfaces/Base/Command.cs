using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace TheGuin3.Interfaces.Base
{
    public abstract class Command
    {
        public Command(Context context)
        {
            Context = context;

            OnCommand[] attributes = (OnCommand[]) (GetType().GetTypeInfo().GetCustomAttributes(typeof(OnCommand), true));
            bool canContinue = false;
            foreach (var attribute in attributes)
            {
                if (attribute.Name.ToLower() == Context.CommandName.ToLower())
                {
                    if ((!attribute.AdminOnly || (attribute.AdminOnly && Context.User.IsAdmin)))
                        canContinue = true;
                    else
                        Context.Channel.SendMessage(String.Format("You can't do that, {0}!", Context.User.Nickname));
                }
            }

            try
            {
                if (canContinue)
                    Execute();
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        protected Context Context;

        protected abstract void Execute();
    }
}
