using System;
using System.Reflection;
using System.Drawing;
using System.Linq;
using TheGuin3.Interfaces;   
using TheGuin3.Interfaces.Discord;   

namespace TheGuin3.Modules.Core
{
	public abstract class BaseLog
	{
		public abstract string Title { get; } 
		public abstract Color Colour { get; }
		
		public BaseLog(Server server, User user)
		{
			var logChannel = server.FindTextChannel("theguin-log");
			if (logChannel == null)
				return;
			
			Embed embed = new Embed();
			
			embed.Author.Name = user.Nickname;
			embed.Author.IconUrl = user.AvatarUrl;
			embed.Colour = Colour;
			embed.ThumbnailUrl = user.AvatarUrl;
			
			Embed.Section section = new Embed.Section();
			section.Name = Title;
			section.Value = user.DataString;
			section.IsInline = false;
			embed.Sections.Add(section);
			
			embed.Footer.IconUrl = user.AvatarUrl;
			embed.Footer.Text = String.Format("Time: {0}", DateTime.Now.ToString("h:mm:ss tt"));
			
			Message message = new Message(embed);
			
			logChannel.SendMessage(message);
		}
	}
	
    [OnUserJoined]
    class JoinLog : BaseLog
    {
		public override string Title => "User Joined!";
		public override Color Colour => Color.FromArgb(46,204,113);
		
        public JoinLog(Server server, User user) : base(server,user)
		{}
    }
	
	[OnUserLeft]
	class LeaveLog : BaseLog
	{
		public override string Title => "User Left!";
		public override Color Colour => Color.FromArgb(231,76,60);
		
		public LeaveLog(Server server, User user) : base(server, user)
		{}
	}
	
	[OnUserBanned]
	class BanLog : BaseLog
	{
		public override string Title => "User Banned!";
		public override Color Colour => Color.FromArgb(0,0,0);
		
		public BanLog(Server server, User user) : base (server, user)
		{}
	}
	
	[OnUserUnbanned]
	class UnbanLog : BaseLog
	{
		public override string Title => "User Unbanned!";
		public override Color Colour => Color.FromArgb(255,255,255);
		
		public UnbanLog(Server server, User user) : base (server, user)
		{}
	}
	
	[OnUserChange]
	class ChangeLog
	{
		
		public ChangeLog(Server server, User oldUser, User newUser)
		{
			var logChannel = server.FindTextChannel("theguin-log");
			if (logChannel == null)
				return;
			
			if (oldUser.Nickname != newUser.Nickname ||
				oldUser.Username != newUser.Username)
			{
				Embed embed = new Embed();
				
				embed.Author.Name = newUser.Nickname;
				embed.Author.IconUrl = newUser.AvatarUrl;
				embed.Colour = Color.FromArgb(205, 137, 31);
				embed.ThumbnailUrl = newUser.AvatarUrl;
				
				Embed.Section section = new Embed.Section();
				section.Name = "Name Changed!";
				section.Value = String.Format("{0}\nto\n{1}", oldUser.DataString, newUser.DataString);
				section.IsInline = false;
				embed.Sections.Add(section);
				
				embed.Footer.IconUrl = newUser.AvatarUrl;
				embed.Footer.Text = String.Format("Time: {0}", DateTime.Now.ToString("h:mm:ss tt"));
				
				Message message = new Message(embed);
				
				logChannel.SendMessage(message);
			}
			
			if (oldUser.AvatarUrl != newUser.AvatarUrl)
			{
				Embed embed = new Embed();
				
				embed.Author.Name = newUser.Nickname;
				embed.Author.IconUrl = oldUser.AvatarUrl;
				embed.Colour = Color.FromArgb(205, 137, 31);
				embed.ThumbnailUrl = newUser.AvatarUrl;
				
				Embed.Section section = new Embed.Section();
				section.Name = "Avatar Changed!";
				section.Value = newUser.DataString;
				section.IsInline = false;
				embed.Sections.Add(section);
				
				embed.Footer.IconUrl = oldUser.AvatarUrl;
				embed.Footer.Text = String.Format("Time: {0}", DateTime.Now.ToString("h:mm:ss tt"));
				
				Message message = new Message(embed);
				
				logChannel.SendMessage(message);
			}
			
			if (oldUser.Roles.Count != newUser.Roles.Count)
			{
				TheGuin3.Interfaces.Base.Role newRole;
				if (oldUser.Roles.Count > newUser.Roles.Count)
					newRole = oldUser.Roles.Except(newUser.Roles).First();
				else
					newRole = newUser.Roles.Except(oldUser.Roles).First();
				
				if (newRole != null)
				{
					if (newRole.Name == "@everyone")
						return;
					
					Embed embed = new Embed();
					
					embed.Author.Name = newUser.Nickname;
					embed.Author.IconUrl = newUser.AvatarUrl;
					embed.Colour = Color.FromArgb(205, 137, 31);
					embed.ThumbnailUrl = newUser.AvatarUrl;
					
					Embed.Section section = new Embed.Section();
					section.Name = "User Gained Role!";
					section.Value = String.Format("{0}\ngained role\n{1} ({2})", newUser.DataString, newRole.Name, newRole.Id);
					section.IsInline = false;
					embed.Sections.Add(section);
					
					embed.Footer.IconUrl = oldUser.AvatarUrl;
					embed.Footer.Text = String.Format("Time: {0}", DateTime.Now.ToString("h:mm:ss tt"));
					
					Message message = new Message(embed);
					
					logChannel.SendMessage(message);
				}
			}
		}
	}
}
