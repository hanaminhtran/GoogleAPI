let wordPerPage = 20;
let currentPage = 0;
const dictLocal: Dict = {}
let randomFrom = 0;
let randomTo = 50;
let playInterval = 3000;
let showMeaning = false;
let curretMeaning = "";
var interval;


window.onload = () => {
    //GetURL();
    var res = document.getElementById("temp");
    var text = (res.innerText || res.textContent);
    starredIntoLocal(text);//html text
    var button = document.getElementById("bntBack");
    (button as HTMLInputElement).disabled = true;
    (document.getElementById("bntAutoPlay") as HTMLButtonElement).disabled = true;
    LoadSetting();
    (document.getElementById("Setting") as HTMLInputElement).hidden = true;
    
   // var dictLocal: TestDic = {};
    
    
};

async function SaveSetting() {
    
    randomFrom = Number((document.getElementById("txtrandomFrom") as HTMLInputElement).value);
    randomTo = Number((document.getElementById("txtrandomTo") as HTMLInputElement).value);
    wordPerPage = Number((document.getElementById("txtwordPerPage") as HTMLInputElement).value);
    playInterval = Number((document.getElementById("txtPlayInterval") as HTMLInputElement).value);
    (document.getElementById("Setting") as HTMLInputElement).hidden = true;
}

function LoadSetting() {
    (document.getElementById("txtrandomFrom") as HTMLInputElement).value = randomFrom.toString();
    (document.getElementById("txtrandomTo") as HTMLInputElement).value = randomTo.toString();
    (document.getElementById("txtwordPerPage") as HTMLInputElement).value = wordPerPage.toString();
    (document.getElementById("txtPlayInterval") as HTMLInputElement).value = playInterval.toString();    

}

function ShowSetting() {
    LoadSetting();
    (document.getElementById("Setting") as HTMLInputElement).hidden = false;
}


async function PlaySound(word) {
    say(word);

  



}
async function Next() {    
    if ((currentPage + 1) * wordPerPage < dictLocal["en"]["vi"].length) {
        currentPage = currentPage + 1;
        await viewPage(currentPage);
        var button = document.getElementById("bntBack");
        (button as HTMLInputElement).disabled = false;

    }
    else {
        (document.getElementById("bntNext") as HTMLInputElement).disabled = true;
    }
    
}

function randomIntFromInterval(min, max) { // min and max included 
    return Math.floor(Math.random() * (max - min + 1) + min)
}


var isPlaying = false;
function AutoPlay() {
    
    if (!isPlaying) {
        (document.getElementById("bntAutoPlay") as HTMLInputElement).innerText = "Stop";
        (document.getElementById("bntStudyRandom") as HTMLInputElement).disabled = true;        
        isPlaying = true;
        interval = setInterval(function () {
            NextRandom();
        }, playInterval);
        
    }
    else {
        isPlaying = false;
        (document.getElementById("bntAutoPlay") as HTMLInputElement).innerText = "AutoPlay";
        (document.getElementById("bntStudyRandom") as HTMLInputElement).disabled = false;
        clearInterval(interval);
    }




}


const GetWordRandom = function ShowRandom(from, to) {
    if (!showMeaning) {
        let currentWordIndex = randomIntFromInterval(randomFrom, randomTo);
        var en = dictLocal["en"]['vi'][currentWordIndex][0].toString();
        var vi = dictLocal["en"]['vi'][currentWordIndex][1].toString();
        curretMeaning = vi;
        (document.getElementById("bntNextRandom") as HTMLInputElement).setAttribute("style", "background-color:#04AA6D");
        showMeaning = true;
        PlaySound(en);
        return en;
    } else {
        //show vietnamese meang// change css

        showMeaning = false;
        (document.getElementById("bntNextRandom") as HTMLInputElement).setAttribute("style", "background-color:#dbc9c9");
            
        //
        return curretMeaning;
    }
    
}
async function NextRandom() {
         
    (document.getElementById("bntNextRandom") as HTMLInputElement).innerText = GetWordRandom(randomFrom, randomTo);
    const randomKey = randomEnumKey(ColorRow);
    (document.getElementById("bntNextRandom") as HTMLInputElement).style.backgroundColor = ColorRow[randomKey];

}
async function StudyRandom() {        
    var currentWord = GetWordRandom(randomFrom, randomTo);    
    let value = (document.getElementById("bntStudyRandom") as HTMLInputElement).innerText;
    if (value == "Randomize words") {
        (document.getElementById("navigation") as HTMLInputElement).hidden = (document.getElementById("showdata") as HTMLInputElement).hidden = true;
        (document.getElementById("randometable") as HTMLInputElement).hidden = false;
        (document.getElementById("bntStudyRandom") as HTMLInputElement).innerText = "Phrasebooks list";                
        (document.getElementById("bntNextRandom") as HTMLInputElement).innerText = GetWordRandom(randomFrom, randomTo);
        (document.getElementById("bntAutoPlay") as HTMLInputElement).hidden = false;
        (document.getElementById("bntAutoPlay") as HTMLButtonElement).disabled = false;
        return;

    }
    if (value == "Phrasebooks list") {
        (document.getElementById("bntAutoPlay") as HTMLButtonElement).disabled = true;
        (document.getElementById("navigation") as HTMLInputElement).hidden = (document.getElementById("showdata") as HTMLInputElement).hidden = false;
        (document.getElementById("randometable") as HTMLInputElement).hidden = true;
        (document.getElementById("bntStudyRandom") as HTMLInputElement).innerText = "Randomize words";
        (document.getElementById("bntAutoPlay") as HTMLInputElement).hidden = true;
        return;

    }       

}
async function Back() {
    if (currentPage > 0) {
        currentPage = currentPage - 1;
        await viewPage(currentPage);
    }
    if (currentPage == 0)
        (document.getElementById("bntBack") as HTMLInputElement).disabled = true;
    
}


