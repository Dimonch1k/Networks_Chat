using System.Net;
using System.Net.Sockets;
using System.Text;

namespace net_cw_1
{
    internal class Program
    {
        static List<TcpClient> clients = new List<TcpClient>();
        static object lockObj = new object();

        static string connectionString = "Server=34.118.84.47;Database=ChatApp;User ID=dmytro;";

        //static string connectionString = ConfigurationManager.ConnectionStrings["DB_ChatApp"].ConnectionString;


        static void Main(string[] args)
        {
            TcpListener server = new TcpListener(IPAddress.Any, 25564);
            server.Start();
            Console.WriteLine("Multi thread server started. Waiting for clients...");

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                lock (lockObj)
                {
                    clients.Add(client);
                }
                Console.WriteLine("New client connected.");

                Thread clientThread = new Thread(() => HandleClient(client));
                clientThread.Start();
            }
        }

        static void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            string clientEndPoint = client.Client.RemoteEndPoint.ToString();

            Console.WriteLine($"Client {clientEndPoint} connected.");

            try
            {
                while (true)
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string messageReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"Client {clientEndPoint}: {messageReceived}");

                    if (messageReceived.ToLower() == "exit")
                    {
                        Console.WriteLine($"Client {clientEndPoint} has finished the chat.");
                        break;
                    }
                    BroadcastMessage(messageReceived, clientEndPoint);
                }
            }
            catch (Exception ex) { Console.WriteLine($"Error with client {clientEndPoint}: {ex.Message}"); }
            finally
            {
                lock (lockObj) clients.Remove(client);

                stream.Close();
                client.Close();
                Console.WriteLine($"Client {clientEndPoint} disconnected.");
            }
        }

        static void BroadcastMessage(string message, string sender)
        {
            byte[] data = Encoding.UTF8.GetBytes($"{sender}: {message}");

            lock (lockObj)
            {
                foreach (TcpClient client in clients)
                {
                    try
                    {
                        NetworkStream stream = client.GetStream();
                        stream.Write(data, 0, data.Length);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error during sending message: {ex.Message}");
                    }
                }
            }
        }

        static void LogConnection(TcpClient client)
        {
            string clientAddress = client.Client.RemoteEndPoint.ToString();
            string query = "INSERT INTO ConnectionLogs (ClientAddress, ConnectionTime) VALUES (@ClientAddress, @ConnectionTime)";

            using (ConnectionString = new(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ClientAddress", clientAddress);
                cmd.Parameters.AddWithValue("@ConnectionTime", DateTime.Now);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

        }

        static void LogDisconnection(TcpClient client)
        {
            string clientAddress = client.Client.RemoteEndPoint.ToString();
            string query = "UPDATE ConnectionLogs SET DisconnectionTime = @DisconnectionTime WHERE ClientAddress = @ClientAddress AND DisconnectionTime IS NULL";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ClientAddress", clientAddress);
                cmd.Parameters.AddWithValue("@DisconnectionTime", DateTime.Now);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
