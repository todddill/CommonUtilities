using System.IdentityModel.Selectors;
using System.ServiceModel.Description;
using System.ServiceModel.Security;

namespace WcfWsSecurityAuthentication
{
    public class CustomCredentials : ClientCredentials
    {
        public string TokenNamespace { get; set; }
        public string TokenElementNamespace { get; set; }
        public string CreatedDate { get; set; }

        private SecurityVersion _securityVersion;

        public CustomCredentials(SecurityVersion securityVersion)
        {
            _securityVersion = securityVersion;
        }

        protected CustomCredentials(CustomCredentials cc)
            : base(cc)
        {
            
        }

        public override SecurityTokenManager CreateSecurityTokenManager()
        {
            CustomSecurityTokenManager customSecurityTokenManager = new CustomSecurityTokenManager(this);
            CustomTokenSerializerSettings settings = new CustomTokenSerializerSettings {
                SecurityVersion = _securityVersion
            };

            if (!string.IsNullOrEmpty(TokenNamespace)) settings.TokenNamespace = TokenNamespace;
            if (!string.IsNullOrEmpty(TokenElementNamespace)) settings.TokenElementNamespace = TokenElementNamespace;
            if (!string.IsNullOrEmpty(CreatedDate)) settings.CreatedDate = CreatedDate;

            return new CustomSecurityTokenManager(this);
        }

        protected override ClientCredentials CloneCore()
        {
            return new CustomCredentials(this);
        }
    }
}
