using System;
using System.Text;
using System.Text.RegularExpressions;

namespace myPostman
{
    /// <summary>
    /// Helper class for encoding conversions and text processing
    /// </summary>
    public class EncodingHelper
    {
        /// <summary>
        /// Processes the response text, converting hex-encoded characters to readable text
        /// Supports Traditional Chinese (Big5), Simplified Chinese (GB2312/GBK), and UTF-8
        /// </summary>
        /// <param name="text">Input text that may contain hex-encoded characters</param>
        /// <returns>Processed text with decoded characters</returns>
        public static string ProcessResponse(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            string result = text;

            // Process Unicode escape sequences (\uXXXX)
            result = DecodeUnicodeEscapes(result);

            // Process URL-encoded sequences (%XX)
            result = DecodeUrlEncodedSequences(result);

            // Process hex sequences (0xXX or \xXX)
            result = DecodeHexSequences(result);

            // Process HTML entities
            result = DecodeHtmlEntities(result);

            return result;
        }

        /// <summary>
        /// Decodes Unicode escape sequences like \u4E2D\u6587 (中文)
        /// </summary>
        private static string DecodeUnicodeEscapes(string text)
        {
            try
            {
                // Pattern for \uXXXX
                Regex regex = new Regex(@"\\u([0-9A-Fa-f]{4})", RegexOptions.Compiled);
                
                return regex.Replace(text, delegate(Match match)
                {
                    try
                    {
                        string hex = match.Groups[1].Value;
                        int charCode = Convert.ToInt32(hex, 16);
                        return ((char)charCode).ToString();
                    }
                    catch
                    {
                        return match.Value;
                    }
                });
            }
            catch
            {
                return text;
            }
        }

        /// <summary>
        /// Decodes URL-encoded sequences like %E4%B8%AD%E6%96%87 (中文 in UTF-8)
        /// </summary>
        private static string DecodeUrlEncodedSequences(string text)
        {
            try
            {
                // First try to find consecutive %XX patterns (likely UTF-8)
                Regex consecutivePattern = new Regex(@"(%[0-9A-Fa-f]{2})+", RegexOptions.Compiled);
                
                return consecutivePattern.Replace(text, delegate(Match match)
                {
                    try
                    {
                        string encoded = match.Value;
                        
                        // Extract all hex bytes
                        Regex bytePattern = new Regex(@"%([0-9A-Fa-f]{2})");
                        MatchCollection byteMatches = bytePattern.Matches(encoded);
                        
                        byte[] bytes = new byte[byteMatches.Count];
                        for (int i = 0; i < byteMatches.Count; i++)
                        {
                            bytes[i] = Convert.ToByte(byteMatches[i].Groups[1].Value, 16);
                        }

                        // Try UTF-8 first
                        string decoded = TryDecodeBytes(bytes);
                        
                        // If decoding produces replacement characters, return original
                        if (decoded.Contains("\uFFFD"))
                        {
                            return match.Value;
                        }
                        
                        return decoded;
                    }
                    catch
                    {
                        return match.Value;
                    }
                });
            }
            catch
            {
                return text;
            }
        }

        /// <summary>
        /// Decodes hex sequences like 0xE4B8AD or \xE4\xB8\xAD
        /// </summary>
        private static string DecodeHexSequences(string text)
        {
            try
            {
                // Pattern for consecutive \xXX sequences
                Regex hexPattern = new Regex(@"(\\x[0-9A-Fa-f]{2})+", RegexOptions.Compiled);
                
                text = hexPattern.Replace(text, delegate(Match match)
                {
                    try
                    {
                        Regex bytePattern = new Regex(@"\\x([0-9A-Fa-f]{2})");
                        MatchCollection byteMatches = bytePattern.Matches(match.Value);
                        
                        byte[] bytes = new byte[byteMatches.Count];
                        for (int i = 0; i < byteMatches.Count; i++)
                        {
                            bytes[i] = Convert.ToByte(byteMatches[i].Groups[1].Value, 16);
                        }

                        string decoded = TryDecodeBytes(bytes);
                        
                        if (decoded.Contains("\uFFFD"))
                        {
                            return match.Value;
                        }
                        
                        return decoded;
                    }
                    catch
                    {
                        return match.Value;
                    }
                });

                // Pattern for 0xXX format (less common, usually single bytes)
                Regex hex0xPattern = new Regex(@"0x([0-9A-Fa-f]{2})(?:\s*0x[0-9A-Fa-f]{2})*", RegexOptions.Compiled);
                
                text = hex0xPattern.Replace(text, delegate(Match match)
                {
                    try
                    {
                        Regex bytePattern = new Regex(@"0x([0-9A-Fa-f]{2})");
                        MatchCollection byteMatches = bytePattern.Matches(match.Value);
                        
                        byte[] bytes = new byte[byteMatches.Count];
                        for (int i = 0; i < byteMatches.Count; i++)
                        {
                            bytes[i] = Convert.ToByte(byteMatches[i].Groups[1].Value, 16);
                        }

                        string decoded = TryDecodeBytes(bytes);
                        
                        if (decoded.Contains("\uFFFD"))
                        {
                            return match.Value;
                        }
                        
                        return decoded;
                    }
                    catch
                    {
                        return match.Value;
                    }
                });

                return text;
            }
            catch
            {
                return text;
            }
        }

