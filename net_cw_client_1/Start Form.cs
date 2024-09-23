using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace net_cw_client_1
{
    public partial class Start_Form : Form
    {
        string initialIpAdressText = "34.118.84.47";
        Button btnConnect;
        TextBox txtIPAddress;
        Label lblFeedback;
        TextBox txtUsername;
        TextBox txtPassword;
        Button btnLogin;
        ListBox lstMessages;
        TextBox txtMessage;
        Button btnSend;
        TcpClient tcpClient;
        NetworkStream stream;
        /// <ObjectsForForm>
        ObjectsForConnectionForm objectsForConnectionForm;

        /// </ObjectsForForm>
        int port = 25564;
        public Start_Form()
        {
            InitializeComponent();
            InitializeComponentPart2();
            StartConnection();



        }
        private void StartConnection()
        {
            Connection connection = new Connection(this, objectsForConnectionForm, port);
        }
        private void InitializeComponentPart2()
        {
            this.txtIPAddress = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.lblFeedback = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.lstMessages = new System.Windows.Forms.ListBox();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();

            // 
            // txtIPAddress
            // 
            this.txtIPAddress.Location = new System.Drawing.Point(20, 20);
            this.txtIPAddress.Size = new System.Drawing.Size(200, 20);
            this. txtIPAddress.Visible = false;

            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(230, 20);
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.Text = "Connect";
            this.btnConnect.Visible = false;

            // 
            // lblFeedback
            // 
            this.lblFeedback.Location = new System.Drawing.Point(20, 50);
            this.lblFeedback.Size = new System.Drawing.Size(300, 20);
            this.lblFeedback.Visible = false;

            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(20, 80);
            this.txtUsername.Size = new System.Drawing.Size(200, 20);
            this.txtUsername.Visible = false; // Initially hidden

            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(20, 110);
            this.txtPassword.Size = new System.Drawing.Size(200, 20);
            this.txtPassword.UseSystemPasswordChar = true; // Hide password input
            this.txtPassword.Visible = false; // Initially hidden

            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(230, 110);
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.Text = "Login";
            this.btnLogin.Visible = false; // Initially hidden

            // 
            // lstMessages
            // 
            this.lstMessages.Location = new System.Drawing.Point(20, 140);
            this.lstMessages.Size = new System.Drawing.Size(300, 100);
            this.lstMessages.Visible = false; // Initially hidden

            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(20, 250);
            this.txtMessage.Size = new System.Drawing.Size(200, 20);
            this.txtMessage.Visible = false; // Initially hidden

            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(230, 250);
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.Text = "Send";
            this.btnSend.Visible = false; // Initially hidden

            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(350, 300);
            this.Controls.Add(this.txtIPAddress);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.lblFeedback);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.lstMessages);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.btnSend);

            objectsForConnectionForm = new ObjectsForConnectionForm(connectBtn, txtIPAddress, lblFeedback);
        }
   


    }
}
