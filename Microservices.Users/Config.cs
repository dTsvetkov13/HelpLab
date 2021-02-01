using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Microservices.Users
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            var identityResources = new List<IdentityResource>();
            identityResources.Add(new IdentityResources.OpenId());
            identityResources.Add(new IdentityResources.Profile());
            identityResources.Add(new IdentityResources.Email());
            identityResources.Add(new IdentityResources.Phone());

            return identityResources;
        }

        public static IEnumerable<ApiResource> GetAPIs()
        {
            var apis = new List<ApiResource>();
            apis.Add(new ApiResource("Microservices.Users", "Microservices.Users") { Scopes = { "Microservices.Users" } });

            return apis;
        }

        public static IEnumerable<ApiScope> GetAPIScopes()
        {
            var apiScopes = new List<ApiScope> { new("Microservices.Users", "Microservices.Users") };

            return apiScopes;
        }

        public static IEnumerable<Client> GetClients()
        {
            var clients = new List<Client>();

            var apiClient = new Client
            {
                ClientId = "api",
                RequireClientSecret = false,
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    "Microservices.Users"
                }
            };

            clients.Add(apiClient);

            return clients;
        }
    }
}
