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
            string url = Constants.JIRA_URL;
            string user = Constants.JIRA_USER;
            string pw = Constants.JIRA_PASSWORD;
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