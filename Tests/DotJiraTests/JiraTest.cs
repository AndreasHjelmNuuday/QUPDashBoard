using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DotJira
{
    public class JiraTest
    {
        DotJira.Jira jira;
        [SetUp]
        public void Setup()
        {
            string url = Credentials.JIRA_URL;
            string user = Credentials.JIRA_USER;
            string pw = Credentials.JIRA_PASSWORD;
            jira = new DotJira.Jira(url, user, pw);
        }

        [Test]
        public void ConnectToJira()
        {
            string expectedStatus;            
            checkConnection(out expectedStatus);                        
        }

        private static void checkConnection(out string expectedStatus)
        {
            string statusCode = "";
            List<Projects> projects; projects = Jira.GetProjects(out statusCode);
            expectedStatus = "OK";
            Assert.AreEqual(expectedStatus, statusCode);
            Assert.IsNotNull(projects);
        }       
 
    }
}