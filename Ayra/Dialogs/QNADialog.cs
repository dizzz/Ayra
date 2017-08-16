using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
namespace Ayra.Dialogs {
    public class QNADialog{

        public string getQNAstring(IDialogContext context, LuisResult result) {
            string responseString = string.Empty;
            var query = result.Query; //User Query
            var knowledgebaseId = "0071945f-e61d-438b-bc8b-f935b029a8e8"; // Use knowledge base id created.
            var qnamakerSubscriptionKey = "b46f5c7ab108491f8cb061aa4158f2e5"; //Use subscription key assigned to you.
            Uri qnamakerUriBase = new Uri("https://westus.api.cognitive.microsoft.com/qnamaker/v1.0");
            var builder = new UriBuilder($"{qnamakerUriBase}/knowledgebases/{knowledgebaseId}/generateAnswer");
            var postBody = $"{{\"question\": \"{query}\"}}";
            using (WebClient client = new WebClient()) {
                client.Encoding = System.Text.Encoding.UTF8;
                client.Headers.Add("Ocp-Apim-Subscription-Key", qnamakerSubscriptionKey);
                client.Headers.Add("Content-Type", "application/json");
                responseString = client.UploadString(builder.Uri, postBody);
            }
            QnAMakerResult response;
            try {
                response = JsonConvert.DeserializeObject<QnAMakerResult>(responseString);
            } catch {
                throw new Exception("Unable to deserialize QnA Maker response string.");
            }
            string answer = response.Answer;
            if (answer.Equals("No good match found in the KB")) {
                var a = context.MakeMessage();
                a.Text = result.Query;
                Search bing = new Search();
                String question = result.Query;
                return bing.BingToSearch(question);
            } else {
                return answer;
            }
        }
    }
}