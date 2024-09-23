namespace net_cw_client_2
{
    partial class ChatForm
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
            this.messageHistory = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // messageHistory
            // 
            this.messageHistory.Dock = System.Windows.Forms.DockStyle.Top;
            this.messageHistory.Location = new System.Drawing.Point(0, 0);
            this.messageHistory.Multiline = true;
            this.messageHistory.Name = "messageHistory";
            this.messageHistory.ReadOnly = true;
            this.messageHistory.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.messageHistory.Size = new System.Drawing.Size(400, 354);
            this.messageHistory.TabIndex = 1;
            // 
            // btnSend
            // 
            this.btnSend.BackColor = System.Drawing.Color.BlanchedAlmond;
            this.btnSend.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnSend.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSend.Location = new System.Drawing.Point(0, 355);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(400, 44);
            this.btnSend.TabIndex = 2;
            this.btnSend.Text = "Send message";
            this.btnSend.UseVisualStyleBackColor = false;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // ChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 399);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.messageHistory);
            this.Name = "ChatForm";
            this.Text = "ChatForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChatForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox messageHistory;
        private System.Windows.Forms.Button btnSend;
    }
}