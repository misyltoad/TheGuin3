using System;
using System.Threading;
using System.Collections.Generic;
using TheGuin3;
using TheGuin3.Interfaces;

namespace TheGuin3.Modules.Audio
{	
    [OnCommand("queue", "Queues something.")]
    class QueueCommand : TheGuin3.Interfaces.Base.Command
    {
        public QueueCommand(Context context) : base (context)
		{}
		
		public struct QueueData
		{
			public string url;
		}
		
		public override void Execute()
        {
			var audioChannel = Context.User.AudioChannel;
			if (audioChannel == null)
			{
				Context.Channel.SendMessage("You need to be in a voice channel for me to join!");
				return;
			}
		
			VideoQueueMutex.WaitOne();		
			if (!VideoQueues.ContainsKey(Context.Server.Id))
				VideoQueues[Context.Server.Id] = new List<QueueData>();
				
			QueueData data = new QueueData();
			data.url = Context.ArgsString;
		
			VideoQueues[Context.Server.Id].Add(data);
			VideoQueueMutex.ReleaseMutex();
			
			Context.Channel.SendMessage("Added to queue.");
			
			VideoQueueMutex.WaitOne();
			var areWeFirst = VideoQueues[Context.Server.Id].Count == 1;
			VideoQueueMutex.ReleaseMutex();
			
			if (areWeFirst)
			{
				audioChannel.Join();
				
				VideoQueueMutex.WaitOne();
				var isCountGreater = VideoQueues[Context.Server.Id].Count > 0;
				VideoQueueMutex.ReleaseMutex();
				while (isCountGreater)
				{
					// Start playing queue if we are the starter of the queue.
					// Become the queue handler for this server.
					VideoQueueMutex.WaitOne();
					QueueData newData = VideoQueues[Context.Server.Id][0];
					VideoQueueMutex.ReleaseMutex();
					
					audioChannel.PlayFile(newData.url);
					
					VideoQueueMutex.WaitOne();
					VideoQueues[Context.Server.Id].RemoveAt(0);
					isCountGreater = VideoQueues[Context.Server.Id].Count > 0;
					VideoQueueMutex.ReleaseMutex();
				}
			}
		}
		
		public static Mutex VideoQueueMutex = new Mutex();
		public static Dictionary<string, List<QueueData>> VideoQueues = new Dictionary<string, List<QueueData>>();
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
				QueueCommand.VideoQueueMutex.WaitOne();
				QueueCommand.VideoQueues.Clear();
				QueueCommand.VideoQueueMutex.ReleaseMutex();
				Context.Channel.SendMessage("Cleaned queue.");
				
				new QueueCommand(Context).Execute();
			}
			else
			{
				Context.Channel.SendMessage("You need to be an admin to force play a song!");
			}
		}
    }
}
