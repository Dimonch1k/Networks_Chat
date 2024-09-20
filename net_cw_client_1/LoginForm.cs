using System;
using System.Data.SqlClient;
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

        public LoginForm()
        {
            InitializeComponent();

        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Input your username and password to connect chat.");
            string username = usernameTB.Text;
            string password = passwordTB.Text;

            if (Authenticate(username, password))
            {
                // Open chat form
                ChatForm chatForm = new ChatForm(username);
                chatForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid credentials.");
            }

        }

        private bool Authenticate(string username, string password)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Users WHERE Username = @username AND Password = @password";
                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password); // Hash passwords in real applications
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
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