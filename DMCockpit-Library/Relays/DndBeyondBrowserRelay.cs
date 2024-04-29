using HtmlAgilityPack;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DMCockpit_Library.Relays
{
    public interface DndBeyondBrowserRelay
    {
        Task IntakeDNDBeyondRequest(WebView webView, string url);

        event DNDBeyondUpdated DNDBeyondUpdateEvent;
    }

    public delegate void DNDBeyondUpdated(string html);

    public class WebviewInterceptor : DndBeyondBrowserRelay
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
                var parsedHTML = await GetHTMLByClassName(webView, "mon-stat-block") + await GetHTMLByClassName(webView, "more-info-content");

                await AddButtonToElementByClassName(webView, "monster-image");

                OnImageUpdated(parsedHTML);
            }
        }

        private async Task AddButtonToElementByClassName(WebView webView, string className)
        {
            var jsText = $"AddGetImageSrcButton(GetElementsByClass(\"{className}\"), \"{className}\")";
            await webView.EvaluateJavaScriptAsync(jsText);
        }

        private async Task<string> GetHTMLByClassName(WebView webView, string className)
        {
            var rawHTML = await GetRawHTMLByClassName(webView, className);
            var parsedHTML = GetParsedHTMLByClass(rawHTML, className);

            return parsedHTML;
        }

        private async Task<string> GetRawHTMLByClassName(WebView webView, string className)
        {
            var html = await webView.EvaluateJavaScriptAsync($"document.getElementsByClassName(\"{className}\")[0].outerHTML");
            html = Regex.Replace(html, @"\\[Uu]([0-9A-Fa-f]{4})", m => char.ToString((char)ushort.Parse(m.Groups[1].Value, NumberStyles.AllowHexSpecifier)));
            html = Regex.Unescape(html);

            return html;
        }

        private string GetParsedHTMLByClass(string html, string className)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var htmlNodes = doc.DocumentNode.SelectNodes($"//div[contains(@class, '{className}')]").ToList();
            var node = htmlNodes[0];

            return node.OuterHtml;
        }

        protected virtual void OnImageUpdated(string base64Image) => DNDBeyondUpdateEvent?.Invoke(base64Image);
    }
}