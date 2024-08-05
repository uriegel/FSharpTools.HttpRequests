namespace FSharpTools.HttpRequests
open System.Net.Http

module Request = 

    let private call settings onlyHeaders = 
        new HttpRequestMessage(
            settings.Method, 
            (settings.BaseUrl |> Option.defaultValue "") + settings.Url,
            Version = new System.Version(settings.Version.Major, settings.Version.Minor))
