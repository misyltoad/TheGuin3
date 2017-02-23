using System;
using System.Collections.Generic;
using System.Text;

namespace TheGuin3.Interfaces.Base
{
    public class Message
    {
        public Message(string value)
        {
            Text = value;
        }

        public static implicit operator string(Message msg)
        {
            return msg.Text;
        }

        public static implicit operator Message(string text)
        {
            return new Message(text);
        }

        public string Text { get; }
    }
}
