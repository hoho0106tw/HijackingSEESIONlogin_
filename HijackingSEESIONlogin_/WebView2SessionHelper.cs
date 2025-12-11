using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Web.WebView2.Core;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;

public class WebView2SessionHelper
{
    private CoreWebView2 _core;

    /// <summary>
    /// 初始化 WebView2（呼叫時傳入 WebView2 控制項）
    /// </summary>
    public async Task InitializeAsync(Microsoft.Web.WebView2.WinForms.WebView2 webView)
    {
        await webView.EnsureCoreWebView2Async();
        _core = webView.CoreWebView2;
    }

    /// <summary>
    /// 導航到指定網址
    /// </summary>
    public void Navigate(string url)
    {
        _core?.Navigate(url);
    }

    /// <summary>
    /// 執行 JavaScript（可用來控制網頁元素）
    /// </summary>
    public async Task ExecuteJsAsync(string script)
    {
        if (_core != null)
            await _core.ExecuteScriptAsync(script);
    }

    /// <summary>
    /// 建立可使用 WebView2 Session 的 HttpClient
    /// </summary>
    private async Task<HttpClient> CreateHttpClientAsync(string url)
    {
        var cookies = await _core.CookieManager.GetCookiesAsync(url);
        var container = new CookieContainer();

        foreach (var c in cookies)
        {
            var netCookie = new Cookie(c.Name, c.Value, c.Path, c.Domain);
            container.Add(netCookie);
        }

        var handler = new HttpClientHandler()
        {
            CookieContainer = container,
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
        };

        return new HttpClient(handler);
    }

    /// <summary>
    /// 自動下載受保護的檔案（不需要 WebView2 事件）
    /// </summary>
    public async Task DownloadFileAsync(string fileUrl, string savePath)
    {
        var client = await CreateHttpClientAsync(fileUrl);

        using var res = await client.GetAsync(fileUrl);
        res.EnsureSuccessStatusCode();

        byte[] data = await res.Content.ReadAsByteArrayAsync();

        Directory.CreateDirectory(Path.GetDirectoryName(savePath));
        File.WriteAllBytes(savePath, data);
    }
}
