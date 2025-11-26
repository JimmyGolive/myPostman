using System;

namespace myPostman
{
    /// <summary>
    /// Authentication types supported by myPostman
    /// </summary>
    public enum AuthType
    {
        None,
        BasicAuth,
        BearerToken,
        ApiKey
    }

    /// <summary>
    /// Authentication configuration for HTTP requests
    /// </summary>
    public class AuthConfig
    {
        public AuthType Type { get; set; }
        
        // Basic Auth fields
        public string Username { get; set; }
        public string Password { get; set; }
        
        // Bearer Token field
        public string Token { get; set; }
        
        // API Key fields
        public string ApiKeyName { get; set; }
        public string ApiKeyValue { get; set; }

        public AuthConfig()
        {
            Type = AuthType.None;
            Username = "";
            Password = "";
            Token = "";
            ApiKeyName = "";
            ApiKeyValue = "";
        }

        /// <summary>
        /// Creates a Basic Auth configuration
        /// </summary>
        public static AuthConfig CreateBasicAuth(string username, string password)
        {
            return new AuthConfig
            {
                Type = AuthType.BasicAuth,
                Username = username,
                Password = password
            };
        }

        /// <summary>
        /// Creates a Bearer Token configuration
        /// </summary>
        public static AuthConfig CreateBearerToken(string token)
        {
            return new AuthConfig
            {
                Type = AuthType.BearerToken,
                Token = token
            };
        }

        /// <summary>
        /// Creates an API Key configuration
        /// </summary>
        public static AuthConfig CreateApiKey(string keyName, string keyValue)
        {
            return new AuthConfig
            {
                Type = AuthType.ApiKey,
                ApiKeyName = keyName,
                ApiKeyValue = keyValue
            };
        }

        /// <summary>
        /// Returns the authentication header string based on the auth type
        /// </summary>
        public string GetAuthorizationHeader()
        {
            switch (Type)
            {
                case AuthType.BasicAuth:
                    if (!string.IsNullOrEmpty(Username))
                    {
                        string credentials = Username + ":" + Password;
                        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(credentials);
                        string base64 = Convert.ToBase64String(bytes);
                        return "Authorization: Basic " + base64;
                    }
                    break;

                case AuthType.BearerToken:
                    if (!string.IsNullOrEmpty(Token))
                    {
                        return "Authorization: Bearer " + Token;
                    }
                    break;

                case AuthType.ApiKey:
                    if (!string.IsNullOrEmpty(ApiKeyName) && !string.IsNullOrEmpty(ApiKeyValue))
                    {
                        return ApiKeyName + ": " + ApiKeyValue;
                    }
                    break;
            }

            return "";
        }
    }
}
