namespace FSharpTools.HttpRequests
open System
open System.Net.Http
open System.Net.Http.Json
open FSharpTools.AsyncResult

module Response = 
    
    let asJson<'a> (msg: HttpResponseMessage) = 
        let mapException (exp: Exception) = 
            { Message = exp.Message; Type = Deserialization }

        let asJson () = 
            // TODO cancellationToken
            msg.Content.ReadFromJsonAsync<'a>()
        catch asJson
        |> AsyncResult.mapError mapException

