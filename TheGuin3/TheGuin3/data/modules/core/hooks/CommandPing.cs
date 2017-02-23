using System;
using TheGuin3;
using TheGuin3.Interfaces;

namespace TheGuin3.Modules.Core
{
    [OnCommand("ping", "Pings back!")]
    class PingCommand : TheGuin3.Interfaces.Base.Command
    {
        public PingCommand(TheGuin3.Interfaces.Base.Command.Context Context)
        {
			if (Context.Channel != null)
				Context.Channel.SendMessage("Ping!");
		}
    }
}
