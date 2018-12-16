using System;
using System.Windows.Forms;
using System.DirectoryServices;
using System.Collections;

namespace Sunlord.CommonFrame
{
    /// <summary>
    /// This class performs user authentication against Active Directory and
    /// Novell Edirectory.
    /// </summary>
    public class Authenticate
    {
        /// <summary>
        /// string specifying user name
        /// </summary>
        private string strUser;

        private string strDisplayName;

        /// <summary>
        /// string specifying user password
        /// </summary>
        private string strPass;

        /// <summary>
        /// string specifying user domain
        /// </summary>
        private string strDomain;

        /// <summary>
        /// AuthenticationTypes specifying the security 
        /// protocol to use, lvi.e. Secure, SSL
        /// </summary>
        private AuthenticationTypes atAuthentType;

        /// <summary>
        /// default constructor
        /// </summary>
        public Authenticate()
        {
            strDisplayName = "";
        }

        /// <summary>
        /// function that sets the domain name
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns>It returns true, if user passed 
        ///          something; otherwise, false</returns>
        public bool SetDomain(string strValue)
        {
            if (strValue.Length <= 0)
                return false;

            this.strDomain = "LDAP://" + strValue;
            return true;
        }


        public string GetDisplayName()
        {
            if(strDisplayName == "")
                strDisplayName = GetAttribute(this.strDomain, this.strUser, "displayName");
            return strDisplayName;
        }

        /// <summary>
        /// function that sets user name
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns>It returns true, if user passed 
        ///          something; otherwise, false</returns>
        public bool SetUser(string strValue)
        {
            if (strValue.Length <= 0)
                return false;

            this.strUser = strValue;
            return true;
        }

        /// <summary>
        /// function that sets user password
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns>It returns true, if user passed 
        ///          something; otherwise, false</returns>
        public bool SetPass(string strValue)
        {
            if (strValue.Length <= 0)
                return false;

            this.strPass = strValue;
            return true;
        }

        /// <summary>
        /// function that sets user authentication type
        /// </summary>
        /// <param name="bValue"></param>
        public void SetAuthenticationType(bool bValue)
        {
            // set ssl to true if true is found
            if (bValue)
                atAuthentType = AuthenticationTypes.SecureSocketsLayer;
            // otherwise set it to secure  
            else
                atAuthentType = AuthenticationTypes.Secure;
        }

        /// working code

        public static string GetAttribute(string ldappath, string sAMAccountName, string attribute)
        {
            string OUT = string.Empty;

            try
            {
                DirectoryEntry de = new DirectoryEntry(ldappath,
                                                                 "luoninglin",
                                                                 "davidluo123",
                                                                 AuthenticationTypes.Secure);
                // if user is verified then it will welcome then  
                try
                {
                    string do_test = de.Name;//绝对不能去掉该行!!!
                }
                catch (Exception exp)
                {
                    return "LDAP验证失败:" + exp.Message;
                }

                DirectorySearcher ds = new DirectorySearcher(de);
                ds.Filter = "(&(objectClass=user)(objectCategory=person)(sAMAccountName=" + sAMAccountName + "))";

                SearchResultCollection results = ds.FindAll();

                foreach (SearchResult result in results)
                {
                    OUT = GetProperty(result, attribute);
                }
            }
            catch 
            {
                // System.Diagnostics.Debug.WriteLine(t.Message);
            }

            return (OUT != null) ? OUT : string.Empty;
        }

        public string _GetAttribute(string sAMAccountName, string attribute)
        {
            string OUT = string.Empty;

            try
            {
                DirectoryEntry de = _de;
                DirectorySearcher ds = new DirectorySearcher(de);
                ds.Filter = "(&(objectClass=user)(objectCategory=person)(sAMAccountName=" + sAMAccountName + "))";

                SearchResultCollection results = ds.FindAll();

                foreach (SearchResult result in results)
                {
                    OUT = GetProperty(result, attribute);
                }
            }
            catch 
            {
                // System.Diagnostics.Debug.WriteLine(t.Message);
            }

            return (OUT != null) ? OUT : string.Empty;
        }

        public static string GetProperty(SearchResult searchResult, string PropertyName)
        {
            if (searchResult.Properties.Contains(PropertyName))
            {
                return searchResult.Properties[PropertyName][0].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        private DirectoryEntry _de;
        /// <summary>
        /// function that performs login task
        /// and welcomes user if they are verified
        /// </summary>
        public String Login()
        {
            // now create the directory entry to establish connection
            _de = new DirectoryEntry(this.strDomain,
                                                                 this.strUser,
                                                                 this.strPass,
                                                                 this.atAuthentType);
                // if user is verified then it will welcome then  
                try
                {
                    string do_test = _de.Name;//绝对不能去掉该行!!!
                }
                catch (Exception exp)
                {
                    return "LDAP验证失败:" + exp.Message;
                }
                strDisplayName = _GetAttribute(this.strUser, "displayName");
                return "";

        }
    }
}