using System;
using System.Text;

namespace myPostman
{
    /// <summary>
    /// Authentication types supported by the application
    /// </summary>
    public enum AuthenticationType
    {
        None,
        Basic,
        BearerToken,
        ApiKey,
        OAuth2
    }

    /// <summary>
    /// API Key location options
    /// </summary>
    public enum ApiKeyLocation
    {
        Header,
        QueryParam
    }

    /// <summary>
    /// Configuration for authentication settings
    /// </summary>
    public class AuthenticationConfig
    {
        public AuthenticationType Type { get; set; }
        
        // Basic Auth
        public string Username { get; set; }
        public string Password { get; set; }
        
        // Bearer Token / OAuth2
        public string Token { get; set; }
        
        // API Key
        public string ApiKeyName { get; set; }
        public string ApiKeyValue { get; set; }
        public ApiKeyLocation ApiKeyLocation { get; set; }

        public AuthenticationConfig()
        {
            Type = AuthenticationType.None;
            Username = "";
            Password = "";
            Token = "";
            ApiKeyName = "";
            ApiKeyValue = "";
            ApiKeyLocation = ApiKeyLocation.Header;
        }
    }

    /// <summary>
    /// Helper class for handling authentication
    /// </summary>
    public class AuthenticationHelper
    {
        /// <summary>
        /// Generates the Authorization header value for Basic authentication
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password (can be empty for some APIs)</param>
        /// <returns>Base64-encoded credentials</returns>
        /// <remarks>
        /// Note: Empty passwords are allowed as some APIs may accept username-only Basic auth.
        /// The password will be treated as an empty string if null.
        /// </remarks>
        public static string GetBasicAuthHeader(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("Username cannot be empty");
            }

            // Allow empty password as some APIs may accept it
            if (password == null)
            {
                password = "";
            }

            string credentials = username + ":" + password;
            byte[] credentialsBytes = Encoding.UTF8.GetBytes(credentials);
            string base64Credentials = Convert.ToBase64String(credentialsBytes);
            return "Basic " + base64Credentials;
        }

        /// <summary>
        /// Generates the Authorization header value for Bearer token authentication
        /// </summary>
        /// <param name="token">Bearer token (JWT, OAuth token, etc.)</param>
        /// <returns>Bearer token header value</returns>
        public static string GetBearerTokenHeader(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException("Token cannot be empty");
            }

            return "Bearer " + token;
        }

        /// <summary>
        /// Applies authentication to the headers string
        /// </summary>
        /// <param name="headers">Existing headers</param>
        /// <param name="config">Authentication configuration</param>
        /// <returns>Updated headers with authentication</returns>
        public static string ApplyAuthentication(string headers, AuthenticationConfig config)
        {
            if (config == null || config.Type == AuthenticationType.None)
            {
                return headers;
            }

            StringBuilder headerBuilder = new StringBuilder();
            
            // Add existing headers first
            if (!string.IsNullOrEmpty(headers))
            {
                headerBuilder.Append(headers);
                if (!headers.EndsWith("\n") && !headers.EndsWith("\r\n"))
                {
                    headerBuilder.AppendLine();
                }
            }

            // Add authentication header based on type
            switch (config.Type)
            {
                case AuthenticationType.Basic:
                    if (!string.IsNullOrEmpty(config.Username))
                    {
                        string authHeader = GetBasicAuthHeader(config.Username, config.Password ?? "");
                        headerBuilder.AppendLine("Authorization: " + authHeader);
                    }
                    break;

                case AuthenticationType.BearerToken:
                case AuthenticationType.OAuth2:
                    if (!string.IsNullOrEmpty(config.Token))
                    {
                        string authHeader = GetBearerTokenHeader(config.Token);
                        headerBuilder.AppendLine("Authorization: " + authHeader);
                    }
                    break;

                case AuthenticationType.ApiKey:
                    if (config.ApiKeyLocation == ApiKeyLocation.Header && 
                        !string.IsNullOrEmpty(config.ApiKeyName) && 
                        !string.IsNullOrEmpty(config.ApiKeyValue))
                    {
                        headerBuilder.AppendLine(config.ApiKeyName + ": " + config.ApiKeyValue);
                    }
                    // Query param is handled separately in URL
                    break;
            }

            return headerBuilder.ToString().TrimEnd('\r', '\n');
        }

        /// <summary>
        /// Applies API Key to URL if configured as query parameter
        /// </summary>
        /// <param name="url">Original URL</param>
        /// <param name="config">Authentication configuration</param>
        /// <returns>Updated URL with API key if applicable</returns>
        public static string ApplyApiKeyToUrl(string url, AuthenticationConfig config)
        {
            if (config == null || 
                config.Type != AuthenticationType.ApiKey || 
                config.ApiKeyLocation != ApiKeyLocation.QueryParam ||
                string.IsNullOrEmpty(config.ApiKeyName) ||
                string.IsNullOrEmpty(config.ApiKeyValue))
            {
                return url;
            }

            string separator = url.Contains("?") ? "&" : "?";
            return url + separator + Uri.EscapeDataString(config.ApiKeyName) + "=" + Uri.EscapeDataString(config.ApiKeyValue);
        }
    }
}
