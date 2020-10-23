using System.Configuration;
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

                //CHECK FOR BAYCROFT EMAIL ADDRESS
                if (ConfigurationManager.AppSettings.Get("BaycroftCheck") == "yes")
                {
                    ResultPropertyValueCollection emailProp = result.Properties["proxyaddresses"];
                    for (int item = 0; item < emailProp.Count; item++)
                    {
                        if (!string.IsNullOrEmpty(emailProp[item].ToString()) && emailProp[item].ToString().ToLower().Contains("smtp:") && emailProp[item].ToString().ToLower().Contains("baycroft"))
                        {
                            return emailProp[item].ToString().ToLower().Replace("smtp:", "");
                        }
                    }
                }

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
            }
        }
    }
}