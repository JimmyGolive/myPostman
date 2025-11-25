using System;
using System.IO;
using System.Net;
using System.Text;

namespace myPostman
{
    /// <summary>
    /// Helper class for sending HTTP requests
    /// </summary>
    public class HttpRequestHelper
    {
        /// <summary>
        /// Sends an HTTP request and returns the response
        /// </summary>
        /// <param name="url">Target URL</param>
        /// <param name="method">HTTP method (GET, POST, PUT, DELETE)</param>
        /// <param name="headers">Request headers (format: "Header1: Value1\nHeader2: Value2")</param>
        /// <param name="body">Request body</param>
        /// <param name="timeout">Timeout in milliseconds</param>
        /// <returns>HttpResponse containing status, headers, and body</returns>
        public static HttpResponse SendRequest(string url, string method, string headers, string body, int timeout)
        {
            HttpResponse response = new HttpResponse();
            HttpWebRequest request = null;
            HttpWebResponse webResponse = null;
            Stream responseStream = null;
            StreamReader reader = null;

            try
            {
                // Validate URL
                if (string.IsNullOrEmpty(url))
                {
                    throw new ArgumentException("URL cannot be empty");
                }

                // Create request
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = method.ToUpper();
                request.Timeout = timeout;

                // Set default User-Agent
                request.UserAgent = "myPostman/1.0";

                // Parse and set headers
                if (!string.IsNullOrEmpty(headers))
                {
                    string[] headerLines = headers.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string headerLine in headerLines)
                    {
                        int colonIndex = headerLine.IndexOf(':');
                        if (colonIndex > 0)
                        {
                            string headerName = headerLine.Substring(0, colonIndex).Trim();
                            string headerValue = headerLine.Substring(colonIndex + 1).Trim();
                            SetHeader(request, headerName, headerValue);
                        }
                    }
                }

                // Set request body for POST, PUT methods
                string methodUpper = method.ToUpper();
                if (!string.IsNullOrEmpty(body) && (methodUpper == "POST" || methodUpper == "PUT" || methodUpper == "PATCH"))
                {
                    byte[] bodyBytes = Encoding.UTF8.GetBytes(body);
                    request.ContentLength = bodyBytes.Length;

                    if (string.IsNullOrEmpty(request.ContentType))
                    {
                        request.ContentType = "application/json";
                    }

                    using (Stream requestStream = request.GetRequestStream())
                    {
                        requestStream.Write(bodyBytes, 0, bodyBytes.Length);
                    }
                }

                // Send request and get response
                webResponse = (HttpWebResponse)request.GetResponse();
                response.StatusCode = (int)webResponse.StatusCode;
                response.StatusDescription = webResponse.StatusDescription;

                // Get response headers
                StringBuilder headerBuilder = new StringBuilder();
                for (int i = 0; i < webResponse.Headers.Count; i++)
                {
                    headerBuilder.AppendLine(webResponse.Headers.Keys[i] + ": " + webResponse.Headers[i]);
                }
                response.Headers = headerBuilder.ToString();

                // Get response body
                responseStream = webResponse.GetResponseStream();
                if (responseStream != null)
                {
                    // Determine encoding from Content-Type header
                    Encoding encoding = GetEncodingFromContentType(webResponse.ContentType);
                    reader = new StreamReader(responseStream, encoding);
                    response.Body = reader.ReadToEnd();
                }

                response.Success = true;
            }
            catch (WebException ex)
            {
                response.Success = false;

                if (ex.Response != null)
                {
                    webResponse = (HttpWebResponse)ex.Response;
                    response.StatusCode = (int)webResponse.StatusCode;
                    response.StatusDescription = webResponse.StatusDescription;

                    // Get response headers
                    StringBuilder headerBuilder = new StringBuilder();
                    for (int i = 0; i < webResponse.Headers.Count; i++)
                    {
                        headerBuilder.AppendLine(webResponse.Headers.Keys[i] + ": " + webResponse.Headers[i]);
                    }
                    response.Headers = headerBuilder.ToString();

                    // Get error response body
                    try
                    {
                        responseStream = webResponse.GetResponseStream();
                        if (responseStream != null)
                        {
                            Encoding encoding = GetEncodingFromContentType(webResponse.ContentType);
                            reader = new StreamReader(responseStream, encoding);
                            response.Body = reader.ReadToEnd();
                        }
                    }
                    catch
                    {
                        response.Body = "";
                    }
                }

                response.ErrorMessage = FormatErrorMessage(ex);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = FormatErrorMessage(ex);
            }
            finally
            {
                if (reader != null) reader.Close();
                if (responseStream != null) responseStream.Close();
                if (webResponse != null) webResponse.Close();
            }

            return response;
        }

