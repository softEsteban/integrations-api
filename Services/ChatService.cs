using System.Reflection.Metadata;
using IntegrationsApi.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Text;
using System;
using Microsoft.Net.Http.Headers;

namespace IntegrationsApi.Services
{
    public class ChatService
    {
        private readonly IConfiguration _configuration;
        private static readonly Dictionary<string, string> Users = new Dictionary<string, string>();

        public ChatService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            IConfiguration config = builder.Build();
            _configuration = configuration;
        }

        public bool AddUserToList(string userToAdd)
        {
            lock (Users)
            {
                foreach (var user in Users)
                {
                    if (user.Key.ToLower() == userToAdd.ToLower())
                    {
                        return false;
                    }
                }
                Users.Add(userToAdd, null);
                return true;
            }
        }

        public void AddUserConnectionId(string user, string connectionId)
        {
            lock (Users)
            {
                if (Users.ContainsKey(user))
                {
                    Users[user] = connectionId;
                }
            }
        }

        public string GetUserByConnectionId(string connectionId)
        {
            lock (Users)
            {
                return Users
                    .Where(x => x.Value == connectionId)
                    .Select(x => x.Key)
                    .FirstOrDefault();
            }
        }

        public string GetConnectionIdByUser(string user)
        {
            lock (Users)
            {
                return Users.Where(x => x.Key == user).Select(x => x.Value).FirstOrDefault();
            }
        }

        public void RemoveUserFromList(string user)
        {
            lock (Users)
            {
                if (Users.ContainsKey(user))
                {
                    Users.Remove(user);
                }
            }
        }

        public string[] GetOnlineUsers()
        {
            lock (Users)
            {
                return Users.OrderBy(x => x.Key).Select(x => x.Key).ToArray();
            }
        }
    }
}
