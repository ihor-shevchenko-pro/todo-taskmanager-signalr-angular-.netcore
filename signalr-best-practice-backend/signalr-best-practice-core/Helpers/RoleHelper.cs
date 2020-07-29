using signalr_best_practice_core.Entities.UserAccount;
using System;
using System.Collections.Generic;
using System.Linq;

namespace signalr_best_practice_core.Helpers
{
    public class RoleHelper
    {
        private static readonly RoleHelper _instance = new RoleHelper();
        public static RoleHelper Current => _instance;

        private RoleHelper()
        {
            Admin = new Role() { Id = "89a586fe-2d77-49e4-122b-a9er197456f5", Name = "Admin" };
            User = new Role() { Id = "18a569fe-12f7-49c4-a22b-a909197a46f5", Name = "User" };

            Roles = new List<Role>()
            {
                Admin,
                User,
            };
        }

        public List<Role> Roles { get; private set; }

        public Role Admin { get; private set; }
        public Role User { get; private set; }

        public string GetName(string roleId)
        {
            var roleName = Roles.FirstOrDefault(x => x.Id == roleId).Name;
            return roleName ?? throw new ArgumentException($"Role {roleId} is not found");
        }

        public Role GetRole(string id)
        {
            return Roles.FirstOrDefault(x => x.Id == id);
        }
    }
}
