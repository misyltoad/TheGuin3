using System;
using System.Threading;
using System.Collections.Generic;
using System.Collections.Concurrent;
using TheGuin3;
using TheGuin3.Interfaces;

namespace TheGuin3.Modules.Audio
{	
    [OnCommand("queue", "Queues something.")]
    class QueueCommand : TheGuin3.Interfaces.Base.Command
    {
        public QueueCommand(Context context) : base (context)
		{}
		
		public class QueueData
		{
			public string url;
		}
		
		public bool IsQueueEmpty()
		{
			return VideoQueues[Context.Server.Id] == null || VideoQueues[Context.Server.Id].Count < 1;
		}
		
		public override void Execute()
        {
			var audioChannel = Context.User.AudioChannel;
			if (audioChannel == null)
			{
				Context.Channel.SendMessage("You need to be in a voice channel for me to join!");
				return;
			}
			
			if (!VideoQueues.ContainsKey(Context.Server.Id))
				VideoQueues[Context.Server.Id] = new List<QueueData>();
				
			QueueData data = new QueueData();
			data.url = Context.ArgsString;
		
			VideoQueues[Context.Server.Id].Add(data);
			
			Context.Channel.SendMessage("Added to queue.");
			
			var areWeFirst = VideoQueues[Context.Server.Id].Count == 1;
			
			if (areWeFirst)
			{
				audioChannel.Join();
				
				while (!IsQueueEmpty())
				{
					// Start playing queue if we are the starter of the queue.
					// Become the queue handler for this server.

					string url = null;
					List<QueueData> queue = null;
					
					VideoQueues.TryGetValue(Context.Server.Id, out queue);
					
					if (queue != null)
					{
						if (queue.Count != 0)
						{
							QueueData currentVideoData = queue[0];
							if (currentVideoData != null)
								url = currentVideoData.url;
						}	
					}
					
					if (url != null && !String.IsNullOrEmpty(url))
					{
						audioChannel.PlayFile(url).Wait();
						queue.RemoveAt(0);
					}
				}
			}
		}
		
		public static ConcurrentDictionary<string, List<QueueData>> VideoQueues = new ConcurrentDictionary<string, List<QueueData>>();
    }
	
		
	[OnCommand("clearqueue", "Clears the queue for this server.")]
    class ClearQueueCommand : TheGuin3.Interfaces.Base.Command
    {
        public ClearQueueCommand(Context context) : base (context)
		{}
		
		public override void Execute()
        {
			var audioChannel = Context.User.AudioChannel;
			if (audioChannel == null)
			{
				Context.Channel.SendMessage("You need to be in a voice channel for me to join!");
				return;
			}
			
			if (Context.User.IsAdmin)
			{
				QueueCommand.VideoQueues.Clear();
				Context.Channel.SendMessage("Cleaned queue.");
			}
			else
			{
				Context.Channel.SendMessage("You need to be an admin to force play a song!");
			}
		}
    }
	
	[OnCommand("play", "Plays an audio file from the internet.")]
    class PlayCommand : TheGuin3.Interfaces.Base.Command
    {
        public PlayCommand(Context context) : base (context)
		{}
		
		public override void Execute()
        {
			var audioChannel = Context.User.AudioChannel;
			if (audioChannel == null)
			{
				Context.Channel.SendMessage("You need to be in a voice channel for me to join!");
				return;
			}
			
			if (Context.User.IsAdmin)
			{
				new ClearQueueCommand(Context).Execute();
				new QueueCommand(Context).Execute();
			}
			else
			{
				Context.Channel.SendMessage("You need to be an admin to force play a song!");
			}
		}
    }
}
