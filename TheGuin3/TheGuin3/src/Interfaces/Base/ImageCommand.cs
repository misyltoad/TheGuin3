using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using ImageSharp;

namespace TheGuin3.Interfaces.Base
{
    public abstract class ImageCommand : Command
    {
        public ImageCommand(Context context)
            : base(context)
        {
        }

        public override void Execute()
        {
            if (Context.Args.Count == 0)
            {
                Context.Channel.SendMessage("You need to give me a URL dummy.");
                return;
            }

            try
            {
                WebRequest request = WebRequest.Create(Context.ArgsString);
                WebResponse response = request.GetResponseAsync().Result;
                Stream dataSteam = response.GetResponseStream();
                MemoryStream memoryStream = new MemoryStream();
                dataSteam.CopyTo(memoryStream);

                Image<Rgba32> image = Image.Load(memoryStream.ToArray());
                HandleImage(ref image);

                MemoryStream editedStream = new MemoryStream();
                image.SaveAsPng(editedStream);
                Context.Channel.SendFile(new MemoryStream(editedStream.ToArray()), "GeneratedImage.png");
            }
            catch (Exception)
            {
                Context.Channel.SendMessage("Unable to get image from URL.");
            }
        }

        public abstract void HandleImage(ref Image<Rgba32> image);
    }
}
