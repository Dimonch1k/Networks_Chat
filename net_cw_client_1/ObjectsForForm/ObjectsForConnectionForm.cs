using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace net_cw_client_1
{
    public class ObjectsForConnectionForm
    {
        public Button btnConnect { get; set; }
        public TextBox txtIpAdress { get; set; }
        public Label lblFeedback { get; set; }

        public ObjectsForConnectionForm(Button btnConnect, TextBox txtIpAdress, Label lblFeedback)
        {
            this.btnConnect = btnConnect;
            this.txtIpAdress = txtIpAdress;
            this.lblFeedback = lblFeedback;
        }
    }
}
