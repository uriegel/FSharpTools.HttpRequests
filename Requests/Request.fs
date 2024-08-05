namespace FSharpTools.HttpRequests
open System
open System.Net.Http
open System.Net.Sockets
open System.Threading
open FSharpTools.Functional
open FSharpTools.AsyncResult
open Client

module Request = 
    open System.Net.Http
    open System.Net
    let private mapException (exp: Exception): RequestError =
        match exp with
        | :? HttpRequestException as hre when hre.HttpRequestError = HttpRequestError.NameResolutionError
            -> { Message = hre.Message; Type = HostNotFound }
        | :? HttpRequestException as hre 
            -> match hre.InnerException with
                | :? SocketException as se when se.SocketErrorCode = SocketError.ConnectionRefused 
                    -> { Message = hre.Message; Type = ConnectionRefused }
                | _ -> { Message = hre.Message; Type = Unknown exp }
        | _ -> { Message = exp.Message; Type = Unknown exp }
    
    let private call settings onlyHeaders = 

        let httpToError (msg: HttpResponseMessage) =
            match msg.StatusCode with
            | HttpStatusCode.OK -> Ok msg
            | _ -> Error { Message = msg.ReasonPhrase; Type = Http (LanguagePrimitives.EnumToValue msg.StatusCode) }

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
                                Version = new Version(settings.Version.Major, settings.Version.Minor))
            |> sideEffect addHeaders

        let sendAsync () = 
            let whatHeaders = if onlyHeaders then HttpCompletionOption.ResponseHeadersRead else HttpCompletionOption.ResponseContentRead
            let sendAsync () = 
                let client = getClient ()

                match settings.Timeout with
                | Some t -> (getClient ()).SendAsync (msg, whatHeaders, (new CancellationTokenSource(t)).Token)  
                | None -> (getClient ()).SendAsync (msg, whatHeaders)  
                    
            catch sendAsync

        sendAsync ()
        |> AsyncResult.mapError mapException
        |> AsyncResult.bindToError httpToError
            
    let callTest = call 