class Student {
    fullName: string;
    constructor(public firstName: string, public middleInitial: string, public lastName: string) {
        this.fullName = firstName + " " + middleInitial + " " + lastName;
    }
}

interface Person {
    firstName: string;
    lastName: string;
}

function greeter(person: Person) {
    return "Hello, " + person.firstName + " " + person.lastName;
}



const langCode = (detectedLanguage: string) => detectedLanguage.split("-")[0]

function colorize() {
    var r = ('0' + (Math.random() * 255 | 0).toString(16)).slice(-2),
        g = ('0' + (Math.random() * 255 | 0).toString(16)).slice(-2),
        b = ('0' + (Math.random() * 255 | 0).toString(16)).slice(-2);

    for (var i = 0; i < document.getElementsByTagName("td").length; i++) {
        document.getElementsByTagName("td")[i].style.backgroundColor = "#" + r + g + b;
    }
}

enum ColorRow {
    c1 = "#99d4ee",
    c2 = "#9ddbc3",
    c3 = "#e7c2d7",
    c4 = "#fbe9c7"
};

const randomEnumKey = enumeration => {
    const keys = Object.keys(enumeration)
        .filter(k => !(Math.abs(Number.parseInt(k)) + 1));
    const enumKey = keys[Math.floor(Math.random() * keys.length)];
    return enumKey;
};

function colorizeInArray() {

    
    
    for (var i = 0; i < document.getElementsByTagName("tr").length; i++) {
        const randomKey = randomEnumKey(ColorRow);
        document.getElementsByTagName("tr")[i].style.backgroundColor = ColorRow[randomKey];
    }
    for (var i = 0; i < document.getElementsByTagName("td").length; i++) {
        const randomKey = randomEnumKey(ColorRow);
        document.getElementsByTagName("td")[i].style.backgroundColor = ColorRow[randomKey];
    }

}

