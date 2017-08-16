using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Ayra {
    public class Search {
        public string BingToSearch(string question) {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "281dbe3f0d724b7eb3be917128cf6271");
            var query = "https://api.cognitive.microsoft.com/bing/v7.0/search?" + "q=" + question + "&count=100&mkt=zh-CN" + "site:nuaa.edu.cn";

            HttpResponseMessage httpResponseMessage = client.GetAsync(query).Result;

            var responseContentString = httpResponseMessage.Content.ReadAsStringAsync().Result;
            Newtonsoft.Json.Linq.JObject responseObjects = Newtonsoft.Json.Linq.JObject.Parse(responseContentString);
            if (httpResponseMessage.IsSuccessStatusCode) {
                string[] rankingGroups = new string[] { "pole", "mainline", "sidebar" };
                foreach (string rankingName in rankingGroups) {
                    Newtonsoft.Json.Linq.JToken rankingResponseItems = responseObjects.SelectToken($"rankingResponse.{rankingName}.items");
                    if (rankingResponseItems != null) {
                        Newtonsoft.Json.Linq.JObject rankingResponseItem = (Newtonsoft.Json.Linq.JObject)rankingResponseItems.ElementAt(0);
                        var resultIndex = rankingResponseItem.GetValue("resultIndex");
                        Newtonsoft.Json.Linq.JToken items = responseObjects.SelectToken("webPages.value");
                        Newtonsoft.Json.Linq.JToken item = items.ElementAt((int)resultIndex);
                        string url = (string)item["url"];
                        string snippet = (string)item["snippet"];
                        return snippet;
                    }
                }
                return null;
            } else {
                return null;
            }
        }
    }
}