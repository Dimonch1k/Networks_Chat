using MySqlX.XDevAPI;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace net_cw_client_1
{
    public class Connection
    {
        Form connectionForm;
        ObjectsForConnectionForm objectsForConnectionForm;
        TcpClient tcpClient;
        NetworkStream stream;
        string lastConnectionStatus;
        Button connectBtn;
        Label lblFeedback;
        TextBox txtIPAdress;
        int port;

        public Connection(Form form, ObjectsForConnectionForm objectsForConnectionForm, int port) 
        {
            connectionForm = form;
            this.objectsForConnectionForm = objectsForConnectionForm;
            this.port = port;
            this.connectBtn =  objectsForConnectionForm.btnConnect;
            this.lblFeedback = objectsForConnectionForm.lblFeedback;
            this.txtIPAdress = objectsForConnectionForm.txtIpAdress;
            connectBtn.Click += btnConnect_Click;
            this.tcpClient = tcpClient;
            this.stream = stream;
         
        }
        private void  hideFormElements()
        {
            this.connectBtn.Visible = false;
            this.txtIPAdress.Visible = false;
            this.lblFeedback.Visible = false;
        }
        private void showFormElements()
        {
            txtIPAdress.Visible = true;
            connectBtn.Visible = true;
            lblFeedback.Visible = true;
        }
        private void btnConnect_Click(object sender, EventArgs e)
        {
            string ipAddress = txtIPAdress.Text;

            if (IPAddress.TryParse(ipAddress, out _))
            {
                try
                {
                    tcpClient = new TcpClient(ipAddress, this.port); // Attempt to connect to the server
                    stream = tcpClient.GetStream(); // Get the network stream

                    if (tcpClient.Connected)
                    {
                        lastConnectionStatus = "Connected to the server!";

                        // Show login fields
                        txtUsername.Visible = true;
                        txtPassword.Visible = true;
                        btnLogin.Visible = true;
                    }
                }
                catch (SocketException)
                {
                    lastConnectionStatus = "Could not connect to server. Please check the IP address and port.";
                }
                catch (Exception ex)
                {
                    lastConnectionStatus = "Error: " + ex.Message;
                }
            }
            else
            {
                lastConnectionStatus = "Invalid IP Address.";
            }
        }
    }
}
