namespace FSharpTools.HttpRequests
open System.Net.Http
open System

type Version = {
    Major: int
    Minor: int
}

type Header = {
    Key: string
    Value: string    
}

type Settings = {
    Method: HttpMethod
    BaseUrl: string option
    Url: string
    Version: Version
    Headers: Header array option
    AddContent: (unit->HttpContent) option
    Timeout: TimeSpan option
}

module Settings =
    let getDefaultSettings () = 
        {
            Method = HttpMethod.Get
            BaseUrl = None
            Url = "" 
            Version = { Major = 3; Minor = 0}
            Headers = None
            AddContent = None
            Timeout = None
        }