async function viewPage(page) {

    const dictEntries = Object.entries(dictLocal)
    var returnText = document.getElementById("showdata");    
    returnText.innerHTML = "";
    returnText.textContent = "";
    var outText = ""
    if (dictEntries.length > 0) {
        //chrome.storage.local.clear()

        for (const [fromLang, toLangs] of dictEntries) {
            if (fromLang == "en") {
                let to = wordPerPage * (currentPage + 1);
                let from = wordPerPage *currentPage;
                for (let j = from; j < to; j++) {
                    let en = toLangs['vi'][j][0].toString();// need fix more than 1 word cannot pass to function playsound
                    en= en.replace(/(\r.|\n|\r)/gm, "");
                    let vn = toLangs['vi'][j][1].toString();
                    outText += "<tr > <td>";
                    outText += en + "</td><td >" + vn + "</td><td>" +
                        "<div>  <img src = 'https://cdn-icons-png.flaticon.com/128/59/59284.png' onclick = 'PlaySound(\""
                        + en + "\")'  height = '18' width = '18'> </div>";
                    outText += "</td></tr>";
                }
            }
            //await chrome.storage.local.set({ [fromLang]: toLangs })
        }
        returnText.innerHTML = outText;
        colorizeInArray();
        return true
    }
    return false


}
const saveDict = async (dict: Dict) => {
    const dictEntries = Object.entries(dict)
    var returnText = document.getElementById("showdata");
    returnText.innerHTML = "";
    var outText = ""
    if (dictEntries.length > 0) {
        //chrome.storage.local.clear()
        
        for (const [fromLang, toLangs] of dictEntries) {         
            if (fromLang == "en")
            {

                for (let i = 0; i < 100; i++) {
                    outText += "<tr> <td>";
                    outText += toLangs['vi'][i][0].toString() + "</td><td >" + toLangs['vi'][i][1].toString() + "</td><td>" +
                        "<div>  <img src = 'https://cdn-icons-png.flaticon.com/128/59/59284.png'  height = '18' width = '18'> </div>";
                    outText += "</td></tr>";
                }
             }
            //await chrome.storage.local.set({ [fromLang]: toLangs })
        }
        returnText.innerHTML = outText;
        return true
    }
    return false
}
const addWord = (dict: Dict) => ([, fromLang, toLang, fromWord, toWord]: DataRow) => {
    fromLang = langCode(fromLang)
    toLang = langCode(toLang)
    fromWord = fromWord.toLowerCase()
    toWord = toWord.toLowerCase()

    if (dict[fromLang] === undefined) dict[fromLang] = {}
    if (dict[fromLang][toLang] === undefined) dict[fromLang][toLang] = []

    const words = dict[fromLang][toLang]
    if (words === undefined || words[fromWord] !== toWord) {
        dict[fromLang][toLang].push([fromWord, toWord])
    }

    return dict
}

const scriptTags = (html: string) =>
    Array.from(new DOMParser().parseFromString(html, "text/html").querySelectorAll("script"))

const scriptTag1s = function par(html) {
    Array.from(new DOMParser().parseFromString(html, "text/html").querySelectorAll("script"))
}

const parseTag = (text: string): DataRow[] | undefined => {
    let rows: DataRow[]
    try {
        rows = JSON.parse(
            text.split("data:", 2)[1]?.split("sideChannel:", 1)[0].trimEnd().slice(0, -1)
        )[0]
        if (!Array.isArray(rows) || rows.length === 0 || rows[0].length !== 6) {
            return undefined
        }
    } catch {
        return undefined
    }
    return rows
}

const okok = function aaa(html: string) {
    var sss = Array.from(new DOMParser().parseFromString(html, "text/html").querySelectorAll("script"))    
    return sss
}

const starredIntoLocal = async (res) => {
          
    const add = addWord(dictLocal)
    // First success breaks and stop the iteration
    var html = res;

    try {
        var bbb = await okok(await res);
        bbb.some(({ innerText }) => {
            const dataRows = parseTag(innerText)
            if (dataRows && dataRows.length > 0) {
                dataRows.forEach(add)
                return true
            }
        })
    } catch (e) {
        alert(e);
    }
    await viewPage(currentPage);
    //const success = await saveDict(dictLocal)
}








//playsound 

const API_DOMAIN = "https://api.pearson.com";
const LMD_UTL = "https://www.ldoceonline.com/dictionary/";
const EXAMPLE_PREFIX = "ex) ";
let selectionText = "hi";
let request = new XMLHttpRequest();
    // Open a new connection, using the GET request on the URL endpoint
    //request tra ve json    


function say(m) {

    selectionText = m;
    request.open('GET', API_DOMAIN + '/v2/dictionaries/ldoce5/entries?limit=5&headword=' + selectionText, true);
    request.addEventListener("load", useLoadedMainApi, false);
    //request.addEventListener("load", requestSynonymApi, false);
    request.send();

    //sendAjaxRequest("POST", "INDEX", m, GetMp3);

}



function GetMp3(r) {
    alert(r);
}

function sendAjaxRequest(_type: string, _url: string, _params: string, GetMp3) {

    try {
        var request = $.ajax({
            type: _type,
            url: "https://localhost:44350/" + _url,
            data: _params,
            contentType: 'json'
        });
        request.done(function (res) {
            GetMp3(res);
        });
        request.fail(function (jqXHR, textStatus) {
            console.error(jqXHR)
            GetMp3({ err: true, message: "Request failed: " + textStatus });
        });

    } catch (e) {
        alert(e)
    }
    

}

function CallAjax() {
    var xhttp = new XMLHttpRequest();    
    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            document.getElementById("demo").innerHTML =
                this.responseText;
        }
    };
    xhttp.open("GET", "ajax_info.txt", true);
    xhttp.send();
}


