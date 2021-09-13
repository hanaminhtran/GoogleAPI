using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ConsoleApp3
{
    class Program
    {

        //convert curl to c# https://curl.olsh.me/
        //curl to another language : https://curl.trillworks.com/
        //sent curl https://reqbin.com/req/python/c-xgafmluu/convert-curl-to-python-requests
        //copy XHR from development tools to curl base
        static void SavePhraseBook(string phraseWordsFrom, string phraseWordsTo, string fromLang, string toLang)
        {
            var handler = new HttpClientHandler();
            // If you are using .NET Core 3.0+ you can replace `~DecompressionMethods.None` to `DecompressionMethods.All`
            handler.AutomaticDecompression = ~DecompressionMethods.None;
            var content = "f.req=%5B%5B%5B%22Mgjtcb%22%2C%22%5B%5C%22" + fromLang + "%5C%22%2C%5C%22" + toLang + "%5C%22%2C%5C%22" + phraseWordsFrom + "%5C%22%2C%5C%22" + phraseWordsTo + "%5C%22%2Cnull%5D%22%2Cnull%2C%22generic%22%5D%5D%5D&at=AD08yZm5b70GY7dyDLRNzoYqfdqC%3A1631191876837&";
            // In production code, don't destroy the HttpClient through using, but better reuse an existing instance
            // https://www.aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/
            using (var httpClient = new System.Net.Http.HttpClient(handler))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://translate.google.com/_/TranslateWebserverUi/data/batchexecute?rpcids=Mgjtcb&f.sid=-5613176623783499255&bl=boq_translate-webserver_20210907.10_p1&hl=vi&soc-app=1&soc-platform=1&soc-device=1&_reqid=471479&rt=c"))
                {
                    request.Headers.TryAddWithoutValidation("authority", "translate.google.com");
                    request.Headers.TryAddWithoutValidation("sec-ch-ua", "\"Google Chrome\";v=\"93\", \" Not;A Brand\";v=\"99\", \"Chromium\";v=\"93\"");
                    request.Headers.TryAddWithoutValidation("x-same-domain", "1");
                    request.Headers.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
                    request.Headers.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/93.0.4577.63 Safari/537.36");
                    request.Headers.TryAddWithoutValidation("sec-ch-ua-platform", "\"Windows\"");
                    request.Headers.TryAddWithoutValidation("accept", "*/*");
                    request.Headers.TryAddWithoutValidation("origin", "https://translate.google.com");
                    request.Headers.TryAddWithoutValidation("x-client-data", "CJS2yQEIo7bJAQjEtskBCKmdygEI9dDKAQi49soBCO/yywEItPjLAQie+csBCPX5ywEIr/rLAQii/ssBCL/+ywEInv/LAQji/8sBGI6eywE=");
                    request.Headers.TryAddWithoutValidation("sec-fetch-site", "same-origin");
                    request.Headers.TryAddWithoutValidation("sec-fetch-mode", "cors");
                    request.Headers.TryAddWithoutValidation("sec-fetch-dest", "empty");
                    request.Headers.TryAddWithoutValidation("referer", "https://translate.google.com/");
                    request.Headers.TryAddWithoutValidation("accept-language", "en-US,en;q=0.9");
                    request.Headers.TryAddWithoutValidation("cookie", "SEARCH_SAMESITE=CgQIr5MB; _ga=GA1.3.34731402.1629790903; OTZ=6124787_28_28__28_; _gid=GA1.3.498777425.1630940493; SID=BwgETE-gTbYpocDMe5gao0KBTuWxUzYLaQRfAcC9xkPGDgMmgB9hcWC2wWQvaD2Bve18Cg.; __Secure-1PSID=BwgETE-gTbYpocDMe5gao0KBTuWxUzYLaQRfAcC9xkPGDgMm4bnQfuroUeGRZTcs0TNwlA.; __Secure-3PSID=BwgETE-gTbYpocDMe5gao0KBTuWxUzYLaQRfAcC9xkPGDgMm3o8Eem99Ko2mSoUn2tuw_w.; HSID=Acqv_Nvt07hH3Ub6f; SSID=A9ajWsfmTWtqUhrAz; APISID=yt9cVdl0SntL9Q4x/A7Dgq4dD-zRU_qa18; SAPISID=fjQd2E7sQ8SxsXHL/AaBCV6LeuUlZvmaYE; __Secure-1PAPISID=fjQd2E7sQ8SxsXHL/AaBCV6LeuUlZvmaYE; __Secure-3PAPISID=fjQd2E7sQ8SxsXHL/AaBCV6LeuUlZvmaYE; NID=223=RR0Haf163dRip1rBvt4q2AfHVbA-xePajwdpiZTwXyALUE77mNLnobPv2MJYiT23eGAHM8x5bYNebTWPLqaAfSVp5gJAjEaOwocgncWPt7yi8uKX5svDoSfoYBk08Z-UHGxsZFX9CohvkqkgTdtDwOq2chNT19bqi0OIOUTGF__ZksYgf-YJaabMUoZdJzgm3FM49-Kno0y1h8cwysrcEAqShJxJ4XxbIHmKyo81pGEMzcDzNfoU_2hXv1cOmMKr2Pe_OWaCqObrD58Z51zA_nvV5sZd6y-uUJGh5aLqXY5mcTQfZe08Mp9XeodzVNkbc-Tn1x22mOX-gNkS; 1P_JAR=2021-09-09-12; ab.storage.sessionId.7af503ae-0c84-478f-98b0-ecfff5d67750=%7B%22g%22%3A%227b9d0623-b8f1-b7b0-f9a5-3c75573e282c%22%2C%22e%22%3A2131191878071%2C%22c%22%3A1631191627542%2C%22l%22%3A1631191878071%7D; SIDCC=AJi4QfGuf65lt-SfF69VV_S5RoQDZO65b1eZnYvGpXe58fffsur6DGRaIF3TyUE2PUqpb6-kSi8; __Secure-3PSIDCC=AJi4QfHsY5uvsDa1fzJX2YYylFQXaO474b4dd-AjH7_uiAzCcy5VbATfZi_R8rGR-DTbXC0tyBg");
                    request.Content = new StringContent(content);
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded;charset=UTF-8");
                    var response = httpClient.SendAsync(request);
                    response.Wait();
                    string result = response.Result.ToString();
                    Console.WriteLine(result);
                }
            }


        }
        static void Main(string[] args)
        {


            //Application.EnableVisualStyles();
            //Application.Run(new Form()); // or whatever          
            SavePhraseBook("shit what is wrong", "chos ", "en", "vi");
        }
    }
}
