using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TheGuin3.Interfaces.Base
{
    public abstract class AudioChannel
    {
        public abstract void Join();

        private void DeleteFile(string file)
        {
            try
            {
                try
                {
                    File.Delete(file);
                }
                catch (IOException)
                {
                    Thread.Sleep(100);
                    DeleteFile(file);
                }
            }
            catch
            {
                // The other exceptions like missing file and crap don't matter.
            }
        }

        private void DeleteFiles(string[] files)
        {
            foreach (var file in files)
            {
                DeleteFile(file);
            }
        }
        public Task PlayFile(string filename)
        {
            List<Process> serverProcesses = null;
            if (CachedProcesses.TryGetValue(Id, out serverProcesses) && serverProcesses != null)
            {

                foreach (Process process in serverProcesses)
                {
                    try
                    {
                        process.Kill();
                    }
                    catch
                    {
                        // Process was lost or some crap.
                        // Doesn't matter.
                    }
                }
                CachedProcesses.Remove(Id);
            }

            try
            {
                bool isYoutube = filename.Contains("//youtube") || filename.Contains("//youtu.be") || filename.Contains("//www.youtube") || filename.Contains("//www.youtu.be");
                if (isYoutube)
                {
                    Process youtubedlProcess = Audio.Stream.MakeYoutubeProcess(filename);
                    Process ffmpegProcess = Audio.Stream.MakeAudioProcess("-");

                    List<Process> processes = new List<Process>();
                    processes.Add(youtubedlProcess);
                    processes.Add(ffmpegProcess);

                    CachedProcesses.Add(Id, processes);

                    Audio.Stream.CopyStreamAsync(youtubedlProcess.StandardOutput.BaseStream, ffmpegProcess.StandardInput.BaseStream);
                    Task.Run(() =>
                   {
                       try
                       {
                           youtubedlProcess.WaitForExit();
                       }
                       catch
                       {

                       }

                       Thread.Sleep(3500);

                       try
                       {
                           ffmpegProcess.Kill();
                       }
                       catch
                       {

                       }
                   });
                    return Audio.Stream.CopyStreamAsync(ffmpegProcess.StandardOutput.BaseStream, AudioStream);
                }
                else
                {
                    Process ffmpegProcess = Audio.Stream.MakeAudioProcess(filename);

                    List<Process> processes = new List<Process>();
                    processes.Add(ffmpegProcess);
                    CachedProcesses.Add(Id, processes);

                    return Audio.Stream.CopyStreamAsync(ffmpegProcess.StandardOutput.BaseStream, AudioStream);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("FAILED TO COPY STREAM ASYNC! {0}", e.Message);
                return Task.FromResult<object>(null);
            }
        }

        public abstract Stream AudioStream { get; }
        public abstract string Id { get; }

        public static Dictionary<string, List<Process>> CachedProcesses = new Dictionary<string, List<Process>>();
    }
}
