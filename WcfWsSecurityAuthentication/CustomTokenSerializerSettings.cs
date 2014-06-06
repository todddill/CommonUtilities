using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.ServiceModel.Security;
using System.Text;

namespace WcfWsSecurityAuthentication
{
    public class CustomTokenSerializerSettings
    {
        private string _guid = Guid.NewGuid().ToString();

        public SecurityVersion SecurityVersion{ get; set; }

        public string TokenNamespace { get; set; }

        public string TokenElementNamespace { get; set; }

        public string CreatedDate { get; set; }

        public CustomTokenSerializerSettings()
        {
            TokenNamespace = "wss";
            TokenElementNamespace = "wsu";
            CreatedDate = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss.fffZ");
            SecurityVersion = SecurityVersion.WSSecurity11;
        }

        public string Nonce
        {
            get 
            { 
                return GetSHA1String(_guid); 
            }
        }

        private string GetPasswordDigest(string[] args)
        {
            string stringToHash = string.Empty;
            foreach (string arg in args) {
                stringToHash += arg;
            }

            return GetSHA1String(stringToHash);
        }

        private string GetSHA1String(string stringToHash)
        {
            SHA1CryptoServiceProvider sha1Hasher = new SHA1CryptoServiceProvider();
            byte[] hashedBytes = sha1Hasher.ComputeHash(Encoding.UTF8.GetBytes(stringToHash));
            return Convert.ToBase64String(hashedBytes);
        }

        public string GetPassword(string password, bool usePasswordDigest)
        {
            if (!string.IsNullOrEmpty(password))
            {
                if (usePasswordDigest) return GetPasswordDigest(new List<string> { CreatedDate, Nonce, password }.ToArray());
            }

            return password;
        }
    }
}