function useLoadedMainApi() {
    // Begin accessing JSON data here

    
    var tem = "https://translate.google.com/translate_tts?ie=UTF-8&q=LONG_TEXT_...&tl=en&total=1&idx=0&textlen=13&client=tw-ob&prev=input&ttsspeed=1"
    const dataContent = analyzeMainApiJson(JSON.parse(this.response));
    if (dataContent.length > 0) {
        for (let i = 0; i < dataContent.length; i++) {
            if ("audio" in dataContent[i]) {
                let audio = document.createElement("audio");
                audio.src = dataContent[i]["audio"];                
                audio.preload = "auto";
                audio.play();
            }
        }

    }
    else {
        //alert("goole")
        var msg = new SpeechSynthesisUtterance();
        var voices = window.speechSynthesis.getVoices();
        msg.voice = voices[10];
        msg.volume = 1;
        msg.rate = 1;
        msg.pitch = 0.8;
        msg.text = selectionText;
        msg.lang = 'en-US';
        speechSynthesis.speak(msg);

    }

    

}


function analyzeMainApiJson(responseJson) {
    let jsonAry = [];
    if (!("total" in responseJson) || responseJson["total"] === 0) {
        return jsonAry;
    }
    for (let responseAryKey in responseJson["results"]) {
        const jsonObj = responseJson["results"][responseAryKey];

        if (selectionText !== jsonObj["headword"]) {
            continue;
        }

        let json :  any = {};
        // HEADWORD
        json.headword = jsonObj["headword"];
        // PART OF SPEECH
        if ("part_of_speech" in jsonObj) {
            json.partOfSpeech = jsonObj["part_of_speech"];
        }
        // IPA AUDIO
        if ("pronunciations" in jsonObj) {
            const pronunciations = jsonObj["pronunciations"];
            // if there is only one element and more than 2 audios, ipa is common but pro is diff
            if (pronunciations.length === 1) {
                for (let audioIndex in pronunciations[0]["audio"]) {
                    if (pronunciations[0]["audio"][audioIndex]["lang"] !== "American English") {
                        continue;
                    }
                    json.ipa = pronunciations[0]["ipa"];
                    json.audio = API_DOMAIN + pronunciations[0]["audio"][audioIndex]["url"];
                    //alert(json.audio);
                    //console.log(json.audio);
                }
            } else {
                // if there is two element, each has own ipa
                for (let pronunciationsIndex in pronunciations) {
                    if (!("lang" in pronunciations[pronunciationsIndex])
                        || pronunciations[pronunciationsIndex]["lang"] !== "American English") {
                        continue;
                    }
                    if ("ipa" in pronunciations[pronunciationsIndex]) {
                        json.ipa = pronunciations[pronunciationsIndex]["ipa"];
                        if ("audio" in pronunciations[pronunciationsIndex]) {
                            for (let audio in pronunciations[pronunciationsIndex]["audio"]) {
                                json.audio = API_DOMAIN + pronunciations[pronunciationsIndex]["audio"][audio]["url"];
                                //alert(json.audio);
                                console.log(json.audio);
                            }
                        }
                    }
                }
            }
        }

        // SENSES
        if ("senses" in jsonObj && jsonObj["senses"]) {
            for (let key in jsonObj["senses"][0]) {
                if (jsonObj["senses"][0][key] instanceof Array) {
                    if (jsonObj["senses"][0][key][0] instanceof Object) {
                        if ("text" in jsonObj["senses"][0][key][0]) {
                            // EXAMPLE
                            json.example = {};
                            json.example.text = EXAMPLE_PREFIX + jsonObj["senses"][0][key][0]["text"];
                            if ("audio" in jsonObj["senses"][0][key][0]) {
                                json.example.audio = API_DOMAIN + jsonObj["senses"][0][key][0]["audio"][0]["url"];
                                //alert(json.audio);
                                //console.log(json.audio);
                            }

                        }
                    } else {
                        // DEFINITION
                        json.definition = jsonObj["senses"][0][key][0];
                    }
                } else if (jsonObj["senses"][0][key] instanceof Object
                    && "type" in jsonObj["senses"][0][key]) {
                    // GRAMMATICAL INFO
                    json.grammaticalInfo = jsonObj["senses"][0][key]["type"];
                } else if ("synonym" in jsonObj["senses"][0]) {
                    // SYNONYM
                    json.synonym = jsonObj["senses"][0][key];
                }
            }
        }

        // URL
        json.url = "https://www.ldoceonline.com/dictionary/" + json.headword;
        jsonAry.push(json);
    }

    return jsonAry;
}









