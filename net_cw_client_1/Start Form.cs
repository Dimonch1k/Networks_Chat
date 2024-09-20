using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace net_cw_client_1
{
    public partial class Start_Form : Form
    {
        string initialIpAdressText = "input IP Adress";
        MyClient myClient;
        public Start_Form()
        {
            InitializeComponent();
            myClient = new MyClient();
        }

        private void connectBtn_Click(object sender, EventArgs e)
        {

        }


    }
}
