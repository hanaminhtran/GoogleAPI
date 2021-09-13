using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HanaToolUtilities.Pages
{
    public class ReverseProxyMiddleware
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly RequestDelegate _nextMiddleware;

        public ReverseProxyMiddleware(RequestDelegate nextMiddleware)
        {
            _nextMiddleware = nextMiddleware;
        }
        // ReverseProxyApplication/ReverseProxyMiddleware.cs
        private bool IsContentOfType(HttpResponseMessage responseMessage, string type)
        {
            var result = false;

            if (responseMessage.Content?.Headers?.ContentType != null)
            {
                result = responseMessage.Content.Headers.ContentType.MediaType == type;
            }

            return result;
        }
        // ReverseProxyApplication/ReverseProxyMiddleware.cs
        private Uri BuildTargetUri(HttpRequest request)
        {
            Uri targetUri = null;
            PathString remainingPath;

            if (request.Path.StartsWithSegments("/googleforms", out remainingPath))
            {
                targetUri = new Uri("https://docs.google.com/forms" + remainingPath);
                targetUri = new Uri("https://translate.google.com/saved");
            }

            if (request.Path.StartsWithSegments("/google", out remainingPath))
            {
                targetUri = new Uri("https://www.google.com" + remainingPath);
            }

            if (request.Path.StartsWithSegments("/googlestatic", out remainingPath))
            {
                targetUri = new Uri(" https://www.gstatic.com" + remainingPath);
            }
            
            return targetUri;
        }

        // ReverseProxyApplication/ReverseProxyMiddleware.cs
        private HttpRequestMessage CreateTargetMessage(HttpContext context, Uri targetUri)
        {
            var requestMessage = new HttpRequestMessage();

            CopyFromOriginalRequestContentAndHeaders(context, requestMessage);

            targetUri = new Uri(QueryHelpers.AddQueryString(targetUri.OriginalString,
                       new Dictionary<string, string>() { { "entry.1884265043", "John Doe" } }));

            requestMessage.RequestUri = targetUri;
            requestMessage.Headers.Host = targetUri.Host;
            requestMessage.Method = GetMethod(context.Request.Method);

            return requestMessage;
        }

        // ReverseProxyApplication/ReverseProxyMiddleware.cs
        private async Task ProcessResponseContent(HttpContext context, HttpResponseMessage responseMessage)
        {
            
            var content = await responseMessage.Content.ReadAsByteArrayAsync();

            if (IsContentOfType(responseMessage, "text/html") ||
                IsContentOfType(responseMessage, "text/javascript"))
            {
                var stringContent = Encoding.UTF8.GetString(content); 
                await context.Response.WriteAsync(stringContent, Encoding.UTF8);
            }
            else
            {
                await context.Response.Body.WriteAsync(content);
            }
        }

        public async Task Invoke(HttpContext context)
        {
            var targetUri = BuildTargetUri(context.Request);

            if (targetUri != null)
            {
                
                var targetRequestMessage = CreateTargetMessage(context, targetUri);
                targetRequestMessage.Headers.TryAddWithoutValidation("authority", "translate.google.com");
                targetRequestMessage.Headers.TryAddWithoutValidation("cache-control", "max-age=0");
                targetRequestMessage.Headers.TryAddWithoutValidation("sec-ch-ua", "\"Google Chrome\";v=\"93\", \" Not;A Brand\";v=\"99\", \"Chromium\";v=\"93\"");
                targetRequestMessage.Headers.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
                targetRequestMessage.Headers.TryAddWithoutValidation("sec-ch-ua-platform", "\"Windows\"");
                targetRequestMessage.Headers.TryAddWithoutValidation("upgrade-insecure-requests", "1");
                targetRequestMessage.Headers.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/93.0.4577.63 Safari/537.36");
                targetRequestMessage.Headers.TryAddWithoutValidation("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                targetRequestMessage.Headers.TryAddWithoutValidation("x-client-data", "CJS2yQEIo7bJAQjEtskBCKmdygEI9dDKAQi49soBCO/yywEItPjLAQie+csBCPX5ywEIr/rLAQii/ssBCL/+ywEInv/LAQji/8sBCOr/ywEYjp7LAQ==");
                targetRequestMessage.Headers.TryAddWithoutValidation("sec-fetch-site", "none");
                targetRequestMessage.Headers.TryAddWithoutValidation("sec-fetch-mode", "navigate");
                targetRequestMessage.Headers.TryAddWithoutValidation("sec-fetch-user", "?1");
                targetRequestMessage.Headers.TryAddWithoutValidation("sec-fetch-dest", "document");
                targetRequestMessage.Headers.TryAddWithoutValidation("accept-language", "en-US,en;q=0.9");
                targetRequestMessage.Headers.TryAddWithoutValidation("cookie", "SEARCH_SAMESITE=CgQIr5MB; _ga=GA1.3.34731402.1629790903; OTZ=6124787_28_28__28_; _gid=GA1.3.498777425.1630940493; SID=BwgETE-gTbYpocDMe5gao0KBTuWxUzYLaQRfAcC9xkPGDgMmgB9hcWC2wWQvaD2Bve18Cg.; __Secure-1PSID=BwgETE-gTbYpocDMe5gao0KBTuWxUzYLaQRfAcC9xkPGDgMm4bnQfuroUeGRZTcs0TNwlA.; __Secure-3PSID=BwgETE-gTbYpocDMe5gao0KBTuWxUzYLaQRfAcC9xkPGDgMm3o8Eem99Ko2mSoUn2tuw_w.; HSID=Acqv_Nvt07hH3Ub6f; SSID=A9ajWsfmTWtqUhrAz; APISID=yt9cVdl0SntL9Q4x/A7Dgq4dD-zRU_qa18; SAPISID=fjQd2E7sQ8SxsXHL/AaBCV6LeuUlZvmaYE; __Secure-1PAPISID=fjQd2E7sQ8SxsXHL/AaBCV6LeuUlZvmaYE; __Secure-3PAPISID=fjQd2E7sQ8SxsXHL/AaBCV6LeuUlZvmaYE; NID=223=AAzx6izQwRtI78DYdjDOr_kS8U2glFv8obmFG3JT6KOSEgjyojA_gEHUNExyZgUl-Vi3BVO99Bdcn31PfrmlNLGS6MTziBx8D8GA5YxDoLbCFb4qjgE9jdDpVZQMUpQbQz9CWQpIi43blPrC53Gj9cOVRXrKdBFk0QcLI7ZslvjPV_8ks4R_h-itO3ZfbiVI7Y08XQY3kbqbSvhj5_AGut0Ys86nZ1WTsKboExMgnNf62i1Pkg3cBu0NtA7kIp8LnqyuYkN9-UwuhJdVnrW2XkZmN1R8LBFIRleCAN-ZUtRmxr-cXGb6gcPvYkKsjEV4Fn6p6mvFw8rm_A3b; 1P_JAR=2021-09-10-19; ab.storage.sessionId.7af503ae-0c84-478f-98b0-ecfff5d67750=%7B%22g%22%3A%227b9d0623-b8f1-b7b0-f9a5-3c75573e282c%22%2C%22e%22%3A2131301010132%2C%22c%22%3A1631191627542%2C%22l%22%3A1631301010132%7D; SIDCC=AJi4QfEs6_uPa8g6kX_AJo2A9ypenp0tbYm7w7BSTtyCwxwObt-rOWLYk6w6C-8IWCoGDrDv9r8; __Secure-3PSIDCC=AJi4QfEXXkokcqwWxg-y99OvUPpSmD0i4vNUuSpu8Xm966vIMnjNW4E4zFpcFSvr2utu7zxhDd0");
                var response = "";
                using (var responseMessage = await _httpClient.SendAsync(targetRequestMessage, HttpCompletionOption.ResponseHeadersRead, context.RequestAborted))
                {
                    context.Response.StatusCode = (int)responseMessage.StatusCode;
                    CopyFromTargetResponseHeaders(context, responseMessage);
                    await ProcessResponseContent(context, responseMessage);
                    response = responseMessage.RequestMessage.ToString();

                }
                


                return;
            }
            await _nextMiddleware(context);
        }
        

        private void CopyFromOriginalRequestContentAndHeaders(HttpContext context, HttpRequestMessage requestMessage)
        {
            var requestMethod = context.Request.Method;

            if (!HttpMethods.IsGet(requestMethod) &&
              !HttpMethods.IsHead(requestMethod) &&
              !HttpMethods.IsDelete(requestMethod) &&
              !HttpMethods.IsTrace(requestMethod))
            {
                var streamContent = new StreamContent(context.Request.Body);
                requestMessage.Content = streamContent;
            }

            foreach (var header in context.Request.Headers)
            {
                requestMessage.Content?.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
            }
        }

        private void CopyFromTargetResponseHeaders(HttpContext context, HttpResponseMessage responseMessage)
        {
            foreach (var header in responseMessage.Headers)
            {
                context.Response.Headers[header.Key] = header.Value.ToArray();
            }

            foreach (var header in responseMessage.Content.Headers)
            {
                context.Response.Headers[header.Key] = header.Value.ToArray();
            }
            context.Response.Headers.Remove("transfer-encoding");
        }
        private static HttpMethod GetMethod(string method)
        {
            if (HttpMethods.IsDelete(method)) return HttpMethod.Delete;
            if (HttpMethods.IsGet(method)) return HttpMethod.Get;
            if (HttpMethods.IsHead(method)) return HttpMethod.Head;
            if (HttpMethods.IsOptions(method)) return HttpMethod.Options;
            if (HttpMethods.IsPost(method)) return HttpMethod.Post;
            if (HttpMethods.IsPut(method)) return HttpMethod.Put;
            if (HttpMethods.IsTrace(method)) return HttpMethod.Trace;
            return new HttpMethod(method);
        }
        

        // ReverseProxyApplication/ReverseProxyMiddleware.cs
        
    }
}
