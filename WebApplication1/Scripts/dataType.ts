/**
 * Local dictionary for content parse.
 */
declare type LocalDict = Map<string, string>


/**
 * From one language to its all other starred word's language translates.
 */
declare type Dict = {
    [fromLang: string]: {
        [toLang: string]: [string, string][]
    }
}

/**
 * Word list in two languages. `{ Acne: "Pattanás", ... }`
 * TODO: Map instead of object, insert in right order
 */
declare type Words = {
    [word: string]: string
}

/**
 * Google's POST request data for star new word.
 */
declare type AddRequestData = {
    formData: {
        q: string[]
        utrans: string[]
    }
}

declare type WordsFound = {
    type: "WORDS_FOUND"
    count: number
}

type LanguageDetected = {
    type: "TAB_LANGUAGE"
    language: string
}

type ProcessingMessage = {
    type: "TAB_PROCESSING"
}

type FetchDictionary = {
    type: "FETCH_DICTIONARY"
}

type DataRow = [string, string, string, string, string, number]
type DataChunk = {
    key: string
    isError: boolean
    hash: string
    data: any
    sideChannel: any
}

interface Window {
    AF_initDataChunkQueue?: DataChunk[]
}


/**
 *
 */
declare type AppMessage = WordsFound | LanguageDetected | ProcessingMessage | FetchDictionary

declare type NotificationType = "IMPORT_SUCCESS" | "IMPORT_FAIL" | "FETCH_FAIL"


