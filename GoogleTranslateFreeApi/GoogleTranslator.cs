using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GoogleTranslateFreeApi.TranslationData;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GoogleTranslateFreeApi
{
	/// <summary>
	/// Represent a class for translate the text using <see href="http://translate.google.com"/>
	/// </summary>
	public class GoogleTranslator: ITranslator
  {
    private readonly GoogleKeyTokenGenerator _generator;
	  private readonly HttpClient _httpClient;
		private TimeSpan _timeOut;
		private IWebProxy _proxy;

		protected Uri Address;
		
		/// <summary>
		/// Requests timeout
		/// </summary>
		public TimeSpan TimeOut
		{
			get { return _timeOut; }
			set
			{
				_timeOut = value;
				_generator.TimeOut = value;
			}
		}
		
		/// <summary>
		/// Requests proxy
		/// </summary>
		public IWebProxy Proxy
		{
			get { return _proxy; }
			set
			{
				_proxy = value;
				_generator.Proxy = value;
			}
		}

		public string Domain
		{
			get { return Address.AbsoluteUri.GetTextBetween("https://", "/translate_a/single"); }
			set { Address = new Uri($"https://{value}/translate_a/single"); }
		}

		/// <summary>
		/// An Array of supported languages by google translate
		/// </summary>
		public static Language[] LanguagesSupported { get; }

		/// <param name="language">Full name of the required language</param>
		/// <example>GoogleTranslator.GetLanguageByName("English")</example>
		/// <returns>Language object from the LanguagesSupported array</returns>
		public static Language GetLanguageByName(string language)
			=> LanguagesSupported.FirstOrDefault(i
				=> i.FullName.Equals(language, StringComparison.OrdinalIgnoreCase));

	  /// <param name="iso">ISO of the required language</param>
	  /// <example>GoogleTranslator.GetLanguageByISO("en")</example>
	  /// <returns>Language object from the LanguagesSupported array</returns>
	  // ReSharper disable once InconsistentNaming
		public static Language GetLanguageByISO(string iso)
			=> LanguagesSupported.FirstOrDefault(i
				=> i.ISO639.Equals(iso, StringComparison.OrdinalIgnoreCase));

		/// <summary>
		/// Check is available language to translate
		/// </summary>
		/// <param name="language">Checked <see cref="Language"/> </param>
		/// <returns>Is it available language or not</returns>
		public static bool IsLanguageSupported(Language language)
		{
			if (language.Equals(Language.Auto))
				return true;
			
			return LanguagesSupported.Contains(language) ||
						 LanguagesSupported.FirstOrDefault(language.Equals) != null;
		}

		static GoogleTranslator()
		{
			var assembly = typeof(GoogleTranslator).GetTypeInfo().Assembly;
			Stream stream = assembly.GetManifestResourceStream("GoogleTranslateFreeApi.Languages.json");

			using (StreamReader reader = new StreamReader(stream))
			{
				string languages = reader.ReadToEnd();
				LanguagesSupported = JsonConvert
					.DeserializeObject<Language[]>(languages);
			}
		}

		/// <param name="domain">A Domain name which will be used to execute requests</param>
		public GoogleTranslator(string domain = "translate.google.com")
		{
			Address = new Uri($"https://{domain}/translate_a/single");
			_generator = new GoogleKeyTokenGenerator();
			_httpClient = new HttpClient();
		}

		/// <summary>
		/// <p>
		/// Async text translation from language to language. Include full information about the translation.
		/// </p>
		/// </summary>
		/// <param name="originalText">Text to translate</param>
		/// <param name="fromLanguage">Source language</param>
		/// <param name="toLanguage">Target language</param>
		/// <exception cref="LanguageIsNotSupportedException">Language is not supported</exception>
		/// <exception cref="InvalidOperationException">Thrown when target language is auto</exception>
		/// <exception cref="GoogleTranslateIPBannedException">Thrown when the IP used for requests is banned </exception>
		/// <exception cref="HttpRequestException">Thrown when getting the HTTP exception</exception>
		public async Task<TranslationResult> TranslateAsync(string originalText, Language fromLanguage, Language toLanguage)
		{
			return await GetTranslationResultAsync(originalText, fromLanguage, toLanguage, true);
		}

		/// <summary>
		/// <p>
		/// Async text translation from language to language. Include full information about the translation.
		/// </p>
		/// </summary>
		/// <param name="item">The object that implements the interface ITranslatable</param>
		/// <exception cref="LanguageIsNotSupportedException">Language is not supported</exception>
		/// <exception cref="InvalidOperationException">Thrown when target language is auto</exception>
		/// <exception cref="GoogleTranslateIPBannedException">Thrown when the IP used for requests is banned </exception>
		/// <exception cref="HttpRequestException">Thrown when getting the HTTP exception</exception>
		public async Task<TranslationResult> TranslateAsync(ITranslatable item)
		{
			return await TranslateAsync(item.OriginalText, item.FromLanguage, item.ToLanguage);
		}

		/// <summary>
		/// <p>
		/// Async text translation from language to language. 
		/// In contrast to the TranslateAsync doesn't include additional information such as ExtraTranslation and Definition.
		/// </p>
		/// </summary>
		/// <param name="originalText">Text to translate</param>
		/// <param name="fromLanguage">Source language</param>
		/// <param name="toLanguage">Target language</param>
		/// <exception cref="LanguageIsNotSupportedException">Language is not supported</exception>
		/// <exception cref="InvalidOperationException">Thrown when target language is auto</exception>
		/// <exception cref="GoogleTranslateIPBannedException">Thrown when the IP used for requests is banned </exception>
		/// <exception cref="HttpRequestException">Thrown when getting the HTTP exception</exception>
		public async Task<TranslationResult> TranslateLiteAsync(string originalText, Language fromLanguage, Language toLanguage)
	  {
		  return await GetTranslationResultAsync(originalText, fromLanguage, toLanguage, false);
	  }

	  /// <summary>
	  /// <p>
	  /// Async text translation from language to language. 
	  /// In contrast to the TranslateAsync doesn't include additional information such as ExtraTranslation and Definition.
	  /// </p>
	  /// </summary>
	  /// <param name="item">The object that implements the interface ITranslatable</param>
	  /// <exception cref="LanguageIsNotSupportedException">Language is not supported</exception>
	  /// <exception cref="InvalidOperationException">Thrown when target language is auto</exception>
	  /// <exception cref="GoogleTranslateIPBannedException">Thrown when the IP used for requests is banned</exception>
	  /// <exception cref="HttpRequestException">Thrown when getting the HTTP exception</exception>
	  public async Task<TranslationResult> TranslateLiteAsync(ITranslatable item)
	  {
		  return await TranslateLiteAsync(item.OriginalText, item.FromLanguage, item.ToLanguage);
	  }


		//convert curl to c# https://curl.olsh.me/
		//curl to another language : https://curl.trillworks.com/
		//sent curl https://reqbin.com/req/python/c-xgafmluu/convert-curl-to-python-requests
		//copy XHR from development tools to curl base
		public void SavePhraseBook(string phraseWordsFrom, string phraseWordsTo, string fromLang, string toLang)
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
					
				}
			}


		}
		protected virtual async Task<TranslationResult> GetTranslationResultAsync(string originalText, Language fromLanguage,
		  Language toLanguage, bool additionInfo)
	  {
		  if (!IsLanguageSupported(fromLanguage))
				throw new LanguageIsNotSupportedException(fromLanguage);
			if (!IsLanguageSupported(toLanguage))
				throw new LanguageIsNotSupportedException(toLanguage);
			if (toLanguage.Equals(Language.Auto))
				throw new InvalidOperationException("A destination Language is auto");

			if (originalText.Trim() == string.Empty)
			{
				return new TranslationResult()
				{
					OriginalText = originalText, 
					FragmentedTranslation = new string[0], 
					SourceLanguage = fromLanguage, 
					TargetLanguage = toLanguage
				};
			}

			string token = await _generator.GenerateAsync(originalText);

			string postData = $"sl={fromLanguage.ISO639}&" +
												$"tl={toLanguage.ISO639}&" +
												$"hl=en&" +
												$"q={Uri.EscapeDataString(originalText)}&" +
												$"tk={token}&" +
												"client=t&" +
												"dt=at&dt=bd&dt=ex&dt=ld&dt=md&dt=qca&dt=rw&dt=rm&dt=ss&dt=t&" +
												"ie=UTF-8&" +
												"oe=UTF-8&" +
												"otf=1&" +
												"ssel=0&" +
												"tsel=0&" +
												"kc=7";

			string result;

			try
			{
				result = await _httpClient.GetStringAsync($"{Address}?{postData}");
			}
			catch (HttpRequestException ex) when (ex.Message.Contains("503"))
			{
				throw new GoogleTranslateIPBannedException(GoogleTranslateIPBannedException.Operation.Translation);
			}
			catch
			{
				if (_generator.IsExternalKeyObsolete)
					return await TranslateAsync(originalText, fromLanguage, toLanguage);

				throw;
			}


			return ResponseToTranslateResultParse(result, originalText, fromLanguage, toLanguage, additionInfo);
	  }
	  
		protected virtual TranslationResult ResponseToTranslateResultParse(string result, string sourceText, 
			Language sourceLanguage, Language targetLanguage, bool additionInfo)
		{
			TranslationResult translationResult = new TranslationResult();

			JToken tmp = JsonConvert.DeserializeObject<JToken>(result);
			
			string originalTextTranscription = null, translatedTextTranscription = null;

			var mainTranslationInfo = tmp[0];

			GetMainTranslationInfo(mainTranslationInfo, out var translation,
				ref originalTextTranscription, ref translatedTextTranscription);
			
			translationResult.FragmentedTranslation = translation;
			translationResult.OriginalText = sourceText;

			translationResult.OriginalTextTranscription = originalTextTranscription;
			translationResult.TranslatedTextTranscription = translatedTextTranscription;

			translationResult.Corrections = GetTranslationCorrections(tmp);

			translationResult.SourceLanguage = sourceLanguage;
			translationResult.TargetLanguage = targetLanguage;

			if (tmp[8] is JArray languageDetections)
				translationResult.LanguageDetections = GetLanguageDetections(languageDetections).ToArray();


			if (!additionInfo) 
				return translationResult;
			
			translationResult.ExtraTranslations = 
				TranslationInfoParse<ExtraTranslations>(tmp[1]);

			translationResult.Synonyms = tmp.Count() >= 12
				? TranslationInfoParse<Synonyms>(tmp[11])
				: null;

			translationResult.Definitions = tmp.Count() >= 13
				? TranslationInfoParse<Definitions>(tmp[12])
				: null;

			translationResult.SeeAlso = tmp.Count() >= 15
				? GetSeeAlso(tmp[14])
				: null;

			return translationResult;
		}


		protected static T TranslationInfoParse<T>(JToken response) where T : TranslationInfoParser
	  {
		  if (!response.HasValues)
			  return null;
			
		  T translationInfoObject = TranslationInfoParser.Create<T>();
			
		  foreach (var item in response)
		  {
			  string partOfSpeech = (string)item[0];

			  JToken itemToken = translationInfoObject.ItemDataIndex == -1 ? item : item[translationInfoObject.ItemDataIndex];
				
			  //////////////////////////////////////////////////////////////
			  // I delete the white spaces to work auxiliary verb as well //
			  //////////////////////////////////////////////////////////////
			  if (!translationInfoObject.TryParseMemberAndAdd(partOfSpeech.Replace(' ', '\0'), itemToken))
			  {
					#if DEBUG
				  //sometimes response contains members without name. Just ignore it.
				  Debug.WriteLineIf(partOfSpeech.Trim() != String.Empty, 
					  $"class {typeof(T).Name} doesn't contains a member for a part " +
					  $"of speech {partOfSpeech}");
					#endif
			  }
		  }
			
		  return translationInfoObject;
	  }

	  protected static string[] GetSeeAlso(JToken response)
	  {
		  return !response.HasValues ? new string[0] : response[0].ToObject<string[]>(); 
	  }
	  
		protected static void GetMainTranslationInfo(JToken translationInfo, out string[] translate, 
			ref string originalTextTranscription, ref string translatedTextTranscription)
		{
			List<string> translations = new List<string>();
			
			foreach (var item in translationInfo)
			{
				if (item.Count() >= 5)
					translations.Add(item.First.Value<string>());
				else
				{
					var transcriptionInfo = item;
					int elementsCount = transcriptionInfo.Count();

					if (elementsCount == 3)
					{
						translatedTextTranscription = (string)transcriptionInfo[elementsCount - 1];
					}
					else
					{
						if (transcriptionInfo[elementsCount - 2] != null)
							translatedTextTranscription = (string)transcriptionInfo[elementsCount - 2];
						else
							translatedTextTranscription = (string)transcriptionInfo[elementsCount - 1];

						originalTextTranscription = (string)transcriptionInfo[elementsCount - 1];
					}
				}
			}

			translate = translations.ToArray();
		}

		protected static Corrections GetTranslationCorrections(JToken response)
		{
			if (!response.HasValues)
				return new Corrections();

			Corrections corrections = new Corrections();

			JToken textCorrectionInfo = response[7];

			if (textCorrectionInfo.HasValues)
			{
				Regex pattern = new Regex(@"<b><i>(.*?)</i></b>");
				MatchCollection matches = pattern.Matches((string)textCorrectionInfo[0]);

				var correctedText = (string)textCorrectionInfo[1];
				var correctedWords = new string[matches.Count];

				for (int i = 0; i < matches.Count; i++)
					correctedWords[i] = matches[i].Groups[1].Value;

				corrections.CorrectedWords = correctedWords;
				corrections.CorrectedText = correctedText;
				corrections.TextWasCorrected = true;
			}

			return corrections;
		}

		protected IEnumerable<LanguageDetection> GetLanguageDetections(JArray item)
		{
			JArray languages = item[0] as JArray;
			JArray confidences = item[2] as JArray;

			if (languages == null || confidences == null || languages.Count != confidences.Count)
				yield break;

			for (int i = 0; i < languages.Count; i++)
			{
				yield return new LanguageDetection(GetLanguageByISO((string) languages[i]), (double) confidences[i]);
			}
		}
  }
}
