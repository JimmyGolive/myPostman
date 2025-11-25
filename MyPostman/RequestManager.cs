using System;
using System.IO;
using System.Net;
using System.Text;

namespace MyPostman
{
    /// <summary>
    /// Manages HTTP requests and request data persistence
    /// 管理 HTTP 請求和請求資料的持久化
    /// </summary>
    public class RequestManager
    {
        /// <summary>
        /// Send an HTTP request and return the response
        /// 發送 HTTP 請求並返回回應
        /// </summary>
        public string SendRequest(string url, string method, string headers, string body)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = method;
            request.Timeout = 30000; // 30 seconds timeout

            // Parse and set headers
            if (!string.IsNullOrEmpty(headers))
            {
                ParseAndSetHeaders(request, headers);
            }

            // Set body for POST, PUT, PATCH methods
            if (!string.IsNullOrEmpty(body) && 
                (method == "POST" || method == "PUT" || method == "PATCH"))
            {
                byte[] bodyBytes = Encoding.UTF8.GetBytes(body);
                request.ContentLength = bodyBytes.Length;
                
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(bodyBytes, 0, bodyBytes.Length);
                }
            }

            // Get response
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                return ReadResponse(response);
            }
        }

        /// <summary>
        /// Parse header text and set headers on the request
        /// 解析標頭文字並設定到請求上
        /// </summary>
        private void ParseAndSetHeaders(HttpWebRequest request, string headerText)
        {
            string[] lines = headerText.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            
            foreach (string line in lines)
            {
                int colonIndex = line.IndexOf(':');
                if (colonIndex > 0)
                {
                    string headerName = line.Substring(0, colonIndex).Trim();
                    string headerValue = line.Substring(colonIndex + 1).Trim();
                    
                    // Handle special headers
                    if (headerName.Equals("Content-Type", StringComparison.OrdinalIgnoreCase))
                    {
                        request.ContentType = headerValue;
                    }
                    else if (headerName.Equals("Accept", StringComparison.OrdinalIgnoreCase))
                    {
                        request.Accept = headerValue;
                    }
                    else if (headerName.Equals("User-Agent", StringComparison.OrdinalIgnoreCase))
                    {
                        request.UserAgent = headerValue;
                    }
                    else if (headerName.Equals("Host", StringComparison.OrdinalIgnoreCase))
                    {
                        // Host header is set automatically
                    }
                    else
                    {
                        try
                        {
                            request.Headers.Add(headerName, headerValue);
                        }
                        catch (Exception)
                        {
                            // Skip invalid headers
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Read response content from HttpWebResponse
        /// 從 HttpWebResponse 讀取回應內容
        /// </summary>
        private string ReadResponse(HttpWebResponse response)
        {
            StringBuilder result = new StringBuilder();
            
            // Add response info
            result.AppendLine("=== 回應資訊 (Response Info) ===");
            result.AppendLine(string.Format("狀態碼 (Status): {0} {1}", (int)response.StatusCode, response.StatusDescription));
            result.AppendLine(string.Format("Content-Type: {0}", response.ContentType));
            result.AppendLine();
            
            // Add response headers
            result.AppendLine("=== 回應標頭 (Response Headers) ===");
            foreach (string key in response.Headers.AllKeys)
            {
                result.AppendLine(string.Format("{0}: {1}", key, response.Headers[key]));
            }
            result.AppendLine();
            
            // Add response body
            result.AppendLine("=== 回應內容 (Response Body) ===");
            
            // Detect encoding from content-type
            Encoding encoding = GetEncodingFromContentType(response.ContentType);
            
            using (Stream responseStream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(responseStream, encoding))
            {
                string content = reader.ReadToEnd();
                result.Append(content);
            }
            
            return result.ToString();
        }

        /// <summary>
        /// Get encoding from content-type header
        /// 從 content-type 標頭取得編碼
        /// </summary>
        private Encoding GetEncodingFromContentType(string contentType)
        {
            if (string.IsNullOrEmpty(contentType))
            {
                return Encoding.UTF8;
            }

            // Try to parse charset
            string[] parts = contentType.Split(';');
            foreach (string part in parts)
            {
                string trimmedPart = part.Trim();
                if (trimmedPart.StartsWith("charset=", StringComparison.OrdinalIgnoreCase))
                {
                    string charset = trimmedPart.Substring(8).Trim('"', ' ');
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
        /// Save request data to file
        /// 將請求資料儲存到檔案
        /// </summary>
        public void SaveRequest(string filePath, RequestData data)
        {
            StringBuilder json = new StringBuilder();
            json.AppendLine("{");
            json.AppendLine(string.Format("  \"url\": \"{0}\",", EscapeJsonString(data.Url)));
            json.AppendLine(string.Format("  \"method\": \"{0}\",", EscapeJsonString(data.Method)));
            json.AppendLine(string.Format("  \"headers\": \"{0}\",", EscapeJsonString(data.Headers)));
            json.AppendLine(string.Format("  \"body\": \"{0}\"", EscapeJsonString(data.Body)));
            json.AppendLine("}");
            
            File.WriteAllText(filePath, json.ToString(), Encoding.UTF8);
        }

        /// <summary>
        /// Load request data from file
        /// 從檔案載入請求資料
        /// </summary>
        public RequestData LoadRequest(string filePath)
        {
            string json = File.ReadAllText(filePath, Encoding.UTF8);
            
            RequestData data = new RequestData();
            data.Url = ExtractJsonValue(json, "url");
            data.Method = ExtractJsonValue(json, "method");
            data.Headers = ExtractJsonValue(json, "headers");
            data.Body = ExtractJsonValue(json, "body");
            
            return data;
        }

        /// <summary>
        /// Escape string for JSON
        /// 為 JSON 轉義字串
        /// </summary>
        private string EscapeJsonString(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }
            
            return str
                .Replace("\\", "\\\\")
                .Replace("\"", "\\\"")
                .Replace("\r", "\\r")
                .Replace("\n", "\\n")
                .Replace("\t", "\\t");
        }

        /// <summary>
        /// Extract value from simple JSON string
        /// 從簡單 JSON 字串中提取值
        /// </summary>
        private string ExtractJsonValue(string json, string key)
        {
            string searchKey = "\"" + key + "\":";
            int keyIndex = json.IndexOf(searchKey);
            
            if (keyIndex < 0)
            {
                return string.Empty;
            }
            
            int valueStart = json.IndexOf('"', keyIndex + searchKey.Length) + 1;
            if (valueStart <= 0)
            {
                return string.Empty;
            }
            
            int valueEnd = valueStart;
            while (valueEnd < json.Length)
            {
                // Check if we found an unescaped closing quote
                if (json[valueEnd] == '"')
                {
                    // Check if the previous character is an escape character
                    // valueEnd is at least valueStart, so valueEnd - 1 >= valueStart - 1 >= 0
                    if (valueEnd == valueStart || json[valueEnd - 1] != '\\')
                    {
                        break;
                    }
                }
                valueEnd++;
            }
            
            string value = json.Substring(valueStart, valueEnd - valueStart);
            
            // Unescape JSON string
            return value
                .Replace("\\n", "\n")
                .Replace("\\r", "\r")
                .Replace("\\t", "\t")
                .Replace("\\\"", "\"")
                .Replace("\\\\", "\\");
        }
    }

    /// <summary>
    /// Data class to hold request information
    /// 保存請求資訊的資料類別
    /// </summary>
    public class RequestData
    {
        public string Url { get; set; }
        public string Method { get; set; }
        public string Headers { get; set; }
        public string Body { get; set; }

        public RequestData()
        {
            Url = string.Empty;
            Method = "GET";
            Headers = string.Empty;
            Body = string.Empty;
        }
    }
}
