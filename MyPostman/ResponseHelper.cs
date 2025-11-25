using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace MyPostman
{
    /// <summary>
    /// Helper class for processing response data
    /// 處理回應資料的輔助類別
    /// </summary>
    public static class ResponseHelper
    {
        /// <summary>
        /// Convert hexadecimal encoded characters to readable text (Traditional Chinese, Simplified Chinese, or English)
        /// 將十六進位編碼字元轉換為可讀文字（繁體中文、簡體中文或英文）
        /// </summary>
        public static string ConvertHexToReadable(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            string result = input;

            // Convert Unicode escape sequences like \uXXXX
            result = ConvertUnicodeEscapes(result);

            // Convert URL encoded sequences like %XX
            result = ConvertUrlEncoded(result);

            // Convert HTML entities like &#xXXXX; or &#XXXX;
            result = ConvertHtmlEntities(result);

            // Convert hex byte sequences like 0xXX or \xXX
            result = ConvertHexBytes(result);

            return result;
        }

        /// <summary>
        /// Convert Unicode escape sequences (\uXXXX) to characters
        /// 將 Unicode 轉義序列 (\uXXXX) 轉換為字元
        /// </summary>
        private static string ConvertUnicodeEscapes(string input)
        {
            try
            {
                // Pattern for \uXXXX
                Regex unicodePattern = new Regex(@"\\u([0-9A-Fa-f]{4})");
                
                return unicodePattern.Replace(input, match =>
                {
                    string hex = match.Groups[1].Value;
                    int codePoint = Convert.ToInt32(hex, 16);
                    // Validate code point range (exclude surrogates and values > 0x10FFFF)
                    if (codePoint >= 0xD800 && codePoint <= 0xDFFF)
                    {
                        return match.Value; // Keep original for invalid surrogate pairs
                    }
                    if (codePoint > 0x10FFFF)
                    {
                        return match.Value; // Keep original for out of range values
                    }
                    return char.ConvertFromUtf32(codePoint);
                });
            }
            catch
            {
                return input;
            }
        }

        /// <summary>
        /// Convert URL encoded sequences (%XX) to characters
        /// 將 URL 編碼序列 (%XX) 轉換為字元
        /// </summary>
        private static string ConvertUrlEncoded(string input)
        {
            try
            {
                // Handle URL encoding
                if (input.Contains("%"))
                {
                    // Decode UTF-8 URL encoded string using manual implementation
                    return UrlDecode(input);
                }
                return input;
            }
            catch
            {
                return input;
            }
        }

        /// <summary>
        /// Manual URL decode implementation
        /// 手動 URL 解碼實現
        /// </summary>
        private static string UrlDecode(string input)
        {
            List<byte> bytes = new List<byte>();
            int i = 0;
            
            while (i < input.Length)
            {
                if (input[i] == '%' && i + 2 <= input.Length - 1)
                {
                    string hex = input.Substring(i + 1, 2);
                    if (IsValidHexPair(hex))
                    {
                        bytes.Add(Convert.ToByte(hex, 16));
                        i += 3;
                        continue;
                    }
                }
                else if (input[i] == '+')
                {
                    bytes.Add((byte)' ');
                    i++;
                    continue;
                }
                
                // Add as UTF-8 bytes
                byte[] charBytes = Encoding.UTF8.GetBytes(new char[] { input[i] });
                bytes.AddRange(charBytes);
                i++;
            }
            
            return Encoding.UTF8.GetString(bytes.ToArray());
        }

        /// <summary>
        /// Check if a string is a valid two-character hexadecimal string
        /// 檢查字串是否為有效的兩字元十六進位字串
        /// </summary>
        private static bool IsValidHexPair(string str)
        {
            if (str == null || str.Length != 2)
            {
                return false;
            }
            
            foreach (char c in str)
            {
                if (!((c >= '0' && c <= '9') || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f')))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Convert HTML entities (&#xXXXX; or &#XXXX;) to characters
        /// 將 HTML 實體 (&#xXXXX; 或 &#XXXX;) 轉換為字元
        /// </summary>
        private static string ConvertHtmlEntities(string input)
        {
            try
            {
                // Pattern for &#xXXXX; (hexadecimal)
                Regex hexEntityPattern = new Regex(@"&#x([0-9A-Fa-f]+);");
                string result = hexEntityPattern.Replace(input, match =>
                {
                    string hex = match.Groups[1].Value;
                    int codePoint = Convert.ToInt32(hex, 16);
                    if (codePoint <= 0x10FFFF)
                    {
                        return char.ConvertFromUtf32(codePoint);
                    }
                    return match.Value;
                });

                // Pattern for &#XXXX; (decimal)
                Regex decEntityPattern = new Regex(@"&#(\d+);");
                result = decEntityPattern.Replace(result, match =>
                {
                    string dec = match.Groups[1].Value;
                    int codePoint = int.Parse(dec);
                    if (codePoint <= 0x10FFFF)
                    {
                        return char.ConvertFromUtf32(codePoint);
                    }
                    return match.Value;
                });

                return result;
            }
            catch
            {
                return input;
            }
        }

        /// <summary>
        /// Convert hex byte sequences (\xXX or 0xXX) to characters
        /// 將十六進位位元組序列 (\xXX 或 0xXX) 轉換為字元
        /// </summary>
        private static string ConvertHexBytes(string input)
        {
            try
            {
                // Pattern for \xXX sequences (multiple consecutive)
                Regex hexPattern = new Regex(@"((?:\\x[0-9A-Fa-f]{2})+)");
                
                string result = hexPattern.Replace(input, match =>
                {
                    string hexSequence = match.Value;
                    // Extract all hex bytes
                    Regex bytePattern = new Regex(@"\\x([0-9A-Fa-f]{2})");
                    MatchCollection matches = bytePattern.Matches(hexSequence);
                    
                    byte[] bytes = new byte[matches.Count];
                    for (int i = 0; i < matches.Count; i++)
                    {
                        bytes[i] = Convert.ToByte(matches[i].Groups[1].Value, 16);
                    }
                    
                    // Try UTF-8 first
                    try
                    {
                        return Encoding.UTF8.GetString(bytes);
                    }
                    catch
                    {
                        return hexSequence;
                    }
                });

                return result;
            }
            catch
            {
                return input;
            }
        }

        /// <summary>
        /// Format WebException to a readable error message
        /// 將 WebException 格式化為可讀的錯誤訊息
        /// </summary>
        public static string FormatWebException(WebException ex)
        {
            StringBuilder errorMessage = new StringBuilder();
            
            errorMessage.AppendLine("=== 錯誤資訊 (Error Information) ===");
            errorMessage.AppendLine();

            // Translate common error status
            string statusDescription = GetStatusDescription(ex.Status);
            errorMessage.AppendLine(string.Format("錯誤狀態 (Error Status): {0}", statusDescription));
            errorMessage.AppendLine();

            // Get HTTP response if available
            if (ex.Response != null)
            {
                try
                {
                    HttpWebResponse response = (HttpWebResponse)ex.Response;
                    errorMessage.AppendLine(string.Format("HTTP 狀態碼 (HTTP Status Code): {0} - {1}", 
                        (int)response.StatusCode, 
                        GetHttpStatusDescription(response.StatusCode)));
                    errorMessage.AppendLine();

                    // Read error response body
                    using (Stream responseStream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(responseStream, Encoding.UTF8))
                    {
                        string responseBody = reader.ReadToEnd();
                        if (!string.IsNullOrEmpty(responseBody))
                        {
                            errorMessage.AppendLine("=== 伺服器回應 (Server Response) ===");
                            errorMessage.AppendLine(ConvertHexToReadable(responseBody));
                        }
                    }
                }
                catch (Exception innerEx)
                {
                    errorMessage.AppendLine("無法讀取伺服器回應 (Cannot read server response): " + innerEx.Message);
                }
            }
            else
            {
                errorMessage.AppendLine(string.Format("錯誤訊息 (Error Message): {0}", ex.Message));
            }

            return errorMessage.ToString();
        }

        /// <summary>
        /// Get translated description for WebExceptionStatus
        /// 取得 WebExceptionStatus 的翻譯描述
        /// </summary>
        private static string GetStatusDescription(WebExceptionStatus status)
        {
            switch (status)
            {
                case WebExceptionStatus.Success:
                    return "成功 (Success)";
                case WebExceptionStatus.NameResolutionFailure:
                    return "無法解析主機名稱 (Cannot resolve hostname)";
                case WebExceptionStatus.ConnectFailure:
                    return "無法連接到伺服器 (Cannot connect to server)";
                case WebExceptionStatus.ReceiveFailure:
                    return "接收資料失敗 (Failed to receive data)";
                case WebExceptionStatus.SendFailure:
                    return "發送資料失敗 (Failed to send data)";
                case WebExceptionStatus.PipelineFailure:
                    return "管道失敗 (Pipeline failure)";
                case WebExceptionStatus.RequestCanceled:
                    return "請求已取消 (Request canceled)";
                case WebExceptionStatus.ProtocolError:
                    return "協定錯誤 (Protocol error)";
                case WebExceptionStatus.ConnectionClosed:
                    return "連接已關閉 (Connection closed)";
                case WebExceptionStatus.TrustFailure:
                    return "憑證驗證失敗 (Certificate validation failed)";
                case WebExceptionStatus.SecureChannelFailure:
                    return "安全通道建立失敗 (Secure channel establishment failed)";
                case WebExceptionStatus.ServerProtocolViolation:
                    return "伺服器協定違規 (Server protocol violation)";
                case WebExceptionStatus.KeepAliveFailure:
                    return "保持連接失敗 (Keep-alive failure)";
                case WebExceptionStatus.Pending:
                    return "請求待處理 (Request pending)";
                case WebExceptionStatus.Timeout:
                    return "請求逾時 (Request timeout)";
                case WebExceptionStatus.ProxyNameResolutionFailure:
                    return "無法解析代理伺服器名稱 (Cannot resolve proxy hostname)";
                case WebExceptionStatus.UnknownError:
                    return "未知錯誤 (Unknown error)";
                case WebExceptionStatus.MessageLengthLimitExceeded:
                    return "訊息長度超過限制 (Message length limit exceeded)";
                case WebExceptionStatus.CacheEntryNotFound:
                    return "找不到快取項目 (Cache entry not found)";
                case WebExceptionStatus.RequestProhibitedByCachePolicy:
                    return "快取策略禁止請求 (Request prohibited by cache policy)";
                case WebExceptionStatus.RequestProhibitedByProxy:
                    return "代理伺服器禁止請求 (Request prohibited by proxy)";
                default:
                    return string.Format("{0} ({1})", status.ToString(), (int)status);
            }
        }

        /// <summary>
        /// Get translated description for HTTP status codes
        /// 取得 HTTP 狀態碼的翻譯描述
        /// </summary>
        private static string GetHttpStatusDescription(HttpStatusCode statusCode)
        {
            switch (statusCode)
            {
                // 2XX Success
                case HttpStatusCode.OK:
                    return "成功 (OK)";
                case HttpStatusCode.Created:
                    return "已創建 (Created)";
                case HttpStatusCode.Accepted:
                    return "已接受 (Accepted)";
                case HttpStatusCode.NoContent:
                    return "無內容 (No Content)";
                    
                // 3XX Redirection
                case HttpStatusCode.MovedPermanently:
                    return "永久移動 (Moved Permanently)";
                case HttpStatusCode.Found:
                    return "已找到/臨時移動 (Found/Temporarily Moved)";
                case HttpStatusCode.NotModified:
                    return "未修改 (Not Modified)";
                    
                // 4XX Client Errors
                case HttpStatusCode.BadRequest:
                    return "錯誤請求 (Bad Request)";
                case HttpStatusCode.Unauthorized:
                    return "未授權 (Unauthorized)";
                case HttpStatusCode.PaymentRequired:
                    return "需要付款 (Payment Required)";
                case HttpStatusCode.Forbidden:
                    return "禁止訪問 (Forbidden)";
                case HttpStatusCode.NotFound:
                    return "找不到資源 (Not Found)";
                case HttpStatusCode.MethodNotAllowed:
                    return "方法不允許 (Method Not Allowed)";
                case HttpStatusCode.NotAcceptable:
                    return "不可接受 (Not Acceptable)";
                case HttpStatusCode.RequestTimeout:
                    return "請求逾時 (Request Timeout)";
                case HttpStatusCode.Conflict:
                    return "衝突 (Conflict)";
                case HttpStatusCode.Gone:
                    return "資源已消失 (Gone)";
                case HttpStatusCode.RequestEntityTooLarge:
                    return "請求實體太大 (Request Entity Too Large)";
                case HttpStatusCode.UnsupportedMediaType:
                    return "不支援的媒體類型 (Unsupported Media Type)";
                case (HttpStatusCode)422:
                    return "無法處理的實體 (Unprocessable Entity)";
                case (HttpStatusCode)429:
                    return "請求過多 (Too Many Requests)";
                    
                // 5XX Server Errors
                case HttpStatusCode.InternalServerError:
                    return "伺服器內部錯誤 (Internal Server Error)";
                case HttpStatusCode.NotImplemented:
                    return "未實作 (Not Implemented)";
                case HttpStatusCode.BadGateway:
                    return "錯誤的閘道 (Bad Gateway)";
                case HttpStatusCode.ServiceUnavailable:
                    return "服務不可用 (Service Unavailable)";
                case HttpStatusCode.GatewayTimeout:
                    return "閘道逾時 (Gateway Timeout)";
                case HttpStatusCode.HttpVersionNotSupported:
                    return "不支援的 HTTP 版本 (HTTP Version Not Supported)";
                    
                default:
                    return statusCode.ToString();
            }
        }

        /// <summary>
        /// Format general Exception to a readable error message
        /// 將一般 Exception 格式化為可讀的錯誤訊息
        /// </summary>
        public static string FormatException(Exception ex)
        {
            StringBuilder errorMessage = new StringBuilder();
            
            errorMessage.AppendLine("=== 錯誤資訊 (Error Information) ===");
            errorMessage.AppendLine();
            
            // Translate common exception types
            string exceptionType = GetExceptionTypeDescription(ex);
            errorMessage.AppendLine(string.Format("錯誤類型 (Error Type): {0}", exceptionType));
            errorMessage.AppendLine();
            
            errorMessage.AppendLine(string.Format("錯誤訊息 (Error Message): {0}", ex.Message));
            
            if (ex.InnerException != null)
            {
                errorMessage.AppendLine();
                errorMessage.AppendLine(string.Format("內部錯誤 (Inner Error): {0}", ex.InnerException.Message));
            }

            return errorMessage.ToString();
        }

        /// <summary>
        /// Get translated description for exception types
        /// 取得例外類型的翻譯描述
        /// </summary>
        private static string GetExceptionTypeDescription(Exception ex)
        {
            string typeName = ex.GetType().Name;
            
            switch (typeName)
            {
                case "ArgumentException":
                    return "參數錯誤 (Argument Error)";
                case "ArgumentNullException":
                    return "參數為空 (Null Argument)";
                case "InvalidOperationException":
                    return "無效操作 (Invalid Operation)";
                case "NotSupportedException":
                    return "不支援的操作 (Not Supported)";
                case "IOException":
                    return "輸入/輸出錯誤 (IO Error)";
                case "FileNotFoundException":
                    return "找不到檔案 (File Not Found)";
                case "DirectoryNotFoundException":
                    return "找不到目錄 (Directory Not Found)";
                case "UnauthorizedAccessException":
                    return "存取被拒 (Access Denied)";
                case "TimeoutException":
                    return "逾時 (Timeout)";
                case "FormatException":
                    return "格式錯誤 (Format Error)";
                case "OverflowException":
                    return "溢位 (Overflow)";
                case "OutOfMemoryException":
                    return "記憶體不足 (Out of Memory)";
                case "NullReferenceException":
                    return "空參考 (Null Reference)";
                case "UriFormatException":
                    return "URL 格式錯誤 (URL Format Error)";
                default:
                    return typeName;
            }
        }
    }
}
