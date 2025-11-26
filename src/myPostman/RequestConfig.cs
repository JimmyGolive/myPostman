using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace myPostman
{
    /// <summary>
    /// Request configuration data structure for saving/loading
    /// </summary>
    public class RequestConfig
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Method { get; set; }
        public string Headers { get; set; }
        public string Body { get; set; }
        public AuthenticationConfig Authentication { get; set; }

        public RequestConfig()
        {
            Name = "";
            Url = "";
            Method = "GET";
            Headers = "";
            Body = "";
            Authentication = new AuthenticationConfig();
        }

        public RequestConfig(string name, string url, string method, string headers, string body)
        {
            Name = name;
            Url = url;
            Method = method;
            Headers = headers;
            Body = body;
            Authentication = new AuthenticationConfig();
        }

        public RequestConfig(string name, string url, string method, string headers, string body, AuthenticationConfig auth)
        {
            Name = name;
            Url = url;
            Method = method;
            Headers = headers;
            Body = body;
            Authentication = auth ?? new AuthenticationConfig();
        }
    }

    /// <summary>
    /// Helper class for saving and loading request configurations
    /// </summary>
    public class RequestConfigManager
    {
        private static string GetConfigFilePath()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string configDir = Path.Combine(appData, "myPostman");
            
            if (!Directory.Exists(configDir))
            {
                Directory.CreateDirectory(configDir);
            }

            return Path.Combine(configDir, "requests.xml");
        }

        /// <summary>
        /// Saves a list of request configurations to file
        /// </summary>
        public static void SaveConfigs(List<RequestConfig> configs)
        {
            string filePath = GetConfigFilePath();

            XmlDocument doc = new XmlDocument();
            XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(declaration);

            XmlElement root = doc.CreateElement("RequestConfigs");
            doc.AppendChild(root);

            foreach (RequestConfig config in configs)
            {
                XmlElement requestElement = doc.CreateElement("Request");

                XmlElement nameElement = doc.CreateElement("Name");
                nameElement.InnerText = config.Name ?? "";
                requestElement.AppendChild(nameElement);

                XmlElement urlElement = doc.CreateElement("Url");
                urlElement.InnerText = config.Url ?? "";
                requestElement.AppendChild(urlElement);

                XmlElement methodElement = doc.CreateElement("Method");
                methodElement.InnerText = config.Method ?? "GET";
                requestElement.AppendChild(methodElement);

                XmlElement headersElement = doc.CreateElement("Headers");
                headersElement.InnerText = config.Headers ?? "";
                requestElement.AppendChild(headersElement);

                XmlElement bodyElement = doc.CreateElement("Body");
                bodyElement.InnerText = config.Body ?? "";
                requestElement.AppendChild(bodyElement);

                // Add authentication configuration
                if (config.Authentication != null)
                {
                    XmlElement authElement = doc.CreateElement("Authentication");
                    
                    XmlElement authTypeElement = doc.CreateElement("Type");
                    authTypeElement.InnerText = config.Authentication.Type.ToString();
                    authElement.AppendChild(authTypeElement);

                    XmlElement usernameElement = doc.CreateElement("Username");
                    usernameElement.InnerText = config.Authentication.Username ?? "";
                    authElement.AppendChild(usernameElement);

                    XmlElement passwordElement = doc.CreateElement("Password");
                    passwordElement.InnerText = config.Authentication.Password ?? "";
                    authElement.AppendChild(passwordElement);

                    XmlElement tokenElement = doc.CreateElement("Token");
                    tokenElement.InnerText = config.Authentication.Token ?? "";
                    authElement.AppendChild(tokenElement);

                    XmlElement apiKeyNameElement = doc.CreateElement("ApiKeyName");
                    apiKeyNameElement.InnerText = config.Authentication.ApiKeyName ?? "";
                    authElement.AppendChild(apiKeyNameElement);

                    XmlElement apiKeyValueElement = doc.CreateElement("ApiKeyValue");
                    apiKeyValueElement.InnerText = config.Authentication.ApiKeyValue ?? "";
                    authElement.AppendChild(apiKeyValueElement);

                    XmlElement apiKeyLocationElement = doc.CreateElement("ApiKeyLocation");
                    apiKeyLocationElement.InnerText = config.Authentication.ApiKeyLocation.ToString();
                    authElement.AppendChild(apiKeyLocationElement);

                    requestElement.AppendChild(authElement);
                }

                root.AppendChild(requestElement);
            }

            doc.Save(filePath);
        }

        /// <summary>
        /// Loads request configurations from file
        /// </summary>
        public static List<RequestConfig> LoadConfigs()
        {
            List<RequestConfig> configs = new List<RequestConfig>();
            string filePath = GetConfigFilePath();

            if (!File.Exists(filePath))
            {
                return configs;
            }

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);

                XmlNodeList requestNodes = doc.SelectNodes("/RequestConfigs/Request");
                if (requestNodes != null)
                {
                    foreach (XmlNode node in requestNodes)
                    {
                        RequestConfig config = new RequestConfig();

                        XmlNode nameNode = node.SelectSingleNode("Name");
                        if (nameNode != null) config.Name = nameNode.InnerText;

                        XmlNode urlNode = node.SelectSingleNode("Url");
                        if (urlNode != null) config.Url = urlNode.InnerText;

                        XmlNode methodNode = node.SelectSingleNode("Method");
                        if (methodNode != null) config.Method = methodNode.InnerText;

                        XmlNode headersNode = node.SelectSingleNode("Headers");
                        if (headersNode != null) config.Headers = headersNode.InnerText;

                        XmlNode bodyNode = node.SelectSingleNode("Body");
                        if (bodyNode != null) config.Body = bodyNode.InnerText;

                        // Load authentication configuration
                        XmlNode authNode = node.SelectSingleNode("Authentication");
                        if (authNode != null)
                        {
                            config.Authentication = new AuthenticationConfig();

                            XmlNode authTypeNode = authNode.SelectSingleNode("Type");
                            if (authTypeNode != null)
                            {
                                try
                                {
                                    config.Authentication.Type = (AuthenticationType)Enum.Parse(typeof(AuthenticationType), authTypeNode.InnerText);
                                }
                                catch
                                {
                                    config.Authentication.Type = AuthenticationType.None;
                                }
                            }

                            XmlNode usernameNode = authNode.SelectSingleNode("Username");
                            if (usernameNode != null) config.Authentication.Username = usernameNode.InnerText;

                            XmlNode passwordNode = authNode.SelectSingleNode("Password");
                            if (passwordNode != null) config.Authentication.Password = passwordNode.InnerText;

                            XmlNode tokenNode = authNode.SelectSingleNode("Token");
                            if (tokenNode != null) config.Authentication.Token = tokenNode.InnerText;

                            XmlNode apiKeyNameNode = authNode.SelectSingleNode("ApiKeyName");
                            if (apiKeyNameNode != null) config.Authentication.ApiKeyName = apiKeyNameNode.InnerText;

                            XmlNode apiKeyValueNode = authNode.SelectSingleNode("ApiKeyValue");
                            if (apiKeyValueNode != null) config.Authentication.ApiKeyValue = apiKeyValueNode.InnerText;

                            XmlNode apiKeyLocationNode = authNode.SelectSingleNode("ApiKeyLocation");
                            if (apiKeyLocationNode != null)
                            {
                                try
                                {
                                    config.Authentication.ApiKeyLocation = (ApiKeyLocation)Enum.Parse(typeof(ApiKeyLocation), apiKeyLocationNode.InnerText);
                                }
                                catch
                                {
                                    config.Authentication.ApiKeyLocation = ApiKeyLocation.Header;
                                }
                            }
                        }

                        configs.Add(config);
                    }
                }
            }
            catch (Exception)
            {
                // Return empty list if file is corrupted
                return new List<RequestConfig>();
            }

            return configs;
        }

        /// <summary>
        /// Exports a single request configuration to a file
        /// </summary>
        public static void ExportConfig(RequestConfig config, string filePath)
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(declaration);

            XmlElement root = doc.CreateElement("Request");
            doc.AppendChild(root);

            XmlElement nameElement = doc.CreateElement("Name");
            nameElement.InnerText = config.Name ?? "";
            root.AppendChild(nameElement);

            XmlElement urlElement = doc.CreateElement("Url");
            urlElement.InnerText = config.Url ?? "";
            root.AppendChild(urlElement);

            XmlElement methodElement = doc.CreateElement("Method");
            methodElement.InnerText = config.Method ?? "GET";
            root.AppendChild(methodElement);

            XmlElement headersElement = doc.CreateElement("Headers");
            headersElement.InnerText = config.Headers ?? "";
            root.AppendChild(headersElement);

            XmlElement bodyElement = doc.CreateElement("Body");
            bodyElement.InnerText = config.Body ?? "";
            root.AppendChild(bodyElement);

            // Add authentication configuration
            if (config.Authentication != null)
            {
                XmlElement authElement = doc.CreateElement("Authentication");
                
                XmlElement authTypeElement = doc.CreateElement("Type");
                authTypeElement.InnerText = config.Authentication.Type.ToString();
                authElement.AppendChild(authTypeElement);

                XmlElement usernameElement = doc.CreateElement("Username");
                usernameElement.InnerText = config.Authentication.Username ?? "";
                authElement.AppendChild(usernameElement);

                XmlElement passwordElement = doc.CreateElement("Password");
                passwordElement.InnerText = config.Authentication.Password ?? "";
                authElement.AppendChild(passwordElement);

                XmlElement tokenElement = doc.CreateElement("Token");
                tokenElement.InnerText = config.Authentication.Token ?? "";
                authElement.AppendChild(tokenElement);

                XmlElement apiKeyNameElement = doc.CreateElement("ApiKeyName");
                apiKeyNameElement.InnerText = config.Authentication.ApiKeyName ?? "";
                authElement.AppendChild(apiKeyNameElement);

                XmlElement apiKeyValueElement = doc.CreateElement("ApiKeyValue");
                apiKeyValueElement.InnerText = config.Authentication.ApiKeyValue ?? "";
                authElement.AppendChild(apiKeyValueElement);

                XmlElement apiKeyLocationElement = doc.CreateElement("ApiKeyLocation");
                apiKeyLocationElement.InnerText = config.Authentication.ApiKeyLocation.ToString();
                authElement.AppendChild(apiKeyLocationElement);

                root.AppendChild(authElement);
            }

            doc.Save(filePath);
        }

        /// <summary>
        /// Imports a request configuration from a file
        /// </summary>
        public static RequestConfig ImportConfig(string filePath)
        {
            RequestConfig config = new RequestConfig();

            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);

            XmlNode root = doc.SelectSingleNode("/Request");
            if (root != null)
            {
                XmlNode nameNode = root.SelectSingleNode("Name");
                if (nameNode != null) config.Name = nameNode.InnerText;

                XmlNode urlNode = root.SelectSingleNode("Url");
                if (urlNode != null) config.Url = urlNode.InnerText;

                XmlNode methodNode = root.SelectSingleNode("Method");
                if (methodNode != null) config.Method = methodNode.InnerText;

                XmlNode headersNode = root.SelectSingleNode("Headers");
                if (headersNode != null) config.Headers = headersNode.InnerText;

                XmlNode bodyNode = root.SelectSingleNode("Body");
                if (bodyNode != null) config.Body = bodyNode.InnerText;

                // Load authentication configuration
                XmlNode authNode = root.SelectSingleNode("Authentication");
                if (authNode != null)
                {
                    config.Authentication = new AuthenticationConfig();

                    XmlNode authTypeNode = authNode.SelectSingleNode("Type");
                    if (authTypeNode != null)
                    {
                        try
                        {
                            config.Authentication.Type = (AuthenticationType)Enum.Parse(typeof(AuthenticationType), authTypeNode.InnerText);
                        }
                        catch
                        {
                            config.Authentication.Type = AuthenticationType.None;
                        }
                    }

                    XmlNode usernameNode = authNode.SelectSingleNode("Username");
                    if (usernameNode != null) config.Authentication.Username = usernameNode.InnerText;

                    XmlNode passwordNode = authNode.SelectSingleNode("Password");
                    if (passwordNode != null) config.Authentication.Password = passwordNode.InnerText;

                    XmlNode tokenNode = authNode.SelectSingleNode("Token");
                    if (tokenNode != null) config.Authentication.Token = tokenNode.InnerText;

                    XmlNode apiKeyNameNode = authNode.SelectSingleNode("ApiKeyName");
                    if (apiKeyNameNode != null) config.Authentication.ApiKeyName = apiKeyNameNode.InnerText;

                    XmlNode apiKeyValueNode = authNode.SelectSingleNode("ApiKeyValue");
                    if (apiKeyValueNode != null) config.Authentication.ApiKeyValue = apiKeyValueNode.InnerText;

                    XmlNode apiKeyLocationNode = authNode.SelectSingleNode("ApiKeyLocation");
                    if (apiKeyLocationNode != null)
                    {
                        try
                        {
                            config.Authentication.ApiKeyLocation = (ApiKeyLocation)Enum.Parse(typeof(ApiKeyLocation), apiKeyLocationNode.InnerText);
                        }
                        catch
                        {
                            config.Authentication.ApiKeyLocation = ApiKeyLocation.Header;
                        }
                    }
                }
            }

            return config;
        }
    }
}
