using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace net_cw_1._1requestBytes
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TcpClient client = new TcpClient("127.0.0.1", 25564);
            Console.WriteLine("Connected to the server");

            NetworkStream stream = client.GetStream();

            Thread receiveThread = new Thread(() => ReceiveMessages(stream));
            receiveThread.Start();

            while (true)
            {
                string messageToSend = Console.ReadLine();
                byte[] dataToSend = Encoding.UTF8.GetBytes(messageToSend);
                stream.Write(dataToSend, 0, dataToSend.Length);

                if (messageToSend.ToLower() == "exit")
                {
                    Console.WriteLine("The chat has finished.");
                    break;
                }
            }

            stream.Close();
            client.Close();
        }

        static void ReceiveMessages(NetworkStream stream)
        {
            byte[] buffer = new byte[1024];
            while (true)
            {
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string messageReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Server: {messageReceived}");

                if (messageReceived.ToLower() == "exit")
                {
                    Console.WriteLine("Server has finished the chat.");
                    break;
                }
            }
        }
    }
}
