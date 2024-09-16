using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace net_cw_client_2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Start()
        {
            TcpClient client = new TcpClient();
            client.Connect("34.118.84.47", 25564);
            Console.WriteLine("Connected to the server.");

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

        private void ReceiveMessages(NetworkStream stream)
        {
            byte[] buffer = new byte[1024];
            while (true)
            {
                int bytesread = stream.Read(buffer, 0, buffer.Length);
                string messagereceived = Encoding.UTF8.GetString(buffer, 0, bytesread);
                Console.WriteLine($"server: {messagereceived}");

                if (messagereceived.ToLower() == "exit")
                {
                    Console.WriteLine("server has finished the chat.");
                    break;
                }
            }
        }
    }
}
