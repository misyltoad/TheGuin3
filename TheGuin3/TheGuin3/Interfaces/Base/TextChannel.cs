using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TheGuin3.Interfaces.Base
{
    public abstract class TextChannel
    {
        public abstract void SendMessage(Message message);
        public abstract void SendFile(string filename);
        public abstract void SendFile(Stream stream, string fakeFilename);

        public abstract List<User> Users { get; }
        public abstract Server Server { get; }
        public abstract string Name { get; }
        public abstract string Id { get; }
    }
}
