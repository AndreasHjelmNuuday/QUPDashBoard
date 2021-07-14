using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotJira
{
    public class QUPHelper
    {
        private readonly string jiraUrl = Credentials.JIRA_URL;
        private readonly string jiraUser = Credentials.JIRA_USER;
        private readonly string jiraPassword = Credentials.JIRA_PASSWORD;
        Jira jira;

        public QUPHelper()
        {            
        }

        public List<Issue> GetAllQUPIssuesInProject(string project, string qup)
        {
            Connect();
          
            string jql = String.Format("project = {0} AND issuetype in (\"Tribe Objective\", \"Squad Objective\", Epic) AND (QUP = \"{1}\" OR issueFunction in linkedIssuesOf(\"QUP = '{1}'\", \"is implemented by\")OR issueFunction in portfolioChildrenOf(\"QUP = '{1}'\")) ORDER BY Rank ASC", project, qup);
            
            //string jql = String.Format("project = {0} AND issuetype in (\"Tribe Objective\", \"Squad Objective\", Epic) AND QUP = \"{1}\" ORDER BY Rank ASC", project, qup);

            System.Collections.Generic.List<string> fields = new System.Collections.Generic.List<string>(new string[] {
                "summary",
                Constants.TEAM_CUSTOM_FIELD_ID,
                Constants.RAG_CUSTOM_FIELD_ID,
                Constants.PARENT_CUSTOM_FIELD_ID,
                Constants.ISSUE_TYPE_ID,
                "issuelinks",
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
            List<Issue> epics = new List<Issue>();
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
                    else if (issue.Fields.Type.Name.Equals(Constants.ISSUE_TYPE_Epic))
                    {
                        epics.Add(issue);
                    }
                }
            }

            addChildren(tribeObjectives, squadObjectives);
            addChildren(squadObjectives, epics);
            return tribeObjectives;
        }

        private static void addChildren(List<Issue> parents, List<Issue> children)
        {
            foreach (Issue childIssue in children)
            {
                string parent = childIssue.Fields.Parent;
                Issue parentIssue = parents.Find(i => i.Key.Equals(parent));
                if (parentIssue != null)
                {                
                    parentIssue.Children.Add(childIssue);
                    IncreaseDoneChildren(childIssue, parentIssue);
                }
                else if (parentIssue == null) //Epic linked by Implements
                {   
                    if(childIssue.Fields.linkedIssues.Count > 0)
                    {
                        foreach (LinkedIssue linked in childIssue.Fields.linkedIssues) {

                            if (linked.OutwardIssue != null && linked.LinkType.name.Equals(Constants.LINK_TYPE_IMPLEMENTATION_ID))
                            {
                                parentIssue = parents.Find(i => i.Key.Equals(linked.OutwardIssue.Key));
                                if(parentIssue != null)
                                {
                                    parentIssue.IsImplementedBy.Add(childIssue);
                                    IncreaseDoneChildren(childIssue, parentIssue);
                                }
                            }
                        }                        
                    }                    
                }
            }
        }

        private static void IncreaseDoneChildren(Issue childIssue, Issue parentIssue)
        {
            if (childIssue.Fields.Status.Name.ToLower().Equals("done"))
            {
                parentIssue.DoneChildren++;
            }
        }
    }
}
