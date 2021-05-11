using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotJira
{
    public class QUPHelper
    {
        private readonly string jiraUrl = Constants.JIRA_URL;
        private readonly string jiraUser = Constants.JIRA_USER;
        private readonly string jiraPassword = Constants.JIRA_PASSWORD;
        Jira jira;

        public QUPHelper()
        {            
        }

        public List<Issue> GetAllQUPIssuesInProject(string project, string qup)
        {
            Connect();
            string jql = String.Format("project = {0} AND issuetype in (\"Tribe Objective\", \"Squad Objective\") AND QUP = \"{1}\" ORDER BY Rank ASC", project, qup);
            //string jql = String.Format("project = {0} AND issuetype in (\"Tribe Objective\", \"Squad Objective\", Initiative, Epic) AND QUP = \"{1}\" ORDER BY Rank ASC", project, qup);

            System.Collections.Generic.List<string> fields = new System.Collections.Generic.List<string>(new string[] {
                "summary",
                Constants.TEAM_CUSTOM_FIELD_ID,
                Constants.RAG_CUSTOM_FIELD_ID,
                Constants.PARENT_CUSTOM_FIELD_ID,
                Constants.ISSUE_TYPE_ID,
                "status"
            }); ;

            string statusCode;
            System.Collections.Generic.List<Issue> issues = jira.SearchIssues(out statusCode, jql, fields);
            return issues;
        }

        private void Connect()
        {
            string statusCode = "";
            Connect(out statusCode);
        }

        public void Connect(out string statusCode)
        {
           jira = new Jira(jiraUrl, jiraUser, jiraPassword);
           jira.checkConnection(out statusCode);
        }

        public List<Issue> getAllQupIssuesSorted(string project, string quarter)
        {
            List<Issue> tribeObjectives = new List<Issue>();
            List<Issue> squadObjectives = new List<Issue>();
            List<Issue> issues = GetAllQUPIssuesInProject(project, quarter);

            foreach (Issue issue in issues)
            {
                if (issue.Fields != null && issue.Fields.Type != null)
                {
                    if (issue.Fields.Type.Name.Equals(Constants.ISSUE_TYPE_TRIBE_OBJECTIVE))
                    {
                        tribeObjectives.Add(issue);
                    }
                    else if (issue.Fields.Type.Name.Equals(Constants.ISSUE_TYPE_SQUAD_OBJECTIVE))
                    {
                        squadObjectives.Add(issue);
                    }
                }
            }

            sortSquadObjectives(tribeObjectives, squadObjectives);
            return tribeObjectives;
        }

        private static void sortSquadObjectives(List<Issue> tribeObjectives, List<Issue> squadObjectives)
        {
            foreach (Issue squadObjective in squadObjectives)
            {
                string parent = squadObjective.Fields.Parent;
                Issue parentTribeObjective = tribeObjectives.Find(i => i.Key.Equals(parent));
                if (parentTribeObjective != null)
                {
                    if(parentTribeObjective.Children == null)
                    {
                        parentTribeObjective.Children = new List<Issue>();
                    }
                    parentTribeObjective.Children.Add(squadObjective);
                }
            }
        }
    }
}
