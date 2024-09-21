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
                string request = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                if (request.StartsWith("REGISTER"))
                {
                    string[] registrationData = request.Split(';');
                    string username = registrationData[1];
                    string password = registrationData[2];
                    string serverAddress = registrationData[3];

                    bool registrationSuccess = RegisterUser(username, password, serverAddress);

                    if (registrationSuccess)
                    {
                        byte[] successMessage = Encoding.UTF8.GetBytes("REGISTER_OK");
                        stream.Write(successMessage, 0, successMessage.Length);
                    }
                    else
                    {
                        byte[] errorMessage = Encoding.UTF8.GetBytes("REGISTER_ERROR");
                        stream.Write(errorMessage, 0, errorMessage.Length);
                    }

                    client.Close();
                }
                else if (request.StartsWith("LOGIN"))
                {
                    HandleLogin(client, stream, request);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static bool RegisterUser(string username, string password, string serverAddress)
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Check if the username already exists
                    string checkQuery = "SELECT COUNT(*) FROM Users WHERE username = @username";
                    using (var checkCommand = new MySqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@username", username);
                        long userCount = (long)checkCommand.ExecuteScalar();

                        if (userCount > 0)
                        {
                            Console.WriteLine($"Registration failed: Username {username} already exists.");
                            return false;
                        }
                    }

                    // Insert the new user into the database
                    string query = "INSERT INTO Users (username, password, server_address) VALUES (@username, @password, @serverAddress)";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@password", password);
                        command.Parameters.AddWithValue("@serverAddress", serverAddress);
                        command.ExecuteNonQuery();
                    }
                    Console.WriteLine($"User {username} registered successfully.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error registering user: {ex.Message}");
                return false;
            }
        }

        static void HandleLogin(TcpClient client, NetworkStream stream, string loginRequest)
        {
            string[] credentials = loginRequest.Split(';');

            string username = credentials[1];
            string password = credentials[2];
            string serverAddress = credentials[3];

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