namespace myPostman
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.splitContainerLeft = new System.Windows.Forms.SplitContainer();
            this.groupBoxRequest = new System.Windows.Forms.GroupBox();
            this.chkIgnoreCertificate = new System.Windows.Forms.CheckBox();
            this.chkFormatJson = new System.Windows.Forms.CheckBox();
            this.lblTimeout = new System.Windows.Forms.Label();
            this.txtTimeout = new System.Windows.Forms.TextBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.tabControlRequest = new System.Windows.Forms.TabControl();
            this.tabPageHeaders = new System.Windows.Forms.TabPage();
            this.txtHeaders = new System.Windows.Forms.TextBox();
            this.tabPageBody = new System.Windows.Forms.TabPage();
            this.txtBody = new System.Windows.Forms.TextBox();
            this.cmbMethod = new System.Windows.Forms.ComboBox();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.lblUrl = new System.Windows.Forms.Label();
            this.groupBoxSavedRequests = new System.Windows.Forms.GroupBox();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.listBoxSavedRequests = new System.Windows.Forms.ListBox();
            this.groupBoxResponse = new System.Windows.Forms.GroupBox();
            this.tabControlResponse = new System.Windows.Forms.TabControl();
            this.tabPageResponseBody = new System.Windows.Forms.TabPage();
            this.txtResponse = new System.Windows.Forms.TextBox();
            this.tabPageResponseHeaders = new System.Windows.Forms.TabPage();
            this.txtResponseHeaders = new System.Windows.Forms.TextBox();
            this.panelStatus = new System.Windows.Forms.Panel();
            this.btnCopyResponse = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.splitContainerLeft.Panel1.SuspendLayout();
            this.splitContainerLeft.Panel2.SuspendLayout();
            this.splitContainerLeft.SuspendLayout();
            this.groupBoxRequest.SuspendLayout();
            this.tabControlRequest.SuspendLayout();
            this.tabPageHeaders.SuspendLayout();
            this.tabPageBody.SuspendLayout();
            this.groupBoxSavedRequests.SuspendLayout();
            this.groupBoxResponse.SuspendLayout();
            this.tabControlResponse.SuspendLayout();
            this.tabPageResponseBody.SuspendLayout();
            this.tabPageResponseHeaders.SuspendLayout();
            this.panelStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.splitContainerLeft);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.groupBoxResponse);
            this.splitContainerMain.Size = new System.Drawing.Size(1000, 600);
            this.splitContainerMain.SplitterDistance = 450;
            this.splitContainerMain.TabIndex = 0;
            // 
            // splitContainerLeft
            // 
            this.splitContainerLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerLeft.Location = new System.Drawing.Point(0, 0);
            this.splitContainerLeft.Name = "splitContainerLeft";
            this.splitContainerLeft.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerLeft.Panel1
            // 
            this.splitContainerLeft.Panel1.Controls.Add(this.groupBoxRequest);
            // 
            // splitContainerLeft.Panel2
            // 
            this.splitContainerLeft.Panel2.Controls.Add(this.groupBoxSavedRequests);
            this.splitContainerLeft.Size = new System.Drawing.Size(450, 600);
            this.splitContainerLeft.SplitterDistance = 380;
            this.splitContainerLeft.TabIndex = 0;
            // 
            // groupBoxRequest
            // 
            this.groupBoxRequest.Controls.Add(this.chkIgnoreCertificate);
            this.groupBoxRequest.Controls.Add(this.chkFormatJson);
            this.groupBoxRequest.Controls.Add(this.lblTimeout);
            this.groupBoxRequest.Controls.Add(this.txtTimeout);
            this.groupBoxRequest.Controls.Add(this.btnClear);
            this.groupBoxRequest.Controls.Add(this.btnSave);
            this.groupBoxRequest.Controls.Add(this.btnSend);
            this.groupBoxRequest.Controls.Add(this.tabControlRequest);
            this.groupBoxRequest.Controls.Add(this.cmbMethod);
            this.groupBoxRequest.Controls.Add(this.txtUrl);
            this.groupBoxRequest.Controls.Add(this.lblUrl);
            this.groupBoxRequest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxRequest.Location = new System.Drawing.Point(0, 0);
            this.groupBoxRequest.Name = "groupBoxRequest";
            this.groupBoxRequest.Size = new System.Drawing.Size(450, 380);
            this.groupBoxRequest.TabIndex = 0;
            this.groupBoxRequest.TabStop = false;
            this.groupBoxRequest.Text = "請求 / Request";
            // 
            // chkIgnoreCertificate
            // 
            this.chkIgnoreCertificate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkIgnoreCertificate.AutoSize = true;
            this.chkIgnoreCertificate.Location = new System.Drawing.Point(120, 352);
            this.chkIgnoreCertificate.Name = "chkIgnoreCertificate";
            this.chkIgnoreCertificate.Size = new System.Drawing.Size(150, 17);
            this.chkIgnoreCertificate.TabIndex = 10;
            this.chkIgnoreCertificate.Text = "忽略SSL憑證 / Ignore SSL";
            this.chkIgnoreCertificate.UseVisualStyleBackColor = true;
            this.chkIgnoreCertificate.CheckedChanged += new System.EventHandler(this.chkIgnoreCertificate_CheckedChanged);
            // 
            // chkFormatJson
            // 
            this.chkFormatJson.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkFormatJson.AutoSize = true;
            this.chkFormatJson.Checked = true;
            this.chkFormatJson.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFormatJson.Location = new System.Drawing.Point(340, 48);
            this.chkFormatJson.Name = "chkFormatJson";
            this.chkFormatJson.Size = new System.Drawing.Size(100, 17);
            this.chkFormatJson.TabIndex = 9;
            this.chkFormatJson.Text = "格式化 JSON";
            this.chkFormatJson.UseVisualStyleBackColor = true;
            // 
            // lblTimeout
            // 
            this.lblTimeout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTimeout.AutoSize = true;
            this.lblTimeout.Location = new System.Drawing.Point(230, 50);
            this.lblTimeout.Name = "lblTimeout";
            this.lblTimeout.Size = new System.Drawing.Size(47, 13);
            this.lblTimeout.TabIndex = 8;
            this.lblTimeout.Text = "逾時(秒):";
            // 
            // txtTimeout
            // 
            this.txtTimeout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTimeout.Location = new System.Drawing.Point(283, 47);
            this.txtTimeout.Name = "txtTimeout";
            this.txtTimeout.Size = new System.Drawing.Size(50, 20);
            this.txtTimeout.TabIndex = 7;
            this.txtTimeout.Text = "30";
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.Location = new System.Drawing.Point(365, 346);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 28);
            this.btnClear.TabIndex = 6;
            this.btnClear.Text = "清除 / Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(284, 346);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 28);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "儲存 / Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSend.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSend.Location = new System.Drawing.Point(10, 346);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(100, 28);
            this.btnSend.TabIndex = 4;
            this.btnSend.Text = "發送 / Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // tabControlRequest
            // 
            this.tabControlRequest.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlRequest.Controls.Add(this.tabPageHeaders);
            this.tabControlRequest.Controls.Add(this.tabPageBody);
            this.tabControlRequest.Location = new System.Drawing.Point(10, 73);
            this.tabControlRequest.Name = "tabControlRequest";
            this.tabControlRequest.SelectedIndex = 0;
            this.tabControlRequest.Size = new System.Drawing.Size(430, 267);
            this.tabControlRequest.TabIndex = 3;
            // 
            // tabPageHeaders
            // 
            this.tabPageHeaders.Controls.Add(this.txtHeaders);
            this.tabPageHeaders.Location = new System.Drawing.Point(4, 22);
            this.tabPageHeaders.Name = "tabPageHeaders";
            this.tabPageHeaders.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageHeaders.Size = new System.Drawing.Size(422, 241);
            this.tabPageHeaders.TabIndex = 0;
            this.tabPageHeaders.Text = "Headers";
            this.tabPageHeaders.UseVisualStyleBackColor = true;
            // 
            // txtHeaders
            // 
            this.txtHeaders.AcceptsReturn = true;
            this.txtHeaders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtHeaders.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHeaders.Location = new System.Drawing.Point(3, 3);
            this.txtHeaders.Multiline = true;
            this.txtHeaders.Name = "txtHeaders";
            this.txtHeaders.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtHeaders.Size = new System.Drawing.Size(416, 235);
            this.txtHeaders.TabIndex = 0;
            this.txtHeaders.Text = "Content-Type: application/json";
            // 
            // tabPageBody
            // 
            this.tabPageBody.Controls.Add(this.txtBody);
            this.tabPageBody.Location = new System.Drawing.Point(4, 22);
            this.tabPageBody.Name = "tabPageBody";
            this.tabPageBody.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageBody.Size = new System.Drawing.Size(422, 241);
            this.tabPageBody.TabIndex = 1;
            this.tabPageBody.Text = "Body";
            this.tabPageBody.UseVisualStyleBackColor = true;
            // 
            // txtBody
            // 
            this.txtBody.AcceptsReturn = true;
            this.txtBody.AcceptsTab = true;
            this.txtBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBody.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBody.Location = new System.Drawing.Point(3, 3);
            this.txtBody.Multiline = true;
            this.txtBody.Name = "txtBody";
            this.txtBody.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtBody.Size = new System.Drawing.Size(416, 235);
            this.txtBody.TabIndex = 0;
            // 
            // cmbMethod
            // 
            this.cmbMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMethod.FormattingEnabled = true;
            this.cmbMethod.Items.AddRange(new object[] {
            "GET",
            "POST",
            "PUT",
            "DELETE",
            "PATCH",
            "HEAD",
            "OPTIONS"});
            this.cmbMethod.Location = new System.Drawing.Point(10, 45);
            this.cmbMethod.Name = "cmbMethod";
            this.cmbMethod.Size = new System.Drawing.Size(80, 21);
            this.cmbMethod.TabIndex = 2;
            // 
            // txtUrl
            // 
            this.txtUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUrl.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUrl.Location = new System.Drawing.Point(96, 45);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(128, 22);
            this.txtUrl.TabIndex = 1;
            // 
            // lblUrl
            // 
            this.lblUrl.AutoSize = true;
            this.lblUrl.Location = new System.Drawing.Point(7, 25);
            this.lblUrl.Name = "lblUrl";
            this.lblUrl.Size = new System.Drawing.Size(32, 13);
            this.lblUrl.TabIndex = 0;
            this.lblUrl.Text = "URL:";
            // 
            // groupBoxSavedRequests
            // 
            this.groupBoxSavedRequests.Controls.Add(this.btnImport);
            this.groupBoxSavedRequests.Controls.Add(this.btnExport);
            this.groupBoxSavedRequests.Controls.Add(this.btnDelete);
            this.groupBoxSavedRequests.Controls.Add(this.btnLoad);
            this.groupBoxSavedRequests.Controls.Add(this.listBoxSavedRequests);
            this.groupBoxSavedRequests.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxSavedRequests.Location = new System.Drawing.Point(0, 0);
            this.groupBoxSavedRequests.Name = "groupBoxSavedRequests";
            this.groupBoxSavedRequests.Size = new System.Drawing.Size(450, 216);
            this.groupBoxSavedRequests.TabIndex = 0;
            this.groupBoxSavedRequests.TabStop = false;
            this.groupBoxSavedRequests.Text = "已儲存的請求 / Saved Requests";
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Location = new System.Drawing.Point(365, 182);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(75, 28);
            this.btnImport.TabIndex = 4;
            this.btnImport.Text = "匯入 / Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Location = new System.Drawing.Point(284, 182);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 28);
            this.btnExport.TabIndex = 3;
            this.btnExport.Text = "匯出 / Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Location = new System.Drawing.Point(91, 182);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 28);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "刪除 / Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLoad.Location = new System.Drawing.Point(10, 182);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 28);
            this.btnLoad.TabIndex = 1;
            this.btnLoad.Text = "載入 / Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // listBoxSavedRequests
            // 
            this.listBoxSavedRequests.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxSavedRequests.FormattingEnabled = true;
            this.listBoxSavedRequests.Location = new System.Drawing.Point(10, 20);
            this.listBoxSavedRequests.Name = "listBoxSavedRequests";
            this.listBoxSavedRequests.Size = new System.Drawing.Size(430, 147);
            this.listBoxSavedRequests.TabIndex = 0;
            this.listBoxSavedRequests.DoubleClick += new System.EventHandler(this.listBoxSavedRequests_DoubleClick);
            // 
            // groupBoxResponse
            // 
            this.groupBoxResponse.Controls.Add(this.tabControlResponse);
            this.groupBoxResponse.Controls.Add(this.panelStatus);
            this.groupBoxResponse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxResponse.Location = new System.Drawing.Point(0, 0);
            this.groupBoxResponse.Name = "groupBoxResponse";
            this.groupBoxResponse.Size = new System.Drawing.Size(546, 600);
            this.groupBoxResponse.TabIndex = 0;
            this.groupBoxResponse.TabStop = false;
            this.groupBoxResponse.Text = "回應 / Response";
            // 
            // tabControlResponse
            // 
            this.tabControlResponse.Controls.Add(this.tabPageResponseBody);
            this.tabControlResponse.Controls.Add(this.tabPageResponseHeaders);
            this.tabControlResponse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlResponse.Location = new System.Drawing.Point(3, 56);
            this.tabControlResponse.Name = "tabControlResponse";
            this.tabControlResponse.SelectedIndex = 0;
            this.tabControlResponse.Size = new System.Drawing.Size(540, 541);
            this.tabControlResponse.TabIndex = 1;
            // 
            // tabPageResponseBody
            // 
            this.tabPageResponseBody.Controls.Add(this.txtResponse);
            this.tabPageResponseBody.Location = new System.Drawing.Point(4, 22);
            this.tabPageResponseBody.Name = "tabPageResponseBody";
            this.tabPageResponseBody.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageResponseBody.Size = new System.Drawing.Size(532, 515);
            this.tabPageResponseBody.TabIndex = 0;
            this.tabPageResponseBody.Text = "Body";
            this.tabPageResponseBody.UseVisualStyleBackColor = true;
            // 
            // txtResponse
            // 
            this.txtResponse.BackColor = System.Drawing.SystemColors.Window;
            this.txtResponse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtResponse.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtResponse.Location = new System.Drawing.Point(3, 3);
            this.txtResponse.Multiline = true;
            this.txtResponse.Name = "txtResponse";
            this.txtResponse.ReadOnly = true;
            this.txtResponse.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtResponse.Size = new System.Drawing.Size(526, 509);
            this.txtResponse.TabIndex = 0;
            // 
            // tabPageResponseHeaders
            // 
            this.tabPageResponseHeaders.Controls.Add(this.txtResponseHeaders);
            this.tabPageResponseHeaders.Location = new System.Drawing.Point(4, 22);
            this.tabPageResponseHeaders.Name = "tabPageResponseHeaders";
            this.tabPageResponseHeaders.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageResponseHeaders.Size = new System.Drawing.Size(532, 515);
            this.tabPageResponseHeaders.TabIndex = 1;
            this.tabPageResponseHeaders.Text = "Headers";
            this.tabPageResponseHeaders.UseVisualStyleBackColor = true;
            // 
            // txtResponseHeaders
            // 
            this.txtResponseHeaders.BackColor = System.Drawing.SystemColors.Window;
            this.txtResponseHeaders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtResponseHeaders.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtResponseHeaders.Location = new System.Drawing.Point(3, 3);
            this.txtResponseHeaders.Multiline = true;
            this.txtResponseHeaders.Name = "txtResponseHeaders";
            this.txtResponseHeaders.ReadOnly = true;
            this.txtResponseHeaders.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtResponseHeaders.Size = new System.Drawing.Size(526, 509);
            this.txtResponseHeaders.TabIndex = 0;
            // 
            // panelStatus
            // 
            this.panelStatus.Controls.Add(this.btnCopyResponse);
            this.panelStatus.Controls.Add(this.lblStatus);
            this.panelStatus.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelStatus.Location = new System.Drawing.Point(3, 16);
            this.panelStatus.Name = "panelStatus";
            this.panelStatus.Size = new System.Drawing.Size(540, 40);
            this.panelStatus.TabIndex = 0;
            // 
            // btnCopyResponse
            // 
            this.btnCopyResponse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopyResponse.Location = new System.Drawing.Point(462, 6);
            this.btnCopyResponse.Name = "btnCopyResponse";
            this.btnCopyResponse.Size = new System.Drawing.Size(75, 28);
            this.btnCopyResponse.TabIndex = 1;
            this.btnCopyResponse.Text = "複製 / Copy";
            this.btnCopyResponse.UseVisualStyleBackColor = true;
            this.btnCopyResponse.Click += new System.EventHandler(this.btnCopyResponse_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(7, 13);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(144, 13);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "狀態 / Status: 準備就緒 / Ready";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Controls.Add(this.splitContainerMain);
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "myPostman - 簡易 HTTP 請求工具 / Simple HTTP Request Tool";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            this.splitContainerMain.ResumeLayout(false);
            this.splitContainerLeft.Panel1.ResumeLayout(false);
            this.splitContainerLeft.Panel2.ResumeLayout(false);
            this.splitContainerLeft.ResumeLayout(false);
            this.groupBoxRequest.ResumeLayout(false);
            this.groupBoxRequest.PerformLayout();
            this.tabControlRequest.ResumeLayout(false);
            this.tabPageHeaders.ResumeLayout(false);
            this.tabPageHeaders.PerformLayout();
            this.tabPageBody.ResumeLayout(false);
            this.tabPageBody.PerformLayout();
            this.groupBoxSavedRequests.ResumeLayout(false);
            this.groupBoxResponse.ResumeLayout(false);
            this.tabControlResponse.ResumeLayout(false);
            this.tabPageResponseBody.ResumeLayout(false);
            this.tabPageResponseBody.PerformLayout();
            this.tabPageResponseHeaders.ResumeLayout(false);
            this.tabPageResponseHeaders.PerformLayout();
            this.panelStatus.ResumeLayout(false);
            this.panelStatus.PerformLayout();
            this.ResumeLayout(false);

            // Set default selection
            this.cmbMethod.SelectedIndex = 0;
        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.SplitContainer splitContainerLeft;
        private System.Windows.Forms.GroupBox groupBoxRequest;
        private System.Windows.Forms.Label lblUrl;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.ComboBox cmbMethod;
        private System.Windows.Forms.TabControl tabControlRequest;
        private System.Windows.Forms.TabPage tabPageHeaders;
        private System.Windows.Forms.TextBox txtHeaders;
        private System.Windows.Forms.TabPage tabPageBody;
        private System.Windows.Forms.TextBox txtBody;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label lblTimeout;
        private System.Windows.Forms.TextBox txtTimeout;
        private System.Windows.Forms.CheckBox chkFormatJson;
        private System.Windows.Forms.GroupBox groupBoxSavedRequests;
        private System.Windows.Forms.ListBox listBoxSavedRequests;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.GroupBox groupBoxResponse;
        private System.Windows.Forms.Panel panelStatus;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnCopyResponse;
        private System.Windows.Forms.TabControl tabControlResponse;
        private System.Windows.Forms.TabPage tabPageResponseBody;
        private System.Windows.Forms.TextBox txtResponse;
        private System.Windows.Forms.TabPage tabPageResponseHeaders;
        private System.Windows.Forms.TextBox txtResponseHeaders;
        private System.Windows.Forms.CheckBox chkIgnoreCertificate;
    }
}
