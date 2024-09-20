using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace net_cw_client_1
{
    public partial class ChatForm : Form
    {
        private TcpClient client;
        private NetworkStream stream;
        private string username;

        public ChatForm(string username)
        {
            InitializeComponent();
            this.username = username;

            // Connect to server
            client = new TcpClient("127.0.0.1", 25564); // Adjust IP and port as needed
            stream = client.GetStream();
            BeginReceivingMessages();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string message = txtMessage.Text;
            if (!string.IsNullOrEmpty(message))
            {
                byte[] data = Encoding.UTF8.GetBytes($"{username}: {message}");
                stream.Write(data, 0, data.Length);
                txtMessage.Clear();
            }
        }

        private void BeginReceivingMessages()
        {
            var receiveThread = new Thread(() =>
            {
                byte[] buffer = new byte[1024];
                while (true)
                {
                    try
                    {
                        int bytesRead = stream.Read(buffer, 0, buffer.Length);
                        if (bytesRead == 0) break;

                        string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        this.Invoke(new Action(() => lstMessages.Items.Add(message)));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error receiving message: {ex.Message}");
                        break;
                    }
                }
            });
            receiveThread.Start();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            stream.Close();
            client.Close();
        }
    }
}
