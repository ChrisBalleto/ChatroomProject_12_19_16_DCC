using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Chatroom
{
    class Server
    {
        TcpListener serverListen;
        IPAddress ipAddress;
        int port;
        string data;
        int bytesFromClient;
        Byte[] bytes;

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
            StartLoop();
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
            serverListen = new TcpListener(IPAddress.Any, 10000);
            serverListen.Start();
            Console.WriteLine("Server Listening...\n\r");
        }
        public void MakeBuffer()
        {
            bytes = new Byte[10025];
            data = null;
        }
        
        public void StartLoop()
        {
            try
            {
                while(true)
               {
                    StartListening();
                    MakeBuffer();
                    Console.WriteLine("Currently No Connection...Waiting...");
                    TcpClient newClient = serverListen.AcceptTcpClient();
                    Console.WriteLine("Ding Ding Ding....Connection!!");
                    data = null;
                    NetworkStream stream = newClient.GetStream();
                    while ((bytesFromClient = stream.Read(bytes, 0, bytes.Length)) !=0)
                    {
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, bytesFromClient);
                        Console.WriteLine("Recieved: {0} \n\r", data);
                        Console.Write("Type Message\n\r");
                        string message = Console.ReadLine();
                        bytes = System.Text.Encoding.ASCII.GetBytes(message);
                        stream = newClient.GetStream();
                        stream.Write(bytes, 0, bytes.Length);
                        Console.WriteLine("Sent: {0}", message);
                    }
                    newClient.Close();
                }
            }
            catch(SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

        }

        
    }
}
