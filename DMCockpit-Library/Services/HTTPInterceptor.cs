using DMCockpit.Services;
using HtmlAgilityPack;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using static DMCockpit_Library.Services.HTTPInterceptor;

namespace DMCockpit_Library.Services
{
    public interface IHTTPInterceptor
    {
        Task IntakeDNDBeyondRequest(WebView webView, string url);

        event DNDBeyondUpdated DNDBeyondUpdateEvent;
    }

    public delegate void DNDBeyondUpdated(string html);

    public class HTTPInterceptor : IHTTPInterceptor
    {
        public event DNDBeyondUpdated? DNDBeyondUpdateEvent;

        private enum DNDTypes
        {
            Monsters,
        }

        public async Task IntakeDNDBeyondRequest(WebView webView, string url)
        {
            if (url.Contains("/monsters/"))
            {
                var html = await webView.EvaluateJavaScriptAsync("document.documentElement.innerHTML");
                html = Regex.Replace(html, @"\\[Uu]([0-9A-Fa-f]{4})", m => char.ToString((char)ushort.Parse(m.Groups[1].Value, NumberStyles.AllowHexSpecifier)));
                html = Regex.Unescape(html);

                var doc = new HtmlDocument();
                doc.LoadHtml(html);

                var htmlNodes = doc.DocumentNode.SelectNodes("//div[contains(@class, 'mon-stat-block')]").ToList();
                var monsterNode = htmlNodes[0];
                htmlNodes = doc.DocumentNode.SelectNodes("//div[contains(@class, 'more-info-content')]").ToList();
                var moreInfoBlock = htmlNodes[0];

                var parsedHTML = monsterNode.OuterHtml + moreInfoBlock.OuterHtml;

                OnImageUpdated(parsedHTML);
            }
        }

        protected virtual void OnImageUpdated(string base64Image) => DNDBeyondUpdateEvent?.Invoke(base64Image);
    }
}