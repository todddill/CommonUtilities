using System.IdentityModel.Selectors;
using System.ServiceModel;
using System.ServiceModel.Security;

namespace WcfWsSecurityAuthentication
{
    public class CustomSecurityTokenManager : ClientCredentialsSecurityTokenManager
    {
        public CustomTokenSerializerSettings CustomTokenSerializerSettings { get; set; }

        public CustomSecurityTokenManager(CustomCredentials cred)
            : base(cred)
        {
            CustomTokenSerializerSettings = new CustomTokenSerializerSettings();
        }

        public override SecurityTokenSerializer CreateSecurityTokenSerializer(SecurityTokenVersion version)
        {
            return new CustomTokenSerializer(CustomTokenSerializerSettings);
        }
    }
}
