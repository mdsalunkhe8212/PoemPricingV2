using POEM.Model.Model;
using POEM.Services.Repository;
using System;
using System.Linq;
using System.Web.Security;

namespace POEMPricing.Security
{
    public class CustomRoleProvider : RoleProvider
    {
        public override string[] GetRolesForUser(string username)
        {
            using (var db = new ApplicationDbContext())
            {
                var role = (from u in db.Users
                            join r in db.UserRoles
                                on u.RoleId equals r.RoleId
                            where u.Email == username
                                  && u.IsActive
                            select r.Role)
                           .FirstOrDefault();

                if (string.IsNullOrEmpty(role))
                    return new string[] { };

                return new[] { role };
            }
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            return GetRolesForUser(username)
                .Contains(roleName);
        }

        public override string ApplicationName
        {
            get { return "POEM"; }
            set { }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}