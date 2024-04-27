using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMCockpit_Library.Services
{
    public interface IHTTPInterceptor
    {
        Task IntakeDNDBeyondRequest(WebView webView, string url);
    }

    public class HTTPInterceptor : IHTTPInterceptor
    {
        public async Task IntakeDNDBeyondRequest(WebView webView, string url)
        {
            if (url.Contains("/monster/"))
            {
                var html = await webView.EvaluateJavaScriptAsync("document.documentElement.outerHTML");

                var monStatBlocIndex = html.IndexOf("<div class=\"mon-stat-block\">");

                var closingIndex = GetClosingTagIndex(html, monStatBlocIndex);
            }
        }

        private int GetClosingTagIndex(string html, int startingIndex)
        {
            var closingDivIndex = startingIndex;
            var openDivs = 1;
            while (openDivs > 0)
            {
                closingDivIndex++;
                if (html[closingDivIndex] == '<')
                {
                    if (html[closingDivIndex + 1] == '/')
                    {
                        openDivs--;
                    }
                    else
                    {
                        openDivs++;
                    }
                }
            }

            return closingDivIndex;
        }
    }
}
