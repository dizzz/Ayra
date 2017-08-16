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
    [LuisModel("060e64fc-a3ad-43cf-a515-abe17680bcae", "ffc3d20557684d9f83fd10ec05ecb23a")]
    [Serializable]
    public class RootLuisDialog : LuisDialog<object> {
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result) {
            /*string responseString = string.Empty;
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
                String str = bing.BingToSearch(question);
                await context.PostAsync(str);
            } else {
                await context.PostAsync(answer);
            }*/
            QNADialog qnadialog =  new QNADialog();
            await context.PostAsync(qnadialog.getQNAstring(context, result));
            context.Wait(MessageReceived);
        }
        [LuisIntent("打招呼")]
        public async Task Hello(IDialogContext context, LuisResult result) {
            string message = $"你好，我是Ayra，属于南京理工大学微软学生俱乐部的BOT，你可以问我有关俱乐部活动、部门、福利的问题。";
            await context.PostAsync(message);
            context.Wait(this.MessageReceived);
        }
        [LuisIntent("报名")]
        public async Task Register(IDialogContext context, LuisResult result) {
            string message = $"在每年十月份俱乐部会组织招新，详情请关注俱乐部公众号:njust_mstc。";
            await context.PostAsync(message);
            context.Wait(this.MessageReceived);
        }
        [LuisIntent("活动")]
        public async Task Activity(IDialogContext context, LuisResult result) {
            string act = "";
            string message = "";
            if (TryToFindAct(result, out act)) {
                if (act == "技术沙龙") {
                    message = $"技术沙龙主要是面向俱乐部成员以讲座为形式，技术分享为内容的一个活动。";
                } else {
                    message = $"兴趣小组是俱乐部小伙伴分组实现一些小项目的活动。";
                }
            } else {
                message = $"俱乐部的有吸猫(雾)，技术沙龙，兴趣小组，和一些福利活动例如春游秋游约桌游。技术沙龙主要是面向俱乐部成员以讲座为形式，技术分享为内容的一个活动。兴趣小组是俱乐部小伙伴分组实现一些小项目的活动。";

            }
            await context.PostAsync(message);
            context.Wait(this.MessageReceived);
        }
        public bool TryToFindAct(LuisResult result, out String act) {
            act = "";
            EntityRecommendation title;
            if (result.TryFindEntity("活动名", out title)) {
                act = title.Entity;
            } else {
                act = "";
            }
            return !act.Equals("");
        }
        [LuisIntent("介绍")]
        public async Task Rep(IDialogContext context, LuisResult result) {
            string message = $"俱乐部现在有美工部，技术部，新媒体策划部，活动部四个部门。美工部的工作主要是做海报和宣传视频，技术部的工作主要开展一些技术活动，写代码QAQ，新媒体策划部的工作主要是管理公众号和微博，活动部的工作主要是进行活动的组织和策划";
            await context.PostAsync(message);
            context.Wait(this.MessageReceived);
        }

    }
}