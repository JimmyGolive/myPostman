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
        public AuthConfig Auth { get; set; }

        public RequestConfig()
        {
            Name = "";
            Url = "";
            Method = "GET";
            Headers = "";
            Body = "";
            Auth = new AuthConfig();
        }

        public RequestConfig(string name, string url, string method, string headers, string body)
        {
            Name = name;
            Url = url;
            Method = method;
            Headers = headers;
            Body = body;
            Auth = new AuthConfig();
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

                // Save authentication configuration
                if (config.Auth != null)
                {
                    XmlElement authElement = doc.CreateElement("Auth");
                    
                    XmlElement authTypeElement = doc.CreateElement("Type");
                    authTypeElement.InnerText = config.Auth.Type.ToString();
                    authElement.AppendChild(authTypeElement);
                    
                    XmlElement usernameElement = doc.CreateElement("Username");
                    usernameElement.InnerText = config.Auth.Username ?? "";
                    authElement.AppendChild(usernameElement);
                    
                    XmlElement passwordElement = doc.CreateElement("Password");
                    passwordElement.InnerText = config.Auth.Password ?? "";
                    authElement.AppendChild(passwordElement);
                    
                    XmlElement tokenElement = doc.CreateElement("Token");
                    tokenElement.InnerText = config.Auth.Token ?? "";
                    authElement.AppendChild(tokenElement);
                    
                    XmlElement apiKeyNameElement = doc.CreateElement("ApiKeyName");
                    apiKeyNameElement.InnerText = config.Auth.ApiKeyName ?? "";
                    authElement.AppendChild(apiKeyNameElement);
                    
                    XmlElement apiKeyValueElement = doc.CreateElement("ApiKeyValue");
                    apiKeyValueElement.InnerText = config.Auth.ApiKeyValue ?? "";
                    authElement.AppendChild(apiKeyValueElement);
                    
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
                        XmlNode authNode = node.SelectSingleNode("Auth");
                        if (authNode != null)
                        {
                            config.Auth = new AuthConfig();
                            
                            XmlNode authTypeNode = authNode.SelectSingleNode("Type");
                            if (authTypeNode != null)
                            {
                                try
                                {
                                    config.Auth.Type = (AuthType)Enum.Parse(typeof(AuthType), authTypeNode.InnerText);
                                }
                                catch
                                {
                                    config.Auth.Type = AuthType.None;
                                }
                            }
                            
                            XmlNode usernameNode = authNode.SelectSingleNode("Username");
                            if (usernameNode != null) config.Auth.Username = usernameNode.InnerText;
                            
                            XmlNode passwordNode = authNode.SelectSingleNode("Password");
                            if (passwordNode != null) config.Auth.Password = passwordNode.InnerText;
                            
                            XmlNode tokenNode = authNode.SelectSingleNode("Token");
                            if (tokenNode != null) config.Auth.Token = tokenNode.InnerText;
                            
                            XmlNode apiKeyNameNode = authNode.SelectSingleNode("ApiKeyName");
                            if (apiKeyNameNode != null) config.Auth.ApiKeyName = apiKeyNameNode.InnerText;
                            
                            XmlNode apiKeyValueNode = authNode.SelectSingleNode("ApiKeyValue");
                            if (apiKeyValueNode != null) config.Auth.ApiKeyValue = apiKeyValueNode.InnerText;
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

            // Export authentication configuration
            if (config.Auth != null)
            {
                XmlElement authElement = doc.CreateElement("Auth");
                
                XmlElement authTypeElement = doc.CreateElement("Type");
                authTypeElement.InnerText = config.Auth.Type.ToString();
                authElement.AppendChild(authTypeElement);
                
                XmlElement usernameElement = doc.CreateElement("Username");
                usernameElement.InnerText = config.Auth.Username ?? "";
                authElement.AppendChild(usernameElement);
                
                XmlElement passwordElement = doc.CreateElement("Password");
                passwordElement.InnerText = config.Auth.Password ?? "";
                authElement.AppendChild(passwordElement);
                
                XmlElement tokenElement = doc.CreateElement("Token");
                tokenElement.InnerText = config.Auth.Token ?? "";
                authElement.AppendChild(tokenElement);
                
                XmlElement apiKeyNameElement = doc.CreateElement("ApiKeyName");
                apiKeyNameElement.InnerText = config.Auth.ApiKeyName ?? "";
                authElement.AppendChild(apiKeyNameElement);
                
                XmlElement apiKeyValueElement = doc.CreateElement("ApiKeyValue");
                apiKeyValueElement.InnerText = config.Auth.ApiKeyValue ?? "";
                authElement.AppendChild(apiKeyValueElement);
                
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

                // Import authentication configuration
                XmlNode authNode = root.SelectSingleNode("Auth");
                if (authNode != null)
                {
                    config.Auth = new AuthConfig();
                    
                    XmlNode authTypeNode = authNode.SelectSingleNode("Type");
                    if (authTypeNode != null)
                    {
                        try
                        {
                            config.Auth.Type = (AuthType)Enum.Parse(typeof(AuthType), authTypeNode.InnerText);
                        }
                        catch
                        {
                            config.Auth.Type = AuthType.None;
                        }
                    }
                    
                    XmlNode usernameNode = authNode.SelectSingleNode("Username");
                    if (usernameNode != null) config.Auth.Username = usernameNode.InnerText;
                    
                    XmlNode passwordNode = authNode.SelectSingleNode("Password");
                    if (passwordNode != null) config.Auth.Password = passwordNode.InnerText;
                    
                    XmlNode tokenNode = authNode.SelectSingleNode("Token");
                    if (tokenNode != null) config.Auth.Token = tokenNode.InnerText;
                    
                    XmlNode apiKeyNameNode = authNode.SelectSingleNode("ApiKeyName");
                    if (apiKeyNameNode != null) config.Auth.ApiKeyName = apiKeyNameNode.InnerText;
                    
                    XmlNode apiKeyValueNode = authNode.SelectSingleNode("ApiKeyValue");
                    if (apiKeyValueNode != null) config.Auth.ApiKeyValue = apiKeyValueNode.InnerText;
                }
            }

            return config;
        }
    }
}
