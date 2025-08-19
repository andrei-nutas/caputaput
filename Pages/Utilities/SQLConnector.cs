 using System;
using System.Collections.Generic;
using System.Xml;
using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlTypes;

namespace TAF_iSAMS.Pages.Utilities
{
    public class SQLConnector
    {
        string connectionString;

        // Public dictionary to store the modules
        public Dictionary<int, string> Modules { get; private set; } = new Dictionary<int, string>();
        public SQLConnector()
        {
            string configPath = GetConnectionStringFilePath();
            connectionString = LoadConnectionStringFromFile(configPath);
        }

        /// <summary>
        /// Checks boths paths to see if a connectionstrings.config file exists.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        private string GetConnectionStringFilePath()
        {
            var possiblePaths = new[]
            {
                 @"C:\Dev\iSAMS\iSAMS.Website\connectionstrings.config",
                 @"C:\iSAMS\iSAMS.Multibrowser\iSAMS.Legacy\connectionstrings.config"
            };

            return possiblePaths.FirstOrDefault(File.Exists)
            ?? throw new FileNotFoundException("No valid connection string config file found in expected locations.");
        }

        /// <summary>
        /// This method reads through the connectionstrings.config file and extracts the connection string line then stores that in a local variable to be used by an SQL connection.
        /// </summary>
        /// <param name="filePath">The path to the connectionstrings.config file obtained by GetConnectionStringFilePath()</param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="Exception"></exception>
        private string LoadConnectionStringFromFile(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Connection string config file not found.", filePath);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            XmlNode node = xmlDoc.SelectSingleNode("/connectionStrings/add");
            if (node == null)
                throw new Exception("No <add> element found in connection string file.");

            var connStr = node.Attributes["connectionString"]?.Value;
            if (string.IsNullOrEmpty(connStr))
                throw new Exception("Connection string is empty or missing.");

            TestContext.WriteLine($"Connection String Found in: {filePath}");
            return connStr;
        }



        /// <summary>
        /// This function will get the module ID and name from the database and store them in a dictionary.
        /// </summary>
        public void GetModuleInfo()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Connection to the database was successful!");

