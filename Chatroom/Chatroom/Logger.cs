﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Chatroom
{
    class Logger
    {
        
        public void WriteToFile(string data, StreamWriter writer)
        {
            writer.WriteLine(data);
            writer.Flush();         
        }
    }
}
