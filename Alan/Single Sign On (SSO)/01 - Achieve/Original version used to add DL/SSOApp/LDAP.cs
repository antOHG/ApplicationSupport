using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.DirectoryServices;

namespace SSOApp
{
    public class LDAP
    {
        public static string GetEmail(string username)
        {
            // create and return new LDAP connection with desired settings

            DirectoryEntry ldapConnection = new DirectoryEntry("LDAP://OHG.local");
            DirectorySearcher search = new DirectorySearcher(ldapConnection);
            search.Filter = "(&(samaccountname=" + username + "))";

            SearchResult result = search.FindOne();

            if (result != null)
            {
                ResultPropertyValueCollection x = result.Properties["mail"];

                if (x.Count > 0)
                {
                    return x[0].ToString();
                }
                else
                {
                    return null;
                }

            }
            else
            {
                return null;

                //test
            }
        }
    }
}