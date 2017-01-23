using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.IO;

namespace Chatroom
{
    class Program
    {
        static void Main(string[] args)
        {
            ILogger text = new Text();
            Server newServer = new Server(text);
            newServer.StartUp();

            Console.ReadLine();
        }
    }
}
