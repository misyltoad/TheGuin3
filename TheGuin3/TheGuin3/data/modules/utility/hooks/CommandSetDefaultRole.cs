using System;
using TheGuin3;
using TheGuin3.Interfaces;

namespace TheGuin3.Modules.Core
{
	public class DefaultRoleConfigSchema
	{
		public string DefaultRole { get; set; }
	}
	
	public class DefaultDefaultRoleConfig : Config.DefaultConfig
	{
		public DefaultDefaultRoleConfig()
		{
			DefaultContent = "{\n'DefaultRole': '@everyone'\n}";
		}
	}
	
	public class DefaultRoleConfig : Config.ConfigInterface<DefaultRoleConfigSchema, DefaultDefaultRoleConfig>
	{
	}
	
    [OnCommand("setdefaultrole", "Set the default role for new members on the server!", true)]
    class SetDefaultRoleCommand : TheGuin3.Interfaces.Base.Command
    {
        public SetDefaultRoleCommand(Context context) : base (context)
		{}
		
		protected override void Execute()
        {
			if (Context.Channel != null)
				Context.Channel.SendMessage("Pong!");
		}
    }
}