        /// <summary>
        /// Decodes HTML entities like &amp;#20013; or &amp;#x4E2D; (中)
        /// </summary>
        private static string DecodeHtmlEntities(string text)
        {
            try
            {
                // Decode numeric entities (&#XXXXX;)
                Regex decimalPattern = new Regex(@"&#(\d+);", RegexOptions.Compiled);
                text = decimalPattern.Replace(text, delegate(Match match)
                {
                    try
                    {
                        int charCode = int.Parse(match.Groups[1].Value);
                        if (charCode <= 0x10FFFF)
                        {
                            if (charCode <= 0xFFFF)
                            {
                                return ((char)charCode).ToString();
                            }
                            else
                            {
                                // Surrogate pair for characters beyond BMP
                                charCode -= 0x10000;
                                char high = (char)(0xD800 + (charCode >> 10));
                                char low = (char)(0xDC00 + (charCode & 0x3FF));
                                return new string(new char[] { high, low });
                            }
                        }
                        return match.Value;
                    }
                    catch
                    {
                        return match.Value;
                    }
                });

                // Decode hex entities (&#xXXXX;)
                Regex hexPattern = new Regex(@"&#x([0-9A-Fa-f]+);", RegexOptions.Compiled);
                text = hexPattern.Replace(text, delegate(Match match)
                {
                    try
                    {
                        int charCode = Convert.ToInt32(match.Groups[1].Value, 16);
                        if (charCode <= 0x10FFFF)
                        {
                            if (charCode <= 0xFFFF)
                            {
                                return ((char)charCode).ToString();
                            }
                            else
                            {
                                charCode -= 0x10000;
                                char high = (char)(0xD800 + (charCode >> 10));
                                char low = (char)(0xDC00 + (charCode & 0x3FF));
                                return new string(new char[] { high, low });
                            }
                        }
                        return match.Value;
                    }
                    catch
                    {
                        return match.Value;
                    }
                });

                // Decode common named entities
                text = text.Replace("&lt;", "<");
                text = text.Replace("&gt;", ">");
                text = text.Replace("&amp;", "&");
                text = text.Replace("&quot;", "\"");
                text = text.Replace("&apos;", "'");
                text = text.Replace("&nbsp;", " ");

                return text;
            }
            catch
            {
                return text;
            }
        }

        /// <summary>
        /// Tries to decode bytes using different encodings
        /// Priority: UTF-8 > GBK (Simplified Chinese) > Big5 (Traditional Chinese) > ASCII
        /// </summary>
        private static string TryDecodeBytes(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
            {
                return "";
            }

            // Try UTF-8 first
            try
            {
                Encoding utf8 = new UTF8Encoding(false, true);
                string result = utf8.GetString(bytes);
                
                // Verify the result doesn't contain replacement characters
                if (!result.Contains("\uFFFD"))
                {
                    return result;
                }
            }
            catch { }

            // Try GBK (Simplified Chinese - code page 936)
            try
            {
                Encoding gbk = Encoding.GetEncoding(936);
                string result = gbk.GetString(bytes);
                return result;
            }
            catch { }

            // Try Big5 (Traditional Chinese - code page 950)
            try
            {
                Encoding big5 = Encoding.GetEncoding(950);
                string result = big5.GetString(bytes);
                return result;
            }
            catch { }

            // Fall back to default encoding
            return Encoding.Default.GetString(bytes);
        }

        /// <summary>
        /// Formats JSON text with proper indentation for readability
        /// </summary>
        public static string FormatJson(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return json;
            }

            // Simple check if it looks like JSON
            string trimmed = json.Trim();
            if (!trimmed.StartsWith("{") && !trimmed.StartsWith("["))
            {
                return json;
            }

            try
            {
                StringBuilder sb = new StringBuilder();
                int indent = 0;
                bool inString = false;
                bool escaped = false;

                foreach (char c in json)
                {
                    if (escaped)
                    {
                        sb.Append(c);
                        escaped = false;
                        continue;
                    }

                    if (c == '\\' && inString)
                    {
                        sb.Append(c);
                        escaped = true;
                        continue;
                    }

                    if (c == '"' && !escaped)
                    {
                        inString = !inString;
                        sb.Append(c);
                        continue;
                    }

                    if (inString)
                    {
                        sb.Append(c);
                        continue;
                    }

                    switch (c)
                    {
                        case '{':
                        case '[':
                            sb.Append(c);
                            sb.AppendLine();
                            indent++;
                            sb.Append(new string(' ', indent * 2));
                            break;
                        case '}':
                        case ']':
                            sb.AppendLine();
                            indent--;
                            sb.Append(new string(' ', indent * 2));
                            sb.Append(c);
                            break;
                        case ',':
                            sb.Append(c);
                            sb.AppendLine();
                            sb.Append(new string(' ', indent * 2));
                            break;
                        case ':':
                            sb.Append(c);
                            sb.Append(' ');
                            break;
                        case ' ':
                        case '\t':
                        case '\r':
                        case '\n':
                            // Skip whitespace outside strings
                            break;
                        default:
                            sb.Append(c);
                            break;
                    }
                }

                return sb.ToString();
            }
            catch
            {
                return json;
            }
        }

        /// <summary>
        /// Detects if text is likely JSON
        /// </summary>
        public static bool IsJson(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            string trimmed = text.Trim();
            return (trimmed.StartsWith("{") && trimmed.EndsWith("}")) ||
                   (trimmed.StartsWith("[") && trimmed.EndsWith("]"));
        }

        /// <summary>
        /// Detects if text is likely XML
        /// </summary>
        public static bool IsXml(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            string trimmed = text.Trim();
            return trimmed.StartsWith("<") && trimmed.EndsWith(">");
        }
    }
}
