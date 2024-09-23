namespace net_cw_client_2
{
    partial class MessageInputDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            messageTB = new TextBox();
            btnSend = new Button();
            btnCancel = new Button();
            SuspendLayout();
            // 
            // messageTB
            // 
            messageTB.Dock = DockStyle.Top;
            messageTB.Location = new Point(0, 0);
            messageTB.Margin = new Padding(4, 3, 4, 3);
            messageTB.Multiline = true;
            messageTB.Name = "messageTB";
            messageTB.ScrollBars = ScrollBars.Vertical;
            messageTB.Size = new Size(260, 93);
            messageTB.TabIndex = 0;
            // 
            // btnSend
            // 
            btnSend.BackColor = Color.PaleGreen;
            btnSend.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnSend.ForeColor = SystemColors.ControlText;
            btnSend.Location = new Point(13, 100);
            btnSend.Margin = new Padding(4, 3, 4, 3);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(113, 36);
            btnSend.TabIndex = 1;
            btnSend.Text = "Send";
            btnSend.UseVisualStyleBackColor = false;
            btnSend.Click += btnSend_Click;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.LightCoral;
            btnCancel.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCancel.Location = new Point(133, 100);
            btnCancel.Margin = new Padding(4, 3, 4, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(113, 36);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // MessageInputDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(260, 145);
            Controls.Add(btnCancel);
            Controls.Add(btnSend);
            Controls.Add(messageTB);
            Margin = new Padding(4, 3, 4, 3);
            Name = "MessageInputDialog";
            Text = "MessageInputDialog";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox messageTB;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnCancel;
    }
}