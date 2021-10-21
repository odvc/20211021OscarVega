using API.Security.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;
using System.DirectoryServices;

namespace API.Security.Providers
{
    class LdapAuthProvider
    {

        private string _path;
        private string _filterAttribute;

        public LdapAuthProvider(string path)
        { _path = path; }

        public Identity SignIn(string userName, string password, string domain)
        {
            string domainAndUsername = domain + @"\" + userName;
            DirectoryEntry entry = new DirectoryEntry(_path, domainAndUsername, password);
            //DirectoryEntry entry = new DirectoryEntry("cccserver01.enterprise.local");
            //entry.Path = _path;
            //entry.Username = userName;
            //entry.Password = password;
            Identity identity = null;

            try
            {
                //Bind to the native AdsObject to force authentication.
                object obj = entry.NativeObject;

                DirectorySearcher search = new DirectorySearcher(entry)
                {
                    Filter = "(SAMAccountName=" + userName + ")"
                };
                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();

                if (result == null)
                { return identity; }

                identity = new Identity() { Domain = domain, Username = userName, Email = userName, Fullname = userName, Id = domainAndUsername };

                //Update the new path to the user in the directory.
                _path = result.Path;
                _filterAttribute = (string)result.Properties["cn"][0];
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return identity;
        }

        public string GetGroups()
        {
            DirectorySearcher search = new DirectorySearcher(_path);
            search.Filter = "(cn=" + _filterAttribute + ")";
            search.PropertiesToLoad.Add("memberOf");
            StringBuilder groupNames = new StringBuilder();

            try
            {
                SearchResult result = search.FindOne();
                int propertyCount = result.Properties["memberOf"].Count;
                string dn;
                int equalsIndex, commaIndex;

                for (int propertyCounter = 0; propertyCounter < propertyCount; propertyCounter++)
                {
                    dn = (string)result.Properties["memberOf"][propertyCounter];
                    equalsIndex = dn.IndexOf("=", 1);
                    commaIndex = dn.IndexOf(",", 1);
                    if (-1 == equalsIndex)
                    {
                        return null;
                    }
                    groupNames.Append(dn.Substring((equalsIndex + 1), (commaIndex - equalsIndex) - 1));
                    groupNames.Append("|");
                }
            }
            catch (Exception ex)
            { throw new Exception("Error obtaining group names. " + ex.Message); }
            return groupNames.ToString();
        }
    }

}