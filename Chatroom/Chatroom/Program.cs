﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Net;
using System.Threading.Tasks;

namespace Chatroom
{
    class Program
    {
        static void Main(string[] args)
        {
            Server newServer = new Server();
            newServer.StartUp();

            Console.ReadLine();
        }
    }
}
