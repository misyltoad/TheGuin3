using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TheGuin3.Audio
{
    public class Stream
    {
        static public Process MakeAudioProcess(string path)
        {
            var ffmpeg = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-i {path} -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
            };
            return Process.Start(ffmpeg);
        }

        static public Process MakeYoutubeProcess(string path, string name)
        {
            var ffmpeg = new ProcessStartInfo
            {
                FileName = "youtube-dl",
                Arguments = $"--extract-audio --output \"{name}.%(ext)s\" {path}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
            };
            return Process.Start(ffmpeg);
        }

        static public Task CopyStreamAsync(System.IO.Stream from, System.IO.Stream to)
        {
           return Task.Run(() =>
           {
               try
               {
                   from.CopyToAsync(to).Wait();
                   to.FlushAsync().Wait();
               }
               catch
               {
                   // Literally do nothing because it doesn't matter.
                   // Just changed to another song...
               }
           });
        }
    }
}
