using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatroom
{
    class ServerClient
    {
        string username;

        public ServerClient()
        {
            
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
