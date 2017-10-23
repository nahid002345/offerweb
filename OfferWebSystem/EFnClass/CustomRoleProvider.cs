using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace OfferWebSystem.EFnClass
{
    public class CustomRoleProvider : RoleProvider
    {
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            
        }

        


        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
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
            using (OFFERDBEntities oEntity = new OFFERDBEntities())
            {
                var enumType = Convert.ToInt32(Enumaretion.DBEnumType.SysUserType);
                string[] ret = oEntity.SysEnums.Where(t=>t.EnumType == enumType).Select(x => x.EnumName.ToLower()).ToArray();
                return ret; 
            }
        }

        public override string[] GetRolesForUser(string UserId)
        {
            using (OFFERDBEntities oEntity = new OFFERDBEntities())
            {
                var enumType = Convert.ToInt32(Enumaretion.DBEnumType.SysUserType);
                string[] ret = oEntity.SysUsers.Where(t=>t.UserId == UserId).Select(x=>x.SysEnum.EnumName.ToLower()).ToArray();
                return ret;
            }
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            using (OFFERDBEntities db = new OFFERDBEntities())
            {
                SysUser user = db.SysUsers.FirstOrDefault(u => u.UserId == username && u.SysEnum.EnumName.ToLower().Contains(roleName.ToLower()));
                if (user != null)
                    return true;
                else
                    return false;
            }
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