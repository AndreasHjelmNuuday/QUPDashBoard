using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using DotJira;

namespace PlectoUploader
{
    public class PlectoUploader
    {
		public string WriteJiraTicketToPlecto(string issueKey = "",		string issueType = "",		string summary = "",		string status = "",		string rag = "")
        {
			// Please do let us know if you have a better C# example :-)
			string userName = "andhj@nuuday.dk";
			string userPassword = "BKyrKxnfzghrPs4$";
			string teamUUID = "32118bf9432b46af8f2d00be9f25ebdd";

			var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://app.plecto.com/api/v2/registrations/");
			httpWebRequest.ContentType = "application/json";
			httpWebRequest.Method = "POST";
			string authInfo = userName + ":" + userPassword;
			authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
			httpWebRequest.Headers["Authorization"] = "Basic " + authInfo;

			using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
			{
				string json = JsonConvert.SerializeObject(new
				{
					data_source = "b8cdbcf38fe5454fb5503a1a90053d20",  // Data source UUID
																	   //'member' = '<Member UUID>',  either provide the member uuid or member_api_provider and member_api_id and member_name
					member_api_provider = "Jira", // Use appropriate name for the system that use the below ID. Can also be e.g. salesforce, podio, pipedrive or the name of any external system.
					member_api_id = "f86cdba4e3f7465ab309f9abf8afbfc8", // Provide a user ID from the above system
					member_name = "Andreas Hjelm", // Provide the user ID from the above system
					team = teamUUID, // Optional, make registration for team
					external_id = issueKey,  // Change to a unique ID
					


					IssueKey = issueKey,					
					IssueType = issueType,
					Summary =summary,
					Status = status,
					RAG = rag
				});

				streamWriter.Write(json);
			}

			var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			string result;
			using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
			{
				result = streamReader.ReadToEnd();
			}
			return result;
		}

        public string WriteJiraTicketToPlecto(Issue issue)
        {
            throw new NotImplementedException();
        }
    }
}
