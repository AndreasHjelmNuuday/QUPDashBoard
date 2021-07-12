
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
            string project = "MUSIC";
            string quarter = "2021 Q2";
            List<Issue> issues = qup.GetAllQUPIssuesInProject(project, quarter);
            Assert.NotNull(issues);
        }

        [Test]
        public void TeamFieldIsSet()
        {
            string project = "MUSIC";
            string quarter = "2021 Q2";
            List<Issue> issues = qup.GetAllQUPIssuesInProject(project, quarter);
            Issue issueWithTeamName = issues.Find(i => !i.Fields.Team.Equals(null));
            Assert.NotNull(issueWithTeamName);
        }
        [Test]
        public void RAGFieldIsSet()
        {
            string project = "MUSIC";
            string quarter = "2021 Q2";
            List<Issue> issues = qup.GetAllQUPIssuesInProject(project, quarter);
            Issue issueWithRagSet = issues.Find(i => i.Fields.RAG != null && i.Fields.RAG.Value.Contains("Green"));
            Assert.NotNull(issueWithRagSet);
        }

        [Test]
        public void IssueTypeIsSet()
        {
            string project = "MUSIC";
            string quarter = "2021 Q2";
            List<Issue> issues = qup.GetAllQUPIssuesInProject(project, quarter);
            Issue issueWithTypeSet = issues.Find(i => i.Fields != null && i.Fields.Type != null && i.Fields.Type.Name.Contains("Tribe Objective"));
            Assert.NotNull(issueWithTypeSet);
        }
        [Test]
        public void IssueIconUrlIsSet()
        {
            string project = "MUSIC";
            string quarter = "2021 Q2";
            List<Issue> issues = qup.GetAllQUPIssuesInProject(project, quarter);
            Issue issueWithIconUrlSet = issues.Find(i => i.Fields != null && i.Fields.Type != null && i.Fields.Type.IconUrl != null);
            Assert.NotNull(issueWithIconUrlSet);
        }

        [Test]
        public void ParentIsSet()
        {
            string project = "MUSIC";
            string quarter = "2021 Q2";
            List<Issue> issues = qup.GetAllQUPIssuesInProject(project, quarter);
            Issue issueWithParentSet = issues.Find(i => i.Fields != null && i.Fields.Parent != null);
            Assert.NotNull(issueWithParentSet);
        }

        [Test]
        public void LinkedIssuesIsSet()
        {
            string project = "MUSIC";
            string quarter = "2021 Q2";
            List<Issue> issues = qup.GetAllQUPIssuesInProject(project, quarter);
            Issue issue = issues.Find(i => i.Fields != null && i.Fields.linkedIssues.Count != 0);
            Assert.NotNull(issue);
        }
        [Test]
        public void TribeObjectivesAreSortedWithSquadObejctivesAsChildren()
        {
            string project = "MUSIC";
            string quarter = "2021 Q2";
            List<Issue> issues = qup.getAllQupIssuesSorted(project, quarter);

            Assert.Greater(issues.Count, 0);
            foreach (Issue issue in issues)
            {
                Assert.AreEqual("Tribe Objective", issue.Fields.Type.Name);
                Assert.NotNull(issue.Children);
            }
        }

        [Test]
        public void AllEpicsThatAreChildrenToASquadObjectiveAreFound()
        {
            string project = "MUSIC";
            string quarter = "2021 Q3";
            List<Issue> issues = qup.getAllQupIssuesSorted(project, quarter);

            Assert.Greater(issues.Count, 0);
            foreach (Issue tribeObjective in issues)
            {
                Assert.AreEqual(tribeObjective.Fields.Type.Name, "Tribe Objective");
                if (tribeObjective.Children != null)
                {
                    foreach (Issue squadObjective in tribeObjective.Children)
                    {
                        Assert.AreEqual(squadObjective.Fields.Type.Name, "Squad Objective");
                        Assert.NotNull(squadObjective.Children);                        
                            foreach (Issue epic in squadObjective.Children)
                            {
                                Assert.AreEqual("Epic", epic.Fields.Type.Name);
                            }                        
                    }
                }
            }
        }

        [Test]
        public void AllEpicsThatAreLinkedByImplemented()
        {
            string project = "MUSIC";
            string quarter = "2021 Q3";
            const string squadObjectiveKey = "MUSIC-9585";
            const string epicKey = "MUSIC-9492";
            
        List<Issue> issues = qup.getAllQupIssuesSorted(project, quarter);

            Assert.Greater(issues.Count, 0);
            foreach (Issue tribeObjective in issues)
            {
                Assert.AreEqual(tribeObjective.Fields.Type.Name, "Tribe Objective");
                if (tribeObjective.Children != null)
                {
                    foreach (Issue squadObjective in tribeObjective.Children)
                    {
                        Assert.AreEqual(squadObjective.Fields.Type.Name, "Squad Objective");
                        Assert.NotNull(squadObjective.IsImplementedBy);                        
                        foreach (Issue epic in squadObjective.IsImplementedBy)
                        {                            
                            if (epic.Key.Equals(epicKey))
                            {
                                Assert.AreEqual(squadObjectiveKey, squadObjective.Key); 
                                Assert.NotNull(squadObjective.IsImplementedBy);
                                Assert.NotNull(squadObjective.IsImplementedBy.Find(i => i.Key.Equals(epicKey))); //MUSIC-9585 is implemented by MUSIC-9492
                            }
                        }
                    }
                }
            }
        }
    }
}
