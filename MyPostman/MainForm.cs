using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace MyPostman
{
    public partial class MainForm : Form
    {
        private RequestManager requestManager;

        public MainForm()
        {
            InitializeComponent();
            requestManager = new RequestManager();
            InitializeHttpMethods();
        }

        private void InitializeHttpMethods()
        {
            cmbMethod.Items.AddRange(new object[] { "GET", "POST", "PUT", "DELETE", "PATCH", "HEAD", "OPTIONS" });
            cmbMethod.SelectedIndex = 0;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            SendRequest();
        }

        private void SendRequest()
        {
            string url = txtUrl.Text.Trim();
            if (string.IsNullOrEmpty(url))
            {
                MessageBox.Show("請輸入 URL (Please enter URL)", "提示 (Notice)", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                btnSend.Enabled = false;
                txtResponse.Text = "正在發送請求... (Sending request...)";
                Application.DoEvents();

                string method = cmbMethod.SelectedItem.ToString();
                string headers = txtHeaders.Text;
                string body = txtBody.Text;

                var response = requestManager.SendRequest(url, method, headers, body);
                
                // Convert hex characters to readable text
                string convertedResponse = ResponseHelper.ConvertHexToReadable(response);
                txtResponse.Text = convertedResponse;

                // Update status
                lblStatus.Text = "請求成功 (Request successful)";
            }
            catch (WebException webEx)
            {
                string errorMessage = ResponseHelper.FormatWebException(webEx);
                txtResponse.Text = errorMessage;
                lblStatus.Text = "請求失敗 (Request failed)";
            }
            catch (Exception ex)
            {
                string errorMessage = ResponseHelper.FormatException(ex);
                txtResponse.Text = errorMessage;
                lblStatus.Text = "發生錯誤 (Error occurred)";
            }
            finally
            {
                btnSend.Enabled = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveRequest();
        }

        private void SaveRequest()
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
            saveDialog.DefaultExt = "json";
            saveDialog.Title = "儲存請求 (Save Request)";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var requestData = new RequestData
                    {
                        Url = txtUrl.Text,
                        Method = cmbMethod.SelectedItem?.ToString() ?? "GET",
                        Headers = txtHeaders.Text,
                        Body = txtBody.Text
                    };

                    requestManager.SaveRequest(saveDialog.FileName, requestData);
                    MessageBox.Show("請求已儲存 (Request saved)", "成功 (Success)", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("儲存失敗: " + ex.Message + "\n(Save failed)", "錯誤 (Error)", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadRequest();
        }

        private void LoadRequest()
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
            openDialog.Title = "載入請求 (Load Request)";

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var requestData = requestManager.LoadRequest(openDialog.FileName);
                    
                    txtUrl.Text = requestData.Url;
                    txtHeaders.Text = requestData.Headers;
                    txtBody.Text = requestData.Body;

                    // Set method
                    int methodIndex = cmbMethod.Items.IndexOf(requestData.Method);
                    if (methodIndex >= 0)
                    {
                        cmbMethod.SelectedIndex = methodIndex;
                    }

                    MessageBox.Show("請求已載入 (Request loaded)", "成功 (Success)", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("載入失敗: " + ex.Message + "\n(Load failed)", "錯誤 (Error)", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearAll();
        }

        private void ClearAll()
        {
            txtUrl.Text = string.Empty;
            txtHeaders.Text = string.Empty;
            txtBody.Text = string.Empty;
            txtResponse.Text = string.Empty;
            cmbMethod.SelectedIndex = 0;
            lblStatus.Text = "就緒 (Ready)";
        }
    }
}
