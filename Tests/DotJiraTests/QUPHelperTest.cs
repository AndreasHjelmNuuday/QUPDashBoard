
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotJira
{
    class QUPHelperTest
    {
        QUPHelper qup;
        [SetUp]
        public void Setup()
        {
            qup = new QUPHelper();
        }

        [Test]
        public void ConnectToJira()
        {
            string expectedStatus;
            checkConnection(out expectedStatus);
        }

        private void checkConnection(out string expectedStatus)
        {
            string statusCode = "";
            qup.Connect(out statusCode);
            expectedStatus = "OK";
            Assert.AreEqual(expectedStatus, statusCode);
        }

        [Test]
        public void getAllQUPIssuesInMUSICProject()
        {
            string project = "Music";
            string quarter = "2021 Q2";
            List<Issue> issues = qup.GetAllQUPIssuesInProject(project, quarter);
            Assert.NotNull(issues);
        }

        [Test]
        public void TeamFieldIsSet()
        {
            string project = "Music";
            string quarter = "2021 Q2";
            List<Issue> issues = qup.GetAllQUPIssuesInProject(project, quarter);
            Issue issueWithTeamName = issues.Find(i => !i.Fields.Team.Equals(null));
            Assert.NotNull(issueWithTeamName);
        }
        [Test]
        public void RAGFieldIsSet()
        {
            string project = "Music";
            string quarter = "2021 Q2";
            List<Issue> issues = qup.GetAllQUPIssuesInProject(project, quarter);
            Issue issueWithRagSet = issues.Find(i => i.Fields.RAG != null && i.Fields.RAG.Value.Contains("Green"));
            Assert.NotNull(issueWithRagSet);
        }

        [Test]
        public void IssueTypeIsSet()
        {
            string project = "Music";
            string quarter = "2021 Q2";
            List<Issue> issues = qup.GetAllQUPIssuesInProject(project, quarter);
            Issue issueWithTypeSet = issues.Find(i => i.Fields != null && i.Fields.Type != null && i.Fields.Type.Name.Contains("Tribe Objective"));
            Assert.NotNull(issueWithTypeSet);
        }
        [Test]
        public void IssueIconUrlIsSet()
        {
            string project = "Music";
            string quarter = "2021 Q2";
            List<Issue> issues = qup.GetAllQUPIssuesInProject(project, quarter);
            Issue issueWithIconUrlSet = issues.Find(i => i.Fields != null && i.Fields.Type != null && i.Fields.Type.IconUrl != null);
            Assert.NotNull(issueWithIconUrlSet);
        }

        [Test]
        public void ParentIsSet()
        {
            string project = "Music";
            string quarter = "2021 Q2";
            List<Issue> issues = qup.GetAllQUPIssuesInProject(project, quarter);
            Issue issueWithParentSet = issues.Find(i => i.Fields != null && i.Fields.Parent != null);
            Assert.NotNull(issueWithParentSet);
        }

        [Test]
        public void TribeObjectivesAreSortedWithSquadObejctivesAsChildren()
        {
            string project = "Music";
            string quarter = "2021 Q2";
            List<Issue> issues = qup.getAllQupIssuesSorted(project, quarter);

            Assert.Greater(issues.Count, 0);
            foreach (Issue issue in issues)
            {
                Assert.AreEqual(issue.Fields.Type.Name, "Tribe Objective");
                Assert.NotNull(issue.Children);
            }
        }
    }
}
