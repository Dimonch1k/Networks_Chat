using System.Net;
using System.Net.Sockets;
using System.Text;
using MySql;
using MySql.Data.MySqlClient;

namespace net_cw_1
{
    internal class Program
    {
        static List<TcpClient> clients = new List<TcpClient>();
        static object lockObj = new object();
        static string connectionString = "Server=34.118.84.47;Database=ChatApp;User ID=dmytro;Password=your_password";

        static void Main(string[] args)
        {
            TcpListener server = new TcpListener(IPAddress.Any, 25564);
            server.Start();
            Console.WriteLine("Multi-threaded server started. Waiting for clients...");

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                Thread clientThread = new Thread(() => HandleClient(client));
                clientThread.Start();
            }
        }

        static void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];

            try
            {
                // Read username, password, and server address sent by the client
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string loginData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                string[] credentials = loginData.Split(';');

                string username = credentials[0];
                string password = credentials[1];
                string serverAddress = credentials[2];

                bool isAuthenticated = AuthenticateUser(username, password, serverAddress);

                if (isAuthenticated)
                {
                    byte[] okMessage = Encoding.UTF8.GetBytes("OK");
                    stream.Write(okMessage, 0, okMessage.Length);

                    lock (lockObj)
                    {
                        clients.Add(client);
                    }

                    HandleChat(client, username);
                }
                else
                {
                    byte[] errorMessage = Encoding.UTF8.GetBytes("ERROR: Authentication failed");
                    stream.Write(errorMessage, 0, errorMessage.Length);
                    client.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static bool AuthenticateUser(string username, string password, string serverAddress)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Users WHERE username = @username AND password = @password";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string dbServerAddress = reader["server_address"].ToString();
                            return dbServerAddress == serverAddress;
                        }
                    }
                }
            }
            return false;
        }

        static void HandleChat(TcpClient client, string username)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            Console.WriteLine($"User {username} authenticated.");

            try
            {
                while (true)
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"{username}: {message}");
                    BroadcastMessage($"{username}: {message}", client);
                }
            }
            finally
            {
                lock (lockObj)
                {
                    clients.Remove(client);
                }
                stream.Close();
                client.Close();
            }
        }

        static void BroadcastMessage(string message, TcpClient sender)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            lock (lockObj)
            {
                foreach (TcpClient client in clients)
                {
                    if (client != sender)
                    {
                        try
                        {
                            NetworkStream stream = client.GetStream();
                            stream.Write(data, 0, data.Length);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error sending message: {ex.Message}");
                        }
                    }
                }
            }
        }
    }
}