using System;
using TheGuin3;
using TheGuin3.Interfaces;

namespace TheGuin3.Modules.Fun
{
    [OnCommand("jeff", "Tells your your name.")]
    class JeffCommand : TheGuin3.Interfaces.Base.Command
    {
        public JeffCommand(Context context) : base (context)
		{}
		
		public override void Execute()
        {
			if (Context.Channel != null)
				Context.Channel.SendMessage("Yes, that is your name, Jeff.");
		}
    }
}
