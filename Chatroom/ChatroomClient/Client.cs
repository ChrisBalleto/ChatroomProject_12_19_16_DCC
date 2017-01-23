using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace ChatroomClient 
{
    class Client
    {
        IPAddress ipAddress;
        int port;
        TcpClient newTcpClient;
        string message;
        string username;
        NetworkStream stream;
        public void StartUp(Client client)
        {
            SetIpAddress();
            SetPort();
            SetUsername();
            ConnectToServer();
            SendUsername();
            Parallel.Invoke(StartRecieveLoop, StartSendLoop);
        }
        public void SetIpAddress()
        {
            Console.WriteLine("...Welcome to B-Chat!...");
            Console.WriteLine("What is the Server's IP Address?\n\r");
            ipAddress = IPAddress.Parse(Console.ReadLine());
        }
        public void SetPort()
        {
            Console.WriteLine("What Port is the Server using?\n\r");
            port = int.Parse(Console.ReadLine());
        }
        public void ConnectToServer()
        {
            newTcpClient = new TcpClient();            
            newTcpClient.Connect(ipAddress, port); 
            Console.WriteLine("Connection Made.\n\r");
            stream = newTcpClient.GetStream();
        }
        public void SetUsername()
        {
            Console.WriteLine("What is your username?");
            username = Console.ReadLine();
        }
        public void StartRecieveLoop()
        {
            while (true)
            {
                Byte[] data = new Byte[256];          
                stream.Read(data, 0, data.Length);
                string responseData = Encoding.ASCII.GetString(data);
                Console.WriteLine(responseData.Trim('\0'));
            }

        }    
        public void StartSendLoop()
        {
            while (true)
            {
                Byte[] data = new Byte[256];
                message = Console.ReadLine();
                data = System.Text.Encoding.ASCII.GetBytes(message);
                stream.Write(data, 0, data.Length);
            }
        }
        public void SendUsername()
        {
            Byte[] data = new Byte[256];
            data = Encoding.ASCII.GetBytes(username);
            stream.Write(data, 0, data.Length);
        }
    }
}
