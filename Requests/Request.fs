namespace FSharpTools.HttpRequests
open System.Net.Http
open System.Threading
open FSharpTools.Functional
open Client

module Request = 
    

    let private call settings onlyHeaders = 

        let addHeaders (msg: HttpRequestMessage) = 
            let addHeader header = 
                if not (msg.Headers.TryAddWithoutValidation (header.Key, header.Value) && not (isNull msg.Content)) then
                    msg.Content.Headers.TryAddWithoutValidation (header.Key, header.Value) |> ignore
            settings.Headers
            |> Option.iter(fun headers -> 
                headers |> Seq.iter addHeader)

        let msg = 
            new HttpRequestMessage(
                settings.Method, 
                (settings.BaseUrl |> Option.defaultValue "") + settings.Url, 
                                Version = new System.Version(settings.Version.Major, settings.Version.Minor))
            |> sideEffect addHeaders

        let whatHeaders = if onlyHeaders then HttpCompletionOption.ResponseHeadersRead else HttpCompletionOption.ResponseContentRead
        let response = 
            match settings.Timeout with
            | Some t -> (getClient ()).SendAsync (msg, whatHeaders, (new CancellationTokenSource(t)).Token)  
            | None -> (getClient ()).SendAsync (msg, whatHeaders)  
        // TODO SendAsync function returnning AsyncResult<response, HttpException
            
