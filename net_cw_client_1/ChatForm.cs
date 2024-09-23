using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace net_cw_client_1
{
    public partial class ChatForm : Form
    {
        private string username;
        private NetworkStream stream;
        private TcpClient client;
        private Thread listenThread;

        public ChatForm(string username, NetworkStream stream, TcpClient client)
        {
            InitializeComponent();

            this.username = username;
            this.stream = stream;
            this.client = client;

            listenThread = new Thread(ListenForMessages);
            listenThread.Start();
        }

        private void btnSend_Click(object sender, System.EventArgs e)
        {
            MessageInputDialog inputDialog = new MessageInputDialog();
            inputDialog.ShowDialog();
            if (inputDialog.DialogResult == DialogResult.OK)
            {
                string message = inputDialog.MessageText;
                SendMessage(message);
            }
        }

        private void SendMessage(string message)
        {
            try
            {
                string formattedMessage = $"{username}: {message}";
                byte[] data = Encoding.UTF8.GetBytes(formattedMessage);
                stream.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending message: {ex.Message}", "Error", MessageBoxButtons.OK);
            }
        }

        private void ListenForMessages()
        {
            byte[] buffer = new byte[1024];
            while (true)
            {
                try
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string messageReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    DisplayMessage(messageReceived);
                }
                catch (Exception)
                {
                    this.Invoke((MethodInvoker)delegate { this.Close(); });
                }
            }
        }

        private void DisplayMessage(string message)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(DisplayMessage), message);
                return;
            }
            messageHistory.AppendText(message + Environment.NewLine);
        }


        private void ChatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            stream.Close();
            client.Close();
            listenThread.Abort();
        }
    }
}
