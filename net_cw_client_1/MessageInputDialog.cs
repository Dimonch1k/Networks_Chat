using System;
using System.Windows.Forms;

namespace net_cw_client_1
{
    public partial class MessageInputDialog : Form
    {
        public string MessageText { get; private set; }

        public MessageInputDialog()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            MessageText = messageTB.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
