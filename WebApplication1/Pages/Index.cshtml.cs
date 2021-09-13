using com.sun.org.apache.xerces.@internal.parsers;
using com.sun.tools.doclets.formats.html.markup;
using HanaToolUtilities.Models;
using javax.swing.text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace WebApplication1.Pages
{

    public class IndexModel : PageModel
    {

        private readonly MvcOptions _mvcOptions;
        private string stringContent = "";        
        const string SessionName = "temp";
        public IList<GoogleWord> GoogleWords { get; set; }
        public object JsonRequestBehavior { get; private set; }

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //do something with the person class
            string output = true ? "Welcome to the Admin User" : "Welcome to the User";
            return Page();
        }

        public ActionResult GetMp3(string name)
        {
            // Initialization.              
            // Prepare Ajax Response as JSON Data Result.  
            string output = true ? "Welcome to the Admin User" : "Welcome to the User";             
            return Json(output, JsonRequestBehavior);
            
        }
        
        public JsonResult Test()
        {
            return new JsonResult("Ajax Test");
        }

        private ActionResult Json(string output, object allowGet)
        {
            throw new NotImplementedException();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            //GoogleWords = new List<GoogleWord>();
            //GoogleWords.Add(new GoogleWord("tu1", "nghia 1"));
            //GoogleWords.Add(new GoogleWord("tu1", "nghia 1"));
            //GoogleWords.Add(new GoogleWord("tu1", "nghia 1"));
            //GoogleWords.Add(new GoogleWord("tu1", "nghia 1"));
            //GoogleWords.Add(new GoogleWord("tu1", "nghia 1"));

            
            await GetDict();
            HttpContext.Session.SetString(SessionName, stringContent);
            return Page();
        }
        private Double speed;
        


        public async Task GetVoice(string phrase)
        {
            var handler = new HttpClientHandler();
            handler.AutomaticDecompression = ~DecompressionMethods.None;
            string url = String.Format("https://translate.google.com.vn/translate_tts?ie=UTF-8&q={0}&tl=en&client=tw-ob&ttsspeed={1}", phrase, speed.ToString("F2", CultureInfo.CreateSpecificCulture("en-GB")));
            using (var httpClient = new HttpClient(handler))            
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), url))
                {
                    request.Headers.TryAddWithoutValidation("authority", "translate.google.com");
                    request.Headers.TryAddWithoutValidation("cache-control", "max-age=0");
                    request.Headers.TryAddWithoutValidation("sec-ch-ua", "\"Google Chrome\";v=\"93\", \" Not;A Brand\";v=\"99\", \"Chromium\";v=\"93\"");
                    request.Headers.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
                    request.Headers.TryAddWithoutValidation("sec-ch-ua-platform", "\"Windows\"");
                    request.Headers.TryAddWithoutValidation("upgrade-insecure-requests", "1");
                    request.Headers.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/93.0.4577.63 Safari/537.36");
                    request.Headers.TryAddWithoutValidation("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                    request.Headers.TryAddWithoutValidation("x-client-data", "CJS2yQEIo7bJAQjEtskBCKmdygEI9dDKAQi49soBCO/yywEItPjLAQie+csBCPX5ywEIr/rLAQii/ssBCL/+ywEInv/LAQji/8sBCOr/ywEYjp7LAQ==");
                    request.Headers.TryAddWithoutValidation("sec-fetch-site", "none");
                    request.Headers.TryAddWithoutValidation("sec-fetch-mode", "navigate");
                    request.Headers.TryAddWithoutValidation("sec-fetch-user", "?1");
                    request.Headers.TryAddWithoutValidation("sec-fetch-dest", "document");
                    request.Headers.TryAddWithoutValidation("accept-language", "en-US,en;q=0.9");
                    request.Headers.TryAddWithoutValidation("cookie", "SEARCH_SAMESITE=CgQIr5MB; _ga=GA1.3.34731402.1629790903; OTZ=6124787_28_28__28_; _gid=GA1.3.498777425.1630940493; SID=BwgETE-gTbYpocDMe5gao0KBTuWxUzYLaQRfAcC9xkPGDgMmgB9hcWC2wWQvaD2Bve18Cg.; __Secure-1PSID=BwgETE-gTbYpocDMe5gao0KBTuWxUzYLaQRfAcC9xkPGDgMm4bnQfuroUeGRZTcs0TNwlA.; __Secure-3PSID=BwgETE-gTbYpocDMe5gao0KBTuWxUzYLaQRfAcC9xkPGDgMm3o8Eem99Ko2mSoUn2tuw_w.; HSID=Acqv_Nvt07hH3Ub6f; SSID=A9ajWsfmTWtqUhrAz; APISID=yt9cVdl0SntL9Q4x/A7Dgq4dD-zRU_qa18; SAPISID=fjQd2E7sQ8SxsXHL/AaBCV6LeuUlZvmaYE; __Secure-1PAPISID=fjQd2E7sQ8SxsXHL/AaBCV6LeuUlZvmaYE; __Secure-3PAPISID=fjQd2E7sQ8SxsXHL/AaBCV6LeuUlZvmaYE; NID=223=AAzx6izQwRtI78DYdjDOr_kS8U2glFv8obmFG3JT6KOSEgjyojA_gEHUNExyZgUl-Vi3BVO99Bdcn31PfrmlNLGS6MTziBx8D8GA5YxDoLbCFb4qjgE9jdDpVZQMUpQbQz9CWQpIi43blPrC53Gj9cOVRXrKdBFk0QcLI7ZslvjPV_8ks4R_h-itO3ZfbiVI7Y08XQY3kbqbSvhj5_AGut0Ys86nZ1WTsKboExMgnNf62i1Pkg3cBu0NtA7kIp8LnqyuYkN9-UwuhJdVnrW2XkZmN1R8LBFIRleCAN-ZUtRmxr-cXGb6gcPvYkKsjEV4Fn6p6mvFw8rm_A3b; 1P_JAR=2021-09-10-19; ab.storage.sessionId.7af503ae-0c84-478f-98b0-ecfff5d67750=%7B%22g%22%3A%227b9d0623-b8f1-b7b0-f9a5-3c75573e282c%22%2C%22e%22%3A2131301010132%2C%22c%22%3A1631191627542%2C%22l%22%3A1631301010132%7D; SIDCC=AJi4QfEs6_uPa8g6kX_AJo2A9ypenp0tbYm7w7BSTtyCwxwObt-rOWLYk6w6C-8IWCoGDrDv9r8; __Secure-3PSIDCC=AJi4QfEXXkokcqwWxg-y99OvUPpSmD0i4vNUuSpu8Xm966vIMnjNW4E4zFpcFSvr2utu7zxhDd0");

                    using (var responseMessage = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, CancellationToken.None))
                    {

                        var content = await responseMessage.Content.ReadAsByteArrayAsync();
                        stringContent = Encoding.UTF8.GetString(content);

                        SoundPlayer player = new SoundPlayer();
                        string soundurl = url;
                        player.SoundLocation = soundurl;
                        player.Play();
                        System.Threading.Thread.Sleep(2000);
                        player.Stop();

                    }


                }
            }



        }

        public async  Task GetDict()
        {
            var handler = new HttpClientHandler();       
            handler.AutomaticDecompression = ~DecompressionMethods.None;           
            using (var httpClient = new HttpClient(handler))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://translate.google.com/saved"))
                {
                    request.Headers.TryAddWithoutValidation("authority", "translate.google.com");
                    request.Headers.TryAddWithoutValidation("cache-control", "max-age=0");
                    request.Headers.TryAddWithoutValidation("sec-ch-ua", "\"Google Chrome\";v=\"93\", \" Not;A Brand\";v=\"99\", \"Chromium\";v=\"93\"");
                    request.Headers.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
                    request.Headers.TryAddWithoutValidation("sec-ch-ua-platform", "\"Windows\"");
                    request.Headers.TryAddWithoutValidation("upgrade-insecure-requests", "1");
                    request.Headers.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/93.0.4577.63 Safari/537.36");
                    request.Headers.TryAddWithoutValidation("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                    request.Headers.TryAddWithoutValidation("x-client-data", "CJS2yQEIo7bJAQjEtskBCKmdygEI9dDKAQi49soBCO/yywEItPjLAQie+csBCPX5ywEIr/rLAQii/ssBCL/+ywEInv/LARiOnssB");
                    request.Headers.TryAddWithoutValidation("sec-fetch-site", "none");
                    request.Headers.TryAddWithoutValidation("sec-fetch-mode", "navigate");
                    request.Headers.TryAddWithoutValidation("sec-fetch-user", "?1");
                    request.Headers.TryAddWithoutValidation("sec-fetch-dest", "document");
                    request.Headers.TryAddWithoutValidation("accept-language", "en-US,en;q=0.9");
                    request.Headers.TryAddWithoutValidation("cookie", "1P_JAR=2021-09-12-19; ab.storage.userId.7af503ae-0c84-478f-98b0-ecfff5d67750=%7B%22g%22%3A%22browser-1629881044654-2%22%2C%22c%22%3A1631473438163%2C%22l%22%3A1631473438163%7D; ab.storage.deviceId.7af503ae-0c84-478f-98b0-ecfff5d67750=%7B%22g%22%3A%220d6d58ea-9707-640a-72ea-27d17a8e793e%22%2C%22c%22%3A1631473438165%2C%22l%22%3A1631473438165%7D; _ga=GA1.3.800676570.1631473459; _gid=GA1.3.60656329.1631473459; OTZ=6152824_28_28__28_; SID=BwgETD2w4FI6qQIDEmiiM1fEZea_FwhGGwgfldUQdxgRwWNT-If4Knm8yw0qBVcqbGt7yQ.; __Secure-1PSID=BwgETD2w4FI6qQIDEmiiM1fEZea_FwhGGwgfldUQdxgRwWNTGKmgQmhamdGbUUUs02lTQA.; __Secure-3PSID=BwgETD2w4FI6qQIDEmiiM1fEZea_FwhGGwgfldUQdxgRwWNTi8ZdovjUe8TmCBk4H2WiYg.; HSID=AOqvKulPmOGisVqDw; SSID=ApX7fogvRlwvuRB95; APISID=odPQgerB5N9H8GVH/ANEjv3yFVE9ZfrOCM; SAPISID=ue-t7cln38MHE_rS/AyOF-EprhMt0P-PWV; __Secure-1PAPISID=ue-t7cln38MHE_rS/AyOF-EprhMt0P-PWV; __Secure-3PAPISID=ue-t7cln38MHE_rS/AyOF-EprhMt0P-PWV; SEARCH_SAMESITE=CgQIwpMB; NID=223=l6Fh5BXHvkkweBYHp0R7Q-t3xnvy6NMNjgM7P9kGj7P6Tsn-TZlWOg-E2NsJfganb4AVQg10yHyUuDHBLvAyscJu5crjVcjzvbmwGvzd9ihms3PqzRk1SI9icxoQCV5olxDvWLkjXfnfIpLDA2cTL28s9t7Rcl3BIun0HrsveAfpvIw51tHgw7KZqkAELgCuMqFpJeG9lQaxUyFupF9o8xJGS7QBUmr8ZmDGKrXrb0K10Dor1g-jtOM3OxKYzHk_rhJgYrZ6ml7TC_dkg8UXAMlLatvX; ab.storage.sessionId.7af503ae-0c84-478f-98b0-ecfff5d67750=%7B%22g%22%3A%22e3aace39-c604-ef8e-e753-57859f74637c%22%2C%22e%22%3A2131473551000%2C%22c%22%3A1631473438164%2C%22l%22%3A1631473551000%7D; SIDCC=AJi4QfGZQJnbqqwGRZvldRxsFqmmcA4WTo6EEPx6iqUI85YA9mF2v_E8wy91o92lmP2KjBXLdA; __Secure-3PSIDCC=AJi4QfEVzLrEy3I0ofLHKzTXn-TA2fAJlorollnbO-RRon5j3PE_fOAht9A8eil3zD9-NHPJ");

                    using (var responseMessage = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, CancellationToken.None))
                    {

                        var content = await responseMessage.Content.ReadAsByteArrayAsync();
                        stringContent = Encoding.UTF8.GetString(content);                        
                        ViewData["temp"] = stringContent;                        

                    }                    


                }
            }



        }

    }        


}
