using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System;

using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace net_cw_client_1
{
    public partial class LoginForm : Form
    {
        string username;
        string password;
        
        TcpClient client = new TcpClient();

        public LoginForm()
        {
            InitializeComponent();

        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string serverAddress = txtServerAddress.Text;

            try
            {
                client = new TcpClient(serverAddress, 25564);
                NetworkStream stream = client.GetStream();

                // Send credentials to server
                string loginData = $"{username};{password};{serverAddress}";
                byte[] data = Encoding.UTF8.GetBytes(loginData);
                stream.Write(data, 0, data.Length);

                // Wait for server's response
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string serverResponse = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                if (serverResponse == "OK")
                {
                    MessageBox.Show("Connected to server!", "Success", MessageBoxButtons.OK);
                    StartChat(stream);
                }
                else
                {
                    MessageBox.Show("Authentication failed.", "Error", MessageBoxButtons.OK);
                    client.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK);
            }

        }

        //private bool Authenticate(string username, string password)
        //{
        //    using (var connection = new MySqlConnection(connectionString))
        //    {
        //        connection.Open();
        //        string query = "SELECT COUNT(*) FROM Users WHERE Username = @username AND Password = @password";
        //        using (var cmd = new MySqlCommand(query, connection))
        //        {
        //            cmd.Parameters.AddWithValue("@username", username);
        //            cmd.Parameters.AddWithValue("@password", password); // Hash passwords in real applications
        //            int count = Convert.ToInt32(cmd.ExecuteScalar());
        //            return count > 0;
        //        }
        //    }
        //}


        private void Start()
        {
            
            client.Connect("34.118.84.47", 25564);
     

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