﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;

namespace Chatroom
{
    class Text : ILogger
    {
        StreamWriter writer = new StreamWriter("chatlog.txt", true);
        public void WriteToFile(string data)
        {
            writer.WriteLine(data);
            writer.Flush();
        }
    }
}
