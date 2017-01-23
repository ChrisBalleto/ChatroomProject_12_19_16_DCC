using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.ComponentModel;
using System.Collections.Specialized;

namespace Chatroom 
{
    class Server : ISubscriber
    {
        
        Dictionary<int, ServerClient> chatroomMembers = new Dictionary<int, ServerClient>();
        Queue<string> messageQueue = new Queue<string>();
        ILogger logger;
        TcpListener serverListen;
        IPAddress ipAddress;
        TcpClient newClient;
        //StreamWriter sWriter;
        string serverUsername;
        int port;
        Byte[] bytes;
        int userCount = 0;
        public Server(ILogger logger)
        {
            this.logger = logger;
        }
        public IPAddress IpAddress
        {
            get
            {
                return ipAddress;
            }
            set
            {
                ipAddress = value;
            }
        }
        public int Port
        {
            get
            {
                return port;
            }
            set
            {
                port = value;
            }
        }
        public void StartUp()
        {
            SetIpAddress();
            SetPort();
            SetServerName();
            StartListening();           
        }
        public void SetIpAddress()
        {
            Console.WriteLine("Server Booting Up...\n\r");
            Console.WriteLine("What is your IP Address?\n\r");
            ipAddress = IPAddress.Parse(Console.ReadLine());
        }
        public void SetPort()
        {
            Console.WriteLine("What is your Port Number?\n\r");
            port = int.Parse(Console.ReadLine());
        }
        public void StartListening()
        {
            Console.WriteLine("Server Listening...\n\r");
            serverListen = new TcpListener(IPAddress.Any, port);
            serverListen.Start();
            while (true)
            {
                bytes = new Byte[256];
                newClient = serverListen.AcceptTcpClient();
                NetworkStream stream = newClient.GetStream();
                ServerClient newServerClient = new ServerClient(stream);
                AddUserData(newServerClient);             
                Task.Run(() => RecieveMessageLoop(newServerClient));
                Task.Run(() => SendMessageLoop());
                Task.Run(() => SendMessageToQueue());
            }                   
        }
        public void SetServerName()
        {
            Console.WriteLine("What is your username Server?");
            serverUsername = Console.ReadLine();          
        }       
        public void RecieveMessageLoop(ServerClient newServerClient)
        {
            while(true)
            {              
                Byte[] data = new Byte[256];
                newServerClient.clientNetStream.Read(data, 0, data.Length);
                string responseData = Encoding.ASCII.GetString(data);
                string toQueue = newServerClient.Username.Trim('\0') + ": " + responseData.Trim('\0');
                messageQueue.Enqueue(toQueue);
            }
        }
        public void SendMessageToQueue()
        {
            while (true)
            {
                string message = serverUsername + ": " + Console.ReadLine();
                messageQueue.Enqueue(message);
            }
        }
         public void SendMessageLoop()
        {
            while (true)
            {
                if (messageQueue.Count != 0)
                {
                    string message = messageQueue.Dequeue().Trim('\0');                   
                    logger.WriteToFile(message.Trim('\0'));
                    foreach (ServerClient client in chatroomMembers.Values)
                    {
                        Byte[] data = new Byte[256];
                        data = Encoding.ASCII.GetBytes(message);
                        client.clientNetStream.Write(data, 0, data.Length);
                    }
                    PrintToServerConsole(message);
                }
            }
        }
        public void PrintToServerConsole(string message)
        {
            Console.WriteLine(message.Trim('\0'));
        }
        public void AddUserData(ServerClient newServerClient)
        {
            userCount++;
            Byte[] data = new Byte[256];
            newServerClient.clientNetStream.Read(data, 0, data.Length);
            string responseData = Encoding.ASCII.GetString(data).Trim('\0');
            newServerClient.Username = responseData;
            //NotifyAll(newServerClient.Username.Trim('\0'));
            string trimmed = newServerClient.Username.Trim('\0');
            string connected = newServerClient.Username.Trim('\0') + " has connected.";
            messageQueue.Enqueue(connected.Trim('\0'));
            chatroomMembers.Add(userCount, newServerClient);            
        }

        public void NotifyAll(string name)
        {
            foreach(ServerClient client in chatroomMembers.Values)
            {
                client.Notify(name);
            }

        }
        public void Notify(string name)
        {
            Console.Write("{0} has joined the chat.", name);
        }
    }
}
