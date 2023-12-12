using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace TaHooK.IdentityProvider.App
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                new ("tahookclientaudience"),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new("tahookapi")
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new()
                {
                    ClientName = "TaHooK Client",
                    ClientId = "tahookclient",
                    AllowOfflineAccess = true,
                    RedirectUris = new List<string>
                    {
                        "https://oauth.pstmn.io/v1/callback", 
                        "https://localhost:7289/authentication/login-callback",
                       "https://app-iw5-2023-team-xpekni01-web.azurewebsites.net/authentication/login-callback"
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        "https://localhost:7289/",
                        "https://app-iw5-2023-team-xpekni01-web.azurewebsites.net/"
                    },
                    AllowedGrantTypes = new List<string>
                    {
                        GrantType.ClientCredentials,
                        GrantType.ResourceOwnerPassword,
                        GrantType.AuthorizationCode
                    },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "tahookapi"
                    },
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    RequireClientSecret = false
                }
            };
    }
}