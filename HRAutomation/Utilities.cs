using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace HRAutomation
{
    public class Utilities
    {
        public static List<ADProp> GetActiveDirectoryData()
        {
            var adpropCollect = new List<ADProp>();

            try
            {
                using (var context = new PrincipalContext(ContextType.Domain, "ohg.local"))
                {
                    using (var searcher = new PrincipalSearcher(new UserPrincipal(context)))
                    {
                        foreach (var result in searcher.FindAll())
                        {
                            var de = result.GetUnderlyingObject() as DirectoryEntry;
                            if (de.Properties["mail"].Value == null || string.IsNullOrEmpty(de.Properties["mail"].Value.ToString())) continue;

                            var adprop = new ADProp();
                            adprop.cn = de.Properties["cn"].Value.ToString();
                            adprop.company = "" + de.Properties["company"].Value;
                            adprop.department = "" + de.Properties["department"].Value;
                            adprop.description = "" + de.Properties["description"].Value;
                            adprop.displayName = "" + de.Properties["displayName"].Value;
                            adprop.distinguishedName = "" + de.Properties["distinguishedName"].Value;
                            adprop.mail = "" + de.Properties["mail"].Value;
                            adprop.mailNickname = "" + de.Properties["mailNickname"].Value;
                            adprop.manager = "" + de.Properties["manager"].Value;
                            adprop.name = "" + de.Properties["name"].Value;
                            adprop.physicalDeliveryOfficeName = "" + de.Properties["physicalDeliveryOfficeName"].Value;
                            adprop.postalCode = "" + de.Properties["postalCode"].Value;
                            adprop.postOfficeBox = "" + de.Properties["postOfficeBox"].Value;
                            adprop.primaryGroupID = "" + de.Properties["primaryGroupID"].Value;
                            adprop.sAMAccountName = "" + de.Properties["sAMAccountName"].Value;
                            adprop.sAMAccountType = "" + de.Properties["sAMAccountType"].Value;
                            adprop.sn = "" + de.Properties["sn"].Value;
                            adprop.st = "" + de.Properties["st"].Value;
                            adprop.telephoneNumber = "" + de.Properties["telephoneNumber"].Value;
                            adprop.thumbnailPhoto = "" + de.Properties["thumbnailPhoto"].Value;
                            adprop.title = "" + de.Properties["title"].Value;
                            adprop.userAccountControl = "" + de.Properties["userAccountControl"].Value;
                            adprop.userPrincipalName = "" + de.Properties["userPrincipalName"].Value;
                            adprop.userWorkstations = "" + de.Properties["userWorkstations"].Value;
                            adprop.whenChanged = "" + de.Properties["whenChanged"].Value;
                            adprop.whenCreated = "" + de.Properties["whenCreated"].Value;

                            adpropCollect.Add(adprop);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return adpropCollect;
        }

        public static void SaveToDb(List<ADProp> adpropCollect)
        {
            var connStr = "Data Source=OHGDEVAPP01;User ID=DATA_LOAD;Password=DATA_LOAD;Initial Catalog=HRSystems;";
            var query = "INSERT INTO ActiveDirectory values (@cn,@company,@department,@description,@displayName,@distinguishedName,@mail,@mailNickname,@manager,@name,@physicalDeliveryOfficeName,@postalCode,@postOfficeBox,@primaryGroupID,@sAMAccountName,@sAMAccountType,@sn,@st,@telephoneNumber,@thumbnailPhoto,@title,@userAccountControl,@userPrincipalName,@userWorkstations,@whenChanged,@whenCreated)";

            foreach (ADProp item in adpropCollect)
            {
                using (var connection = new SqlConnection(connStr))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@cn", SqlDbType.NVarChar).Value = item.cn;
                    command.Parameters.Add("@company", SqlDbType.NVarChar).Value = item.company;
                    command.Parameters.Add("@department", SqlDbType.NVarChar).Value = item.department;
                    command.Parameters.Add("@description", SqlDbType.NVarChar).Value = item.description;
                    command.Parameters.Add("@displayName", SqlDbType.NVarChar).Value = item.displayName;
                    command.Parameters.Add("@distinguishedName", SqlDbType.NVarChar).Value = item.distinguishedName;
                    command.Parameters.Add("@mail", SqlDbType.NVarChar).Value = item.mail;
                    command.Parameters.Add("@mailNickname", SqlDbType.NVarChar).Value = item.mailNickname;
                    command.Parameters.Add("@manager", SqlDbType.NVarChar).Value = item.manager;
                    command.Parameters.Add("@name", SqlDbType.NVarChar).Value = item.name;
                    command.Parameters.Add("@physicalDeliveryOfficeName", SqlDbType.NVarChar).Value = item.physicalDeliveryOfficeName;
                    command.Parameters.Add("@postalCode", SqlDbType.NVarChar).Value = item.postalCode;
                    command.Parameters.Add("@postOfficeBox", SqlDbType.NVarChar).Value = item.postOfficeBox;
                    command.Parameters.Add("@primaryGroupID", SqlDbType.NVarChar).Value = item.primaryGroupID;
                    command.Parameters.Add("@sAMAccountName", SqlDbType.NVarChar).Value = item.sAMAccountName;
                    command.Parameters.Add("@sAMAccountType", SqlDbType.NVarChar).Value = item.sAMAccountType;
                    command.Parameters.Add("@sn", SqlDbType.NVarChar).Value = item.sn;
                    command.Parameters.Add("@st", SqlDbType.NVarChar).Value = item.st;
                    command.Parameters.Add("@telephoneNumber", SqlDbType.NVarChar).Value = item.telephoneNumber;
                    command.Parameters.Add("@thumbnailPhoto", SqlDbType.NVarChar).Value = item.thumbnailPhoto;
                    command.Parameters.Add("@title", SqlDbType.NVarChar).Value = item.title;
                    command.Parameters.Add("@userAccountControl", SqlDbType.NVarChar).Value = item.userAccountControl;
                    command.Parameters.Add("@userPrincipalName", SqlDbType.NVarChar).Value = item.userPrincipalName;
                    command.Parameters.Add("@userWorkstations", SqlDbType.NVarChar).Value = item.userWorkstations;
                    command.Parameters.Add("@whenChanged", SqlDbType.NVarChar).Value = item.whenChanged;
                    command.Parameters.Add("@whenCreated", SqlDbType.NVarChar).Value = item.whenCreated;

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
    }

    public class ADProp
    {
        public string cn { get; set; }
        public string company { get; set; }
        public string department { get; set; }
        public string description { get; set; }
        public string displayName { get; set; }
        public string distinguishedName { get; set; }
        public string mail { get; set; }
        public string mailNickname { get; set; }
        public string manager { get; set; }
        public string name { get; set; }
        public string physicalDeliveryOfficeName { get; set; }
        public string postalCode { get; set; }
        public string postOfficeBox { get; set; }
        public string primaryGroupID { get; set; }
        public string sAMAccountName { get; set; }
        public string sAMAccountType { get; set; }
        public string sn { get; set; }
        public string st { get; set; }
        public string telephoneNumber { get; set; }
        public string thumbnailPhoto { get; set; }
        public string title { get; set; }
        public string userAccountControl { get; set; }
        public string userPrincipalName { get; set; }
        public string userWorkstations { get; set; }
        public string whenChanged { get; set; }
        public string whenCreated { get; set; }
    }
}
