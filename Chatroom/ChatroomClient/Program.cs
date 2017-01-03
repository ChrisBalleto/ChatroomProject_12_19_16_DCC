using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Net;
using System.Threading.Tasks;

namespace ChatroomClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Client newClient = new Client();
            newClient.StartUp(newClient);

            Console.ReadLine();
        }
    }
}
