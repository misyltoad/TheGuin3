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
        public void PlayFile(string filename)
        {
            Process currentProcess = null;
            if (CachedProcesses.TryGetValue(Id, out currentProcess) && currentProcess != null)
            {
                try
                {
                    CachedProcesses.Remove(Id);
                    currentProcess.Kill();
                    currentProcess.WaitForExit();
                }
                catch { }
            }

            try
            {
                Process process = null;
                string[] files = null;
                if (filename.Contains("youtube"))
                {
                    Directory.CreateDirectory("data");
                    Directory.CreateDirectory("data/temp");
                    Directory.CreateDirectory("data/temp/youtube");

                    string basePath = "data/temp/youtube";
                    string name = String.Format("YOUTUBE_VIDEO_{0}", CountId.ToString());
                    string path = String.Format("{0}/{1}", basePath, name);
                    CountId++;

                    process = Audio.Stream.MakeYoutubeProcess(filename, path);
                    process.WaitForExit();

                    files = Directory.GetFiles(basePath, $"{name}.*");
                    if (files.Length > 0)
                        process = Audio.Stream.MakeAudioProcess(files[0]);
                }
                else
                    process = Audio.Stream.MakeAudioProcess(filename);

                CachedProcesses.Add(Id, process);
                Audio.Stream.CopyStreamAsync(process.StandardOutput.BaseStream, AudioStream).Wait();

                if (files != null)
                    DeleteFiles(files);
            }
            catch (Exception e)
            {
                Console.WriteLine("FAILED TO COPY STREAM ASYNC! {0}", e.Message);
            }
        }

        public static int CountId = 0;

        public abstract Stream AudioStream { get; }
        public abstract string Id { get; }

        public static Dictionary<string, Process> CachedProcesses = new Dictionary<string, Process>();
    }
}
