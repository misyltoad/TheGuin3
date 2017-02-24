using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Drawing;
using TheGuin3.Interfaces;   
using TheGuin3.Interfaces.Discord; 

namespace TheGuin3.Modules.Core
{
    [OnCommand("modules", "Get some help configuring TheGuin modules", true)]
    public class ModuleCommand : TheGuin3.Interfaces.Base.Command
    {
		public ModuleCommand(Context context) : base (context) {}
		
		private string[] MagicalWords = {
			"Alakazam!",
			"Hocus Pocus!",
			"Kapow!",
			"Kazam!",
		};
		
		protected override void Execute()
		{
			var schema = Config.Schema.ModuleListConfig.Get(Context.Server);
			var currentModules = schema.Modules;
			
			if (Context.Args.Count != 2 || (Context.Args.Count >= 1 && Context.Args[0].ToLower() == "list"))
			{
				Embed embed = new Embed();
				
				embed.Author.Name = String.Format("{0} | Modules", Context.Server.Name);
				embed.Author.IconUrl = Context.Server.AvatarUrl;
				
				embed.Colour = Color.FromArgb(52, 152, 219);
				
				embed.ThumbnailUrl = Context.Server.AvatarUrl;
				
				var output = "";
				var availableModules = new List<string>();
				
				foreach (var module in Context.Client.ModuleRegistry.Modules)
				{
					availableModules.Add(module.Meta.Name);
				}
				
				foreach (var module in availableModules)
				{
					output += String.Format("{0} {1}{2}\n", currentModules.Contains(module) ? "✅" : "❌",  module, module == "core" ? " [Required]" : "");
				}
				
				Embed.Section section = new Embed.Section();
				section.Name = String.Format("Available modules for {0}:", Context.Server.Name);
				section.Value = output;
				section.IsInline = false;
				embed.Sections.Add(section);
				
				section = new Embed.Section();
				section.Name = "Enabling/Disabling modules";
				section.Value = "To enable/disable a module you can can call this command with those arguments respectively\n";
				section.Value += "For example, \n``!modules enable utility`` or \n``!modules disable imagefun``\n\n";
				
				section.IsInline = false;
				embed.Sections.Add(section);
				
				embed.Footer.IconUrl = "http://i.imgur.com/E48G7oi.png";
				embed.Footer.Text = "TheGuin says: \"To see this again, do !modules list\"";
				
				Message message = new Message(embed);
				Context.Channel.SendMessage(message);
				return;
			}
			
			if (
			(Context.Args[0].ToLower() == "enable" && currentModules.Contains(Context.Args[1])) || 
			(Context.Args[0].ToLower() == "disable" && !currentModules.Contains(Context.Args[1]))
			   )
			{
				Embed embed = new Embed();
				embed.Author.Name = String.Format("{0} | Modules", Context.Server.Name);
				embed.Author.IconUrl = Context.Server.AvatarUrl;
				embed.Colour = Color.FromArgb(205, 137, 31);
				
				Embed.Section section = new Embed.Section();
				section.Name = "Issue with your command";
				section.Value = String.Format("The module, '{0}' is already {1}d.", Context.Args[1].ToLower(), Context.Args[0].ToLower());
				section.IsInline = false;
				embed.Sections.Add(section);
				
				embed.Footer.IconUrl = "http://i.imgur.com/E48G7oi.png";
				embed.Footer.Text = String.Format("TheGuin says: \"Did you mean !modules {0} {1}?\"", Context.Args[0].ToLower() == "enable" ? "disable" : "enable", Context.Args[1].ToLower());
				
				Message message = new Message(embed);
				Context.Channel.SendMessage(message);
				return;
			}
			
			if (Context.Args[0].ToLower() == "enable")
				schema.Modules.Add(Context.Args[1]);
			
			if (Context.Args[0].ToLower() == "disable")
				schema.Modules.Remove(Context.Args[1]);
			
			{
				Config.Schema.ModuleListConfig.Set(schema, Context.Server);
				
				Embed embed = new Embed();
				embed.Author.Name = String.Format("{0} | Modules", Context.Server.Name);
				embed.Author.IconUrl = Context.Server.AvatarUrl;
				embed.Colour = Context.Args[0].ToLower() == "enable" ? Color.FromArgb(46,204,113) : Color.FromArgb(231,76,60);
				
				Embed.Section section = new Embed.Section();
				
				Random random = new Random();
				var magicValue = random.Next(0, MagicalWords.Length);
				var magicalWord = MagicalWords[magicValue];
				
				section.Name = magicalWord;
				section.Value = String.Format("The module, '{0}' is {1}d!", Context.Args[1].ToLower(), Context.Args[0].ToLower());
				section.IsInline = false;
				embed.Sections.Add(section);
				
				embed.Footer.IconUrl = "http://i.imgur.com/E48G7oi.png";
				embed.Footer.Text = "TheGuin says: \"Magic!\"";
				
				Message message = new Message(embed);
				Context.Channel.SendMessage(message);
			}
		}
    }
}
