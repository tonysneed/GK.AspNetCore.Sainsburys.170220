// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ResourceOwnerClient
{
    public class Program
    {
        public static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

        private static async Task MainAsync()
        {

            TokenResponse tokenResponse = null;
            Console.Title = "Client";
            Console.WriteLine("Resource Owner Client");
            Console.WriteLine("---------------------");

            // TODO: Prompt for username and password
            //Console.WriteLine("\nAuthenticate User? {Y/N}");
            //if (Console.ReadLine().ToUpper() == "Y")
            //{
            //    string username, password;
            //    GetCredentials(out username, out password);

            //    tokenResponse = await GetToken(username, password);
            //    Console.WriteLine("Access Token:");
            //    if (tokenResponse.IsError)
            //    {
            //        Console.WriteLine(tokenResponse.Error);
            //        return;
            //    }
            //    Console.WriteLine(tokenResponse.Json);
            //}

            Console.WriteLine("\nPress Enter to invoke Web API.");
            Console.ReadLine();

            var client = new HttpClient();
            await GetApiResponse(client, "http://localhost:5001/api/hello", "Message", tokenResponse);
            Console.WriteLine();
            await GetApiResponse(client, "http://localhost:5001/api/claims", "Claims", tokenResponse);

        }

        private static void GetCredentials(out string username, out string password)
        {
            username = "alice";
            Console.WriteLine($"\nUsername ({username}):");
            var input1 = Console.ReadLine();
            username = string.IsNullOrWhiteSpace(input1) ? username : input1;

            password = "password";
            Console.WriteLine($"Password ({password}):");
            var input2 = Console.ReadLine();
            password = string.IsNullOrWhiteSpace(input2) ? password : input2;
        }

        private async static Task GetApiResponse(HttpClient client, string uri, string title,
            TokenResponse tokenResponse)
        {
            // TODO: Set token on http client
            //if (tokenResponse != null)
            //    client.SetBearerToken(tokenResponse.AccessToken);

            var response = await client.GetAsync(uri);

            Console.WriteLine($"{title}:");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(JArray.Parse(content));
            }
        }

        private static async Task<TokenResponse> GetToken(string username, string password)
        {
            Console.WriteLine("Press Enter to discover endpoints from metadata.");
            Console.ReadLine();

            // TODO: Discover token endpoint
            //var disco = await DiscoveryClient.GetAsync("http://localhost:5000");
            //Console.WriteLine($"Token endpoint: {disco.TokenEndpoint}");

            Console.WriteLine("\nPress Enter to retrieve access token from service.");
            Console.ReadLine();

            // TODO: Get access token for resource owner with username and password
            //var tokenClient = new TokenClient(disco.TokenEndpoint, "ro.client", "secret");
            //var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync(username, password, "api1");
            //return tokenResponse;

            // TODO: Remove
            return await Task.FromResult<TokenResponse>(null);
        }
    }
}