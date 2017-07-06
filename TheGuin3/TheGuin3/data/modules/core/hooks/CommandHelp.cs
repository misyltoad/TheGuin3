using System;
using System.Reflection;
using System.Drawing;
using TheGuin3.Interfaces;   
using TheGuin3.Interfaces.Discord;   

namespace TheGuin3.Modules.Core
{
    [OnCommand("help", "Get some help with Summer")]
    public class HelpCommand : TheGuin3.Interfaces.Base.Command
    {
		public HelpCommand(Context context) : base (context) {}
		
		public override void Execute()
		{
			string output = "";
			
			var commandTypes = Context.Client.GetAllTypesWithAttributeInAvailableModules<OnCommand>(Context.Server);
			foreach (var type in commandTypes)
			{
				OnCommand[] attributes = (OnCommand[])type.GetTypeInfo().GetCustomAttributes(typeof(OnCommand), true);
				foreach (var attribute in attributes)
				{
					if (!attribute.AdminOnly || (attribute.AdminOnly && Context.User.IsAdmin))
						output += String.Format("• {0}{1} - {2}\n", Config.Schema.BotConfig.Get().CommandPrefix, attribute.Name, attribute.Description);
				}
			}
			
			Embed embed = new Embed();
			
			embed.Footer.IconUrl = "http://i.imgur.com/iXoxFgq.png";
			embed.Footer.Text =  "Summer says: \"Want me on your server? Click my name at the top of this box!\"";
			
			embed.Author.Name = "Summer";
			embed.Author.IconUrl = "http://i.imgur.com/iXoxFgq.png";
			embed.Author.HyperlinkUrl = "https://discordapp.com/oauth2/authorize?client_id=330740477542400002&scope=bot&permissions=0";
			
			embed.Colour = Color.FromArgb(52, 152, 219);
			
			embed.HyperlinkUrl = "https://discordapp.com/oauth2/authorize?client_id=330740477542400002&scope=bot&permissions=0";
			embed.ThumbnailUrl = "http://i.imgur.com/iXoxFgq.png";
			
			Embed.Section section = new Embed.Section();
			section.Name = String.Format("Commands for you in {0}: \n", Context.Server.Name);
			section.Value = output;
			section.IsInline = false;
			embed.Sections.Add(section);
		
			
			Message message = new Message(embed);
			
			Context.Channel.SendMessage(message);
		}
    }
}
