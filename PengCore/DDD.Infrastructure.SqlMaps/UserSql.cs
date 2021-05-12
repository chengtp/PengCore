using System;
using System.Collections.Generic;
using System.Text;

namespace DDD.Infrastructure.SqlMaps
{
    public class UserSql
    {
        public static string UserInfoLogin(string LoginName, string Password)
        {
            return "select * from UserInfo where LoginName=@loginName and Password=@password and Disabled=0 and Deleted=0 ";
        }
    }
}
