using System;
using TheGuin3;
using TheGuin3.Interfaces;

namespace TheGuin3.Modules.Core
{
    [OnCommand("ping", "Pings back!")]
    class PingCommand : TheGuin3.Interfaces.Base.Command
    {
        public PingCommand(Context context) : base (context)
		{}
		
		public override void Execute()
        {
			if (Context.Channel != null)
				Context.Channel.SendMessage("Pong!");
		}
    }
}
