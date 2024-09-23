using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace net_cw_client_2
{
    public partial class LoginForm : Form
    {
        private TcpClient client;
        private NetworkStream stream;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            string username = usernameTB.Text;
            string password = passwordTB.Text;
            string serverAddress = serverIpTB.Text;

            try
            {

                client = new TcpClient("34.118.84.47", 25564);
                stream = client.GetStream();
                MessageBox.Show("2", "Success", MessageBoxButtons.OK);

                // Login request to server
                string loginData = $"LOGIN;{username};{password};{serverAddress}";
                byte[] data = Encoding.UTF8.GetBytes(loginData);
                stream.Write(data, 0, data.Length);


                // Wait for server's response about login
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string serverResponse = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                if (serverResponse == "OK")
                {
                    MessageBox.Show("Connected to server!", "Success", MessageBoxButtons.OK);
                    OpenChatWindow(username, stream, client);
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

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string username = usernameTB.Text;
            string password = passwordTB.Text;
            string serverAddress = serverIpTB.Text;

            try
            {
                client = new TcpClient(serverAddress, 25564);
                stream = client.GetStream();

                // Send registration data to server
                string registrationData = $"REGISTER;{username};{password};{serverAddress}";
                byte[] data = Encoding.UTF8.GetBytes(registrationData);
                stream.Write(data, 0, data.Length);

                // Wait for server's response
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string serverResponse = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                if (serverResponse == "REGISTER_OK")
                {
                    MessageBox.Show("Registration successful!", "Success", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("Registration failed. Username may already be taken.", "Error", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK);
            }
            //finally
            //{
            //    stream.Close();
            //    client.Close();
            //}
        }
        private void OpenChatWindow(string username, NetworkStream stream, TcpClient client)
        {
            this.Hide();
            ChatForm chatForm = new ChatForm(username, stream, client);
            chatForm.ShowDialog();
            client.Close();
            this.Show();
        }

        //private void StartChat(NetworkStream stream)
        //{
        //    TcpClient client = new TcpClient();
        //    client.Connect("34.118.84.47", 25564);

        //    NetworkStream stream = client.GetStream();

        //    Thread receiveThread = new Thread(() => ReceiveMessages(stream));
        //    receiveThread.Start();

        //    while (true)
        //    {
        //        string messageToSend = Console.ReadLine();
        //        byte[] dataToSend = Encoding.UTF8.GetBytes(messageToSend);
        //        stream.Write(dataToSend, 0, dataToSend.Length);

        //        if (messageToSend.ToLower() == "exit")
        //        {
        //            Console.WriteLine("The chat has finished.");
        //            break;
        //        }
        //    }

        //    stream.Close();
        //    client.Close();
        //}

        //private void ReceiveMessages(NetworkStream stream)
        //{
        //    byte[] buffer = new byte[1024];
        //    while (true)
        //    {
        //        int bytesread = stream.Read(buffer, 0, buffer.Length);
        //        string messagereceived = Encoding.UTF8.GetString(buffer, 0, bytesread);
        //        Console.WriteLine($"server: {messagereceived}");

        //        if (messagereceived.ToLower() == "exit")
        //        {
        //            Console.WriteLine("server has finished the chat.");
        //            break;
        //        }
        //    }
        //}
    }
}