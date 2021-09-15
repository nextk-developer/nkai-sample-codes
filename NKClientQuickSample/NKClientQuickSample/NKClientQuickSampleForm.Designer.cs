
namespace NKClientQuickSample
{
    partial class NKClientQuickSampleForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.tbUri = new System.Windows.Forms.TextBox();
            this.btnAPIConnection = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.rtbPayload = new System.Windows.Forms.RichTextBox();
            this.rtbResponse = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.rtbRpcResponse = new System.Windows.Forms.RichTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tbTCbaseUri = new System.Windows.Forms.TextBox();
            this.tbTChttpPort = new System.Windows.Forms.TextBox();
            this.label111 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.tbTCRpcPort = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbTCNodeIp = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.tbLastNodeId = new System.Windows.Forms.TextBox();
            this.tbLastChannelId = new System.Windows.Forms.TextBox();
            this.rbAddCompute = new System.Windows.Forms.RadioButton();
            this.rbAddChannel = new System.Windows.Forms.RadioButton();
            this.rbAddROI = new System.Windows.Forms.RadioButton();
            this.rbRemoveCompute = new System.Windows.Forms.RadioButton();
            this.rbRemoveChannel = new System.Windows.Forms.RadioButton();
            this.rbRemoveROI = new System.Windows.Forms.RadioButton();
            this.rbGetCompute = new System.Windows.Forms.RadioButton();
            this.label14 = new System.Windows.Forms.Label();
            this.rbGetChannel = new System.Windows.Forms.RadioButton();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.rbGetRoi = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.pbDrawBox = new System.Windows.Forms.PictureBox();
            this.lbDrawFrameChannelId = new System.Windows.Forms.Label();
            this.tbLastRoiId = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbDrawBox)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label1.Location = new System.Drawing.Point(550, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Setting";
            // 
            // tbUri
            // 
            this.tbUri.Location = new System.Drawing.Point(71, 27);
            this.tbUri.Name = "tbUri";
            this.tbUri.Size = new System.Drawing.Size(473, 23);
            this.tbUri.TabIndex = 6;
            // 
            // btnAPIConnection
            // 
            this.btnAPIConnection.Location = new System.Drawing.Point(9, 205);
            this.btnAPIConnection.Name = "btnAPIConnection";
            this.btnAPIConnection.Size = new System.Drawing.Size(535, 33);
            this.btnAPIConnection.TabIndex = 8;
            this.btnAPIConnection.Text = "Send";
            this.btnAPIConnection.UseVisualStyleBackColor = true;
            this.btnAPIConnection.Click += new System.EventHandler(this.btnAPIConnection_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label4.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label4.Location = new System.Drawing.Point(13, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 15);
            this.label4.TabIndex = 10;
            this.label4.Text = "Request API";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(15, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 21);
            this.label2.TabIndex = 11;
            this.label2.Text = "POST";
            // 
            // rtbPayload
            // 
            this.rtbPayload.Location = new System.Drawing.Point(9, 54);
            this.rtbPayload.Name = "rtbPayload";
            this.rtbPayload.Size = new System.Drawing.Size(535, 148);
            this.rtbPayload.TabIndex = 14;
            this.rtbPayload.Text = "";
            // 
            // rtbResponse
            // 
            this.rtbResponse.Location = new System.Drawing.Point(9, 271);
            this.rtbResponse.Name = "rtbResponse";
            this.rtbResponse.ReadOnly = true;
            this.rtbResponse.Size = new System.Drawing.Size(535, 151);
            this.rtbResponse.TabIndex = 15;
            this.rtbResponse.Text = "";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label3.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label3.Location = new System.Drawing.Point(9, 252);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 15);
            this.label3.TabIndex = 16;
            this.label3.Text = "Response API";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label5.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label5.Location = new System.Drawing.Point(9, 426);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 15);
            this.label5.TabIndex = 17;
            this.label5.Text = "Response Meta";
            // 
            // rtbRpcResponse
            // 
            this.rtbRpcResponse.Location = new System.Drawing.Point(550, 444);
            this.rtbRpcResponse.Name = "rtbRpcResponse";
            this.rtbRpcResponse.ReadOnly = true;
            this.rtbRpcResponse.Size = new System.Drawing.Size(320, 329);
            this.rtbRpcResponse.TabIndex = 18;
            this.rtbRpcResponse.Text = "";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(550, 342);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(311, 15);
            this.label8.TabIndex = 21;
            this.label8.Text = "* VA Method : RegistNode > CreateChannel > CreateRoi";
            // 
            // tbTCbaseUri
            // 
            this.tbTCbaseUri.Location = new System.Drawing.Point(670, 26);
            this.tbTCbaseUri.Name = "tbTCbaseUri";
            this.tbTCbaseUri.Size = new System.Drawing.Size(169, 23);
            this.tbTCbaseUri.TabIndex = 22;
            // 
            // tbTChttpPort
            // 
            this.tbTChttpPort.Location = new System.Drawing.Point(670, 103);
            this.tbTChttpPort.MaxLength = 8;
            this.tbTChttpPort.Name = "tbTChttpPort";
            this.tbTChttpPort.Size = new System.Drawing.Size(169, 23);
            this.tbTChttpPort.TabIndex = 24;
            this.tbTChttpPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textbox_KeyPress_OnlyNumberic);
            // 
            // label111
            // 
            this.label111.AutoSize = true;
            this.label111.Location = new System.Drawing.Point(604, 30);
            this.label111.Name = "label111";
            this.label111.Size = new System.Drawing.Size(60, 15);
            this.label111.TabIndex = 24;
            this.label111.Text = "Base URI :";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(602, 108);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(62, 15);
            this.label10.TabIndex = 25;
            this.label10.Text = "httpPort : ";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(607, 132);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(57, 15);
            this.label11.TabIndex = 27;
            this.label11.Text = "rpcPort : ";
            // 
            // tbTCRpcPort
            // 
            this.tbTCRpcPort.Location = new System.Drawing.Point(670, 128);
            this.tbTCRpcPort.MaxLength = 8;
            this.tbTCRpcPort.Name = "tbTCRpcPort";
            this.tbTCRpcPort.Size = new System.Drawing.Size(169, 23);
            this.tbTCRpcPort.TabIndex = 25;
            this.tbTCRpcPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textbox_KeyPress_OnlyNumberic);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(621, 82);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(43, 15);
            this.label9.TabIndex = 29;
            this.label9.Text = "Host : ";
            // 
            // tbTCNodeIp
            // 
            this.tbTCNodeIp.Location = new System.Drawing.Point(670, 78);
            this.tbTCNodeIp.Name = "tbTCNodeIp";
            this.tbTCNodeIp.Size = new System.Drawing.Size(169, 23);
            this.tbTCNodeIp.TabIndex = 23;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(591, 157);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(74, 15);
            this.label12.TabIndex = 31;
            this.label12.Text = "Current ID : ";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(562, 183);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(103, 15);
            this.label13.TabIndex = 32;
            this.label13.Text = "Last Channel ID : ";
            // 
            // tbLastNodeId
            // 
            this.tbLastNodeId.Location = new System.Drawing.Point(670, 154);
            this.tbLastNodeId.Name = "tbLastNodeId";
            this.tbLastNodeId.ReadOnly = true;
            this.tbLastNodeId.Size = new System.Drawing.Size(169, 23);
            this.tbLastNodeId.TabIndex = 26;
            // 
            // tbLastChannelId
            // 
            this.tbLastChannelId.Location = new System.Drawing.Point(670, 179);
            this.tbLastChannelId.Name = "tbLastChannelId";
            this.tbLastChannelId.ReadOnly = true;
            this.tbLastChannelId.Size = new System.Drawing.Size(169, 23);
            this.tbLastChannelId.TabIndex = 27;
            // 
            // rbAddCompute
            // 
            this.rbAddCompute.AutoSize = true;
            this.rbAddCompute.Location = new System.Drawing.Point(647, 259);
            this.rbAddCompute.Name = "rbAddCompute";
            this.rbAddCompute.Size = new System.Drawing.Size(47, 19);
            this.rbAddCompute.TabIndex = 35;
            this.rbAddCompute.TabStop = true;
            this.rbAddCompute.Text = "Add";
            this.rbAddCompute.UseVisualStyleBackColor = true;
            this.rbAddCompute.CheckedChanged += new System.EventHandler(this.rbAddCompute_CheckedChanged);
            // 
            // rbAddChannel
            // 
            this.rbAddChannel.AutoSize = true;
            this.rbAddChannel.Location = new System.Drawing.Point(647, 281);
            this.rbAddChannel.Name = "rbAddChannel";
            this.rbAddChannel.Size = new System.Drawing.Size(47, 19);
            this.rbAddChannel.TabIndex = 36;
            this.rbAddChannel.TabStop = true;
            this.rbAddChannel.Text = "Add";
            this.rbAddChannel.UseVisualStyleBackColor = true;
            this.rbAddChannel.CheckedChanged += new System.EventHandler(this.rbAddChannel_CheckedChanged);
            // 
            // rbAddROI
            // 
            this.rbAddROI.AutoSize = true;
            this.rbAddROI.Location = new System.Drawing.Point(647, 302);
            this.rbAddROI.Name = "rbAddROI";
            this.rbAddROI.Size = new System.Drawing.Size(47, 19);
            this.rbAddROI.TabIndex = 37;
            this.rbAddROI.TabStop = true;
            this.rbAddROI.Text = "Add";
            this.rbAddROI.UseVisualStyleBackColor = true;
            this.rbAddROI.CheckedChanged += new System.EventHandler(this.rbAddROI_CheckedChanged);
            // 
            // rbRemoveCompute
            // 
            this.rbRemoveCompute.AutoSize = true;
            this.rbRemoveCompute.Location = new System.Drawing.Point(753, 259);
            this.rbRemoveCompute.Name = "rbRemoveCompute";
            this.rbRemoveCompute.Size = new System.Drawing.Size(68, 19);
            this.rbRemoveCompute.TabIndex = 39;
            this.rbRemoveCompute.TabStop = true;
            this.rbRemoveCompute.Text = "Remove";
            this.rbRemoveCompute.UseVisualStyleBackColor = true;
            this.rbRemoveCompute.CheckedChanged += new System.EventHandler(this.rbRemoveCompute_CheckedChanged);
            // 
            // rbRemoveChannel
            // 
            this.rbRemoveChannel.AutoSize = true;
            this.rbRemoveChannel.Location = new System.Drawing.Point(753, 281);
            this.rbRemoveChannel.Name = "rbRemoveChannel";
            this.rbRemoveChannel.Size = new System.Drawing.Size(68, 19);
            this.rbRemoveChannel.TabIndex = 40;
            this.rbRemoveChannel.TabStop = true;
            this.rbRemoveChannel.Text = "Remove";
            this.rbRemoveChannel.UseVisualStyleBackColor = true;
            this.rbRemoveChannel.CheckedChanged += new System.EventHandler(this.rbRemoveChannel_CheckedChanged);
            // 
            // rbRemoveROI
            // 
            this.rbRemoveROI.AutoSize = true;
            this.rbRemoveROI.Location = new System.Drawing.Point(753, 301);
            this.rbRemoveROI.Name = "rbRemoveROI";
            this.rbRemoveROI.Size = new System.Drawing.Size(68, 19);
            this.rbRemoveROI.TabIndex = 41;
            this.rbRemoveROI.TabStop = true;
            this.rbRemoveROI.Text = "Remove";
            this.rbRemoveROI.UseVisualStyleBackColor = true;
            this.rbRemoveROI.CheckedChanged += new System.EventHandler(this.rbRemoveROI_CheckedChanged);
            // 
            // rbGetCompute
            // 
            this.rbGetCompute.AutoSize = true;
            this.rbGetCompute.Location = new System.Drawing.Point(701, 259);
            this.rbGetCompute.Name = "rbGetCompute";
            this.rbGetCompute.Size = new System.Drawing.Size(43, 19);
            this.rbGetCompute.TabIndex = 42;
            this.rbGetCompute.TabStop = true;
            this.rbGetCompute.Text = "Get";
            this.rbGetCompute.UseVisualStyleBackColor = true;
            this.rbGetCompute.CheckedChanged += new System.EventHandler(this.rbGetCompute_CheckedChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(550, 261);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(93, 15);
            this.label14.TabIndex = 43;
            this.label14.Text = "ComputeNode :";
            // 
            // rbGetChannel
            // 
            this.rbGetChannel.AutoSize = true;
            this.rbGetChannel.Location = new System.Drawing.Point(701, 281);
            this.rbGetChannel.Name = "rbGetChannel";
            this.rbGetChannel.Size = new System.Drawing.Size(43, 19);
            this.rbGetChannel.TabIndex = 44;
            this.rbGetChannel.TabStop = true;
            this.rbGetChannel.Text = "Get";
            this.rbGetChannel.UseVisualStyleBackColor = true;
            this.rbGetChannel.CheckedChanged += new System.EventHandler(this.rbGetChannel_CheckedChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(585, 283);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(58, 15);
            this.label15.TabIndex = 45;
            this.label15.Text = "Channel :";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(612, 303);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(31, 15);
            this.label16.TabIndex = 46;
            this.label16.Text = "Roi :";
            // 
            // rbGetRoi
            // 
            this.rbGetRoi.AutoSize = true;
            this.rbGetRoi.Location = new System.Drawing.Point(701, 301);
            this.rbGetRoi.Name = "rbGetRoi";
            this.rbGetRoi.Size = new System.Drawing.Size(43, 19);
            this.rbGetRoi.TabIndex = 47;
            this.rbGetRoi.TabStop = true;
            this.rbGetRoi.Text = "Get";
            this.rbGetRoi.UseVisualStyleBackColor = true;
            this.rbGetRoi.CheckedChanged += new System.EventHandler(this.rbGetRoi_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label6.Location = new System.Drawing.Point(550, 54);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 15);
            this.label6.TabIndex = 48;
            this.label6.Text = "* ComputeNode";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label7.Location = new System.Drawing.Point(553, 235);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(70, 15);
            this.label7.TabIndex = 49;
            this.label7.Text = "* Select API";
            // 
            // pbDrawBox
            // 
            this.pbDrawBox.BackColor = System.Drawing.Color.Black;
            this.pbDrawBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pbDrawBox.Location = new System.Drawing.Point(9, 444);
            this.pbDrawBox.Name = "pbDrawBox";
            this.pbDrawBox.Size = new System.Drawing.Size(535, 329);
            this.pbDrawBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbDrawBox.TabIndex = 50;
            this.pbDrawBox.TabStop = false;
            // 
            // lbDrawFrameChannelId
            // 
            this.lbDrawFrameChannelId.AutoSize = true;
            this.lbDrawFrameChannelId.BackColor = System.Drawing.SystemColors.Highlight;
            this.lbDrawFrameChannelId.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lbDrawFrameChannelId.ForeColor = System.Drawing.Color.White;
            this.lbDrawFrameChannelId.Location = new System.Drawing.Point(453, 731);
            this.lbDrawFrameChannelId.Name = "lbDrawFrameChannelId";
            this.lbDrawFrameChannelId.Size = new System.Drawing.Size(59, 21);
            this.lbDrawFrameChannelId.TabIndex = 54;
            this.lbDrawFrameChannelId.Text = "empty";
            // 
            // tbLastRoiId
            // 
            this.tbLastRoiId.Location = new System.Drawing.Point(670, 204);
            this.tbLastRoiId.Name = "tbLastRoiId";
            this.tbLastRoiId.ReadOnly = true;
            this.tbLastRoiId.Size = new System.Drawing.Size(169, 23);
            this.tbLastRoiId.TabIndex = 55;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(589, 208);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(76, 15);
            this.label17.TabIndex = 56;
            this.label17.Text = "Last Roi ID : ";
            // 
            // NKClientQuickSampleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(873, 775);
            this.Controls.Add(this.tbLastRoiId);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.lbDrawFrameChannelId);
            this.Controls.Add(this.pbDrawBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.rbGetRoi);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.rbGetChannel);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.rbGetCompute);
            this.Controls.Add(this.rbRemoveROI);
            this.Controls.Add(this.rbRemoveChannel);
            this.Controls.Add(this.rbRemoveCompute);
            this.Controls.Add(this.rbAddROI);
            this.Controls.Add(this.rbAddChannel);
            this.Controls.Add(this.rbAddCompute);
            this.Controls.Add(this.tbLastChannelId);
            this.Controls.Add(this.tbLastNodeId);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.tbTCNodeIp);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.tbTCRpcPort);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label111);
            this.Controls.Add(this.tbTChttpPort);
            this.Controls.Add(this.tbTCbaseUri);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.rtbRpcResponse);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.rtbResponse);
            this.Controls.Add(this.rtbPayload);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnAPIConnection);
            this.Controls.Add(this.tbUri);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "NKClientQuickSampleForm";
            this.Text = "NKClientQuickSample";
            ((System.ComponentModel.ISupportInitialize)(this.pbDrawBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbUri;
        private System.Windows.Forms.Button btnAPIConnection;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox rtbPayload;
        private System.Windows.Forms.RichTextBox rtbResponse;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RichTextBox rtbRpcResponse;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbTCbaseUri;
        private System.Windows.Forms.TextBox tbTChttpPort;
        private System.Windows.Forms.Label label111;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tbTCRpcPort;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbTCNodeIp;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox tbLastNodeId;
        private System.Windows.Forms.TextBox tbLastChannelId;
        private System.Windows.Forms.RadioButton rbAddCompute;
        private System.Windows.Forms.RadioButton rbAddChannel;
        private System.Windows.Forms.RadioButton rbAddROI;
        private System.Windows.Forms.RadioButton rbRemoveCompute;
        private System.Windows.Forms.RadioButton rbRemoveChannel;
        private System.Windows.Forms.RadioButton rbRemoveROI;
        private System.Windows.Forms.RadioButton rbGetCompute;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.RadioButton rbGetChannel;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.RadioButton rbGetRoi;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.PictureBox pbDrawBox;
        private System.Windows.Forms.Label lbDrawFrameChannelId;
        private System.Windows.Forms.TextBox tbLastRoiId;
        private System.Windows.Forms.Label label17;
    }
}

