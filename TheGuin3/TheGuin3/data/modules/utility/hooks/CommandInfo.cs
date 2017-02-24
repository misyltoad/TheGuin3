using System;
using System.Reflection;
using System.Drawing;
using TheGuin3.Interfaces;   
using TheGuin3.Interfaces.Discord;   

namespace TheGuin3.Modules.Core
{
    [OnCommand("info", "Get some server info and info about TheGuin")]
    public class InfoCommand : TheGuin3.Interfaces.Base.Command
    {
		public InfoCommand(Context context) : base (context) {}
		
		protected override void Execute()
		{
			Embed embed = new Embed();
			
			embed.Author.Name = String.Format("{0} | Info", Context.Server.Name);
			embed.Author.IconUrl = Context.Server.AvatarUrl;
			
			embed.Colour = Color.FromArgb(52, 152, 219);
			
			embed.ThumbnailUrl = Context.Server.AvatarUrl;
			
			Embed.Section section = new Embed.Section();
			section.Name = String.Format("Information about {0}: \n", Context.Server.Name);
			section.Value = "";
			
			section.Value += String.Format("Owner: {0}\n", Context.Server.Owner.DataString);
			
			int adminCount = 0;
			foreach (var user in Context.Server.Users)
			{
				if (user.IsAdmin)
					adminCount++;
			}
			
			section.Value += String.Format("Admins: {0}\n", adminCount);
			section.Value += String.Format("Members: {0}\n", Context.Server.Users.Count);
			
			section.IsInline = true;
			embed.Sections.Add(section);
			
			embed.Footer.IconUrl = "http://i.imgur.com/E48G7oi.png";
			embed.Footer.Text = "TheGuin says: \"I was made by Joshua Ashton!\"";
		
			Message message = new Message(embed);
			
			Context.Channel.SendMessage(message);
		}
    }
}