        /// <summary>
        /// Sets HTTP header on request, handling special headers
        /// </summary>
        private static void SetHeader(HttpWebRequest request, string name, string value)
        {
            string nameLower = name.ToLower();

            switch (nameLower)
            {
                case "content-type":
                    request.ContentType = value;
                    break;
                case "accept":
                    request.Accept = value;
                    break;
                case "user-agent":
                    request.UserAgent = value;
                    break;
                case "content-length":
                    // Handled automatically
                    break;
                case "connection":
                    if (value.ToLower() == "keep-alive")
                        request.KeepAlive = true;
                    else if (value.ToLower() == "close")
                        request.KeepAlive = false;
                    break;
                case "referer":
                    request.Referer = value;
                    break;
                case "host":
                    // Handled automatically
                    break;
                case "expect":
                    request.Expect = value;
                    break;
                case "if-modified-since":
                    DateTime date;
                    if (DateTime.TryParse(value, out date))
                        request.IfModifiedSince = date;
                    break;
                case "transfer-encoding":
                    request.TransferEncoding = value;
                    break;
                default:
                    request.Headers.Add(name, value);
                    break;
            }
        }

        /// <summary>
        /// Gets encoding from Content-Type header
        /// </summary>
        private static Encoding GetEncodingFromContentType(string contentType)
        {
            if (string.IsNullOrEmpty(contentType))
            {
                return Encoding.UTF8;
            }

            // Look for charset in content-type
            string[] parts = contentType.Split(';');
            foreach (string part in parts)
            {
                string trimmed = part.Trim();
                if (trimmed.ToLower().StartsWith("charset="))
                {
                    string charset = trimmed.Substring(8).Trim().Trim('"');
                    try
                    {
                        return Encoding.GetEncoding(charset);
                    }
                    catch
                    {
                        return Encoding.UTF8;
                    }
                }
            }

            return Encoding.UTF8;
        }

        /// <summary>
        /// Formats exception message to be more readable
        /// </summary>
        public static string FormatErrorMessage(Exception ex)
        {
            StringBuilder sb = new StringBuilder();

            if (ex is WebException)
            {
                WebException webEx = (WebException)ex;
                sb.AppendLine("【網路錯誤】Network Error");
                sb.AppendLine("");

                switch (webEx.Status)
                {
                    case WebExceptionStatus.NameResolutionFailure:
                        sb.AppendLine("無法解析主機名稱 / Unable to resolve hostname");
                        sb.AppendLine("請檢查 URL 是否正確 / Please check if the URL is correct");
                        break;
                    case WebExceptionStatus.ConnectFailure:
                        sb.AppendLine("無法連線到伺服器 / Unable to connect to server");
                        sb.AppendLine("請檢查網路連線或伺服器是否正常運作 / Please check network connection or server status");
                        break;
                    case WebExceptionStatus.Timeout:
                        sb.AppendLine("連線逾時 / Connection timeout");
                        sb.AppendLine("伺服器沒有在指定時間內回應 / Server did not respond within the specified time");
                        break;
                    case WebExceptionStatus.ProtocolError:
                        sb.AppendLine("協定錯誤 / Protocol error");
                        if (webEx.Response != null)
                        {
                            HttpWebResponse errorResponse = (HttpWebResponse)webEx.Response;
                            sb.AppendLine(string.Format("HTTP 狀態碼 / HTTP Status: {0} {1}", (int)errorResponse.StatusCode, errorResponse.StatusDescription));
                        }
                        break;
                    case WebExceptionStatus.TrustFailure:
                        sb.AppendLine("SSL/TLS 憑證驗證失敗 / SSL/TLS certificate validation failed");
                        sb.AppendLine("伺服器憑證可能無效或不受信任 / Server certificate may be invalid or untrusted");
                        break;
                    case WebExceptionStatus.SecureChannelFailure:
                        sb.AppendLine("無法建立安全連線 / Unable to establish secure connection");
                        sb.AppendLine("可能是 SSL/TLS 版本不相容 / SSL/TLS version may be incompatible");
                        break;
                    default:
                        sb.AppendLine(string.Format("狀態 / Status: {0}", webEx.Status));
                        break;
                }

                sb.AppendLine("");
                sb.AppendLine("詳細資訊 / Details:");
                sb.AppendLine(webEx.Message);
            }
            else if (ex is ArgumentException)
            {
                sb.AppendLine("【參數錯誤】Parameter Error");
                sb.AppendLine("");
                sb.AppendLine(ex.Message);
            }
            else if (ex is UriFormatException)
            {
                sb.AppendLine("【URL 格式錯誤】URL Format Error");
                sb.AppendLine("");
                sb.AppendLine("URL 格式不正確 / Invalid URL format");
                sb.AppendLine("請確保 URL 包含協定 (http:// 或 https://) / Please ensure URL includes protocol (http:// or https://)");
                sb.AppendLine("");
                sb.AppendLine("詳細資訊 / Details:");
                sb.AppendLine(ex.Message);
            }
            else
            {
                sb.AppendLine("【錯誤】Error");
                sb.AppendLine("");
                sb.AppendLine(ex.Message);
            }

            return sb.ToString();
        }
    }

    /// <summary>
    /// HTTP response data structure
    /// </summary>
    public class HttpResponse
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public string Headers { get; set; }
        public string Body { get; set; }
        public string ErrorMessage { get; set; }

        public HttpResponse()
        {
            Success = false;
            StatusCode = 0;
            StatusDescription = "";
            Headers = "";
            Body = "";
            ErrorMessage = "";
        }
    }
}
