using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace myPostman
{
    public partial class MainForm : Form
    {
        private List<RequestConfig> savedConfigs;
        private bool isRequestInProgress = false;
        private AuthenticationConfig currentAuthConfig;

        public MainForm()
        {
            InitializeComponent();
            currentAuthConfig = new AuthenticationConfig();
            LoadSavedConfigs();
        }

        private void LoadSavedConfigs()
        {
            try
            {
                savedConfigs = RequestConfigManager.LoadConfigs();
                UpdateSavedRequestsList();
            }
            catch (Exception ex)
            {
                savedConfigs = new List<RequestConfig>();
                MessageBox.Show("無法載入已儲存的請求 / Unable to load saved requests: " + ex.Message,
                    "警告 / Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void UpdateSavedRequestsList()
        {
            listBoxSavedRequests.Items.Clear();
            foreach (RequestConfig config in savedConfigs)
            {
                listBoxSavedRequests.Items.Add(config.Name + " [" + config.Method + "] " + config.Url);
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (isRequestInProgress)
            {
                return;
            }

            string url = txtUrl.Text.Trim();
            string method = cmbMethod.SelectedItem.ToString();
            string headers = txtHeaders.Text;
            string body = txtBody.Text;
            int timeout;

            if (!int.TryParse(txtTimeout.Text, out timeout) || timeout <= 0)
            {
                timeout = 30000; // Default 30 seconds
            }
            else
            {
                timeout = timeout * 1000; // Convert to milliseconds
            }

            if (string.IsNullOrEmpty(url))
            {
                MessageBox.Show("請輸入 URL / Please enter URL", "錯誤 / Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Add protocol if missing
            if (!url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                url = "http://" + url;
                txtUrl.Text = url;
            }

            isRequestInProgress = true;
            btnSend.Enabled = false;
            btnSend.Text = "發送中... / Sending...";
            txtResponse.Text = "正在發送請求... / Sending request...";
            txtResponseHeaders.Text = "";
            lblStatus.Text = "狀態 / Status: 發送中... / Sending...";
            Application.DoEvents();

            // Get current authentication configuration from UI
            AuthenticationConfig authConfig = GetAuthenticationConfigFromUI();

            // Use a thread to avoid freezing the UI
            Thread requestThread = new Thread(delegate()
            {
                HttpResponse response = HttpRequestHelper.SendRequest(url, method, headers, body, timeout, authConfig);

                // Update UI on the main thread
                this.BeginInvoke((MethodInvoker)delegate
                {
                    DisplayResponse(response);
                    isRequestInProgress = false;
                    btnSend.Enabled = true;
                    btnSend.Text = "發送 / Send";
                });
            });

            requestThread.IsBackground = true;
            requestThread.Start();
        }

        private void DisplayResponse(HttpResponse response)
        {
            if (response.Success)
            {
                lblStatus.Text = string.Format("狀態 / Status: {0} {1}", response.StatusCode, response.StatusDescription);
                lblStatus.ForeColor = Color.Green;
            }
            else
            {
                if (response.StatusCode > 0)
                {
                    lblStatus.Text = string.Format("狀態 / Status: {0} {1}", response.StatusCode, response.StatusDescription);
                    lblStatus.ForeColor = Color.Red;
                }
                else
                {
                    lblStatus.Text = "狀態 / Status: 錯誤 / Error";
                    lblStatus.ForeColor = Color.Red;
                }
            }

            txtResponseHeaders.Text = response.Headers;

            // Process response body
            string processedBody = EncodingHelper.ProcessResponse(response.Body);

            // Format JSON if applicable
            if (chkFormatJson.Checked && EncodingHelper.IsJson(processedBody))
            {
                processedBody = EncodingHelper.FormatJson(processedBody);
            }

            if (!string.IsNullOrEmpty(response.ErrorMessage))
            {
                txtResponse.Text = response.ErrorMessage;
                if (!string.IsNullOrEmpty(processedBody))
                {
                    txtResponse.Text += "\r\n\r\n--- 回應內容 / Response Body ---\r\n" + processedBody;
                }
            }
            else
            {
                txtResponse.Text = processedBody;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string url = txtUrl.Text.Trim();
            if (string.IsNullOrEmpty(url))
            {
                MessageBox.Show("請先輸入 URL / Please enter URL first", "錯誤 / Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string name = InputDialog.Show(
                "請輸入請求名稱 / Enter request name:", 
                "儲存請求 / Save Request", 
                "Request " + (savedConfigs.Count + 1));

            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            AuthenticationConfig authConfig = GetAuthenticationConfigFromUI();

            RequestConfig config = new RequestConfig(
                name,
                txtUrl.Text.Trim(),
                cmbMethod.SelectedItem.ToString(),
                txtHeaders.Text,
                txtBody.Text,
                authConfig
            );

            savedConfigs.Add(config);

            try
            {
                RequestConfigManager.SaveConfigs(savedConfigs);
                UpdateSavedRequestsList();
                MessageBox.Show("請求已儲存 / Request saved", "成功 / Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("無法儲存請求 / Unable to save request: " + ex.Message,
                    "錯誤 / Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (listBoxSavedRequests.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇一個已儲存的請求 / Please select a saved request",
                    "提示 / Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            RequestConfig config = savedConfigs[listBoxSavedRequests.SelectedIndex];
            txtUrl.Text = config.Url;
            
            for (int i = 0; i < cmbMethod.Items.Count; i++)
            {
                if (cmbMethod.Items[i].ToString() == config.Method)
                {
                    cmbMethod.SelectedIndex = i;
                    break;
                }
            }

            txtHeaders.Text = config.Headers;
            txtBody.Text = config.Body;

            // Load authentication configuration
            if (config.Authentication != null)
            {
                LoadAuthenticationConfigToUI(config.Authentication);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listBoxSavedRequests.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇一個要刪除的請求 / Please select a request to delete",
                    "提示 / Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show(
                "確定要刪除此請求? / Are you sure you want to delete this request?",
                "確認刪除 / Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                savedConfigs.RemoveAt(listBoxSavedRequests.SelectedIndex);

                try
                {
                    RequestConfigManager.SaveConfigs(savedConfigs);
                    UpdateSavedRequestsList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("無法刪除請求 / Unable to delete request: " + ex.Message,
                        "錯誤 / Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (listBoxSavedRequests.SelectedIndex < 0)
            {
                MessageBox.Show("請選擇一個要匯出的請求 / Please select a request to export",
                    "提示 / Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "XML Files (*.xml)|*.xml";
            dialog.DefaultExt = "xml";
            dialog.FileName = savedConfigs[listBoxSavedRequests.SelectedIndex].Name + ".xml";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    RequestConfigManager.ExportConfig(savedConfigs[listBoxSavedRequests.SelectedIndex], dialog.FileName);
                    MessageBox.Show("請求已匯出 / Request exported", "成功 / Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("無法匯出請求 / Unable to export request: " + ex.Message,
                        "錯誤 / Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "XML Files (*.xml)|*.xml";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    RequestConfig config = RequestConfigManager.ImportConfig(dialog.FileName);
                    savedConfigs.Add(config);
                    RequestConfigManager.SaveConfigs(savedConfigs);
                    UpdateSavedRequestsList();
                    MessageBox.Show("請求已匯入 / Request imported", "成功 / Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("無法匯入請求 / Unable to import request: " + ex.Message,
                        "錯誤 / Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void listBoxSavedRequests_DoubleClick(object sender, EventArgs e)
        {
            btnLoad_Click(sender, e);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtUrl.Text = "";
            cmbMethod.SelectedIndex = 0;
            txtHeaders.Text = "";
            txtBody.Text = "";
            txtResponse.Text = "";
            txtResponseHeaders.Text = "";
            lblStatus.Text = "狀態 / Status: 準備就緒 / Ready";
            lblStatus.ForeColor = SystemColors.ControlText;
        }

        private void btnCopyResponse_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtResponse.Text))
            {
                Clipboard.SetText(txtResponse.Text);
                MessageBox.Show("回應已複製到剪貼簿 / Response copied to clipboard", "成功 / Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // SSL/TLS configuration for .NET 3.5
            try
            {
                // Enable all available security protocols
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072 | // TLS 1.2
                                                       (SecurityProtocolType)768 |  // TLS 1.1
                                                       SecurityProtocolType.Tls |
                                                       SecurityProtocolType.Ssl3;
            }
            catch
            {
                // Fall back to default if TLS 1.2 is not available
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
            }

            // Note: Certificate validation is enabled by default for security
            // If you need to bypass certificate validation for testing, 
            // use the checkbox option and be aware of the security implications
        }

        private void chkIgnoreCertificate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIgnoreCertificate.Checked)
            {
                DialogResult result = MessageBox.Show(
                    "警告：忽略 SSL 憑證驗證可能會導致安全風險。\n" +
                    "僅在測試環境中使用此選項。\n\n" +
                    "Warning: Ignoring SSL certificate validation may pose security risks.\n" +
                    "Only use this option in testing environments.\n\n" +
                    "確定要繼續嗎? / Are you sure you want to continue?",
                    "安全警告 / Security Warning",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                }
                else
                {
                    chkIgnoreCertificate.Checked = false;
                }
            }
            else
            {
                ServicePointManager.ServerCertificateValidationCallback = null;
            }
        }

        private AuthenticationConfig GetAuthenticationConfigFromUI()
        {
            AuthenticationConfig config = new AuthenticationConfig();

            // Get authentication type from combo box
            if (cmbAuthType.SelectedIndex == 0) // None
            {
                config.Type = AuthenticationType.None;
            }
            else if (cmbAuthType.SelectedIndex == 1) // Basic
            {
                config.Type = AuthenticationType.Basic;
                config.Username = txtAuthUsername.Text;
                config.Password = txtAuthPassword.Text;
            }
            else if (cmbAuthType.SelectedIndex == 2) // Bearer Token
            {
                config.Type = AuthenticationType.BearerToken;
                config.Token = txtAuthToken.Text;
            }
            else if (cmbAuthType.SelectedIndex == 3) // OAuth 2.0
            {
                config.Type = AuthenticationType.OAuth2;
                config.Token = txtAuthToken.Text;
            }
            else if (cmbAuthType.SelectedIndex == 4) // API Key
            {
                config.Type = AuthenticationType.ApiKey;
                config.ApiKeyName = txtApiKeyName.Text;
                config.ApiKeyValue = txtApiKeyValue.Text;
                config.ApiKeyLocation = rdoApiKeyHeader.Checked ? ApiKeyLocation.Header : ApiKeyLocation.QueryParam;
            }

            return config;
        }

        private void LoadAuthenticationConfigToUI(AuthenticationConfig config)
        {
            if (config == null)
            {
                config = new AuthenticationConfig();
            }

            // Set authentication type
            switch (config.Type)
            {
                case AuthenticationType.None:
                    cmbAuthType.SelectedIndex = 0;
                    break;
                case AuthenticationType.Basic:
                    cmbAuthType.SelectedIndex = 1;
                    txtAuthUsername.Text = config.Username ?? "";
                    txtAuthPassword.Text = config.Password ?? "";
                    break;
                case AuthenticationType.BearerToken:
                    cmbAuthType.SelectedIndex = 2;
                    txtAuthToken.Text = config.Token ?? "";
                    break;
                case AuthenticationType.OAuth2:
                    cmbAuthType.SelectedIndex = 3;
                    txtAuthToken.Text = config.Token ?? "";
                    break;
                case AuthenticationType.ApiKey:
                    cmbAuthType.SelectedIndex = 4;
                    txtApiKeyName.Text = config.ApiKeyName ?? "";
                    txtApiKeyValue.Text = config.ApiKeyValue ?? "";
                    if (config.ApiKeyLocation == ApiKeyLocation.Header)
                    {
                        rdoApiKeyHeader.Checked = true;
                    }
                    else
                    {
                        rdoApiKeyQuery.Checked = true;
                    }
                    break;
            }

            UpdateAuthenticationControlsVisibility();
        }

        private void cmbAuthType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateAuthenticationControlsVisibility();
        }

        private void UpdateAuthenticationControlsVisibility()
        {
            // Hide all authentication controls
            lblAuthUsername.Visible = false;
            txtAuthUsername.Visible = false;
            lblAuthPassword.Visible = false;
            txtAuthPassword.Visible = false;
            lblAuthToken.Visible = false;
            txtAuthToken.Visible = false;
            lblApiKeyName.Visible = false;
            txtApiKeyName.Visible = false;
            lblApiKeyValue.Visible = false;
            txtApiKeyValue.Visible = false;
            rdoApiKeyHeader.Visible = false;
            rdoApiKeyQuery.Visible = false;
            lblApiKeyLocation.Visible = false;

            // Show relevant controls based on selected authentication type
            if (cmbAuthType.SelectedIndex == 1) // Basic
            {
                lblAuthUsername.Visible = true;
                txtAuthUsername.Visible = true;
                lblAuthPassword.Visible = true;
                txtAuthPassword.Visible = true;
            }
            else if (cmbAuthType.SelectedIndex == 2 || cmbAuthType.SelectedIndex == 3) // Bearer Token or OAuth 2.0
            {
                lblAuthToken.Visible = true;
                txtAuthToken.Visible = true;
            }
            else if (cmbAuthType.SelectedIndex == 4) // API Key
            {
                lblApiKeyName.Visible = true;
                txtApiKeyName.Visible = true;
                lblApiKeyValue.Visible = true;
                txtApiKeyValue.Visible = true;
                lblApiKeyLocation.Visible = true;
                rdoApiKeyHeader.Visible = true;
                rdoApiKeyQuery.Visible = true;
            }
        }
    }
}
