using System;
using TheGuin3;
using TheGuin3.Interfaces;

using System;
using System.Numerics;
using System.Threading.Tasks;
using ImageSharp.PixelFormats;
using ImageSharp.Processing;
using ImageSharp.Processing.Processors;
using ImageSharp;

namespace TheGuin3.Modules.Fun
{
    [OnCommand("awwa", "Flips an image, but the other way")]
    class AwwaCommand : TheGuin3.Interfaces.Base.ImageCommand
    {
        public AwwaCommand(Context context) : base (context)
		{}
		
		public override void HandleImage(ref Image<Rgba32> image)
        {
			int halfWidth = (int) Math.Floor((float)image.Width / 2.0f);
			for (int x = 0; x < image.Width; x++)
			{
				for (int y = 0; y < image.Height; y++)
				{
					if (x <= halfWidth)
					{
						image[x, y] = image[image.Width - x, y];
					}
				}
			}
		}
    }
}

