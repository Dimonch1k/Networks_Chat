using MySql.Data.MySqlClient;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace net_cw_1
{
    internal class Program
    {
        static List<TcpClient> clients = new List<TcpClient>();
        static object lockObj = new object();
        static string connectionString = "Server=34.118.84.47;Database=bdatab;User ID=bogdan;Password=!Bogdan666;";

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

        static bool RegisterUser(string username, string password)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string checkQuery = "SELECT COUNT(*) FROM Users WHERE username = @username";
                using (var checkCommand = new MySqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@username", username);
                    int userCount = Convert.ToInt32(checkCommand.ExecuteScalar());

                    if (userCount > 0) return false;
                }


                string query = "INSERT INTO Users (username, password) VALUES (@username, @password)";
                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@passwordHash", password);

                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
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