                    string query = "SELECT TbliSAMSManagerModulesID, txtModuleName FROM TbliSAMSManagerModules ORDER BY TbliSAMSManagerModulesID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32(0);
                                string moduleName = reader.GetString(1);
                                Modules[id] = moduleName;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    TestContext.WriteLine($"An error occurred: {ex.Message}");                    
                }
            }
        }

        /// <summary>
        /// This function will check if a SQL user exists with the name "TAFUser", if it does not one will be created.
        /// </summary>
        public void CreateTestUser()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    TestContext.WriteLine("Connection to the database was successful!");

                    // Check if the user exists
                    string checkUserQuery = "SELECT COUNT(*) FROM TbliSAMSManagerUsers WHERE txtUsername = 'TAFUser'";
                    using (SqlCommand checkUserCommand = new SqlCommand(checkUserQuery, connection))
                    {
                        int userCount = (int)checkUserCommand.ExecuteScalar();
                        if (userCount == 0)
                        {
                            // User does not exist, insert the new user
                            string insertUserQuery = @"
                        INSERT INTO TbliSAMSManagerUsers (txtUsername, txtUserCode, txtTitle, txtFirstname, txtSurname, txtFullName, txtNickname, txtEmailAddress,
                                                          txtWebsite, intGroupID, txtSecurityID, txtPassword, txtQuestion, txtAnswer, txtNotes, intFailedLogins,
                                                          txtLastFailedLoginDateTime, intFailedLoginsDisabled, txtLastDisabledLoginDate, txtStatus, txtCreateDateTime,
                                                          txtCreateBy, txtUpdateDateTime, txtUpdateBy, IntReadTerms, intIMActive, txtLoginDateTime, txtPicture, intTimeZone,
                                                          txtPrimaryRole, txtDirectLine, txtExt, txtAddress, txtPostcode, txtHomeTel, txtMobileTel, txtHomeEmail, txtHomepage,
                                                          intHidePersonal, txtotherinformation, intHideOtherInfo, intAllowNTSpace, intAllowShared, txtFolderPath, intQuota,
                                                          intNoExceed, intAlertExceed, txtQuotaLimit, intAllowFTP, intAllowShare, txtBannedFiles, txtIPAddress, txtCurrentModule,
                                                          intSPLogin, intiSAMSLogin, intBXWLogin, intBXILogin, intAPLogin, intWebShopLogin, intPPLogin, intCPMLogin, txtUserType,
                                                          intIdentityProviderId, intReset, txtADDomain, txtADShortDomain, blnApiEnabled, intApiAccessArea, intPasswordVersion,
                                                          dtePasswordChanged, intPersonID, guidPersonUniqueID, intQuestionVersion, intAnswerVersion, txtPreName,
                                                          blnTwoFactorAuthenticationConfigured, blnTwoFactorAuthenticationEnabled, txtTotpKey)
                        VALUES ('TAFUser', 'TAFUser', 'Mr', 'iSAMS', 'Administrator', 'Mr iSAMS Administrator', NULL, 'Administrator@isams.co.uk', NULL, 1, 'A',
                                '$2a$10$0UpJpz8jXZkw4xtoGDR4W.Yg9VB8PD.rC7Nc26faHjq7TVswLDmYS', '8753E16EC744C0867037473EAA3C7113EB43', '96CFE7D944F580C80696CA3918FE1E', NULL, 0, NULL, 0, NULL, 0, GETDATE(), 'Pete',
                                NULL, NULL, 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, 0, 0, 0, NULL, 0, 0, 0, 0, 0, 0, 0, '0.0.0.0',
                                'iSAMS_ADMISSIONSMANAGER', 1, 1, 1, 1, NULL, 0, 1, 0, 'SystemUser', 0, 0, NULL, NULL, 0, 7, 3, GETDATE(), NULL, NULL, 1, 1, NULL, 1, 0, NULL);
                        UPDATE TbliSAMSManagerUsers SET blnTwoFactorAuthenticationEnabled = 0 WHERE txtUsername = 'TAFUser'";

                            using (SqlCommand insertUserCommand = new SqlCommand(insertUserQuery, connection))
                            {
                                insertUserCommand.ExecuteNonQuery();
                                TestContext.WriteLine("User created successfully!");
                            }
                        }
                        else
                        {
                            TestContext.WriteLine("User already exists.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    TestContext.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }
        /// <summary>
        /// This is a generic SQL Executor where you can pass in a simple query to run.
        /// </summary>
        /// <param name="query">The SQL query you would like to run.</param>
        public void ExecuteQuery(string query)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    TestContext.WriteLine("Connection to the database was successful!");

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        int rowsAffected = command.ExecuteNonQuery();
                        TestContext.WriteLine($"Query executed successfully. Rows affected: {rowsAffected}");
                    }
                }
                catch (Exception ex)
                {
                    TestContext.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }


        /// <summary>
        /// This function will set 'Simultaneous Logins' to 'Disabled'.  This will prevent an additional page from appearing during the login process (if the user is already logged in) whereby the user has to choose between 'New Session' or 'Clear All'. 
        /// When this is set to 'Disabled', and the user is already logged in, the new login will just automatically override the existing session with no additional pop ups in the login process
        /// </summary>
        public void DisableSimultaneousLogins()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Connection to the database was successful!");

                    string query = "UPDATE TbliSAMSManagerSecurityOptions SET Simultaneous = 0";


                    using (SqlCommand insertUserCommand = new SqlCommand(query, connection))
                    {
                        insertUserCommand.ExecuteNonQuery();
                        TestContext.WriteLine("'Simultaneous Logins' set to disabled successfully!");
                    }
                }

                catch (Exception ex)
                {
                    TestContext.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// This function will set 'Simultaneous Logins' to 'Enabled'.  This will present an additional page during the login process (if the user is already logged in) whereby the user has to choose between 'New Session' or 'Clear All'. 
        /// When this is set to 'Enabled', and the user is already logged in, the user will have no choice but to choose one of the options stated above on the additional screen that will appear, rather than the system just automatically overriding the existing session
        /// </summary>
        public void EnableSimultaneousLogins()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Connection to the database was successful!");

                    string query = "UPDATE TbliSAMSManagerSecurityOptions SET Simultaneous = 1";


                    using (SqlCommand insertUserCommand = new SqlCommand(query, connection))
                    {
                        insertUserCommand.ExecuteNonQuery();
                        TestContext.WriteLine("'Simultaneous Logins' set to enabled successfully!");
                    }
                }

                catch (Exception ex)
                {
                    TestContext.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }
    }
}
