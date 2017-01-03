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
        Byte[] data;
        String responseData;
        NetworkStream stream;




        public void StartUp(Client client)
        {
            SetIpAddress();
            SetPort();
            ConnectToServer();
            StartChatLoop();   
        }
        public void SetIpAddress()
        {
            Console.WriteLine("...Welcome to B-Chat!...");
            Console.WriteLine("What is your IP Address?\n\r");
            ipAddress = IPAddress.Parse(Console.ReadLine());
        }
        public void SetPort()
        {
            Console.WriteLine("What is your Port Number?\n\r");
            port = int.Parse(Console.ReadLine());
        }
        public void ConnectToServer()
        {
            newTcpClient = new TcpClient();
            newTcpClient.Connect(ipAddress, port);  //change to (ipAddress,port) when project is completed
            Console.WriteLine("Connection Made.\n\r");
            Console.Write("Type Message\n\r");
        }
        public void StartChatLoop()
        {
            bool isChatting = true;
            while(isChatting == true)
            {
                string message = Console.ReadLine();               
                data = System.Text.Encoding.ASCII.GetBytes(message);
                stream = newTcpClient.GetStream();
                stream.Write(data, 0, data.Length);              
                Console.WriteLine("Sent: {0}", message);
                data = new Byte[10025];
                responseData = String.Empty;
                int bytes = stream.Read(data, 0, data.Length);
                responseData = Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Recieved: {0}", responseData);
            }
            stream.Close();
            newTcpClient.Close();
        }

    }
}
