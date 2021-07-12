using DotJira;
using NUnit.Framework;
using System.Collections.Generic;

namespace PlectoUploader
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void WriteEmptyJiraTicketToPlecto()
        {
            PlectoUploader uploader = new PlectoUploader();
            string result = uploader.WriteJiraTicketToPlecto();

            Assert.NotNull(result);
        }

        [Test]
        public void WriteCustomJiraTicketToPlecto()
        {
            string issueID = "MUSIC - 7788";
            string issueType = "Epic";
            string summary = "Storage usage investigation";
            string status = "In Progress";
            string rag = "GREEN";

            PlectoUploader uploader = new PlectoUploader();
            string result = uploader.WriteJiraTicketToPlecto(issueID, issueType, summary, status, rag);

            Assert.NotNull(result);
        }

        [Test]
        public void WriteQUPJiraTicketsToPlecto()
        {
            DotJira.QUPHelper qup = new DotJira.QUPHelper();
            List<Issue> qupIssues = qup.GetAllQUPIssuesInProject("Music", "2021 Q2");

            PlectoUploader uploader = new PlectoUploader();
            foreach (Issue issue in qupIssues)
            {                
                string result = uploader.WriteJiraTicketToPlecto(issue);
                Assert.NotNull(result);
            }
        }
    }
}