using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Security.Claims;

namespace Identity.API
{
    public static class InMemoryConfig
    {
        // Identity resources map to scopes that give access to identity-related information
        // With the OpenId method, we support a subject id or sub value to be included.We include the Profile method as well to support profile information like given_name or family_name.
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource
                {
                    Name = "role",
                    UserClaims = new List<string> { "role" }
                }
            };

        public static List<TestUser> GetUsers() =>
            new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "a9ea0f25-b964-409f-bcce-c923266249b4",
                    Username = "Jaro_Manager",
                    Password = "JaroPassw0rd",
                    Claims = new List<Claim>
                    {
                        new Claim("given_name", "Jaro"),
                        new Claim("family_name", "Biedron"),
                        new Claim("role", "Manager")
                    }
                },
                new TestUser
                {
                    SubjectId = "c95ddb8c-79ec-488a-a485-fe57a1462340",
                    Username = "Jaro_Buyer",
                    Password = "JaroPassw0rd",
                    Claims = new List<Claim>
                    {
                        new Claim("given_name", "Jaro"),
                        new Claim("family_name", "Biedron"),
                        new Claim("role", "Buyer")
                    }
                }
            };

        public static IEnumerable<Client> GetClients() =>
            new List<Client>
            {
               new Client
               {
                    ClientId = "client_api",
                    ClientSecrets = new [] { new Secret("client_api_pass".Sha512()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes = { IdentityServerConstants.StandardScopes.OpenId, "Catalog.API", "roles" }
                }
            };

       public static IEnumerable<ApiScope> ApiScopes => new ApiScope[]
       {
            new ApiScope("Catalog.API", "Catalog API"),
            new ApiScope("roles", "My Roles"),
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
            } ,
            new ApiResource("roles", "My Roles", new[] { "role" })
            { 
                Scopes = { "roles" }
            },
            /*
            new ApiResource("myApi")
            {
                Scopes = new List<string>{ "myApi.read","myApi.write" },
                ApiSecrets = new List<Secret>{ new Secret("supersecret".Sha256()) }
            }*/
        };
    }
}
