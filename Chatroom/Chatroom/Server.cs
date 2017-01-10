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
    class Server
    {
        IDictionary<int, ServerClient> chatroomMembers = new Dictionary<int, ServerClient>();
        Queue<string> messageQueue = new Queue<string>();
        Logger chatlog = new Logger();
        TcpListener serverListen;
        IPAddress ipAddress;
        TcpClient newClient;
        int port;
        string data;
        Byte[] bytes;
        int userCount = 0;
        public Server()
        {
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
                MakeBuffer();
                newClient = serverListen.AcceptTcpClient();
                NetworkStream stream = newClient.GetStream();
                ServerClient newServerClient = new ServerClient(stream);
                AddUserData(newServerClient);             
                Task.Run(() => RecieveMessageLoop(newServerClient));
                Task.Run(() => SendMessageLoop());
                Task.Run(() => SendMessageToUsers());
            }                   
        }
        public void MakeBuffer()
        {
            bytes = new Byte[256];
            data = null;
        }        
        public void RecieveMessageLoop(ServerClient newServerClient)
        {
            while(true)
            {              
                Byte[] data = new Byte[256];
                newServerClient.clientNetStream.Read(data, 0, data.Length);
                string responseData = Encoding.ASCII.GetString(data);
                string toQueue = newServerClient.Username.Trim('\0') + ":" + responseData.Trim('\0');
                messageQueue.Enqueue(toQueue);
            }
            newServerClient.clientNetStream.Close();
            newClient.Close();
        }
        public void SendMessageToUsers()
        {
            while (true)
            {
                string message = "Server:" + Console.ReadLine();
                messageQueue.Enqueue(message);
                //Console.WriteLine("Server: {0}", message.Trim('\0'));
            }
        }
         public void SendMessageLoop()
        {
            while (true)
            {
                if (messageQueue.Count != 0)
                {
                    //string message = messageQueue.Dequeue().Trim('\0');
                    //Byte[] data = new Byte[256];
                    //data = Encoding.ASCII.GetBytes(message);
                    //stream.Write(data, 0, data.Length);
                    string message = messageQueue.Dequeue().Trim('\0');
                    Console.WriteLine(message.Trim('\0'));
                    chatlog.WriteToFile(message.Trim('\0'));
                    foreach (ServerClient client in chatroomMembers.Values)
                    {
                        Byte[] data = new Byte[256];
                        data = Encoding.ASCII.GetBytes(message);
                        client.clientNetStream.Write(data, 0, data.Length);
                    }                    
                }
            }
        }
        public void AddUserData(ServerClient newServerClient)
        {
            userCount++;
            Byte[] data = new Byte[256];
            newServerClient.clientNetStream.Read(data, 0, data.Length);
            string responseData = Encoding.ASCII.GetString(data).Trim('\0');
            newServerClient.Username = responseData;
            string trimmed = newServerClient.Username.Trim('\0');
            string connected = newServerClient.Username.Trim('\0') + " has connected.";
            messageQueue.Enqueue(connected.Trim('\0'));
            chatroomMembers.Add(userCount, newServerClient);            
        }
    }
}
