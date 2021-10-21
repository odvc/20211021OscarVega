using API.Security.Entities;
using API.Security.Providers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;


namespace API.Security
{
    public static class SigninManager
    {
        public static Identity SignIn(string user, string password)
        {
            Identity userAuthenticated = null;
            //Identity userValidated = null;
            int authenticateType = int.Parse(ConfigurationManager.AppSettings["AuthenticateType"]);

            //Checking for credentials
            switch (authenticateType)
            {
                case 0:
                    userAuthenticated = SingInLdap(user, password);
                    break;
                //case 1:
                //    userAuthenticated = SingInLocal(user, password);
                //    break;
                //case 2:
                //   When I've decided use ASPNetUsers or when I cant implement it

                default:
                    break;
            }


            return userAuthenticated;

        }

        private static Identity SingInLdap(string user, string password)
        {
            string[] Domain = user.Split('@');
            string PathLDAP = "LDAP://";
            string dnsDomain = "";
            Entities.Identity userIdentity = null;

            if (Domain.Length > 1)
            {
                user = Domain[0];
                dnsDomain = Domain[1];
            }
            else
                dnsDomain = Environment.GetEnvironmentVariable("USERDNSDOMAIN");

            foreach (var item in dnsDomain.Split('.'))
                PathLDAP = PathLDAP + string.Format("DC={0},", item);

            PathLDAP = PathLDAP.Substring(0, PathLDAP.Length - 1);

            LdapAuthProvider LDAP = new LdapAuthProvider(PathLDAP);

            userIdentity = LDAP.SignIn(user, password, dnsDomain);

            return userIdentity;
        }

        //private static Identity SingInLocal(string user, string password)
        //{
        //    Identity userIdentity = null;
        //    string domainLocal = "";
        //    string[] Domain = user.Split('@');

        //    if (Domain.Length > 1)
        //        domainLocal = Domain[1];
        //    else
        //        domainLocal = Environment.GetEnvironmentVariable("USERDOMAIN");

        //    MASLocalAuthProvider LO = new MASLocalAuthProvider();
        //    userIdentity = LO.SignIn(user, password, domainLocal);

        //    return userIdentity;
        //}

        //private static Identity ValidateUser(string user)
        //{

        //    Identity identity = null;

        //    using (ExecuteQuery execute = new ExecuteQuery())
        //    {
        //        IEnumerable<object> userRawInfo = execute.Run("uspValidarUsuario", user);

        //        var userInfo = userRawInfo.FirstOrDefault();

        //        if (userInfo != null)
        //            identity = new Identity()
        //            {
        //                Fullname = ((dynamic)userInfo).Nombre,
        //                Email = ((dynamic)userInfo).Email,
        //                Id = (string)((dynamic)userInfo).Id.ToString()
        //            };
        //    }
        //    return identity;
        //}
    }

}