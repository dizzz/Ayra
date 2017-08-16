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
            string query = result.Query;
            if (query.Contains("澡堂")) {
                await context.PostAsync("很遗憾，我们俱乐部并没有澡堂");
            } else if (query.Contains("微软")) {
                await context.PostAsync("俱乐部提供了接近微软的渠道。加入俱乐部，就有参观微软，更有机会参与微软实习");

            } else {
                QNA qnadialog = new QNA();
                await context.PostAsync(qnadialog.getQNAstring(context, result));
            }
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
            string message = $"在每年十月份俱乐部会组织招新，详情请关注俱乐部公众号:njust_mstc";
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
            string message = "";
            if (result.Query.Contains("美工部")) {
                message = "美工部的工作主要是做海报和宣传视频";
            } else if (result.Query.Contains("技术部")) {
                message = "技术部的工作主要开展一些技术活动，写代码QAQ";
            } else if (result.Query.Contains("新媒体策划部")) {
                message = "新媒体策划部的工作主要是管理公众号和微博";
            } else if (result.Query.Contains("活动部")) {
                message = "活动部的工作主要是进行活动的组织和策划";
            } else
                message = $"俱乐部现在有美工部，技术部，新媒体策划部，活动部四个部门，你想要了解那个部门呀？";
            await context.PostAsync(message);
            context.Wait(this.MessageReceived);
        }
        [LuisIntent("比赛")]
        public async Task Competation(IDialogContext context, LuisResult result) {
            string message = $"每年上半年，我们有微软组织的编程之美和南京四校的hackathon。";
            await context.PostAsync(message);
            context.Wait(this.MessageReceived);
        }

    }
}