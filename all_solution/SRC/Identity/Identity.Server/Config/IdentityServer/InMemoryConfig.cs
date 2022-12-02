using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Security.Claims;

namespace Identity.Server.Config.IdentityServer
{
    public class InMemoryConfig
    {
        // returns a TestUser with some specific JWT Claims.
        public static List<TestUser> TestUsers => new List<TestUser>
        {
            new TestUser
            {
                SubjectId = "1144",
                Username = "mukesh",
                Password = "mukesh",
                Claims =
                {
                    new Claim(JwtClaimTypes.Name, "Mukesh Murugan"),
                    new Claim(JwtClaimTypes.GivenName, "Mukesh"),
                    new Claim(JwtClaimTypes.FamilyName, "Murugan"),
                    new Claim(JwtClaimTypes.WebSite, "http://codewithmukesh.com"),
                }
            }
        };

        // Identity Resources are data like userId, email, a phone number that is something unique to a particular identity/user
        public static IEnumerable<IdentityResource> IdentityResources => new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

        // main intention is to secure an API, so this API can have scopes. Scopes in the context of, what the authorized user can do
        public static IEnumerable<ApiScope> ApiScopes => new ApiScope[]
        {
            new ApiScope("Catalog.API", "Catalog API")
            /*
            new ApiScope("myApi.read"),
            new ApiScope("myApi.write"),
            */
        };

        // this defines the API itself. We will give it a name myApi and mention the supported scopes as well, along with the secret. Ensure to hash this secret code. This hashed code will be saved internally within IdentityServe
        public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
        {
            new ApiResource("Catalog.API", "Catalog API")
            {
                Scopes = { "Catalog.API" }
            } 
            /*
            new ApiResource("myApi")
            {
                Scopes = new List<string>{ "myApi.read","myApi.write" },
                ApiSecrets = new List<Secret>{ new Secret("supersecret".Sha256()) }
            }*/
        };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "company-employee",
                    ClientSecrets = new [] { new Secret("codemazesecret".Sha512()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes = { IdentityServerConstants.StandardScopes.OpenId, "Catalog.API" }
                 }
             };


        /*
        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                
                new Client
                {
                    ClientId = "cwm.client",
                    ClientName = "Client Credentials Client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedScopes = { "myApi.read" }
                },

            };*/
    }
}
