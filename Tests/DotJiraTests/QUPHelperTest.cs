
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
        public void GetAllQUPIssuesInMUSICProject()
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
            Issue issueWithTeamName = issues.Find(i => i.Fields.Team != null);
            Assert.NotNull(issueWithTeamName);
        }
        [Test]
        public void RAGFieldIsSet()
        {
            string project = "MUSIC";
            string quarter = "2021 Q2";
            List<Issue> issues = qup.GetAllQUPIssuesInProject(project, quarter);
            Issue issueWithRagSet = issues.Find(i => i.Fields.RAG != null);
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
        public void LinkedIssuesIsSet()
        {
            string project = "MUSIC";
            string quarter = "2021 Q2";
            List<Issue> issues = qup.GetAllQUPIssuesInProject(project, quarter);
            Issue issue = issues.Find(i => i.Fields != null && i.Fields.linkedIssues.Count != 0);
            Assert.NotNull(issue);
        }

        [Test]
        public void KeyResultIsSet()
        {
            string project = "MUSIC";
            string quarter = "2021 Q3";
            List<Issue> issues = qup.GetAllQUPIssuesInProject(project, quarter);
            Issue issue = issues.Find(i => i.Fields != null && i.Fields.KeyResult != null);
            Assert.NotNull(issue);
        }

        [Test]
        public void KeyResultsAreSplitBySeparator()
        {
            string project = "MUSIC";
            string quarter = "2021 Q3";
            List<Issue> issues = qup.GetAllQUPIssuesInProject(project, quarter);
            Issue issue = issues.Find(i => i.Key.Equals("MUSIC-9552"));
            Assert.NotNull(issue);
            string[] keyResults = issue.Fields.SplitKeyResults();
            Assert.AreEqual(2, keyResults.Length);

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
        [Test]
        public void SquadObjectiveHasBothChildAndLinkedEpicsDoneChildrenIsIncreased()
        {
            string project = "MUSIC";
            string quarter = "2021 Q1";

            List<Issue> issues = qup.getAllQupIssuesSorted(project, quarter);

            Assert.Greater(issues.Count, 0);
            foreach (Issue tribeObjective in issues)
            {
                Assert.AreEqual(tribeObjective.Fields.Type.Name, "Tribe Objective");
                if (tribeObjective.Children != null)
                {
                    foreach (Issue squadObjective in tribeObjective.Children)
                    {

                        if (squadObjective.Key.Equals("MUSIC-9632"))
                        {
                            Assert.AreEqual(2, squadObjective.DoneChildren);
                        }
                    }
                }
            }
        }

        [Test]
        public void SpecificIssueCanBeFound()
        {
             
            string issueKey = "MUSIC-10617"; //Squad Objective
            List<Issue> issues = qup.GetSpecificIssue(issueKey);

            Assert.NotNull(issues.First());
            Assert.AreEqual("Squad Objective", issues.First().Fields.Type.Name);
            Issue specificIssue = issues.Find(i => i.Key.Equals(issueKey));
            Assert.NotNull(specificIssue);

        }

        [Test]
        public void SpecificIssueAndLinkedEpicsAndSubIssuesCanBeFound()
        {

            string issueKey = "MUSIC-10544"; //Squad Objective
            List<Issue> issues = qup.GetSpecificIssue(issueKey);

            Assert.NotNull(issues.First());
            Assert.AreEqual("Squad Objective", issues.First().Fields.Type.Name);
            Issue specificIssue = issues.Find(i => i.Key.Equals(issueKey));            
            Assert.NotNull(specificIssue);
            Assert.AreEqual(2, specificIssue.Children.Count);
            string epicKey = "MUSIC-9116";
            Issue epic = specificIssue.Children.Find(i => i.Key.Equals(epicKey));
            Assert.AreEqual(8, epic.Children.Count);
        }

        [Test]
        public void SpecificEpicSubIssuesCanBeFound()
        {
            string issueKey = "MUSIC-10544"; //Squad Objective
            List<Issue> issues = qup.GetSpecificIssue(issueKey);
            Assert.NotNull(issues.First());
            Assert.AreEqual("Squad Objective", issues.First().Fields.Type.Name);
            Issue specificIssue = issues.Find(i => i.Key.Equals(issueKey));
            Assert.NotNull(specificIssue);
            Assert.AreEqual(2, specificIssue.Children.Count);
            
            string epicKey = "MUSIC-9116";            
            Issue epic = specificIssue.Children.Find(i => i.Key.Equals(epicKey));
            Assert.NotNull(epic);
            Assert.AreEqual(8, epic.Children.Count);
        }

        [Test]
        public void SpecificIssueThatDontBelongToQUPCanBeFound()
        {
            string issueKey = "MUSIC-10634";
            string epicKey = "MUSIC-9480"; //epic
            List<Issue> issues = qup.GetSpecificIssue(epicKey);
            
            Issue squadObjective = issues.Find(i => i.Key.Equals(issueKey));
            Assert.NotNull(squadObjective);
            Issue specificIssue = squadObjective.Children.Find(i => i.Key.Equals(epicKey));
            Assert.NotNull(specificIssue);

        }

        [Test]
        public void AllTeamIssuesFromAQuarterCanBeFound()
        {
            string project = "MUSIC";
            string quarter = "2021 Q4";
            string team = "454";
            List<Issue> issues = qup.GetTeamIssues(project, quarter, team);

            Assert.AreEqual(3, issues.Count);

            Assert.AreEqual("Squad Objective", issues.First().Fields.Type.Name);
            Assert.AreEqual("Squad Objective", issues.Last().Fields.Type.Name);
            Issue specificIssue = issues.Find(i => i.Fields.Team.Equals(team));
            Assert.NotNull(specificIssue);
        }

    }
}
