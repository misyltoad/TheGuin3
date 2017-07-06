using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.WebSocket;
using System.IO;
using System.Threading.Tasks;

namespace TheGuin3.Interfaces.Discord
{
    public class AudioChannel : Base.AudioChannel
    {
        public AudioChannel(SocketVoiceChannel channel)
        {
            DiscordInterface = channel;
        }

        public override void Join()
        {
            try
            {
                var client = DiscordInterface.ConnectAsync().Result;
                CachedStreams[DiscordInterface.Id] = client.CreatePCMStream(global::Discord.Audio.AudioApplication.Music);
            } catch (Exception e)
            {
                Console.WriteLine("FAILED TO JOIN AND CREATE STREAM! {0}", e.Message);
            }
        }

        public override Stream AudioStream
        {
            get
            {
                return CachedStreams[DiscordInterface.Id];
            }
        }

        public override string Id
        {
            get
            {
                return DiscordInterface.Id.ToString();
            }
        }

        public static Dictionary<ulong, Stream> CachedStreams = new Dictionary<ulong, Stream>();
        private SocketVoiceChannel DiscordInterface;
    }
}
