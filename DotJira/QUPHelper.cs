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

        public List<Issue> GetAllQUPIssuesInProject(string project, string quarter)
        {
            string jql = String.Format(
                "project = {0} AND " +
                "(issueFunction in portfolioChildrenOf(\"QUP = '{1}'\") OR(QUP = \"{1}\" OR issueFunction in linkedIssuesOf(\"QUP = '{1}'\", \"is implemented by\")))" +
                "  ORDER BY Rank ASC", project, quarter);

            List<Issue> issues = GetIssues(jql);
            if (issues != null)
            {
                return SortAllIssues(issues);
            }
            return null;
        }



        public List<Issue> GetIssues(string jQLQuery)
        {
            Connect();
            System.Collections.Generic.List<string> fields = new(new string[] {
                "summary",
                Constants.TEAM_CUSTOM_FIELD_ID,
                Constants.RAG_CUSTOM_FIELD_ID,
                Constants.PARENT_CUSTOM_FIELD_ID,
                Constants.ISSUE_TYPE_ID,
                Constants.KEY_RESULT_CUSTOM_FIELD_ID,
                Constants.EPIC_LINK_CUSTOM_FIELD_ID,
                Constants.ASSIGNEE_ID,
                "issuelinks",
                "status"
            });

            string statusCode;
            System.Collections.Generic.List<Issue> issues = jira.SearchIssues(out statusCode, jQLQuery, fields);
            return issues;
        }

        public List<Issue> GetTeamIssues(string project, string quarter, string team)
        {
            String jql = String.Format(
                "project = {0} " +
                "AND (issueFunction in portfolioChildrenOf(\"Team = {2} AND QUP = '{1}'\") " +
                "OR Team = {2} AND QUP='{1}' " +
                "OR  issueFunction in issuesInEpics(\"issueFunction in portfolioChildrenOf(\\\"Team = {2} AND QUP = '{1}'\\\")\")) ORDER BY rank ASC", project, quarter, team);
            List<Issue> issues = GetIssues(jql);
            bool allowOrphans = true;
            if (issues != null)
            {
                return SortAllIssues(issues, allowOrphans);
            }
            return null;
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

            List<Issue> issues = GetAllQUPIssuesInProject(project, quarter);

            return SortAllIssues(issues);
        }

        private static List<Issue> SortAllIssues(List<Issue> issues, bool allowOrphans = false)
        {
            List<Issue> tribeObjectives = new();
            List<Issue> squadObjectives = new();
            List<Issue> epics = new();
            List<Issue> subIssues = new();
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
                    else
                    {
                        subIssues.Add(issue);
                    }
                }
            }

            AddChildren(tribeObjectives, squadObjectives, allowOrphans);
            AddChildren(squadObjectives, epics);
            AddChildren(epics, subIssues);
            return tribeObjectives;
        }

        private static void AddChildren(List<Issue> parents, List<Issue> children, bool allowOrphans = false)
        {
            foreach (Issue childIssue in children)
            {
                string parentKey = childIssue.Fields.Parent;
                Issue parentIssue = parents.Find(i => i.Key.Equals(parentKey));

                if (allowOrphans) //hack for squad objectives that are not linked to a tribe objective
                {
                    parents.Remove(parentIssue);
                    //if(childIssue.Fields.Type.Name.Equals("Squad Objective"))
                    //{
                        parents.Add(childIssue);
                    //}
                }

                else if (parentIssue != null)
                {
                    AddChild(childIssue, parentIssue);
                }                
                else if (parentIssue == null) //Issues linked by Implements
                {
                    foreach (LinkedIssue linked in childIssue.Fields.linkedIssues)
                    {

                        if (linked.OutwardIssue != null && linked.LinkType.name.Equals(Constants.LINK_TYPE_IMPLEMENTATION_ID))
                        {
                            parentIssue = parents.Find(i => i.Key.Equals(linked.OutwardIssue.Key));
                            if (parentIssue != null)
                            {
                                AddChild(childIssue, parentIssue);
                            }
                        }
                    }
                    if (childIssue.Fields.EpicLink != null)//epic links
                    {
                        parentIssue = parents.Find(i => i.Key.Equals(childIssue.Fields.EpicLink));
                        if (parentIssue != null)
                        {
                            AddChild(childIssue, parentIssue);
                        }
                    }
                }

            }
        }

        private static void AddChild(Issue childIssue, Issue parentIssue)
        {
            parentIssue.Children.Add(childIssue);
            childIssue.Parent = parentIssue.Key;
            IncreaseDoneChildren(childIssue, parentIssue);
        }

        private static void IncreaseDoneChildren(Issue childIssue, Issue parentIssue)
        {
            if (childIssue.Fields.Status.Name.ToLower().Equals("done"))
            {
                parentIssue.DoneChildren++;
            }
        }

        public List<Issue> GetSpecificIssue(string issueKey)
        {
            String jql = String.Format("issueFunction in issuesInEpics('Key = {0}') " +
                "OR issueFunction in issuesInEpics(\"issueFunction in portfolioChildrenOf('Key = {0}')\") " +
                "OR issueFunction in portfolioChildrenOf('Key = {0}') " +
                "OR issueFunction in linkedIssuesOf('Key = {0}', 'is implemented by') " +
                "OR issueFunction in linkedIssuesOf('Key = {0}', 'implements') " + 
                "OR issueFunction in issuesInEpics(\"issueFunction in linkedIssuesOf('Key = {0}', 'is implemented by')\")" +
                "OR issueFunction in portfolioParentsOf('Key = {0}') OR Key = {0}", issueKey);

            List<Issue> issues = GetIssues(jql);
            bool allowOrphans = true;
            if (issues != null)
            {
                return SortAllIssues(issues, allowOrphans);
            }
            return null;
        }
    }
}
