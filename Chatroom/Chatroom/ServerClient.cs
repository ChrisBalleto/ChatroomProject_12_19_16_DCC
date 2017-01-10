using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Sockets;

namespace Chatroom
{
    class ServerClient
    {
        public NetworkStream clientNetStream;
        string username;
        public ServerClient(NetworkStream stream)
        {
            clientNetStream = stream;
        }
        public ServerClient(string name)
        {
            username = name;
        }
        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
            }
        }
    }
}
